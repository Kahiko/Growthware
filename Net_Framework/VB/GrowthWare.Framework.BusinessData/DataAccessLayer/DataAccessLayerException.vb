﻿Imports System.Runtime.Serialization

Namespace DataAccessLayer
    ''' <summary>
    ''' Created to distinguish errors created in the data access layer.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()>
    Public NotInheritable Class DataAccessLayerException
        Inherits Exception

        ''' <summary>
        ''' Creates a new instance of the DataAccessLayerException class
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

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

        ''' <summary>
        ''' Calls base method
        ''' </summary>
        ''' <param name="info"></param>
        ''' <param name="context"></param>
        ''' <remarks></remarks>
        Private Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class

End Namespace
