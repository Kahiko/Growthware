Imports GrowthWare.Framework.BusinessData.DataAccessLayer.MySql.Base
Imports GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.Framework.Model.Profiles
Imports System
Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.Globalization


Namespace DataAccessLayer.MySql.V5_6_21
    Public Class DFunctions
        Inherits DDBInteraction
        Implements IDFunction

#Region "Member Objects"
        Private m_Profile As MFunctionProfile = Nothing
        Private m_SecurityEntitySeqId As Integer = -2
#End Region

#Region "Public Properties"
        Public ReadOnly Property GetFunction As System.Data.DataRow Implements IDFunction.GetFunction
            Get
                checkValid()
                Dim mParameters() As MySqlParameter =
                {
                 New MySqlParameter("@P_Function_SeqID", Profile.Id)
                }
                Return MyBase.GetDataRow("ZGWSecurity.Get_Function", mParameters)
            End Get
        End Property

        Public ReadOnly Property GetFunctions As System.Data.DataSet Implements IDFunction.GetFunctions
            Get
                checkValid()
                Dim mDSFunctions As DataSet = Nothing
                Dim mParameters() As MySqlParameter =
                {
                 New MySqlParameter("@P_Function_SeqID", m_Profile.Id)
                }
                Try
                    Dim mFunctions As DataTable = MyBase.GetDataTable("ZGWSecurity.Get_Function", mParameters)
                    mDSFunctions = Me.getSecurity()
                    mDSFunctions.Tables(0).TableName = "DerivedRoles"
                    mDSFunctions.Tables(1).TableName = "AssignedRoles"
                    mDSFunctions.Tables(2).TableName = "Groups"


                    Dim mHasAssingedRoles As Boolean = False
                    Dim mHasGroups As Boolean = False
                    mFunctions.TableName = "Functions"
                    If mDSFunctions.Tables("AssignedRoles").Rows.Count > 0 Then mHasAssingedRoles = True
                    If mDSFunctions.Tables("Groups").Rows.Count > 0 Then mHasGroups = True
                    mDSFunctions.Tables.Add(mFunctions.Copy)

                    Dim mRelation As DataRelation = New DataRelation("DerivedRoles", mDSFunctions.Tables("Functions").Columns("Function_Seq_ID"), mDSFunctions.Tables("DerivedRoles").Columns("Function_Seq_ID"))
                    mDSFunctions.Relations.Add(mRelation)
                    If mHasAssingedRoles Then
                        mRelation = New DataRelation("AssignedRoles", mDSFunctions.Tables("Functions").Columns("Function_Seq_ID"), mDSFunctions.Tables("AssignedRoles").Columns("Function_Seq_ID"))
                        mDSFunctions.Relations.Add(mRelation)
                    End If
                    If mHasGroups Then
                        mRelation = New DataRelation("Groups", mDSFunctions.Tables("Functions").Columns("Function_Seq_ID"), mDSFunctions.Tables("Groups").Columns("Function_Seq_ID"))
                        mDSFunctions.Relations.Add(mRelation)
                    End If
                Catch ex As Exception
                    If Not mDSFunctions Is Nothing Then
                        mDSFunctions.Dispose()
                    End If
                    Throw
                End Try
                Return mDSFunctions
            End Get
        End Property

        Public Property Profile As MFunctionProfile Implements IDFunction.Profile
            Get
                Return m_Profile
            End Get
            Set(value As MFunctionProfile)
                m_Profile = value
            End Set
        End Property

        Public Property SecurityEntitySeqId As Integer Implements IDFunction.SecurityEntitySeqId
            Get
                Return m_SecurityEntitySeqId
            End Get
            Set(value As Integer)
                m_SecurityEntitySeqId = value
            End Set
        End Property
#End Region

