Imports System
Imports System.Data
Imports System.Reflection
Imports GrowthWare.Framework.ModelObjects.Base.Interfaces

Namespace ModelObjects.Base
	''' <summary>
	''' Contains all profile base properties
	''' </summary>
	''' <remarks></remarks>
	<Serializable()> _
	Public MustInherit Class MProfile
		Implements IMProfile

		Private m_Id As Integer = -1

#Region "Public Properties"
		Public Property AddedBy As Integer Implements IMProfile.AddedBy
		Public Property AddedDate As Date Implements IMProfile.AddedDate
		Public Property Id As Integer Implements IMProfile.Id
			Get
				Return m_Id
			End Get
			Set(value As Integer)
				m_Id = value
			End Set
		End Property
		Public Property Name As String Implements IMProfile.Name
		Public Property UpdatedBy As Integer Implements IMProfile.UpdatedBy
		Public Property UpdatedDate As Date Implements IMProfile.UpdatedDate
#End Region

#Region "Protected Methods"
		''' <summary>
		''' Initializes values given a DataRow
		''' </summary>
		''' <param name="Datarow">DataRow</param>
		Protected Overridable Sub Initialize(ByRef Datarow As DataRow)
			Me.AddedBy = Me.GetInt(Datarow, "Added_By")
			Me.AddedDate = Me.GetDateTime(Datarow, "Added_Date", DateTime.Now)
			Me.UpdatedBy = Me.GetInt(Datarow, "Updated_By")
			Me.UpdatedDate = Me.GetDateTime(Datarow, "Updated_Date", DateTime.Now)
		End Sub

		' ''' <summary>
		' ''' Returns a boolean from a datarow.
		' ''' </summary>
		' ''' <param name="dr">DataRow</param>
		' ''' <param name="columnName">Name of the column to retrieve from the data row</param>
		'Protected Sub setBool(ByRef YourObject As Boolean, ByRef ColumnName As String, ByRef dr As DataRow)
		'	If Not dr Is Nothing And dr.Table.Columns.Contains(ColumnName) And Not Convert.IsDBNull(dr(ColumnName)) Then
		'		YourObject = Boolean.Parse(dr(ColumnName).ToString())
		'	End If
		'End Sub

		' ''' <summary>
		' ''' Returns a date time object from a datarow.
		' ''' </summary>
		' ''' <param name="dr">DataRow</param>
		' ''' <param name="columnName">Name of the column to retrieve from the data row</param>
		' ''' <param name="defaultDateTime">Date time object used as default</param>
		'Protected Sub setDate(ByRef YourObject As Date, ByRef ColumnName As String, ByRef DR As DataRow, ByVal DefaultDateTime As DateTime)
		'	YourObject = DefaultDateTime
		'	If Not DR Is Nothing And DR.Table.Columns.Contains(ColumnName) And Not Convert.IsDBNull(DR(ColumnName)) Then
		'		YourObject = DateTime.Parse(DR(ColumnName).ToString())
		'	End If
		'End Sub

		'Protected Sub setInteger(ByRef YourObject As Integer, ByRef ColumnName As String, ByRef DR As DataRow)
		'	If Not DR Is Nothing And DR.Table.Columns.Contains(ColumnName) And Not Convert.IsDBNull(DR(ColumnName)) Then
		'		YourObject = Integer.Parse(DR(ColumnName).ToString())
		'	End If
		'End Sub

		'Protected Sub setString(ByRef YourObject As String, ByRef ColumnName As String, ByRef DR As DataRow)
		'	If Not DR Is Nothing And DR.Table.Columns.Contains(ColumnName) And Not Convert.IsDBNull(DR(ColumnName)) Then
		'		YourObject = DR(ColumnName).ToString()
		'	End If
		'End Sub

		''' <summary>
		''' Returns a boolean given the a DataRow and Column name.
		''' </summary>
		''' <param name="Datarow">DataRow</param>
		''' <param name="ColumnName">String</param>
		''' <returns>Boolean</returns>
		''' <remarks></remarks>
		Protected Function GetBool(ByRef Datarow As DataRow, ByVal ColumnName As String) As Boolean
			Dim mRetVal As Boolean = False
			If Not Datarow Is Nothing AndAlso Datarow.Table.Columns.Contains(ColumnName) AndAlso Not Convert.IsDBNull(Datarow(ColumnName)) Then
				If Not Datarow(ColumnName).ToString() = "0" Then
					mRetVal = True
				End If
			End If
			Return mRetVal
		End Function

		''' <summary>
		''' Returns a DateTime given the a DataRow and Column name and the defaul value.
		''' </summary>
		''' <param name="Datarow">DataRow</param>
		''' <param name="ColumnName">String</param>
		''' <param name="DefaultDateTime">DateTime</param>
		''' <returns>DateTime</returns>
		''' <remarks></remarks>
		Protected Function GetDateTime(ByRef Datarow As DataRow, ByVal ColumnName As String, ByVal DefaultDateTime As DateTime) As DateTime
			Dim mRetVal As DateTime = DefaultDateTime
			If Not Datarow Is Nothing AndAlso Datarow.Table.Columns.Contains(ColumnName) AndAlso Not Convert.IsDBNull(Datarow(ColumnName)) Then
				mRetVal = DateTime.Parse(Datarow(ColumnName).ToString())
			End If
			Return mRetVal
		End Function

		''' <summary>
		''' Returns a Integer given the a DataRow and Column name.
		''' </summary>
		''' <param name="Datarow">DataRow</param>
		''' <param name="ColumnName">String</param>
		''' <returns>Integer</returns>
		''' <remarks></remarks>
		Protected Function GetInt(ByRef Datarow As DataRow, ByVal ColumnName As String) As Integer
			Dim mRetVal As Integer = -1
			If Not Datarow Is Nothing AndAlso Datarow.Table.Columns.Contains(ColumnName) AndAlso Not Convert.IsDBNull(Datarow(ColumnName)) Then
				mRetVal = Integer.Parse(Datarow(ColumnName).ToString())
			End If
			Return mRetVal
		End Function

		''' <summary>
		''' Returns a String given the a DataRow and Column name.
		''' </summary>
		''' <param name="Datarow">DataRow</param>
		''' <param name="ColumnName">String</param>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Protected Function GetString(ByRef Datarow As DataRow, ByVal ColumnName As String) As String
			Dim mRetVal As String = String.Empty
			If Not Datarow Is Nothing AndAlso Datarow.Table.Columns.Contains(ColumnName) AndAlso Not Convert.IsDBNull(Datarow(ColumnName)) Then
				mRetVal = Datarow(ColumnName).ToString().Trim()
			End If
			Return mRetVal
		End Function

#End Region

		''' <summary>
		''' Returns all properties encapsulated by angle brackets seporated by the Seporator parameter
		''' </summary>
		''' <param name="Seporator">string</param>
		''' <returns>string</returns>
		Public Function GetTags(ByVal Seporator As String) As String
			Dim mRetVal As String = String.Empty
			Dim mPropertyInfo As PropertyInfo() = Me.GetType.GetProperties()
			For Each mPropertyItem As PropertyInfo In mPropertyInfo
				mRetVal = mRetVal & "<" & mPropertyItem.Name & ">" & Seporator
			Next
			Return mRetVal
		End Function
	End Class
End Namespace