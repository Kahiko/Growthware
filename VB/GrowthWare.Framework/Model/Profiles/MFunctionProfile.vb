Imports GrowthWare.Framework.Model.Profiles.Base
Imports GrowthWare.Framework.Model.Profiles.Interfaces

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
        Private m_NavTypeSeqId As Integer = 2
        Private m_Function_Type_Seq_ID As Integer = 1
        'Private m_ALLOW_HTML_INPUT As Integer = 1
        'Private m_ALLOW_COMMENT_HTML_INPUT As Integer = 1
        Private m_ParentmFunction_Seq_ID As Integer = 1
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
        ''' <param name="profileDatarow">A data row containing the Function information</param>
        ''' <param name="derivedRoles">A data row containing all of the derived roles</param>
        ''' <param name="assignedRoles">A data row containing all of the assigned roles</param>
        ''' <param name="groups">A data row containing all of the assigned groups</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal profileDatarow As DataRow, ByVal derivedRoles() As DataRow, ByVal assignedRoles() As DataRow, ByVal groups() As DataRow)
            Initialize(profileDatarow, derivedRoles, assignedRoles, groups)
        End Sub
#End Region

#Region "Private methods"
        ''' <summary>
        ''' Popluates the profile.
        ''' </summary>
        ''' <param name="profileDatarow">Datarow containing the profile information</param>
        ''' <param name="derivedRoles">An array of datarows</param>
        ''' <param name="assignedRoles">An array of datarows</param>
        ''' <param name="groups">An array of datarows</param>
        ''' <remarks></remarks>
        Protected Overloads Sub Initialize(ByVal profileDatarow As DataRow, ByVal derivedRoles() As DataRow, ByVal assignedRoles() As DataRow, ByVal groups() As DataRow)
            MyBase.Id = CInt(profileDatarow("FUNCTION_SEQ_ID"))
            MyBase.IdColumnName = "FUNCTION_SEQ_ID"
            m_Function_Type_Seq_ID = Me.GetInt(profileDatarow, "FUNCTION_TYPE_SEQ_ID")
            Name = Me.GetString(profileDatarow, "NAME")
            Description = Me.GetString(profileDatarow, "DESCRIPTION")
            Notes = Me.GetString(profileDatarow, "NOTES")
            Source = Me.GetString(profileDatarow, "SOURCE")
            EnableViewState = Me.GetBool(profileDatarow, "ENABLE_VIEW_STATE")
            EnableNotifications = Me.GetBool(profileDatarow, "ENABLE_NOTIFICATIONS")
            RedirectOnTimeout = Me.GetBool(profileDatarow, "REDIRECT_ON_TIMEOUT")
            IsNav = Me.GetBool(profileDatarow, "IS_NAV")
            LinkBehavior = Me.GetInt(profileDatarow, "LINK_BEHAVIOR")
            NoUI = Me.GetBool(profileDatarow, "No_UI")
            NavigationTypeSeqId = Me.GetInt(profileDatarow, "NAVIGATION_NVP_SEQ_DET_ID")
            m_ParentmFunction_Seq_ID = Me.GetInt(profileDatarow, "PARENT_FUNCTION_SEQ_ID")
            Action = Me.GetString(profileDatarow, "ACTION")
            ' need to set the the base class name with the action.
            ' the names can repeate but the action is unique and lower case.
            MyBase.Name = Action.ToLower
            MetaKeyWords = Me.GetString(profileDatarow, "META_KEY_WORDS")
            MyBase.Initialize(profileDatarow, derivedRoles, assignedRoles, groups)
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
        ''' Use to determin if a function is a navigation function
        ''' </summary>
        ''' <remarks>
        ''' Should be replaced by LinkBehavior
        ''' </remarks>
        Public Property IsNav() As Boolean

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
        Public Property FunctionTypeSeqID() As Integer
            Get
                Return m_Function_Type_Seq_ID
            End Get
            Set(ByVal value As Integer)
                m_Function_Type_Seq_ID = value
            End Set
        End Property

        Public Property MetaKeyWords() As String

        Public Shadows Property Name() As String

        Public Property NavigationTypeSeqId() As Integer
            Get
                Return m_NavTypeSeqId
            End Get
            Set(ByVal Value As Integer)
                m_NavTypeSeqId = Value
            End Set
        End Property

        Public Property Notes() As String

        Public Property NoUI() As Boolean

        Public Property ParentID() As Integer
            Get
                Return m_ParentmFunction_Seq_ID
            End Get
            Set(ByVal Value As Integer)
                m_ParentmFunction_Seq_ID = Value
            End Set
        End Property

        Public Property RedirectOnTimeout() As Boolean

        Public Property Source() As String
#End Region

    End Class

End Namespace
