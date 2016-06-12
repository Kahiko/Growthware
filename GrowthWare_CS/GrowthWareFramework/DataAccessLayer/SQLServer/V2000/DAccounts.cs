using System;
using System.Data;
using System.Data.SqlClient;
using GrowthWare.Framework.DataAccessLayer.SQLServer.Base;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.Framework.Model.Enumerations;

namespace GrowthWare.Framework.DataAccessLayer.SQLServer.V2000
{
	/// <summary>
	/// DAccounts provides all database interaction to SQL Server 2000 to 2005
	/// </summary>
	/// <remarks>
	///		The Profile and SecurityEntitySeqID properties must be set
	///		before using any methods.
	///		Properties where chosen instead of parameters because all
	///		methods will need one or both to perform their work.
	///	</remarks>
	public class DAccounts : DDBInteraction, IDAccount
	{
		private MAccountProfile m_Profile = null;

		private int m_SecurityEntitySeqID = -2;

		/// <summary>
		/// Account profile as defined in the GrowthWare.ModelObjects.Accounts namespace.
		/// </summary>
		/// <value></value>
		/// <returns>None uses setters and getters</returns>
		/// <remarks></remarks>
		MAccountProfile IDAccount.Profile
		{
			get { return m_Profile; }
			set { m_Profile = value; }
		}

		int IDAccount.SecurityEntitySeqID
		{
			get { return m_SecurityEntitySeqID; }
			set { m_SecurityEntitySeqID = value; }
		}

		/// <summary>
		/// Retruns a DataRow of account the account details
		/// </summary>
		/// <returns>DataRow</returns>
		/// <remarks>Usefull for populating MAccountProfile</remarks>
		DataRow IDAccount.GetAccount
		{
			get
			{
				checkValid();
				String mStoredProcedure = "ZFP_GET_ACCT";
				SqlParameter[] mParameters = { 
				new SqlParameter("@P_IS_SYSTEM_ADMIN", Convert.ToInt32(m_Profile.IsSystemAdmin)), 
				new SqlParameter("@P_ACCOUNT", m_Profile.Account), 
				new SqlParameter("@P_SE_SEQ_ID", m_SecurityEntitySeqID), 
				base.GetSqlParameter("@P_ErrorCode", -1, ParameterDirection.Output) };
				return base.GetDataRow(ref mStoredProcedure, ref mParameters);
			}
		}

		DataTable IDAccount.GetAccounts
		{
			get
			{
				checkValid();
				String mStoredProcedure = "ZFP_GET_ACCT";
				SqlParameter[] mParameters =
				{
					new SqlParameter("@P_Is_System_Admin", m_Profile.IsSystemAdmin),
					new SqlParameter("@P_SE_SEQ_ID", m_SecurityEntitySeqID),
					new SqlParameter("@P_Account", ""),
					base.GetSqlParameter("@P_ErrorCode", -1, ParameterDirection.Output)
				};
				return base.GetDataTable(ref mStoredProcedure, ref mParameters);
			}
		}

		DataTable IDAccount.GetMenu(string account, MenuType menuType) 
		{
			String mStoredProcedure = "ZFP_GET_MENU_DATA";
			SqlParameter[] mParameters =
			{
			 new SqlParameter("@P_SE_SEQ_ID", m_SecurityEntitySeqID),
			 new SqlParameter("@P_NAV_TYPE_ID", menuType),
			 new SqlParameter("@P_ACCT", m_Profile.Account),
			 base.GetSqlParameter("@P_ErrorCode", -1, ParameterDirection.Output)
			};
			return base.GetDataTable(ref mStoredProcedure, ref mParameters);		
		}

		DataTable IDAccount.GetRoles()
		{
			checkValid();
			String mStoredProcedure = "ZFP_GET_ACCT_RLS";
			SqlParameter[] mParameters = { 
				new SqlParameter("@P_ACCOUNT", m_Profile.Account), 
				new SqlParameter("@P_SE_SEQ_ID", m_SecurityEntitySeqID), 
				base.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output) };
			return base.GetDataTable(ref mStoredProcedure, ref mParameters);
		}

