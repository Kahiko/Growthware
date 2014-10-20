Imports GrowthWare.Framework.Model.Profiles.Base
Imports GrowthWare.Framework.Model.Profiles.Interfaces

Namespace Model.Profiles
    <Serializable(), CLSCompliant(True)>
    Public Class MGroupProfile
        Inherits MProfile

#Region "Member Properties"
        Private mDESCRIPTION As String = String.Empty
        Private mSE_SEQ_ID As Integer = 1
#End Region

#Region "Protected Methods"
        ''' <summary>
        ''' Initializes the properties using the specified DataRow.
        ''' </summary>
        ''' <param name="dr">The dr.</param>
        Protected Overloads Sub Initialize(ByVal dr As DataRow)
            Me.IdColumnName = "GROUP_SEQ_ID"
            Me.NameColumnName = "NAME"
            MyBase.Initialize(dr)
            mDESCRIPTION = CStr(dr("DESCRIPTION")).Trim
        End Sub
#End Region

#Region "Public Methods"
        ''' <summary>
        ''' Will return a message profile with the default vaules
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="MGroupProfile" /> class.
        ''' </summary>
        ''' <param name="dr">The DataRow</param>
        Public Sub New(ByVal dr As DataRow)
            Me.Initialize(dr)
        End Sub
#End Region

#Region "Public Properties"
        ''' <summary>
        ''' Gets or sets the security entity ID.
        ''' </summary>
        ''' <value>The security entity ID.</value>
        Public Property SecurityEntityId() As Integer
            Get
                Return mSE_SEQ_ID
            End Get
            Set(ByVal value As Integer)
                mSE_SEQ_ID = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the description.
        ''' </summary>
        ''' <value>The description.</value>
        Public Property Description() As String
            Get
                Return mDESCRIPTION
            End Get
            Set(ByVal value As String)
                mDESCRIPTION = value.Trim
            End Set
        End Property
#End Region
    End Class
End Namespace