using System;
using System.Data;
using System.Data.SqlClient;
using GrowthWare.Framework.DataAccessLayer.Interfaces;
using GrowthWare.Framework.DataAccessLayer.SQLServer.Base;
using GrowthWare.Framework.Model.Profiles;

namespace GrowthWare.Framework.DataAccessLayer.SQLServer.V2008
{
	/// <summary>
	/// Provides data access to SQL Server 2008
	/// </summary>
	public class DSecurityEntity : DDBInteraction, IDSecurityEntity
	{
		DataTable IDSecurityEntity.GetSecurityEntities()
		{
			string mStoredProcedure = "ZGWSecurity.Get_Security_Entity";
			SqlParameter[] mParameters =
			{
			  new SqlParameter("@P_SE_SEQ_ID", -1),
			  base.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.InputOutput)
			};
			return base.GetDataTable(ref mStoredProcedure, ref mParameters);
		}

		DataTable IDSecurityEntity.GetSecurityEntities(String account, int securityEntityID, bool isSecurityEntityAdministrator)
		{
			if (String.IsNullOrEmpty(account)) { throw new NotImplementedException("ACCT not given"); };
			if (securityEntityID == -1) { throw new NotImplementedException("SE_SEQ_ID not given"); };
			string mStoredProcedure = "ZGWSecurity.Get_Valid_Security_Entity";
			SqlParameter[] mParameters =
			{
			new SqlParameter("@P_ACCT", account),
			new SqlParameter("@P_IS_SE_ADMIN", isSecurityEntityAdministrator),
			new SqlParameter("@P_SE_SEQ_ID", securityEntityID),
			base.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
			};
			return base.GetDataTable(ref mStoredProcedure, ref mParameters);
		}

		int IDSecurityEntity.Save(ref MSecurityEntityProfile profile)
		{
			string mStoredProcedure = "ZGWSecurity.Set_Security_Entity";
			SqlParameter[] mParameters =
			 {
			   new SqlParameter("@P_SE_SEQ_ID", profile.Id),
			   new SqlParameter("@P_NAME", profile.Name),
			   new SqlParameter("@P_DESCRIPTION", profile.Description),
			   new SqlParameter("@P_URL", profile.Url),
			   new SqlParameter("@P_STATUS_SEQ_ID", profile.StatusSeqId),
			   new SqlParameter("@P_DAL", profile.DAL),
			   new SqlParameter("@P_DAL_NAME", profile.DAL),
			   new SqlParameter("@P_DAL_NAME_SPACE", profile.DALNamespace),
			   new SqlParameter("@P_DAL_STRING", profile.ConnectionString),
			   new SqlParameter("@P_SKIN", profile.Skin),
			   new SqlParameter("@P_STYLE", profile.Style),
			   new SqlParameter("@P_PARENT_SE_SEQ_ID", profile.ParentSeqId),
			   new SqlParameter("@P_ENCRYPTION_TYPE", profile.EncryptionType),
			   new SqlParameter("@P_ADDED_UPDATED_BY", profile.AddedBy),
			   new SqlParameter("@P_ADDED_UPDATED_DATE", DateTime.Now),
			   base.GetSqlParameter("@P_PRIMARY_KEY", null, ParameterDirection.Output),
			   base.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
			 };
			base.ExecuteNonQuery(ref mStoredProcedure, ref mParameters);
			profile.Id = int.Parse(base.GetParameterValue("@P_PRIMARY_KEY", ref mParameters).ToString());
			return profile.Id;
		}

	}
}
