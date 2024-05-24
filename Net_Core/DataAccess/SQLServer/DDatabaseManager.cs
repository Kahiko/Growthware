using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace GrowthWare.DataAccess.SQLServer
{
    public class DDatabaseManager : AbstractDBInteraction, IDatabaseManager
    {
        private string m_DatabaseName = string.Empty;
        private string m_ScriptPath = string.Empty;

        public string DatabaseName
        {
            get { return this.m_DatabaseName; }
            set { this.m_DatabaseName = value; }
        }

        public void Create()
        {
            this.IsValid();
            string mCommandText = string.Empty;
            string mScriptDirectory = this.GetScriptPath("Upgrade");
            using (SqlConnection mSqlConnection = new(this.ConnectionString))
            {
                mSqlConnection.Open();

                // Define the DDL scirpt
                string mCreationFile = mScriptDirectory + "Version_0.0.0.0.sql";

                // Define the DML scirpt
                string mInsertFile = mScriptDirectory + "Version_1.0.0.0.sql";

                Boolean mSuccess = this.replace_N_Run(mCreationFile, mSqlConnection);
                if (!mSuccess)
                {
                    string mError = "Was not able to create the '{0}' database file name {1}.";
                    mError = String.Format(mError, this.DatabaseName, mCreationFile);
                    throw new Exception(mError);
                }
                mSuccess = this.replace_N_Run(mInsertFile, mSqlConnection);
                if (!mSuccess)
                {
                    string mError = "Was not able to insert the data in into '{0}'.";
                    mError = String.Format(mError, mCreationFile);
                    throw new Exception(mError);
                }

                // Set db context to the database name given
                mCommandText = "USE [{0}]";
                mCommandText = String.Format(mCommandText, this.DatabaseName);
                using SqlCommand mSqlCommand = new(mCommandText, mSqlConnection);
                mSqlCommand.CommandType = CommandType.Text;
                mSqlCommand.ExecuteNonQuery();
                // Encrypt the password
                CryptoUtility.TryEncrypt("none", out string mEncryptedPassword, ConfigSettings.EncryptionType, ConfigSettings.EncryptionSaltExpression);
                // Update all of the just added security entites to the configured data access layer information
                mSqlCommand.CommandText = @"SELECT [SecurityEntitySeqId] FROM [ZGWSecurity].[Security_Entities];";
                using SqlDataAdapter mSqlDataAdapter = new(mSqlCommand);
                if (mSqlDataAdapter != null)
                {
                    DataSet mDataSet = new DataSet();
                    mSqlDataAdapter.Fill(mDataSet);
                    if(mDataSet != null && mDataSet.Tables != null && mDataSet.Tables.Count > 0)
                    {
                        DataTable mDataTable = mDataSet.Tables[0];
                        if(mDataTable != null && mDataTable.Rows != null && mDataTable.Rows.Count > 0)
                        {
                            foreach(DataRow mRow in mDataTable.Rows)
                            {
                                // Encrypt the connection string
                                CryptoUtility.TryEncrypt(ConfigSettings.ConnectionString, out string mEncryptedConnectionString, ConfigSettings.EncryptionType, ConfigSettings.EncryptionSaltExpression);
                                mCommandText = @"
                                UPDATE [ZGWSecurity].[Security_Entities] SET 
                                    [DAL] = N'{0}'
                                    , [DAL_Name] = N'{1}'
                                    , [DAL_Name_Space] = N'{2}'
                                    , [DAL_String] = N'{3}'
                                WHERE [SecurityEntitySeqId] = {4};";
                                mCommandText = String.Format(
                                    mCommandText,
                                    "SQLServer",
                                    ConfigSettings.DataAccessLayerAssemblyName,
                                    ConfigSettings.DataAccessLayerNamespace,
                                    mEncryptedConnectionString,
                                    mRow["SecurityEntitySeqId"].ToString()
                                );
                                mSqlCommand.CommandText = mCommandText;
                                mSqlCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
                mSqlDataAdapter.Dispose();
                mCommandText = @"
                UPDATE [ZGWSecurity].[Accounts] SET [Password] = '{0}';";
                mCommandText = String.Format(mCommandText, mEncryptedPassword);
                mSqlCommand.CommandText = mCommandText;
                mSqlCommand.ExecuteNonQuery();
            }
        }

        public void Delete()
        {
            this.IsValid();
            string mCommandText = string.Empty;
            using (SqlConnection mSqlConnection = new(this.ConnectionString))
            {
                string mCreationFile = string.Empty;
                string mCurrentDirectory = this.GetScriptPath("Downgrade");
                mCreationFile = mCurrentDirectory + "Version_1.0.0.0.sql";
                mCreationFile = mCreationFile.Replace(@"\", @"/");
                mCreationFile = mCreationFile.Replace(@"/", Path.DirectorySeparatorChar.ToString());
                string mCreationText = File.ReadAllText(mCreationFile);
                mCreationText = mCreationText.Replace("YourDatabaseName", DatabaseName);
                File.WriteAllText(mCreationFile, mCreationText);
                mSqlConnection.Open();
                // Create the database 
                bool mSuccess = this.ExecuteScriptFile(mCreationFile, mSqlConnection);
                if (!mSuccess)
                {
                    string mError = "Was not able to create the database using {0}";
                    mError = String.Format(mError, mCreationFile);
                    throw new Exception(mError);
                }
                mCreationText = File.ReadAllText(mCreationFile);
                mCreationText = mCreationText.Replace(DatabaseName, "YourDatabaseName");
                File.WriteAllText(mCreationFile, mCreationText);
            }
        }

        public bool Exists()
        {
            this.IsValid();
            bool mRetVal = true;
            string mSqlStatement = "SELECT [name] FROM [master].[sys].[databases] WHERE [name] = N'{0}'";
            mSqlStatement = String.Format(mSqlStatement, this.m_DatabaseName);
            try
            {
                DataRow mDataRow = this.GetDataRow(mSqlStatement);
                if (mDataRow != null)
                {
                    if (mDataRow["name"] == null || mDataRow["name"].ToString().Length == 0)
                    {
                        mRetVal = false;
                    }
                }
                else
                {
                    mRetVal = false;
                }
            }
            catch (System.Exception)
            {
                mRetVal = false;
            }
            return mRetVal;
        }

        private bool replace_N_Run(string scriptFile, SqlConnection sqlConnection)
        {
            bool mSuccess = false;
            // Replace 'YourDatabaseName' with the given database name
            string mAllText = File.ReadAllText(scriptFile);
            mAllText = mAllText.Replace("YourDatabaseName", DatabaseName);
            File.WriteAllText(scriptFile, mAllText);
            try
            {
                // Create the database 
                mSuccess = this.ExecuteScriptFile(scriptFile, sqlConnection);
            }
            catch (System.Exception)
            {
                // We do not want to throw anything that will be done by caller
            }
            finally
            {
                // Replace the given database name with 'YourDatabaseName'
                mAllText = mAllText.Replace(DatabaseName, "YourDatabaseName");
                File.WriteAllText(scriptFile, mAllText);
            }
            return mSuccess;
        }

        public bool ExecuteScriptFile(string scriptWithPath)
        {
            using (SqlConnection mSqlConnection = new(this.ConnectionString))
            {
                return this.ExecuteScriptFile(scriptWithPath, mSqlConnection);
            }
        }

        public bool ExecuteScriptFile(string scriptWithPath, SqlConnection sqlConnection)
        {
            this.IsValid();
            try
            {
                string mAllText = File.ReadAllText(scriptWithPath);

                // split script on GO command
                IEnumerable<string> mCommands = Regex.Split(
                    mAllText,
                    @"^\s*GO\s*$",
                    RegexOptions.Multiline | RegexOptions.IgnoreCase
                );
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                foreach (string mCommandText in mCommands)
                {
                    if (!string.IsNullOrEmpty(mCommandText) && !string.IsNullOrWhiteSpace(mCommandText))
                    {
                        try
                        {
                            using SqlCommand mSqlCommand = new(mCommandText, sqlConnection);
                            mSqlCommand.CommandType = CommandType.Text;
                            mSqlCommand.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            string spError = mCommandText.Length > 100 ? mCommandText.Substring(0, 100) + " ...\n..." : mCommandText;
                            Console.WriteLine(string.Format("Please check the SqlServer script.\nFile: {0} \nLine: {1} \nError: {2} \nSQL Command: \n{3}", scriptWithPath, ex.LineNumber, ex.Message, spError));
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public string GetScriptPath(string theDirection)
        {
            if (m_ScriptPath == string.Empty)
            {
                string mCurrentDirectory = Directory.GetCurrentDirectory();
                if (!mCurrentDirectory.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    mCurrentDirectory += Path.DirectorySeparatorChar.ToString();
                }
                mCurrentDirectory += "Scripts/TheDirection/" + ConfigSettings.DataAccessLayer + Path.DirectorySeparatorChar;
                mCurrentDirectory = mCurrentDirectory.Replace(@"/", @"\");
                mCurrentDirectory = mCurrentDirectory.Replace(@"\", Path.DirectorySeparatorChar.ToString());
                this.m_ScriptPath = mCurrentDirectory;
            }
            return m_ScriptPath.Replace("TheDirection", theDirection);
        }

        public Version GetVersion()
        {
            this.IsValid();
            Version mRetVal = new Version("0.0.0.0");
            if (this.Exists())
            {
                string mCommandText = "SELECT [Version] FROM [ZGWSystem].[Database_Information]";
                DataRow mDataRow = this.GetDataRow(mCommandText);
                mRetVal = new Version(mDataRow["Version"].ToString());
            }
            return mRetVal;
        }

        protected override void IsValid()
        {
            base.IsValid();
            if (String.IsNullOrEmpty(this.m_DatabaseName) | String.IsNullOrWhiteSpace(this.m_DatabaseName))
            {
                throw new DataAccessLayerException("The DatabaseName property cannot be null or blank!");
            }
        }

        public void UpdateLogPath()
        {
            string mCommandText = String.Format("UPDATE [ZGWOptional].[Directories] SET [Directory] = '{0}' WHERE [FunctionSeqId] = (SELECT [FunctionSeqId] FROM [ZGWSecurity].[Functions] WHERE [Action] = 'Manage_Logs')", ConfigSettings.LogPath);
            this.ExecuteNonQuery(mCommandText);
        }
    }
}
