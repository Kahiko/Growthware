Public Class TestNaturalSort
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mSortDirection As String = "ASC"
        If Request.QueryString("SortDirection") Is Nothing Then
            dropSortDirection.SelectedIndex = 0
        Else
            mSortDirection = Request.QueryString("SortDirection").ToString()
            If mSortDirection = "ASC" Then
                dropSortDirection.SelectedIndex = 0
            Else
                dropSortDirection.SelectedIndex = 1
            End If
        End If
        BindData(mSortDirection)
    End Sub

    Private Sub BindData(ByVal sortDirection As String)
        Dim oTable As New DataTable("MyTable")
        oTable.Columns.Add("COL1", System.Type.GetType("System.String"))
        oTable.Columns.Add("COL2", System.Type.GetType("System.String"))
        Dim oRow As DataRow = oTable.NewRow()
        oRow = oTable.NewRow()
        oRow("COL1") = "Chapter(10)"
        oRow("COL2") = "Chapter(10)"
        oTable.Rows.Add(oRow)
        oRow = oTable.NewRow()
        oRow("COL1") = "Chapter 2 Ep 2-3"
        oRow("COL2") = "Chapter 2 Ep 2-3"
        oTable.Rows.Add(oRow)
        oRow = oTable.NewRow()
        oRow("COL1") = "Chapter 2 Ep 1-2"
        oRow("COL2") = "Chapter 2 Ep 1-2"
        oTable.Rows.Add(oRow)
        oRow = oTable.NewRow()
        oRow("COL1") = ""
        oRow("COL2") = ""
        oTable.Rows.Add(oRow)
        oRow = oTable.NewRow()
        oRow("COL1") = "Rocky(IV)"
        oRow("COL2") = "Rocky(IV)"
        oTable.Rows.Add(oRow)
        oRow = oTable.NewRow()
        oRow("COL1") = "Chapter(1)"
        oRow("COL2") = "Chapter(1)"
        oTable.Rows.Add(oRow)
        oRow = oTable.NewRow()
        oRow("COL1") = "Chapter(11)"
        oRow("COL2") = "Chapter(11)"
        oTable.Rows.Add(oRow)
        oRow = oTable.NewRow()
        oRow("COL1") = "Rocky(I)"
        oRow("COL2") = "Rocky(I)"
        oTable.Rows.Add(oRow)
        oRow = oTable.NewRow()
        oRow("COL1") = "Rocky(II)"
        oRow("COL2") = "Rocky(II)"
        oTable.Rows.Add(oRow)
        oRow = oTable.NewRow()
        oRow("COL1") = "Rocky(IX)"
        oRow("COL2") = "Rocky(IX)"
        oTable.Rows.Add(oRow)
        oRow = oTable.NewRow()
        oRow("COL1") = "Rocky(X)"
        oRow("COL2") = "Rocky(X)"
        oTable.Rows.Add(oRow)
        oRow = oTable.NewRow()
        oRow("COL1") = "Chapter(2)"
        oRow("COL2") = "Chapter(2)"
        oTable.Rows.Add(oRow)
        oRow = oTable.NewRow()
        oRow("COL1") = "Chapter 1 Ep 2-3"
        oRow("COL2") = "Chapter 1 Ep 2-3"
        oTable.Rows.Add(oRow)
        Dim myDV As DataView = oTable.DefaultView
        'GridView1.DataSource = oTable
        'GridView1.DataBind()
        'DropDownList1.DataSource = oTable
        'DropDownList1.DataTextField = "COL1"
        'DropDownList1.DataBind()
        Dim mySorter As New Framework.Common.SortTable
        'mySorter.SortTable(oTable, (oTable.Columns(0)), dropSortDirection.SelectedValue)
        mySorter.Sort(oTable, "COL1", sortDirection)
        StartTime.Text = mySorter.StartTime
        StopTime.Text = mySorter.StopTime
        Dim ts As TimeSpan = mySorter.StopTime.Subtract(mySorter.StartTime)

        lblTotalTime.Text = ts.TotalMilliseconds
        GridView2.DataSource = oTable
        GridView2.DataBind()
        DropDownList2.DataSource = oTable
        DropDownList2.DataTextField = "COL1"
        DropDownList2.DataBind()
        Dim mySort As String = "COL1 " + sortDirection
        myDV.Sort = mySort
        DropDownList3.DataSource = myDV
        DropDownList3.DataTextField = "COL1"
        DropDownList3.DataBind()
        GridView3.DataSource = myDV
        GridView3.DataBind()
        oTable.Dispose()
        myDV.Dispose()
    End Sub

End Class