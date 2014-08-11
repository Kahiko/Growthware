Imports System
Imports System.Reflection

Namespace Common
    ''' <summary>
    ''' The ObjectFactory will create an instance of an object from any 
    ''' Assembley give the assembly name, namespace, and the object/class name.
    ''' </summary>
    ''' <remarks>
    ''' None
    ''' </remarks>
    Public NotInheritable Class ObjectFactory
        Private Sub New()
        End Sub

        ''' <summary>
        ''' Creates an instance of an object.
        ''' </summary>
        ''' <param name="assemblyName">
        ''' The name of the assembley (DLL).  Must be 
        ''' included in your solution in order to find the file.
        ''' </param>
        ''' <param name="TheNamespace">
        ''' The name space where the class is located.
        ''' </param>
        ''' <param name="className">
        ''' The name of the class you need an instance of.
        ''' </param>
        ''' <returns>An object</returns>
        ''' <remarks></remarks>
        Shared Function Create(ByVal assemblyName As String, ByVal theNamespace As String, ByVal className As String) As Object
            If (assemblyName Is Nothing) Then
                Throw New ArgumentNullException("assemblyName", "theAssemblyName cannot be a null reference (Nothing in Visual Basic)")
            End If
            If theNamespace Is Nothing Then
                Throw New ArgumentNullException("theNamespace", "theNamespace cannot be a null reference (Nothing in Visual Basic)")
            End If
            If className Is Nothing Then
                Throw New ArgumentNullException("className", "theClassName cannot be a null reference (Nothing in Visual Basic)")
            End If
            Dim mReturnObject As Object = Nothing
            Try
                Dim TheAssembly As Assembly = System.Reflection.Assembly.Load(assemblyName)
                If theNamespace.Length > 0 Then
                    mReturnObject = TheAssembly.CreateInstance(theNamespace & "." & className, True)
                Else
                    mReturnObject = TheAssembly.CreateInstance(className, True)
                End If
            Catch ex As Exception
                Throw
            End Try
            If mReturnObject Is Nothing Then
                Dim exMessage As String = String.Concat("Object ", theNamespace, ".", className, " could not be created from assembly ", assemblyName, System.Environment.NewLine)

                Throw New ObjectFactoryException(exMessage)
            End If
            Return mReturnObject
        End Function
    End Class
End Namespace