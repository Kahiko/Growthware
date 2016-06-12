Imports System
Imports System.Web
Imports System.Web.Security

Public Class BasePage
	Inherits System.Web.UI.Page

	'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
	' fields
	'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

	' Setup the name of the hidden field on the client for storing the viewstate key, 
	Private Const VIEW_STATE_FIELD_NAME As String = "__vi"

	' Set the number of latest pages' ViewState data to keep
	' can also come from a Web.Config setting value, use a few more CPU cycles  
	Private Const VIEW_STATE_NUM_PAGES As Integer = 16

	'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
	'methods
	'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

	Public Sub New()
	End Sub


	Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
		If Convert.ToBoolean(ConfigurationSettings.AppSettings.Item("ServerSideViewState")) Then
			Return Me.LoadViewState
		End If
		Return MyBase.LoadPageStateFromPersistenceMedium
	End Function


	Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
		If Convert.ToBoolean(ConfigurationSettings.AppSettings.Item("ServerSideViewState")) Then
			Me.SaveViewState(viewState)
			MyBase.SavePageStateToPersistenceMedium("")
		Else
			MyBase.SavePageStateToPersistenceMedium(viewState)
		End If
	End Sub

	Private Shadows Function LoadViewState() As Object
		Dim text1 As String = ""
		Dim obj1 As Object = Nothing
		Try
			text1 = MyBase.Request.Form.Item("__vi")
			HttpContext.Current.Session.Item("__RequestNumber") = Integer.Parse(text1)
			If (Not HttpContext.Current.Session.Item(("__vi" & text1)) Is Nothing) Then
				obj1 = Me.Session.Item(("__vi" & text1))
			End If
		Catch obj3 As Exception
		End Try
		Return obj1
	End Function


	Private Shadows Sub SaveViewState(ByVal viewState As Object)
		Dim num1 As Integer = 0
		If (Not HttpContext.Current.Session.Item("__RequestNumber") Is Nothing) Then
			num1 = (CInt(HttpContext.Current.Session.Item("__RequestNumber")) + 1)
			If (6 = num1) Then
				num1 = 0
			End If
		End If
		HttpContext.Current.Session.Item("__RequestNumber") = num1
		Me.Session.Item(("__vi" & num1.ToString)) = viewState
		Me.RegisterHiddenField("__vi", num1.ToString)
	End Sub

	Protected Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		Dim Action As String = Request.QueryString("Action")
		If Not Action Is Nothing Then
			Select Case Action.Trim.ToLower
				Case "logon", "logoff", "generichome"
				Case Else
					checkSession()
			End Select
		Else
			checkSession()
		End If
	End Sub

	Private Sub checkSession()
		If Not HttpContext.Current.Session Is Nothing Then
			If HttpContext.Current.Session.IsNewSession Then
				Dim szCookieHeader As String = Request.Headers("Cookie")
				If Not szCookieHeader Is Nothing And szCookieHeader.IndexOf("ASP.NET_SessionId") >= 0 Then
					'remove all sesssion information
					HttpContext.Current.Session.RemoveAll()
					' re-create the context item for the account info
					FormsAuthentication.SignOut()
					Response.Redirect("~/Pages/System/TimeOut.aspx")
				Else
					HttpContext.Current.Session.RemoveAll()
					FormsAuthentication.SignOut()
					NavControler.NavTo("generichome")
				End If
			End If
		End If
	End Sub
End Class