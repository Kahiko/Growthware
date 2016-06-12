Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Common.Cache
Imports System.Data

#Region " Notes "
' The CalendarUtility class aids in managing the calendar data.
' This was added in an effort to show how it is possible
' to have a single set of code perform the same operation
' but allow for seporate data
#End Region
Public Class CalendarUtility
	Public Shared Function GetCalendarData(ByVal State As String, ByVal Calendar_Name As String, ByRef dsCalendarData As System.Data.DataSet) As Boolean
		Dim success As Boolean = False
		Dim myDS As DataSet
		myDS = CType(HttpContext.Current.Cache.Item(State & Calendar_Name & "CalendarData"), DataSet)
		If myDS Is Nothing Then
            success = BCalendarData.GetCalendarData(State, Calendar_Name, myDS)
            If success Then
                dsCalendarData = myDS
                CacheControler.AddToCacheDependency(State & Calendar_Name & "CalendarData", myDS)
            End If
        Else
            dsCalendarData = myDS
            success = True
        End If
        Return success
    End Function

    Public Shared Function SaveCalendarData(ByVal State As String, ByVal Calendar_Name As String, ByVal Entry As String, ByVal EntryDate As Date) As Boolean
        Dim success As Boolean = BCalendarData.SaveCalendarData(State, Calendar_Name, Entry, EntryDate)
        If success Then
            CacheControler.RemoveFromCache(State & Calendar_Name & "CalendarData")
        End If
        Return success
    End Function

    Public Shared Function DeleteCalendarData(ByVal State As String, ByVal Calendar_Name As String, ByVal Entry As String, ByVal EntryDate As Date) As Boolean
        Dim success As Boolean = BCalendarData.DeleteCalendarData(State, Calendar_Name, Entry, EntryDate)
        If success Then
            CacheControler.RemoveFromCache(State & Calendar_Name & "CalendarData")
        End If
        Return success
    End Function
End Class
