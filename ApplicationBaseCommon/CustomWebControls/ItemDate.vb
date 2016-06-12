Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports System.Diagnostics
Imports ApplicationBase.Model
Imports ApplicationBase.Common.CustomWebControls

'*********************************************************************
'
' ItemDate Class
'
' Abstract class for displaying a date in a template.
' This class handles issues with time conversions.
'
'*********************************************************************

Public MustInherit Class ItemDate
    Inherits WebControl

    Private _dateFormatString As String = "{0:D}"
    Private _displayTimeZone As Boolean = True

    '*********************************************************************
    '
    ' Date Property
    '
    ' Stores date in view state.
    '
    '*********************************************************************
    Public Property [Date]() As DateTime
        Get
            Return CType(ViewState("Date"), DateTime)
        End Get
        Set(ByVal value As DateTime)
            ViewState("Date") = value
        End Set
    End Property

    '*********************************************************************
    '
    ' DisplayTimeZone Property
    '
    ' Determines whether time zone is displayed.
    '
    '*********************************************************************
    Public Property DisplayTimeZone() As Boolean
        Get
            Return _displayTimeZone
        End Get
        Set(ByVal value As Boolean)
            _displayTimeZone = value
        End Set
    End Property

    '*********************************************************************
    '
    ' DateFormatString Property
    '
    ' Enables the date to be formatted.
    '
    '*********************************************************************
    Public Property DateFormatString() As String
        Get
            Return _dateFormatString
        End Get
        Set(ByVal value As String)
            _dateFormatString = value
        End Set
    End Property

    '*********************************************************************
    '
    ' OnDataBinding Method
    '
    ' Get the date commented from container.
    '
    '*********************************************************************
    Protected Overrides Sub OnDataBinding(ByVal e As EventArgs)
        Dim item As ContentItem

        If NamingContainer.GetType.Name.ToLower = "contentitem" Then
            item = CType(NamingContainer, ContentItem)
        Else
            item = CType(NamingContainer.NamingContainer, ContentItem)
        End If

        Dim objContentInfo As ContentInfo = CType(item.DataItem, ContentInfo)
        AssignContentItem(objContentInfo)
    End Sub 'OnDataBinding

    '*********************************************************************
    '
    ' AssignContentItem Method
    '
    ' Override this method to add an item to the Date property.
    '
    '*********************************************************************
    Protected MustOverride Sub AssignContentItem(ByVal contentInfo As ContentInfo)

    '*********************************************************************
    '
    ' LocalTimeZoneAbbreviation Property
    '
    ' Get the abbreviated name of server time zone.
    '
    '*********************************************************************
    Private ReadOnly Property LocalTimeZoneAbbreviation() As String
        Get
            Dim splitTZ As String() = TimeZone.CurrentTimeZone.StandardName.Split(" "c)
            Dim _abb As String = String.Empty
            Dim word As String
            For Each word In splitTZ
                _abb += Left(word, 1)
            Next word
            Return _abb
        End Get
    End Property

    '*********************************************************************
    '
    ' RenderContents Method
    '
    ' Render the date.
    ' Note: We convert the date to the local server time.
    '
    '*********************************************************************
    Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
        writer.Write(String.Format(_dateFormatString, [Date].ToLocalTime()))
        If _displayTimeZone Then
            writer.Write(String.Format(" ({0})", LocalTimeZoneAbbreviation))
        End If
    End Sub 'RenderContents 
End Class 'ItemDate 