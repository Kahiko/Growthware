Imports System.Runtime.Serialization

''' <summary>
''' Created to distinguish errors created in the Business LogicLayer Exceptions.
''' </summary>
''' <remarks></remarks>
<Serializable>
Public Class BusinessLogicLayerException
    Inherits Exception

    ''' <summary>
    ''' Calls base method
    ''' </summary>
    Public Sub New()

    End Sub

    ''' <summary>
    ''' Calls base method
    ''' </summary>
    ''' <param name="info">SerializationInfo</param>
    ''' <param name="context">StreamingContext</param>
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
