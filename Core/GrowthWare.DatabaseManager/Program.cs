using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using System;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace GrowthWare.DatabaseManager
{
    internal class Program
    {
        private static Version m_DesiredVersion = null;

        private static string GetArgument(string[] args, string name)
        {
            string mRetVal = null;
            for (int i = 0; i < args.Length; i++)
            {
                string argument = args[i];
                if (argument.ToLower() == name.ToLower() && i < args.Length)
                {
                    mRetVal = args[i + 1];
                    break;
                }
            }
            return mRetVal;
        }

        static void Main(string[] args)
        {
            if (args == null || args.Length == 0) // Check for null array
            {
                ShowHelp();
                return;
            }
            else
            {
                string mHelp = GetArgument(args, "--Help");
                if (mHelp != null)
                {
                    ShowHelp();
                    return;
                }
                mHelp = GetArgument(args, "--H");
                if (mHelp != null)
                {
                    ShowHelp();
                    return;
                }
                m_DesiredVersion = new Version(GetArgument(args, "--Version"));
            }
            string mAssemblyName = ConfigSettings.DataAccessLayerAssemblyName;
            DataTable mAvalibleFiles = null;
            List<Version> mAvalibleVersions = new List<Version>();
            string mConnectionString = ConfigSettings.ConnectionString;
            string[] mConnectionStringParts = mConnectionString.Split(";");
            string mDataBaseName = string.Empty;
            Boolean mDeleteDatabase = false;
            string mNameSpace = ConfigSettings.DataAccessLayerNamespace;
            string mOriginalConnectionString = mConnectionString;
            string[] mParameterParts = null;
            Stopwatch mWatch = new Stopwatch();
            mWatch.Start();

            for (int i = 0; i < mConnectionStringParts.Length; i++)
            {
                mParameterParts = mConnectionStringParts[i].Split("=");
                if (mParameterParts[0].ToLower() == "database")
                {
                    mDataBaseName = mParameterParts[1];
                    break;
                }
            }
            IDatabaseManager mDatabaseManager = (IDatabaseManager)ObjectFactory.Create(mAssemblyName, mNameSpace, "DDatabaseManager");
            mDatabaseManager.DatabaseName = mDataBaseName;
            mConnectionString = mConnectionString.Replace("database=" + mDataBaseName, "");
            mDatabaseManager.ConnectionString = mConnectionString;
            if (m_DesiredVersion == new Version("0.0.0.0"))
            {
                mDeleteDatabase = true;
            }
            if (mDeleteDatabase)
            {
                if (mDatabaseManager.Exists())
                {
                    mDatabaseManager.Delete();
                    Console.WriteLine(String.Format("The '{0}' database has been deleted.", mDataBaseName));
                }
                else
                {
                    Console.WriteLine(String.Format("The '{0}' database does not exist, nothing to delete.", mDataBaseName));
                    mWatch.Stop();
                    Console.WriteLine("Time elapsed as per stopwatch: {0} ", mWatch.Elapsed);
                }
            }
            else
            {
                if (!mDatabaseManager.Exists())
                {
                    Console.WriteLine(String.Format("Attempting to create the '{0}' database.", mDataBaseName));
                    mDatabaseManager.Create();
                    Console.WriteLine(String.Format("The '{0}' database has been created.", mDataBaseName));
                }
                else
                {
                    Console.WriteLine(String.Format("The '{0}' database exists no need to create.", mDataBaseName));
                }
                mDatabaseManager.ConnectionString = mOriginalConnectionString;
                Console.WriteLine("Starting upgrade/downgrade process.");
                Version mCurrentVersion = mDatabaseManager.GetVersion();
                string mUpOrDown = "Upgrade";
                if (m_DesiredVersion == mCurrentVersion)
                {
                    Console.WriteLine("Database version matches requested version no work done.");
                    mWatch.Stop();
                    Console.WriteLine("Time elapsed as per stopwatch: {0} ", mWatch.Elapsed);
                    return;
                }
                if (m_DesiredVersion < mCurrentVersion)
                {
                    mUpOrDown = "Downgrade";
                }
                string mScriptPath = mDatabaseManager.GetScriptPath(mUpOrDown);
                if (mUpOrDown == "Upgrade")
                {
                    Console.WriteLine("Attempting to Upgrade the database.");
                    mAvalibleFiles = FileUtility.GetDirectory(mScriptPath, true, "Name", "ASC");
                }
                else
                {
                    Console.WriteLine("Attempting to Downgrade the database.");
                    mAvalibleFiles = FileUtility.GetDirectory(mScriptPath, true, "Name", "DESC");
                }
                string mVersionString = string.Empty;
                foreach (DataRow mDataRow in mAvalibleFiles.Rows)
                {
                    mVersionString = mDataRow["Name"].ToString().Split("_")[1].Replace(".sql", "").ToString();
                    Version mVersion = new Version(mVersionString);
                    mAvalibleVersions.Add(mVersion);
                }
                if (mUpOrDown == "Upgrade")
                {
                    var mVersions = mAvalibleVersions.Where(version => version > mCurrentVersion && version <= m_DesiredVersion);
                    if (mVersions == null || mVersions.Count() == 0)
                    {
                        string mMsg = "There are no '{0}' files to execute that match the version. Requested: '{1}', Current: '{2}'";
                        Console.WriteLine(string.Format(mMsg, mUpOrDown, m_DesiredVersion.ToString(), mCurrentVersion.ToString()));
                    }
                    foreach (Version item in mVersions)
                    {
                        string mScriptWithPath = mDatabaseManager.GetScriptPath("Upgrade") + "Version_" + item.ToString() + ".sql";
                        string mFileName = "Version_" + item.ToString() + ".sql";
                        Stopwatch mScriptWatch = new Stopwatch();
                        mScriptWatch.Start();
                        mDatabaseManager.ExecuteScriptFile(mScriptWithPath);
                        Console.WriteLine("Elapsed time: {0} File: '{1}' ", mWatch.Elapsed, mFileName);
                    }
                }
                else
                {
                    var mVersions = mAvalibleVersions.Where(version => version <= mCurrentVersion && version > m_DesiredVersion && version != new Version("1.0.0.0"));
                    if (mVersions == null || mVersions.Count() == 0)
                    {
                        string mMsg = "There are no '{0}' files to execute that match the version. Requested: '{1}', Current: '{2}'";
                        Console.WriteLine(string.Format(mMsg, mUpOrDown, m_DesiredVersion.ToString(), mCurrentVersion.ToString()));
                    }
                    foreach (Version item in mVersions)
                    {
                        string mScriptWithPath = mDatabaseManager.GetScriptPath("Downgrade") + "Version_" + item.ToString() + ".sql";
                        string mFileName = "Version_" + item.ToString() + ".sql";
                        Stopwatch mScriptWatch = new Stopwatch();
                        mScriptWatch.Start();
                        mDatabaseManager.ExecuteScriptFile(mScriptWithPath);
                        Console.WriteLine("Elapsed time: {0} File: '{1}' ", mWatch.Elapsed, mFileName);
                    }
                }
            }
            mWatch.Stop();
            Console.WriteLine("Time elapsed as per stopwatch: {0} ", mWatch.Elapsed);
        }

        private static void ShowHelp()
        {
            string mTab = "    ";
            Console.WriteLine("args is null");
            Console.WriteLine("Usage:");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(mTab + "--Version=1.0.0.0  a requested Version of 0.0.0.0 will delete the database.");
        }
    }
}
