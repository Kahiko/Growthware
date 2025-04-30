using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.SQLServer;

public class DState : AbstractDBInteraction, IState
{

#region Member Fields
    private MState m_Profile = null;
    private int m_SecurityEntitySeqID = -2;
#endregion

#region Constructors
    public DState(string connectionString, int securityEntitySeqId): base() 
    {
        this.ConnectionString = connectionString;
        m_SecurityEntitySeqID = securityEntitySeqId;
    }
#endregion

    public MState Profile { get{return m_Profile;} set{this.m_Profile = value;} }
    public int SecurityEntitySeqId { get{return this.m_SecurityEntitySeqID;} set{this.m_SecurityEntitySeqID = value;} }

    private void checkValid()
    {
        base.IsValid();
        if(this.m_Profile == null)
        {
            throw new DataAccessLayerException("Profile property must be set before calling methods from this class");
        }
        if(this.m_SecurityEntitySeqID == -2)
        {
            throw new DataAccessLayerException("SecurityEntitySeqID property must be set before calling methods from this class");
        }
    }

    private SqlParameter[] getInsertUpdateParameters()
    {
        SqlParameter[] mParameters = [
            base.GetSqlParameter("@P_State      "   , m_Profile.State, ParameterDirection.Input),
            base.GetSqlParameter("@P_Description"   , m_Profile.Description, ParameterDirection.Input),
            base.GetSqlParameter("@P_StatusSeqId"   , m_Profile.StatusId, ParameterDirection.Input),
            base.GetSqlParameter("@P_Updated_By"    , m_Profile.UpdatedBy, ParameterDirection.Input),
            base.GetSqlParameter("@P_Primary_Key"   , m_Profile.State, ParameterDirection.InputOutput)
        ];
        return mParameters;
    }

    async Task<DataTable> IState.GetStates()
    {
            this.checkValid();
            String mStoredProcedure  = "[ZGWOptional].[Get_State]";
            SqlParameter[] mParameters = [
                new SqlParameter("@P_State", m_Profile.State)
            ];
            return await base.GetDataTableAsync(mStoredProcedure, mParameters);
    }

    async Task IState.Save()
    {
        SqlParameter[] mParameters = getInsertUpdateParameters();
        String mStoreProc = "[ZGWOptional].[Set_State]";
        await base.ExecuteNonQueryAsync( mStoreProc, mParameters);            
    }
}
