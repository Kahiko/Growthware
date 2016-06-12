Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.MySql.Base
Imports GrowthWare.Framework.Model.Profiles
Imports MySql.Data.MySqlClient

Namespace DataAccessLayer.MySql.V5621
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

        Private m_Profile As MMessageProfile = New MMessageProfile()

        Public Function GetMessages() As DataTable Implements IDMessages.Messages
            checkValid()
            Dim mParameters() As MySqlParameter =
             {
              New MySqlParameter("@P_Message_SeqID", -1),
              New MySqlParameter("@P_Security_Entity_SeqID", Profile.SecurityEntitySeqId)
             }
            Return MyBase.GetDataTable("ZGWCoreWeb.Get_Messages", mParameters)
        End Function

        Public Function GetMessage(ByVal messageSeqId As Integer) As DataRow Implements IDMessages.GetMessage
            checkValid()
            Dim mParameters() As MySqlParameter =
             {
              New MySqlParameter("@P_Message_SeqID", messageSeqId),
              New MySqlParameter("@P_Security_Entity_SeqID", Profile.SecurityEntitySeqId)
             }
            Return MyBase.GetDataRow("ZGWCoreWeb.Get_Messages", mParameters)
        End Function

        Public Property Profile As MMessageProfile Implements IDMessages.Profile
            Get
                Return m_Profile
            End Get
            Set(value As MMessageProfile)
                If Not value Is Nothing Then m_Profile = value
            End Set
        End Property

        Public Sub Save() Implements IDMessages.Save
            checkValid()
            Dim mParameters() As MySqlParameter =
             {
              New MySqlParameter("@P_Message_SeqID", Profile.Id),
              New MySqlParameter("@P_Security_Entity_SeqID", Profile.SecurityEntitySeqId),
              New MySqlParameter("@P_Name", Profile.Name),
              New MySqlParameter("@P_Title", Profile.Title),
              New MySqlParameter("@P_Description", Profile.Description),
              New MySqlParameter("@P_BODY", Profile.Body),
              New MySqlParameter("@P_Format_As_HTML", Profile.FormatAsHtml),
              New MySqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(Profile)),
              GetSqlParameter("@P_PRIMARY_KEY", -1, ParameterDirection.Output)
             }
            MyBase.ExecuteNonQuery("ZGWCoreWeb.Set_Message", mParameters)
        End Sub

        Public Property SecurityEntitySeqId As Integer Implements IDMessages.SecurityEntitySeqId

        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable Implements IDMessages.Search
            If searchCriteria Is Nothing Then Throw New ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!")
            Dim mRetVal As DataTable
            Dim mParameters() As MySqlParameter =
             {
              New MySqlParameter("@P_Columns", searchCriteria.Columns),
              New MySqlParameter("@P_OrderByColumn", searchCriteria.OrderByColumn),
              New MySqlParameter("@P_OrderByDirection", searchCriteria.OrderByDirection),
              New MySqlParameter("@P_PageSize", searchCriteria.PageSize),
              New MySqlParameter("@P_SelectedPage", searchCriteria.SelectedPage),
              New MySqlParameter("@P_TableOrView", "ZGWCoreWeb.vwSearchMessages"),
              New MySqlParameter("@P_WhereClause", searchCriteria.WhereClause)
             }
            mRetVal = MyBase.GetDataTable("ZGWSystem.Get_Paginated_Data", mParameters)
            Return mRetVal
        End Function

        Private Sub checkValid()
            MyBase.IsValid()
            If Profile Is Nothing Then
                Throw New ArgumentException("Profile property must be set before calling methods from this class")
            End If
            If SecurityEntitySeqId = 0 Then
                Throw New ArgumentException("SecurityEntitySeqId property must be set before calling methods from this class")
            End If
        End Sub
    End Class
End Namespace
