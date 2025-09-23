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
using System.Text;
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
        string mDatabaseNameUpper = mDatabaseName.ToUpper();
        string mDatabaseNameLower = mDatabaseName.ToLower();
        
        try
        {
            // 1. Create a common user in the CDB with SYSDBA
            string commonUser = $"c##{mDatabaseNameLower}";
            mCommandText = $@"
                DECLARE
                    user_exists NUMBER;
                    user_count NUMBER;
                BEGIN
                    -- Check if the user exists in any container (case-insensitive)
                    SELECT COUNT(*) INTO user_count 
                    FROM dba_users 
                    WHERE UPPER(username) = UPPER('{commonUser}');
                    
                    -- If user exists, drop it first
                    IF user_count > 0 THEN
                        BEGIN
                            EXECUTE IMMEDIATE 'DROP USER {commonUser} CASCADE';
                            DBMS_OUTPUT.PUT_LINE('Dropped existing user {commonUser}');
                        EXCEPTION
                            WHEN OTHERS THEN
                                DBMS_OUTPUT.PUT_LINE('Error dropping user: ' || SQLERRM);
                                RAISE;
                        END;
                    END IF;
                    
                    -- Create the user
                    EXECUTE IMMEDIATE 'CREATE USER {commonUser} IDENTIFIED BY ""{mDatabasePassword}"" CONTAINER=ALL';
                    EXECUTE IMMEDIATE 'GRANT SYSDBA TO {commonUser} CONTAINER=ALL';
                    DBMS_OUTPUT.PUT_LINE('Created common user {commonUser}');
                END;";

            this.ExecuteNonQuery(mCommandText);
            this.m_Logger.Info($"Created/verified common user {commonUser}");

            // 2. Create the pluggable database
            mCommandText = $@"
                DECLARE
                    pdb_exists NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO pdb_exists 
                    FROM v$pdbs 
                    WHERE name = '{mDatabaseName}';
                    
                    IF pdb_exists = 0 THEN
                        EXECUTE IMMEDIATE '
                            CREATE PLUGGABLE DATABASE {mDatabaseName}
                            ADMIN USER {mDatabaseNameLower} IDENTIFIED BY ""{mDatabasePassword}""
                            ROLES = (dba)
                            DEFAULT TABLESPACE {mDatabaseNameUpper}_USERS
                            DATAFILE ''{mDataFile}'' SIZE 250M AUTOEXTEND ON
                            FILE_NAME_CONVERT = (''{mSeedFileLocation}'', ''{mDataPath}'')
                            STORAGE (MAXSIZE 1G)
                            PATH_PREFIX = ''{mDataPath}''';
                        DBMS_OUTPUT.PUT_LINE('Created PDB {mDatabaseName}');
                    ELSE
                        DBMS_OUTPUT.PUT_LINE('PDB {mDatabaseName} already exists');
                    END IF;
                END;";
                
            this.ExecuteNonQuery(mCommandText);
            this.m_Logger.Info($"Created/verified PDB {mDatabaseName}");
            // Open the pluggable database
            mCommandText = $"ALTER PLUGGABLE DATABASE {mDatabaseName} OPEN READ WRITE";
            this.ExecuteNonQuery(mCommandText);
            this.m_Logger.Info($"Opened pluggable database {mDatabaseName}");
            string[] sysCommands = {
                "ALTER profile \"DEFAULT\" limit password_life_time unlimited",
                "ALTER SYSTEM SET processes=300 SCOPE=spfile",
                "ALTER SYSTEM SET sessions=335 SCOPE=spfile",
                "ALTER SYSTEM SET transactions=400 SCOPE=spfile",
                "ALTER SYSTEM SET open_cursors=300 SCOPE=both",
                "ALTER SYSTEM SET cursor_sharing='FORCE' SCOPE=both"
            };

            foreach (var cmd in sysCommands)
            {
                try
                {
                    this.ExecuteNonQuery(cmd);
                    // this.m_Logger.Debug($"Executed: {cmd}");
                }
                catch (Exception ex)
                {
                    this.m_Logger.Warn($"Could not execute '{cmd}': {ex.Message}");
                }
            }            

            // Switch to the pluggable database
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
                    string mError = "Database '{0}' database was created but could not excute file name {1}.";
                    mError = String.Format(mError, this.DatabaseName, mCreationFile);
                    this.m_Logger.Error(mError);
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
            }

        }
        catch (Exception ex)
        {
            this.m_Logger.Error($"Error during database creation: {ex.Message}");
            throw;
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
        string mError = "Was not able to delete the database using {0}";
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

    public bool ExecuteScriptFile(string scriptWithPath, OracleConnection oracleConnection)
    {
        ArgumentNullException.ThrowIfNull(oracleConnection);
        if (string.IsNullOrWhiteSpace(scriptWithPath) || !File.Exists(scriptWithPath))
        {
            throw new FileNotFoundException("Script file not found", scriptWithPath);
        }

        string mScriptContent = File.ReadAllText(scriptWithPath);
        string[] mForwardSlashCommands = splitIntoForwardSlashCommands(mScriptContent);

        this.m_Logger.Debug($"Parsing script file: {scriptWithPath}");
        this.m_Logger.Debug($"Number of forward slash commands found: {mForwardSlashCommands.Length}");
        if (oracleConnection.State != ConnectionState.Open)
        {
            oracleConnection.Open();
        }
        
        using (var oracleCommand = oracleConnection.CreateCommand())
        {
            foreach (string command in mForwardSlashCommands)
            {
                if (string.IsNullOrWhiteSpace(command)) continue;

                string mProcessedCommand = processCommandText(command.Trim());
                if (string.IsNullOrWhiteSpace(mProcessedCommand)) continue;

                try
                {
                    // Handle transaction control statements first
                    if (mProcessedCommand.TrimStart().StartsWith("COMMIT", StringComparison.OrdinalIgnoreCase) ||
                        mProcessedCommand.TrimStart().StartsWith("ROLLBACK", StringComparison.OrdinalIgnoreCase))
                    {
                        // Remove trailing semicolon if present
                        mProcessedCommand = mProcessedCommand.TrimEnd();
                        if (mProcessedCommand.EndsWith(";"))
                        {
                            mProcessedCommand = mProcessedCommand.Substring(0, mProcessedCommand.Length - 1).Trim();
                        }
                        
                        // Execute the transaction control statement directly
                        oracleCommand.CommandText = mProcessedCommand;
                        oracleCommand.ExecuteNonQuery();
                        continue; // Skip the rest of the loop
                    }

                    // Only wrap if it's a standalone DDL statement (not already in a BEGIN/END block)
                    if ((mProcessedCommand.TrimStart().StartsWith("CREATE ", StringComparison.OrdinalIgnoreCase) ||
                         mProcessedCommand.TrimStart().StartsWith("ALTER ", StringComparison.OrdinalIgnoreCase) ||
                         mProcessedCommand.TrimStart().StartsWith("DROP ", StringComparison.OrdinalIgnoreCase) ||
                         mProcessedCommand.TrimStart().StartsWith("GRANT ", StringComparison.OrdinalIgnoreCase)) &&
                        !mProcessedCommand.TrimStart().StartsWith("BEGIN", StringComparison.OrdinalIgnoreCase) &&
                        !mProcessedCommand.TrimStart().StartsWith("DECLARE", StringComparison.OrdinalIgnoreCase) &&
                        !mProcessedCommand.TrimStart().StartsWith("EXECUTE ", StringComparison.OrdinalIgnoreCase))
                    {
                        // For CREATE OR REPLACE PROCEDURE, don't wrap in EXECUTE IMMEDIATE
                        if (mProcessedCommand.TrimStart().StartsWith("CREATE OR REPLACE PROCEDURE", StringComparison.OrdinalIgnoreCase))
                        {
                            // Just remove trailing semicolon if present and use as is
                            mProcessedCommand = mProcessedCommand.TrimEnd();
                            if (mProcessedCommand.EndsWith(";"))
                            {
                                mProcessedCommand = mProcessedCommand.Substring(0, mProcessedCommand.Length - 1);
                            }
                        }
                        else
                        {
                            // For other DDL and GRANT, use EXECUTE IMMEDIATE with proper escaping
                            mProcessedCommand = mProcessedCommand.TrimEnd();
                            if (mProcessedCommand.EndsWith(";"))
                            {
                                mProcessedCommand = mProcessedCommand.Substring(0, mProcessedCommand.Length - 1);
                            }
                            string singleLineCommand = System.Text.RegularExpressions.Regex.Replace(
                                mProcessedCommand, 
                                @"\s+", 
                                " "
                            ).Trim();
                            string escapedCommand = singleLineCommand.Replace("'", "''");
                            mProcessedCommand = $"BEGIN EXECUTE IMMEDIATE '{escapedCommand}'; END;";
                        }
                    }

                    // this.m_Logger.Debug($"Executing command: {mProcessedCommand}");
                    oracleCommand.CommandText = mProcessedCommand;
                    oracleCommand.CommandTimeout = 300; // 5 minutes
                    oracleCommand.ExecuteNonQuery();
                }
                catch (OracleException ex)
                {
                    this.m_Logger.Error("Connection String: " + oracleConnection.ConnectionString);
                    this.m_Logger.Error("scriptWithPath: " + scriptWithPath);
                    this.m_Logger.Error($"Executing command: {mProcessedCommand}");
                    this.m_Logger.Error($"Oracle Error: {ex.Message}");
                    this.m_Logger.Error($"Error Code: {ex.Number}");
                    this.m_Logger.Error(" --- Stack Trace ---");
                    this.m_Logger.Error(ex.StackTrace);
                    throw new Exception($"Error executing command: {mProcessedCommand}", ex);
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Splits the given script content into individual commands by splitting on forward slashes
    /// that are on their own line.
    /// </summary>
    /// <param name="scriptContent">The script content to split.</param>
    /// <returns>An array of string containing the individual commands.</returns>
    private static string[] splitIntoForwardSlashCommands(string scriptContent)
    {
        // Normalize line endings and remove BOM
        scriptContent = scriptContent.Replace("\r\n", "\n")
                                    .Replace("\r", "\n")
                                    .TrimStart('\uFEFF');
        
        // Split on forward slashes that are on their own line
        return scriptContent.Split(new[] { "\n/", "\r\n/" }, StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    /// Processes the given command text by skipping empty lines, line comments and block comments.
    /// </summary>
    /// <param name="commandText">The command text to process.</param>
    /// <returns>The processed command text.</returns>
    private static string processCommandText(string commandText)
    {
        var mStringBuilder = new StringBuilder();
        bool mIsInBlockComment = false;
        
        string[] mLines = commandText.Split('\n');
        
        foreach (string line in mLines)
        {
            string mTrimmedLine = line.Trim();

            // Skip empty lines
            if (string.IsNullOrWhiteSpace(mTrimmedLine))
            { 
                continue;
            }
                
            // Handle block comments
            if (mIsInBlockComment)
            {
                int mEndComment = mTrimmedLine.IndexOf("*/");
                if (mEndComment >= 0)
                {
                    mIsInBlockComment = false;
                    // Add any content after the comment ends
                    string mRemaining = mTrimmedLine.Substring(mEndComment + 2).Trim();
                    if (!string.IsNullOrEmpty(mRemaining))
                    {
                        mStringBuilder.AppendLine(mRemaining);
                    }
                }
                continue;
            }
            
            // Check for start of block comment
            int mStartComment = mTrimmedLine.IndexOf("/*");
            if (mStartComment >= 0)
            {
                mIsInBlockComment = true;
                // Add any content before the comment starts
                string mBeforeComment = mTrimmedLine.Substring(0, mStartComment).Trim();
                if (!string.IsNullOrEmpty(mBeforeComment))
                {
                    mStringBuilder.AppendLine(mBeforeComment);
                }
                continue;
            }

            // Skip line comments
            if (mTrimmedLine.StartsWith("--") || mTrimmedLine.StartsWith("//"))
            { 
                continue;
            }
            
            // Add the line to the result
            mStringBuilder.AppendLine(mTrimmedLine);
        }
        
        return mStringBuilder.ToString().Trim();
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
        mAllText = mAllText.Replace("YourDatabaseName", this.DatabaseName);
        mAllText = mAllText.Replace("YourPasswordHere", this.getPassword());
        mAllText = mAllText.Replace("YourUpperDatabaseName_users", this.DatabaseName.ToUpper() + "_users");
        File.WriteAllText(scriptFile, mAllText);
        try
        {
            // Create the database 
            mSuccess = this.ExecuteScriptFile(scriptFile, sqlConnection);
        }
        catch (System.Exception)
        {
            throw;
        }
        finally
        {
            // Replace the given database name with 'YourDatabaseName'
            mAllText = mAllText.Replace(this.DatabaseName.ToUpper() + "_users", "YourUpperDatabaseName_users");
            mAllText = mAllText.Replace(this.DatabaseName, "YourDatabaseName");
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