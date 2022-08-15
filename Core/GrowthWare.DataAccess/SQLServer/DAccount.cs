using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace GrowthWare.DataAccess.SQLServer
{
    /// <summary>
    /// DAccounts provides all database interaction to SQL Server 2008
    /// </summary>
    /// <remarks>
    ///		The Profile and SecurityEntitySeqID properties must be set
    ///		before using any methods.
    ///		Properties where chosen instead of parameters because all
    ///		methods will need one or both to perform their work.
    ///	</remarks>    
    public class DAccounts: DSearch, IAccount
    {
#region Private Field

        private MAccountProfile m_Profile = null;
        private int m_SecurityEntitySeqID = -2;

#endregion

#region Public Properties
        MAccountProfile IAccount.Profile
        {
            get { return this.m_Profile; }
            set { this.m_Profile = value; }
        }

        int IAccount.SecurityEntitySeqId
        {
            get { return m_SecurityEntitySeqID; }
            set { m_SecurityEntitySeqID = value; }
        }

        DataRow IAccount.GetAccount
        {
            get
            {
                String mStoredProcedure = "ZGWSecurity.Get_Account";
                SqlParameter[] mParameters = { 
                    GetSqlParameter("@P_Is_System_Admin", m_Profile.IsSystemAdmin, ParameterDirection.Input),
                    GetSqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqID, ParameterDirection.Input),
                    GetSqlParameter("@P_Account", m_Profile.Account, ParameterDirection.Input)
                };
                return base.GetDataRow(mStoredProcedure, mParameters);
            }
        }

        DataTable IAccount.GetAccounts
        {
            get
            {
                checkValid();
                String mStoredProcedure = "ZGWSecurity.Get_Account";
                SqlParameter[] mParameters =
				{
					new SqlParameter("@P_Is_System_Admin", m_Profile.IsSystemAdmin),
					new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqID),
					new SqlParameter("@P_Account", "")
				};
                return base.GetDataTable(mStoredProcedure, mParameters);
            }
        }

#endregion

#region Public Methods
        DataTable IAccount.Roles()
        {
            checkValid();
            String mStoredProcedure = "ZGWSecurity.Get_Account_Roles";
            SqlParameter[] mParameters = { 
				new SqlParameter("@P_Account", m_Profile.Account), 
				new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqID) 
			};
            return base.GetDataTable(mStoredProcedure, mParameters);
        }

        DataTable IAccount.GetMenu(string account, MenuType menuType)
        {
            String mStoredProcedure = "ZGWSecurity.Get_Menu_Data";
            SqlParameter[] mParameters =
			{
			 new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqID),
			 new SqlParameter("@P_Navigation_Types_NVP_DetailSeqId", (int)menuType),
			 new SqlParameter("@P_Account", account)
			};
            return base.GetDataTable(mStoredProcedure, mParameters);
        }

        DataTable IAccount.Groups()
        {
            checkValid();
            String mStoredProcedure = "ZGWSecurity.Get_Account_Groups";
            SqlParameter[] mParameters = { 
				new SqlParameter("@P_Account", m_Profile.Account), 
				new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqID) 
			};
            return base.GetDataTable(mStoredProcedure, mParameters);
        }

        DataTable IAccount.Security()
        {
            String mStoredProcedure = "ZGWSecurity.Get_Account_Security";
            SqlParameter[] mParameters = { 
				new SqlParameter("@P_Account", m_Profile.Account), 
				new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqID) 
			};
            return base.GetDataTable(mStoredProcedure, mParameters);
        }

        int IAccount.Save()
        {
            checkValid();
            int mRetInt;
            String mStoredProcedure = "ZGWSecurity.Set_Account";
            SqlParameter[] mParameters = { 
				GetSqlParameter("@P_AccountSeqId", m_Profile.Id, ParameterDirection.InputOutput),
				new SqlParameter("@P_StatusSeqId", m_Profile.Status),
				new SqlParameter("@P_Account", m_Profile.Account),
				new SqlParameter("@P_First_Name", m_Profile.FirstName),
				new SqlParameter("@P_Last_Name", m_Profile.LastName),
				new SqlParameter("@P_Middle_Name", m_Profile.MiddleName),
				new SqlParameter("@P_Preferred_Name", m_Profile.PreferredName),
				new SqlParameter("@P_Email", m_Profile.Email),
				new SqlParameter("@P_Password", m_Profile.Password),
				new SqlParameter("@P_Password_Last_Set", m_Profile.PasswordLastSet),
				new SqlParameter("@P_Failed_Attempts", m_Profile.FailedAttempts),
				new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile)),
				new SqlParameter("@P_Last_Login", m_Profile.LastLogOn),
				new SqlParameter("@P_Time_Zone", m_Profile.TimeZone),
				new SqlParameter("@P_Location", m_Profile.Location),
				new SqlParameter("@P_Enable_Notifications", m_Profile.EnableNotifications),
				new SqlParameter("@P_Is_System_Admin", m_Profile.IsSystemAdmin)};
            base.ExecuteNonQuery(mStoredProcedure, mParameters);
            mRetInt = int.Parse(GetParameterValue("@P_AccountSeqId", mParameters), CultureInfo.InvariantCulture);
            return mRetInt;
        }

        void IAccount.SaveGroups()
        {
            String mStoredProcedure = "ZGWSecurity.Set_Account_Groups";
            SqlParameter[] mParameters = {
			  new SqlParameter("@P_Account", m_Profile.Account),
			  new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqID),
			  new SqlParameter("@P_Groups", m_Profile.GetCommaSeparatedAssignedGroups),
			  new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile))
			 };
            base.ExecuteNonQuery(mStoredProcedure, mParameters);
        }

        void IAccount.SaveRoles()
        {
            String mStoredProcedure = "ZGWSecurity.Set_Account_Roles";
            SqlParameter[] mParameters = {
			  new SqlParameter("@P_Account", m_Profile.Account),
			  new SqlParameter("@P_SecurityEntitySeqId", m_SecurityEntitySeqID),
			  new SqlParameter("@P_Roles", m_Profile.GetCommaSeparatedAssignedRoles),
			  new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile))
			 };
            base.ExecuteNonQuery(mStoredProcedure, mParameters);
        }

        void IAccount.Delete()
        {
            string myStoreProcedure = "ZGWSecurity.Delete_Account";
            SqlParameter[] myParameters = { new SqlParameter("@P_AccountSeqId", m_Profile.Id) };
            base.ExecuteNonQuery(myStoreProcedure, myParameters);
        }

#endregion

#region Private Methods
        private void checkValid()
        {
            base.IsValid();
            if (m_Profile == null)
            {
                throw new DataAccessLayerException("The Profile property must set before using any methods from this class.");
            }
            if (m_SecurityEntitySeqID == -2)
            {
                throw new DataAccessLayerException("The SecurityEntityID property must set before using any methods from this class.");
            }
        }

#endregion        
    }
}
