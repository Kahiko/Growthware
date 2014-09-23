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
    Public Class CustomDesigner
        Inherits ControlDesigner

        Public Overrides ReadOnly Property AllowResize() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides Function GetDesignTimeHtml() As String
            Dim mControl As WebControl = CType(Component, WebControl)

            Dim mStringWriter As New StringWriter(CultureInfo.InvariantCulture)
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

            Return mStringWriter.ToString()
        End Function 'GetDesignTimeHtml

        Protected Overrides Function GetErrorDesignTimeHtml(ByVal e As Exception) As String
            Return CreatePlaceHolderDesignTimeHtml(("error:" + e.Message + e.StackTrace))
        End Function 'GetErrorDesignTimeHtml
    End Class
End Namespace
