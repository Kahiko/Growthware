Imports System.Runtime.Serialization

''' <summary>
''' Created to distinguish errors created in the GrowthWare.WebSupport project.
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class WebSupportException
    Inherits Exception

    Public Sub New()

    End Sub

    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
    End Sub

    ''' <summary>
    ''' Calls base method
    ''' </summary>
    ''' <param name="message">String</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    ''' <summary>
    ''' Calls base method
    ''' </summary>
    ''' <param name="message">String</param>
    ''' <param name="innerException">Exception</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub
End Class
