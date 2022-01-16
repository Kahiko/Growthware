Imports GrowthWare.Framework.Model.Profiles.Base

Namespace Model.Profiles
    ''' <summary>
    ''' Model object representing GroupRoles
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable(), CLSCompliant(True)>
    Public Class MStateProfile
        Inherits MProfile


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
            Me.Initialize(detailRow)
            Me.Description = Me.GetString(detailRow, "Description")
            Me.State = Me.GetString(detailRow, "State")
            Me.Status_SeqID = Me.GetInt(detailRow, "Status_SeqID")
        End Sub
#End Region


        Private m_AddedUpdatedBy As Integer
        Private m_Description As String = -1
        Private m_State As String = -1
        Private m_Status_SeqID As Integer = 2 ' Active

        ''' <summary>
        ''' Gets or sets the ADD_UP_BY.
        ''' </summary>
        ''' <value>Account added or updated by.</value>
        Public Property AddedUpdatedBy() As Integer
            Get
                Return m_AddedUpdatedBy
            End Get
            Set(ByVal value As Integer)
                m_AddedUpdatedBy = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the Description.
        ''' </summary>
        ''' <value>varchar 128</value>
        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal value As String)
                m_Description = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the Status_SeqID.
        ''' </summary>
        ''' <value>varchar 128</value>
        ''' <remarks>Forign key from [ZGWSystem].[Statuses]</remarks>
        Public Property Status_SeqID As Integer
            Get
                Return m_Status_SeqID
            End Get
            Set(ByVal value As Integer)
                m_Status_SeqID = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the State.
        ''' </summary>
        ''' <value>The Security Entity ID.</value>
        Public Property State() As String
            Get
                Return m_State
            End Get
            Set(ByVal value As String)
                m_State = value
            End Set
        End Property
    End Class
End Namespace
