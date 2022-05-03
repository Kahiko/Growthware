using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace GrowthWare.DataAccess.SQLServer
{
    /// <summary>
    /// Provides data access to SQL Server 2008
    /// </summary>
    public class DSecurityEntities : DSearch, ISecurityEntities
    {
        DataTable ISecurityEntities.GetSecurityEntities()
        {
            string mStoredProcedure = "ZGWSecurity.Get_Security_Entity";
            SqlParameter[] mParameters =
			{
			  new SqlParameter("@P_Security_Entity_SeqID", -1)
			};
            return base.GetDataTable(mStoredProcedure, mParameters);
        }

        DataTable ISecurityEntities.GetSecurityEntities(String account, int securityEntityId, bool isSecurityEntityAdministrator)
        {
            if (String.IsNullOrEmpty(account)) { throw new ArgumentNullException("account", "account cannot be a null reference (Nothing in Visual Basic)!"); };
            if (securityEntityId == -1) { throw new ArgumentNullException("securityEntityId", "securityEntityId cannot be a null reference (Nothing in Visual Basic)!"); };
            string mStoredProcedure = "ZGWSecurity.Get_Valid_Security_Entity";
            SqlParameter[] mParameters =
			{
			new SqlParameter("@P_ACCT", account),
			new SqlParameter("@P_IS_SE_ADMIN", isSecurityEntityAdministrator),
			new SqlParameter("@P_securityEntityID", securityEntityId),
			GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
			};
            return base.GetDataTable(mStoredProcedure, mParameters);
        }

        DataTable ISecurityEntities.GetValidSecurityEntities(string account, int securityEntityId, bool isSystemAdmin)
        {
            if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("account", "account cannot be a null reference (Nothing in Visual Basic)!");
            if (securityEntityId == -1) throw new ArgumentNullException("securityEntityId", "securityEntityId must be greater than -1");

            string mStoreProcedure = "ZGWSecurity.Get_Valid_Security_Entity";
            SqlParameter[] myParameters = 
            { 
                new SqlParameter("@P_Account", account), 
                new SqlParameter("@P_IS_SE_ADMIN", isSystemAdmin), 
                new SqlParameter("@P_Security_Entity_SeqID", securityEntityId)
            };
            return base.GetDataTable(mStoreProcedure, myParameters);
        }

        DataTable ISecurityEntities.Search(MSearchCriteria searchCriteria)
        {
            DataTable mRetVal = base.Search(searchCriteria, "[ZGWSecurity].[Security_Entities]");
            return mRetVal;
        }

        int ISecurityEntities.Save(MSecurityEntity profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile can not be nothing");
            SqlParameter mPrimaryKey = GetSqlParameter("@P_PRIMARY_KEY", null, ParameterDirection.Output);
            mPrimaryKey.Size = 10;
            string mStoredProcedure = "ZGWSecurity.Set_Security_Entity";
            SqlParameter[] mParameters =
			 {
                new SqlParameter("@P_Security_Entity_SeqID", profile.Id),
                new SqlParameter("@P_NAME", profile.Name),
                new SqlParameter("@P_DESCRIPTION", profile.Description),
                new SqlParameter("@P_URL", profile.Url),
                new SqlParameter("@P_Status_SeqID", profile.StatusSeqId),
                new SqlParameter("@P_DAL", profile.DataAccessLayer),
                new SqlParameter("@P_DAL_Name", profile.DataAccessLayerAssemblyName),
                new SqlParameter("@P_DAL_NAME_SPACE", profile.DataAccessLayerNamespace),
                new SqlParameter("@P_DAL_STRING", profile.ConnectionString),
                new SqlParameter("@P_SKIN", profile.Skin),
                new SqlParameter("@P_STYLE", profile.Style),
                new SqlParameter("@P_ENCRYPTION_TYPE", profile.EncryptionType),
                new SqlParameter("@P_Parent_Security_Entity_SeqID", profile.ParentSeqId),
                new SqlParameter("@P_Added_Updated_By",  GetAddedUpdatedBy(profile)),
                mPrimaryKey
			 };
            base.ExecuteNonQuery(mStoredProcedure, mParameters);
            profile.Id = int.Parse(GetParameterValue("@P_PRIMARY_KEY", mParameters).ToString(), CultureInfo.InvariantCulture);
            return profile.Id;
        }

    }
}
