Imports System.Net
Imports System.Web.Http
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Common
Imports GrowthWare.WebSupport
Imports GrowthWare.WebAngularjs14.Models

Namespace Controllers
    Public Class NameValuePairController
        Inherits ApiController

        <HttpPost>
        Public Function SaveNameValuePair(ByVal uiProfile As MUINVPProfile) As IHttpActionResult
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

        <HttpPost>
        Public Function SaveNameValuePairDetail(ByVal uiProfile As MUINVPDetailProfile) As IHttpActionResult
            Dim mRetVal As String = False
            Dim mEditId = Integer.Parse(HttpContext.Current.Items("EditId").ToString())
            Dim mAction As String = GWWebHelper.GetQueryValue(HttpContext.Current.Request, "Action")
            Dim mLog As Logger = Logger.Instance()
            Dim mUpdatingProfile As MAccountProfile = AccountUtility.CurrentProfile()

            If mEditId <> uiProfile.NVP_SEQ_DET_ID Then
                Dim mError As Exception = New Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!")
                mLog.Error(mError)
                Return Me.InternalServerError(mError)
            End If
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(mAction), mUpdatingProfile)
            If uiProfile.NVP_SEQ_DET_ID = -1 Then
                If Not mSecurityInfo.MayAdd Then
                    Dim mError As Exception = New Exception("The account (" + mUpdatingProfile.Account + ") being used does not have the correct permissions to add")
                    mLog.Error(mError)
                    Return Me.InternalServerError(mError)
                End If
            Else
                If Not mSecurityInfo.MayEdit Then
                    Dim mError As Exception = New Exception("The account (" + mUpdatingProfile.Account + ") being used does not have the correct permissions to edit")
                    mLog.Error(mError)
                    Return Me.InternalServerError(mError)
                End If
            End If

            Try
                Dim mProfile As New MNameValuePairDetail
                If Not uiProfile.NVP_SEQ_DET_ID = -1 Then
                    mProfile = NameValuePairUtility.GetNameValuePairDetail(uiProfile.NVP_SEQ_DET_ID, uiProfile.NVP_Seq_ID)
                Else
                    mProfile.AddedBy = mUpdatingProfile.Id
                    mProfile.AddedDate = DateTime.Now
                End If
                mProfile.NameValuePairSeqId = uiProfile.NVP_Seq_ID
                mProfile.UpdatedBy = mUpdatingProfile.Id
                mProfile.UpdatedDate = DateTime.Now
                mProfile.SortOrder = uiProfile.SortOrder
                mProfile.Text = uiProfile.Text
                mProfile.Value = uiProfile.Value
                mProfile.Status = uiProfile.Status
                NameValuePairUtility.SaveDetail(mProfile)
                mRetVal = True.ToString()
            Catch ex As Exception
                mLog.Error(ex)
            End Try

            Return Me.Ok(mRetVal)
        End Function

        <HttpPost>
        Public Function DeleteNameValuePairDetail(ByVal uiProfile As MUINVPDetailProfile) As IHttpActionResult
            Dim mRetVal As String = False
            Dim mEditId = Integer.Parse(HttpContext.Current.Items("EditId").ToString())
            Dim mAction As String = GWWebHelper.GetQueryValue(HttpContext.Current.Request, "Action")
            Dim mLog As Logger = Logger.Instance()
            Dim mUpdatingProfile As MAccountProfile = AccountUtility.CurrentProfile()

            If mEditId <> uiProfile.NVP_SEQ_DET_ID Then
                Dim mError As Exception = New Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!")
                mLog.Error(mError)
                Return Me.InternalServerError(mError)
            End If
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(mAction), mUpdatingProfile)
            If Not mSecurityInfo.MayDelete Then
                Dim mError As Exception = New Exception("The account (" + mUpdatingProfile.Account + ") being used does not have the correct permissions to delete")
                mLog.Error(mError)
                Return Me.InternalServerError(mError)
            End If
            Try
                Dim mProfile As New MNameValuePairDetail
                mProfile.NameValuePairSeqId = uiProfile.NVP_Seq_ID
                mProfile.Id = uiProfile.NVP_SEQ_DET_ID
                mProfile.UpdatedBy = mUpdatingProfile.Id
                mProfile.UpdatedDate = DateTime.Now
                mProfile.SortOrder = uiProfile.SortOrder
                mProfile.Text = uiProfile.Text
                mProfile.Value = uiProfile.Value
                mProfile.Status = uiProfile.Status
                NameValuePairUtility.DeleteDetail(mProfile)
            Catch ex As Exception
                mLog.Error(ex)
            End Try
            Return Me.Ok(mRetVal)
        End Function

        <HttpPost>
        Public Function GetSearchResults(<FromBody> searchCriteria As MSearchCriteria) As IHttpActionResult
            Dim mDataTable As DataTable = Nothing
            Try
                mDataTable = NameValuePairUtility.Search(searchCriteria)
            Catch ex As Exception
                ' Only going to log message
                Dim mLog As Logger = Logger.Instance()
                mLog.Error(ex)
            End Try
            Return Ok(mDataTable)
        End Function

    End Class

End Namespace