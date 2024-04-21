Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports System.Data

Namespace BusinessLogicLayer
    Public Class BCalendarData
        Inherits BaseBusinessLogic

        Private m_DCalendarData As IDCalendarData

        ''' <summary>
        ''' Private sub New() to ensure only new instances with passed parameters is used.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()

        End Sub

        ''' <summary>
        ''' Parameters are need to pass along to the factory for correct connection to the desired datastore.
        ''' </summary>
        ''' <param name="SecurityEntityProfile">The Security Entity profile used to obtain the DAL name, DAL name space, and the Connection String</param>
        ''' <param name="CentralManagement">Boolean value indicating if the system is being used to manage multiple database instances.</param>
        ''' <remarks></remarks>
        ''' <example> This sample shows how to create an instance of the class.
        ''' <code language="VB.NET">
        ''' <![CDATA[
        ''' MSecurityEntityProfile mSecurityEntityProfile = MSecurityEntityProfile = New MSecurityEntityProfile();
        ''' mSecurityEntityProfile.ID = ConfigSettings.DefaultSecurityEntityID;
        ''' mSecurityEntityProfile.DAL = ConfigSettings.DAL;
        ''' mSecurityEntityProfile.DAL_Namespace = ConfigSettings.DAL_NameSpace(mSecurityEntityProfile.DAL);
        ''' mSecurityEntityProfile.DAL_Name = ConfigSettings.DAL_AssemblyName(mSecurityEntityProfile.DAL);
        ''' mSecurityEntityProfile.ConnectionString = ConfigSettings.ConnectionString;
        ''' 
        ''' Dim mBCalendarData As BCalendarData = New BCalendarData(mSecurityEntityProfile, ConfigSettings.CentralManagement)
        ''' ]]>
        ''' </code>
        ''' <code language="C#">
        ''' <![CDATA[
        ''' Dim mSecurityEntityProfile As MSecurityEntityProfile = New MSecurityEntityProfile()
        ''' mSecurityEntityProfile.ID = ConfigSettings.DefaultSecurityEntityID
        ''' mSecurityEntityProfile.DAL = ConfigSettings.DAL
        ''' mSecurityEntityProfile.DAL_Namespace = ConfigSettings.DAL_NameSpace(mSecurityEntityProfile.DAL)
        ''' mSecurityEntityProfile.DAL_Name = ConfigSettings.DAL_AssemblyName(mSecurityEntityProfile.DAL)
        ''' mSecurityEntityProfile.ConnectionString = ConfigSettings.ConnectionString
        ''' 
        ''' BCalendarData mBCalendarData = new BCalendarData(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        ''' ]]>
        ''' </code>
        ''' </example>
        Public Sub New(ByVal securityEntityProfile As MSecurityEntityProfile, ByVal centralManagement As Boolean)
            If (securityEntityProfile Is Nothing) Then
                Throw New ArgumentNullException("securityEntityProfile", "securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!")
            End If
            If Not centralManagement Then
                If m_DCalendarData Is Nothing Then
                    m_DCalendarData = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DCalendarData")
                End If
            Else
                m_DCalendarData = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DCalendarData")
            End If
            m_DCalendarData.ConnectionString = securityEntityProfile.ConnectionString
            m_DCalendarData.SecurityEntitySeqId = securityEntityProfile.Id
        End Sub

        Public Function GetCalendarData(ByVal calendarName As String, ByRef calendarDataSet As DataSet) As Boolean
            m_DCalendarData.CalendarName = calendarName
            Return m_DCalendarData.GetCalendarData(calendarDataSet)
        End Function

        Public Function SaveCalendarData(ByVal calendarName As String, ByVal comment As String, ByVal entryDate As Date, ByVal accountSeqId As Integer) As Boolean
            m_DCalendarData.CalendarName = calendarName
            Return m_DCalendarData.SaveCalendarData(comment, entryDate, accountSeqId)
        End Function

        Public Function DeleteCalendarData(ByVal calendarName As String, ByVal comment As String, ByVal entryDate As Date, ByVal accountSeqId As Integer) As Boolean
            m_DCalendarData.CalendarName = calendarName
            Return m_DCalendarData.DeleteCalendarData(comment, entryDate, accountSeqId)
        End Function
    End Class
End Namespace
