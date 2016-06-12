Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Data
Imports System.Diagnostics
Imports System.Web.UI
Imports System.Web.UI.Design
Imports System.Web.UI.WebControls
Imports System.IO

Public Class CustomDesigner
    Inherits ControlDesigner

    Public Overrides ReadOnly Property AllowResize() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Function GetDesignTimeHtml() As String
        Dim control As WebControl = CType(Component, WebControl)

        Dim sw As New StringWriter
        Dim writer As New HtmlTextWriter(sw)
        writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
        writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "black")
        writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
        writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "GainsBoro")
        writer.AddStyleAttribute(HtmlTextWriterStyle.Width, control.Width.ToString())
        writer.AddStyleAttribute(HtmlTextWriterStyle.Height, control.Height.ToString())
        writer.RenderBeginTag(HtmlTextWriterTag.Table)
        writer.RenderBeginTag(HtmlTextWriterTag.Tr)
        writer.RenderBeginTag(HtmlTextWriterTag.Td)
        writer.Write(control.GetType().Name)
        writer.RenderEndTag()
        writer.RenderEndTag()
        writer.RenderEndTag()

        Return sw.ToString()
    End Function 'GetDesignTimeHtml

    Protected Overrides Function GetErrorDesignTimeHtml(ByVal e As Exception) As String
        Return CreatePlaceHolderDesignTimeHtml(("error:" + e.Message + e.StackTrace))
    End Function 'GetErrorDesignTimeHtml
End Class