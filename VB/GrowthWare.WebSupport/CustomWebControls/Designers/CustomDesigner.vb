Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Diagnostics
Imports System.Web.UI
Imports System.Web.UI.Design
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Globalization

Namespace CustomWebControls.Designers
    ''' <summary>
    ''' Class CustomDesigner
    ''' </summary>
    Public Class CustomDesigner
        Inherits ControlDesigner

        ''' <summary>
        ''' Gets a value indicating whether the control can be resized in the design-time environment.
        ''' </summary>
        ''' <value><c>true</c> if [allow resize]; otherwise, <c>false</c>.</value>
        ''' <returns>true, if the control can be resized; otherwise, false.</returns>
        Public Overrides ReadOnly Property AllowResize() As Boolean
            Get
                Return True
            End Get
        End Property

        ''' <summary>
        ''' Retrieves the HTML markup that is used to represent the control at design time.
        ''' </summary>
        ''' <returns>The HTML markup used to represent the control at design time.</returns>
        ''' <remarks></remarks>
        Public Overrides Function GetDesignTimeHtml() As String
            Dim mControl As WebControl = CType(Component, WebControl)
            Dim mRetVal As String = String.Empty
            Using mStringWriter = New StringWriter(CultureInfo.InvariantCulture)
                Dim mHtmlWriter As New HtmlTextWriter(mStringWriter)
                mHtmlWriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                mHtmlWriter.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "black")
                mHtmlWriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                mHtmlWriter.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "GainsBoro")
                mHtmlWriter.AddStyleAttribute(HtmlTextWriterStyle.Width, mControl.Width.ToString())
                mHtmlWriter.AddStyleAttribute(HtmlTextWriterStyle.Height, mControl.Height.ToString())
                mHtmlWriter.RenderBeginTag(HtmlTextWriterTag.Table)
                mHtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr)
                mHtmlWriter.RenderBeginTag(HtmlTextWriterTag.Td)
                mHtmlWriter.Write(mControl.GetType().Name)
                mHtmlWriter.RenderEndTag()
                mHtmlWriter.RenderEndTag()
                mHtmlWriter.RenderEndTag()
                mRetVal = mStringWriter.ToString()
            End Using
            Return mRetVal
        End Function

        ''' <summary>
        ''' Retrieves the HTML markup that provides information about the specified exception.
        ''' </summary>
        ''' <param name="e">The exception that occurred.</param>
        ''' <returns>The design-time HTML markup for the specified exception.</returns>
        Protected Overrides Function GetErrorDesignTimeHtml(ByVal e As Exception) As String
            If Not e Is Nothing Then
                Return CreatePlaceHolderDesignTimeHtml("error:" + e.Message + e.StackTrace)
            Else
                Return CreatePlaceHolderDesignTimeHtml("Error getting the design time HTML.")
            End If
        End Function
    End Class
End Namespace
