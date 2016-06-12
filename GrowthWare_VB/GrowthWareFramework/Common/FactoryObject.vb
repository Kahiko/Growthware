Imports System
Imports System.Reflection

Namespace Common
	''' <summary>
	''' The FactoryObject will create an instance of an object from any 
	''' Assembley give the assembly name, namespace, and the object/class name.
	''' </summary>
	''' <remarks>
	''' None
	''' </remarks>
	Public NotInheritable Class FactoryObject
		Private Sub New()
		End Sub

		''' <summary>
		''' Creates an instance of an object.
		''' </summary>
		''' <param name="theAssemblyName">
		''' The name of the assembley (DLL).  Must be 
		''' included in your solution in order to find the file.
		''' </param>
		''' <param name="TheNamespace">
		''' The name space where the class is located.
		''' </param>
		''' <param name="TheClassName">
		''' The name of the class you need an instance of.
		''' </param>
		''' <returns>An object</returns>
		''' <remarks></remarks>
		Shared Function Create(ByVal theAssemblyName As String, ByVal theNamespace As String, ByVal theClassName As String) As Object
			If (theAssemblyName Is Nothing) Then
				Throw New ArgumentNullException("theAssemblyName", "theAssemblyName cannot be a null reference (Nothing in Visual Basic)")
			End If
			If theNamespace Is Nothing Then
				Throw New ArgumentNullException("theNamespace", "theNamespace cannot be a null reference (Nothing in Visual Basic)")
			End If
			If theClassName Is Nothing Then
				Throw New ArgumentNullException("theClassName", "theClassName cannot be a null reference (Nothing in Visual Basic)")
			End If
			Dim mReturnObject As Object = Nothing
			Try
				Dim TheAssembly As Assembly = System.Reflection.Assembly.Load(theAssemblyName)
				If theNamespace.Length > 0 Then
					mReturnObject = TheAssembly.CreateInstance(theNamespace & "." & theClassName, True)
				Else
					mReturnObject = TheAssembly.CreateInstance(theClassName, True)
				End If
			Catch ex As Exception
				Throw
			End Try
			If mReturnObject Is Nothing Then
				Dim exMessage As String = "FactoryObject :: Create() theAssemblyName: " + theAssemblyName + " theNamespace: " + theNamespace + " theClassName: " + theClassName + " could not be created" + Environment.NewLine
				Throw New Exception(exMessage)
			End If
			Return mReturnObject
		End Function
	End Class
End Namespace