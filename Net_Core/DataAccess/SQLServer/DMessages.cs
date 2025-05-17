using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;

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
                new("@P_MessageSeqId", m_Profile.Id),
                new("@P_SecurityEntitySeqId", m_Profile.SecurityEntitySeqId),
                new("@P_Name", m_Profile.Name),
                new("@P_Title", m_Profile.Title),
                new("@P_Description", m_Profile.Description),
                new("@P_BODY", m_Profile.Body),
                new("@P_Format_As_HTML", m_Profile.FormatAsHtml),
                new("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile, m_Profile.Id)),
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

    async Task<DataTable> IMessages.Messages()
    {
        String storeProc = "[ZGWCoreWeb].[Get_Messages]";
        SqlParameter[] mParamaters = {
                new("@P_MessageSeqId", -1),
                new("@P_SecurityEntitySeqId", m_Profile.SecurityEntitySeqId)
            };
        return await base.GetDataTableAsync(storeProc, mParamaters);
    }

    DataRow IMessages.Message(int messageSeqId)
    {
        String storeProc = "[ZGWCoreWeb].[Get_Messages]";
        SqlParameter[] mParamaters = {
                new("@P_MessageSeqId", messageSeqId),
                new("@P_SecurityEntitySeqId", m_Profile.SecurityEntitySeqId)
            };
        return GetDataRow(storeProc, mParamaters);
    }

    async Task<int> IMessages.Save()
    {
        String storeProc = "[ZGWCoreWeb].[Set_Message]";
        SqlParameter[] mParameters = getInsertUpdateParameters();
        await base.ExecuteNonQueryAsync(storeProc, mParameters);
        return int.Parse(GetParameterValue("@P_Primary_Key", mParameters), CultureInfo.InvariantCulture);
    }
}
