Imports GrowthWare.Framework.Model.Profiles.Base
Imports System.Collections.ObjectModel
Imports GrowthWare.Framework.Model.Profiles.Interfaces

Namespace Model.Profiles
    ''' <summary>
    ''' Base properties an account Profile you can inherit this class and add
    ''' any properties or methods that suit your project needs.
    ''' </summary>
    ''' <remarks>
    ''' Corresponds to table [ZGWSecurity].[Accounts] and 
    ''' Store procedures: 
    ''' [ZGWSecurity].[Set_Account], [ZGWSecurity].[Get_Account]
    ''' </remarks>
    <Serializable(), CLSCompliant(True)> _
    Public Class MAccountProfile
        Inherits MProfile
        Implements IMGroupRoleSecurity

#Region "Constructors"
        ''' <summary>
        ''' Provides a new account profile with the default vaules
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Will populate values based on the contents of the data row.
        ''' </summary>
        ''' <param name="detailRow">Datarow containing base values</param>
        ''' <remarks>
        ''' Class should be inherited to extend to your project specific properties
        ''' </remarks>
        Public Sub New(ByVal detailRow As DataRow)
            Me.IdColumnName = "ACCT_SEQ_ID"
            Me.NameColumnName = "ACCT"
            Me.Initialize(detailRow)
        End Sub

        Public Sub New(ByVal detailRow As DataRow, ByVal assignedRolesData As DataTable, ByVal assignedGroupsData As DataTable, ByVal derivedRolesData As DataTable)
            If Not detailRow Is Nothing Then
                Me.Initialize(detailRow)
                If Not assignedRolesData Is Nothing Then setRolesOrGroups(m_AssignedRoles, assignedRolesData.Rows, m_RoleColumn)
                If Not assignedGroupsData Is Nothing Then setRolesOrGroups(m_Groups, assignedGroupsData.Rows, m_GroupColumn)
                If Not derivedRolesData Is Nothing Then setRolesOrGroups(m_DerivedRoles, derivedRolesData.Rows, m_RoleColumn)
            End If
        End Sub
#End Region

#Region "Member Fields"
        Private m_RoleColumn As String = "Roles"
        Private m_GroupColumn As String = "Groups"
        Private m_AssignedRoles As Collection(Of String) = New Collection(Of String)
        Private m_Groups As Collection(Of String) = New Collection(Of String)
        Private m_DerivedRoles As Collection(Of String) = New Collection(Of String)
#End Region

#Region "Public Properties"

        ''' <summary>
        ''' Represents the roles that have been directly assigned to the account.
        ''' </summary>
        Public ReadOnly Property AssignedRoles As Collection(Of String) Implements IMGroupRoleSecurity.AssignedRoles
            Get
                Return m_AssignedRoles
            End Get
        End Property

        ''' <summary>
        ''' Represents the roles that have been assigned either directly or through assoication of a role to a group.
        ''' </summary>
        Public ReadOnly Property DerivedRoles As Collection(Of String) Implements IMGroupRoleSecurity.DerivedRoles
            Get
                Return m_DerivedRoles
            End Get
        End Property

        ''' <summary>
        ''' Represents the groups that have been directly assigned to the account.
        ''' </summary>
        Public ReadOnly Property Groups As Collection(Of String) Implements IMGroupRoleSecurity.Groups
            Get
                Return m_Groups
            End Get
        End Property

        ''' <summary>
        ''' Represents the account
        ''' </summary>
        Public Property Account As String

        ''' <summary>
        ''' Represents the email address
        ''' </summary>
        Public Property Email() As String

        ''' <summary>
        ''' Represents the status of the account
        ''' </summary>
        Public Property Status() As Integer

        ''' <summary>
        ''' Indicates the last time the account password was changed
        ''' </summary>
        Public Property PasswordLastSet() As DateTime

        ''' <summary>
        ''' The password for the account
        ''' </summary>
        Public Property Password() As String

        ''' <summary>
        ''' The number of failed logon attemps
        ''' </summary>
        Public Property FailedAttempts() As Integer

        ''' <summary>
        ''' First name of the person for the account
        ''' </summary>
        Public Property FirstName() As String

        ''' <summary>
        ''' Indicates if the account is a system administrator ... used to
        ''' prevent complete lockout when the roles have been
        ''' damaged.
        ''' </summary>
        Public Property IsSystemAdmin() As Boolean

        ''' <summary>
        ''' Last name of the person for the account
        ''' </summary>
        Public Property LastName() As String

        ''' <summary>
        ''' Middle name of the person for the account
        ''' </summary>
        Public Property MiddleName() As String

        ''' <summary>
        ''' Prefered or nick name of the person for the account
        ''' </summary>
        Public Property PreferredName() As String

        ''' <summary>
        ''' The timezone for the account
        ''' </summary>
        Public Property TimeZone() As Integer

        ''' <summary>
        ''' The location of the account
        ''' </summary>
        Public Property Location() As String

        ''' <summary>
        ''' The date and time the account was last loged on
        ''' </summary>
        Public Property LastLogOn() As DateTime

        ''' <summary>
        ''' Used to determine if the client would like to recieve notifications.
        ''' </summary>
        Public Property EnableNotifications() As Boolean
        ''' <summary>
        ''' Converts the collection of AssignedRoles to a comma Separated string.
        ''' </summary>
        ''' <returns>String</returns>
        Public ReadOnly Property GetCommaSeparatedAssignedRoles As String
            Get
                Return getCommaSeportatedString(m_AssignedRoles)
            End Get
        End Property


        ''' <summary>
        ''' Converts the collection of AssignedGroups to a comma Separated string.
        ''' </summary>
        ''' <returns>String</returns>
        Public ReadOnly Property GetCommaSeparatedAssignedGroups As String
            Get
                Return getCommaSeportatedString(m_Groups)
            End Get
        End Property

        ''' <summary>
        ''' Converts the collection of DerivedRoles to a comma Separated string.
        ''' </summary>
        ''' <returns>String</returns>
        Public ReadOnly Property GetCommaSeparatedDerivedRoles As String
            Get
                Return getCommaSeportatedString(m_DerivedRoles)
            End Get
        End Property

