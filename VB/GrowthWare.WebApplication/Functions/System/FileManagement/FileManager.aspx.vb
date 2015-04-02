Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports System.Web.Services
Imports System.IO

Public Class FileManager
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mFunctionProfile As MFunctionProfile = FunctionUtility.CurrentProfile()
        Dim mScript As String = "<script type='text/javascript' language='javascript'>GW.FileManager.currentDirectory = '/'; GW.FileManager.currentFunctionSeqID=" + mFunctionProfile.Id.ToString() + "</script>"
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "", mScript)
    End Sub

    <WebMethod(CacheDuration:=0, EnableSession:=False)>
    Public Shared Function GetDirectoryLinks(ByVal currentDirectoryString As String, functionSeqId As Integer)
        Dim context As HttpContext = HttpContext.Current
        Dim mStringBuilder As StringBuilder = New StringBuilder()
        Dim mStringWriter As StringWriter = New StringWriter(mStringBuilder)
        Dim mWriter As HtmlTextWriter = New HtmlTextWriter(mStringWriter)
        Dim mPath As String = String.Empty
        Dim mCurrentDirectory As String = "/"

        Dim mFirstLink As HyperLink = New HyperLink()
        mFirstLink.Attributes.Add("href", "#")
        mFirstLink.Attributes.Add("onclick", String.Format("javascript:GW.FileManager.changeDirectory('{0}','{1}')", mCurrentDirectory, functionSeqId))
        mFirstLink.Text = "Home\"
        mFirstLink.RenderControl(mWriter)
        mCurrentDirectory = context.Server.UrlDecode(currentDirectoryString)
        If context.Server.UrlDecode(mCurrentDirectory).Length > 1 Then
            Dim mArray As Array = mCurrentDirectory.Split("/")
            For Each item As String In mArray
                If item.Length > 0 Then
                    mPath += "/" + item
                    Dim mLink As HyperLink = New HyperLink()
                    mLink.Attributes.Add("href", "#")
                    mLink.Attributes.Add("onclick", String.Format("javascript:GW.FileManager.changeDirectory('{0}','{1}')", mPath, functionSeqId))
                    mLink.Text = item + "\"
                    mLink.RenderControl(mWriter)
                End If

            Next

        End If
        Return mStringBuilder.ToString()
    End Function
End Class