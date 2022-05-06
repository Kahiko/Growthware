using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System.Data;
using System.Data.SqlClient;

namespace GrowthWare.DataAccess.SQLServer
{
    public class DDBInformation : DDBInteraction, IDBInformation
    {
        private MDBInformation m_Profile = null;
        public MDBInformation Profile { get{return this.m_Profile;} set{this.m_Profile = value;} }

        public DataRow GetProfileRow
        {
            get
            {
                string mStoredProcedure = "ZGWSystem.Get_Database_Information";
                return base.GetDataRow(mStoredProcedure);
            }
        }

        public bool UpdateProfile()
        {
            string mStoredProcedure = "ZGWSystem.Set_DataBase_Information";
            SqlParameter[] mParameters = { 
                new SqlParameter("@P_Database_InformationSeqId", m_Profile.InformationSeqId),
                new SqlParameter("@P_Version", m_Profile.Version),
                new SqlParameter("@P_Enable_Inheritance", m_Profile.EnableInheritance),
                new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile)),

                GetSqlParameter("@P_Primary_Key", -1, ParameterDirection.InputOutput)
            };
            try
            {
                 base.ExecuteNonQuery(mStoredProcedure, mParameters);
                 return true;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
