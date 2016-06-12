Imports System.Globalization
Imports System.Web
Imports GrowthWare.Framework.BusinessLogicLayer
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Web.Controllers

Namespace Utilities
	''' <summary>
	''' ClientChoicesUtility serves as the focal point for any web application needing to utiltize the GrowthWare framework
	''' with regards to ClientChoices.
	''' </summary>
	Public Class ClientChoicesUtility
		Private m_SecurityEntityProfile As MSecurityEntityProfile = Nothing
		Private m_BClientChoices As BClientChoices = Nothing
		Private m_CachedAnonymousChoicesState As String = "AnonymousClientChoicesState"

		''' <summary>
		''' Constructor sets up the private fields
		''' </summary>
		Public Sub New()
			m_SecurityEntityProfile = SecurityEntityUtility.GetDefaultProfile()
			m_BClientChoices = New BClientChoices(m_SecurityEntityProfile, WebConfigSettings.CentralManagement)
		End Sub

		''' <summary>
		''' Returns the client choices given the account
		''' </summary>
		''' <param name="account">String</param>
		''' <returns>MClientChoicesState</returns>
		Public Function GetClientChoicesState(ByVal account) As MClientChoicesState
			Dim mRetVal As MClientChoicesState = Nothing
			If Not HttpContext.Current.Session Is Nothing Then
				mRetVal = CType(HttpContext.Current.Session(MClientChoices.SessionName), MClientChoicesState)
			End If
			If mRetVal Is Nothing Then
				If account.Trim.ToLower(CultureInfo.CurrentCulture) = "anonymous" Then
					mRetVal = CType(HttpContext.Current.Cache(m_CachedAnonymousChoicesState), MClientChoicesState)
					If mRetVal Is Nothing Then
						mRetVal = m_BClientChoices.GetClientChoicesState(account)
						CacheController.AddToCacheDependency(m_CachedAnonymousChoicesState, mRetVal)
					End If
				Else
					If mRetVal.AccountName <> account Then
						mRetVal = m_BClientChoices.GetClientChoicesState(account)
					End If
				End If
			End If
			If Not HttpContext.Current.Session Is Nothing Then
				HttpContext.Current.Session.Add(MClientChoices.SessionName, mRetVal)
			End If
			Return mRetVal
		End Function

		''' <summary>
		''' Save the client choices to the database.
		''' </summary>
		''' <param name="clientChoicesState">MClientChoicesState</param>
		''' <remarks></remarks>
		Public Sub SaveClientChoicesState(ByRef clientChoicesState As MClientChoicesState)
			m_BClientChoices.Save(clientChoicesState)
			If Not HttpContext.Current.Session Is Nothing Then
				HttpContext.Current.Items(MClientChoices.SessionName) = clientChoicesState
			End If
		End Sub
	End Class
End Namespace
