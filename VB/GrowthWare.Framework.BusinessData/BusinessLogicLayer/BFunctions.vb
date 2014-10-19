Imports GrowthWare.Framework.Model.Enumerations
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
    ''' Dim myBll as new BFunctions(mySecurityEntityProfile, ConfigSettings.CentralManagement)
    ''' ]]>
    ''' </code>
    ''' </example>
    Public Class BFunctions
        Inherits BaseBusinessLogic

        Private m_DFunctions As IDFunction

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
                If m_DFunctions Is Nothing Then
                    m_DFunctions = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DFunctions")
                End If
            Else
                m_DFunctions = ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DFunctions")
            End If
            m_DFunctions.ConnectionString = securityEntityProfile.ConnectionString
            m_DFunctions.SecurityEntitySeqId = securityEntityProfile.Id
        End Sub

        ''' <summary>
        ''' Returns a collection of MFunctionProfile objects for the given
        ''' security entity.
        ''' </summary>
        ''' <param name="securityEntitySeqId">Integer</param>
        ''' <returns>Collection(of MFunctionProfile)</returns>
        ''' <remarks></remarks>
        Public Function GetFunctions(ByVal securityEntitySeqId As Integer) As Collection(Of MFunctionProfile)
            'Dim mRetVal As MFunctionProfileCollection = New MFunctionProfileCollection()
            Dim mRetVal As Collection(Of MFunctionProfile) = New Collection(Of MFunctionProfile)
            Dim mDSFunctions As DataSet = Nothing
            If IsDatabaseOnline() Then
                Try
                    'mDSFunctions = New DataSet
                    m_DFunctions.Profile = New MFunctionProfile
                    m_DFunctions.SecurityEntitySeqId = securityEntitySeqId
                    mDSFunctions = m_DFunctions.GetFunctions()
                    Dim mHasAssingedRoles As Boolean = False
                    Dim mHasGroups As Boolean = False
                    If mDSFunctions.Tables(1).Rows.Count > 0 Then mHasAssingedRoles = True
                    If mDSFunctions.Tables(2).Rows.Count > 0 Then mHasGroups = True
                    Dim mGroups() As DataRow = Nothing
                    Dim mAssignedRoles() As DataRow = Nothing
                    Dim mDerivedRoles() As DataRow = Nothing
                    For Each item As DataRow In mDSFunctions.Tables("Functions").Rows
                        mDerivedRoles = item.GetChildRows("DerivedRoles")
                        mAssignedRoles = Nothing
                        If mHasAssingedRoles Then mAssignedRoles = item.GetChildRows("AssignedRoles")
                        mGroups = Nothing
                        If mHasGroups Then mGroups = item.GetChildRows("Groups")
                        Dim mProfile As New MFunctionProfile(item, mDerivedRoles, mAssignedRoles, mGroups)
                        'mRetVal.Add(mProfile.Id, mProfile)
                        mRetVal.Add(mProfile)
                    Next
                Catch ex As Exception
                    Throw
                Finally
                    If Not mDSFunctions Is Nothing Then
                        mDSFunctions.Dispose()
                    End If
                End Try
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the function types.
        ''' </summary>
        ''' <returns>DataTable.</returns>
        Public Function FunctionTypes() As DataTable
            Dim mRetVal As DataTable = Nothing
            If IsDatabaseOnline() Then
                mRetVal = m_DFunctions.FunctionTypes()
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the menu order.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        ''' <returns>DataTable.</returns>
        Public Function GetMenuOrder(ByVal profile As MFunctionProfile) As DataTable
            Dim mRetVal As DataTable = Nothing
            If IsDatabaseOnline() Then
                mRetVal = m_DFunctions.GetMenuOrder(profile)
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Save Function information to the database
        ''' </summary>
        ''' <param name="profile">MFunctionProfile</param>
        ''' <param name="saveGroups">Boolean</param>
        ''' <param name="saveRoles">Boolean</param>
        Public Sub Save(ByVal profile As MFunctionProfile, ByVal saveGroups As Boolean, ByVal saveRoles As Boolean)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            If IsDatabaseOnline() Then
                m_DFunctions.Profile = profile
                profile.Id = m_DFunctions.Save()
                m_DFunctions.Profile = profile
                If saveGroups Then
                    m_DFunctions.SaveGroups(PermissionType.Add)
                    m_DFunctions.SaveGroups(PermissionType.Delete)
                    m_DFunctions.SaveGroups(PermissionType.Edit)
                    m_DFunctions.SaveGroups(PermissionType.View)
                End If
                If saveRoles Then
                    m_DFunctions.SaveRoles(PermissionType.Add)
                    m_DFunctions.SaveRoles(PermissionType.Delete)
                    m_DFunctions.SaveRoles(PermissionType.Edit)
                    m_DFunctions.SaveRoles(PermissionType.View)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Returns a data table given the search criteria
        ''' </summary>
        ''' <param name="searchCriteria">MSearchCriteria</param>
        ''' <returns>DataTable</returns>
        ''' <remarks></remarks>
        Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            If searchCriteria Is Nothing Then Throw New ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!")
            Dim mRetVal As DataTable = Nothing
            If IsDatabaseOnline() Then
                mRetVal = m_DFunctions.Search(searchCriteria)
            End If
            Return mRetVal
        End Function

        Public Sub Delete(ByVal functionSeqId As Integer)
            If IsDatabaseOnline() Then m_DFunctions.Delete(functionSeqId)
        End Sub

        Public Sub MoveMenuOrder(ByVal profile As MFunctionProfile, ByVal direction As DirectionType)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            If profile Is Nothing Then Throw New ArgumentNullException("direction", "direction cannot be a null reference (Nothing in Visual Basic)!")
            If IsDatabaseOnline() Then
                m_DFunctions.UpdateMenuOrder(profile, direction)
            End If
        End Sub
    End Class

End Namespace
