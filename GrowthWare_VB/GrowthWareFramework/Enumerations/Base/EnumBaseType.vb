Imports System.Collections.Generic
Imports System.Collections.ObjectModel

''' <summary>
''' Was exploring replacing enum to reduce warnings for using
''' an enum without a zero value.
''' Decided to mark the clase with
''' [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")
''' This code was derived from http://www.codeproject.com/KB/cs/EnhancedEnums.aspx
''' </summary>
''' <typeparam name="T"></typeparam>
Public MustInherit Class EnumBaseType(Of T)
		Inherits EnumBaseType<T>

		protected m_enumValues As List(Of(T)) = new List(Of(T))

		Public ReadOnly m_Key As Integer
		Public ReadOnly m_Value As String

		Public Sub EnumBaseType(ByVal key As Integer, ByVal value As String)
			m_Key = key
			m_Value = value
			m_enumValues.Add(CType(T, Action(Of T)))
		End Sub

		protected Function GetBaseValues() As ReadOnlyCollection(Of(T))
			Return m_enumValues.AsReadOnly()
		End Function

		Protected Function GetBaseByKey(ByVal key As Integer) As T
			foreach(T t in enumValues)
				If T.Key = key Then
					Return ToString()
				End If

			Return Nothing
		End Function

		Public Overrides Function ToString() As String
			Return Value
		End Function

End Class
