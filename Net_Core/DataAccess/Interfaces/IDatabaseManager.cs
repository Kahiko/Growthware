using GrowthWare.DataAccess.Interfaces.Base;
using System;
using System.Collections.Generic;

namespace GrowthWare.DataAccess.Interfaces
{
    public interface IDatabaseManager : IDBInteraction
    {
        string DatabaseName { get; }
        /// <summary>
        /// Executes the upgrade scripts Version_0.0.0.0.sql and Version_1.0.0.0.sql.
        /// </summary>
        /// <remarks>
        /// Version_0.0.0.0.sql will create the database if it is needed as well as any of the DDL and DML.<br />
        /// Version_1.0.0.0.sql will populate the database with the very first version of data.<br />
        /// NOTE: Changes the underlying ConnectionString<br />
        /// </remarks>
        /// <exception cref="Exception">Thrown when the database name is not valid or when the DDL/DML scripts fail to run.</exception>        
        void Create();

        /// <summary>
        /// Executes downgrade script Version_1.0.0.0.sql.
        /// </summary>
        /// <remarks>
        /// NOTE: Changes the underlying ConnectionString<br />
        /// </remarks>
        /// <exception cref="Exception">Thrown when the database creation fails.</exception>
        void Delete();

        /// <summary>
        /// Determines if the database exists.
        /// </summary>
        /// <remarks>
        /// NOTE: Changes the underlying ConnectionString<br />
        /// </remarks>
        /// <returns>True if the database exists, false otherwise.</returns>
        Boolean Exists();

        /// <summary>
        /// Executes the script file specified in the scriptWithPath parameter.
        /// </summary>
        /// <param name="scriptWithPath">The path including the name to the script file to be executed.</param>
        /// <returns>True if the script execution was successful, false otherwise.</returns>
        bool ExecuteScriptFile(string scriptWithPath);

        /// <summary>
        /// Returns the path to the script file based on the given direction and the ConfigSettings.DataAccessLayer.
        /// </summary>
        /// <param name="theDirection">The direction of the script file.</param>
        /// <returns>The path to the script file.</returns>
        string GetScriptPath(string theDirection);

        /// <summary>
        /// Retrieves the [Version] column value from the [ZGWSystem].[Database_Information] table in the database.
        /// </summary>
        /// <returns>The version number of the database as a Version object.</returns>
        Version GetVersion();

        /// <summary>
        /// Process the script files for the specified direction.
        /// </summary>
        /// <param name="isUpgrade"></param>
        /// <param name="currentVersion"></param>
        /// <param name="desiredVersion"></param>
        /// <param name="availbleVersions"></param>
        /// <remarks>
        /// NOTE: Changes the underlying ConnectionString<br />
        /// </remarks>
        void ProcessScriptFiles(bool isUpgrade, Version currentVersion, Version desiredVersion, IEnumerable<Version> availbleVersions);

        /// <summary>
        /// Sets the DatabaseName given the ConnectionString property.
        /// </summary>
        void SetDatabaseName();

        /// <summary>
        /// Updates the log path in the [ZGWOptional].[Directories] table based on the value of ConfigSettings.LogPath.
        /// </summary>
        void UpdateLogPath();
    }
}
