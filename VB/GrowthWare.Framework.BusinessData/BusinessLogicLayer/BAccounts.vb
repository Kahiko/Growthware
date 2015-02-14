Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports System.Collections.ObjectModel
Imports System.Globalization
Imports GrowthWare.Framework.Common

Namespace BusinessLogicLayer
    Public Class BAccounts
        Inherits BaseBusinessLogic

        Private m_DAccounts As IDAccount

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
                Throw New ArgumentNullException("securityEntityProfile", "securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!")
            End If
            If Not centralManagement Then
                If m_DAccounts Is Nothing Then
                    m_DAccounts = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DAccounts")
                End If
            Else
                m_DAccounts = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DAccounts")
            End If
            m_DAccounts.ConnectionString = securityEntityProfile.ConnectionString
            m_DAccounts.SecurityEntitySeqId = securityEntityProfile.Id
        End Sub

        Public Sub Delete(ByVal accountId As Integer)
            m_DAccounts.Profile = New MAccountProfile()
            m_DAccounts.Profile.Id = accountId
            If DatabaseIsOnline() Then m_DAccounts.Delete()
        End Sub

        ''' <summary>
        ''' Returns Account model based given the acccount name 
        ''' </summary>
        ''' <param name="account">String</param>
        ''' <returns>MAccountProfile</returns>
        ''' <remarks></remarks>
        ''' <example> This sample shows how to create an instance of the class.
        ''' <code language="VB.NET">
        ''' <![CDATA[
        ''' Dim mBll as new BAccounts(mySecurityEntityProfile)
        ''' Dim mMAccountProfile as MAccountProfile = mbill.GetAccountProfile("Tester")
        ''' ]]>
        ''' </code>
        ''' <code language="C#">
        ''' <![CDATA[
        ''' BAccounts mBll = new BAccounts(mySecurityEntityProfile);
        ''' MAccountProfile mMAccountProfile = mbill.GetAccountProfile("Tester");
        ''' ]]>
        ''' </code>
        ''' </example>
        Public Function GetProfile(ByVal account As String) As MAccountProfile
            Dim mRetVal As MAccountProfile = Nothing
            m_DAccounts.Profile = New MAccountProfile()
            m_DAccounts.Profile.Account = account
            If DatabaseIsOnline() Then
                mRetVal = New MAccountProfile(m_DAccounts.GetAccount, m_DAccounts.Roles(), m_DAccounts.Groups(), m_DAccounts.Security())
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Retrieves menu data for a given account and MenuType
        ''' </summary>
        ''' <param name="account">String</param>
        ''' <param name="menuType">MenuType</param>
        ''' <returns>DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetMenu(account, menuType) As DataTable
            Dim mRetVal As DataTable = Nothing
            If DatabaseIsOnline() Then
                mRetVal = m_DAccounts.GetMenu(account, menuType)
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Returns a collection of MAccountProfiles without any role information
        ''' </summary>
        ''' <param name="Profile">An instance of MAccountProfile</param>
        ''' <returns></returns>
        Public Function GetAccounts(ByVal profile As MAccountProfile) As Collection(Of MAccountProfile)
            Dim mDataTable As DataTable = Nothing
            Dim mRetCollection As New Collection(Of MAccountProfile)
            Try
                m_DAccounts.Profile = profile
                If DatabaseIsOnline() Then
                    mDataTable = m_DAccounts.GetAccounts()
                End If
                If mDataTable IsNot Nothing Then
                    For Each dataRow As DataRow In mDataTable.Rows
                        mRetCollection.Add(New MAccountProfile(dataRow))
                    Next
                End If
            Catch ex As Exception
                Throw
            Finally
                If Not mDataTable Is Nothing Then
                    mDataTable.Dispose()
                End If
            End Try
            Return mRetCollection
        End Function

        ''' <summary>
        ''' Performs either insert or update of an MAccountProfile, and re-populates the MAccountProfile with DB information.
        ''' </summary>
        ''' <param name="profile">MAccountProfile</param>
        ''' <param name="saveRoles">MAccountProfile</param>
        ''' <param name="saveGroups">MAccountProfile</param>
        ''' <remarks>
        ''' Updates the model object with information from the database<br></br>
        ''' For example if you are creating a new account the ID will be sent into<br></br>
        ''' this method as -1, after the call to this method the ID will from the database
        ''' </remarks>
        ''' <example> This sample shows how to create an instance of the class.
        ''' <code language="VB.NET">
        ''' <![CDATA[
        ''' Dim mMAccountProfile as new MAccountProfile()
        ''' mMAccountProfile.Account = "Account"
        ''' mMAccountProfile.Password = CryptoUtility.Encrypt("my password", ConfigSettings.EncryptionType)
        ''' mMAccountProfile.UpdatedBy = 1
        ''' mMAccountProfile.UpdatedDate = Date.Now
        ''' Dim mBll as new BAccounts(mySecurityEntityProfile)
        ''' Dim mSaveRoles As Boolean = False;
        ''' Dim mSaveGroups As Boolean = False;
        ''' mMAccountProfile = mBll.SaveAccount(mMAccountProfile, mSaveRoles, mSaveGroups)
        ''' ]]>
        ''' </code>
        ''' <code language="C#">
        ''' MAccountProfile mMAccountProfile = new mMAccountProfile();
        ''' mMAccountProfile.Account = "Account";
        ''' mMAccountProfile.Password = CryptoUtility.Encrypt("my password", ConfigSettings.EncryptionType);
        ''' mMAccountProfile.UpdatedBy = 1;
        ''' mMAccountProfile.UpdatedDate = Date.Now();
        ''' BAccounts mBll = new BAccounts(mySecurityEntityProfile);
        ''' bool mSaveRoles = false;
        ''' bool mSaveGroups = true;
        ''' mMAccountProfile = mBll.SaveAccount(ref mMAccountProfile, mSaveRoles, mSaveGroups);
        ''' </code>
        ''' </example>
        Public Sub Save(ByVal profile As MAccountProfile, ByVal saveRoles As Boolean, ByVal saveGroups As Boolean)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile can not be null (Nothing in Visual Basic)")

            If DatabaseIsOnline() Then
                m_DAccounts.Profile = profile
                profile.Id = m_DAccounts.Save()
                If saveGroups Then
                    m_DAccounts.SaveGroups()
                End If
                If saveRoles Then
                    m_DAccounts.SaveRoles()
                End If
                profile = New MAccountProfile(m_DAccounts.GetAccount(), m_DAccounts.Roles(), m_DAccounts.Groups(), m_DAccounts.Security())
            End If
        End Sub

        ''' <summary>
        ''' Returns a data table given the search criteria
        ''' </summary>
        ''' <param name="searchCriteria">MSearchCriteria</param>
        ''' <returns>DataTable</returns>
        ''' <remarks></remarks>
        Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            Dim mRetVal As DataTable = Nothing
            If DatabaseIsOnline() Then
                mRetVal = m_DAccounts.Search(searchCriteria)
            End If
            Return mRetVal
        End Function
    End Class
End Namespace
