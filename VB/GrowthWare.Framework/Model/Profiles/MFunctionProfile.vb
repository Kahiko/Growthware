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
        Inherits MBaseSecurity
        Implements IMProfile

        Private m_Action As String = String.Empty
        Private m_Description As String = String.Empty
        Private m_Enable_View_State As Boolean = False
        Private m_Enable_Notifications As Boolean = False
        Private m_Is_Nav As Boolean = False
        Private m_No_UI As Boolean = False
        Private m_Function_Seq_ID As Integer = -1
        Private m_Function_Name As String = String.Empty
        Private m_Nav_Type_Seq_ID As Integer = 2
        Private m_Notes As String = String.Empty
        Private m_Function_Type_Seq_ID = -1
        Private m_ALLOW_HTML_INPUT As Integer = 1
        Private m_ALLOW_COMMENT_HTML_INPUT As Integer = 1
        Private m_ParentmFunction_Seq_ID As Integer = 1
        Private m_Source As String = String.Empty
        Private m_Transformations As String = String.Empty
        Private m_Redirect_On_Timeout As Boolean = True
        Private m_MetaKeyWords As String
        ''' <summary>
        ''' Will return a Function profile with the default vaules
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Will return a fully populated Function profile.
        ''' </summary>
        ''' <param name="dataRowMain">A data row containing the Function information</param>
        ''' <param name="dataRowSecurity">A collection of data rows containing all of the roles</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal dataRowMain As DataRow, ByVal dataRowSecurity() As DataRow)
            Me.Initialize(dataRowMain, dataRowSecurity)
        End Sub

        Protected Shadows Sub Initialize(ByVal dataRowMain As DataRow, ByVal dataRowSecurity() As DataRow)
            On Error Resume Next
            MyBase.NameColumnName = "ACTION"
            MyBase.IdColumnName = "FUNCTION_SEQ_ID"
            MyBase.Initialize(dataRowMain, dataRowSecurity)
            m_Function_Type_Seq_ID = CInt(dataRowMain("FUNCTION_TYPE_SEQ_ID"))
            m_Function_Name = CStr(dataRowMain("NAME"))
            m_Description = CStr(dataRowMain("DESCRIPTION"))
            m_Notes = CStr(dataRowMain("NOTES"))
            m_Source = CStr(dataRowMain("SOURCE"))
            m_Enable_View_State = CStr(dataRowMain("ENABLE_VIEW_STATE"))
            m_Enable_Notifications = CStr(dataRowMain("ENABLE_NOTIFICATIONS"))
            m_Redirect_On_Timeout = CStr(dataRowMain("REDIRECT_ON_TIMEOUT"))
            m_Is_Nav = CStr(dataRowMain("IS_NAV"))
            m_No_UI = CStr(dataRowMain("No_UI"))
            m_Nav_Type_Seq_ID = CInt(dataRowMain("NAVIGATION_NVP_SEQ_DET_ID"))
            m_ParentmFunction_Seq_ID = CInt(dataRowMain("PARENT_Function_Seq_ID"))
            m_Action = Trim(CStr(dataRowMain("ACTION")))
            ' need to set the the base class name with the action.
            ' the names can repeate but the action is unique and lower case.
            MyBase.Name = m_Action.ToLower
            m_MetaKeyWords = Trim(CStr(dataRowMain("META_KEY_WORDS")))
        End Sub

        Public Property Action() As String
            Get
                Return m_Action
            End Get
            Set(ByVal Value As String)
                m_Action = Value.Trim
                MyBase.Name = m_Action.ToLower
            End Set
        End Property

        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal Value As String)
                m_Description = Value.Trim
            End Set
        End Property

        Public Property EnableViewState() As Boolean
            Get
                Return m_Enable_View_State
            End Get
            Set(ByVal Value As Boolean)
                m_Enable_View_State = Value
            End Set
        End Property

        Public Property EnableNotifications() As Boolean
            Get
                Return m_Enable_Notifications
            End Get
            Set(ByVal value As Boolean)
                m_Enable_Notifications = value
            End Set
        End Property

        Public Property IS_NAV() As Boolean
            Get
                Return m_Is_Nav
            End Get
            Set(ByVal Value As Boolean)
                m_Is_Nav = Value
            End Set
        End Property

        Public Property Function_Type_Seq_ID() As Integer
            Get
                Return m_Function_Type_Seq_ID
            End Get
            Set(ByVal value As Integer)
                m_Function_Type_Seq_ID = value
            End Set
        End Property

        Public Property MetaKeyWords() As String
            Get
                Return m_MetaKeyWords
            End Get
            Set(ByVal value As String)
                m_MetaKeyWords = value.Trim
            End Set
        End Property

        Public Shadows Property Name() As String
            Get
                Return m_Function_Name
            End Get
            Set(ByVal Value As String)
                m_Function_Name = Value.Trim
            End Set
        End Property

        Public Property NAV_TYPE_SEQ_ID() As Integer
            Get
                Return m_Nav_Type_Seq_ID
            End Get
            Set(ByVal Value As Integer)
                m_Nav_Type_Seq_ID = Value
            End Set
        End Property

        Public Property Notes() As String
            Get
                Return m_Notes
            End Get
            Set(ByVal value As String)
                m_Notes = value.Trim
            End Set
        End Property

        Public Property No_UI() As Boolean
            Get
                Return m_No_UI
            End Get
            Set(ByVal value As Boolean)
                m_No_UI = value
            End Set
        End Property

        Public Property ParentID() As Integer
            Get
                Return m_ParentmFunction_Seq_ID
            End Get
            Set(ByVal Value As Integer)
                m_ParentmFunction_Seq_ID = Value
            End Set
        End Property

        Public Property RedirectOnTimeout() As Boolean
            Get
                Return m_Redirect_On_Timeout
            End Get
            Set(ByVal value As Boolean)
                m_Redirect_On_Timeout = value
            End Set
        End Property

        Public Property Source() As String
            Get
                Return m_Source
            End Get
            Set(ByVal Value As String)
                m_Source = Value.Trim
            End Set
        End Property

    End Class

End Namespace
