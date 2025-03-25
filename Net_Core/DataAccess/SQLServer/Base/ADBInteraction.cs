using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework;
using GrowthWare.Framework.Interfaces;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.SQLServer.Base;

/// <summary>
/// Performs all data store interaction to SQL Server.
/// </summary>
public abstract class AbstractDBInteraction : IDBInteraction
{

#region Member Fields
    private string m_ConnectionString = string.Empty;
    
    internal Logger m_Logger = Logger.Instance();
#endregion

#region Public Properties
    /// <summary>
    /// Used for all methods to connect to the database.
    /// </summary>
    public string ConnectionString
    {
        get
        {
            return m_ConnectionString;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                this.m_ConnectionString = value.Trim();
            }
            else
            {
                throw new DataAccessLayerException("The ConnectionString value cannot be null or blank!");
            }
        }        
    }
#endregion

    /// <summary>
    /// Performs a bulk upload of IDatabaseFunctions objects into the
    /// database. Note: Requires an object with a primary key!
    /// </summary>
    /// <param name="bulkInsertParameters">A DTO representing multiple parameters</param>
    internal void BulkInsert(DTO_BulkInsert_Parameters bulkInsertParameters)
    {
        if (bulkInsertParameters.ListOfProfiles != null && bulkInsertParameters.ListOfProfiles.Any())
        {
            string mCommandText = string.Empty;

            using var mSqlConnection = new SqlConnection(this.ConnectionString);
            mSqlConnection.Open();
            SqlTransaction mSqlTransaction = mSqlConnection.BeginTransaction();
            try
            {
                // 1.) Create Destination Table
                mCommandText = string.Format("SELECT * INTO {0} FROM {1} Where 1 = 2", bulkInsertParameters.TempTableName, bulkInsertParameters.DestinationTableName);
                using (SqlCommand mSqlCommand = new SqlCommand(mCommandText, mSqlConnection, mSqlTransaction))
                {
                    mSqlCommand.CommandType = CommandType.Text;
                    mSqlCommand.ExecuteNonQuery();
                }

                // Retrieve ordered column names from the temporary table
                string[] orderedColumns;
                mCommandText = string.Format("SELECT * FROM {0}", bulkInsertParameters.TempTableName);
                using (SqlCommand mSqlCommand = new SqlCommand(mCommandText, mSqlConnection, mSqlTransaction))
                {
                    mSqlCommand.CommandType = CommandType.Text;
                    using (var reader = mSqlCommand.ExecuteReader())
                    {
                        orderedColumns = new string[reader.FieldCount]; // Initialize array with the number of columns
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            orderedColumns[i] = reader.GetName(i); // Get column names in order
                        }
                    }
                }

                // Remove primary key column name if needed and it exists from orderedColumns
                if (!bulkInsertParameters.IncludePrimaryKey && orderedColumns.Contains(bulkInsertParameters.PrimaryKeyName, StringComparer.OrdinalIgnoreCase))
                {
                    orderedColumns = orderedColumns.Where(x => !string.Equals(x, bulkInsertParameters.PrimaryKeyName, StringComparison.OrdinalIgnoreCase)).ToArray();
                }

                // Create DataTable with the columns in the same order as the temporary table
                IDatabaseTable mFirstObj = (IDatabaseTable)bulkInsertParameters.ListOfProfiles.First();
                DataTable mDataTable = mFirstObj.GetEmptyTable(bulkInsertParameters.TempTableName, bulkInsertParameters.IncludePrimaryKey, orderedColumns);

                // Now populate the DataTable
                foreach (var item in bulkInsertParameters.ListOfProfiles)
                {
                    DataRow mRow = mDataTable.NewRow();
                    PropertyInfo[] mProperties = item.GetType().GetProperties();

                    foreach (var columnName in orderedColumns)
                    {
                        var mPropertyItem = mProperties.FirstOrDefault(p => string.Equals(p.Name, columnName, StringComparison.OrdinalIgnoreCase));
                        if (mPropertyItem != null)
                        {
                            var mValue = mPropertyItem.GetValue(item, null);
                            // Handle nullable properties
                            if (mPropertyItem.PropertyType.IsGenericType && mPropertyItem.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                mValue = mValue ?? DBNull.Value;
                            }

                            mRow[columnName] = mValue ?? DBNull.Value; // Set to DBNull if null
                        }
                    }
                    mDataTable.Rows.Add(mRow);
                }

                if (!bulkInsertParameters.IncludePrimaryKey)
                {
                    mCommandText = string.Format("ALTER TABLE {0} DROP COLUMN IF EXISTS {1}", bulkInsertParameters.TempTableName, bulkInsertParameters.PrimaryKeyName);
                    using (SqlCommand mSqlCommand = new SqlCommand(mCommandText, mSqlConnection, mSqlTransaction))
                    {
                        mSqlCommand.CommandType = CommandType.Text;
                        mSqlCommand.ExecuteNonQuery();
                    }
                }

                // 2.) Perform SqlBulkCopy of the data table in a temporary table
                using (var mSqlBulkCopy = new SqlBulkCopy(mSqlConnection, SqlBulkCopyOptions.Default, mSqlTransaction))
                {
                    mSqlBulkCopy.BatchSize = 5000;
                    mSqlBulkCopy.DestinationTableName = bulkInsertParameters.TempTableName;
                    mSqlBulkCopy.WriteToServer(mDataTable);
                }

                // 3.) Delete all rows associated from the db if needed
                if (bulkInsertParameters.DoDelete)
                {
                    // semi correct should account for the primary key or in other words join on all columns except the primary key
                    // b/c the primary key if one exists is always unique
                    // for now I'm going to insist that the Destination have a single foreign key
                    mCommandText = "DELETE {0} FROM {0} Destination INNER JOIN {1} TempTable ON Destination.{2} = TempTable.{2};";
                    mCommandText = string.Format(mCommandText, bulkInsertParameters.DestinationTableName, bulkInsertParameters.TempTableName, bulkInsertParameters.ForeignKeyName);
                    using (SqlCommand mSqlCommand = new SqlCommand(mCommandText, mSqlConnection, mSqlTransaction))
                    {
                        mSqlCommand.CommandType = CommandType.Text;
                        mSqlCommand.ExecuteNonQuery();
                    }
                }
                // 4.) Insert all the rows from the temp table into Destination
                mCommandText = string.Format("INSERT INTO {0} SELECT * FROM {1}", bulkInsertParameters.DestinationTableName, bulkInsertParameters.TempTableName);
                using (SqlCommand mSqlCommand = new SqlCommand(mCommandText, mSqlConnection, mSqlTransaction))
                {
                    mSqlCommand.CommandType = CommandType.Text;
                    mSqlCommand.ExecuteNonQuery();
                }
                mSqlTransaction.Commit();
            }
            catch (System.Exception ex)
            {
                // Roll back the transaction if an error occurs
                mSqlTransaction.Rollback();
                // provide more context
                DataAccessLayerException mException = new DataAccessLayerException("An error occurred during bulk insert.", ex);
                // Log the exception
                m_Logger.Error(mException);
                throw mException;
            }
            finally
            {
                // 5.) finally delete the temporary table
                mCommandText = string.Format("IF OBJECT_ID('{0}', 'U') IS NOT NULL DROP TABLE {0};", bulkInsertParameters.TempTableName);
                using (SqlCommand mSqlCommand = new SqlCommand(mCommandText, mSqlConnection))
                {
                    mSqlCommand.CommandType = CommandType.Text;
                    mSqlCommand.ExecuteNonQuery();
                }
            }
        }
    }

    /// <summary>
    /// Clean up the string
    /// </summary>
    /// <param name="stringValue"></param>
    /// <returns></returns>
    protected virtual string Cleanup(string stringValue)
    {
        string mRetVal = stringValue;
        if (!string.IsNullOrEmpty(mRetVal))
        {
            mRetVal = mRetVal.Replace(";", "");  // Remove semicolons
            mRetVal = mRetVal.Replace("'", "");  // Remove single quotes
            mRetVal = mRetVal.Replace("\"", ""); // Remove double quotes
            mRetVal = mRetVal.Trim();            // Remove leading and trailing spaces
        }
        return mRetVal;
    }

    /// <summary>
    /// Executes an asynchronous SQL command or stored procedure and returns the value of <paramref name="execution"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value returned by <paramref name="execution"/>.</typeparam>
    /// <param name="commandText">The command text to execute.</param>
    /// <param name="sqlParameters">The parameters to use when executing the command.</param>
    /// <param name="forceCommandText">If true, the command text is executed as a SQL command. If false, the command text is executed as a stored procedure.</param>
    /// <param name="execution">A function that takes a <see cref="SqlCommand"/> and returns a <see cref="Task{T}"/>.</param>
    /// <returns>The result of <paramref name="execution"/>.</returns>
    /// <exception cref="DataAccessLayerException">Thrown if an error occurs while executing the SQL command.</exception>
    private async Task<T> executeAsync<T>(string commandText, SqlParameter[] sqlParameters, bool forceCommandText, Func<SqlCommand, Task<T>> execution)
    {
        if (string.IsNullOrWhiteSpace(m_ConnectionString))
        {
            throw new DataAccessLayerException("The ConnectionString property cannot be null or blank!");
        }
        await using var mSqlConnection = new SqlConnection(m_ConnectionString);
        await mSqlConnection.OpenAsync();
        await using var command = new SqlCommand(commandText, mSqlConnection)
        {
            CommandType = forceCommandText ? CommandType.Text : CommandType.StoredProcedure
        };
        if (sqlParameters != null)
        {
            command.Parameters.AddRange(sqlParameters);
        }
        try
        {
            return await execution(command);
        }
        catch (Exception ex)
        {
            m_Logger.Error(ex);
            throw new DataAccessLayerException("Error executing SQL command.", ex);
        }
    }

    /// <summary>
    /// Executes a non Query given the commandText and sql parameters if any
    /// </summary>
    /// <param name="commandText">String</param>
    /// <param name="sqlParameter">SqlParameter</param>
    /// <exception cref="DataAccessLayerException"></exception>
    protected int ExecuteNonQuery(String commandText, SqlParameter[] sqlParameters, bool forceCommandText = false)
    {
        this.IsValid();
        try
        {
            using (SqlConnection mSqlConnection = new(this.ConnectionString))
            {
                mSqlConnection.Open();
                using (SqlCommand mSqlCommand = new(commandText, mSqlConnection))
                {
                    mSqlCommand.CommandType = CommandType.Text;
                    if (sqlParameters != null)
                    {
                        if (sqlParameters.Length > 0)
                        {
                            if (forceCommandText != true)
                            {
                                mSqlCommand.CommandType = CommandType.StoredProcedure;
                            }
                            foreach (SqlParameter mSqlParameter in sqlParameters)
                            {
                                mSqlCommand.Parameters.Add(mSqlParameter);
                            }
                        }
                    }
                    return mSqlCommand.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException ex)
        {
            if (ex.Message.ToUpper(CultureInfo.InvariantCulture).StartsWith("CANNOT OPEN DATABASE", StringComparison.OrdinalIgnoreCase))
            {
                throw;
            }
            else
            {
                string mFormattedMsg = formatError(sqlParameters, commandText, ex.ToString());
                DataAccessLayerException mDataAccessLayerException = new(mFormattedMsg, ex);
                this.m_Logger.Fatal(mDataAccessLayerException);
                throw mDataAccessLayerException;
            }
        }
        catch (Exception ex)
        {
            string mFormattedMsg = formatError(sqlParameters, commandText, ex.ToString());
            DataAccessLayerException mDataAccessLayerException = new(mFormattedMsg, ex);
            this.m_Logger.Fatal(mDataAccessLayerException);
            throw mDataAccessLayerException;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="commandText">String</param>
    /// <exception cref="DataAccessLayerException"></exception>
    protected int ExecuteNonQuery(String commandText)
    {
        return this.ExecuteNonQuery(commandText, null);
    }

    /// <summary>
    /// Executes an asynchronous SQL command or stored procedure, and returns the number of rows affected.
    /// </summary>
    /// <param name="commandText">The command text to execute.</param>
    /// <param name="sqlParameters">The parameters to use when executing the command.</param>
    /// <param name="forceCommandText">If true, the command text is executed as a SQL command. If false, the command text is executed as a stored procedure.</param>
    /// <returns>The number of rows affected by the execution of the SQL command.</returns>
    /// <exception cref="DataAccessLayerException">Thrown if an error occurs while executing the SQL command.</exception>
    protected virtual Task<int> ExecuteNonQueryAsync(string commandText, SqlParameter[] sqlParameters = null, bool forceCommandText = false)
        => executeAsync(commandText, sqlParameters, forceCommandText, cmd => cmd.ExecuteNonQueryAsync());

    /// <summary>
    /// Executes a non query asynchronously given the commandText and sql parameters if any
    /// </summary>
    /// <param name="commandText">String</param>
    /// <returns>int</returns>
    /// <exception cref="DataAccessLayerException"></exception>
    /// <remarks>This overload will assume the commandText is a SQL command and not a stored procedure.</remarks>
    protected virtual Task<int> ExecuteNonQueryAsync(string commandText)
    {
        return this.ExecuteNonQueryAsync(commandText, null, true);
    }

    /// <summary>
    /// Executes a scalar query asynchronously against the database.
    /// </summary>
    /// <param name="commandText">The command text to execute.</param>
    /// <param name="sqlParameters">The parameters to use when executing the command.</param>
    /// <param name="forceCommandText">If true, the command text is executed as a SQL command. If false, the command text is executed as a stored procedure.</param>
    /// <returns>The value of the first column of the first row of the result set returned by the query. If the result set contains no rows, this method returns <see cref="DBNull.Value"/>.</returns>
    /// <exception cref="DataAccessLayerException">Thrown if an error occurs while executing the SQL command.</exception>
    protected virtual Task<object> ExecuteScalarAsync(string commandText, SqlParameter[] sqlParameters = null, bool forceCommandText = false)
        => executeAsync(commandText, sqlParameters, forceCommandText, cmd => cmd.ExecuteScalarAsync());

    /// <summary>
    /// Executes a scalar query asynchronously against the database.
    /// </summary>
    /// <param name="commandText">The command text to execute.</param>
    /// <returns>this.ExecuteScalarAsync(commandText, null, true)</returns>
    /// <exception cref="DataAccessLayerException">Thrown if an error occurs while executing the SQL command.</exception>
    protected virtual Task<object> ExecuteScalarAsync(string commandText)
    {
        return this.ExecuteScalarAsync(commandText, null, true);
    }

    /// <summary>
    /// Executes a scalar query against the database.
    /// This function takes a command text and optional SQL parameters, and returns the result of the query.
    /// If forceCommandText is set to false and SQL parameters are provided, the command type is set to StoredProcedure.
    /// The function throws a DataAccessLayerException if an error occurs during execution.
    /// </summary>
    /// <param name="commandText">The command text to execute.</param>
    /// <param name="sqlParameters">The SQL parameters to use with the command.</param>
    /// <param name="forceCommandText">Whether to force the command type to Text, even if SQL parameters are provided.</param>
    /// <returns></returns>
    protected object ExecuteScalar(string commandText, SqlParameter[] sqlParameters = null, bool forceCommandText = false)
    {
        this.IsValid();
        try
        {
            using (SqlConnection mSqlConnection = new(this.ConnectionString))
            {
                mSqlConnection.Open();
                using (SqlCommand mSqlCommand = new(commandText, mSqlConnection))
                {
                    mSqlCommand.CommandType = CommandType.Text;
                    if (sqlParameters != null)
                    {
                        if (sqlParameters.Length > 0)
                        {
                            if (forceCommandText != true)
                            {
                                mSqlCommand.CommandType = CommandType.StoredProcedure;
                            }
                            foreach (SqlParameter mSqlParameter in sqlParameters)
                            {
                                mSqlCommand.Parameters.Add(mSqlParameter);
                            }
                        }
                    }
                    return mSqlCommand.ExecuteScalar();
                }
            }
        }
        catch (SqlException ex)
        {
            if (ex.Message.ToUpper(CultureInfo.InvariantCulture).StartsWith("CANNOT OPEN DATABASE", StringComparison.OrdinalIgnoreCase))
            {
                throw;
            }
            else
            {
                string mFormattedMsg = formatError(null, commandText, ex.ToString());
                DataAccessLayerException mDataAccessLayerException = new(mFormattedMsg, ex);
                this.m_Logger.Fatal(mDataAccessLayerException);
                throw mDataAccessLayerException;
            }
        }
        catch (Exception ex)
        {
            string mFormattedMsg = formatError(null, commandText, ex.ToString());
            DataAccessLayerException mDataAccessLayerException = new(mFormattedMsg, ex);
            this.m_Logger.Fatal(mDataAccessLayerException);
            throw mDataAccessLayerException;
        }
    }

    /// <summary>
    /// Executes a scalar query against the database.
    /// </summary>
    /// <param name="commandText">The command text to execute.</param>
    /// <returns>this.ExecuteScalar(commandText, null, true)</returns>
    protected object ExecuteScalar(string commandText)
    {
        return this.ExecuteScalar(commandText, null, true);
    }

    /// <summary>
    /// Formats an error message containing the store procedure name and the parameters/values.
    /// </summary>
    /// <param name="parameters">The sql parameters used when the error was created.</param>
    /// <param name="storedProcedure">The name of the store procedure used when the error was created.</param>
    /// <param name="yourExMSG">The message for the exception object.</param>
    /// <returns>A formatted string</returns>
    /// <remarks></remarks>
    private string formatError(SqlParameter[] parameters, string storedProcedure, string yourExMSG)
    {
        string mMessage = Environment.NewLine + "Error executing '" + storedProcedure + "' :: " + Environment.NewLine;
        SqlParameter testParameter = null;
        if(parameters != null)
        {
            mMessage += "Parameters are as follows:" + Environment.NewLine;
            foreach (SqlParameter testParameter_loopVariable in parameters)
            {
                testParameter = testParameter_loopVariable;
                mMessage += testParameter.ParameterName.ToString() + " = ";
                if (testParameter.Value != null)
                {
                    mMessage += testParameter.Value.ToString() + Environment.NewLine;
                }
                else
                {
                    mMessage += Environment.NewLine;
                }

            }
        }
        mMessage += "Connection string : " + ConnectionString + Environment.NewLine;
        mMessage += yourExMSG + Environment.NewLine;
        return mMessage;
    }

    /// <summary>
    /// Returns the correct integer for added or updated by
    /// </summary>
    /// <param name="profile">Object implementing IProfile</param>
    /// <returns>int</returns>
    protected static int GetAddedUpdatedBy(IBaseModel profile)
    {
        int mAdded_Updated_By = 0;
        if (profile != null)
        {
            if (profile.Id == -1)
            {
                mAdded_Updated_By = profile.AddedBy;
            }
            else
            {
                mAdded_Updated_By = profile.UpdatedBy;
            }
        }
        else
        {
            throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!!");
        }
        return mAdded_Updated_By;
    }

    protected static int GetAddedUpdatedBy(IAddedUpdated profile, int id)
    {
        int mAdded_Updated_By = 0;
        if (profile != null)
        {
            if (id == -1)
            {
                mAdded_Updated_By = profile.AddedBy;
            }
            else
            {
                mAdded_Updated_By = profile.UpdatedBy;
            }
        }
        else
        {
            throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!!");
        }
        return mAdded_Updated_By;
    }

    /// <summary>
    /// Returns a DataRow given the command text and sql parameters
    /// </summary>
    /// <param name="commandText">String</param>
    /// <param name="sqlParameter">SqlParameter[]</param>
    /// <returns>DataRow</returns>
    /// <remarks></remarks>
    protected virtual DataRow GetDataRow(String commandText, SqlParameter[] sqlParameters, bool forceCommandText = false)
    {
        this.IsValid();
        DataTable mDataTable = this.GetDataTable(commandText, sqlParameters, forceCommandText);
        if (mDataTable.Rows.Count > 0)
        {
            return mDataTable.Rows[0];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Returns a DataRow given the command text
    /// </summary>
    /// <param name="commandText">String</param>
    /// <returns>DataRow</returns>
    /// <remarks>Contains no logic</remarks>
    protected virtual DataRow GetDataRow(String commandText)
    {
        return this.GetDataRow(commandText, null);
    }

    protected virtual async Task<DataRow> GetDataRowAsync(string commandText, SqlParameter[] sqlParameters = null, bool forceCommandText = false)
    {
        var mRetVal = await GetDataTableAsync(commandText, sqlParameters, forceCommandText);
        return mRetVal.Rows.Count > 0 ? mRetVal.Rows[0] : null;
    }

    /// <summary>
    /// Returns a DataSet given the store procedure and sql parameters
    /// </summary>
    /// <param name="commandText">String</param>
    /// <returns>DataSet</returns>
    /// <remarks></remarks>
    protected virtual DataSet GetDataSet(String commandText, SqlParameter[] sqlParameter, bool forceCommandText = false)
    {
        this.IsValid();
        DataSet mRetVal = null;
        using (SqlConnection mSqlConnection = new(this.ConnectionString))
        {
            mSqlConnection.Open();
            using (SqlCommand mSqlCommand = new SqlCommand(commandText, mSqlConnection))
            {
                mSqlCommand.CommandType = CommandType.Text;
                if (sqlParameter != null)
                {
                    if (sqlParameter.Length > 0)
                    {
                        if (forceCommandText != true)
                        {
                            mSqlCommand.CommandType = CommandType.StoredProcedure;
                        }
                        foreach (SqlParameter mSqlParameter in sqlParameter)
                        {
                            mSqlCommand.Parameters.Add(mSqlParameter);
                        }
                    }
                }
                using SqlDataAdapter mSqlDataAdapter = new(mSqlCommand);
                mRetVal = new DataSet();
                try
                {
                    mSqlDataAdapter.Fill(mRetVal);
                }
                catch (System.Exception ex)
                {
                    // Log an Info message if the error message starts with "Invalid column name"
                    if (ex.Message.IndexOf("Invalid column name", 0, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        m_Logger.Info(ex);
                    }
                    else
                    {
                        m_Logger.Error(ex);
                    }
                    // Throw the exception
                    throw;
                }
            }
        }
        return mRetVal;
    }

    /// <summary>
    /// Returns a DataSet given the stored procedure
    /// </summary>
    /// <param name="commandText">String</param>
    /// <returns>DataSet</returns>
    /// <remarks>Contains no logic</remarks>
    protected virtual DataSet GetDataSet(String commandText)
    {
        return this.GetDataSet(commandText, null);
    }

    protected virtual async Task<DataSet> GetDataSetAsync(string commandText, SqlParameter[] sqlParameters = null, bool forceCommandText = false)
    {
        var mRetVal = new DataSet();
        await executeAsync(commandText, sqlParameters, forceCommandText, async mSqlCommand =>
        {
            using var mSqlDataAdapter = new SqlDataAdapter(mSqlCommand);
            await Task.Run(() => mSqlDataAdapter.Fill(mRetVal));
            return 0;
        });
        return mRetVal;
    }

    /// <summary>
    /// Returns a DataTable given the command text, Sql Connection and sql parameters
    /// </summary>
    /// <param name="commandText">String</param>
    /// <param name="sqlParameter">SqlParameter[]</param>
    /// <returns>DataTable</returns>
    /// <remarks></remarks>
    protected virtual DataTable GetDataTable(String commandText, SqlParameter[] sqlParameter, bool forceCommandText = false)
    {
        DataSet mDataSet = this.GetDataSet(commandText, sqlParameter, forceCommandText);
        DataTable mDataTable = mDataSet.Tables[0].Copy();
        return mDataTable;
    }

    /// <summary>
    /// Returns a DataTable given the command text and Sql Connection
    /// </summary>
    /// <param name="commandText">String</param>
    /// <returns>DataTable</returns>
    /// <remarks>Contains no logic</remarks>
    protected virtual DataTable GetDataTable(String commandText)
    {
        return this.GetDataTable(commandText, null);
    }
    
    protected virtual async Task<DataTable> GetDataTableAsync(string commandText, SqlParameter[] sqlParameters = null, bool forceCommandText = false)
    {
        DataTable mRetVal = new();
        await executeAsync(commandText, sqlParameters, forceCommandText, async mSqlCommand =>
        {
            await using var reader = await mSqlCommand.ExecuteReaderAsync();
            mRetVal.Load(reader);
            return 0;
        });
        return mRetVal;
    }

    /// <summary>
    /// Returns the value of an output parameter given the parameter name and an array of parameters
    /// </summary>
    /// <param name="parameterName">parameterName</param>
    /// <param name="parameters">parameters</param>
    /// <returns></returns>
    /// <remarks></remarks>
    protected static string GetParameterValue(String parameterName, SqlParameter[] parameters)
    {
        String mRetVal = string.Empty;
        if (parameters != null)
        {
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter.ParameterName.ToLowerInvariant() == parameterName.ToLowerInvariant())
                {
                    if (!Convert.IsDBNull(parameter.Value))
                    {
                        mRetVal = parameter.Value.ToString();
                    }
                }
            }
        }
        else
        {
            throw new ArgumentNullException(nameof(parameters), "parameters cannot be a null reference (Nothing in Visual Basic)!");
        }
        return mRetVal;
    }

    /// <summary>
    /// Returns a SqlParameter given the ParameterName, ParameterValue and Direction.
    /// </summary>
    /// <param name="parameterName">String</param>
    /// <param name="parameterValue">Object</param>
    /// <param name="direction">ParameterDirection</param>
    /// <returns>SqlParameter</returns>
    protected virtual SqlParameter GetSqlParameter(String parameterName, Object parameterValue, ParameterDirection direction)
    {
        SqlParameter mRetVal = new SqlParameter(parameterName, parameterValue);
        switch (direction)
        {
            case ParameterDirection.Input:
                mRetVal.Direction = ParameterDirection.Input;
                break;
            case ParameterDirection.InputOutput:
                mRetVal.Direction = ParameterDirection.InputOutput;
                break;
            case ParameterDirection.Output:
                mRetVal.Direction = ParameterDirection.Output;
                break;
            case ParameterDirection.ReturnValue:
                mRetVal.Direction = ParameterDirection.ReturnValue;
                break;
            default:
                mRetVal.Direction = ParameterDirection.Input;
                break;
        }
        return mRetVal;
    }

    private void isConnectionStringSet()
    {
        if (String.IsNullOrEmpty(this.ConnectionString) | String.IsNullOrWhiteSpace(this.ConnectionString))
        {
            throw new DataAccessLayerException("The ConnectionString property cannot be null or blank!");
        }
    }

    /// <summary>
    /// Ensures  ConnectionString has a value.
    /// </summary>
    /// <remarks>Throws ArgumentException</remarks>
    protected virtual void IsValid()
    {
        this.isConnectionStringSet();
    }

}
