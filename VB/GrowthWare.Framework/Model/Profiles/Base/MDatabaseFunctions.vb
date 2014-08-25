Imports System.Globalization

Namespace Model.Profiles.Base
    ''' <summary>
    ''' Class MDBFunctions servers as the base class for classes needing to populate properties from DB data.
    ''' Code only no properties.
    ''' Inherit from MProfile if you need the base properties as well.
    ''' </summary>
    <CLSCompliant(True)> _
    Public MustInherit Class MDatabaseFunctions
        ''' <summary>
        ''' Returns a boolean given the DataRow and Column name or either bit for int values.
        ''' </summary>
        ''' <param name="dataRow">DataRow</param>
        ''' <param name="columnName">String</param>
        ''' <returns>Boolean</returns>
        ''' <remarks>Integer or int values not equal to 0 are considered true</remarks>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId:="bool")>
        Protected Function GetBool(ByVal dataRow As DataRow, ByVal columnName As String) As Boolean
            Dim mRetVal As Boolean = False
            If Not dataRow Is Nothing AndAlso dataRow.Table.Columns.Contains(columnName) AndAlso Not Convert.IsDBNull(dataRow(columnName)) Then
                If dataRow(columnName).ToString() = "1" Or dataRow(columnName).ToString().ToUpper(CultureInfo.InvariantCulture) = "TRUE" Then
                    mRetVal = True
                End If
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Returns a DateTime given the a DataRow and Column name and the defaul value.
        ''' </summary>
        ''' <param name="dataRow">DataRow</param>
        ''' <param name="columnName">String</param>
        ''' <param name="defaultDateTime">DateTime</param>
        ''' <returns>DateTime</returns>
        ''' <remarks></remarks>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
        Protected Function GetDateTime(ByVal dataRow As DataRow, ByVal columnName As String, ByVal defaultDateTime As DateTime) As DateTime
            Dim mRetVal As DateTime = defaultDateTime
            If Not dataRow Is Nothing AndAlso dataRow.Table.Columns.Contains(columnName) AndAlso Not Convert.IsDBNull(dataRow(columnName)) Then
                mRetVal = DateTime.Parse(dataRow(columnName).ToString(), CultureInfo.InvariantCulture)
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Returns a Integer given the a DataRow and Column name.
        ''' </summary>
        ''' <param name="dataRow">DataRow</param>
        ''' <param name="columnName">String</param>
        ''' <returns>Integer</returns>
        ''' <remarks></remarks>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId:="int")> <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
        Protected Function GetInt(ByVal dataRow As DataRow, ByVal columnName As String) As Integer
            Dim mRetVal As Integer = -1
            If Not dataRow Is Nothing AndAlso dataRow.Table.Columns.Contains(columnName) AndAlso Not Convert.IsDBNull(dataRow(columnName)) Then
                mRetVal = Integer.Parse(dataRow(columnName).ToString(), CultureInfo.InvariantCulture)
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Returns a String given the a DataRow and Column name.
        ''' </summary>
        ''' <param name="dataRow">DataRow</param>
        ''' <param name="columnName">String</param>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")> <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
        Protected Function GetString(ByVal dataRow As DataRow, ByVal columnName As String) As String
            Dim mRetVal As String = String.Empty
            If Not dataRow Is Nothing AndAlso dataRow.Table.Columns.Contains(columnName) AndAlso Not Convert.IsDBNull(dataRow(columnName)) Then
                mRetVal = dataRow(columnName).ToString().Trim()
            End If
            Return mRetVal
        End Function
    End Class
End Namespace
