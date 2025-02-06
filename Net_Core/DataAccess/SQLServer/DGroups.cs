using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace GrowthWare.DataAccess.SQLServer
{
    /// <summary>
    /// Class DGroups
    /// </summary>
    public class DGroups : AbstractDBInteraction, IGroups
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
            SqlParameter[] mParameters = [
                new SqlParameter("@P_SecurityEntitySeqId", Profile.SecurityEntityID), 
                new SqlParameter("@P_GroupSeqId", Profile.Id) 
            ];
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
            SqlParameter[] mParameters = getInsertUpdateParameters();
            base.ExecuteNonQuery( mStoreProc, mParameters);
        }

        /// <summary>
        /// Get's all of the groups for a given Security Entity
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GroupsBySecurityEntity()
        {
            SqlParameter[] mParameters = [ 
                new SqlParameter("@P_SecurityEntitySeqId", Profile.SecurityEntityID), 
                new SqlParameter("@P_GroupSeqId", -1) 
            ];
            String mStoreProc = "ZGWSecurity.Get_Group";
            return base.GetDataTable( mStoreProc, mParameters);
        }

        /// <summary>
        /// Returns a data row necessary to populate MGroup
        /// </summary>
        /// <returns>DataRow</returns>
        public DataRow ProfileData()
        {
            SqlParameter[] mParameters = [
                new SqlParameter("@P_SecurityEntitySeqId", Profile.SecurityEntityID), 
                new SqlParameter("@P_GroupSeqId", Profile.Id) 
            ];
            String mStoreProc = "ZGWSecurity.Get_Group";
            return base.GetDataRow( mStoreProc, mParameters);
        }

        /// <summary>
        /// Updates a group effects all Security Entities
        /// </summary>
        /// <returns>bool</returns>
        public int Save()
        {
            SqlParameter[] mParameters = getInsertUpdateParameters();
            String mStoreProc = "ZGWSecurity.Set_Group";
            base.ExecuteNonQuery( mStoreProc, mParameters);
            int mRetVal = int.Parse(GetParameterValue("@P_PRIMARY_KEY", mParameters));
            return mRetVal;
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
            SqlParameter[] mParameters = [ 
                new SqlParameter("@P_SecurityEntitySeqId", GroupRolesProfile.SecurityEntityID), 
                new SqlParameter("@P_GroupSeqId", GroupRolesProfile.GroupSeqId) 
            ];
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
            SqlParameter[] mParameters = [
                new SqlParameter("@P_GroupSeqId", GroupRolesProfile.GroupSeqId), 
                new SqlParameter("@P_SecurityEntitySeqId", GroupRolesProfile.SecurityEntityID), 
                new SqlParameter("@P_Roles", GroupRolesProfile.Roles??""), 
                new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(GroupRolesProfile)) 
            ];
            base.ExecuteNonQuery(mymStoreProcedure, mParameters);
            return true;
        }

        private SqlParameter[] getInsertUpdateParameters()
        {
            SqlParameter[] mParameters = [
                new SqlParameter("@P_GroupSeqId", Profile.Id), 
                new SqlParameter("@P_Name", Profile.Name), 
                new SqlParameter("@P_Description", Profile.Description), 
                new SqlParameter("@P_SecurityEntitySeqId", Profile.SecurityEntityID), 
                new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(Profile, Profile.Id)), 
                GetSqlParameter("@P_PRIMARY_KEY", Profile.Id, ParameterDirection.Output) 
            ];
            return mParameters;
        }
    }
}
