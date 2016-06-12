Imports System.Collections.ObjectModel
Imports System.Web
Imports GrowthWare.Framework.BusinessLogicLayer
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Web.Controllers

Namespace Utilities

	''' <summary>
	''' AccountUtility serves as the focal point for any web application needing to utiltize the GrowthWare framework.
	''' Web needs such as caching are handeled here.
	''' </summary>
	Public NotInheritable Class AccountUtility

		'Private m_SecutiryEntityCacheKey As String = "SecurityEntity_XX_Account"
		Private m_SecurityEntityProfile As MSecurityEntityProfile = Nothing
		Private m_BAccount As BAccounts = Nothing
		Private m_CachedAnonymousAccount As String = "AnonymousProfile"
		Private m_AnonymousAccount As String = "Anonymous"

		''' <summary>
		''' Constructor sets up the private fields
		''' </summary>
		Public Sub New()
			m_SecurityEntityProfile = SecurityEntityUtility.GetCurrentProfile()
			m_BAccount = New BAccounts(m_SecurityEntityProfile, ConfigSettings.CentralManagement)
		End Sub

		''' <summary>
		''' Retruns a collection of MAccountProfiles given an MAccountProfile and the current SecurityEntitySeqID
		''' </summary>
		''' <param name="profile">MAccountProfile</param>
		''' <remarks>If the Profiles.IsSysAdmin is true then all accounts will be returned</remarks>
		Public Function GetAccounts(ByVal profile As MAccountProfile) As Collection(Of MAccountProfile)
			' was thinking of adding cache here but
			' when you think of it it's not needed
			' account information needs to come from
			' the db to help ensure passwords are correct and what not.
			' besides which a list of accounts is only necessary
			' when editing an account and it that case
			' what accounts that are returned are dependend on the requesting account.IsSysAdmin bit.
			Return m_BAccount.GetAccounts(profile)
		End Function

		''' <summary>
		''' Retrieves the current profile.
		''' </summary>
		''' <returns>MAccountProfile</returns>
		''' <remarks>If context does not contain a referance to an account anonymous will be returned</remarks>
		Public Function GetCurrentProfile() As MAccountProfile
			Dim mRetProfile As MAccountProfile = Nothing
			Dim mAccountName As String = HttpContext.Current.User.Identity.Name
			If mAccountName = String.Empty Then mAccountName = m_AnonymousAccount
			If mAccountName.Trim() = m_AnonymousAccount Then
				If Not HttpContext.Current.Cache Is Nothing Then
					mRetProfile = CType(HttpContext.Current.Cache(m_CachedAnonymousAccount), MAccountProfile)
					If mRetProfile Is Nothing Then
						mRetProfile = Me.GetProfile(mAccountName)
						CacheController.AddToCacheDependency(m_CachedAnonymousAccount, mRetProfile)
					End If
				End If
			End If
			If mRetProfile Is Nothing Then
				mRetProfile = CType(HttpContext.Current.Session(mAccountName), MAccountProfile)
				If mRetProfile Is Nothing Then
					mRetProfile = Me.GetProfile(mAccountName)
					HttpContext.Current.Session.Add(mAccountName + "_Session", mRetProfile)
				End If
			End If
			Return mRetProfile
		End Function

		''' <summary>
		''' Retrieves an account profile given the account
		''' </summary>
		''' <param name="account">String</param>
		''' <returns>a populated MAccountProfile</returns>
		Public Function GetProfile(ByVal account As String) As MAccountProfile
			Return m_BAccount.GetProfile(account)
		End Function

		''' <summary>
		''' Inserts or updates account information
		''' </summary>
		''' <param name="profile">MAccountProfile</param>
		''' <param name="saveRoles">Boolean</param>
		''' <param name="saveGroups">Boolean</param>
		''' <remarks>Changes will be reflected in the profile passed as a reference.</remarks>
		Public Function Save(ByVal profile As MAccountProfile, ByVal saveRoles As Boolean, ByVal saveGroups As Boolean)
			m_BAccount.Save(profile, saveRoles, saveGroups)
			Return profile
		End Function

	End Class

End Namespace
