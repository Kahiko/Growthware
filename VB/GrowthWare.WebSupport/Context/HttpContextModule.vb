Imports System.Web
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Common

Namespace Context
    ''' <summary>
    ''' HttpContext entry point
    ''' </summary>
    ''' <remarks>Will initiate the ClientChoicesHttpModule</remarks>
    Public Class HttpContextModule
        Implements IHttpModule

        Private m_DisposedValue As Boolean ' To detect redundant calls
        Private m_Filter As OutputFilterStream

        ''' <summary>
        ''' Implements IDispose
        ''' </summary>
        ''' <param name="disposing">Boolean</param>
        ''' <remarks></remarks>
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.m_DisposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If
            End If
            Me.m_DisposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        Protected Overrides Sub Finalize()
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(False)
            MyBase.Finalize()
        End Sub

        ''' <summary>
        ''' Implements Dispose
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Dispose() Implements IHttpModule.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        ''' <summary>
        ''' Initializes the HTTP module
        ''' </summary>
        ''' <param name="context">as HttpApplication</param>
        ''' <remarks>
        ''' Servers as a wrapper to the ClientChoicesHttpModule as well as 
        ''' placing the current function, security profiles into the context.
        ''' </remarks>
        Public Sub Init(ByVal context As System.Web.HttpApplication) Implements System.Web.IHttpModule.Init
            If Not context Is Nothing Then
                'Dim mInitClientChoices As New ClientChoicesHttpModule()
                'mInitClientChoices.Init(context)
                AddHandler context.Context.ApplicationInstance.Error, AddressOf Me.onApplicationError
                AddHandler context.BeginRequest, AddressOf Me.onBeginRequest
                AddHandler context.AcquireRequestState, AddressOf Me.onAcquireRequestState
                AddHandler context.EndRequest, AddressOf Me.onEndRequest
            End If
        End Sub

        ''' <summary>
        ''' Ons the application error.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        Private Sub onApplicationError(ByVal sender As Object, ByVal e As EventArgs)
            Dim mEx As Exception = HttpContext.Current.Server.GetLastError
            If mEx IsNot Nothing Then

            End If
        End Sub

        ''' <summary>
        ''' Ons the state of the acquire request.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        Private Sub onAcquireRequestState(ByVal sender As Object, ByVal e As EventArgs)
            Dim mLog As Logger = Logger.Instance()
            mLog.Debug("onAcquireRequestState():: Started")
            mLog.Debug("onAcquireRequestState():: Done")
        End Sub

        ''' <summary>
        ''' Ons the begin request.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        Private Sub onBeginRequest(ByVal sender As Object, ByVal e As EventArgs)
            If processRequest() Then
                m_Filter = New OutputFilterStream(HttpContext.Current.Response.Filter)
                HttpContext.Current.Response.Filter = m_Filter
            End If
        End Sub

        ''' <summary>
        ''' Ons the end request.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        ''' <exception cref="System.Exception"></exception>
        Private Sub onEndRequest(ByVal sender As Object, ByVal e As EventArgs)
            If processRequest() Then
                Dim mContext As HttpContext = TryCast(sender, HttpApplication).Context
                Dim mSendError As Boolean = False
                Try
                    If mContext.Response.Headers("jsonerror") IsNot Nothing Then
                        Dim mError As String = String.Empty
                        If m_Filter IsNot Nothing Then
                            mError = m_Filter.ReadStream()
                            If mContext.Response.Headers("jsonerror").ToString().ToLower(CultureInfo.InvariantCulture).Trim() = "true" Then
                                mSendError = True
                                formatError(mError)
                                Throw (New Exception([String].Concat("An AJAX error has occurred: ", System.Environment.NewLine, mError)))
                            End If
                        Else
                            If mContext.Response.Headers("jsonerror").ToString().ToLower().Trim() = "true" Then
                                mSendError = True
                                Throw (New Exception([String].Concat("An AJAX error has occurred: ", System.Environment.NewLine)))
                            End If
                        End If
                    End If
                Catch ex As Exception
                    If mSendError Then
                        Try
                            If Not ex.ToString().Contains("Invalid JSON primitive") Then
                                Dim mLog As Logger = Logger.Instance()
                                mLog.Error(ex)
                            End If
                            Dim mCurrentResponse As HttpResponse = mContext.Response
                            mCurrentResponse.Clear()
                            mCurrentResponse.Write("{""Message"":""We are very sorry but an error has occurred, please try your request again.""}")
                            mCurrentResponse.ContentType = "text/html"
                            mCurrentResponse.StatusDescription = "500 Internal Error"
                            mCurrentResponse.StatusCode = 500
                            mCurrentResponse.TrySkipIisCustomErrors = True
                            mCurrentResponse.Flush()
                            HttpContext.Current.Server.ClearError()
                            HttpContext.Current.ApplicationInstance.CompleteRequest()
                        Catch
                        End Try
                    End If
                Finally
                    If m_Filter IsNot Nothing Then
                        m_Filter.Dispose()
                        m_Filter = Nothing
                    End If
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Chars the match.
        ''' </summary>
        ''' <param name="match">The match.</param>
        ''' <returns>System.String.</returns>
        Private Function charMatch(ByVal match As Match) As String
            Dim code = match.Groups("code").Value
            Dim value As Integer = Convert.ToInt32(code, 16)
            Return (ChrW(value)).ToString()
        End Function

        ''' <summary>
        ''' Formats the error.
        ''' </summary>
        ''' <param name="message">The message.</param>
        Private Sub formatError(ByRef message As String)
            'http://stackoverflow.com/questions/6990347/how-to-decode-u0026-in-a-url
            message = message.Replace("\r\n", System.Environment.NewLine)
            message = HttpUtility.UrlDecode(message)
            message = Regex.Replace(message, "\\u(?<code>\d{4})", AddressOf charMatch)
        End Sub

        ''' <summary>
        ''' Determines if the request if one of "ASPX", "ASHX", OR "ASMX"
        ''' </summary>
        ''' <returns>boolean</returns>
        ''' <remarks>There's no need to process logic for the other file types or extension</remarks>
        Private Function processRequest() As Boolean
            Dim mRetval As Boolean = False
            If Not HttpContext.Current Is Nothing Then
                Dim mPath As String = HttpContext.Current.Request.Path.ToUpper(New CultureInfo("en-US", False))
                Dim mFileExtension = mPath.Substring(mPath.LastIndexOf(".") + 1)
                Dim mProcessingTypes As String() = {"ASPX", "ASHX", "ASMX"}
                If mProcessingTypes.Contains(mFileExtension) Or mPath.IndexOf("/API/") > -1 Then
                    mRetval = True
                End If
            End If
            Return mRetval
        End Function
    End Class

End Namespace
