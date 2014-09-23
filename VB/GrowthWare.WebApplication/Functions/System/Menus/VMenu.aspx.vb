Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.WebSupport.Utilities

Public Class VMenu
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mAccount As String = AccountUtility.HttpContextUserName()
        Dim myDataTable As DataTable = AccountUtility.GetMenu(mAccount, MenuType.Vertical)
        Navtrail.DataSource = myDataTable.DefaultView
        Navtrail.DataBind()
    End Sub

End Class