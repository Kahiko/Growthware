using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace GrowthWare.DataAccess.SQLServer;

/// <summary>
/// Class DMessages
/// </summary>
public class DMessages : AbstractDBInteraction, IMessages
{

#region Member Fields
    private MMessage m_Profile = new MMessage();
#endregion

#region Constructors
    public DMessages(string connectionString) : base() 
    { 
        this.ConnectionString = connectionString;
    }
#endregion


    private SqlParameter[] getInsertUpdateParameters()
    {
        SqlParameter[] myParameters = {
                new SqlParameter("@P_MessageSeqId", m_Profile.Id),
                new SqlParameter("@P_SecurityEntitySeqId", m_Profile.SecurityEntitySeqId),
                new SqlParameter("@P_Name", m_Profile.Name),
                new SqlParameter("@P_Title", m_Profile.Title),
                new SqlParameter("@P_Description", m_Profile.Description),
                new SqlParameter("@P_BODY", m_Profile.Body),
                new SqlParameter("@P_Format_As_HTML", m_Profile.FormatAsHtml),
                new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile, m_Profile.Id)),
                GetSqlParameter("@P_Primary_Key", -1, ParameterDirection.Output)
            };
        return myParameters;
    }

    MMessage IMessages.Profile
    {
        get
        {
            return m_Profile;
        }
        set
        {
            m_Profile = value;
        }
    }

    int IMessages.SecurityEntitySeqId { get; set; }

    DataTable IMessages.Messages()
    {
        String storeProc = "ZGWCoreWeb.Get_Messages";
        SqlParameter[] mParamaters = {
                new SqlParameter("@P_MessageSeqId", -1),
                new SqlParameter("@P_SecurityEntitySeqId", m_Profile.SecurityEntitySeqId)
            };
        return GetDataTable(storeProc, mParamaters);
    }

    DataRow IMessages.Message(int messageSeqId)
    {
        String storeProc = "ZGWCoreWeb.Get_Messages";
        SqlParameter[] mParamaters = {
                new SqlParameter("@P_MessageSeqId", messageSeqId),
                new SqlParameter("@P_SecurityEntitySeqId", m_Profile.SecurityEntitySeqId)
            };
        return GetDataRow(storeProc, mParamaters);
    }

    int IMessages.Save()
    {
        String storeProc = "ZGWCoreWeb.Set_Message";
        SqlParameter[] mParameters = getInsertUpdateParameters();
        ExecuteNonQuery(storeProc, mParameters);
        return int.Parse(GetParameterValue("@P_Primary_Key", mParameters), CultureInfo.InvariantCulture);
    }
}
