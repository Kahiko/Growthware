Imports System.Collections.ObjectModel
Imports System.Web
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Web.Controllers
Imports GrowthWare.Framework.BusinessLogicLayer

Namespace Utilities

	''' <summary>
	''' SecurityEntityUtility serves as the focal point for any web application needing to utiltize the GrowthWare framework.
	''' Web needs such as caching are handeled here
	''' </summary>
	Public Class SecurityEntityUtility

		Private Shared m_ProfileContextName As String = "ContextSecurityEntityProfile"
		Private Shared m_DefaultProfile As MSecurityEntityProfile = Nothing
		Private Shared m_BSecurityEntity As BSecurityEntity = Nothing

		'Sub New()
		'	m_DefaultProfile = GetDefaultProfile()
		'	m_BSecurityEntity = New BSecurityEntity(m_DefaultProfile, WebConfigSettings.CentralManagement)
		'End Sub

		''' <summary>
		''' Creates and returns MSecurityEntityProfile populated with information from the
		''' configuration file.
		''' </summary>
		''' <returns>MSecurityEntityProfile</returns>
		Public Shared Function GetDefaultProfile() As MSecurityEntityProfile
			If m_DefaultProfile Is Nothing Then
				Dim mDefaultProfile As MSecurityEntityProfile = New MSecurityEntityProfile()
				mDefaultProfile.Id = ConfigSettings.DefaultSecurityEntityID
				mDefaultProfile.DAL = ConfigSettings.DAL
				mDefaultProfile.DALNamespace = ConfigSettings.DAL_Namespace(mDefaultProfile.DAL)
				mDefaultProfile.DALAssemblyName = ConfigSettings.DAL_AssemblyName(mDefaultProfile.DAL)
				mDefaultProfile.ConnectionString = ConfigSettings.ConnectionString(mDefaultProfile.DAL)
				m_DefaultProfile = mDefaultProfile
			End If
			Return m_DefaultProfile
		End Function

		''' <summary>
		''' Returns the current MSecurityEntityProfile from context.  If one is not found in context then 
		''' the default values from the config file will be returned.
		''' </summary>
		''' <returns>MSecurityEntityProfile</returns>
		Public Shared Function GetCurrentProfile() As MSecurityEntityProfile
			Dim mAccount As String = String.Empty
			If Not HttpContext.Current Is Nothing AndAlso Not HttpContext.Current.User Is Nothing Then
				mAccount = HttpContext.Current.User.Identity.Name
			End If
			If mAccount Is Nothing Or mAccount = String.Empty Then
				mAccount = "Anonymous"
			End If
			Dim mClientChoicesUtility As ClientChoicesUtility = New ClientChoicesUtility()
			Dim mClientChoicesState As MClientChoicesState = mClientChoicesUtility.GetClientChoicesState(mAccount)
			Dim mCurrentSecurityEntityID As Integer = mClientChoicesState(MClientChoices.SecurityEntityID)
			Dim mProfiles As Collection(Of MSecurityEntityProfile)
			mProfiles = GetProfiles()

			Dim mResult = From mProfile In mProfiles Where mProfile.Id = mCurrentSecurityEntityID Select mProfile
			Dim mRetVal As MSecurityEntityProfile = mResult.First
			If mRetVal Is Nothing Then mRetVal = GetDefaultProfile()
			Return mRetVal
		End Function

		Public Shared Function GetProfiles() As Collection(Of MSecurityEntityProfile)
			Dim mCacheName As String = "SecurityEntityProfiles"
			Dim mSecurityEntityCollection As Collection(Of MSecurityEntityProfile) = Nothing
			mSecurityEntityCollection = CType(HttpContext.Current.Cache(mCacheName), Collection(Of MSecurityEntityProfile))
			If mSecurityEntityCollection Is Nothing Then
				If m_BSecurityEntity Is Nothing Then
					m_BSecurityEntity = New BSecurityEntity(GetDefaultProfile(), ConfigSettings.CentralManagement)
				End If
				mSecurityEntityCollection = m_BSecurityEntity.GetSecurityEntities()
				CacheController.AddToCacheDependency(mCacheName, mSecurityEntityCollection)
			End If
			Return mSecurityEntityCollection
		End Function
	End Class
End Namespace
