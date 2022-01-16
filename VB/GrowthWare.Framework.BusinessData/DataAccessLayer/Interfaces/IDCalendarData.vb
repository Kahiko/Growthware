Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base

Namespace DataAccessLayer.Interfaces
	Public Interface IDCalendarData
		Inherits IDDBInteraction

		''' <summary>
		''' Used by all methods and must be set to send parameters to the data store
		''' </summary>
		Property CalendarName As String

		''' <summary>
		''' Used by all methods and must be set to send parameters to the data store
		''' </summary>
		Property SecurityEntitySeqId() As Integer

		Function GetCalendarData(ByRef dsCalendarData As DataSet) As Boolean
		Function SaveCalendarData(ByVal comment As String, ByVal entryDate As Date, ByVal accountSeqId As Integer) As Boolean
		Function DeleteCalendarData(ByVal comment As String, ByVal entryDate As Date, ByVal accountSeqId As Integer) As Boolean

	End Interface
End Namespace
