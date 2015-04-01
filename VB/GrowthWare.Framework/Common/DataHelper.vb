Imports GrowthWare.Framework.Model.Profiles

Namespace Common
    ''' <summary>
    ''' Class DataHelper is a helper class for System.Data objects
    ''' </summary>
    Public Module DataHelper
        Public ReadOnly TotalRowColumnName = "TotalRows"
        Public ReadOnly RowNumberColumnName = "RowNumber"

        ''' <summary>
        ''' Adds the auto increment field named using the RowNumberColumnName property.
        ''' </summary>
        ''' <param name="table">The table.</param>
        Public Sub AddAutoIncrementField(ByRef table As DataTable)
            AddAutoIncrementField(table, RowNumberColumnName)
        End Sub

        ''' <summary>
        ''' Adds the auto increment field.
        ''' </summary>
        ''' <param name="table">The table.</param>
        ''' <param name="columnName">Name of the column.</param>
        Public Sub AddAutoIncrementField(ByRef table As DataTable, ByVal columnName As String)
            If Not table.Columns.Contains(columnName) Then
                Dim mColumn As DataColumn = New DataColumn(columnName, Type.GetType("System.Int32"))
                mColumn.AutoIncrement = True
                mColumn.AutoIncrementSeed = 1
                mColumn.AutoIncrementStep = 1
                table.Columns.Add(mColumn)
                Dim intCtr As Integer = 0
                For Each mRow As DataRow In table.Rows
                    intCtr += 1
                    mRow.Item(columnName) = intCtr
                Next
                mColumn.ReadOnly = True
            End If
        End Sub

        ''' <summary>
        ''' Adds the total rows field.
        ''' </summary>
        ''' <param name="table">The table.</param>
        Public Sub AddTotalRowsField(ByRef table As DataTable)
            Dim mColumnName As String = TotalRowColumnName
            If Not table.Columns.Contains(mColumnName) Then
                Dim mColumn As DataColumn = New DataColumn(mColumnName, Type.GetType("System.Int32"))
                table.Columns.Add(mColumn)
                Dim mRowCount As Integer = table.Rows.Count
                For Each mRow As DataRow In table.Rows
                    mRow.Item(mColumnName) = mRowCount
                Next
                mColumn.ReadOnly = True
            End If
        End Sub

        ''' <summary>
        ''' Gets the page of data.
        ''' </summary>
        ''' <param name="dataTable">The data table.</param>
        ''' <param name="sort">The sort.</param>
        ''' <param name="filter">The filter.</param>
        ''' <param name="searchCriteria">The search criteria.</param>
        ''' <returns>System.Object.</returns>
        Public Function GetPageOfData(ByRef dataTable As DataTable, ByVal sort As String, ByVal filter As String, ByRef searchCriteria As MSearchCriteria)
            ' create a dataview object
            Dim mSortingDataView As DataView = dataTable.DefaultView
            ' apply any sorting using the searchCriteria
            If sort Is Nothing Then
                mSortingDataView.Sort = searchCriteria.OrderByColumn + " " + searchCriteria.OrderByDirection
            Else
                mSortingDataView.Sort = sort
            End If
            ' apply filtering
            If searchCriteria.WhereClause.Trim() <> "1 = 1" Then
                mSortingDataView.RowFilter = searchCriteria.WhereClause
            End If

            Dim mTempTable As DataTable = mSortingDataView.Table.Clone()
            For Each drv As DataRowView In mSortingDataView
                mTempTable.ImportRow(drv.Row)
            Next
            ' add the total rows field
            AddTotalRowsField(mTempTable)
            ' add the rownumber field
            AddAutoIncrementField(mTempTable)

            mSortingDataView = mTempTable.DefaultView
            ' apply paging data filter logic
            Dim mStartingRow As Integer = 1
            If searchCriteria.SelectedPage > 1 Then
                mStartingRow = searchCriteria.PageSize * (searchCriteria.SelectedPage - 1)
            End If
            Dim mEndingRow As Integer = mStartingRow + searchCriteria.PageSize
            mSortingDataView.RowFilter = "RowNumber >= " + mStartingRow.ToString() + " and RowNumber <= " + mEndingRow.ToString()
            Dim mRetTable As DataTable = mSortingDataView.Table.Clone()
            For Each drv As DataRowView In mSortingDataView
                mRetTable.ImportRow(drv.Row)
            Next
            Return mRetTable
        End Function

        ''' <summary>
        ''' Gets the page of data.
        ''' </summary>
        ''' <param name="dataTable">The data table.</param>
        ''' <param name="searchCriteria">The search criteria.</param>
        ''' <returns>DataTable.</returns>
        Public Function GetPageOfData(ByRef dataTable As DataTable, ByRef searchCriteria As MSearchCriteria) As DataTable
            Return GetPageOfData(dataTable, Nothing, Nothing, searchCriteria)
        End Function

        ''' <summary>
        ''' Gets a table given a DataView.
        ''' </summary>
        ''' <param name="dataView">The data view.</param>
        ''' <returns>DataTable.</returns>
        Public Function GetTable(ByRef dataView As DataView) As DataTable
            Dim mRetVal As DataTable = dataView.Table.Clone()
            For Each item As DataRowView In dataView
                mRetVal.ImportRow(item.Row)
            Next
            Return mRetVal
        End Function
    End Module
End Namespace
