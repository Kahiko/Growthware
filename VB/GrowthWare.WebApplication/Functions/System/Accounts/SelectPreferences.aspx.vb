Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Base
Imports GrowthWare.WebSupport.Utilities
Imports System.Globalization

Public Class SelectPreferences
    Inherits ClientChoicesPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim rowFilter As String = "FUNCTION_TYPE_SEQ_ID <> 2 AND FUNCTION_TYPE_SEQ_ID <> 3"
        Dim myDataView As DataView = AccountUtility.GetMenu(AccountUtility.CurrentProfile().Account, MenuType.Hierarchical).DefaultView
        myDataView.Sort = "Title asc"
        myDataView.RowFilter = rowFilter
        dropFavorite.DataSource = myDataView
        dropFavorite.DataValueField = "url"
        dropFavorite.DataTextField = "Title"
        dropFavorite.DataBind()
        txtPreferedRecordsPerPage.Text = ClientChoicesState(MClientChoices.RecordsPerPage).ToString()
        NameValuePairUtility.SetDropSelection(dropFavorite, ClientChoicesState(MClientChoices.Action).ToString())
        Dim x As Integer = 0
        For x = 1 To 5
            Dim button As HtmlInputRadioButton = Me.FindControl("Radio" & x)
            If Left(button.Value, Len(ClientChoicesState(MClientChoices.ColorScheme))).ToUpper(New CultureInfo("en-US", False)) = ClientChoicesState(MClientChoices.ColorScheme).ToUpper(New CultureInfo("en-US", False)) Then
                button.Checked = True
                Exit For
            End If
        Next
    End Sub

End Class