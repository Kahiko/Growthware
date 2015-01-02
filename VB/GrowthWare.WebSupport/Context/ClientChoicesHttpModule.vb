Imports System.Web
Imports System.Web.SessionState
Imports System.Globalization
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities

Namespace Context
    ''' <summary>
    ''' The ClientChoicesModule ensures that the client choices are avalible 
    ''' to the HTTPCONTEXT
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ClientChoicesHttpModule
        Implements IHttpModule, IRequiresSessionState

        Private m_Disposing As Boolean = False

        ''' <summary>
        ''' Initializes the ClientChoicesHttpModule subscribing to HttpModule events.
        ''' </summary>
        ''' <param name="context"></param>
        Public Sub Init(ByVal context As HttpApplication) Implements IHttpModule.Init
            If context Is Nothing Then Throw New ArgumentNullException("context", "context cannot be a null reference (Nothing in Visual Basic)!")
            AddHandler context.AcquireRequestState, AddressOf Me.AcquireRequestState
            AddHandler context.EndRequest, AddressOf Me.EndRequest
        End Sub

        ''' <summary>
        ''' Implements dispose required by IHttpModule
        ''' </summary>
        Public Sub Dispose() Implements IHttpModule.Dispose
            If Not m_Disposing Then
                m_Disposing = True
            End If
        End Sub

        ''' <summary>
        ''' Keeps the MClientChoicesState in context.
        ''' </summary>
        ''' <param name="sender">object</param>
        ''' <param name="eventArgs">EventArgs</param>
        Public Sub AcquireRequestState(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Dim mAccountName As String = AccountUtility.HttpContextUserName()
            Dim mClientChoicesState As MClientChoicesState = ClientChoicesUtility.GetClientChoicesState(mAccountName)
            HttpContext.Current.Items(MClientChoices.SessionName) = mClientChoicesState
            'If HttpContext.Current.Session IsNot Nothing Then
            '	Dim mAccountName As String = AccountUtility.GetHttpContextUserName()
            '	Dim mClientChoicesState As MClientChoicesState = ClientChoicesUtility.GetClientChoicesState(mAccountName)
            '	HttpContext.Current.Items(MClientChoices.SessionName) = mClientChoicesState
            'End If
        End Sub

        ''' <summary>
        ''' Saves changes to MClientChoicesState to the database.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="eventArgs"></param>
        Public Sub EndRequest(ByVal sender As Object, ByVal eventArgs As EventArgs)
            If ConfigSettings.DBStatus.ToUpper(CultureInfo.InvariantCulture) = "INSTALL" Then Exit Sub
            If processRequest() Then
                Dim mState As MClientChoicesState = CType(HttpContext.Current.Items(MClientChoices.SessionName), MClientChoicesState)
                If mState IsNot Nothing Then
                    If mState.IsDirty Then
                        ClientChoicesUtility.Save(mState)
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Determines if the request if one of "ASPX", "ASCX", "ASHX", OR "ASMX"
        ''' </summary>
        ''' <returns>boolean</returns>
        ''' <remarks>There's no need to process logic for the other file types or extension</remarks>
        Private Shared Function processRequest() As Boolean
            Dim mRetval As Boolean = False
            Dim mLogger As Logger = Logger.Instance()
            If Not HttpContext.Current Is Nothing Then
                Dim mPath As String = HttpContext.Current.Request.Path.ToUpper(CultureInfo.InvariantCulture)
                Dim mFileExtension = mPath.Substring(mPath.LastIndexOf(".", StringComparison.OrdinalIgnoreCase) + 1)
                Dim mProcessingTypes As String() = {"ASPX", "ASHX", "ASMX"}
                mLogger.Debug("mPath: " + mPath.ToString())
                mLogger.Debug("Processing types: " + mProcessingTypes.ToString() + System.Environment.NewLine + " mFileExtension: " + mFileExtension.ToString() + " mPath.IndexOf(/API/, StringComparison.OrdinalIgnoreCase): " + mPath.IndexOf("/API/", StringComparison.OrdinalIgnoreCase).ToString())
                If mProcessingTypes.Contains(mFileExtension) Or mPath.IndexOf("/API/", StringComparison.OrdinalIgnoreCase) > -1 Then
                    mRetval = True
                End If
            Else
                mLogger.Info("HttpContext.Current is null can not process")
            End If
            Return mRetval
        End Function
    End Class
End Namespace
