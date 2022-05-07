using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace GrowthWare.DataAccess.SQLServer
{
    public class DState : ASearch, IState
    {
        private MState m_Profile = null;
        private int m_SecurityEntitySeqID = -2;

        public MState Profile { get{return m_Profile;} set{this.m_Profile = value;} }
        public int SecurityEntitySeqId { get{return this.m_SecurityEntitySeqID;} set{this.m_SecurityEntitySeqID = value;} }

        public DataRow GetState 
        {
            get
            {
                this.checkValid();
                String mStoredProcedure  = "ZGWOptional.Get_State";
                SqlParameter[] mParameters = {
                    new SqlParameter("@P_State", m_Profile.State)
                };
                return base.GetDataRow(mStoredProcedure, mParameters);
            }
        }

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

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public DataTable GetStates => throw new NotImplementedException();

        public void Save()
        {
            throw new NotImplementedException();
        }

        public DataTable Search(MSearchCriteria searchCriteria)
        {
            DataTable mRetVal = base.Search(searchCriteria, "[ZGWOptional].[vwSearchStates]");
            return mRetVal;
        }
    }
}

