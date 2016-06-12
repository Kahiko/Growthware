Imports GrowthWare.Framework.Model.Profiles.Base
Imports GrowthWare.Framework.Model.Profiles.Interfaces
Imports System.Globalization

Namespace Model.Profiles
    ''' <summary>
    ''' Base properties an account Profile you can inherit this class and add
    ''' any properties or methods that suit your project needs.
    ''' </summary>
    ''' <remarks>
    ''' Corresponds to table [ZGWSecurity].[Functions] and 
    ''' Store procedures: 
    ''' [ZGWSecurity].[Set_Function], [ZGWSecurity].[Get_Function]
    ''' </remarks>
    <Serializable(), CLSCompliant(True)> _
    Public Class MFunctionProfile
        Inherits MGroupRolePermissionSecurity
        Implements IMProfile

#Region "Member Fields"
        Private m_NavigationTypeSeqId As Integer = 2
        Private m_FunctionTypeSeqId As Integer = -1
        'Private m_ALLOW_HTML_INPUT As Integer = 1
        'Private m_ALLOW_COMMENT_HTML_INPUT As Integer = 1
        Private m_ParentFunctionSeqId As Integer = 1
        Private m_LinkBehavior As Integer = 1
#End Region

#Region "Pubic methods"
        ''' <summary>
        ''' Will return a Function profile with the default vaules
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Will return a fully populated Function profile.
        ''' </summary>
        ''' <param name="profileDataRow">A data row containing the Function information</param>
        ''' <param name="derivedRoles">A data row containing all of the derived roles</param>
        ''' <param name="assignedRoles">A data row containing all of the assigned roles</param>
        ''' <param name="groups">A data row containing all of the assigned groups</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal profileDataRow As DataRow, ByVal derivedRoles() As DataRow, ByVal assignedRoles() As DataRow, ByVal groups() As DataRow)
            Initialize(profileDataRow, derivedRoles, assignedRoles, groups)
        End Sub
#End Region

#Region "Private methods"
        ''' <summary>
        ''' Populates the profile.
        ''' </summary>
        ''' <param name="profileDataRow">Data row containing the profile information</param>
        ''' <param name="derivedRoles">An array of data rows</param>
        ''' <param name="assignedRoles">An array of data rows</param>
        ''' <param name="groups">An array of data rows</param>
        ''' <remarks></remarks>
        Friend Overloads Sub Initialize(ByVal profileDataRow As DataRow, ByVal derivedRoles() As DataRow, ByVal assignedRoles() As DataRow, ByVal groups() As DataRow)
            MyBase.IdColumnName = "FUNCTION_SEQ_ID"
            MyBase.NameColumnName = "NAME"
            MyBase.Initialize(profileDataRow, derivedRoles, assignedRoles, groups)
            Action = MyBase.GetString(profileDataRow, "ACTION")
            Description = MyBase.GetString(profileDataRow, "DESCRIPTION")
            EnableNotifications = MyBase.GetBool(profileDataRow, "ENABLE_NOTIFICATIONS")
            EnableViewState = MyBase.GetBool(profileDataRow, "ENABLE_VIEW_STATE")
            IsNavigable = MyBase.GetBool(profileDataRow, "IS_NAV")
            LinkBehavior = MyBase.GetInt(profileDataRow, "LINK_BEHAVIOR")
            MetaKeywords = MyBase.GetString(profileDataRow, "META_KEY_WORDS")
            NavigationTypeSeqId = MyBase.GetInt(profileDataRow, "NAVIGATION_NVP_SEQ_DET_ID")
            Notes = MyBase.GetString(profileDataRow, "NOTES")
            NoUI = MyBase.GetBool(profileDataRow, "No_UI")
            RedirectOnTimeout = MyBase.GetBool(profileDataRow, "REDIRECT_ON_TIMEOUT")
            Source = MyBase.GetString(profileDataRow, "SOURCE")
            Controller = MyBase.GetString(profileDataRow, "Controller")
            m_ParentFunctionSeqId = MyBase.GetInt(profileDataRow, "PARENT_FUNCTION_SEQ_ID")
            m_FunctionTypeSeqId = MyBase.GetInt(profileDataRow, "FUNCTION_TYPE_SEQ_ID")
        End Sub
#End Region

#Region "Public properties"
        ''' <summary>
        ''' Represents the Action to be take within the system.
        ''' </summary>
        ''' <remarks>This is a unique value</remarks>
        Public Property Action() As String

        ''' <summary>
        ''' Used as description of the profile
        ''' </summary>
        ''' <remarks>Designed to be used in any search options</remarks>
        Public Property Description() As String

        ''' <summary>
        ''' Indicates to the system if the "page's" view state should be enabled.
        ''' </summary>
        ''' <remarks>Legacy usage</remarks>
        Public Property EnableViewState() As Boolean

        ''' <summary>
        ''' Intended to be used to send notifications when this profile is "used" by the client
        ''' </summary>
        Public Property EnableNotifications() As Boolean

        ''' <summary>
        ''' Use to determine if a function is a navigation function
        ''' </summary>
        ''' <remarks>
        ''' Should be replaced by LinkBehavior
        ''' </remarks>
        Public Property IsNavigable() As Boolean

        ''' <summary>
        ''' Represents the link behavior of a function.
        ''' </summary>
        ''' <returns>Integer</returns>
        ''' <remarks>
        ''' Data stored in ZGWSecurity.Functions and related to ZGWCoreWeb.Link_Behaviors
        ''' </remarks>
        Public Property LinkBehavior() As Integer
            Get
                Return m_LinkBehavior
            End Get
            Set(value As Integer)
                m_LinkBehavior = value
            End Set
        End Property

        ''' <summary>
        ''' Represents the type of function Module,Security, Menu Item etc
        ''' </summary>
        ''' <value>Integer/int</value>
        ''' <returns>Integer/int</returns>
        ''' <remarks>
        ''' Data stored in ZGWSecurity.Functions related to ZGWSecurity.Function_Types
        ''' </remarks>
        Public Property FunctionTypeSeqId() As Integer
            Get
                Return m_FunctionTypeSeqId
            End Get
            Set(ByVal value As Integer)
                m_FunctionTypeSeqId = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the meta keywords.
        ''' </summary>
        ''' <value>The meta keywords.</value>
        Public Property MetaKeywords() As String

        Public Property NavigationTypeSeqId() As Integer
            Get
                Return m_NavigationTypeSeqId
            End Get
            Set(ByVal Value As Integer)
                m_NavigationTypeSeqId = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the notes.
        ''' </summary>
        ''' <value>The notes.</value>
        Public Property Notes() As String

        ''' <summary>
        ''' Gets or sets a value indicating whether [no UI].
        ''' </summary>
        ''' <value><c>true</c> if [no UI]; otherwise, <c>false</c>.</value>
        Public Property NoUI() As Boolean

        Public Property ParentId() As Integer
            Get
                Return m_ParentFunctionSeqId
            End Get
            Set(ByVal Value As Integer)
                m_ParentFunctionSeqId = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating whether [redirect on timeout].
        ''' </summary>
        ''' <value><c>true</c> if [redirect on timeout]; otherwise, <c>false</c>.</value>
        Public Property RedirectOnTimeout() As Boolean

        ''' <summary>
        ''' Gets or sets the source.
        ''' </summary>
        ''' <value>The source.</value>
        Public Property Source() As String

        ''' <summary>
        ''' Gets or sets the controller.
        ''' </summary>
        ''' <value>The controller.</value>
        Public Property Controller() As String
#End Region

    End Class

End Namespace
