using GrowthWare.DataAccess.Interfaces.Base;
using System;
using System.Data.SqlClient;

namespace GrowthWare.DataAccess.Interfaces
{
    public interface IDatabaseManager : IDBInteraction
    {
        string DatabaseName { get; set; }

        void Create();
        void Delete();
        Boolean Exists();
        bool ExecuteScriptFile(string scriptWithPath);
        // Boolean ExecuteScriptFile(string scriptWithPath, SqlConnection sqlConnection);
        string GetScriptPath(string theDirection);
        Version GetVersion();
        void UpdateLogPath();
    }
}
