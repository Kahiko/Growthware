using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.Oracle.Base;
using GrowthWare.Framework;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GrowthWare.DataAccess.Oracle;

public class DDatabaseManager : AbstractDBInteraction, IDatabaseManager
{
    internal string m_ConnectionWithoutDatabaseName = string.Empty;
    private string m_DatabaseName = string.Empty;

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
        /**
         * A database can not be created outside of the installation process in Oracle.
         * So, we only need to check if we can connect to the database.
         * If we can, return true if not throw an exception there is nothing to do.
         */
    }

    public void Delete()
    {
        throw new NotImplementedException();
    }

    public bool ExecuteScriptFile(string scriptWithPath)
    {
        using OracleConnection mSqlConnection = new(this.ConnectionString);
        return this.replace_N_Run(scriptWithPath, mSqlConnection);
    }

    private bool ExecuteScriptFile(string scriptWithPath, OracleConnection OracleConnection)
    {
        throw new NotImplementedException();
    }

    public bool Exists()
    {
        this.IsValid();
        bool mRetVal = false;
        using (OracleConnection mOracleConnection = new OracleConnection(this.ConnectionString))
        {
            mOracleConnection.Open();
            string mSqlStatement = "SELECT COUNT(*) FROM dba_users WHERE username = :username";
            try
            {
                int mUserCount = Convert.ToInt32(this.ExecuteScalar(mSqlStatement));
                if (mUserCount > 0)
                {
                    mRetVal = true;
                }
            }
            catch (System.Exception)
            {
                mRetVal = false;
            }
        }
        return mRetVal;
    }

    public string GetScriptPath(string theDirection)
    {
        throw new NotImplementedException();
    }

    public Version GetVersion()
    {
        throw new NotImplementedException();
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
            if(isUpgrade)
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

    private bool replace_N_Run(string scriptFile, OracleConnection sqlConnection)
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

    public void UpdateLogPath()
    {
        throw new NotImplementedException();
    }
}