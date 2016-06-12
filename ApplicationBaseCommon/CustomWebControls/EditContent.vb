Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.Accounts.Security
Imports ApplicationBase.Model.Modules

Namespace CustomWebControls
    '*********************************************************************
    '
    ' EditContent Class
    '
    ' WebControl that displays different actions that can be taken
    ' with a content page such as adding, deleting, moving, and editing.
    ' This control checks the current users permissions before
    ' displaying options.
    '
    '*********************************************************************

    Public Class EditContent
        Inherits WebControl

        Private _addText As String = String.Empty
        Private _editText As String = String.Empty
        Private _deleteText As String = String.Empty
        Private _moveText As String = String.Empty
        Private _commentText As String = String.Empty
        Private _moderateText As String = String.Empty
        Private _rateText As String = String.Empty

        Private _addUrl As String
        Private _editUrl As String
        Private _deleteUrl As String
        Private _moveUrl As String
        Private _commentUrl As String
        Private _moderateUrl As String
        Private _rateUrl As String



        '*********************************************************************
        '
        ' EditContent Constructor
        '
        ' Creates a WebControl with a table tag as its containing
        ' tag.
        '
        '*********************************************************************
        Public Sub New()
            MyBase.New(HtmlTextWriterTag.Table)
        End Sub 'New 

        '*********************************************************************
        '
        ' AddText Property
        '
        ' The text displayed for adding new content.
        '
        '*********************************************************************

        Public WriteOnly Property AddText() As String
            Set(ByVal value As String)
                _addText = value
            End Set
        End Property

        '*********************************************************************
        '
        ' EditText Property
        '
        ' The text displayed for editing content.
        '
        '*********************************************************************

        Public WriteOnly Property EditText() As String
            Set(ByVal value As String)
                _editText = value
            End Set
        End Property

        '*********************************************************************
        '
        ' DeleteText Property
        '
        ' The text displayed for deleting content.
        '
        '*********************************************************************

        Public WriteOnly Property DeleteText() As String
            Set(ByVal value As String)
                _deleteText = value
            End Set
        End Property


        '*********************************************************************
        '
        ' MoveText Property
        '
        ' The text displayed for moving content.
        '
        '*********************************************************************

        Public WriteOnly Property MoveText() As String
            Set(ByVal value As String)
                _moveText = value
            End Set
        End Property



        '*********************************************************************
        '
        ' ModerateText Property
        '
        ' The text displayed for moderating content.
        '
        '*********************************************************************

        Public WriteOnly Property ModerateText() As String
            Set(ByVal value As String)
                _moderateText = value
            End Set
        End Property

        '*********************************************************************
        '
        ' RateText Property
        '
        ' The text displayed for rating content.
        '
        '*********************************************************************

        Public WriteOnly Property RateText() As String
            Set(ByVal value As String)
                _rateText = value
            End Set
        End Property

        '*********************************************************************
        '
        ' CommentText Property
        '
        ' The text displayed for commenting on content.
        '
        '*********************************************************************

        Public WriteOnly Property CommentText() As String
            Set(ByVal value As String)
                _commentText = value
            End Set
        End Property

        '*********************************************************************
        '
        ' AddUrl Property
        '
        ' The URL for adding new content.
        '
        '*********************************************************************

        Public Property AddUrl() As String
            Get
                Return _addUrl
            End Get
            Set(ByVal value As String)
                _addUrl = value
            End Set
        End Property


        '*********************************************************************
        '
        ' EditUrl Property
        '
        ' The URL for editing content.
        '
        '*********************************************************************

        Public Property EditUrl() As String
            Get
                Return _editUrl
            End Get
            Set(ByVal value As String)
                _editUrl = value
            End Set
        End Property


        '*********************************************************************
        '
        ' DeleteUrl Property
        '
        ' The URL for deleting content.
        '
        '*********************************************************************

        Public Property DeleteUrl() As String
            Get
                Return _deleteUrl
            End Get
            Set(ByVal value As String)
                _deleteUrl = value
            End Set
        End Property



        '*********************************************************************
        '
        ' MoveUrl Property
        '
        ' The URL for moving content.
        '
        '*********************************************************************

        Public Property MoveUrl() As String
            Get
                Return _moveUrl
            End Get
            Set(ByVal value As String)
                _moveUrl = value
            End Set
        End Property



        '*********************************************************************
        '
        ' ModerateUrl Property
        '
        ' The URL for moderating content.
        '
        '*********************************************************************

        Public Property ModerateUrl() As String
            Get
                Return _moderateUrl
            End Get
            Set(ByVal value As String)
                _moderateUrl = value
            End Set
        End Property


        '*********************************************************************
        '
        ' RateUrl Property
        '
        ' The URL for rating content.
        '
        '*********************************************************************

        Public Property RateUrl() As String
            Get
                Return _rateUrl
            End Get
            Set(ByVal value As String)
                _rateUrl = value
            End Set
        End Property


        '*********************************************************************
        '
        ' CommentUrl Property
        '
        ' The URL for commenting on content.
        '
        '*********************************************************************

        Public Property CommentUrl() As String
            Get
                Return _commentUrl
            End Get
            Set(ByVal value As String)
                _commentUrl = value
            End Set
        End Property




        '*********************************************************************
        '
        ' CreateChildControls Method
        '
        ' Adds the links to this control Controls collection.
        '
        '*********************************************************************
        Protected Overrides Sub CreateChildControls()
            Dim lnk As HyperLink
            ' needs lots of help
            ' Get UserInfo
            'Dim objUserInfo As MAccountProfileInfo = CType(Context.Items("UserInfo"), UserInfo)

            ' Get SectionInfo
            'Dim objSectionInfo As SectionInfo = CType(Context.Items("SectionInfo"), SectionInfo)

            Dim objModuleProfileInfo As MModuleProfileInfo = CType(Context.Items("ModuleProfileInfo"), MModuleProfileInfo)
            Dim objAccountSecurityInfo As MAccountSecurityInfo = New MAccountSecurityInfo(objModuleProfileInfo)

            ' Add Link
            If (objAccountSecurityInfo.MayAdd OrElse Array.IndexOf(objModuleProfileInfo.AddRoles, "Authenticated") <> -1) AndAlso _addText <> String.Empty Then
                lnk = New HyperLink()
                lnk.Text = _addText
                lnk.NavigateUrl = CalculatePath(_addUrl)
                Controls.Add(lnk)
            End If

            ' Edit Link
            If (objAccountSecurityInfo.MayEdit OrElse Array.IndexOf(objModuleProfileInfo.EditRoles, "Authenticated") <> -1) AndAlso _editText <> String.Empty Then
                lnk = New HyperLink()
                lnk.Text = _editText
                lnk.NavigateUrl = CalculatePath(_editUrl)
                Controls.Add(lnk)
            End If

            ' Delete Link
            If (objAccountSecurityInfo.MayDelete OrElse Array.IndexOf(objModuleProfileInfo.DeleteRoles, "Authenticated") <> -1) AndAlso _deleteText <> String.Empty Then
                lnk = New HyperLink()
                lnk.Text = _deleteText
                lnk.NavigateUrl = CalculatePath(_deleteUrl)
                Controls.Add(lnk)
            End If

            ' Move Link
            'If (objAccountSecurityInfo.MayModerate OrElse Array.IndexOf(objModuleProfileInfo.ModerateRoles, "Authenticated") <> -1 OrElse (objUserInfo.MayEdit OrElse Array.IndexOf(objSectionInfo.EditRoles, "Authenticated") <> -1)) AndAlso _moveText <> String.Empty Then
            '    lnk = New HyperLink()
            '    lnk.Text = _moveText
            '    lnk.NavigateUrl = CalculatePath(_moveUrl)
            '    Controls.Add(lnk)
            'End If
            lnk = New HyperLink()
            lnk.Text = _moveText
            lnk.NavigateUrl = CalculatePath(_moveUrl)
            Controls.Add(lnk)

            ' Comment Link
            'If objSectionInfo.EnableComments Then
            '    If (objAccountSecurityInfo.MayComment OrElse Array.IndexOf(objModuleProfileInfo.CommentRoles, "Authenticated") <> -1) AndAlso _commentText <> String.Empty Then
            '        lnk = New HyperLink()
            '        lnk.Text = _commentText
            '        lnk.NavigateUrl = CalculatePath(_commentUrl)
            '        Controls.Add(lnk)
            '    End If
            'End If
            lnk = New HyperLink()
            lnk.Text = _commentText
            lnk.NavigateUrl = CalculatePath(_commentUrl)
            Controls.Add(lnk)

            ' Moderate Link
            'If objSectionInfo.EnableModeration Then
            '    If (objAccountSecurityInfo.MayModerate OrElse Array.IndexOf(objModuleProfileInfo.ModerateRoles, "Authenticated") <> -1) AndAlso _moderateText <> String.Empty Then
            '        lnk = New HyperLink()
            '        lnk.Text = _moderateText
            '        lnk.NavigateUrl = CalculatePath(_moderateUrl)
            '        Controls.Add(lnk)
            '    End If
            'End If
            lnk = New HyperLink()
            lnk.Text = _moderateText
            lnk.NavigateUrl = CalculatePath(_moderateUrl)
            Controls.Add(lnk)

            ' Rate Link
            'If objSectionInfo.EnableRatings Then
            '    If (objAccountSecurityInfo.MayRate OrElse Array.IndexOf(objModuleProfileInfo.RateRoles, "Authenticated") <> -1) AndAlso _rateText <> String.Empty Then
            '        lnk = New HyperLink()
            '        lnk.Text = _rateText
            '        lnk.NavigateUrl = CalculatePath(_rateUrl)
            '        Controls.Add(lnk)
            '    End If
            'End If
            lnk = New HyperLink()
            lnk.Text = _rateText
            lnk.NavigateUrl = CalculatePath(_rateUrl)
            Controls.Add(lnk)
        End Sub 'CreateChildControls

        Private Function CalculatePath(ByVal Action As String) As String
            CalculatePath = BaseSettings.FQDNPage & "?Action=" & Action & BaseSettings.getURL
        End Function


        '*********************************************************************
        '
        ' Render Method
        '
        ' Renders links to the browser by iterating through
        ' the Controls collection.
        '
        '*********************************************************************
        Protected Overrides Sub Render(ByVal tw As HtmlTextWriter)
            If Controls.Count > 0 Then
                tw.RenderBeginTag(HtmlTextWriterTag.Table)
                tw.RenderBeginTag(HtmlTextWriterTag.Tr)

                Dim controlCount As Integer = Controls.Count
                Dim counter As Integer = 0

                Dim control As Control
                For Each control In Controls
                    counter += 1
                    tw.RenderBeginTag(HtmlTextWriterTag.Td)
                    control.RenderControl(tw)
                    tw.RenderEndTag() ' close td
                    ' Render Separator
                    If counter < controlCount Then
                        tw.RenderBeginTag(HtmlTextWriterTag.Td)
                        tw.Write("|")
                        tw.RenderEndTag() ' close td
                    End If
                Next control
                tw.RenderEndTag() ' close tr
                tw.RenderEndTag() ' close table
            End If
        End Sub 'Render
    End Class 'EditContent
End Namespace