#Region "Pulbic Methods"
        ''' <summary>
        ''' Deletes an account
        ''' </summary>
        Public Sub Delete(ByVal functionSeqId As Integer) Implements IDFunction.Delete
            Dim myParameters() As MySqlParameter =
            {
              New MySqlParameter("@P_Function_SeqID", functionSeqId), _
              GetSqlParameter("@P_ErrorCode", "", ParameterDirection.Output)
            }
            MyBase.ExecuteNonQuery("ZGWSecurity.Delete_Function", myParameters)
        End Sub

        Public Function FunctionTypes() As DataTable Implements IDFunction.FunctionTypes
            Dim mStoreProcedure As String = "ZGWSecurity.Get_Function_Types"
            Dim mParameters() As MySqlParameter = {New MySqlParameter("@P_Function_Type_SeqID", -1)}
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        Public Function GetMenuOrder(ByVal profile As MFunctionProfile) As DataTable Implements IDFunction.GetMenuOrder
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mStoreProcedure As String = "ZGWSecurity.Get_Function_Sort"
            Dim mParameters() As MySqlParameter = {New MySqlParameter("@P_Function_SeqID", profile.Id)}
            Return MyBase.GetDataTable(mStoreProcedure, mParameters)
        End Function

        ''' <summary>
        ''' Inserts or updates account information
        ''' </summary>
        Public Function Save() As Integer Implements IDFunction.Save
            checkValid()
            Dim mRetVal As Integer = -1
            Dim mParameters() As MySqlParameter =
             {
              GetSqlParameter("@P_Function_SeqID", m_Profile.Id, ParameterDirection.InputOutput),
              New MySqlParameter("@P_Name", m_Profile.Name),
              New MySqlParameter("@P_Description", m_Profile.Description),
              New MySqlParameter("@P_Function_Type_SeqID", m_Profile.FunctionTypeSeqId),
              New MySqlParameter("@P_Source", m_Profile.Source),
              New MySqlParameter("@P_Enable_View_State", m_Profile.EnableViewState),
              New MySqlParameter("@P_Enable_Notifications", m_Profile.EnableNotifications),
              New MySqlParameter("@P_Redirect_On_Timeout", m_Profile.RedirectOnTimeout),
              New MySqlParameter("@P_IS_NAV", m_Profile.IsNavigable),
              New MySqlParameter("P_Link_Behavior", m_Profile.LinkBehavior),
              New MySqlParameter("@P_NO_UI", m_Profile.NoUI),
              New MySqlParameter("@P_NAV_TYPE_ID", m_Profile.NavigationTypeSeqId),
              New MySqlParameter("@P_Action", m_Profile.Action),
              New MySqlParameter("@P_Meta_Key_Words", m_Profile.MetaKeywords),
              New MySqlParameter("@P_Parent_SeqID", m_Profile.ParentId),
              New MySqlParameter("@P_Notes", m_Profile.Notes),
              New MySqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile))
             }
            MyBase.ExecuteNonQuery("ZGWSecurity.Set_Function", mParameters)
            mRetVal = Integer.Parse(GetParameterValue("@P_Function_SeqID", mParameters), CultureInfo.InvariantCulture)
            Return mRetVal
        End Function

        ''' <summary>
        ''' Save groups by passing a string or comma seporated groups to the database.
        ''' </summary>
        ''' <param name="permission">The permission.</param>
        Public Sub SaveGroups(ByVal permission As PermissionType) Implements IDFunction.SaveGroups
            checkValid()
            Dim mCommaSeporatedString As String = m_Profile.GetCommaSeparatedGroups(permission)
            Dim mParameters() As MySqlParameter =
             {
              New MySqlParameter("@P_Function_SeqID", Profile.Id),
              New MySqlParameter("@P_Security_Entity_SeqID", m_SecurityEntitySeqId),
              New MySqlParameter("@P_Groups", mCommaSeporatedString),
              New MySqlParameter("@P_Permissions_NVP_Detail_SeqID", permission),
              New MySqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(Profile))
             }
            MyBase.ExecuteNonQuery("ZGWSecurity.Set_Function_Groups", mParameters)
        End Sub

        ''' <summary>
        ''' Save roles by passing the permission type and sending the profiles comma separated roles to the database.
        ''' </summary>
        ''' <param name="permission">The permission.</param>
        Public Sub SaveRoles(ByVal permission As PermissionType) Implements IDFunction.SaveRoles
            checkValid()
            Dim mCommaSeporatedString As String = Profile.GetCommaSeparatedAssignedRoles(permission)
            Dim mParameters() As MySqlParameter =
             {
              New MySqlParameter("@P_Function_SeqID", Profile.Id),
              New MySqlParameter("@P_Security_Entity_SeqID", m_SecurityEntitySeqId),
              New MySqlParameter("@P_Roles", mCommaSeporatedString),
              New MySqlParameter("@P_Permissions_NVP_Detail_SeqID", permission),
              New MySqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(Profile))
             }
            MyBase.ExecuteNonQuery("ZGWSecurity.Set_Function_Roles", mParameters)
        End Sub

        ''' <summary>
        ''' Returns a data table based on the search criteria.
        ''' </summary>
        ''' <param name="searchCriteria">MSearchCriteria</param>
        ''' <returns>DataTable</returns>
        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable Implements IDFunction.Search
            If searchCriteria Is Nothing Then Throw New ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!")
            Dim mRetVal As DataTable
            Dim mParameters() As MySqlParameter =
             {
              New MySqlParameter("@P_Columns", searchCriteria.Columns),
              New MySqlParameter("@P_OrderByColumn", searchCriteria.OrderByColumn),
              New MySqlParameter("@P_OrderByDirection", searchCriteria.OrderByDirection),
              New MySqlParameter("@P_PageSize", searchCriteria.PageSize),
              New MySqlParameter("@P_SelectedPage", searchCriteria.SelectedPage),
              New MySqlParameter("@P_TableOrView", "ZGWSystem.vwSearchFunctions"),
              New MySqlParameter("@P_WhereClause", searchCriteria.WhereClause)
             }
            mRetVal = MyBase.GetDataTable("ZGWSystem.Get_Paginated_Data", mParameters)
            Return mRetVal
        End Function

        Public Sub UpdateMenuOrder(ByVal profile As MFunctionProfile, ByVal direction As DirectionType) Implements IDFunction.UpdateMenuOrder
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mStoreProcedure As String = "ZGWSecurity.Set_Function_Sort"
            Dim mParameters() As MySqlParameter =
             {
              New MySqlParameter("@P_Function_SeqID", profile.Id),
              New MySqlParameter("@P_Direction", direction),
              New MySqlParameter("@P_Added_Updated_By", profile.UpdatedBy),
              GetSqlParameter("@P_Primary_Key", "", ParameterDirection.Output)
             }
            MyBase.ExecuteNonQuery(mStoreProcedure, mParameters)
        End Sub
#End Region

#Region "Private Methods"
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId:="SecurityEntitySeqId")> Private Sub checkValid()
            MyBase.IsValid()
            If Profile Is Nothing Then
                Throw New InvalidOperationException("The Profile property must be set before calling methods from this class")
            End If
            If m_SecurityEntitySeqId = 0 Then
                Throw New InvalidOperationException("The SecurityEntitySeqId property must be set before calling methods from this class")
            End If
        End Sub

        Private Function getSecurity() As System.Data.DataSet
            Dim myStoreProcedure As String = "ZGWSecurity.Get_Function_Security"
            Dim myParameters() As MySqlParameter =
            {
             New MySqlParameter("@P_Security_Entity_SeqID", m_SecurityEntitySeqId)
            }
            Return MyBase.GetDataSet(myStoreProcedure, myParameters)
        End Function
#End Region
    End Class
End Namespace