		DataTable IDAccount.GetGroups()
		{
			checkValid();
			String mStoredProcedure = "ZFP_GET_ACCT_GRPS";
			SqlParameter[] mParameters = { 
				new SqlParameter("@P_ACCOUNT", m_Profile.Account), 
				new SqlParameter("@P_SE_SEQ_ID", m_SecurityEntitySeqID), 
				base.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output) };
			return base.GetDataTable(ref mStoredProcedure, ref mParameters);
		}

		/// <summary>
		/// Returns a data table of all roles assigned directly and/or by group
		/// </summary>
		/// <returns>DataTable</returns>
		/// <remarks></remarks>
		DataTable IDAccount.GetSecurity()
		{
			checkValid();
			String mStoredProcedure = "ZFP_GET_ACCT_SECURITY";
			SqlParameter[] mParameters = { 
				new SqlParameter("@P_ACCT", m_Profile.Account), 
				new SqlParameter("@P_SE_SEQ_ID", m_SecurityEntitySeqID), 
				base.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output) };
			return base.GetDataTable(ref mStoredProcedure, ref mParameters);
		}

		/// <summary>
		/// Inserts or updates account information
		/// </summary>
		/// <returns>DataRow</returns>
		/// <remarks>
		/// </remarks>
		int IDAccount.Save()
		{
			checkValid();
			int mRetInt;
			String mStoredProcedure = "ZFP_SET_ACCOUNT";
			SqlParameter[] mParameters = { 
				new SqlParameter("@P_ACCT_SEQ_ID", m_Profile.Id), 
				new SqlParameter("@P_STATUS_SEQ_ID", m_Profile.Status), 
				new SqlParameter("@P_ACCOUNT", m_Profile.Account), 
				new SqlParameter("@P_FIRST_NAME", m_Profile.FirstName), 
				new SqlParameter("@P_LAST_NAME", m_Profile.LastLogin), 
				new SqlParameter("@P_MIDDLE_NAME", m_Profile.MiddleName), 
				new SqlParameter("@P_PREFERED_NAME", m_Profile.PreferedName), 
				new SqlParameter("@P_EMAIL", m_Profile.EMail), 
				new SqlParameter("@P_PWD", m_Profile.Password), 
				new SqlParameter("@P_PASSWORD_LAST_SET", m_Profile.PasswordLastSet), 
				new SqlParameter("@P_FAILED_ATTEMPTS", m_Profile.FailedAttempts), 
				new SqlParameter("@P_ADDED_BY", m_Profile.AddedBy), 
				new SqlParameter("@P_ADDED_DATE", m_Profile.AddedDate), 
				new SqlParameter("@P_LAST_LOGIN", m_Profile.LastLogin), 
				new SqlParameter("@P_TIME_ZONE", m_Profile.TimeZone), 
				new SqlParameter("@P_LOCATION", m_Profile.Location), 
				new SqlParameter("@P_ENABLE_NOTIFICATIONS", m_Profile.Enable_Notifications), 
				new SqlParameter("@P_IS_SYSTEM_ADMIN", m_Profile.IsSystemAdmin), 
				new SqlParameter("@P_UPDATED_BY", m_Profile.UpdatedBy), 
				new SqlParameter("@P_UPDATED_DATE", m_Profile.UpdatedDate), 
				base.GetSqlParameter("@P_PRIMARY_KEY", -1, ParameterDirection.Output),
				base.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output) };
			base.ExecuteNonQuery(ref mStoredProcedure, ref mParameters);
			mRetInt = int.Parse(base.GetParameterValue("@P_PRIMARY_KEY", ref mParameters));
			return mRetInt;
		}

		void IDAccount.SaveGroups()
		{
			String mStoredProcedure = "ZFP_SET_ACCT_GRPS";
			String mCommaSeporatedString = m_Profile.GetCommaSeporatedAssignedGroups();
			SqlParameter[] mParameters = {
			  new SqlParameter("@P_ACCT", m_Profile.Account),
			  new SqlParameter("@P_SE_SEQ_ID", m_SecurityEntitySeqID),
			  new SqlParameter("@P_GROUPS", mCommaSeporatedString),
			  new SqlParameter("@P_ADDUPD_BY", base.GetAddedUpdatedBy(m_Profile)),
			  base.GetSqlParameter("@P_ErrorCode", -1, ParameterDirection.Output)
			 };
			base.ExecuteNonQuery(ref mStoredProcedure, ref mParameters);
		}

		void IDAccount.SaveRoles()
		{
			String mStoredProcedure  = "ZFP_SET_ACCT_RLS";
			String mCommaSeporatedString = m_Profile.GetCommaSeporatedAssingedRoles();
			SqlParameter[] mParameters = {
			  new SqlParameter("@P_ACCT", m_Profile.Account),
			  new SqlParameter("@P_SE_SEQ_ID", m_SecurityEntitySeqID),
			  new SqlParameter("@P_ROLES", mCommaSeporatedString),
			  new SqlParameter("@P_ADDUPD_BY", base.GetAddedUpdatedBy(m_Profile)),
			  base.GetSqlParameter("@P_ErrorCode", -1, ParameterDirection.Output)
			 };
			base.ExecuteNonQuery(ref mStoredProcedure, ref mParameters);
		}

		void IDAccount.Delete()
		{
			string mStoreProcedure = "ZFP_DEL_ACCOUNT";
			SqlParameter[] mParameters = { 
				new SqlParameter("@P_ACCT_SEQ_ID", m_Profile.Id), 
				base.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
			};
			base.ExecuteNonQuery(ref mStoreProcedure, ref mParameters);		
		}

		private void checkValid()
		{
			base.isValid();
			if (m_Profile == null)
			{
				throw new ArgumentNullException("The Profile property must set before using any methods from this class.");
			}
			if (m_SecurityEntitySeqID == -1)
			{
				throw new ArgumentNullException("The SE_SEQ_ID property must set before using any methods from this class.");
			}
		}

	}
}
