using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.Oracle.Base;
using GrowthWare.Framework;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GrowthWare.DataAccess.Oracle;

public class DDatabaseManager : AbstractDBInteraction, IDatabaseManager
{
    private string m_DatabaseName = string.Empty;
    private string m_ScriptPath = string.Empty;

    public string DatabaseName
    {
        get
        {
            return this.m_DatabaseName;
        }
        set
        {
            this.m_DatabaseName = value;
        }
    }

    public void Create()
    {
        string mCommandText = string.Empty;
        this.ConnectionString = ConfigSettings.ContainerConnectionString;

        // Get the file location for the new PDB
        string mDatabaseName = ConfigSettings.DataAccessLayerDatabaseName;
        string mDatabasePassword = getPassword();
        string mFileLocationQuery = "SELECT file_name FROM dba_data_files WHERE tablespace_name = 'SYSTEM'";
        string mSeedFileLocation = this.ExecuteScalar(mFileLocationQuery).ToString().Replace("system01.dbf", "", StringComparison.OrdinalIgnoreCase).TrimEnd(Path.DirectorySeparatorChar);
        mSeedFileLocation = mSeedFileLocation.Replace(@"\", @"/");
        mSeedFileLocation = mSeedFileLocation.Replace(@"/", Path.DirectorySeparatorChar.ToString());
        // Construct the datafile path
        string mDataPath = $@"{mSeedFileLocation}\{ConfigSettings.DataAccessLayerDatabaseName}";
        string mDataFile = $@"{mDataPath}\{mDatabaseName}_users01.dbf";
        string mUpper = mDatabaseName.ToUpper();
        string mLower = mDatabaseName.ToLower();
        // Create the pluggable database
        mCommandText = $@"CREATE PLUGGABLE DATABASE {mDatabaseName}
  ADMIN USER {mLower} IDENTIFIED BY ""{mDatabasePassword}""
  ROLES = (dba)
  DEFAULT TABLESPACE {mUpper}_USERS
    DATAFILE '{mDataFile}' SIZE 250M AUTOEXTEND ON
  FILE_NAME_CONVERT = ('{mSeedFileLocation + Path.DirectorySeparatorChar}',
                       '{mDataPath + Path.DirectorySeparatorChar}')
  STORAGE (MAXSIZE 1G)
  PATH_PREFIX = '{mDataPath + Path.DirectorySeparatorChar}'";
        try
        {
            this.ExecuteNonQuery(mCommandText);
            // Open the pluggable database
            string mOpenPdbQuery = $"ALTER PLUGGABLE DATABASE {mUpper} OPEN READ WRITE";
            this.ExecuteNonQuery(mOpenPdbQuery);
        }
        catch (OracleException ex)
        {
            Logger.Instance().Error(ex);
            throw;
        }
        this.ConnectionString = ConfigSettings.ConnectionString;
        string mScriptDirectory = this.GetScriptPath("Upgrade");
        // Define the DDL scirpt
        string mCreationFile = mScriptDirectory + "Version_0.0.0.0.sql";

        // Define the DML scirpt
        string mInsertFile = mScriptDirectory + "Version_1.0.0.0.sql";
        using (OracleConnection mOracleConnection = new(this.ConnectionString))
        {
            mOracleConnection.Open();
            // Boolean mSuccess = this.ExecuteScriptFile(mCreationFile, mOracleConnection);
            Boolean mSuccess = this.replace_N_Run(mCreationFile, mOracleConnection);
            if (!mSuccess)
            {
                string mError = "Was not able to create the '{0}' database file name {1}.";
                mError = String.Format(mError, this.DatabaseName, mCreationFile);
                throw new Exception(mError);
            }
            // mSuccess = this.ExecuteScriptFile(mInsertFile, mOracleConnection);
            mSuccess = this.replace_N_Run(mInsertFile, mOracleConnection);
            if (!mSuccess)
            {
                string mError = "Was not able to insert the data in into '{0}'.";
                mError = String.Format(mError, mCreationFile);
                throw new Exception(mError);
            }

            // // Update all of the just added security entites to the configured data access layer information
            // using OracleCommand mOracleCommand = new(mCommandText, mOracleConnection);
            // mOracleCommand.CommandText = @"SELECT [SecurityEntitySeqId] FROM [ZGWSecurity].[Security_Entities];";
            // using (OracleDataAdapter mOracleDataAdapter = new OracleDataAdapter(mOracleCommand))
            // {
            //     DataSet mDataSet = new DataSet();
            //     mOracleDataAdapter.Fill(mDataSet);
            //     if(mDataSet != null && mDataSet.Tables != null && mDataSet.Tables.Count > 0)
            //     {
            //         DataTable mDataTable = mDataSet.Tables[0];
            //         if(mDataTable != null && mDataTable.Rows != null && mDataTable.Rows.Count > 0)
            //         {
            //             foreach(DataRow mRow in mDataTable.Rows)
            //             {
            //                 // Encrypt the connection string
            //                 CryptoUtility.TryEncrypt(ConfigSettings.ConnectionString, out string mEncryptedConnectionString, ConfigSettings.EncryptionType, ConfigSettings.EncryptionSaltExpression);
            //                 mCommandText = @"
            //                 UPDATE [ZGWSecurity].[Security_Entities] SET 
            //                     [DAL] = N'{0}'
            //                     , [DAL_Name] = N'{1}'
            //                     , [DAL_Name_Space] = N'{2}'
            //                     , [DAL_String] = N'{3}'
            //                 WHERE [SecurityEntitySeqId] = {4};";
            //                 mCommandText = String.Format(
            //                     mCommandText,
            //                     "SQLServer",
            //                     ConfigSettings.DataAccessLayerAssemblyName,
            //                     ConfigSettings.DataAccessLayerNamespace,
            //                     mEncryptedConnectionString,
            //                     mRow["SecurityEntitySeqId"].ToString()
            //                 );
            //                 mOracleCommand.CommandText = mCommandText;
            //                 mOracleCommand.ExecuteNonQuery();
            //             }
            //         }
            //     }
            // }

            // mOracleCommand.CommandText = @"SELECT [AccountSeqId] FROM [ZGWSecurity].[Accounts];";
            // using (OracleDataAdapter mOracleDataAdapter = new OracleDataAdapter(mOracleCommand))
            // {
            //     DataSet mDataSet = new DataSet();
            //     mOracleDataAdapter.Fill(mDataSet);
            //     if(mDataSet != null && mDataSet.Tables != null && mDataSet.Tables.Count > 0)
            //     {
            //         DataTable mDataTable = mDataSet.Tables[0];
            //         if(mDataTable != null && mDataTable.Rows != null && mDataTable.Rows.Count > 0)
            //         {
            //             foreach(DataRow mRow in mDataTable.Rows)
            //             {
            //                 // Encrypt the password
            //                 CryptoUtility.TryEncrypt("none", out string mEncryptedPassword, ConfigSettings.EncryptionType, ConfigSettings.EncryptionSaltExpression);
            //                 mCommandText = @"
            //                 UPDATE [ZGWSecurity].[Accounts] SET 
            //                     [Password] = '{0}'
            //                 WHERE [AccountSeqId] = {1};";
            //                 mCommandText = String.Format(
            //                     mCommandText,
            //                     mEncryptedPassword,
            //                     mRow["AccountSeqId"].ToString()
            //                 );
            //                 mOracleCommand.CommandText = mCommandText;
            //                 mOracleCommand.ExecuteNonQuery();
            //             }
            //         }
            //     }
            // }
        }
    }

    public void Delete()
    {
        string mCommandText = string.Empty;
        string mOrigConnectionString = this.ConnectionString;
        this.ConnectionString = this.removeProperty(ConfigSettings.ConnectionString, "Database");
        string mVersionOneFile = string.Empty;
        string mCurrentDirectory = this.GetScriptPath("Downgrade");
        mVersionOneFile = mCurrentDirectory + "Version_1.0.0.0.sql";
        mVersionOneFile = mVersionOneFile.Replace(@"\", @"/");
        mVersionOneFile = mVersionOneFile.Replace(@"/", Path.DirectorySeparatorChar.ToString());
        string mError = "Was not able to create the database using {0}";
        mError = String.Format(mError, mVersionOneFile);
        string mVersionOneText = File.ReadAllText(mVersionOneFile);
        using (OracleConnection mOracleConnection = new(this.ConnectionString))
        {
            try
            {
                mVersionOneText = mVersionOneText.Replace("YourDatabaseName", DatabaseName);
                File.WriteAllText(mVersionOneFile, mVersionOneText);
                mOracleConnection.Open();
                // Delete the database
                bool mSuccess = this.replace_N_Run(mVersionOneFile, mOracleConnection);
                if (!mSuccess)
                {
                    throw new Exception(mError);
                }                
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(mError);
                DataAccessLayerException mException = new(mError, ex);
                base.m_Logger.Error(mException);
                throw;
            }
            finally
            {
                mVersionOneText = File.ReadAllText(mVersionOneFile);
                mVersionOneText = mVersionOneText.Replace(DatabaseName, "YourDatabaseName");
                File.WriteAllText(mVersionOneFile, mVersionOneText);
                this.ConnectionString = mOrigConnectionString;
            }
        }
    }

    public bool ExecuteScriptFile(string scriptWithPath)
    {
        using OracleConnection mSqlConnection = new(this.ConnectionString);
        return this.ExecuteScriptFile(scriptWithPath, mSqlConnection);
    }

    private bool ExecuteScriptFile(string scriptWithPath, OracleConnection oracleConnection)
    {
        this.IsValid();
        try
        {
            string mAllText = File.ReadAllText(scriptWithPath);
            // split script on semi-colon
            IEnumerable<string> mCommands = mAllText.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            if (oracleConnection.State == ConnectionState.Closed)
            {
                oracleConnection.Open();
            }
            foreach (string mCommandText in mCommands)
            {
                if (!string.IsNullOrEmpty(mCommandText) && !string.IsNullOrWhiteSpace(mCommandText) && !mCommandText.Trim().StartsWith("--"))
                {
                    try
                    {
                        using OracleCommand mOracleCommand = new(mCommandText, oracleConnection);
                        mOracleCommand.CommandType = CommandType.Text;
                        mOracleCommand.ExecuteNonQuery();
                    }
                    catch (OracleException ex)
                    {
                        string spError = mCommandText.Length > 100 ? mCommandText.Substring(0, 100) + " ...\n..." : mCommandText;
                        string mMsg = string.Format("Please check the Oracle script.\nFile: {0} \nError: {1} \nSQL Command: \n{3}", scriptWithPath, ex.Message, spError, mCommandText);
                        Console.WriteLine(mMsg);
                        Logger.Instance().Error(mMsg);
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

    public bool Exists()
    {
        this.ConnectionString = ConfigSettings.ContainerConnectionString;
        bool mRetVal = false;
        // string mSqlStatement = string.Format("SELECT COUNT(*) FROM v$pdbs vp WHERE vp.name = '{0}'", ConfigSettings.DataAccessLayerDatabaseName.ToUpper());
        string mSqlStatement = "SELECT COUNT(*) FROM v$pdbs vp WHERE vp.name = :databaseName";
        try
        {
            OracleParameter[] mOracleParameters = {
                new("databaseName", ConfigSettings.DataAccessLayerDatabaseName.ToUpper())
            };

            int mUserCount = Convert.ToInt32(this.ExecuteScalar(mSqlStatement, mOracleParameters, true));
            if (mUserCount > 0)
            {
                mRetVal = true;
            }
        }
        catch (System.Exception ex)
        {
            DataAccessLayerException mException = new("DDatabaseManager.Exists", ex);
            Logger.Instance().Error(mException);
            mRetVal = false;
        }
        return mRetVal;
    }

    /// <summary>
    /// Helper function to pull the passwrod from the connection string
    /// </summary>
    /// <returns></returns>
    private string getPassword()
    {
        string mRetVal = string.Empty;
        string[] mConnectionStringParts = ConfigSettings.ConnectionString.Split(';');
        foreach (string item in mConnectionStringParts)
        {
            string[] mNameValuePair = item.Split('=');
            if (mNameValuePair.Length > 1)
            {
                if(mNameValuePair[0].Equals("password", StringComparison.OrdinalIgnoreCase))
                {
                    mRetVal = mNameValuePair[1];
                    break;
                }
            }
        }
        return mRetVal;
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
                this.ConnectionString = ConfigSettings.ConnectionString;
                string mCommandText = "SELECT Version FROM ZGWSystem.Database_Information";
                DataRow mDataRow = this.GetDataRow(mCommandText);
                mRetVal = new Version(mDataRow["Version"].ToString());
            }
            return mRetVal;
    }

    public void SetDatabaseName()
    {
        this.m_DatabaseName = ConfigSettings.DataAccessLayerDatabaseName;
    }

    /// <summary>
    /// Calls base method to ensure connection string is present then
    /// ensures that the database name is set.
    /// </summary>
    /// <exception cref="DataAccessLayerException"></exception>
    protected override void IsValid()
    {
        base.IsValid();
        if (String.IsNullOrEmpty(this.m_DatabaseName) | String.IsNullOrWhiteSpace(this.m_DatabaseName))
        {
            throw new DataAccessLayerException("The DatabaseName property cannot be null or blank!");
        }
    }

    public void ProcessScriptFiles(bool isUpgrade, Version currentVersion, Version desiredVersion, IEnumerable<Version> availbleVersions)
    {
        IEnumerable<Version> mVersions = null;
        string mTheDirection = isUpgrade ? "Upgrade" : "Downgrade";
        if (isUpgrade)
        {
            mVersions = availbleVersions.Where(version => version > currentVersion && version <= desiredVersion);
        }
        else
        {
            mVersions = availbleVersions.Where(version => version <= currentVersion && version > desiredVersion && version != new Version("1.0.0.0"));
        }
        if (mVersions == null || mVersions.Count() == 0)
        {
            string mMsg = "There are no '{0}' files to execute that match the version. Requested: '{1}', Current: '{2}'";
            Console.WriteLine(string.Format(mMsg, mTheDirection, desiredVersion.ToString(), currentVersion.ToString()));
            return;
        }
        this.ConnectionString = ConfigSettings.ConnectionString;
        foreach (Version item in mVersions)
        {
            string mScriptWithPath = this.GetScriptPath(mTheDirection) + "Version_" + item.ToString() + ".sql";
            string mFileName = "Version_" + item.ToString() + ".sql";
            Stopwatch mScriptWatch = new();
            mScriptWatch.Start();
            this.ExecuteScriptFile(mScriptWithPath);
            Console.WriteLine("Elapsed time: {0} File: '{1}' ", mScriptWatch.Elapsed, mFileName);
        }
    }

    /// <summary>
    /// Removes the given property from the connection string
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    private string removeProperty(string connectionString, string propertyName)
    {
        string mRetVal = string.Empty;
        string[] mParameterParts = null;
        string[] mConnectionStringParts = this.ConnectionString.Split(";");
        for (int i = 0; i < mConnectionStringParts.Length; i++)
        {
            mParameterParts = mConnectionStringParts[i].Split("=");
            if (!mParameterParts[0].Equals(propertyName, StringComparison.InvariantCultureIgnoreCase))
            {
                mRetVal += mParameterParts[0] + "=" + mParameterParts[1] + ";";
            }
        }
        return mRetVal;
    }

    private bool replace_N_Run(string scriptFile, OracleConnection sqlConnection)
    {
        bool mSuccess = false;
        // Replace 'YourDatabaseName' with the given database name
        string mAllText = File.ReadAllText(scriptFile);
        mAllText = mAllText.Replace("YourDatabaseName", DatabaseName.ToUpper());
        mAllText = mAllText.Replace("YourPasswordHere", this.getPassword());
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
            mAllText = mAllText.Replace(DatabaseName.ToUpper(), "YourDatabaseName");
            mAllText = mAllText.Replace(this.getPassword(), "YourPasswordHere");
            File.WriteAllText(scriptFile, mAllText);
        }
        return mSuccess;
    }

    /// <summary>
    /// Updates the [Directory] column in the ZGWOptional.Directories table
    /// for the function Manage_Logs
    /// </summary>
    public void UpdateLogPath()
    {
            // string mCommandText = String.Format("UPDATE ZGWOptional.Directories SET Directory = '{0}' WHERE [FunctionSeqId] = (SELECT [FunctionSeqId] FROM [ZGWSecurity].[Functions] WHERE [Action] = 'Manage_Logs')", ConfigSettings.LogPath);
            // this.ExecuteNonQuery(mCommandText);
    }
}