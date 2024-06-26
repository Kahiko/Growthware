using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using System.Text.RegularExpressions;

namespace GrowthWare.DataAccess.Oracle;

public class DDatabaseManager : AbstractDBInteraction, IDatabaseManager
{
    public string DatabaseName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
        throw new NotImplementedException();
    }

    public string GetScriptPath(string theDirection)
    {
        throw new NotImplementedException();
    }

    public Version GetVersion()
    {
        throw new NotImplementedException();
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