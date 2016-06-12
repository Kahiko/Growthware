Imports Common.Cache
Imports BLL.Base.SQLServer
Imports DALModel

Public Class DropBoxUtility
	Public Shared DropBoxCacheName As String = "CachedDropBoxes"

	Public Shared Sub GET_DROP_BOX_NAME(ByVal ACCOUNT_SEQ_ID As Integer, ByRef yourDataView As DataView)
		BDropBox.GET_DROP_BOX_NAME(ACCOUNT_SEQ_ID, yourDataView)
	End Sub

	Public Shared Function GET_DROP_BOX_DETAILS(ByVal DROP_BOX_ID As Integer) As DataView
		Dim myDataView As New DataView
		GET_DROP_BOX_DETAIL(myDataView)
		myDataView.RowFilter = "DROP_BOX_SEQ_ID=" & DROP_BOX_ID
		Return myDataView
	End Function

	Public Shared Function ADD_DROP_BOX_DETAIL(ByVal DROP_BOX_DET_code As Integer, ByVal DROP_BOX_DET_value As String, ByVal DROP_BOX_DET_status As Integer, ByVal DROP_BOX_ID As Integer, ByVal ACCOUNT_SEQ_ID As Integer) As Boolean
		HttpContext.Current.Cache.Remove(DropBoxCacheName)
		Return BDropBox.ADD_DROP_BOX_DETAIL(DROP_BOX_DET_code, DROP_BOX_DET_value, DROP_BOX_DET_status, DROP_BOX_ID, ACCOUNT_SEQ_ID)
	End Function

	Public Shared Sub BIND_DROP_BOX_DETIALS(ByRef DROP_BOX As DropDownList, ByVal DROP_BOX_ID As Integer, Optional ByVal SortOrder As String = "ASC")
		Dim myDataView As New DataView
		GET_DROP_BOX_DETAIL(myDataView)
		myDataView.RowFilter = "DROP_BOX_SEQ_ID=" & DROP_BOX_ID & " and DROP_BOX_DET_STATUS = " & Base.MSystemStatus.value.Active
		myDataView.Sort = "DROP_BOX_DET_CODE " & SortOrder
		DROP_BOX.DataSource = myDataView
		DROP_BOX.DataValueField = "drop_box_det_id"
		DROP_BOX.DataTextField = "drop_box_det_value"
		DROP_BOX.DataBind()
	End Sub

	Public Shared Function EDIT_DROP_BOX(ByVal DROP_BOX_DET_ID As Integer, ByVal DROP_BOX_DET_code As Integer, ByVal DROP_BOX_DET_value As String, ByVal DROP_BOX_DET_status As Integer, ByVal DROP_BOX_ID As Integer, ByVal ACCOUNT_SEQ_ID As Integer) As Boolean
		HttpContext.Current.Cache.Remove(DropBoxCacheName)
		Return BDropBox.UPDATE_DROP_BOX(DROP_BOX_DET_ID, DROP_BOX_DET_code, DROP_BOX_DET_value, DROP_BOX_DET_status, DROP_BOX_ID, ACCOUNT_SEQ_ID)
	End Function

	Private Shared Sub GET_DROP_BOX_DETAIL(ByRef yourDataView As DataView)
		yourDataView = HttpContext.Current.Cache(DropBoxCacheName)
		If yourDataView Is Nothing Then
			BDropBox.GET_DROP_BOX_DETAIL(yourDataView)
			CacheControler.AddToCacheDependency(DropBoxCacheName, yourDataView)
		End If
	End Sub
End Class