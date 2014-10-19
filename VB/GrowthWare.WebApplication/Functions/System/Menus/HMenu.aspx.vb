Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Model.Enumerations

Public Class HMenu
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mAccount As String = AccountUtility.HttpContextUserName()
        Dim myDataTable As DataTable = AccountUtility.GetMenu(mAccount, MenuType.Horizontal)
        NavigationTrail.DataSource = myDataTable.DefaultView
        NavigationTrail.DataBind()
    End Sub

End Class