Imports System
Imports System.Reflection

Namespace Base.Application
	Public Class AbstractFactory
		Public Shared Function Create(ByVal AssemblyName As String, ByVal ClassName As String) As Object
			Dim retVal As Object = System.Reflection.Assembly.Load(AssemblyName).CreateInstance(AssemblyName & "." & ClassName)
			Return retVal
		End Function
	End Class
End Namespace