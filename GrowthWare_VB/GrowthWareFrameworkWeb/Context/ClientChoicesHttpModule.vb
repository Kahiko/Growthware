Imports GrowthWare.Framework.Web
Imports GrowthWare.Framework.Web.Utilities
Imports GrowthWare.Framework.Model.Profiles
Imports System.Web
Imports System.Web.SessionState

Namespace Context
	''' <summary>
	''' The ClientChoicesModule ensures that the client choices are avalible 
	''' to the HTTPCONTEXT
	''' </summary>
	''' <remarks></remarks>
	Public Class ClientChoicesHttpModule
		Implements IHttpModule, IRequiresSessionState

		Private m_SEProfile As MSecurityEntityProfile = SecurityEntityUtility.GetCurrentProfile()

		Public Sub Init(ByVal App As System.Web.HttpApplication) Implements IHttpModule.Init
			AddHandler App.AcquireRequestState, AddressOf Me.AcquireRequestState
			AddHandler App.EndRequest, AddressOf Me.EndRequest
		End Sub

		Public Sub Dispose() Implements IHttpModule.Dispose
		End Sub

		Public Sub AcquireRequestState(ByVal Sender As Object, ByVal E As EventArgs)

			If WebConfigSettings.DBStatus.ToUpper <> GWWebHelper.DBStatusOnline.ToUpper Then Exit Sub
			If HttpContext.Current.Request.Path.ToUpper.IndexOf(".ASPX") = -1 Then Exit Sub
			If HttpContext.Current.Session Is Nothing Then Exit Sub
			Dim mAccountName As String = "Anonymous"
			If HttpContext.Current.Request.IsAuthenticated Then
				mAccountName = HttpContext.Current.User.Identity.Name
			End If
			Dim mClientChoicesState As MClientChoicesState
			Dim mClientChoicesUtility As ClientChoicesUtility = New ClientChoicesUtility()
			mClientChoicesState = mClientChoicesUtility.GetClientChoicesState(mAccountName)
			' Add ClientChoicesState object to the context items for use
			' throughout the application.
			HttpContext.Current.Items(MClientChoices.SessionName) = mClientChoicesState
		End Sub

		Public Sub EndRequest(ByVal Sender As Object, ByVal E As EventArgs)
			If WebConfigSettings.DBStatus.ToUpper = "INSTALL" Then Exit Sub
			'If HttpContext.Current.Request.Path.ToUpper.IndexOf(".ASPX") = -1 Then Exit Sub
			'Save ClientChoicesState back to data store
			Dim mState As MClientChoicesState = CType(HttpContext.Current.Items(MClientChoices.SessionName), MClientChoicesState)
			If (Not mState Is Nothing) Then
				If mState.isDirty Then
					Dim mClientChoicesUtility As New ClientChoicesUtility()
					mClientChoicesUtility.SaveClientChoicesState(mState)
				End If
			End If
		End Sub
	End Class
End Namespace