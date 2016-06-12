Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Web.Utilities
Imports System.Collections.ObjectModel

Public Class _Default
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

	End Sub

	Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
		Dim mStartTime As Date = Date.Now
		Dim mAccountUtility = New AccountUtility()
		Dim mFunctionUtility = New FunctionUtility()

		Dim mAccountProfile = mAccountUtility.GetCurrentProfile
		Dim mAccountCollection As Collection(Of MAccountProfile) = mAccountUtility.GetAccounts(mAccountProfile)
		Dim mFunctionCollection As Collection(Of MFunctionProfile) = mFunctionUtility.GetFunctions()
		Dim mFunctionProfile As MFunctionProfile = mFunctionUtility.GetFunction("generic_home")
		Dim mSecurtyInfo As MSecurityInfo = New MSecurityInfo(mFunctionProfile, mAccountProfile.DerivedRoles)
		'todo add code to test saving client choices
		mAccountProfile = mAccountUtility.GetProfile("Developer")
		mAccountProfile.SetRoles("Authenticated,Developer")
		mAccountUtility.Save(mAccountProfile, True, False)
		mAccountProfile.SetRoles("AlwaysLogon,Authenticated,Developer")
		mAccountUtility.Save(mAccountProfile, True, False)

		Dim mEndTime As Date = Date.Now
		Dim mTS As TimeSpan = mEndTime.Subtract(mStartTime)
		lblStartTime.Text = mStartTime.ToString()
		lblStopTime.Text = mEndTime.ToString()
		lblDuration.Text = mTS.ToString()

	End Sub
End Class