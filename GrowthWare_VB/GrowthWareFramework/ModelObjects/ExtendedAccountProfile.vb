Namespace ModelObjects
    ''' <summary>
    ''' Example of using inheritence to extend the base AccountProfile
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ExtendedAccountProfile
        Inherits MAccountProfile

#Region "Private Members"
        Private m_ExtraProperty As String = String.Empty
#End Region

#Region "Public Methods"
        Public Sub New()

        End Sub

        Public Sub New(ByVal dr As DataRow)
            On Error Resume Next
            MyBase.Init(dr)
            m_ExtraProperty = dr("ExtraProperty")
        End Sub
#End Region

#Region "Public Properties"
        Public Property extraProperty() As String
            Get
                Return m_ExtraProperty
            End Get
            Set(ByVal value As String)
                m_ExtraProperty = value.Trim
            End Set
        End Property
#End Region
    End Class
End Namespace