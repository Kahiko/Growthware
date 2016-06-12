Imports BLL.Base.ClientChoices
Imports Common.Cache
Imports DALModel.Special.ClientChoices
Imports System
Imports System.Web
Imports System.Collections
Imports System.Collections.Specialized


Namespace ClientChoices
    Public Class ClientChoicesModule
        Implements IHttpModule, SessionState.IRequiresSessionState
#Region " Notes "
#End Region

        Private cachedAnonymousChoicesState As String = "AnonymousClientChoicesState"

        Public Sub Init(ByVal App As System.Web.HttpApplication) Implements IHttpModule.Init
            AddHandler App.PreRequestHandlerExecute, AddressOf Me.PreRequestHandlerExecute
            AddHandler App.EndRequest, AddressOf Me.EndRequest
        End Sub

        Public Sub Dispose() Implements IHttpModule.Dispose
        End Sub

        Public Sub PreRequestHandlerExecute(ByVal Sender As Object, ByVal E As EventArgs)
            Dim AccountName As String = "Anonymous"
            If HttpContext.Current.Request.IsAuthenticated Then
                AccountName = HttpContext.Current.User.Identity.Name
            End If
            Dim clientChoicesState As BClientChoicesState
            If HttpContext.Current.Session Is Nothing Then
                HttpContext.Current.Items(MClientChoices.sessionName) = New BClientChoicesState(AccountName)
                Exit Sub
            End If
            Dim sessionClientChoicesState As BClientChoicesState
            sessionClientChoicesState = CType(HttpContext.Current.Session(MClientChoices.sessionName), BClientChoicesState)
            If AccountName.Trim.ToLower = "anonymous" Then
                sessionClientChoicesState = CType(HttpContext.Current.Cache(cachedAnonymousChoicesState), BClientChoicesState)
                If sessionClientChoicesState Is Nothing Then
                    sessionClientChoicesState = New BClientChoicesState(AccountName)
                    CacheControler.AddToCacheDependency(cachedAnonymousChoicesState, sessionClientChoicesState)
                End If
            End If
            If sessionClientChoicesState Is Nothing Then
                clientChoicesState = New BClientChoicesState(AccountName)
                HttpContext.Current.Session(MClientChoices.sessionName) = clientChoicesState
            Else
                If AccountName.ToLower <> sessionClientChoicesState(MClientChoices.AccountName).Trim.ToLower Then
                    clientChoicesState = New BClientChoicesState(AccountName)
                    HttpContext.Current.Session(MClientChoices.sessionName) = clientChoicesState
                Else
                    clientChoicesState = sessionClientChoicesState
                End If
            End If
            ' Add ClientChoicesState object to the context items for use
            ' throughout the application.
            HttpContext.Current.Items(MClientChoices.sessionName) = clientChoicesState
        End Sub

        Public Sub EndRequest(ByVal Sender As Object, ByVal E As EventArgs)
            'Save ClientChoicesState back to data store
            Dim hshTable As NameValueCollection = CType(HttpContext.Current.GetConfig("appSettings"), NameValueCollection)
            Dim myState As BClientChoicesState = CType(HttpContext.Current.Items(MClientChoices.sessionName), BClientChoicesState)
            If (Not myState Is Nothing) Then
                myState.Save()
            End If
        End Sub
    End Class
End Namespace