
Imports System.Data.SqlClient
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.Base
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces

Namespace DataAccessLayer.SQLServer.V2008
    Public Class DCalendarData
        Inherits DDBInteraction
        Implements IDCalendarData

#Region "Private Fields"
        Private m_CalendarName As String = String.Empty
        Private m_SecurityEntitySeqID As Integer = -2
#End Region

#Region "Public Properties"
        Public Property CalendarName As String Implements IDCalendarData.CalendarName
            Get
                Return m_CalendarName
            End Get
            Set(ByVal value As String)
                If (Not value = Nothing) Then
                    m_CalendarName = value
                End If
            End Set
        End Property

        Public Property SecurityEntitySeqId As Integer Implements IDCalendarData.SecurityEntitySeqId
            Get
                Return m_SecurityEntitySeqID
            End Get
            Set(ByVal value As Integer)
                If (Not value = Nothing) Then
                    m_SecurityEntitySeqID = value
                End If
            End Set
        End Property
#End Region

#Region "Public Methods"
        Public Function GetCalendarData(ByRef calendarDataSet As DataSet) As Boolean Implements IDCalendarData.GetCalendarData
            Dim mRetVal As Boolean = False
            Dim mStoredProcedure As String = "ZGWOptional.Delete_Calendar_Data"
            Dim mParameters() As SqlParameter =
                {
                    New SqlParameter("@P_Security_Entity_SeqID", m_SecurityEntitySeqID),
                    New SqlParameter("@P_Calendar_Name", m_CalendarName)
                }
            Try
                calendarDataSet = MyBase.GetDataSet(mStoredProcedure, mParameters)
                mRetVal = True
            Catch ex As Exception
                Throw ex
            End Try
            Return mRetVal
        End Function

        Public Function SaveCalendarData(ByVal comment As String, ByVal entryDate As Date, ByVal accountSeqId As Integer) As Boolean Implements IDCalendarData.SaveCalendarData
            checkValid()
            Dim mStoredProcedure As String = "ZGWOptional.Set_Calendar_Data"
            Dim mParameters() As SqlParameter =
                {
                    New SqlParameter("@P_Security_Entity_SeqID", m_SecurityEntitySeqID),
                    New SqlParameter("@P_Calendar_Name", m_CalendarName),
                    New SqlParameter("@P_Comment", comment),
                    New SqlParameter("@P_EntryDate", entryDate),
                    New SqlParameter("@P_ADDUPD_BY", accountSeqId)
                }
            Dim mRetVal As Boolean = False
            Try
                MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
                mRetVal = True
            Catch ex As Exception
                Throw ex
            End Try
            Return mRetVal
        End Function

        Public Function DeleteCalendarData(ByVal comment As String, ByVal entryDate As Date, ByVal accountSeqId As Integer) As Boolean Implements IDCalendarData.DeleteCalendarData
            checkValid()
            Dim mStoredProcedure As String = "ZGWOptional.Delete_Calendar_Data"
            Dim mParameters() As SqlParameter =
                {
                    New SqlParameter("@P_Security_Entity_SeqID", m_SecurityEntitySeqID),
                    New SqlParameter("@P_Calendar_Name", m_CalendarName),
                    New SqlParameter("@P_Comment", comment),
                    New SqlParameter("@P_EntryDate", entryDate),
                    New SqlParameter("@P_ADDUPD_BY", accountSeqId)
                }
            Dim mRetVal As Boolean = False
            Try
                MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
                mRetVal = True
            Catch ex As Exception
                Throw ex
            End Try
            Return mRetVal
        End Function
#End Region

#Region "Private Methods"
        Private Sub checkValid()
            MyBase.IsValid()
            If String.IsNullOrEmpty(m_CalendarName) Or String.IsNullOrWhiteSpace(m_CalendarName) Then
                Throw New DataAccessLayerException("CalendarName property must be set before calling methods from this class")
            End If
            If m_SecurityEntitySeqID = -2 Then
                Throw New DataAccessLayerException("SecurityEntitySeqID property must be set before calling methods from this class")
            End If
        End Sub
#End Region
    End Class
End Namespace
