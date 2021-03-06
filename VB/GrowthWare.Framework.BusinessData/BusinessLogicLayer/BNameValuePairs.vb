﻿Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports GrowthWare.Framework.Common

Namespace BusinessLogicLayer
    Public Class BNameValuePairs
        Inherits BaseBusinessLogic

        Private m_DNameValuePair As IDNameValuePair

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
        ''' Dim mBAccount As BAccounts = New BAccounts(mSecurityEntityProfile, ConfigSettings.CentralManagement)
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
        ''' BAccounts mBAccount = new BAccounts(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        ''' ]]>
        ''' </code>
        ''' </example>
        Public Sub New(ByVal securityEntityProfile As MSecurityEntityProfile, ByVal centralManagement As Boolean)
            If (securityEntityProfile Is Nothing) Then
                Throw New ArgumentNullException("securityEntityProfile", "securityEntityProfile can not be null or empty!")
            End If
            If Not centralManagement Then
                If m_DNameValuePair Is Nothing Then
                    m_DNameValuePair = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DNameValuePair")
                End If
            Else
                m_DNameValuePair = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DNameValuePair")
            End If
            m_DNameValuePair.ConnectionString = securityEntityProfile.ConnectionString
            m_DNameValuePair.SecurityEntityId = securityEntityProfile.Id
        End Sub

        ''' <summary>
        ''' Deletes the NVP detail.
        ''' </summary>
        ''' <param name="detailProfile">The detail profile.</param>
        Public Sub DeleteNameValuePairDetail(ByVal detailProfile As MNameValuePairDetail)
            If DatabaseIsOnline() Then m_DNameValuePair.DeleteNameValuePairDetail(detailProfile)
        End Sub

        ''' <summary>
        ''' Gets all NVP.
        ''' </summary>
        ''' <returns>DataTable.</returns>
        Public Function AllNameValuePairs() As DataTable
            Dim mRetVal As DataTable = Nothing
            m_DNameValuePair.SecurityEntityId = ConfigSettings.DefaultSecurityEntityId ' for future use ... the DB is capable of dividing the NVPs by BU
            m_DNameValuePair.AccountId = -1
            m_DNameValuePair.NameValuePairProfile.Id = -1
            If DatabaseIsOnline() Then mRetVal = m_DNameValuePair.AllNameValuePairs
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets all NVP.
        ''' </summary>
        ''' <param name="accountId">The account ID.</param>
        ''' <returns>DataTable.</returns>
        Public Function AllNameValuePairs(ByVal accountId As Integer) As DataTable
            Dim mRetVal As DataTable = Nothing
            m_DNameValuePair.SecurityEntityId = ConfigSettings.DefaultSecurityEntityId ' for future use ... the DB is capable of dividing the NVPs by BU
            m_DNameValuePair.AccountId = accountId
            m_DNameValuePair.NameValuePairProfile.Id = -1
            If DatabaseIsOnline() Then mRetVal = m_DNameValuePair.AllNameValuePairs
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the NVP detail.
        ''' </summary>
        ''' <param name="nameValuePairDetailSeqId">The NVP seq det ID.</param>
        ''' <param name="nameValuePairSeqID">The NVP seq ID.</param>
        ''' <returns>DataRow.</returns>
        Public Function GetNameValuePairDetails(ByVal nameValuePairDetailSeqId As Integer, ByVal nameValuePairSeqId As Integer) As DataRow
            Dim mRetVal As DataRow = Nothing
            If DatabaseIsOnline() Then mRetVal = m_DNameValuePair.NameValuePairDetails(nameValuePairDetailSeqId, nameValuePairSeqId)
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets all NVP detail.
        ''' </summary>
        ''' <returns>DataTable.</returns>
        Public Function GetAllNameValuePairDetail() As DataTable
            Dim mRetVal As DataTable = Nothing
            If DatabaseIsOnline() Then mRetVal = m_DNameValuePair.AllNameValuePairDetail
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets all NVP detail.
        ''' </summary>
        ''' <param name="nameValuePairSeqId">The NVP seq ID.</param>
        ''' <returns>DataTable.</returns>
        Public Function GetAllNameValuePairDetail(ByVal nameValuePairSeqId As Integer) As DataTable
            Dim mRetVal As DataTable = Nothing
            If DatabaseIsOnline() Then mRetVal = m_DNameValuePair.GetAllNameValuePairDetail(nameValuePairSeqId)
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the NVP.
        ''' </summary>
        ''' <param name="nameValuePairSeqId">The name value pair seq ID.</param>
        ''' <returns>DataRow.</returns>
        Public Function GetNameValuePair(ByVal nameValuePairSeqId As Integer) As DataRow
            Dim mRetVal As DataRow = Nothing
            m_DNameValuePair.NameValuePairProfile = New MNameValuePair()
            m_DNameValuePair.NameValuePairProfile.Id = nameValuePairSeqId
            If DatabaseIsOnline() Then mRetVal = m_DNameValuePair.NameValuePair
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the NVP roles.
        ''' </summary>
        ''' <param name="nameValuePairSeqId">The name value pair seq ID.</param>
        ''' <returns>DataTable.</returns>
        Public Function GetNameValuePairRoles(ByVal nameValuePairSeqId As Integer) As DataTable
            Dim mRetVal As DataTable = Nothing
            If DatabaseIsOnline() Then mRetVal = m_DNameValuePair.GetRoles(nameValuePairSeqId)
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the NVP groups.
        ''' </summary>
        ''' <param name="nameValuePairSeqId">The name value pair seq ID.</param>
        ''' <returns>DataTable.</returns>
        Public Function GetNameValuePairGroups(ByVal nameValuePairSeqId As Integer) As DataTable
            Dim mRetVal As DataTable = Nothing
            If DatabaseIsOnline() Then mRetVal = m_DNameValuePair.GetGroups(nameValuePairSeqId)
            Return mRetVal
        End Function

        ''' <summary>
        ''' Saves the specified profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Function Save(ByVal profile As MNameValuePair) As Integer
            Dim mRetVal As Integer = -1
            m_DNameValuePair.NameValuePairProfile = profile
            If DatabaseIsOnline() Then mRetVal = m_DNameValuePair.Save()
            Return mRetVal
        End Function

        ''' <summary>
        ''' Returns a data table given the search criteria
        ''' </summary>
        ''' <param name="searchCriteria">MSearchCriteria</param>
        ''' <returns>DataTable</returns>
        ''' <remarks></remarks>
        Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            Dim mRetVal As DataTable = Nothing
            If DatabaseIsOnline() Then mRetVal = m_DNameValuePair.Search(searchCriteria)
            Return mRetVal
        End Function

        ''' <summary>
        ''' Saves the name value pari detail.
        ''' </summary>
        ''' <param name="detailProfile">The detail profile.</param>
        Public Sub SaveNameValuePairDetail(ByVal detailProfile As MNameValuePairDetail)
            If DatabaseIsOnline() Then m_DNameValuePair.SaveNameValuePairDetail(detailProfile)
        End Sub

        ''' <summary>
        ''' Updates the groups.
        ''' </summary>
        ''' <param name="nameValuePairSeqId">The NameValuePair ID.</param>
        ''' <param name="securityEntityId">The security entity ID.</param>
        ''' <param name="commaSeparatedGroups">The comma Separated groups.</param>
        ''' <param name="nameValuePairProfile">MNameValuePair</param>
        Public Sub UpdateGroups(ByVal nameValuePairSeqId As Integer, ByVal securityEntityId As Integer, ByVal commaSeparatedGroups As String, ByVal nameValuePairProfile As MNameValuePair)
            If DatabaseIsOnline() Then m_DNameValuePair.UpdateGroups(nameValuePairSeqId, securityEntityId, commaSeparatedGroups, nameValuePairProfile)
        End Sub

        ''' <summary>
        ''' Updates the roles.
        ''' </summary>
        ''' <param name="nameValuePairSeqId">The NameValuePair sequence Id.</param>
        ''' <param name="securityEntityId">The security entity ID.</param>
        ''' <param name="commaSeparatedRoles">The comma Separated roles.</param>
        ''' <param name="nameValuePairProfile">MNameValuePair</param>
        Public Sub UpdateRoles(ByVal nameValuePairSeqId As Integer, ByVal securityEntityId As Integer, ByVal commaSeparatedRoles As String, ByVal nameValuePairProfile As MNameValuePair)
            If DatabaseIsOnline() Then m_DNameValuePair.UpdateRoles(nameValuePairSeqId, securityEntityId, commaSeparatedRoles, nameValuePairProfile)
        End Sub
    End Class
End Namespace
