using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.SQLServer;
public class DLogging : AbstractDBInteraction, ILogging
{

#region Constructors
    public DLogging(string connectionString) : base() 
    { 
        this.ConnectionString = connectionString;
    }

    public async IAsyncEnumerable<IDataRecord> GetLogs([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using SqlConnection mSqlConnection = new SqlConnection(this.ConnectionString);
        await mSqlConnection.OpenAsync(cancellationToken);
        string mCommandText = $"SELECT {DBLogColumns.GetCommaSeparatedString()} FROM [ZGWSystem].[Logging] ORDER BY [LogDate] DESC;";
        using SqlCommand mSqlCommand = new SqlCommand(mCommandText, mSqlConnection);
        using SqlDataReader mReader = await mSqlCommand.ExecuteReaderAsync(CommandBehavior.SequentialAccess, cancellationToken);

        while (await mReader.ReadAsync(cancellationToken))
        {
            yield return mReader;
        }
    }
    #endregion

    async Task<MLoggingProfile> ILogging.GetLog(int logSeqId)
    {
        String mStoredProcedure = "[ZGWSystem].[Get_Logs]";
        SqlParameter[] mParameters = [
           new SqlParameter("@P_LogSeqId", logSeqId)
        ];
        DataRow mDataRow = await base.GetDataRowAsync(mStoredProcedure, mParameters);
        if(mDataRow != null)
        {
            return new MLoggingProfile(mDataRow);
        } 
        else 
        {
            return new MLoggingProfile();
        }
    }

    async Task ILogging.Save(MLoggingProfile profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!");
        String mStoredProcedure = "[ZGWSystem].[Set_Log]";
        SqlParameter[] mParameters = [
            new SqlParameter("@P_Account", profile.Account),
            new SqlParameter("@P_ClassName", profile.ClassName),
            new SqlParameter("@P_Component", profile.Component),
            new SqlParameter("@P_Level", profile.Level),
            new SqlParameter("@P_MethodName", profile.MethodName),
            new SqlParameter("@P_Msg", profile.Msg),
            base.GetSqlParameter("@P_Primary_Key", 0, ParameterDirection.Output)
        ];
        await base.ExecuteNonQueryAsync(mStoredProcedure, mParameters);
    }
}