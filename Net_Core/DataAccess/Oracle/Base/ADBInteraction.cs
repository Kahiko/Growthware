using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework;
using GrowthWare.Framework.Interfaces;
using GrowthWare.Framework.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace GrowthWare.DataAccess.Oracle.Base;

/// <summary>
/// Performs all data store interaction to SQL Server.
/// </summary>
public abstract class AbstractDBInteraction : IDBInteraction, IDisposable
{
    private string m_ConnectionString = string.Empty;
    private bool m_DisposedValue;
    private Logger m_Logger = Logger.Instance();

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
            this.m_ConnectionString = value;
        }
    }

    /// <summary>
    /// Performs a bulk upload of IDatabaseFunctions objects into the
    /// database. Note: Requires an object with a primary key!
    /// </summary>
    /// <param name="listOfProfiles">An array of IDatabaseFunctions</param>
    /// <param name="doDelete"></param>
    /// <param name="includePrimaryKey"></param>
    internal void BulkInsert(DTO_BulkInsert_Parameters bulkInsertParameters)
    {
        if (bulkInsertParameters.ListOfProfiles != null && bulkInsertParameters.ListOfProfiles.Any())
        {
            DataTable mDataTable = bulkInsertParameters.EmptyTable.Clone();
            string mCommandText = string.Empty;

            // Populate mDataTable with the data from the listOfIDatabaseFunctions
            foreach (var item in bulkInsertParameters.ListOfProfiles)
            {
                DataRow mRow = mDataTable.NewRow();
                PropertyInfo[] mProperties = item.GetType().GetProperties();
                foreach (PropertyInfo mPropertyItem in mProperties)
                {
                    var mValue = mPropertyItem.GetValue(item, null);
                    var mPropertyType = mPropertyItem.PropertyType;
                    // deal with nullable properties
                    if (mPropertyType.IsGenericType && mPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        mPropertyType = mPropertyType.GetGenericArguments()[0];
                    }
                    if (mValue == null)
                    {
                        mValue = DBNull.Value;
                    }
                    if (!bulkInsertParameters.IncludePrimaryKey)
                    {
                        if (!bulkInsertParameters.PrimaryKeyName.Replace("[", "").Replace("]", "").Equals(mPropertyItem.Name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            mRow[mPropertyItem.Name] = mValue;
                        }
                    }
                }
                mDataTable.Rows.Add(mRow);
            }

            using var mOracleConnection = new OracleConnection(this.ConnectionString);
            mOracleConnection.Open();
            OracleTransaction mOracleTransaction = mOracleConnection.BeginTransaction();

            // 1.) Create Destination Table
            mCommandText = string.Format("SELECT * INTO {0} FROM {1} Where 1 = 2", bulkInsertParameters.TempTableName, bulkInsertParameters.DestinationTableName);
            using (OracleCommand mOracleCommand = new OracleCommand(mCommandText))
            {
                mOracleCommand.Connection = mOracleConnection;
                mOracleCommand.CommandType = CommandType.Text;
                mOracleCommand.Transaction = mOracleTransaction;
                if (mOracleConnection.State != ConnectionState.Open)
                {
                    mOracleConnection.Open();
                }
                mOracleCommand.ExecuteNonQuery();
            }
            if (!bulkInsertParameters.IncludePrimaryKey)
            {
                mCommandText = string.Format("ALTER TABLE {0} DROP COLUMN IF EXISTS {1}", bulkInsertParameters.TempTableName, bulkInsertParameters.PrimaryKeyName);
                using (OracleCommand mOracleCommand = new OracleCommand(mCommandText))
                {
                    mOracleCommand.Connection = mOracleConnection;
                    mOracleCommand.CommandType = CommandType.Text;
                    mOracleCommand.Transaction = mOracleTransaction;
                    if (mOracleConnection.State != ConnectionState.Open)
                    {
                        mOracleConnection.Open();
                    }
                    mOracleCommand.ExecuteNonQuery();
                }
            }

            // 2.) Perform OracleBulkCopy of the data table in a temporary table
            using (var mOracleBulkCopy = new OracleBulkCopy(mOracleConnection, OracleBulkCopyOptions.Default))
            {
                // mOracleBulkCopy.BatchSize = 5000;
                mOracleBulkCopy.BatchSize = mDataTable.Rows.Count;
                mOracleBulkCopy.DestinationTableName = bulkInsertParameters.TempTableName;
                mOracleBulkCopy.WriteToServer(mDataTable);
                mOracleBulkCopy.Close();
            }
            // 3.) Delete all rows associated from the db if needed
            if (bulkInsertParameters.DoDelete)
            {
                // semi correct should account for the primary key or in other words join on all columns except the primary key
                // b/c the primary key if one exists is always unique
                // for now I'm going to insist that the Destination have a single foreign key
                mCommandText = "DELETE {0} FROM {0} Destination INNER JOIN {1} TempTable ON Destination.{2} = TempTable.{2};";
                mCommandText = string.Format(mCommandText, bulkInsertParameters.DestinationTableName, bulkInsertParameters.TempTableName, bulkInsertParameters.ForeignKeyName);
                using OracleCommand mOracleCommand = new OracleCommand(mCommandText)
                {
                    Connection = mOracleConnection,
                    CommandType = CommandType.Text,
                    Transaction = mOracleTransaction
                };
                if (mOracleConnection.State != ConnectionState.Open)
                {
                    mOracleConnection.Open();
                }
                mOracleCommand.ExecuteNonQuery();
            }
            // 4.) Insert all the rows from the temp table into Destination
            mCommandText = string.Format("INSERT INTO {0} SELECT * FROM {1}", bulkInsertParameters.DestinationTableName, bulkInsertParameters.TempTableName);
            using (OracleCommand mOracleCommand = new OracleCommand(mCommandText))
            {
                mOracleCommand.Connection = mOracleConnection;
                mOracleCommand.CommandType = CommandType.Text;
                mOracleCommand.Transaction = mOracleTransaction;
                if (mOracleConnection.State != ConnectionState.Open)
                {
                    mOracleConnection.Open();
                }
                mOracleCommand.ExecuteNonQuery();
            }
            // 5.) finally delete the temporary table
            mCommandText = string.Format("DROP TABLE {0}", bulkInsertParameters.TempTableName);
            using (OracleCommand mOracleCommand = new OracleCommand(mCommandText))
            {
                mOracleCommand.Connection = mOracleConnection;
                mOracleCommand.CommandType = CommandType.Text;
                mOracleCommand.Transaction = mOracleTransaction;
                if (mOracleConnection.State != ConnectionState.Open)
                {
                    mOracleConnection.Open();
                }
                mOracleCommand.ExecuteNonQuery();
            }
            mOracleTransaction.Commit();
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
            mRetVal = mRetVal.Replace(";", "");
            mRetVal = mRetVal.Replace("'", "");
            mRetVal = mRetVal.Replace("\"", "");
        }
        return mRetVal;
    }

    /// <summary>
    /// Implements IDispose
    /// </summary>
    /// <param name="disposing">Boolean</param>
    /// <remarks></remarks>
    protected virtual void Dispose(bool disposing)
    {
        // Check to see if Dispose has already been called.
        if (!m_DisposedValue)
        {
            if (disposing)
            {
                // // Dispose managed resources if you have any.
            }
            // TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            // TODO: set large fields to null.

        }
        m_DisposedValue = true;
    }

    /// <summary>
    /// Implements Dispose
    /// </summary>
    /// <remarks></remarks>
    public void Dispose()
    {
        //Do not change this code.  Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Executes a non Query given the commandText and sql parameters if any
    /// </summary>
    /// <param name="commandText">String</param>
    /// <param name="OracleParameter">OracleParameter</param>
    /// <exception cref="DataAccessLayerException"></exception>
    protected int ExecuteNonQuery(String commandText, OracleParameter[] OracleParameters, bool forceCommandText = false)
    {
        this.IsValid();
        try
        {
            using (OracleConnection mOracleConnection = new(this.ConnectionString))
            {
                mOracleConnection.Open();
                using (OracleCommand mOracleCommand = new(commandText, mOracleConnection))
                {
                    mOracleCommand.CommandType = CommandType.Text;
                    if (OracleParameters != null)
                    {
                        if (OracleParameters.Length > 0)
                        {
                            if (forceCommandText != true)
                            {
                                mOracleCommand.CommandType = CommandType.StoredProcedure;
                            }
                            foreach (OracleParameter mOracleParameter in OracleParameters)
                            {
                                mOracleCommand.Parameters.Add(mOracleParameter);
                            }
                        }
                    }
                    return mOracleCommand.ExecuteNonQuery();
                }
            }
        }
        catch (OracleException ex)
        {
            if (ex.Message.ToUpper(CultureInfo.InvariantCulture).StartsWith("CANNOT OPEN DATABASE", StringComparison.OrdinalIgnoreCase))
            {
                throw;
            }
            else
            {
                string mFormattedMsg = formatError(OracleParameters, commandText, ex.ToString());
                DataAccessLayerException mDataAccessLayerException = new(mFormattedMsg, ex);
                this.m_Logger.Fatal(mDataAccessLayerException);
                throw mDataAccessLayerException;
            }
        }
        catch (Exception ex)
        {
            string mFormattedMsg = formatError(OracleParameters, commandText, ex.ToString());
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
    /// Executes a scalar query against the database.
    /// This function takes a command text and optional SQL parameters, and returns the result of the query.
    /// If forceCommandText is set to false and SQL parameters are provided, the command type is set to StoredProcedure.
    /// The function throws a DataAccessLayerException if an error occurs during execution.
    /// </summary>
    /// <param name="commandText">The command text to execute.</param>
    /// <param name="OracleParameters">The SQL parameters to use with the command.</param>
    /// <param name="forceCommandText">Whether to force the command type to Text, even if SQL parameters are provided.</param>
    /// <returns></returns>
    protected object ExecuteScalar(string commandText, OracleParameter[] OracleParameters, bool forceCommandText = false)
    {
        this.IsValid();
        try
        {
            using (OracleConnection mOracleConnection = new(this.ConnectionString))
            {
                mOracleConnection.Open();
                using (OracleCommand mOracleCommand = new(commandText, mOracleConnection))
                {
                    mOracleCommand.CommandType = CommandType.Text;
                    if (OracleParameters != null)
                    {
                        if (OracleParameters.Length > 0)
                        {
                            if (forceCommandText != true)
                            {
                                mOracleCommand.CommandType = CommandType.StoredProcedure;
                            }
                            foreach (OracleParameter mOracleParameter in OracleParameters)
                            {
                                mOracleCommand.Parameters.Add(mOracleParameter);
                            }
                        }
                    }
                    return mOracleCommand.ExecuteScalar();
                }
            }
        }
        catch (OracleException ex)
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
    private string formatError(OracleParameter[] parameters, string storedProcedure, string yourExMSG)
    {
        string mMessage = Environment.NewLine + "Error executing '" + storedProcedure + "' :: " + Environment.NewLine;
        OracleParameter testParameter = null;
        if (parameters != null)
        {
            mMessage += "Parameters are as follows:" + Environment.NewLine;
            foreach (OracleParameter testParameter_loopVariable in parameters)
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
            throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!");
        }
        return mAdded_Updated_By;
    }

    /// <summary>
    /// Returns a DataRow given the command text and sql parameters
    /// </summary>
    /// <param name="commandText">String</param>
    /// <param name="OracleParameter">OracleParameter[]</param>
    /// <returns>DataRow</returns>
    /// <remarks></remarks>
    protected virtual DataRow GetDataRow(String commandText, OracleParameter[] OracleParameters, bool forceCommandText = false)
    {
        this.IsValid();
        DataTable mDataTable = this.GetDataTable(commandText, OracleParameters, forceCommandText);
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

    /// <summary>
    /// Returns a DataSet given the store procedure and sql parameters
    /// </summary>
    /// <param name="commandText">String</param>
    /// <returns>DataSet</returns>
    /// <remarks></remarks>
    protected virtual DataSet GetDataSet(String commandText, OracleParameter[] OracleParameter, bool forceCommandText = false)
    {
        this.IsValid();
        DataSet mRetVal = null;

        using (OracleConnection mOracleConnection = new(this.ConnectionString))
        {
            mOracleConnection.Open();
            using (OracleCommand mOracleCommand = new OracleCommand(commandText, mOracleConnection))
            {
                mOracleCommand.CommandType = CommandType.Text;
                if (OracleParameter != null)
                {
                    if (OracleParameter.Length > 0)
                    {
                        if (forceCommandText != true)
                        {
                            mOracleCommand.CommandType = CommandType.StoredProcedure;
                        }
                        foreach (OracleParameter mOracleParameter in OracleParameter)
                        {
                            mOracleCommand.Parameters.Add(mOracleParameter);
                        }
                    }
                }
                using OracleDataAdapter mOracleDataAdapter = new(mOracleCommand);
                mRetVal = new DataSet();
                try
                {
                    mOracleDataAdapter.Fill(mRetVal);
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

    /// <summary>
    /// Returns a DataTable given the command text, Sql Connection and sql parameters
    /// </summary>
    /// <param name="commandText">String</param>
    /// <param name="OracleParameter">OracleParameter[]</param>
    /// <returns>DataTable</returns>
    /// <remarks></remarks>
    protected virtual DataTable GetDataTable(String commandText, OracleParameter[] OracleParameter, bool forceCommandText = false)
    {
        DataSet mDataSet = this.GetDataSet(commandText, OracleParameter, forceCommandText);
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

    /// <summary>
    /// Returns the value of an output parameter given the parameter name and an array of parameters
    /// </summary>
    /// <param name="parameterName">parameterName</param>
    /// <param name="parameters">parameters</param>
    /// <returns></returns>
    /// <remarks></remarks>
    protected static string GetParameterValue(String parameterName, OracleParameter[] parameters)
    {
        String mRetVal = string.Empty;
        if (parameters != null)
        {
            foreach (OracleParameter parameter in parameters)
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
            throw new ArgumentNullException("parameters", "parameters cannot be a null reference (Nothing in Visual Basic)!");
        }
        return mRetVal;
    }

    /// <summary>
    /// Returns a OracleParameter given the ParameterName, ParameterValue and Direction.
    /// </summary>
    /// <param name="parameterName">String</param>
    /// <param name="parameterValue">Object</param>
    /// <param name="direction">ParameterDirection</param>
    /// <returns>OracleParameter</returns>
    protected virtual OracleParameter GetOracleParameter(String parameterName, Object parameterValue, ParameterDirection direction)
    {
        OracleParameter mRetVal = new OracleParameter(parameterName, parameterValue);
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

    //// TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    //~DDBInteraction() 
    //{
    //    // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    //    Dispose(false);
    //}

}
