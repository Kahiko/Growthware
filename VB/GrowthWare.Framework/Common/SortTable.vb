Imports System.Globalization

Namespace Common
    Public Class SortTable
        Private m_StartTime As DateTime = DateTime.Now
        Private m_StopTime As DateTime = DateTime.Now

        Public ReadOnly Property StartTime() As DateTime
            Get
                Return m_StartTime
            End Get
        End Property

        Public ReadOnly Property StopTime() As DateTime
            Get
                Return m_StopTime
            End Get
        End Property

        ''' <summary>
        ''' Initializes a new instance of the <see cref="SortTable" /> class.
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Sorts the specified data table.
        ''' </summary>
        ''' <param name="dataTable">The data table.</param>
        ''' <param name="dataColumn">The column.</param>
        ''' <param name="sortDirection">The sort direction.</param>
        Public Sub Sort(ByVal dataTable As DataTable, ByVal dataColumn As DataColumn, ByVal sortDirection As String)
            If dataTable Is Nothing Then Throw New ArgumentNullException("dataTable", "dataTable can not be null!")
            If dataColumn Is Nothing Then Throw New ArgumentNullException("dataColumn", "dataColumn can not be null!")
            If String.IsNullOrEmpty(sortDirection) Then Throw New ArgumentNullException("sortDirection", "sortDirection can not be null!")
            m_StartTime = DateTime.Now
            Dim rowCount As Integer = dataTable.Rows.Count - 1
            Dim sortValues(rowCount) As String
            Dim sortIndex(rowCount) As String
            For i As Integer = 0 To rowCount
                sortIndex(i) = i.ToString(CultureInfo.InvariantCulture)
                sortValues(i) = dataTable.Rows(i)(dataColumn).ToString()
            Next
            If sortDirection = "ASC" Then
                Array.Sort(sortValues, sortIndex, New NaturalComparer(NaturalComparerOption.None))
            Else
                Array.Sort(sortValues, sortIndex, New NaturalComparer(NaturalComparerOption.None, NaturalComparerDirections.Descending))
            End If
            For i As Integer = 0 To sortIndex.GetUpperBound(0)
                dataTable.ImportRow(dataTable.Rows(Integer.Parse(sortIndex(i), CultureInfo.InvariantCulture)))
            Next
            For i As Integer = 0 To sortIndex.GetUpperBound(0)
                dataTable.Rows.RemoveAt(0)
            Next
            m_StopTime = DateTime.Now
        End Sub

        ''' <summary>
        ''' Sorts the specified data table.
        ''' </summary>
        ''' <param name="dataTable">The data table.</param>
        ''' <param name="dataColumn">The column.</param>
        ''' <remarks>Calls Sort passing sortDirection of "ASC"</remarks>
        Public Sub Sort(ByVal dataTable As DataTable, ByVal dataColumn As DataColumn)
            If dataTable Is Nothing Then Throw New ArgumentNullException("dataTable", "dataTable can not be null!")
            If dataColumn Is Nothing Then Throw New ArgumentNullException("dataColumn", "dataColumn can not be null!")
            Sort(dataTable, dataColumn, "ASC")
        End Sub

        ''' <summary>
        ''' Sorts the specified data table.
        ''' </summary>
        ''' <param name="dataTable">The data table.</param>
        ''' <param name="columnName">The column.</param>
        ''' <param name="sortDirection">The sort direction.</param>
        Public Sub Sort(ByVal dataTable As DataTable, ByVal columnName As String, ByVal sortDirection As String)
            If dataTable Is Nothing Then Throw New ArgumentNullException("dataTable", "dataTable can not be null!")
            If String.IsNullOrEmpty(columnName) Then Throw New ArgumentNullException("columnName", "columnName can not be null!")
            If String.IsNullOrEmpty(sortDirection) Then Throw New ArgumentNullException("sortDirection", "sortDirection can not be null!")
            m_StartTime = DateTime.Now
            Dim rowCount As Integer = dataTable.Rows.Count - 1
            Dim sortValues(rowCount) As String
            Dim sortIndex(rowCount) As String
            For i As Integer = 0 To rowCount
                sortIndex(i) = i.ToString(CultureInfo.InvariantCulture)
                sortValues(i) = dataTable.Rows(i)(columnName).ToString
            Next
            If sortDirection = "ASC" Then
                Array.Sort(sortValues, sortIndex, New NaturalComparer(NaturalComparerOption.None))
            Else
                Array.Sort(sortValues, sortIndex, New NaturalComparer(NaturalComparerOption.None, NaturalComparerDirections.Descending))
            End If
            For i As Integer = 0 To sortIndex.GetUpperBound(0)
                dataTable.ImportRow(dataTable.Rows(Integer.Parse(sortIndex(i), CultureInfo.InvariantCulture)))
            Next
            For i As Integer = 0 To sortIndex.GetUpperBound(0)
                dataTable.Rows.RemoveAt(0)
            Next
            m_StopTime = DateTime.Now
        End Sub

        ''' <summary>
        ''' Sorts the specified data table.
        ''' </summary>
        ''' <param name="dataTable">The data table.</param>
        ''' <param name="columnName">The column.</param>
        ''' <remarks>Calls Sort passing "ASC" as sortDirection</remarks>
        Public Sub Sort(ByVal dataTable As DataTable, ByVal columnName As String)
            If dataTable Is Nothing Then Throw New ArgumentNullException("dataTable", "dataTable can not be null!")
            If String.IsNullOrEmpty(columnName) Then Throw New ArgumentNullException("columnName", "columnName can not be null!")
            Sort(dataTable, columnName, "ASC")
        End Sub
    End Class
End Namespace