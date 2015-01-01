Imports System.Text
Imports System.Globalization

Namespace Utilities
    ''' <summary>
    ''' Utiltiy class to aid in a web application
    ''' </summary>
    ''' <remarks>Could be considered Specific to Growthware</remarks>
    Public Module MenuUtility

        ''' <summary>
        ''' Generates and order list form hierarchical data.
        ''' </summary>
        ''' <param name="menuData">Hierarchical datatable</param>
        ''' <param name="value">StringBuiler used to build the ul/li string data</param>
        ''' <returns>String</returns>
        ''' <remarks>Frist Layer of items should have a ParentID of 1</remarks>
        Public Function GenerateUnorderedList(ByVal menuData As DataTable, ByVal value As StringBuilder) As String
            If value Is Nothing Then Throw New ArgumentNullException("value", "value cannot be a null reference (Nothing in Visual Basic)!")
            value.AppendLine("<ul>")
            Dim datView As DataView = Nothing
            Try
                datView = New DataView(menuData)
                datView.RowFilter = "ParentID = 1"
                '//Populate menu with top menu items
                Dim datRow As DataRowView
                For Each datRow In datView
                    '//Define new menu item
                    If Integer.Parse(datRow("FUNCTION_TYPE_SEQ_ID").ToString(), CultureInfo.InvariantCulture) = 3 Then
                        value.AppendLine(createLIItem(datRow("Title"), datRow("URL"), datRow("Description"), True))
                    Else
                        value.AppendLine(createLIItem(datRow("Title"), datRow("URL"), datRow("Description"), False))
                    End If
                    '//Populate child items of this parent
                    addChildItems(menuData, datRow("MenuID"), value)
                Next
            Catch ex As Exception
                Throw
            Finally
                If Not datView Is Nothing Then datView.Dispose()
            End Try
            value.AppendLine("<ul>")
            Return value.ToString()
        End Function

        ''' <summary>
        ''' Add the child itmes to the StringBuiler
        ''' </summary>
        ''' <param name="menuData">DataTable</param>
        ''' <param name="parentID">Integer</param>
        ''' <param name="stringBuilder">StringBuiler</param>
        ''' <remarks></remarks>
        Private Sub addChildItems(ByVal menuData As DataTable, ByVal parentID As Integer, ByRef stringBuilder As StringBuilder)
            '//Populate DataView
            Dim datView As DataView = Nothing
            Try
                datView = New DataView(menuData)
                '//Filter child menu items
                datView.RowFilter = "parentid = " & parentID
                '//Populate parent menu item with child menu items
                Dim datRow As DataRowView
                stringBuilder.AppendLine("<ul>")
                For Each datRow In datView
                    '//Define new menu item
                    If Integer.Parse(datRow("FUNCTION_TYPE_SEQ_ID").ToString(), CultureInfo.InvariantCulture) = 3 Then
                        stringBuilder.AppendLine(createLIItem(datRow("Title"), datRow("URL"), datRow("Description"), True))
                    Else
                        stringBuilder.AppendLine(createLIItem(datRow("Title"), datRow("URL"), datRow("Description"), False))
                    End If

                    'stringBuilder.AppendLine(createLIItem(datRow("Title"), datRow("URL"), datRow("Description"), mHasChildren))
                    '//Populate child items of this parent
                    addChildItems(menuData, datRow("MenuID"), stringBuilder)
                Next
            Catch ex As Exception
                Throw
            Finally
                If Not datView Is Nothing Then datView.Dispose()
            End Try
            stringBuilder.AppendLine("</ul>")
        End Sub

        ''' <summary>
        ''' Add the li with either the class has-sub or without
        ''' </summary>
        ''' <param name="hrefText">Link Text</param>
        ''' <param name="action">Used to build the javascript</param>
        ''' <param name="hrefToolTip">Used for the tooltip property</param>
        ''' <param name="hasChildren">Determines the class='has-sub' for the li tag</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function createLIItem(ByVal hrefText As String, ByVal action As String, ByVal hrefToolTip As String, ByVal hasChildren As Boolean) As String
            Dim retVal As String = String.Empty
            If Not hasChildren Then
                retVal = "<li><a href=""" + action + """ title=""" + hrefToolTip + """><span>" + hrefText + "</span></a>"
            Else
                retVal = "<li class='has-sub'><a href='#' title=""" + hrefToolTip + """><span>" + hrefText + "</span></a>"
            End If
            Return retVal
        End Function
    End Module
End Namespace
