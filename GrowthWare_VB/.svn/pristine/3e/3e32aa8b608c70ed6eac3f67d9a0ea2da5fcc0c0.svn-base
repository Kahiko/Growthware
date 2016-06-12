Imports System.Data.SqlClient
Imports GrowthWare.Framework.DataAccessLayer.SqlServer.Base
Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.Framework.Model.Profiles

Namespace DataAccessLayer.SqlServer.V2000
	''' <summary>
	''' DAccounts provides all database interaction to SQL Server 2000 to 2005
	''' </summary>
	''' <remarks>
	'''		The Profile and SecurityEntitySeqID properties must be set
	'''		before using any methods.
	'''		Properties where chosen instead of parameters because all
	'''		methods will need one or both to perform their work.
	'''	</remarks>
	Public Class DAccounts
		Inherits DDBInteraction
		Implements IDAccount

#Region "Private Fields"
		Private m_Profile As MAccountProfile = Nothing
		Private m_SecurityEntitySeqID As Integer = -2
#End Region

#Region "Public Properties"
		''' <summary>
		''' Retruns a DataRow of account
		''' </summary>
		''' <returns>DataRow</returns>
		''' <remarks>Usefull for populating MAccountProfile</remarks>
		Protected ReadOnly Property GetAccount() As DataRow Implements IDAccount.GetAccount
			Get
				checkValid()
				Dim mStoredProcedure As String = "ZFP_GET_ACCT"
				Dim mParameters() As SqlParameter =
				{
				 New SqlParameter("@P_IS_SYSTEM_ADMIN", m_Profile.IsSystemAdmin),
				 New SqlParameter("@P_SE_SEQ_ID", m_SecurityEntitySeqID),
				 New SqlParameter("@P_ACCOUNT", m_Profile.Account),
				 MyBase.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
				}
				Return MyBase.GetDataRow(mStoredProcedure, mParameters)
			End Get
		End Property

		''' <summary>
		''' Retruns a table of accounts given IS_SYSTEM_ADMIN, Security Entity
		''' </summary>
		''' <returns>DataTable</returns>
		''' <remarks>IS_SYSTEM_ADMIN is a property of the Profile and Security Entity is the SE_SEQ_ID property</remarks>
		Protected ReadOnly Property GetAccounts() As DataTable Implements IDAccount.GetAccounts
			Get
				checkValid()
				Dim myParameters() As SqlParameter = _
				{ _
				 New SqlParameter("@P_IS_SYSTEM_ADMIN", m_Profile.IsSystemAdmin),
				 New SqlParameter("@P_SE_SEQ_ID", m_SecurityEntitySeqID),
				 New SqlParameter("@P_ACCOUNT", ""),
				 MyBase.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
				}

				Return MyBase.GetDataTable("ZFP_GET_ACCT", myParameters)
			End Get
		End Property

		Protected Property Profile() As MAccountProfile Implements IDAccount.Profile
			Get
				Return m_Profile
			End Get
			Set(ByVal value As MAccountProfile)
				m_Profile = value
			End Set
		End Property

		Public Property SecurityEntitySeqID() As Integer Implements IDAccount.SecurityEntitySeqID
			Get
				Return m_SecurityEntitySeqID
			End Get
			Set(ByVal value As Integer)
				m_SecurityEntitySeqID = value
			End Set
		End Property
#End Region

#Region "Public Methods"
		Protected Sub Delete() Implements IDAccount.Delete
			Dim mStoredProcedure As String = "ZFP_DEL_ACCOUNT"
			Dim mParameters() As SqlParameter =
			 {
			  New SqlParameter("@P_ACCT_SEQ_ID", m_Profile.Id),
			  MyBase.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
			 }
			MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
		End Sub

		Protected Function GetGroups() As DataTable Implements IDAccount.GetGroups
			checkValid()
			Dim mStoredProcedure As String = "ZFP_GET_ACCT_GRPS"
			Dim mParameters() As SqlParameter =
			{ _
			  New SqlParameter("@P_ACCOUNT", m_Profile.Account),
			  New SqlParameter("@P_SE_SEQ_ID", m_SecurityEntitySeqID),
			  MyBase.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
			}
			Return MyBase.GetDataTable(mStoredProcedure, mParameters)
		End Function

		Protected Function GetMenu(ByVal account As String, menuType As MenuType) As DataTable Implements IDAccount.GetMenu
			Dim mStoredProcedure As String = "ZFP_GET_MENU_DATA"
			Dim mParameters As SqlParameter() =
			{
			 New SqlParameter("@P_SE_SEQ_ID", m_SecurityEntitySeqID),
			 New SqlParameter("@P_NAV_TYPE_ID", menuType),
			 New SqlParameter("@P_ACCT", m_Profile.Account),
			 MyBase.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
			}
			Return MyBase.GetDataTable(mStoredProcedure, mParameters)
		End Function

		Protected Function GetRoles() As DataTable Implements IDAccount.GetRoles
			checkValid()
			Dim mStoredProcedure As String = "ZFP_GET_ACCT_RLS"
			Dim mParameters() As SqlParameter =
			{ _
			  New SqlParameter("@P_ACCOUNT", m_Profile.Account),
			  New SqlParameter("@P_SE_SEQ_ID", m_SecurityEntitySeqID),
			  MyBase.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
			}
			Return MyBase.GetDataTable(mStoredProcedure, mParameters)
		End Function

		Protected Function GetSecurity() As DataTable Implements IDAccount.GetSecurity
			checkValid()
			Dim mStoredProcedure As String = "ZFP_GET_ACCT_SECURITY"
			Dim mParameters() As SqlParameter =
			{ _
			  New SqlParameter("@P_ACCT", m_Profile.Account),
			  New SqlParameter("@P_SE_SEQ_ID", m_SecurityEntitySeqID),
			  MyBase.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
			}
			Return MyBase.GetDataTable(mStoredProcedure, mParameters)
		End Function

		''' <summary>
		''' Inserts or updates account information
		''' </summary>
		''' <returns>DataRow</returns>
		''' <remarks>
		''' </remarks>
		Protected Function Save() As Integer Implements IDAccount.Save
			Dim mRetInt As Integer
			Dim mStoredProcedure As String = "ZFP_SET_ACCOUNT"
			Dim mParameters() As SqlParameter = getInsertUpdateSqlParameters()
			Dim mCommaSeporatedString As String = String.Empty
			MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
			mRetInt = Integer.Parse(MyBase.GetParameterValue("@P_ErrorCode", mParameters))
			Return mRetInt
		End Function

		Protected Sub SaveGroups() Implements IDAccount.SaveGroups
			Dim mStoredProcedure As String = "ZFP_SET_ACCT_GRPS"
			Dim mCommaSeporatedString As String = m_Profile.GetCommaSeporatedAssignedGroups()
			Dim mParameters() As SqlParameter = {
			  New SqlParameter("@P_ACCT", m_Profile.Account),
			  New SqlParameter("@P_SE_SEQ_ID", m_SecurityEntitySeqID),
			  New SqlParameter("@P_GROUPS", mCommaSeporatedString),
			  New SqlParameter("@P_ADDUPD_BY", MyBase.GetAddedUpdatedBy(m_Profile)),
			  MyBase.GetSqlParameter("@P_ErrorCode", -1, ParameterDirection.Output)
			 }
			MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
		End Sub

		Protected Sub SaveRoles() Implements IDAccount.SaveRoles
			Dim mStoredProcedure As String = "ZFP_SET_ACCT_RLS"
			Dim mCommaSeporatedString As String = m_Profile.GetCommaSeporatedAssingedRoles()
			Dim mParameters As SqlParameter() = {
			  New SqlParameter("@P_ACCT", m_Profile.Account),
			  New SqlParameter("@P_SE_SEQ_ID", m_SecurityEntitySeqID),
			  New SqlParameter("@P_ROLES", mCommaSeporatedString),
			  New SqlParameter("@P_ADDUPD_BY", MyBase.GetAddedUpdatedBy(m_Profile)),
			  MyBase.GetSqlParameter("@P_ErrorCode", -1, ParameterDirection.Output)
			 }
			MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
		End Sub

#End Region

#Region "Private Methods"
		Private Function getInsertUpdateSqlParameters() As SqlParameter()
			Dim mParameters() As SqlParameter =
			 {
			  New SqlParameter("@P_ACCT_SEQ_ID", m_Profile.Id),
			  New SqlParameter("@P_STATUS_SEQ_ID", m_Profile.Status),
			  New SqlParameter("@P_ACCOUNT", m_Profile.Account),
			  New SqlParameter("@P_FIRST_NAME", m_Profile.FirstName),
			  New SqlParameter("@P_LAST_NAME", m_Profile.LastName),
			  New SqlParameter("@P_MIDDLE_NAME", m_Profile.MiddleName),
			  New SqlParameter("@P_PREFERED_NAME", m_Profile.PreferedName),
			  New SqlParameter("@P_EMAIL", m_Profile.EMail),
			  New SqlParameter("@P_PWD", m_Profile.Password),
			  New SqlParameter("@P_PASSWORD_LAST_SET", m_Profile.PasswordLastSet),
			  New SqlParameter("@P_FAILED_ATTEMPTS", m_Profile.FailedAttempts),
			  New SqlParameter("@P_ADDED_BY", m_Profile.AddedBy),
			  New SqlParameter("@P_ADDED_DATE", m_Profile.AddedDate),
			  New SqlParameter("@P_LAST_LOGIN", m_Profile.LastLogin),
			  New SqlParameter("@P_TIME_ZONE", m_Profile.TimeZone),
			  New SqlParameter("@P_LOCATION", m_Profile.Location),
			  New SqlParameter("@P_ENABLE_NOTIFICATIONS", m_Profile.EnableNotifications),
			  New SqlParameter("@P_IS_SYSTEM_ADMIN", m_Profile.IsSystemAdmin),
			  New SqlParameter("@P_UPDATED_BY", m_Profile.UpdatedBy),
			  New SqlParameter("@P_UPDATED_DATE", m_Profile.UpdatedDate),
			  MyBase.GetSqlParameter("@P_PRIMARY_KEY", -1, ParameterDirection.Output),
			  MyBase.GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
			 }
			Return mParameters
		End Function

		Private Sub checkValid()
			MyBase.IsValid()
			If m_Profile Is Nothing Then
				Throw New ArgumentException("Profile property must be set before calling methods from this class")
			End If
			If m_SecurityEntitySeqID = -2 Then
				Throw New ArgumentException("SE_SEQ_ID property must be set before calling methods from this class")
			End If
		End Sub
#End Region
	End Class
End Namespace