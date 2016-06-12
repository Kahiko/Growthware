Public Partial Class Comments_DisplayComments
    Inherits System.Web.UI.UserControl

    ''pnlCommentsVisible
    'Public Property pnlCommentsVisible() As Boolean
    '    Get
    '        Return pnlComments.Visible
    '    End Get
    '    Set(ByVal value As Boolean)
    '        pnlComments.Visible = value
    '    End Set
    'End Property

    'Public Property dropCommentViewSelectedIndex() As Integer
    '    Get
    '        Return CommentView.SelectedIndex
    '    End Get
    '    Set(ByVal value As Integer)
    '        CommentView.SelectedIndex = value
    '    End Set
    'End Property

    Public Property dropCommentViewSelectedItemValue() As Integer
        Get
            Return CommentView.SelectedItem.Value
        End Get
        Set(ByVal value As Integer)
            CommentView.SelectedItem.Value = value
        End Set
    End Property

    'Public ReadOnly Property dropCommentViewItemsFindByValue(ByVal commentViewString As String) As ListItem
    '    Get
    '        Return CommentView.Items.FindByValue(commentViewString)
    '    End Get
    'End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
End Class