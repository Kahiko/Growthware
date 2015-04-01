Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities

Public Class FileManager
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mFunctionProfile As MFunctionProfile = FunctionUtility.CurrentProfile()
        Dim mScript As String = "<script type='text/javascript' language='javascript'>GW.FileManager.currentDirectory = '/'; GW.FileManager.currentFunctionSeqID=" + mFunctionProfile.Id.ToString() + "</script>"
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "", mScript)
    End Sub

End Class