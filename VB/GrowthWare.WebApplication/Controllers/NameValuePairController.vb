Imports System.Net
Imports System.Web.Http
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Common
Imports GrowthWare.WebSupport
Imports GrowthWare.WebApplication.Models

Namespace Controllers
    Public Class NameValuePairController
        Inherits ApiController

        <HttpPost>
        Public Function SaveNameValuePair(ByVal uiProfile As UINVPProfile) As IHttpActionResult
            Dim mRetVal As String = False
            Dim mEditId = Integer.Parse(HttpContext.Current.Items("EditId").ToString())
            Dim mAction As String = GWWebHelper.GetQueryValue(HttpContext.Current.Request, "Action")
            Dim mLog As Logger = Logger.Instance()
            If mEditId <> uiProfile.NVP_SEQ_ID Then
                Dim mError As Exception = New Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!")
                mLog.Error(mError)
                Return Me.InternalServerError(mError)
            End If

            Dim mProfile As MNameValuePair = New MNameValuePair()
            Dim mUpdatingAccount As MAccountProfile = AccountUtility.CurrentProfile()
            Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            Dim mGroups As String = String.Join(",", uiProfile.Groups)
            Dim mRoles As String = String.Join(",", uiProfile.Roles)
            Dim mCommaSepRoles As String = mUpdatingAccount.GetCommaSeparatedAssignedRoles()
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(mAction), mUpdatingAccount)
            If uiProfile.NVP_SEQ_ID = -1 Then
                If Not mSecurityInfo.MayAdd Then
                    Dim mError As Exception = New Exception("The account (" + mUpdatingAccount.Account + ") being used does not have the correct permissions to add")
                    mLog.Error(mError)
                    Return Me.InternalServerError(mError)
                End If
            Else
                If Not mSecurityInfo.MayEdit Then
                    Dim mError As Exception = New Exception("The account (" + mUpdatingAccount.Account + ") being used does not have the correct permissions to edit")
                    mLog.Error(mError)
                    Return Me.InternalServerError(mError)
                End If
            End If
            Try
                If Not uiProfile.NVP_SEQ_ID = -1 Then
                    mProfile = NameValuePairUtility.GetNameValuePair(uiProfile.NVP_SEQ_ID)
                Else
                    mProfile.AddedBy = mUpdatingAccount.Id
                    mProfile.AddedDate = DateTime.Today
                End If
                mProfile.UpdatedBy = mUpdatingAccount.Id
                mProfile.UpdatedDate = DateTime.Today
                mProfile.StaticName = uiProfile.STATIC_NAME
                mProfile.SchemaName = uiProfile.SchemaName
                mProfile.Display = uiProfile.Display
                mProfile.Description = uiProfile.Description
                mProfile.Status = uiProfile.Status
                Dim mID As Integer = NameValuePairUtility.Save(mProfile)
                NameValuePairUtility.UpdateRoles(mID, mSecurityEntityProfile.Id, mRoles, mProfile)
                NameValuePairUtility.UpdateGroups(mID, mSecurityEntityProfile.Id, mGroups, mProfile)
                mRetVal = True
            Catch ex As Exception
                mLog.Error(ex)
            End Try

            Return Me.Ok(mRetVal)
        End Function
    End Class

End Namespace