using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Interfaces;
using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Globalization;
using System.Reflection;

namespace GrowthWare.DataAccess.Oracle.Base;

/// <summary>
/// Performs all data store interaction to Iracle Server.
/// </summary>
public abstract class AbstractDBInteraction : IDBInteraction, IDisposable
{
    #region Private Fields
    private bool m_DisposedValue;
    #endregion

    #region Public Properties
    /// <summary>
    /// Used for all methods to connect to the database.
    /// </summary>
    public string ConnectionString { get; set; }
    #endregion

    #region Private Methods
    /// <summary>
    /// Formats an error message containing the store procedure name and the parameters/values.
    /// </summary>
    /// <param name="parameters">The oracle parameters used when the error was created.</param>
    /// <param name="storedProcedure">The name of the store procedure used when the error was created.</param>
    /// <param name="yourExMSG">The message for the exception object.</param>
    /// <returns>A formatted string</returns>
    /// <remarks></remarks>
    private string formatError(OracleParameter[] parameters, string storedProcedure, string yourExMSG)
    {
        string mMessage = Environment.NewLine + "Error executing '" + storedProcedure + "' :: " + Environment.NewLine;
        OracleParameter testParameter = null;
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
        mMessage += "Connection string : " + ConnectionString + Environment.NewLine;
        mMessage += yourExMSG + Environment.NewLine;
        return mMessage;
    }

    /// <summary>
    /// Performs a bulk upload of IDatabaseFunctions objects into the
    /// database. Note: Requires an object with a primary key!
    /// </summary>
    /// <param name="listOfProfiles">An array of IDatabaseFunctions</param>
    /// <param name="doDelete"></param>
    /// <param name="includePrimaryKey"></param>
    internal void BulkInsert(IDatabaseFunctions[] listOfProfiles, bool doDelete, bool includePrimaryKey)
    {
        if (listOfProfiles != null && listOfProfiles.Length > 0)
        {
            string mTempTableName = "[" + Guid.NewGuid().ToString() + "]";
            IDatabaseFunctions mFirstObj = listOfProfiles[0];
            DataTable mDataTable = mFirstObj.GetEmptyTable(mTempTableName, includePrimaryKey);
            string mDestinationTableName = mFirstObj.GetTableName();
            string mPrimaryKeyName = mFirstObj.GetPrimaryKeyName();
            string mForeignKeyName = mFirstObj.GetForeignKeyName();
            bool mForeignKeyIsNumber = mFirstObj.IsForeignKeyNumber();
            string mCommandText = string.Empty;

            // Populate mDataTable with the data from the listOfIDatabaseFunctions
            foreach (var item in listOfProfiles)
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
                    if (!includePrimaryKey)
                    {
                        if (mPrimaryKeyName.Replace("[", "").Replace("]", "").ToLowerInvariant() != mPropertyItem.Name.ToLowerInvariant())
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
            mCommandText = string.Format("SELECT * INTO {0} FROM {1} Where 1 = 2", mTempTableName, mDestinationTableName);
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
            if (!includePrimaryKey)
            {
                mCommandText = string.Format("ALTER TABLE {0} DROP COLUMN IF EXISTS {1}", mTempTableName, mFirstObj.GetPrimaryKeyName());
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
                mOracleBulkCopy.DestinationTableName = mTempTableName;
                mOracleBulkCopy.WriteToServer(mDataTable);
                mOracleBulkCopy.Close();
            }
            // 3.) Delete all rows associated from the db if needed
            if (doDelete)
            {
                // semi correct should account for the primary key or in other words join on all columns except the primary key
                // b/c the primary key if one exists is always unique
                // for now I'm going to insist that the Destination have a single foreign key
                mCommandText = "DELETE {0} FROM {1} Destination INNER JOIN {2} TempTable ON Destination.{3} = TempTable.{4};";
                mCommandText = string.Format(mCommandText, mDestinationTableName, mDestinationTableName, mTempTableName, mForeignKeyName, mForeignKeyName);
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
            mCommandText = string.Format("INSERT INTO {0} SELECT * FROM {1}", mDestinationTableName, mTempTableName);
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
            mCommandText = string.Format("DROP TABLE {0}", mTempTableName);
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
    /// Ensures  ConnectionString has a value.
    /// </summary>
    /// <remarks>Throws ArgumentException</remarks>
    protected virtual void IsValid()
    {
        if (String.IsNullOrEmpty(this.ConnectionString) | String.IsNullOrWhiteSpace(this.ConnectionString))
        {
            throw new DataAccessLayerException("The ConnectionString property cannot be null or blank!");
        }
    }
    #endregion

    #region IDBInteraction Members
    /// <summary>
    /// Executes a non Query given the commandText and oracle parameters if any
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
                throw new DataAccessLayerException(formatError(OracleParameters, commandText, ex.ToString()), ex);
            }
        }
        catch (Exception ex)
        {
            throw new DataAccessLayerException(formatError(OracleParameters, commandText, ex.ToString()), ex);
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
                throw new DataAccessLayerException(formatError(null, commandText, ex.ToString()), ex);
            }
        }
        catch (Exception ex)
        {
            throw new DataAccessLayerException(formatError(null, commandText, ex.ToString()), ex);
        }
    }

    /// <summary>
    /// Returns a DataSet given the store procedure and oracle parameters
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
                mOracleDataAdapter.Fill(mRetVal);
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
    /// Returns a DataTable given the command text, Oracle Connection and oracle parameters
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
    /// Returns a DataTable given the command text and Oracle Connection
    /// </summary>
    /// <param name="commandText">String</param>
    /// <returns>DataTable</returns>
    /// <remarks>Contains no logic</remarks>
    protected virtual DataTable GetDataTable(String commandText)
    {
        return this.GetDataTable(commandText, null);
    }

    /// <summary>
    /// Returns a DataRow given the command text and oracle parameters
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
    #endregion

    #region IDisposable Members
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

    //// TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    //~DDBInteraction() 
    //{
    //    // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    //    Dispose(false);
    //}

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
    #endregion
}
