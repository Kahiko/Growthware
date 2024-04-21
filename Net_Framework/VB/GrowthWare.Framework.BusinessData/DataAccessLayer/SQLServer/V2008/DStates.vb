Imports GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.Base
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports GrowthWare.Framework.Model.Profiles
Imports System.Data.SqlClient
Imports System.Globalization

Namespace DataAccessLayer.SQLServer.V2008
    Public Class DStates
        Inherits DDBInteraction
        Implements IDState

#Region "Private Fields"
        Private m_Profile As MStateProfile = Nothing
        Private m_SecurityEntitySeqID As Integer = -2
#End Region
#Region "Public Properties"
        Public Property Profile As MStateProfile Implements IDState.Profile
            Get
                Return m_Profile
            End Get
            Set(value As MStateProfile)
                If (value IsNot Nothing) Then
                    m_Profile = value
                End If
            End Set
        End Property

        Public ReadOnly Property GetState As DataRow Implements IDState.GetState
            Get
                checkValid()
                Dim mStoredProcedure As String = "ZGWOptional.Get_State"
                Dim mParameters As SqlParameter() =
                {
                 New SqlParameter("@P_State", m_Profile.State)
                }
                Return MyBase.GetDataRow(mStoredProcedure, mParameters)
            End Get
        End Property

        Public ReadOnly Property GetStates As DataTable Implements IDState.GetStates
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public Sub Delete() Implements IDState.Delete
            Throw New NotImplementedException()
        End Sub

        Public Sub Save() Implements IDState.Save
            checkValid()
            Dim mStoredProcedure As String = "ZGWOptional.Set_State"
            Dim mParameters As SqlParameter() =
                {
                 New SqlParameter("@P_State", m_Profile.State),
                 New SqlParameter("@P_Description", m_Profile.Description),
                 New SqlParameter("@P_Status_SeqID", m_Profile.Status_SeqID),
                 New SqlParameter("@P_Updated_By", m_Profile.UpdatedBy),
                 GetSqlParameter("@P_Primary_Key", m_Profile.State, ParameterDirection.InputOutput)
                }
            MyBase.ExecuteNonQuery(mStoredProcedure, mParameters)
        End Sub

        Public Function Search(searchCriteria As MSearchCriteria) As DataTable Implements IDState.Search
            If searchCriteria Is Nothing Then Throw New ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!.")
            Dim mRetVal As DataTable
            Dim mParameters() As SqlParameter =
             {
              New SqlParameter("@P_Columns", searchCriteria.Columns),
              New SqlParameter("@P_OrderByColumn", searchCriteria.OrderByColumn),
              New SqlParameter("@P_OrderByDirection", searchCriteria.OrderByDirection),
              New SqlParameter("@P_PageSize", searchCriteria.PageSize),
              New SqlParameter("@P_SelectedPage", searchCriteria.SelectedPage),
              New SqlParameter("@P_TableOrView", "[ZGWOptional].[vwSearchStates]"),
              New SqlParameter("@P_WhereClause", searchCriteria.WhereClause)
             }
            mRetVal = MyBase.GetDataTable("ZGWSystem.Get_Paginated_Data", mParameters)
            Return mRetVal
        End Function

        Protected Property SecurityEntitySeqId() As Integer Implements IDState.SecurityEntitySeqId
            Get
                Return m_SecurityEntitySeqID
            End Get
            Set(ByVal value As Integer)
                m_SecurityEntitySeqID = value
            End Set
        End Property
#End Region

#Region "Private Methods"
        Private Sub checkValid()
            MyBase.IsValid()
            If m_Profile Is Nothing Then
                Throw New DataAccessLayerException("Profile property must be set before calling methods from this class")
            End If
            If m_SecurityEntitySeqID = -2 Then
                Throw New DataAccessLayerException("SE_SEQ_ID property must be set before calling methods from this class")
            End If
        End Sub
#End Region
    End Class
End Namespace
