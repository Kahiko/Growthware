Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.WebSupport.Utilities

Public Class VHMenu
    Inherits System.Web.UI.Page

    Private m_MenuRelationName As String = "MenuRelation"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mAccount As String = AccountUtility.HttpContextUserName()
        Dim mDataTable As DataTable = AccountUtility.GetMenu(mAccount, MenuType.Hierarchical)
        Dim mMenuString As String = String.Empty
        If Not mDataTable Is Nothing And mDataTable.Rows.Count > 0 Then
            Dim mDataset As DataSet = New DataSet()
            mDataset.Tables.Add(mDataTable.Copy())
            Dim mMenu As String = String.Empty
            Dim mRelation As DataRelation = New DataRelation(m_MenuRelationName, mDataset.Tables(0).Columns("MenuID"), mDataset.Tables(0).Columns("ParentID"))
            Dim mStringBuiler As StringBuilder = New StringBuilder()
            Try
                mDataset.EnforceConstraints = False
                mDataset.Relations.Add(mRelation)
                mMenuString = MenuUtility.GenerateULLI(mDataTable, mStringBuiler)
            Catch ex As Exception
            End Try

        End If
        cssmenuVHMenu.InnerHtml = mMenuString
    End Sub

End Class