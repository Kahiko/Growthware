Namespace Common
    Public Class SortTable
        Private mStartTime As DateTime = DateTime.Now
        Private mStopTime As DateTime = DateTime.Now

        Public ReadOnly Property StartTime() As DateTime
            Get
                Return mStartTime
            End Get
        End Property

        Public ReadOnly Property StopTime() As DateTime
            Get
                Return mStopTime
            End Get
        End Property

        Public Sub New()

        End Sub

        Public Sub Sort(ByVal dt As DataTable, ByVal col As DataColumn, Optional ByVal SortDirection As String = "ASC")
            mStartTime = DateTime.Now
            Dim rowCount As Integer = dt.Rows.Count - 1
            Dim sortValues(rowCount) As String
            Dim sortIndex(rowCount) As String
            For i As Integer = 0 To rowCount
                sortIndex(i) = i.ToString
                sortValues(i) = dt.Rows(i)(col).ToString
            Next
            If SortDirection = "ASC" Then
                Array.Sort(sortValues, sortIndex, New NaturalComparer(NaturalComparerOptions.None))
            Else
                Array.Sort(sortValues, sortIndex, New NaturalComparer(NaturalComparerOptions.None, NaturalComparerDirection.Descending))
            End If
            For i As Integer = 0 To sortIndex.GetUpperBound(0)
                dt.ImportRow(dt.Rows(sortIndex(i)))
            Next
            For i As Integer = 0 To sortIndex.GetUpperBound(0)
                dt.Rows.RemoveAt(0)
            Next
            mStopTime = DateTime.Now
        End Sub

        Public Sub Sort(ByVal dt As DataTable, ByVal col As String, Optional ByVal SortDirection As String = "ASC")
            mStartTime = DateTime.Now
            Dim rowCount As Integer = dt.Rows.Count - 1
            Dim sortValues(rowCount) As String
            Dim sortIndex(rowCount) As String
            For i As Integer = 0 To rowCount
                sortIndex(i) = i.ToString
                sortValues(i) = dt.Rows(i)(col).ToString
            Next
            If SortDirection = "ASC" Then
                Array.Sort(sortValues, sortIndex, New NaturalComparer(NaturalComparerOptions.None))
            Else
                Array.Sort(sortValues, sortIndex, New NaturalComparer(NaturalComparerOptions.None, NaturalComparerDirection.Descending))
            End If
            For i As Integer = 0 To sortIndex.GetUpperBound(0)
                dt.ImportRow(dt.Rows(sortIndex(i)))
            Next
            For i As Integer = 0 To sortIndex.GetUpperBound(0)
                dt.Rows.RemoveAt(0)
            Next
            mStopTime = DateTime.Now
        End Sub
    End Class
End Namespace