Imports System.Web
Imports System.Web.Services
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Utilities

Public Class DownloadHandler
    Implements System.Web.IHttpHandler

    ''' <summary>
    ''' Enables processing of HTTP Web requests for downloading files that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
    ''' </summary>
    ''' <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim mFilename As String = GWWebHelper.GetQueryValue(context.Request, "fileName")
        Dim mPath As String = GWWebHelper.GetQueryValue(context.Request, "thePath")
        Dim mFunctionSeqIDString As String = GWWebHelper.GetQueryValue(context.Request, "functionSeqID")
        If Not String.IsNullOrEmpty(mFilename) And Not String.IsNullOrEmpty(mFunctionSeqIDString) And Not String.IsNullOrEmpty(mPath) Then
            Dim mFunctionSeqID As Integer = Integer.Parse(mFunctionSeqIDString)
            Dim mDirectoryProfile As MDirectoryProfile = DirectoryUtility.GetProfile(mFunctionSeqID)
            context.Response.ContentType = "application/octet-stream"
            context.Response.AddHeader("Content-Disposition", String.Format("attachment; filename=" + """{0}" + """", mFilename))
            context.Response.WriteFile(mDirectoryProfile.Directory + mPath + "/" + mFilename)
        Else
            context.Response.ContentType = "text/plain"
            context.Response.Write("Invalid filename")
        End If

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class