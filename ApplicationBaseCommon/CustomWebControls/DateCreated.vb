Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports ApplicationBase.Model

Namespace CustomWebControls
    '*********************************************************************
    '
    ' DateCreated Class
    '
    ' Represents the date content was added.
    ' This control gets most of its functionality from the base
    ' DisplayDate class.
    '
    '*********************************************************************
    Public Class DateCreated
        Inherits DisplayDate

        '*********************************************************************
        '
        ' DateCreated Constructor
        '
        ' Assign a default css class
        '
        '*********************************************************************
        Public Sub New()
            CssClass = "dateCreated"
        End Sub 'New

        '*********************************************************************
        '
        ' Date Property
        '
        ' Overrides the base Data property with a particular date
        '
        '*********************************************************************
        Public Overrides ReadOnly Property [Date]() As DateTime
            Get
                Dim objContentInfo As ContentInfo = CType(Context.Items("ContentInfo"), ContentInfo)
                Return objContentInfo.DateCreated
            End Get
        End Property
    End Class 'DateCreated 
End Namespace
