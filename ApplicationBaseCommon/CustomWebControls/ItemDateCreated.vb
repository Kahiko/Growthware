Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports ApplicationBase.Model

Namespace CustomWebControls
    '*********************************************************************
    '
    ' ItemDateCreated Class
    '
    ' Represents a date created displayed in a template
    '
    '*********************************************************************
    <Designer(GetType(CustomWebControls.CustomDesigner))> _
    Public Class ItemDateCreated
        Inherits ItemDate

        '*********************************************************************
        '
        ' ItemDateCreated Constructor
        '
        ' Assign a default css class
        '
        '*********************************************************************
        Public Sub New()
            CssClass = "itemDateCreated"
            EnableViewState = False
        End Sub 'New

        '*********************************************************************
        '
        ' AssignContentItem Method
        '
        ' Assigns the correct content item to the date.
        '
        '*********************************************************************
        Protected Overrides Sub AssignContentItem(ByVal contentInfo As ContentInfo)
            [Date] = contentInfo.DateCreated
        End Sub 'AssignContentItem
    End Class 'ItemDateCreated 
End Namespace