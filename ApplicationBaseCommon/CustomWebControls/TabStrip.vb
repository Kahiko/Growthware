Imports System
Imports System.Collections.Generic
Imports System.Collections
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace CustomWebControls
    ' replace the frist line with the next two lines when converting to framework 2.0


    '<ControlBuilderAttribute(GetType(TabStripControlBuilder)), ParseChildrenAttribute(False)> _
    <Designer(GetType(CustomDesigner))> _
    <ControlBuilderAttribute(GetType(TabStripControlBuilder)), ParseChildrenAttribute(False)> _
    <DefaultProperty("Text"), ToolboxData("<{0}:TabStrip runat=server />")> _
    Public Class TabStrip
        Inherits CompositeControl
        'Inherits WebControl
        ' replase inherits with next line for conversion to framework 2.0

        <Bindable(True), Category("Appearance"), DefaultValue(""), Localizable(True)> _
        Property Text() As String
            Get
                Dim s As String = CStr(ViewState("Text"))
                If s Is Nothing Then
                    Return String.Empty
                Else
                    Return s
                End If
            End Get

            Set(ByVal Value As String)
                ViewState("Text") = Value
            End Set
        End Property

        Private _isUplevel As Boolean
        Private _tabs As New ArrayList

        Public ReadOnly Property Tabs(ByVal TabID As String) As Tab
            Get
                Dim x As Integer = 0
                For x = 0 To _tabs.Count - 1
                    Dim myTab As Tab = _tabs(x)
                    If Not myTab Is Nothing Then
                        If Not myTab.ID Is Nothing Then
                            If myTab.ID.Trim.ToLower = TabID.Trim.ToLower Then
                                Return myTab
                            End If
                        End If
                    End If
                Next
                Return Nothing
            End Get
        End Property

        '*********************************************************************
        '
        ' TabStrip Constructor
        '
        '*********************************************************************
        Public Sub New()

        End Sub       'New

        '*********************************************************************
        '
        ' AddParsedSubObject Method
        '
        ' Only add tabs to the Tabs collection.
        '
        '*********************************************************************
        Protected Overrides Sub AddParsedSubObject(ByVal obj As [Object])
            If TypeOf obj Is Tab Then
                _tabs.Add(obj)
            End If
        End Sub       'AddParsedSubObject

        '*********************************************************************
        '
        ' RenderContents Method
        '
        ' Display tabstrip for uplevel browsers.
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            ' Display the tabs
            Dim i As Integer
            Dim strOut As String
            strOut = vbCrLf & "<table cellspacing='0' cellpadding='0' border='0'>" & vbCrLf
            strOut += "     <tr>" & vbCrLf
            writer.Write(strOut)
            For i = 0 To _tabs.Count - 1
                Dim objTab As Tab = CType(_tabs(i), Tab)
                writer.Write(strOut)
                If i < _tabs.Count - 1 Then
                    strOut = "          <td width='1px'>" & vbCrLf
                    strOut += "" & vbCrLf
                    strOut += "          </td>" & vbCrLf
                    writer.Write(strOut)
                End If
            Next i
            strOut = "     </tr>" & vbCrLf
            strOut += "     <tr>" & vbCrLf
            strOut += "			<td>" & vbCrLf
            strOut += "				<div id=""tabStrip"">" & vbCrLf
            strOut += "					<ul id=""tabStripUL"">" & vbCrLf
            writer.Write(strOut)
            For i = 0 To _tabs.Count - 1
                Dim objTab As Tab = CType(_tabs(i), Tab)
                If objTab.Roles Is Nothing Then             ' if there is no roles then just add the tabs li
                    strOut = "						<li id=""" & objTab.Text & "Tab"">" & vbCrLf
                    writer.Write(strOut)
                    'objTab.ClientID
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, String.Format("javascript:showPanel('{0}')", Me.ClientID.Replace(Me.ID, objTab.PanelID)))
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, String.Format("{0}", Me.ClientID.Replace(Me.ID, objTab.PanelID) & "Anchor"))
                    writer.RenderBeginTag(HtmlTextWriterTag.A)
                    writer.Write(objTab.Text)
                    writer.RenderEndTag()
                    strOut = vbCrLf & "				</li>" & vbCrLf
                    writer.Write(strOut)
                Else                  ' since there are roles then check the roles and add the tab li if necessary
                    Dim role As String
                    For Each role In objTab.Roles.Split(New Char() {";"c})
                        If role <> "" Then
                            If HttpContext.Current.User.IsInRole(role) Then
                                strOut = "						<li id=""" & objTab.Text & "Tab"">" & vbCrLf
                                writer.Write(strOut)
                                writer.AddAttribute(HtmlTextWriterAttribute.Href, String.Format("javascript:showPanel('{0}')", Me.ClientID.Replace(Me.ID, objTab.PanelID)))
                                writer.AddAttribute(HtmlTextWriterAttribute.Id, String.Format("{0}", Me.ClientID.Replace(Me.ID, objTab.PanelID) & "Anchor"))
                                writer.RenderBeginTag(HtmlTextWriterTag.A)
                                writer.Write(objTab.Text)
                                writer.RenderEndTag()
                                strOut = vbCrLf & "				</li>" & vbCrLf
                                writer.Write(strOut)
                                Exit For
                            End If
                        End If
                    Next role
                End If
            Next i
            strOut = "					</ul>" & vbCrLf
            strOut += "					</div>" & vbCrLf
            strOut += "			</td>" & vbCrLf
            strOut += "     </tr>" & vbCrLf
            strOut += "</table>" & vbCrLf
            writer.Write(strOut)
        End Sub       'RenderContents 
    End Class   'TabStrip 

    '*********************************************************************
    '
    ' Tab Strip Control Builder Class
    '
    ' Only parse tabs in the tab strip.
    '
    '*********************************************************************
    <Designer(GetType(CustomDesigner))> _
    Public Class TabStripControlBuilder
        Inherits ControlBuilder

        Public Overrides Function GetChildControlType(ByVal tagName As String, ByVal attributes As IDictionary) As Type

            If String.Compare(tagName, "tab", True) = 0 Then
                Return GetType(Tab)
            End If

            Return Nothing
        End Function    'GetChildControlType
    End Class 'TabStripControlBuilder

    '*********************************************************************
    '
    ' Tab Class
    '
    ' Represents individual tabs in the tab strip.
    '
    '*********************************************************************
    <Designer(GetType(CustomDesigner))> _
    Public Class Tab
        Inherits CompositeControl
        'Inherits Control
        ' replase inherits with next line for converstion to framework 2.0

        Private _text As String
        Private _roles As String
        Private _panelID As String

        Public Property [Text]() As String
            Get
                Return _text
            End Get
            Set(ByVal Value As String)
                _text = Value
            End Set
        End Property

        Public Property Roles() As String
            Get
                Return _roles
            End Get
            Set(ByVal Value As String)
                _roles = Value
            End Set
        End Property

        Public Property PanelID() As String
            Get
                Return _panelID
            End Get
            Set(ByVal Value As String)
                _panelID = Value
            End Set
        End Property
    End Class
End Namespace