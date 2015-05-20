Imports GrowthWare.WebSupport
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities

Public Class AddEditNVPDetails
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim NVP_SEQ_ID As String = GWWebHelper.GetQueryValue(Request, "NVP_SEQ_ID")
        Dim NVP_Detail_SeqID As String = GWWebHelper.GetQueryValue(Request, "NVP_Detail_SeqID")
        If Not String.IsNullOrEmpty(NVP_SEQ_ID) And Not String.IsNullOrEmpty(NVP_Detail_SeqID) Then
            Dim mSeqId As Integer = Integer.Parse(NVP_SEQ_ID)
            Dim mSeqDetId As Integer = Integer.Parse(NVP_Detail_SeqID)
            Dim mProfile As MNameValuePairDetail = New MNameValuePairDetail()
            If Not mSeqDetId = -1 Then
                mProfile = NameValuePairUtility.GetNameValuePairDetail(mSeqDetId, mSeqId)
            End If

            hdnNVP_SEQ_ID.Value = mSeqId
            hdnNVP_SEQ_DET_ID.Value = mProfile.Id
            HttpContext.Current.Session.Add("EditId", mProfile.Id)
            txtValue.Value = mProfile.Value
            txtText.Value = mProfile.Text
            txtSortOrder.Value = mProfile.SortOrder
        End If
    End Sub

End Class