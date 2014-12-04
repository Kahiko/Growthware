using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces;
using GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.Base;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Data;
using System.Data.SqlClient;

namespace GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.V2008
{
    /// <summary>
    /// Class DGroups
    /// </summary>
    public class DGroups : DDBInteraction, IDGroups
    {
        /// <summary>
        /// GroupProfile
        /// </summary>
        /// <value>The profile.</value>
        public MGroupProfile Profile { get; set; }
        /// <summary>
        /// GroupRoles
        /// </summary>
        /// <value>The group roles profile.</value>
        public MGroupRoles GroupRolesProfile { get; set; }

        DataTable IDGroups.Search(MSearchCriteria searchCriteria)
        {
            string mStoredProcedure = "ZGWSystem.Get_Paginated_Data";
            DataTable mRetVal;
            SqlParameter[] mParameters =
			 {
			  new SqlParameter("@P_Columns", searchCriteria.Columns),
			  new SqlParameter("@P_OrderByColumn", searchCriteria.OrderByColumn),
			  new SqlParameter("@P_OrderByDirection", searchCriteria.OrderByDirection),
			  new SqlParameter("@P_PageSize", searchCriteria.PageSize),
			  new SqlParameter("@P_SelectedPage", searchCriteria.SelectedPage),
			  new SqlParameter("@P_TableOrView", "ZGWSecurity.vwSearchGroups"),
			  new SqlParameter("@P_WhereClause", searchCriteria.WhereClause)
			 };
            mRetVal = base.GetDataTable(mStoredProcedure, mParameters);
            return mRetVal;
        }

        /// <summary>
        /// Sets or gets the SecurityEntitySeqID
        /// </summary>
        /// <value>The security entity seq ID.</value>
        public int SecurityEntitySeqID { get; set; }

        /// <summary>
        /// Deletes a group in a given Security Entity
        /// </summary>
        /// <returns>bool</returns>
        public bool DeleteGroup()
        {
            SqlParameter[] mParameters = { new SqlParameter("@P_Security_Entity_SeqID", Profile.SecurityEntityId), new SqlParameter("@P_Group_SeqID", Profile.Id) };
            String mStoreProc = "ZGWSecurity.Delete_Group";
            base.ExecuteNonQuery( mStoreProc, mParameters);
            return true;
        }

        /// <summary>
        /// Adds a group to a Security Entity
        /// </summary>
        public void AddGroup()
        {
            String mStoreProc = "ZGWSecurity.Set_Group";
            SqlParameter[] mParameters = GetInsertUpdateParameters();
            base.ExecuteNonQuery( mStoreProc, mParameters);
        }

        /// <summary>
        /// Get's all of the groups for a given Security Entity
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GroupsBySecurityEntity()
        {
            SqlParameter[] mParameters = { new SqlParameter("@P_Security_Entity_SeqID", Profile.SecurityEntityId), new SqlParameter("@P_Group_SeqID", -1) };
            String mStoreProc = "ZGWSecurity.Get_Group";
            return base.GetDataTable( mStoreProc, mParameters);
        }

        /// <summary>
        /// Returns a data row necessary to populate MGroupProfile
        /// </summary>
        /// <returns>DataRow</returns>
        public DataRow ProfileData()
        {
            SqlParameter[] mParameters = { new SqlParameter("@P_Security_Entity_SeqID", Profile.SecurityEntityId), new SqlParameter("@P_Group_SeqID", Profile.Id) };
            String mStoreProc = "ZGWSecurity.Get_Group";
            return base.GetDataRow( mStoreProc, mParameters);
        }

        /// <summary>
        /// Updates a group effects all Security Entities
        /// </summary>
        /// <returns>bool</returns>
        public void Save()
        {
            SqlParameter[] mParameters = GetInsertUpdateParameters();
            String mStoreProc = "ZGWSecurity.Set_Group";
            base.ExecuteNonQuery( mStoreProc, mParameters);
        }

        /// <summary>
        /// Returns a DataTable of Group roles
        /// </summary>
        /// <returns>DataTable</returns>
        /// <exception cref="System.ApplicationException"></exception>
        public DataTable GroupRoles()
        {
            if (GroupRolesProfile.GroupSeqId == -1)
            {
                throw new ArgumentException("The GroupRoles Profile must be set.");
            }
            string mymStoreProcedure = "ZGWSecurity.Get_Group_Roles";
            SqlParameter[] mParameters = { new SqlParameter("@P_Security_Entity_SeqID", GroupRolesProfile.SecurityEntityId), new SqlParameter("@P_Group_SeqID", GroupRolesProfile.GroupSeqId) };
            return base.GetDataTable(mymStoreProcedure, mParameters);
        }

        /// <summary>
        /// Updates the Groups roles
        /// </summary>
        /// <returns>bool</returns>
        /// <exception cref="System.ApplicationException"></exception>
        public bool UpdateGroupRoles()
        {
            if (GroupRolesProfile.GroupSeqId == -1)
            {
                throw new ArgumentException("The GroupRoles Profile must be set.");
            }
            string mymStoreProcedure = "ZGWSecurity.Set_Group_Roles";
            SqlParameter[] mParameters = { new SqlParameter("@P_Group_SeqID", GroupRolesProfile.GroupSeqId), new SqlParameter("@P_Security_Entity_SeqID", GroupRolesProfile.SecurityEntityId), new SqlParameter("@P_Roles", GroupRolesProfile.Roles), new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(GroupRolesProfile)) };
            base.ExecuteNonQuery(mymStoreProcedure, mParameters);
            return true;
        }

        private SqlParameter[] GetInsertUpdateParameters()
        {
            SqlParameter[] mParameters = { new SqlParameter("@P_Group_SeqID", Profile.Id), new SqlParameter("@P_Name", Profile.Name), new SqlParameter("@P_Description", Profile.Description), new SqlParameter("@P_Security_Entity_SeqID", Profile.SecurityEntityId), new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(Profile)), GetSqlParameter("@P_PRIMARY_KEY", Profile.Id, ParameterDirection.Output) };
            return mParameters;
        }
    }
}
