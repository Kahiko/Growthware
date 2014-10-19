Imports GrowthWare.Framework.Model.Profiles
Imports System.Web.UI
Imports System.IO
Imports System.Web
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Model
Imports GrowthWare.Framework.Common
Imports System.Globalization

Namespace BasePages
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
        ''' <param name="state"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal state As Object)
            If Convert.ToBoolean(ConfigSettings.ServerSideViewState) Then
                Me.SaveViewState(state)
                MyBase.SavePageStateToPersistenceMedium("")
            Else
                MyBase.SavePageStateToPersistenceMedium(state)
            End If
        End Sub

        ''' <summary>
        ''' Overloads LoadViewState
        ''' </summary>
        ''' <returns>Object</returns>
        ''' <remarks></remarks>
        Protected Overloads Function LoadViewState() As Object
            Dim text1 As String = ""
            Dim mViewState As Object = Nothing
            Try
                text1 = MyBase.Request.Form.Item(VIEW_STATE_FIELD_NAME)
                HttpContext.Current.Session.Item(REQUEST_NUMBER) = Integer.Parse(text1, CultureInfo.InvariantCulture)
                If (Not HttpContext.Current.Session.Item((VIEW_STATE_FIELD_NAME & text1)) Is Nothing) Then
                    mViewState = Deserialize(CType(Me.Session.Item((VIEW_STATE_FIELD_NAME & text1)), Byte()))
                End If
            Catch obj3 As Exception
                Throw
            End Try
            Return mViewState
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
            Me.Session.Item(("__vi" & num1.ToString(CultureInfo.InvariantCulture))) = Serialize(viewState)
            Me.ClientScript.RegisterHiddenField(VIEW_STATE_FIELD_NAME, num1.ToString(CultureInfo.InvariantCulture))
        End Sub

        ''' <summary>
        ''' Seralizes the view state
        ''' </summary>
        ''' <param name="obj">Object</param>
        ''' <returns>Byte array</returns>
        ''' <remarks></remarks>
        Private Shared Function Serialize(ByVal obj As Object) As Byte()
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
        Private Shared Function Deserialize(ByVal bytes() As Byte) As Object
            If bytes Is Nothing Then Throw New ArgumentNullException("bytes", "bytes cannot be a null reference (Nothing in Visual Basic)!")
            Dim functionReturnValue As Object = Nothing
            Dim ms As MemoryStream = Nothing
            Dim formater As LosFormatter = Nothing
            Try
                ms = New MemoryStream(bytes)
                formater = New LosFormatter
                functionReturnValue = formater.Deserialize(ms)
            Catch ex As Exception
                Throw
            Finally
                If Not ms Is Nothing Then ms.Close()
                If Not formater Is Nothing Then formater = Nothing
            End Try
            Return functionReturnValue
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

End Namespace
