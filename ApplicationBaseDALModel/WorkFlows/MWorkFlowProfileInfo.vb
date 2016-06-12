Namespace WorkFlows
	<Serializable(), CLSCompliant(True)> _
	Public Class MWorkFlowProfileInfo
		Private _WORK_FLOW_SEQ_ID As Integer = 0
		Private _Order As Integer = 1
		Private _WorkFlowName As String = String.Empty
		Private _Action As String = String.Empty

		Public Sub New()

		End Sub

		Public Sub New(ByVal drowWorkFlow As DataRow)
			On Error Resume Next	' ignore db null
			_WORK_FLOW_SEQ_ID = CInt(drowWorkFlow("WORK_FLOW_SEQ_ID"))
			_Order = CInt(drowWorkFlow("ORDER_ID"))
			_WorkFlowName = CStr(drowWorkFlow("WORK_FLOW_NAME"))
			_Action = CStr(drowWorkFlow("MODULE_SEQ_ID"))
		End Sub	'New

		Public Property WORK_FLOW_SEQ_ID() As Integer
			Get
				Return _WORK_FLOW_SEQ_ID
			End Get
			Set(ByVal Value As Integer)
				_WORK_FLOW_SEQ_ID = Value
			End Set
		End Property
		Public Property Order() As Integer
			Get
				Return _Order
			End Get
			Set(ByVal Value As Integer)
				_Order = Value
			End Set
		End Property

		Public Property WorkFlowName() As String
			Get
				Return _WorkFlowName
			End Get
			Set(ByVal Value As String)
				_WorkFlowName = Value.Trim
			End Set
		End Property

		Public Property Action() As String
			Get
				Return _Action
			End Get
			Set(ByVal Value As String)
				_Action = Value.Trim
			End Set
		End Property
	End Class
End Namespace