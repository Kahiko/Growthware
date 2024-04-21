Imports GrowthWare.WebSupport.Base
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports System.Web.Services

Public Class AddEditDBInformation
    Inherits BaseWebpage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mProfile As MDBInformation = DBInformationUtility.DBInformation()
        lblVersion.Text = mProfile.Version
        NameValuePairUtility.SetDropSelection(dropEnableInheritance, mProfile.EnableInheritance.ToString())
    End Sub

    <WebMethod(CacheDuration:=0, EnableSession:=False)>
    Public Shared Sub InvokeSave(ByVal enableInheritance As Integer)
        Dim mProfile As MDBInformation = DBInformationUtility.DBInformation()
        mProfile.EnableInheritance = enableInheritance
        If DBInformationUtility.UpdateProfile(mProfile) Then FunctionUtility.RemoveCachedFunctions()
    End Sub

End Class