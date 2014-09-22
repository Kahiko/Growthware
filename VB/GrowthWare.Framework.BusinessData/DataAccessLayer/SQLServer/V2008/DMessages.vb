Imports GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.Base
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports GrowthWare.Framework.Model.Profiles
Imports System.Data.SqlClient

Namespace DataAccessLayer.SQLServer.V2008
    ''' <summary>
    ''' DFunctions provides all database interaction to SQL Server 2008
    ''' </summary>
    ''' <remarks>
    '''		The Profile and SecurityEntitySeqID properties must be set
    '''		before using any methods.
    '''		Properties where chosen instead of parameters because all
    '''		methods will need one or both to perform their work.
    '''	</remarks>
    Public Class DMessages
        Inherits DDBInteraction
        Implements IDMessages

        Private m_Profile As MMessageProfile = Nothing

        Public Function GetMessages() As DataTable Implements IDMessages.GetMessages
            checkValid()
            Dim mParameters() As SqlParameter =
             {
              New SqlParameter("@P_Message_SeqID", -1),
              New SqlParameter("@P_Security_Entity_SeqID", Profile.SE_SEQ_ID)
             }
            Return MyBase.GetDataTable("ZGWCoreWeb.Get_Messages", mParameters)
        End Function

        Public Function GetMessage() As DataRow Implements IDMessages.GetMessage
            checkValid()
            Dim mParameters() As SqlParameter =
             {
              New SqlParameter("@P_Message_SeqID", Profile.Id),
              New SqlParameter("@P_Security_Entity_SeqID", Profile.SE_SEQ_ID)
             }
            Return MyBase.GetDataRow("ZGWCoreWeb.Get_Messages", mParameters)
        End Function

        Public Property Profile As MMessageProfile Implements IDMessages.Profile

        Public Sub Save() Implements IDMessages.Save
            checkValid()
            Dim mParameters() As SqlParameter =
             {
              New SqlParameter("@P_Message_SeqID", Profile.Id),
              New SqlParameter("@P_Security_Entity_SeqID", Profile.SE_SEQ_ID),
              New SqlParameter("@P_Name", Profile.Name),
              New SqlParameter("@P_Title", Profile.Title),
              New SqlParameter("@P_Description", Profile.Description),
              New SqlParameter("@P_BODY", Profile.Body),
              New SqlParameter("@P_Format_As_HTML", Profile.FormatAsHTML),
              New SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(Profile)),
              GetSqlParameter("@P_PRIMARY_KEY", -1, ParameterDirection.Output)
             }
            MyBase.ExecuteNonQuery("ZGWCoreWeb.Set_Message", mParameters)
        End Sub

        Public Property SecurityEntitySeqID As Integer Implements IDMessages.SecurityEntitySeqID

        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable Implements IDMessages.Search
            Dim mRetVal As DataTable
            Dim mParameters() As SqlParameter =
             {
              New SqlParameter("@P_Columns", searchCriteria.Columns),
              New SqlParameter("@P_OrderByColumn", searchCriteria.OrderByColumn),
              New SqlParameter("@P_OrderByDirection", searchCriteria.OrderByDirection),
              New SqlParameter("@P_PageSize", searchCriteria.PageSize),
              New SqlParameter("@P_SelectedPage", searchCriteria.SelectedPage),
              New SqlParameter("@P_TableOrView", "ZGWCoreWeb.vwSearchMessages"),
              New SqlParameter("@P_WhereClause", searchCriteria.WhereClause)
             }
            mRetVal = MyBase.GetDataTable("ZGWSystem.Get_Paginated_Data", mParameters)
            Return mRetVal
        End Function

        Private Sub checkValid()
            MyBase.IsValid()
            If Profile Is Nothing Then
                Throw New ArgumentException("Profile property must be set before calling methods from this class")
            End If
            If SecurityEntitySeqID = 0 Then
                Throw New ArgumentException("SE_SEQ_ID property must be set before calling methods from this class")
            End If
        End Sub
    End Class
End Namespace