#End Region

#Region "Protected Methods"
        ''' <summary>
        ''' Populates direct properties as well as passing the DataRow to the abstract class
        ''' for the population of the base properties.
        ''' </summary>
        ''' <param name="DetailRow">DataRow</param>
        Protected Shadows Sub Initialize(ByVal detailRow As DataRow)
            MyBase.IdColumnName = "Account_SeqID"
            MyBase.NameColumnName = "ACCT"
            MyBase.Initialize(detailRow)
            Account = MyBase.GetString(detailRow, "ACCT")
            Email = MyBase.GetString(detailRow, "EMAIL")
            EnableNotifications = MyBase.GetBool(detailRow, "ENABLE_NOTIFICATIONS")
            IsSystemAdmin = MyBase.GetBool(detailRow, "IS_SYSTEM_ADMIN")
            Status = MyBase.GetInt(detailRow, "STATUS_SEQ_ID")
            Password = MyBase.GetString(detailRow, "PWD")
            FailedAttempts = MyBase.GetInt(detailRow, "FAILED_ATTEMPTS")
            FirstName = MyBase.GetString(detailRow, "FIRST_NAME")
            LastLogOn = MyBase.GetDateTime(detailRow, "LAST_LOGIN", Date.Now)
            LastName = MyBase.GetString(detailRow, "LAST_NAME")
            Location = MyBase.GetString(detailRow, "LOCATION")
            PasswordLastSet = MyBase.GetDateTime(detailRow, "PASSWORD_LAST_SET", Date.Now)
            MiddleName = MyBase.GetString(detailRow, "MIDDLE_NAME")
            PreferredName = MyBase.GetString(detailRow, "Preferred_Name")
            TimeZone = MyBase.GetInt(detailRow, "TIME_ZONE")
        End Sub
#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Will set the collection of roles given a comma Separated string of roles.
        ''' </summary>
        ''' <param name="commaSeparatedRoles">String of comma Separated roles</param>
        Public Sub SetRoles(ByVal commaSeparatedRoles As String)
            setRolesOrGroups(m_AssignedRoles, commaSeparatedRoles)
        End Sub

        ''' <summary>
        ''' Will set the collection of groups given a comma Separated string of groups.
        ''' </summary>
        ''' <param name="commaSeparatedGroups">String of comma Separated groups</param>
        Public Sub SetGroups(ByVal commaSeparatedGroups As String)
            setRolesOrGroups(m_Groups, commaSeparatedGroups)
        End Sub
#End Region

#Region "Private Methods"
        ''' <summary>
        ''' Sets the assigned roles or groups.
        ''' </summary>
        ''' <param name="StringCollectionObject">The collection of roles or groups that need to be set</param>
        ''' <param name="GroupsOrRoles">The DataRowCollection that represents either roles or groups</param>
        ''' <param name="ColumnName">The column name to retrieve the data from</param>
        Private Shared Sub setRolesOrGroups(ByRef stringCollectionObject As Collection(Of String), ByVal groupsOrRoles As DataRowCollection, ByVal columnName As String)
            For Each mRow In groupsOrRoles
                If Not IsDBNull(mRow(columnName)) Then
                    stringCollectionObject.Add(mRow(columnName).ToString())
                End If
            Next
        End Sub

        ''' <summary>
        ''' Sets the assigned roles or groups.
        ''' </summary>
        ''' <param name="stringCollectionObject">The collection of roles or groups that need to be set</param>
        ''' <param name="commaSeparatedString">A comma Separated list of roles or groups 'you, me' as an example</param>
        Private Shared Sub setRolesOrGroups(ByRef stringCollectionObject As Collection(Of String), ByRef commaSeparatedString As String)
            Dim mRoles() As String = commaSeparatedString.Split(",")
            stringCollectionObject = New Collection(Of String)
            For Each mRole In mRoles
                stringCollectionObject.Add(mRole.ToString())
            Next
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="collectionOfStrings"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function getCommaSeportatedString(ByVal collectionOfStrings As Collection(Of String)) As String
            Dim mRetValue As String = String.Empty
            If Not collectionOfStrings Is Nothing Then
                If collectionOfStrings.Count > 0 Then
                    For Each item In collectionOfStrings
                        mRetValue += item.ToString() + ","
                    Next
                End If
            End If
            If mRetValue.Length > 0 Then
                mRetValue = mRetValue.Substring(0, mRetValue.Length - 1)
            End If
            Return mRetValue
        End Function

#End Region
    End Class
End Namespace
