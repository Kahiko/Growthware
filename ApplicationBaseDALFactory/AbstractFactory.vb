Imports System
Imports System.Reflection

Public Class FactoryObject
	Public Shared Function Create(ByVal AssemblyName As String, ByVal ClassName As String) As Object
		Dim retVal As New Object
		Try
			retVal = System.Reflection.Assembly.Load(AssemblyName).CreateInstance(AssemblyName & "." & ClassName)
		Catch ex As Exception
			Throw ex
		End Try
		Return retVal
	End Function
End Class