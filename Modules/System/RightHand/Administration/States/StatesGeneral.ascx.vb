Imports Common.Security.BaseSecurity
Imports DALModel.Base.States

Public Class StatesGeneral
    Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
    Protected WithEvents litState As System.Web.UI.WebControls.Literal
    Protected WithEvents txtDescription As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtConnectionString As System.Web.UI.WebControls.TextBox
    Protected WithEvents STATUS_SEQ_ID As System.Web.UI.WebControls.DropDownList

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
		If Not IsPostBack Then
			Dim stateProfileInfo As New MStateProfileInfo
			stateProfileInfo = StatesUtility.GetInfoByState(Request.QueryString("id"))
			If Not stateProfileInfo Is Nothing Then
				litState.Text = stateProfileInfo.State
				txtDescription.Text = stateProfileInfo.LongName
				Dim x As Integer
				BaseHelper.SetDropSelection(STATUS_SEQ_ID, stateProfileInfo.STATUS_SEQ_ID)
			End If
		End If
    End Sub

    Public Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Dim statesProfileInfo As New MStateProfileInfo
		Dim wasSuccess As Boolean
		statesProfileInfo.State = litState.Text.Trim.ToUpper
		statesProfileInfo.LongName = txtDescription.Text.Trim
		statesProfileInfo.STATUS_SEQ_ID = STATUS_SEQ_ID.SelectedValue
		Try
			wasSuccess = StatesUtility.UpdateStateProfileInfo(statesProfileInfo)
			If Not wasSuccess Then
				Dim ex As New Exception("Could not update state information")
				Throw ex
			End If
		Catch ex As Exception
			Throw New Exception("Could not update state information" & vbCrLf & ex.Message)
		End Try
    End Sub
End Class