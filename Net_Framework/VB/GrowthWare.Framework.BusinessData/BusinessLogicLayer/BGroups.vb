﻿Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.BusinessData.DataAccessLayer

Namespace BusinessLogicLayer
    Public Class BGroups
        Inherits BaseBusinessLogic
        Private m_DGroups As IDGroups

        ''' <summary>
        ''' Private sub new() to ensure only new instances with passed parameters is used.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()

        End Sub

        Public Function Save(ByVal profile As MGroupProfile) As Integer
            m_DGroups.Profile = profile
            Return m_DGroups.Save()
        End Function

        ''' <summary>
        ''' Parameters are need to pass along to the factory for correct connection to the desired datastore.
        ''' </summary>
        ''' <param name="securityEntityProfile">The Security Entity profile used to obtain the DAL name, DAL name space, and the Connection String</param>
        ''' <param name="centralManagement">Boolean value indicating if the system is being used to manage multiple database instances.</param>
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
        ''' BGroups mBGroups = New BGroups(mSecurityEntityProfile, ConfigSettings.CentralManagement);
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
        ''' Dim mBGroups As BGroups = New BGroups(mSecurityEntityProfile, ConfigSettings.CentralManagement)
        ''' ]]>
        ''' </code>
        ''' </example>
        Public Sub New(ByVal securityEntityProfile As MSecurityEntityProfile, ByVal centralManagement As Boolean)
            If securityEntityProfile Is Nothing Then Throw New ArgumentNullException("securityEntityProfile", "securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!")
            If Not centralManagement Then
                If m_DGroups Is Nothing Then
                    m_DGroups = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DGroups")
                End If
            Else
                m_DGroups = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DGroups")
            End If
            m_DGroups.ConnectionString = securityEntityProfile.ConnectionString
            m_DGroups.SecurityEntitySeqId = securityEntityProfile.Id
        End Sub

        Public Function GetGroupsBySecurityEntity(ByVal securityEntityId As Integer) As DataTable
            Dim myProfile As New MGroupProfile
            myProfile.SecurityEntityId = securityEntityId
            m_DGroups.Profile = myProfile
            Return m_DGroups.GroupsBySecurityEntity
        End Function

        Public Sub AddGroup(ByVal profile As MGroupProfile)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!")
            m_DGroups.Profile = profile
            If DatabaseIsOnline() Then m_DGroups.Save()
        End Sub

        Public Function GetProfile(ByVal groupId As Integer) As MGroupProfile
            Dim retProfile As New MGroupProfile
            retProfile.Id = groupId
            m_DGroups.Profile = retProfile
            If DatabaseIsOnline() Then retProfile = New MGroupProfile(m_DGroups.ProfileData)
            Return retProfile
        End Function

        Public Sub DeleteGroup(ByVal profile As MGroupProfile)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!")
            m_DGroups.Profile = profile
            If DatabaseIsOnline() Then m_DGroups.DeleteGroup()
        End Sub

        Public Sub UpdateGroup(ByVal profile As MGroupProfile)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!")
            m_DGroups.Profile = profile
            If DatabaseIsOnline() Then m_DGroups.Save()
        End Sub

        ''' <summary>
        ''' Searches the specified search critera.
        ''' </summary>
        ''' <param name="searchCriteria">The search critera.</param>
        ''' <returns>DataTable.</returns>
        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            If searchCriteria Is Nothing Then Throw New ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!!")
            Dim mRetVal As DataTable = Nothing
            If DatabaseIsOnline() Then mRetVal = m_DGroups.Search(searchCriteria)
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the selected roles.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        ''' <returns>System.String[][].</returns>
        Public Function GetSelectedRoles(ByVal profile As MGroupRoles) As String()
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!")
            Dim ClientRoles As New ArrayList
            m_DGroups.GroupRolesProfile = profile
            Dim myDataTable As DataTable = Nothing
            If DatabaseIsOnline() Then
                Try
                    myDataTable = m_DGroups.GroupRoles
                    Dim myDR As DataRow
                    For Each myDR In myDataTable.Rows
                        ClientRoles.Add(myDR("Role").ToString)
                    Next
                Catch ex As DataAccessLayerException
                    Throw
                Finally
                    If Not myDataTable Is Nothing Then myDataTable.Dispose()
                End Try
            End If
            Return CType(ClientRoles.ToArray(GetType(String)), String())
        End Function

        ''' <summary>
        ''' Updates the group roles.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub UpdateGroupRoles(ByVal profile As MGroupRoles)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!!")
            m_DGroups.GroupRolesProfile = profile
            If DatabaseIsOnline() Then m_DGroups.UpdateGroupRoles()
        End Sub
    End Class
End Namespace

