Imports GrowthWare.Framework.Model.Profiles
Imports System.Web.UI
Imports System.IO
Imports System.Web
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Model
Imports GrowthWare.Framework.Common

Public Class BaseWebpage
    Inherits System.Web.UI.Page

    Private Const VIEW_STATE_FIELD_NAME As String = "__vi"
    Private Const REQUEST_NUMBER As String = "__RequestNumber"
    Private VIEW_STATE_NUM_PAGES As Integer = ConfigSettings.ServerSideViewStatePages


    Public Sub New()
    End Sub

    ''' <summary>
    ''' Overrides System.Web.UI.PageLoadPageStateFromPersistenceMedium
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        If Convert.ToBoolean(ConfigSettings.ServerSideViewState) Then
            Return Me.LoadViewState
        End If
        Return MyBase.LoadPageStateFromPersistenceMedium
    End Function

    ''' <summary>
    ''' Overrides SavePageStateToPersistenceMedium if needed
    ''' </summary>
    ''' <param name="viewState"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        If Convert.ToBoolean(ConfigSettings.ServerSideViewState) Then
            Me.SaveViewState(viewState)
            MyBase.SavePageStateToPersistenceMedium("")
        Else
            MyBase.SavePageStateToPersistenceMedium(viewState)
        End If
    End Sub

    ''' <summary>
    ''' Overloads LoadViewState
    ''' </summary>
    ''' <returns>Object</returns>
    ''' <remarks></remarks>
    Protected Overloads Function LoadViewState() As Object
        Dim text1 As String = ""
        LoadViewState = Nothing
        Try
            text1 = MyBase.Request.Form.Item(VIEW_STATE_FIELD_NAME)
            HttpContext.Current.Session.Item(REQUEST_NUMBER) = Integer.Parse(text1)
            If (Not HttpContext.Current.Session.Item((VIEW_STATE_FIELD_NAME & text1)) Is Nothing) Then
                LoadViewState = Deserialize(CType(Me.Session.Item((VIEW_STATE_FIELD_NAME & text1)), Byte()))
            End If
        Catch obj3 As Exception
        End Try
    End Function

    ''' <summary>
    ''' Overloads SaveViewState
    ''' </summary>
    ''' <param name="viewState">Object</param>
    ''' <remarks></remarks>
    Protected Overloads Sub SaveViewState(ByVal viewState As Object)
        Dim num1 As Integer = 0
        If (Not HttpContext.Current.Session.Item(REQUEST_NUMBER) Is Nothing) Then
            num1 = (CInt(HttpContext.Current.Session.Item(REQUEST_NUMBER)) + 1)
            If (VIEW_STATE_NUM_PAGES = num1) Then
                num1 = 0
            End If
        End If
        HttpContext.Current.Session.Item(REQUEST_NUMBER) = num1
        Me.Session.Item(("__vi" & num1.ToString)) = Serialize(viewState)
        Me.ClientScript.RegisterHiddenField(VIEW_STATE_FIELD_NAME, num1.ToString)
    End Sub

    ''' <summary>
    ''' Seralizes the view state
    ''' </summary>
    ''' <param name="obj">Object</param>
    ''' <returns>Byte array</returns>
    ''' <remarks></remarks>
    Private Function Serialize(ByVal obj As Object) As Byte()
        Dim functionReturnValue As Byte() = Nothing
        Dim ms As MemoryStream = Nothing
        Dim formater As LosFormatter = Nothing
        Try
            ms = New MemoryStream()
            formater = New LosFormatter()
            formater.Serialize(ms, obj)
            functionReturnValue = ms.ToArray()
        Catch
            Throw New WebSupportException("Could not Serialize the object")
        Finally
            If Not ms Is Nothing Then ms.Close()
            If Not formater Is Nothing Then formater = Nothing
        End Try
        Return functionReturnValue
    End Function

    ''' <summary>
    ''' Deserializes view state
    ''' </summary>
    ''' <param name="bytes">Byte</param>
    ''' <returns>Object</returns>
    ''' <remarks></remarks>
    Private Function Deserialize(ByVal bytes() As Byte) As Object
        Dim ms As New MemoryStream(bytes)
        Dim formater As New LosFormatter
        Deserialize = formater.Deserialize(ms)
        ms.Close()
        ms.Dispose()
        formater = Nothing
    End Function

    ''' <summary>
    ''' Handles the PreInit event of the Page control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Dim myFunction As MFunctionProfile = FunctionUtility.CurrentProfile()
        If Not myFunction Is Nothing Then
            Me.EnableViewState = myFunction.EnableViewState
        End If
    End Sub

End Class
