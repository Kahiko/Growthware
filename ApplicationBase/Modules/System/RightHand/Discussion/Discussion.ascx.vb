Imports ApplicationBase.Common.CustomWebControls
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.ClientChoices
Imports ApplicationBase.Model.Special.ClientChoices
Imports System.Data
Imports System.Data.SqlClient

Partial Class Discussion
    Inherits ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        '1,"Anonymous", 9, 10, 1,"Date Commented DESC"
        ContentList.DataSource = Discuss.DiscussUtility.GetPosts("Anonymous", 9, 10, 1, "Date Commented DESC")
        ContentList.DataBind()

	End Sub
End Class