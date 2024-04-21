Imports GrowthWare.WebSupport.Base
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports System.Drawing

Public Class CommunityCalendar
    Inherits ClientChoicesPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mFProfile As MFunctionProfile = FunctionUtility.CurrentProfile()
        Dim mAProfile As MAccountProfile = AccountUtility.CurrentProfile()
        CalendarControl.TitleStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeadColor))
        CalendarControl.DayStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.LeftColor))
        CalendarControl.DayHeaderStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.SubheadColor))
        CalendarControl.SelectedDayStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.SubheadColor))
        CalendarControl.TodayDayStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeadColor))
        Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(mFProfile, mAProfile)
        If mSecurityInfo.MayDelete Then
            BtnDelete.Visible = True
        End If
        If mSecurityInfo.MayAdd OrElse mSecurityInfo.MayEdit Then
            BtnSave.Visible = True
        End If
        litError.Text = String.Empty
        litError.Visible = False
    End Sub

End Class