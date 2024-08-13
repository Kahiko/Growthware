using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using System;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;

namespace GrowthWare.DatabaseManager
{
    internal class Program
    {
        private static Boolean m_CreatedDatabase = false;
        private static Version m_DesiredVersion = null;

        /// <summary>
        /// Retrieves the value of the specified command line argument.
        /// </summary>
        /// <param name="args">An array of command line arguments.</param>
        /// <param name="name">The name of the argument to retrieve.</param>
        /// <returns>The value of the specified argument, or null if the argument was not found.</returns>
        private static string getArgument(string[] args, string name)
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

        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0) // Check for null array
            {
                showHelp();
                return;
            }
            if (getArgument(args, "--Help") != null || getArgument(args, "--H") != null)
            {
                showHelp();
                return;
            }
            m_DesiredVersion = new Version(getArgument(args, "--Version"));
            string mAssemblyName = ConfigSettings.DataAccessLayerAssemblyName;
            DataTable mAvailbleFiles = null;
            List<Version> mAvailbleVersions = new List<Version>();
            Boolean mDeleteDatabase = false;
            string mNameSpace = ConfigSettings.DataAccessLayerNamespace;
            Stopwatch mWatch = new Stopwatch();
            mWatch.Start();

            IDatabaseManager mDatabaseManager = (IDatabaseManager)ObjectFactory.Create(mAssemblyName, mNameSpace, "DDatabaseManager");
            mDatabaseManager.ConnectionString = ConfigSettings.ConnectionString;
            mDatabaseManager.SetDatabaseName();
            if (m_DesiredVersion == new Version("0.0.0.0"))
            {
                mDeleteDatabase = true;
            }
            if (mDeleteDatabase)
            {
                if (mDatabaseManager.Exists())
                {
                    mDatabaseManager.Delete();
                    Console.WriteLine(String.Format("The '{0}' database has been deleted.", mDatabaseManager.DatabaseName));
                }
                else
                {
                    Console.WriteLine(String.Format("The '{0}' database does not exist, nothing to delete.", mDatabaseManager.DatabaseName));
                    mWatch.Stop();
                    Console.WriteLine("Time elapsed as per stopwatch: {0} ", mWatch.Elapsed);
                }
            }
            else
            {
                if (!mDatabaseManager.Exists())
                {
                    m_CreatedDatabase = true;
                    Console.WriteLine(String.Format("Attempting to create the '{0}' database.", mDatabaseManager.DatabaseName));
                    mDatabaseManager.Create();
                    Console.WriteLine(String.Format("The '{0}' database has been created.", mDatabaseManager.DatabaseName));
                }
                else
                {
                    Console.WriteLine(String.Format("The '{0}' database exists no need to create.", mDatabaseManager.DatabaseName));
                }
                Console.WriteLine("Starting upgrade/downgrade process.");
                Version mCurrentVersion = mDatabaseManager.GetVersion();
                string mUpOrDown = "Upgrade";
                bool mIsUpgrade = true;
                if (m_DesiredVersion == mCurrentVersion)
                {
                    Console.WriteLine("Database version matches requested version no work done.");
                    mWatch.Stop();
                    Console.WriteLine("Time elapsed as per stopwatch: {0} ", mWatch.Elapsed);
                    return;
                }
                if (m_DesiredVersion < mCurrentVersion)
                {
                    mIsUpgrade = false;
                    mUpOrDown = "Downgrade";
                }
                string mScriptPath = mDatabaseManager.GetScriptPath(mUpOrDown);
                if (mIsUpgrade)
                {
                    mAvailbleFiles = FileUtility.GetDirectory(mScriptPath, true, "Name", "ASC");
                }
                else
                {
                    Console.WriteLine("Attempting to Downgrade the database.");
                    mAvailbleFiles = FileUtility.GetDirectory(mScriptPath, true, "Name", "DESC");
                }
                string mVersionString = string.Empty;
                foreach (DataRow mDataRow in mAvailbleFiles.Rows)
                {
                    mVersionString = mDataRow["Name"].ToString().Split("_")[1].Replace(".sql", "").ToString();
                    Version mVersion = new Version(mVersionString);
                    mAvailbleVersions.Add(mVersion);
                }
                mDatabaseManager.ProcessScriptFiles(mIsUpgrade, mCurrentVersion, m_DesiredVersion, mAvailbleVersions);
            }
            if(m_CreatedDatabase) 
            {
                mDatabaseManager.UpdateLogPath();
            }
            mWatch.Stop();
            Console.WriteLine("Time elapsed as per stopwatch: {0} ", mWatch.Elapsed);
        }

        /// <summary>
        /// Displays the help information.
        /// </summary>
        private static void showHelp()
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
