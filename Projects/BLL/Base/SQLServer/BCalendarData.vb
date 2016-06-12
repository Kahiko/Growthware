Imports DALFactory.Base.Application
Imports DALInterface.Base.Interfaces
Imports System.Runtime.InteropServices

Namespace Base.SQLServer
    Public Class BCalendarData
		'Private Shared iBaseDAL As ICalendarData = FCalendarData.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"))
		Private Shared iBaseDAL As ICalendarData = AbstractFactory.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"), "DCalendarData")

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
End Namespace