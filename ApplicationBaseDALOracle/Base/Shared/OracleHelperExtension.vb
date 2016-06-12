Imports System
Imports System.Data

Namespace SharedOracle
    Public NotInheritable Class OracleHelperExtension
        Private Sub New()
        End Sub

        'Fills a typed DataSet using the DataReader's current result. This method 
        'allows paginated access to the database.
        'Parameters: 
        '-dataReader: The DataReader used to fetch the values.
        '-dataSet: The DataSet used to store the values.
        '-tableName: The name of the DataSet table used to add the DataReader records.
        '-from: The quantity of records skipped before placing values on the DataReader on the DataSet.
        '-count: The maximum quantity of records allowed to fill on the DataSet. (Value of 0 puts all records into the DataSet)
        Public Shared Sub Fill(ByVal dataReader As IDataReader, ByVal dataSet As DataSet, ByVal tableName As String, Optional ByVal from As Integer = 0, Optional ByVal count As Integer = 0)
            On Error Resume Next
            If tableName Is Nothing Then
                tableName = "unknownTable"
            End If
            If dataSet.Tables(tableName) Is Nothing Then
                dataSet.Tables.Add(tableName)
            End If
            ' Get the DataTable reference
            Dim fillTable As DataTable
            If tableName Is Nothing Then
                fillTable = dataSet.Tables(0)
            Else
                fillTable = dataSet.Tables(tableName)
            End If
            Dim fillRow As DataRow
            Dim fieldName As String
            Dim recNumber As Integer = 0
            Dim totalRecords As Integer = from + count
            While dataReader.Read()
                recNumber += 1
                If recNumber >= from Then
                    fillRow = fillTable.NewRow()
                    Dim fieldIdx As Integer
                    For fieldIdx = 0 To dataReader.FieldCount - 1
                        fieldName = dataReader.GetName(fieldIdx)
                        If fillTable.Columns.IndexOf(fieldName) = -1 Then
                            fillTable.Columns.Add(fieldName, dataReader.GetValue(fieldIdx).GetType())
                        End If
                        fillRow(fieldName) = dataReader.GetValue(fieldIdx)
                    Next fieldIdx
                    fillTable.Rows.Add(fillRow)
                End If
                If count <> 0 AndAlso totalRecords <= recNumber Then
                    Exit While
                End If
            End While
            dataSet.AcceptChanges()
        End Sub
    End Class
End Namespace