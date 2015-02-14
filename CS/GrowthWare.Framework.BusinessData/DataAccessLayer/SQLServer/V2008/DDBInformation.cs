using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces;
using GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.Base;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Data;
using System.Data.SqlClient;

namespace GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.V2008
{
    /// <summary>
    /// Class DDBInformation.
    /// </summary>
    public class DDBInformation : DDBInteraction, IDBInformation
    {
        #region "Private Properties"
        #endregion
        private MDBInformation m_Profile;

        /// <summary>
        /// Sets the store procedure name as well as any parameters.
        /// </summary>
        /// <returns>SQL Parameters</returns>
        /// <remarks></remarks>
        public DataRow GetProfile()
        {
            SqlParameter[] myParameters = null;
            String mStoredProcedure = "ZGWSystem.Get_Database_Information";
            return base.GetDataRow(mStoredProcedure, myParameters);
        }

        /// <summary>
        /// Sets the store procedure name as well as any parameters.
        /// </summary>
        /// <returns>SQL Parameters</returns>
        /// <remarks></remarks>
        public bool UpdateProfile()
        {
            SqlParameter[] mParameters = { 
				new SqlParameter("@P_Database_Information_SeqID", m_Profile.Information_SEQ_ID), 
				new SqlParameter("@P_Version", m_Profile.Version), 
				new SqlParameter("@P_Enable_Inheritance", m_Profile.EnableInheritance), 
				new SqlParameter("@P_Added_Updated_By",  GetAddedUpdatedBy(m_Profile)), 
				GetSqlParameter("@P_Primary_Key", -1, ParameterDirection.InputOutput)
			};
            String mStoredProcedure = "ZGWSystem.Set_DataBase_Information";
            try
            {
                base.ExecuteNonQuery(mStoredProcedure, mParameters);
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// DB Information profile as defined in the GrowthWare.ModelObjects namespace.
        /// </summary>
        /// <value></value>
        /// <returns>None uses setters and getters</returns>
        /// <remarks></remarks>
        public MDBInformation Profile
        {
            get { return m_Profile; }
            set { m_Profile = value; }
        }
    }
}
