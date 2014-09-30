Imports GrowthWare.Framework.Model.Profiles
Imports System.Collections.ObjectModel
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports System.Globalization
Imports GrowthWare.Framework.Common

Namespace BusinessLogicLayer
    ''' <summary>
    ''' Process business logic for functions
    ''' </summary>
    ''' <remarks>
    ''' <![CDATA[
    ''' MSecurityEntityProfile can be found in the GrowthWare.Framework.ModelObjects namespace.  
    ''' 
    ''' The following properties are necessary for correct business logic operation.
    ''' .ConnctionString
    ''' .DALName
    ''' .DALNameSpace
    ''' ]]>
    ''' </remarks>
    ''' <example> This sample shows how to create an instance of the class.
    ''' <code language="VB.NET">
    ''' <![CDATA[
    ''' Dim myBll as new BSecurityEntity(mySecurityEntityProfile, ConfigSettings.CentralManagement)
    ''' ]]>
    ''' </code>
    ''' </example>
    Public Class BSecurityEntity
        Inherits BaseBusinessLogic

        Private m_DSecurityEntity As IDSecurityEntity

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
        ''' BFunctions mBFunctions = BFunctions = New BFunctions(mSecurityEntityProfile, ConfigSettings.CentralManagement);
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
        ''' Dim mBFunctions As BFunctions = New BFunctions(mSecurityEntityProfile, ConfigSettings.CentralManagement)
        ''' ]]>
        ''' </code>
        ''' </example>
        Public Sub New(ByVal securityEntityProfile As MSecurityEntityProfile, ByVal centralManagement As Boolean)
            If (securityEntityProfile Is Nothing) Then
                Throw New ArgumentException("securityEntityProfile can not be null or empty!")
            End If
            If Not centralManagement Then
                If m_DSecurityEntity Is Nothing Then
                    m_DSecurityEntity = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DSecurityEntity")
                End If
            Else
                m_DSecurityEntity = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DSecurityEntity")
            End If
            m_DSecurityEntity.ConnectionString = securityEntityProfile.ConnectionString
        End Sub

        ''' <summary>
        ''' Returns a collection of MSecurityEntityProfile objects for the given.
        ''' </summary>
        ''' <returns>Collection(of MSecurityEntityProfile)</returns>
        ''' <remarks></remarks>
        Public Function SecurityEntities() As Collection(Of MSecurityEntityProfile)
            Dim mRetVal As Collection(Of MSecurityEntityProfile) = New Collection(Of MSecurityEntityProfile)
            Dim mDataTable As DataTable = Nothing
            Try
                If IsDatabaseOnline() Then
                    mDataTable = m_DSecurityEntity.GetSecurityEntities()
                    For Each item As DataRow In mDataTable.Rows
                        Dim mProfile As New MSecurityEntityProfile(item)
                        mRetVal.Add(mProfile)
                    Next
                End If
            Catch ex As Exception
                Throw
            Finally
                If Not mDataTable Is Nothing Then
                    mDataTable.Dispose()
                End If
            End Try
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the valid security entities.
        ''' </summary>
        ''' <param name="account">The account.</param>
        ''' <param name="securityEntityId">The security entity ID.</param>
        ''' <param name="isSecurityEntityAdministrator">if set to <c>true</c> [is security entity administrator].</param>
        ''' <returns>DataTable.</returns>
        Public Function GetValidSecurityEntities(ByVal account As String, ByVal securityEntityId As Integer, ByVal isSecurityEntityAdministrator As Boolean) As DataTable
            Dim mRetVal As DataTable = Nothing
            If IsDatabaseOnline() Then
                mRetVal = m_DSecurityEntity.GetValidSecurityEntities(account, securityEntityId, isSecurityEntityAdministrator)
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Save Function information to the database
        ''' </summary>
        ''' <param name="profile">MSecurityEntityProfile</param>
        ''' <returns>Integer</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")>
        Public Function Save(ByVal profile As MSecurityEntityProfile) As Integer
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile can not be Nothing")
            profile.Id = profile.Id
            If IsDatabaseOnline() Then m_DSecurityEntity.Save(profile)
            Return profile.Id
        End Function

        ''' <summary>
        ''' Returns a data table given the search criteria
        ''' </summary>
        ''' <param name="searchCriteria">MSearchCriteria</param>
        ''' <returns>DataTable</returns>
        ''' <remarks></remarks>
        Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            Dim mRetVal As DataTable = Nothing
            If IsDatabaseOnline() Then
                mRetVal = m_DSecurityEntity.Search(searchCriteria)
            End If
            Return mRetVal
        End Function

    End Class
End Namespace

