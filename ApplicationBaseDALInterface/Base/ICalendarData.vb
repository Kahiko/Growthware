Public Interface ICalendarData
	Function GetCalendarData(ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal Calendar_Name As String, ByRef dsCalendarData As DataSet) As Boolean
	Function SaveCalendarData(ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal Calendar_Name As String, ByVal Entry As String, ByVal EntryDate As Date, Optional ByVal Account_Seq_id As Integer = 1) As Boolean
	Function DeleteCalendarData(ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal Calendar_Name As String, ByVal Entry As String, ByVal EntryDate As Date, Optional ByVal Account_Seq_id As Integer = 1) As Boolean
End Interface