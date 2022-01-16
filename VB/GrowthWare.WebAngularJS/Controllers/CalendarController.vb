Imports System.Web.Http
Imports GrowthWare.WebSupport.Utilities

Namespace Controllers
    Public Class CalendarController
        Inherits ApiController

        ' GET api/Calendar/GetCalendarData?calendarName='community calendar'
        <HttpGet>
        Public Function GetCalendarData(ByVal calendarName As String) As DataTable
            Dim mRetVal As New DataSet()
            Dim mSuccess As Boolean = CalendarUtility.GetCalendarData(calendarName, mRetVal)
            If (Not mSuccess) Then
                mRetVal = New DataSet()
            End If
            Return mRetVal.Tables(0)
        End Function
    End Class
End Namespace
