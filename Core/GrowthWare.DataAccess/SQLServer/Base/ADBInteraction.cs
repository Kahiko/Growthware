using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace GrowthWare.DataAccess.SQLServer.Base
{
    /// <summary>
    /// Performs all data store interaction to SQL Server.
    /// </summary>
    public abstract class ADBInteraction : IDBInteraction, IDisposable
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
            if (String.IsNullOrEmpty(this.ConnectionString) | String.IsNullOrWhiteSpace(this.ConnectionString))
            {
                throw new DataAccessLayerException("The ConnectionString property cannot be null or blank!");
            }
        }
#endregion

#region IDDBInteraction Members
        /// <summary>
        /// Executes a non Query given the commandText and sql parameters if any
        /// </summary>
        /// <param name="commandText">String</param>
        /// <param name="sqlParameter">SqlParmeter</param>
        /// <exception cref="DataAccessLayerException"></exception>
        protected int ExecuteNonQuery(String commandText, SqlParameter[] mSqlParameters)
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
                        if (mSqlParameters != null)
                        {
                            if (mSqlParameters.Length > 0)
                            {
                                mSqlCommand.CommandType = CommandType.StoredProcedure;
                                foreach (SqlParameter mSqlParameter in mSqlParameters)
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
                    throw new DataAccessLayerException(formatError(mSqlParameters, commandText, ex.ToString()), ex);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessLayerException(formatError(mSqlParameters, commandText, ex.ToString()), ex);
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
        /// Returns a DataSet given the store procedure and sql parameters
        /// </summary>
        /// <param name="commandText">String</param>
        /// <returns>DataSet</returns>
        /// <remarks></remarks>
        protected virtual DataSet GetDataSet(String commandText, SqlParameter[] sqlParameter)
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
                            mSqlCommand.CommandType = CommandType.StoredProcedure;
                            foreach (SqlParameter mSqlParameter in sqlParameter)
                            {
                                mSqlCommand.Parameters.Add(mSqlParameter);
                            }
                        }
                    }
                    using SqlDataAdapter mSqlDataAdapter = new(mSqlCommand);
                    mRetVal = new DataSet();
                    mSqlDataAdapter.Fill(mRetVal);
                }
            }
            return mRetVal;
        }

        /// <summary>
        /// Returns a DataSet given the stored procedure
        /// </summary>
        /// <param name="commandText">String</param>
        /// <returns>DataSet</returns>
        /// <remarks>Containts no logic</remarks>
        protected virtual DataSet GetDataSet(String commandText)
        {
            return this.GetDataSet(commandText, null);
        }

        /// <summary>
        /// Returns a DataTable given the command text, Sql Connection and sql parameters
        /// </summary>
        /// <param name="commandText">String</param>
        /// <param name="sqlParameter">SqlParameter[]</param>
        /// <returns>DataTable</returns>
        /// <remarks></remarks>
        protected virtual DataTable GetDataTable(String commandText, SqlParameter[] sqlParameter)
        {
            DataTable mDataTable = this.GetDataSet(commandText, sqlParameter).Tables[0];
            return mDataTable;
        }

        /// <summary>
        /// Returns a DataTable given the command text and Sql Connection
        /// </summary>
        /// <param name="commandText">String</param>
        /// <returns>DataTable</returns>
        /// <remarks>Containts no logic</remarks>
        protected virtual DataTable GetDataTable(String commandText)
        {
            return this.GetDataTable(commandText, null);
        }

        /// <summary>
        /// Returns a DataRow given the command text and sql parameters
        /// </summary>
        /// <param name="commandText">String</param>
        /// <param name="sqlParameter">SqlParameter[]</param>
        /// <returns>DataRow</returns>
        /// <remarks></remarks>
        protected virtual DataRow GetDataRow(String commandText, SqlParameter[] parameters)
        {
            this.IsValid();
            return this.GetDataTable(commandText, parameters).Rows[0];
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
                throw new ArgumentNullException("parameters", "parameters cannot be a null reference (Nothing in Visual Basic)!");
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
}
