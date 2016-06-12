Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel

Namespace CustomWebControls
    '*********************************************************************
    '
    ' DisplayDate Class
    '
    ' Abstract class for displaying date
    '
    '*********************************************************************
    <Designer(GetType(CustomWebControls.CustomDesigner))> _
    Public MustInherit Class DisplayDate
        Inherits WebControl

        Private _dateFormatString As String = "{0:D}"
        Private _displayTimeZone As Boolean = True
        '*********************************************************************
        '
        ' Date Property
        '
        ' Override in derived class to display date.
        '
        '*********************************************************************

        Public MustOverride ReadOnly Property [Date]() As DateTime

        '*********************************************************************
        '
        ' DateFormatString Property
        '
        ' Format string for displaying date.
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
        ' Display content by retrieving content from context
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            ' Write date
            writer.Write(String.Format(_dateFormatString, [Date].ToLocalTime()))
            If _displayTimeZone Then
                writer.Write(String.Format(" ({0})", LocalTimeZoneAbbreviation))
            End If
        End Sub 'RenderContents 
    End Class 'DisplayDate 
End Namespace