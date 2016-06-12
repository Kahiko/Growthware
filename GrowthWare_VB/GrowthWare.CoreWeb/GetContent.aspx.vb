Imports GrowthWare.Framework.Web
Imports GrowthWare.Framework.Web.Utilities
Imports GrowthWare.Framework.Model.Profiles


Public Class GetContent
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If WebConfigSettings.DBStatus.ToUpper = "ONLINE" Then
			Dim mControlSource As String = String.Empty
			Dim mAction As String = HttpContext.Current.Request.QueryString("Action")
            Dim mUserControl As New Control
            Dim mFunctionUtil As FunctionUtility = New FunctionUtility()
            Dim mFunction As MFunctionProfile = New MFunctionProfile()
            Select Case mAction
                Case "Get_Menu_Data_Hierarchical"
                    mControlSource = "Functions\System\Menus\Hierarchical.ascx"
                    Exit Select
                Case "Get_Menu_Data_Vertical"
                    mControlSource = "Functions\System\Menus\Vertical.ascx"
                    Exit Select
                Case "Get_Menu_Data_Horizontal"
                    mControlSource = "Functions\System\Menus\Horizontal.ascx"
                    Exit Select
                Case Else
                    mFunction = mFunctionUtil.GetFunction(mAction)
                    If Not mFunction Is Nothing Then
                        mControlSource = mFunction.Source
                    Else
                        mFunction = mFunctionUtil.GetFunction(GWWebHelper.ActionNotAvailable)
                        mControlSource = mFunction.Source
                    End If
            End Select

			'For working with legacy data sources
			If mControlSource.StartsWith("Modules\") Then
				mControlSource = mControlSource.Replace("Modules\", "Functions\")
			End If
			Try
				mUserControl = Me.LoadControl(mControlSource)
				ContentPlaceHolder.Controls.Add(mUserControl)
			Catch ex As Exception
				Dim myErrorMSG As String = String.Empty
				myErrorMSG = "A critical error has occured within the UI.  The UI is: None"
				Dim myEx As New ApplicationException(myErrorMSG, ex)
				mControlSource = "Functions\System\Errors\DisplayError.ascx"
				mUserControl = Me.LoadControl(mControlSource)
				ContentPlaceHolder.Controls.Add(mUserControl)
			End Try
		End If
	End Sub

End Class