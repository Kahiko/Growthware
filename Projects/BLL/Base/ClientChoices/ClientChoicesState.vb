Imports DALFactory.Base.Application
Imports DALInterface.Base.Interfaces

Namespace Base.ClientChoices
    <Serializable()> _
    Public Class BClientChoicesState
        Private Shared iBaseDAL As IClientChoices = AbstractFactory.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"), "DClientChoices")

		Dim m_ClientChoices As System.Collections.Hashtable = New System.Collections.Hashtable
        Dim m_AccountName As String
        Dim m_IsDirty As Boolean

        Public Sub New(ByVal AccountName As String)
            MyBase.New()
            m_AccountName = AccountName
            Try
                Dim dsResult As DataSet = New DataSet
                ' get the client choices data
				dsResult = iBaseDAL.GetClientChoicesData(m_AccountName)
                'If there is no data then -- create new client choices
                If (dsResult.Tables(0).Rows.Count = 0) Then
                    Dim isSuccess As Boolean
                    ' create the client choices information in the data store
					isSuccess = iBaseDAL.CreateClientChoicesAccount(m_AccountName)
                    ' populate dsResult from the client choices account that was just created
                    If isSuccess Then
						dsResult = iBaseDAL.GetClientChoicesData(m_AccountName)
                    Else
                        Throw New Exception("Could not create Client Choices information")
                    End If
                End If

                Dim Row As DataRow = dsResult.Tables(0).Rows(0)
                Dim i As Integer

                For i = 0 To dsResult.Tables(0).Columns.Count - 1
                    Dim Value As Object = Row.Item(dsResult.Tables(0).Columns(i))
                    m_ClientChoices(dsResult.Tables(0).Columns(i).ToString()) = Value.ToString()
                Next i
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Sub

        Public Property AccountName() As String
            Get
                Return m_AccountName
            End Get
            Set(ByVal Value As String)
                m_AccountName = Value
            End Set
        End Property

        Default Public Property Item(ByVal key As String) As String
            Get
                Return CType(m_ClientChoices(key), String)
            End Get
            Set(ByVal Value As String)
                m_ClientChoices(key) = Value
                m_IsDirty = True
            End Set
        End Property

        Public Sub Save()
            If m_IsDirty Then
				m_IsDirty = iBaseDAL.Save(m_ClientChoices)
            End If
        End Sub
    End Class
End Namespace