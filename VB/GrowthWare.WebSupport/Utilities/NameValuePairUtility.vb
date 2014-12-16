Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports System.Web
Imports System.Web.UI
Imports System.Globalization
Imports System.Web.UI.WebControls

Namespace Utilities
    ''' <summary>
    ''' Class NameValuePairUtility
    ''' </summary>
    Public Module NameValuePairUtility
        ''' <summary>
        ''' Cached Name Value Pair Details Table Name
        ''' </summary>
        Public Const CachedNameValuePairDetailsTableName As String = "CachedNVPDetailsTable"

        ''' <summary>
        ''' Cached Name Value Pair Table Name
        ''' </summary>
        Public Const CachedNameValuePairTableName As String = "CachedNVPTable"

        ''' <summary>
        ''' Deletes the detail.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub DeleteDetail(ByVal profile As MNameValuePairDetail)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mBNameValuePairs As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            mBNameValuePairs.DeleteNameValuePairDetail(profile)
            CacheController.RemoveFromCache(CachedNameValuePairDetailsTableName)
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
            Dim mDataView As DataView = Nothing
            Dim mDataTable As DataTable = Nothing
            Dim mRetValue As Integer = 0
            Try
                mDataTable = AllNameValuePairs(-1)
                mDataTable.Locale = CultureInfo.InvariantCulture
                mDataView = mDataTable.DefaultView
                mDataView.RowFilter = "TABLE_NAME = '" & staticName & "'"
                Dim mDataViewRow As DataRowView = mDataView.Item(0)
                mRetValue = Integer.Parse(mDataViewRow.Item("NVP_SEQ_ID").ToString(), CultureInfo.InvariantCulture)
            Catch ex As Exception
                Throw
            Finally
                If Not mDataTable Is Nothing Then
                    mDataTable.Dispose()
                    mDataTable = Nothing
                End If
                If Not mDataView Is Nothing Then
                    mDataView.Dispose()
                    mDataView = Nothing
                End If
            End Try
            Return mRetValue
        End Function

        ''' <summary>
        ''' Gets the name of the NVP.
        ''' </summary>
        ''' <param name="nameValuePairSeqId">The NVP seq ID.</param>
        ''' <returns>System.String.</returns>
        Public Function GetNameValuePairName(ByVal nameValuePairSeqId As Integer) As String
            Dim mDataView As DataView = Nothing
            Dim mDataTable As DataTable = Nothing
            Dim mRetValue As String = String.Empty
            Try
                mDataTable = AllNameValuePairs(-1)
                mDataTable.Locale = CultureInfo.InvariantCulture
                mDataView = mDataTable.DefaultView
                mDataView.RowFilter = "NVP_SEQ_ID = " & nameValuePairSeqId
                Dim mDataViewRow As DataRowView = mDataView.Item(0)
                mRetValue = mDataViewRow.Item("NVP_SEQ_ID").ToString()
            Catch ex As Exception
                Throw
            Finally
                If Not mDataTable Is Nothing Then
                    mDataTable.Dispose()
                    mDataTable = Nothing
                End If
                If Not mDataView Is Nothing Then
                    mDataView.Dispose()
                    mDataView = Nothing
                End If
            End Try
            Return mRetValue
        End Function

        ''' <summary>
        ''' GetNVPs will return all Name Value Pairs reguardless of security
        ''' </summary>
        ''' <param name="yourDataTable">An instance of a data table you would like populated</param>
        Public Sub GetNameValuePairs(ByVal yourDataTable As DataTable)
            yourDataTable = CType(HttpContext.Current.Cache(CachedNameValuePairTableName), DataTable)
            If yourDataTable Is Nothing Then
                Dim mNameValuePair As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
                yourDataTable = mNameValuePair.AllNameValuePairs()
                CacheController.AddToCacheDependency(CachedNameValuePairTableName, yourDataTable)
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
            Dim mDataView As DataView = Nothing
            Dim mDataTable As DataTable = Nothing
            Dim mImportTable As DataTable = Nothing
            Try
                GetNameValuePairDetails(mDataTable, nameValuePairSeqId)
                mDataTable.Locale = CultureInfo.InvariantCulture
                mDataView = mDataTable.DefaultView
                mImportTable = mDataTable.Clone
                mDataView.RowFilter = "NVP_SEQ_DET_ID = " & nameValuePairDetailSeqId
                For Each drv As DataRowView In mDataView
                    mImportTable.ImportRow(drv.Row)
                Next
            Catch ex As Exception
                Throw
            Finally
                If Not mDataTable Is Nothing Then
                    mDataTable.Dispose()
                    mDataTable = Nothing
                End If
                If Not mDataView Is Nothing Then
                    mDataView.Dispose()
                    mDataView = Nothing
                End If
            End Try
            Return New MNameValuePairDetail(mImportTable.Rows(0))
        End Function

        ''' <summary>
        ''' Gets the NVP details.
        ''' </summary>
        ''' <param name="yourDataTable">Your data table.</param>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
        Public Sub GetNameValuePairDetails(ByRef yourDataTable As DataTable)
            yourDataTable = CType(HttpContext.Current.Cache(CachedNameValuePairDetailsTableName), DataTable)
            If yourDataTable Is Nothing Then
                Dim mNameValuePairDetails As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
                yourDataTable = mNameValuePairDetails.GetAllNameValuePairDetail()
                CacheController.AddToCacheDependency(CachedNameValuePairDetailsTableName, yourDataTable)
            End If
        End Sub

        ''' <summary>
        ''' Gets the NVP details.
        ''' </summary>
        ''' <param name="yourDataTable">Your data table.</param>
        ''' <param name="nameValuePairSeqId">The NVP seq ID.</param>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId:="0")>
        Public Sub GetNameValuePairDetails(ByRef yourDataTable As DataTable, ByVal nameValuePairSeqId As Integer)
            Dim mDataView As DataView = Nothing
            Dim mDataTable As DataTable = Nothing
            Try
                GetNameValuePairDetails(mDataTable)
                mDataTable.Locale = CultureInfo.InvariantCulture
                mDataTable.DefaultView.RowFilter = String.Empty
                mDataView = New DataView(mDataTable)
                yourDataTable = mDataView.Table.Clone()
                mDataView.RowFilter = "NVP_SEQ_ID = " & nameValuePairSeqId
                For Each drv As DataRowView In mDataView
                    yourDataTable.ImportRow(drv.Row)
                Next
            Catch ex As Exception
                Dim mLog As Logger = Logger.Instance()
                mLog.Error(ex)
                Throw
            Finally
                If Not mDataTable Is Nothing Then
                    mDataTable.Dispose()
                    mDataTable = Nothing
                End If
                If Not mDataView Is Nothing Then
                    mDataView.Dispose()
                    mDataView = Nothing
                End If
            End Try

        End Sub

        ''' <summary>
        ''' Gets the NVP details.
        ''' </summary>
        ''' <param name="staticName">Name of the static.</param>
        ''' <returns>DataTable.</returns>
        Public Function GetNameValuePairDetails(ByVal staticName As String) As DataTable
            Dim mDataView As DataView = Nothing
            Dim mDataTable As DataTable = Nothing
            Dim mReturnTable As DataTable = Nothing
            Try
                GetNameValuePairDetails(mDataTable)
                mDataTable.Locale = CultureInfo.InvariantCulture
                mDataView = mDataTable.DefaultView
                mReturnTable = mDataView.Table.Clone()
                mDataView.RowFilter = "TABLE_NAME = '" & staticName & "'"
                For Each drv As DataRowView In mDataView
                    mReturnTable.ImportRow(drv.Row)
                Next
            Catch ex As Exception
                Throw
            Finally
                If Not mDataTable Is Nothing Then
                    mDataTable.Dispose()
                    mDataTable = Nothing
                End If
                If Not mDataView Is Nothing Then
                    mDataView.Dispose()
                    mDataView = Nothing
                End If
            End Try
            Return mReturnTable
        End Function

        ''' <summary>
        ''' Gets the NVP details.
        ''' </summary>
        ''' <param name="nameValuePairSeqId">The NVP seq ID.</param>
        ''' <returns>DataTable.</returns>
        Public Function GetNameValuePairDetails(ByVal nameValuePairSeqId As Integer) As DataTable
            Dim mDataView As DataView = Nothing
            Dim mDataTable As DataTable = Nothing
            Dim mReturnTable As DataTable = Nothing
            Try
                GetNameValuePairDetails(mDataTable)
                mDataTable.Locale = CultureInfo.InvariantCulture
                mDataView = mDataTable.DefaultView
                mReturnTable = mDataView.Table.Clone()
                mDataView.RowFilter = "NVP_SEQ_ID = " & nameValuePairSeqId
                For Each drv As DataRowView In mDataView
                    mReturnTable.ImportRow(drv.Row)
                Next

            Catch ex As Exception
                Throw
            Finally
                If Not mDataTable Is Nothing Then
                    mDataTable.Dispose()
                    mDataTable = Nothing
                End If
                If Not mDataView Is Nothing Then
                    mDataView.Dispose()
                    mDataView = Nothing
                End If
            End Try
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
        ''' <param name="nameValuePairSeqId">The name value pair seq ID.</param>
        ''' <returns>System.String[][].</returns>
        Public Function GetSelectedGroups(ByVal nameValuePairSeqId As Integer) As String()
            Dim mNameValuePair As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            Dim mDataTable As DataTable = mNameValuePair.GetNameValuePairGroups(nameValuePairSeqId)
            Return GetStringArrayList(mDataTable, "Groups")
        End Function

        ''' <summary>
        ''' Gets the string array list.
        ''' </summary>
        ''' <param name="yourDataTable">Your DT.</param>
        ''' <param name="rowName">Name of the row.</param>
        ''' <returns>System.String[][].</returns>
        Private Function GetStringArrayList(ByVal yourDataTable As DataTable, ByVal rowName As String) As String()
            Dim mRetrunArrayList As New ArrayList
            Dim mDR As DataRow
            For Each mDR In yourDataTable.Rows
                mRetrunArrayList.Add(mDR(rowName).ToString)
            Next
            Return CType(mRetrunArrayList.ToArray(GetType(String)), String())
        End Function

        ''' <summary>
        ''' Saves the specified profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Function Save(ByVal profile As MNameValuePair) As Integer
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mNameValuePair As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            Dim mRetVal As Integer = -1
            mRetVal = mNameValuePair.Save(profile)
            CacheController.RemoveFromCache(CachedNameValuePairTableName)
            CacheController.RemoveFromCache(CachedNameValuePairDetailsTableName)
            Return mRetVal
        End Function

        ''' <summary>
        ''' Saves the detail.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Sub SaveDetail(ByVal profile As MNameValuePairDetail)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mBNameValuePairs As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            mBNameValuePairs.SaveNameValuePairDetail(profile)
            CacheController.RemoveFromCache(CachedNameValuePairDetailsTableName)
        End Sub

        ''' <summary>
        ''' Searches the specified search criteria.
        ''' </summary>
        ''' <param name="searchCriteria">The search criteria.</param>
        ''' <returns>DataTable.</returns>
        Public Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            If searchCriteria Is Nothing Then Throw New ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!")
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
        Public Sub SetDropSelection(ByVal theDropDown As ListControl, ByVal selectedVale As String)
            If theDropDown Is Nothing Then Throw New ArgumentNullException("theDropDown", "theDropDown cannot be a null reference (Nothing in Visual Basic)!")
            If String.IsNullOrEmpty(selectedVale) Then Throw New ArgumentNullException("selectedVale", "selectedVale cannot be a null reference (Nothing in Visual Basic)!")
            Try
                Dim X As Integer
                For X = 0 To theDropDown.Items.Count - 1
                    If theDropDown.Items(X).Value = selectedVale Then
                        theDropDown.SelectedIndex = X
                        Exit For
                    End If
                Next
            Catch ex As Exception
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' Updates the groups.
        ''' </summary>
        ''' <param name="nameValuePairId">The name value pair Id.</param>
        ''' <param name="securityEntityId">The security entity Id.</param>
        ''' <param name="commaSeparatedGroups">The comma Separated groups.</param>
        ''' <param name="nameValuePairProfile">MNameValuePair</param>
        Public Sub UpdateGroups(ByVal nameValuePairId As Integer, ByVal securityEntityId As Integer, ByVal commaSeparatedGroups As String, ByVal nameValuePairProfile As MNameValuePair)
            Dim mNameValuePair As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            mNameValuePair.UpdateGroups(nameValuePairId, securityEntityId, commaSeparatedGroups, nameValuePairProfile)
        End Sub

        ''' <summary>
        ''' Updates the roles.
        ''' </summary>
        ''' <param name="nameValuePairId">The NV p_ ID.</param>
        ''' <param name="securityEntityId">The security entity ID.</param>
        ''' <param name="commaSeparatedRoles">The comma Separated roles.</param>
        ''' <param name="nameValuePairProfile">nameValuePairProfile</param>
        Public Sub UpdateRoles(ByVal nameValuePairId As Integer, ByVal securityEntityId As Integer, ByVal commaSeparatedRoles As String, ByVal nameValuePairProfile As MNameValuePair)
            Dim mNameValuePair As New BNameValuePairs(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement)
            mNameValuePair.UpdateRoles(nameValuePairId, securityEntityId, commaSeparatedRoles, nameValuePairProfile)
        End Sub
    End Module
End Namespace
