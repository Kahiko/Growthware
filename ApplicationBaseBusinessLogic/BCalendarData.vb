Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Factory
Imports ApplicationBase.Interfaces
Imports System.Runtime.InteropServices

Public Class BCalendarData
	'Private Shared iBaseDAL As ICalendarData = FCalendarData.Create(Configuration.ConfigurationManager.AppSettings("BaseDAL"))
    Private Shared iBaseDAL As ICalendarData = FactoryObject.Create(BaseSettings.applicationBaseDAL, "DCalendarData")

	Public Shared Function GetCalendarData(ByVal State As String, ByVal Calendar_Name As String, ByRef dsCalendarData As System.Data.DataSet) As Boolean
		Return iBaseDAL.GetCalendarData(State, Calendar_Name, dsCalendarData)
	End Function

	Public Shared Function SaveCalendarData(ByVal State As String, ByVal Calendar_Name As String, ByVal Entry As String, ByVal EntryDate As Date, Optional ByVal Account_seq_Id As Integer = 1) As Boolean
		Return iBaseDAL.SaveCalendarData(State, Calendar_Name, Entry, EntryDate, Account_seq_Id)
	End Function

	Public Shared Function DeleteCalendarData(ByVal State As String, ByVal Calendar_Name As String, ByVal Entry As String, ByVal EntryDate As Date, Optional ByVal Account_seq_Id As Integer = 1) As Boolean
		Return iBaseDAL.DeleteCalendarData(State, Calendar_Name, Entry, EntryDate, Account_seq_Id)
	End Function
End Class