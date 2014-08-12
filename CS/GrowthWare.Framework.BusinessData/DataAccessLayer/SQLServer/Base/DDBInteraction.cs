using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base;
using GrowthWare.Framework.Model.Profiles.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.Base
{
    /// <summary>
    /// Performs all data store interaction to SQL Server through the use of stored procedures only.
    /// </summary>
    /// <remarks>Uses Microsoft.Practices.EnterpriseLibrary.Data for underlying database access.</remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DDB")]
    public abstract class DDBInteraction : IDDBInteraction, IDisposable
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
        /// Formats an error message containging the store procedure name and the parameters/values.
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
            mMessage += "Connection string : " + ConnectionString + Environment.NewLine;
            mMessage += yourExMSG + Environment.NewLine;
            return mMessage;
        }

        /// <summary>
        /// Ensures  ConnectionString has a value.
        /// </summary>
        /// <remarks>Throws ArgumentException</remarks>
        protected virtual void IsValid()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new DataAccessLayerException("The connection string property cannot be blank!");
            }
        }

        /// <summary>
        /// Determines the commad timeout based on the connection time out
        /// The default is 600
        /// </summary>
        /// <returns>int</returns>
        private int getCommandTimeOut()
        {
            int mRetVal = 600;
            string[] connectionParameters = ConnectionString.Split(';');
            foreach (string item in connectionParameters)
            {
                string[] mParameter = item.Split('=');
                if (mParameter[0].ToString().ToUpper(CultureInfo.InvariantCulture) == "CONNECT TIMEOUT" || mParameter[0].ToString().ToUpper(CultureInfo.InvariantCulture) == "CONNECTION TIMEOUT")
                {
                    mRetVal = int.Parse(mParameter[1].ToString(), CultureInfo.InvariantCulture);
                }
            }
            return mRetVal;
        }
        #endregion

        #region IDDBInteraction Members
        /// <summary>
        /// Executes a non Query given the store procedure and sql parameters
        /// </summary>
        /// <param name="storedProcedure">String</param>
        /// <param name="parameters">SqlParmeter</param>
        /// <remarks></remarks>
        protected void ExecuteNonQuery(string storedProcedure, SqlParameter[] parameters)
        {
            this.IsValid();
            SqlParameter myParameter = null;
            try
            {
                if ((parameters != null))
                {
                    if (parameters.Length > 0)
                    {
                        SqlDatabase db = new SqlDatabase(ConnectionString);
                        DbCommand dbCommand = db.GetStoredProcCommand(storedProcedure);
                        dbCommand.CommandTimeout = getCommandTimeOut();
                        foreach (SqlParameter myParameter_loopVariable in parameters)
                        {
                            myParameter = myParameter_loopVariable;
                            dbCommand.Parameters.Add(myParameter);
                        }
                        db.ExecuteNonQuery(dbCommand);
                    }
                    else
                    {
                        SqlDatabase db = new SqlDatabase(ConnectionString);
                        db.ExecuteNonQuery(storedProcedure, parameters);
                    }
                }
                else
                {
                    SqlDatabase db = new SqlDatabase(ConnectionString);
                    db.ExecuteNonQuery(CommandType.StoredProcedure, storedProcedure);
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
                    throw new DataAccessLayerException(formatError(parameters, storedProcedure, ex.ToString()), ex);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessLayerException(formatError(parameters, storedProcedure, ex.ToString()), ex);
            }
        }

        /// <summary>
        /// Executes a non Query given the store procedure
        /// </summary>
        /// <param name="storedProcedure">String</param>
        /// <remarks>contains no logic</remarks>
        protected void ExecuteNonQuery(String storedProcedure)
        {
            this.ExecuteNonQuery(storedProcedure, null);
        }

        /// <summary>
        /// Returns a DataSet given the store procedure and sql parameters
        /// </summary>
        /// <param name="storedProcedure">String</param>
        /// <param name="parameters">SqlParmeter</param>
        /// <returns>DataSet</returns>
        /// <remarks></remarks>
        protected virtual DataSet GetDataSet(String storedProcedure, SqlParameter[] parameters)
        {
            this.IsValid();
            SqlParameter mParameter = null;
            SqlDatabase db = new SqlDatabase(ConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand(storedProcedure);
            dbCommand.CommandTimeout = getCommandTimeOut();
            DataSet mRetDataSet = null;
            try
            {
                if (parameters != null)
                {
                    if (parameters.Length > 0)
                    {
                        foreach (SqlParameter myParameter_loopVariable in parameters)
                        {
                            mParameter = myParameter_loopVariable;
                            dbCommand.Parameters.Add(mParameter);
                        }
                        mRetDataSet = db.ExecuteDataSet(dbCommand);
                    }
                    else
                    {
                        mRetDataSet = db.ExecuteDataSet(dbCommand);
                    }
                }
                else
                {
                    mRetDataSet = db.ExecuteDataSet(dbCommand);
                }
                if (mRetDataSet == null || mRetDataSet.Tables.Count == 0 || mRetDataSet.Tables[0].Rows.Count == 0) 
                {
                    string myMessage = "Store procedure '" + storedProcedure + "' executed and no data was found." + Environment.NewLine;
                    if (parameters != null) 
                    {
                        myMessage += "Parameters are as follows:" + Environment.NewLine;
                        foreach (SqlParameter testParameter in parameters)
                        {
                            myMessage += testParameter.ParameterName + " = " + testParameter.Value + Environment.NewLine;
                        }
                    }
                    else
                    {
                        myMessage += "No parameters supplied.";
                    }
                    throw new DataAccessLayerException(myMessage);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if(dbCommand != null) dbCommand.Dispose();
            }
            return mRetDataSet;
        }

        /// <summary>
        /// Returns a DataSet given the stored procedure
        /// </summary>
        /// <param name="storedProcedure">String</param>
        /// <returns>DataSet</returns>
        /// <remarks>Containts no logic</remarks>
        protected virtual DataSet GetDataSet(String storedProcedure)
        {
            SqlParameter[] mParameters = null;
            return this.GetDataSet(storedProcedure, mParameters);
        }

        /// <summary>
        /// Returns a DataTable given the stored procedure and sql parameters
        /// </summary>
        /// <param name="storedProcedure">String</param>
        /// <param name="parameters">SqlParameter</param>
        /// <returns>DataTable</returns>
        /// <remarks></remarks>
        protected virtual DataTable GetDataTable(String storedProcedure, SqlParameter[] parameters)
        {
            this.IsValid();
            DataTable mDataTable = null;
            IDataReader mReader = null;
            try
            {
                if ((parameters != null))
                {
                    if (parameters.Length > 0)
                    {
                        SqlDatabase db = new SqlDatabase(ConnectionString);
                        DbCommand dbCommand = db.GetStoredProcCommand(storedProcedure);
                        dbCommand.CommandTimeout = getCommandTimeOut();
                        foreach (SqlParameter parameter in parameters)
                        {
                            dbCommand.Parameters.Add(parameter);
                        }
                        mReader = db.ExecuteReader(dbCommand);
                    }
                    else
                    {
                        SqlDatabase db = new SqlDatabase(ConnectionString);
                        mReader = db.ExecuteReader(CommandType.StoredProcedure, storedProcedure);
                    }
                }
                else
                {
                    SqlDatabase db = new SqlDatabase(ConnectionString);
                    mReader = db.ExecuteReader(CommandType.StoredProcedure, storedProcedure);
                }
                if ((mReader != null))
                {
                    mDataTable = new DataTable();
                    mDataTable.Locale = CultureInfo.InvariantCulture;
                    mDataTable.Load(mReader);
                    return mDataTable;
                }
                else
                {
                    string mMessage = Environment.NewLine + "Store procedure '" + storedProcedure + "' executed and no data was found." + Environment.NewLine;
                    mMessage += "Parameters are as follows:" + Environment.NewLine;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            mMessage += parameter.ParameterName + " = " + parameter.Value + Environment.NewLine;
                        }
                    }
                    else 
                    {
                        mMessage += "No parameters supplied.";
                    }
                    throw new DataAccessLayerException(mMessage);
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
                    throw new DataAccessLayerException(formatError(parameters, storedProcedure, ex.ToString()), ex);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessLayerException(formatError(parameters, storedProcedure, ex.ToString()), ex);
            }
            finally
            {
                if ((mReader != null))
                {
                    mReader.Close();
                    mReader = null;
                }
                if (mDataTable != null)
                {
                    mDataTable.Dispose();
                    mDataTable = null;
                }
            }
        }

        /// <summary>
        /// Returns a DataTable given the stored procedure
        /// </summary>
        /// <param name="storedProcedure">String</param>
        /// <returns>DataTable</returns>
        /// <remarks>Containts no logic</remarks>
        protected virtual DataTable GetDataTable(String storedProcedure)
        {
            SqlParameter[] mParameters = null;
            return this.GetDataTable(storedProcedure, mParameters);
        }

        /// <summary>
        /// Returns a DataRow given the store procedure and sql parameters
        /// </summary>
        /// <param name="storedProcedure">Stirng</param>
        /// <param name="parameters">SqlParameter</param>
        /// <returns>DataRow</returns>
        /// <remarks></remarks>
        protected virtual DataRow GetDataRow(String storedProcedure, SqlParameter[] parameters)
        {
            this.IsValid();
            return this.GetDataTable(storedProcedure, parameters).Rows[0];
        }

        /// <summary>
        /// Returns a DataRow given the stored procedure
        /// </summary>
        /// <param name="storedProcedure">String</param>
        /// <returns>DataRow</returns>
        /// <remarks>Containts no logic</remarks>
        protected virtual DataRow GetDataRow(String storedProcedure)
        {
            SqlParameter[] mParameters = null;
            return this.GetDataRow(storedProcedure, mParameters);
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
                    if (parameter.ParameterName == parameterName)
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
                throw new ArgumentNullException("parameters", "parameters can not be null");
            }
            return mRetVal;
        }

        /// <summary>
        /// Retruns a SqlParameter given the ParameterName, ParameterValue and Direction.
        /// </summary>
        /// <param name="parameterName">String</param>
        /// <param name="parameterValue">Object</param>
        /// <param name="direction">ParameterDirection</param>
        /// <returns>SqlParameter</returns>
        protected static SqlParameter GetSqlParameter(String parameterName, Object parameterValue, ParameterDirection direction)
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

        /// <summary>
        /// Returns the correct integer for added or updated by
        /// </summary>
        /// <param name="profile">Object implementing IProfile</param>
        /// <returns>int</returns>
        protected static int GetAddedUpdatedBy(IMProfile profile)
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
                throw new ArgumentNullException("profile", "profile can not be null!");
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
}
