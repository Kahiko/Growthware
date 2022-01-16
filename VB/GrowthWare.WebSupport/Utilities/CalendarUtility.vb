Imports System.Web
Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles

Namespace Utilities
#Region " Notes "
    ' The CalendarUtility class aids in managing the calendar data.
    ' This was added in an effort to show how it is possible
    ' to have a single set of code perform the same operation
    ' but allow for seporate data
#End Region
    Public Class CalendarUtility
        Public Shared Function GetCalendarData(ByVal Calendar_Name As String, ByRef dsCalendarData As System.Data.DataSet) As Boolean
            Dim success As Boolean = False
            Dim myDS As DataSet
            Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            Dim mBCalendarData As BCalendarData = New BCalendarData(mSecurityEntityProfile, ConfigSettings.CentralManagement)
            myDS = CType(HttpContext.Current.Cache.Item(mSecurityEntityProfile.Name & Calendar_Name & "CalendarData"), DataSet)
            If myDS Is Nothing Then
                success = mBCalendarData.GetCalendarData(Calendar_Name, myDS)
                If success Then
                    dsCalendarData = myDS
                    CacheController.AddToCacheDependency(mSecurityEntityProfile.Name & "_" & Calendar_Name & "CalendarData", myDS)
                End If
            Else
                dsCalendarData = myDS
                success = True
            End If
            Return success
        End Function

        Public Shared Function SaveCalendarData(ByVal State As String, ByVal Calendar_Name As String, ByVal Entry As String, ByVal EntryDate As Date, ByVal accountSeqId As Integer) As Boolean
            Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            Dim mBCalendarData As BCalendarData = New BCalendarData(mSecurityEntityProfile, ConfigSettings.CentralManagement)
            Dim success As Boolean = mBCalendarData.SaveCalendarData(Calendar_Name, Entry, EntryDate, accountSeqId)
            If success Then
                CacheController.RemoveFromCache(mSecurityEntityProfile.Name & "_" & Calendar_Name & "CalendarData")
            End If
            Return success
        End Function

        Public Shared Function DeleteCalendarData(ByVal Calendar_Name As String, ByVal Entry As String, ByVal EntryDate As Date, ByVal accountSeqId As Integer) As Boolean
            Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            Dim mBCalendarData As BCalendarData = New BCalendarData(mSecurityEntityProfile, ConfigSettings.CentralManagement)
            Dim success As Boolean = mBCalendarData.DeleteCalendarData(Calendar_Name, Entry, EntryDate, accountSeqId)
            If success Then
                CacheController.RemoveFromCache(mSecurityEntityProfile.Name & "_" & Calendar_Name & "CalendarData")
            End If
            Return success
        End Function
    End Class
End Namespace
