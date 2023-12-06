using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace GrowthWare.DataAccess.SQLServer;
public class DLogging : AbstractDBInteraction, ILogging
{

    MLoggingProfile ILogging.GetLog(int logSeqId)
    {
        String mStoredProcedure = "[ZGWSystem].[Get_Logs]";
        SqlParameter[] mParameters =
        {
           new SqlParameter("@P_LogSeqId", logSeqId)
        };
        DataRow mDataRow = base.GetDataRow(mStoredProcedure, mParameters);
        if(mDataRow != null)
        {
            return new MLoggingProfile(mDataRow);
        } 
        else 
        {
            return new MLoggingProfile();
        }
    }

    void ILogging.Save(MLoggingProfile profile)
    {
        if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!");
        String mStoredProcedure = "[ZGWSystem].[Set_Log]";
        SqlParameter[] mParameters =
        {
            new SqlParameter("@P_Account", profile.Account),
            new SqlParameter("@P_ClassName", profile.ClassName),
            new SqlParameter("@P_Component", profile.Component),
            new SqlParameter("@P_Level", profile.Level),
            new SqlParameter("@P_MethodName", profile.MethodName),
            new SqlParameter("@P_Msg", profile.Msg),
            base.GetSqlParameter("@P_Primary_Key", 0, ParameterDirection.Output)
        };
        base.ExecuteNonQuery(mStoredProcedure, mParameters);
    }
}