Imports System.Data

#Region " Notes "
' The OrderedHierarchy class orders two related tables
' in an Heirachal manner.
' The class is not currently used by may prove to be usefull
' in the future.
#End Region
Public Class OrderedHierarchy
	' Receives a Table and orders the rows based on a specific relationship and
	' a parent column.
	Public Shared Function GetOrderedTable( _
	 ByVal baseTable As DataTable, _
	 ByVal relationshipName As String, _
	 ByVal parentColumnName As String _
	   ) As DataTable

		' Configure the ordered table that will be passed out to the client
		Dim tbl As DataTable = baseTable.Clone()
		tbl.Columns.Add(New DataColumn("Depth", GetType(Int32)))
		ComputeHierarchy(tbl, baseTable.Select(parentColumnName & "=0"), 0)
		Return tbl
	End Function

	' Recursive routine that appends rows to the Private 
	' table in an ordered manner
	Private Shared Sub ComputeHierarchy( _
	 ByRef orderedTable As DataTable, _
	 ByVal members() As DataRow, _
	 ByVal depth As Int32 _
	   )

		Dim member As DataRow
		For Each member In members
			orderedTable.ImportRow(member)
			orderedTable.Rows(orderedTable.Rows.Count - 1).Item("Depth") = depth
			ComputeHierarchy(orderedTable, member.GetChildRows("ParentChild"), depth + 1)
		Next
	End Sub
End Class