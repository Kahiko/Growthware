Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports System.Web
Imports System.Web.UI
Imports System.Globalization

Namespace Utilities
    ''' <summary>
    ''' Class NameValuePairUtility
    ''' </summary>
    Public Module NameValuePairUtility
        ''' <summary>
        ''' Cached Name Value Pair Details Table Name
        ''' </summary>
        Public Const CACHED_NAME_VALUE_PAIR_DETAILS_TABLE_NAME As String = "CachedNVPDetailsTable"

        ''' <summary>
        ''' Cached Name Value Pair Table Name
        ''' </summary>
        Public Const CACHED_NAME_VALUE_PAIR_TABLE_NAME As String = "CachedNVPTable"

        ''' <summary>
        ''' Deletes the detail.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub DeleteDetail(ByVal profile As MNameValuePairDetail)
            Dim mBNameValuePairs As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            mBNameValuePairs.DeleteNameValuePairDetail(profile)
            CacheController.RemoveFromCache(CACHED_NAME_VALUE_PAIR_DETAILS_TABLE_NAME)
        End Sub

        ''' <summary>
        ''' GetNVPs will return all Name Value Pairs avalible for a given account
        ''' </summary>
        ''' <param name="accountId">The account ID.</param>
        ''' <returns>Returns a data table of name value pairs for a given account</returns>
        Public Function AllNameValuePairs(ByVal accountId As Integer) As DataTable
            Dim mNameValuePair As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            Return mNameValuePair.AllNameValuePairs(accountId)
        End Function

        ''' <summary>
        ''' Gets the NVPID.
        ''' </summary>
        ''' <param name="staticName">Name of the static.</param>
        ''' <returns>System.Int32.</returns>
        Public Function GetNameValuePairId(ByVal staticName As String) As Integer
            Dim mDV As New DataView
            Dim mDT As New DataTable
            mDT.Locale = CultureInfo.InvariantCulture
            Dim mRetValue As Integer = 0
            mDT = AllNameValuePairs(-1)
            mDV = mDT.DefaultView
            mDV.RowFilter = "TABLE_NAME = '" & staticName & "'"
            Dim mDataViewRow As DataRowView = mDV.Item(0)
            mRetValue = Integer.Parse(mDataViewRow.Item("NVP_SEQ_ID").ToString())
            Return mRetValue
        End Function

        ''' <summary>
        ''' Gets the name of the NVP.
        ''' </summary>
        ''' <param name="nameValuePairSeqId">The NVP seq ID.</param>
        ''' <returns>System.String.</returns>
        Public Function GetNameValuePairName(ByVal nameValuePairSeqId As Integer) As String
            Dim mDV As New DataView
            Dim mDT As New DataTable
            Dim mRetValue As String = String.Empty
            mDT = AllNameValuePairs(-1)
            mDV = mDT.DefaultView
            mDV.RowFilter = "NVP_SEQ_ID = " & nameValuePairSeqId
            Dim mDataViewRow As DataRowView = mDV.Item(0)
            mRetValue = mDataViewRow.Item("NVP_SEQ_ID").ToString()
            Return mRetValue
        End Function

        ''' <summary>
        ''' GetNVPs will return all Name Value Pairs reguardless of security
        ''' </summary>
        ''' <param name="yourDataTable">An instance of a data table you would like populated</param>
        Public Sub GetNameValuePairs(ByVal yourDataTable As DataTable)
            yourDataTable = CType(HttpContext.Current.Cache(CACHED_NAME_VALUE_PAIR_TABLE_NAME), DataTable)
            If yourDataTable Is Nothing Then
                Dim mNameValuePair As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
                yourDataTable = mNameValuePair.AllNameValuePairs()
                CacheController.AddToCacheDependency(CACHED_NAME_VALUE_PAIR_TABLE_NAME, yourDataTable)
            End If
        End Sub

        ''' <summary>
        ''' Gets the NVP.
        ''' </summary>
        ''' <param name="nameValuePairSeqId">The NVP seq ID.</param>
        ''' <returns>MNameValuePair.</returns>
        Public Function GetNameValuePair(ByVal nameValuePairSeqId As Integer) As MNameValuePair
            Dim mNameValuePair As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            Return New MNameValuePair(mNameValuePair.GetNameValuePair(nameValuePairSeqId))
        End Function

        ''' <summary>
        ''' Gets the NVP detail.
        ''' </summary>
        ''' <param name="nameValuePairDetailSeqId">The NVP seq det ID.</param>
        ''' <param name="nameValuePairSeqId">The NVP seq ID.</param>
        ''' <returns>MNameValuePairDetail.</returns>
        Public Function GetNameValuePairDetail(ByVal nameValuePairDetailSeqId As Integer, ByVal nameValuePairSeqId As Integer) As MNameValuePairDetail
            Dim mDV As New DataView
            Dim mDT As New DataTable
            Dim mImportTable As New DataTable
            GetNameValuePairDetails(mDT, nameValuePairSeqId)
            mDV = mDT.DefaultView
            mImportTable = mDT.Clone
            mDV.RowFilter = "NVP_SEQ_DET_ID = " & nameValuePairDetailSeqId
            For Each drv As DataRowView In mDV
                mImportTable.ImportRow(drv.Row)
            Next
            Return New MNameValuePairDetail(mImportTable.Rows(0))
        End Function

        ''' <summary>
        ''' Gets the NVP details.
        ''' </summary>
        ''' <param name="yourDataTable">Your data table.</param>
        Public Sub GetNameValuePairDetails(ByVal yourDataTable As DataTable)
            yourDataTable = CType(HttpContext.Current.Cache(CACHED_NAME_VALUE_PAIR_DETAILS_TABLE_NAME), DataTable)
            If yourDataTable Is Nothing Then
                Dim mNameValuePairDetails As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
                yourDataTable = mNameValuePairDetails.GetAllNameValuePairDetail()
                CacheController.AddToCacheDependency(CACHED_NAME_VALUE_PAIR_DETAILS_TABLE_NAME, yourDataTable)
            End If
        End Sub

        ''' <summary>
        ''' Gets the NVP details.
        ''' </summary>
        ''' <param name="yourDataTable">Your data table.</param>
        ''' <param name="NVPSeqID">The NVP seq ID.</param>
        Public Sub GetNameValuePairDetails(ByVal yourDataTable As DataTable, ByVal NVPSeqID As Integer)
            Dim mDV As New DataView
            Dim mDT As New DataTable
            GetNameValuePairDetails(mDT)
            mDV = mDT.DefaultView
            yourDataTable = mDV.Table.Clone()
            mDV.RowFilter = "NVP_SEQ_ID = " & NVPSeqID
            For Each drv As DataRowView In mDV
                yourDataTable.ImportRow(drv.Row)
            Next
        End Sub

        ''' <summary>
        ''' Gets the NVP details.
        ''' </summary>
        ''' <param name="staticName">Name of the static.</param>
        ''' <returns>DataTable.</returns>
        Public Function GetNameValuePairDetails(ByVal staticName As String) As DataTable
            Dim mDV As New DataView
            Dim mDT As New DataTable
            Dim mReturnTable As DataTable
            GetNameValuePairDetails(mDT)
            mDV = mDT.DefaultView
            mReturnTable = mDV.Table.Clone()
            mDV.RowFilter = "TABLE_NAME = '" & staticName & "'"
            For Each drv As DataRowView In mDV
                mReturnTable.ImportRow(drv.Row)
            Next
            Return mReturnTable
        End Function

        ''' <summary>
        ''' Gets the NVP details.
        ''' </summary>
        ''' <param name="nameValuePairSeqId">The NVP seq ID.</param>
        ''' <returns>DataTable.</returns>
        Public Function GetNameValuePairDetails(ByVal nameValuePairSeqId As Integer) As DataTable
            Dim mDV As New DataView
            Dim mDT As New DataTable
            Dim mReturnTable As DataTable
            GetNameValuePairDetails(mDT)
            mDV = mDT.DefaultView
            mReturnTable = mDV.Table.Clone()
            mDV.RowFilter = "NVP_SEQ_ID = " & nameValuePairSeqId
            For Each drv As DataRowView In mDV
                mReturnTable.ImportRow(drv.Row)
            Next
            Return mReturnTable
        End Function

        ''' <summary>
        ''' Gets the selected roles.
        ''' </summary>
        ''' <param name="nameValuePairSeqId">The name value pair seq ID.</param>
        ''' <returns>System.String[][].</returns>
        Public Function GetSelectedRoles(ByVal nameValuePairSeqId As Integer) As String()
            Dim mNameValuePair As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            Dim mDataTable As DataTable = mNameValuePair.GetNameValuePairRoles(nameValuePairSeqId)
            Return GetStringArrayList(mDataTable, "Roles")
        End Function

        ''' <summary>
        ''' Gets the selected groups.
        ''' </summary>
        ''' <param name="nameValuePairSeqID">The name value pair seq ID.</param>
        ''' <returns>System.String[][].</returns>
        Public Function GetSelectedGroups(ByVal nameValuePairSeqID As Integer) As String()
            Dim mNameValuePair As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            Dim mDataTable As DataTable = mNameValuePair.GetNameValuePairGroups(nameValuePairSeqID)
            Return GetStringArrayList(mDataTable, "Groups")
        End Function

        ''' <summary>
        ''' Gets the string array list.
        ''' </summary>
        ''' <param name="yourDT">Your DT.</param>
        ''' <param name="rowName">Name of the row.</param>
        ''' <returns>System.String[][].</returns>
        Private Function GetStringArrayList(ByVal yourDT As DataTable, ByVal rowName As String) As String()
            Dim mRetrunArrayList As New ArrayList
            Dim mDR As DataRow
            For Each mDR In yourDT.Rows
                mRetrunArrayList.Add(mDR(rowName).ToString)
            Next
            Return CType(mRetrunArrayList.ToArray(GetType(String)), String())
        End Function

        ''' <summary>
        ''' Saves the specified profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Function Save(ByVal profile As MNameValuePair) As Integer
            Dim mNameValuePair As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            Dim mRetVal As Integer = -1
            mRetVal = mNameValuePair.Save(profile)
            CacheController.RemoveFromCache(CACHED_NAME_VALUE_PAIR_TABLE_NAME)
            CacheController.RemoveFromCache(CACHED_NAME_VALUE_PAIR_DETAILS_TABLE_NAME)
            Return mRetVal
        End Function

        ''' <summary>
        ''' Saves the detail.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub SaveDetail(ByVal profile As MNameValuePairDetail)
            Dim mBNameValuePairs As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            mBNameValuePairs.SaveNameValuePairDetail(profile)
            CacheController.RemoveFromCache(CACHED_NAME_VALUE_PAIR_DETAILS_TABLE_NAME)
        End Sub

        ''' <summary>
        ''' Searches the specified search criteria.
        ''' </summary>
        ''' <param name="searchCriteria">The search criteria.</param>
        ''' <returns>DataTable.</returns>
        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            Try
                Dim mBNameValuePairs As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
                Return mBNameValuePairs.Search(searchCriteria)
            Catch ex As IndexOutOfRangeException
                'no data is not a problem
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Sets the drop selection.
        ''' </summary>
        ''' <param name="theDropDown">The drop down.</param>
        ''' <param name="selectedVale">The selected vale.</param>
        Public Sub SetDropSelection(ByVal theDropDown As WebControls.DropDownList, ByVal selectedVale As String)
            Try
                Dim X As Integer
                For X = 0 To theDropDown.Items.Count - 1
                    If theDropDown.Items(X).Value = selectedVale Then
                        theDropDown.SelectedIndex = X
                        Exit For
                    End If
                Next
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' Updates the groups.
        ''' </summary>
        ''' <param name="nameValuePairId">The name value pair Id.</param>
        ''' <param name="securityEntityID">The security entity Id.</param>
        ''' <param name="commaSeparatedGroups">The comma Separated groups.</param>
        ''' <param name="profile">MNameValuePair</param>
        Public Sub UpdateGroups(ByVal nameValuePairId As Integer, ByVal securityEntityId As Integer, ByVal commaSeparatedGroups As String, ByVal profile As MNameValuePair)
            Dim mNameValuePair As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            mNameValuePair.UpdateGroups(nameValuePairId, securityEntityId, commaSeparatedGroups, profile)
        End Sub

        ''' <summary>
        ''' Updates the roles.
        ''' </summary>
        ''' <param name="nameValuePairId">The NV p_ ID.</param>
        ''' <param name="securityEntityID">The security entity ID.</param>
        ''' <param name="commaSeparatedRoles">The comma Separated roles.</param>
        ''' <param name="profile">MNameValuePair</param>
        Public Sub UpdateRoles(ByVal nameValuePairId As Integer, ByVal securityEntityID As Integer, ByVal commaSeparatedRoles As String, ByVal nvpProfile As MNameValuePair)
            Dim mNameValuePair As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            mNameValuePair.UpdateRoles(nameValuePairId, securityEntityID, commaSeparatedRoles, nvpProfile)
        End Sub
    End Module
End Namespace
