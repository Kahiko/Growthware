Imports ApplicationBase.ClientChoices
Imports ApplicationBase.Common.CustomWebControls
Imports ApplicationBase.Model
Imports ApplicationBase.Model.Discuss
Imports System.Data
Imports System.Data.SqlClient

Partial Public Class DiscussionDetails
    Inherits ClientChoicesUserControl

    Private _headerTemplate As String = "Comments_DisplayComments-HeaderTemplate"
    Private _itemTemplate As String = "Comments_DisplayComments-ItemTemplate"
    Private _separatorTemplate As String = "Comments_DisplayComments-SeparatorTemplate"
    Private _footerTemplate As String = "Comments_DisplayComments-FooterTemplate"

    Private pnlComments As Panel
    Private dropCommentView As DropDownList
    Private dropOrderBy As DropDownList
    Private ctlCommentList As ContentList

    Private commentView As Integer
    Private colComments As CommentCollection
    Private objContentInfo As ContentInfo

    '*********************************************************************
    '
    ' ContentPageID Property
    '
    ' Represents the Content Page ID of the resource being commented on.
    '
    '*********************************************************************

    Public Property ContentPageID() As Integer
        Get
            If ViewState("ContentPageID") Is Nothing Then
                Return -1
            Else
                Return Fix(ViewState("ContentPageID"))
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("ContentPageID") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        pnlComments = Comments_DisplayComments1.FindControl("pnlComments")
        dropCommentView = Comments_DisplayComments1.FindControl("CommentView")
        dropOrderBy = Comments_DisplayComments1.FindControl("OrderBy")
        ctlCommentList = Comments_DisplayComments1.FindControl("CommentList")

        ' Get ContentInfo
        If Not (Context Is Nothing) Then
            ContentPageID = Request.QueryString("ID")
            Dim _contentInfo As ContentInfo = GetPostInfo("Anonymous", ContentPageID)
            Context.Items("ContentInfo") = _contentInfo
            objContentInfo = _contentInfo
        End If

    End Sub
    '*********************************************************************
    '
    ' InitializeHandlers Method
    '
    ' Assigns ItemDataBound handlers to the Repeater depending on
    ' the view of the comments (threaded, nested, and so on).
    '
    '*********************************************************************
    Private Sub InitializeHandlers()
        ' Display Comments
        Select Case commentView
            Case 1
                AddHandler ctlCommentList.ItemDataBound, AddressOf Repeater_ItemDataBoundNested
            Case 2
                AddHandler ctlCommentList.ItemDataBound, AddressOf Repeater_ItemDataBoundThreaded
            Case 3
                AddHandler ctlCommentList.ItemDataBound, AddressOf Repeater_ItemDataBoundNested
            Case Else
                AddHandler ctlCommentList.ItemDataBound, AddressOf Repeater_ItemDataBoundFlat
        End Select
    End Sub
    '*********************************************************************
    '
    ' InitializeTemplates Method
    '
    ' Assigns templates to the Repeater depending on
    ' the view of the comments (threaded, nested, and so on).
    '
    '*********************************************************************
    Private Sub InitializeTemplates()

        ' Display Comments
        Select Case commentView
            Case 1
                ctlCommentList.HeaderTemplate = LoadTemplate((_headerTemplate + "-Nested.ascx"))
                ctlCommentList.ItemTemplate = LoadTemplate((_itemTemplate + "-Nested.ascx"))
                ctlCommentList.SeparatorTemplate = LoadTemplate((_separatorTemplate + "-Nested.ascx"))
                ctlCommentList.FooterTemplate = LoadTemplate((_footerTemplate + "-Nested.ascx"))
            Case 2
                ctlCommentList.HeaderTemplate = LoadTemplate((_headerTemplate + "-Threaded.ascx"))
                ctlCommentList.ItemTemplate = LoadTemplate((_itemTemplate + "-Threaded.ascx"))
                ctlCommentList.SeparatorTemplate = LoadTemplate((_separatorTemplate + "-Threaded.ascx"))
                ctlCommentList.FooterTemplate = LoadTemplate((_footerTemplate + "-Threaded.ascx"))
            Case 3
                ctlCommentList.HeaderTemplate = LoadTemplate((_headerTemplate + "-Embedded.ascx"))
                ctlCommentList.ItemTemplate = LoadTemplate((_itemTemplate + "-Embedded.ascx"))
                ctlCommentList.SeparatorTemplate = LoadTemplate((_separatorTemplate + "-Embedded.ascx"))
                ctlCommentList.FooterTemplate = LoadTemplate((_footerTemplate + "-Embedded.ascx"))
            Case Else
                ctlCommentList.HeaderTemplate = LoadTemplate((_headerTemplate + "-Flat.ascx"))
                ctlCommentList.ItemTemplate = LoadTemplate((_itemTemplate + "-Flat.ascx"))
                ctlCommentList.SeparatorTemplate = LoadTemplate((_separatorTemplate + "-Flat.ascx"))
                ctlCommentList.FooterTemplate = LoadTemplate((_footerTemplate + "-Flat.ascx"))
        End Select
    End Sub 'InitializeTemplates

    '*********************************************************************
    '
    ' BindComments Method
    '
    ' Retrieves and displays the comments.
    '
    '*********************************************************************
    Private Sub BindComments()
        ' Get Comments
        'colComments = GetComments(objUserInfo.Username, ContentPageID, Int32.Parse(dropOrderBy.SelectedItem.Value))
        colComments = GetComments("Anonymous", ContentPageID, Int32.Parse(dropOrderBy.SelectedItem.Value))
        'Discuss.DiscussUtility.GetPosts("Anonymous", 9, 10, 1, "Date Commented DESC")
        If colComments.Count > 0 Then
            ' Show Comments Panel
            pnlComments.Visible = True
            'Comments_DisplayComments1
            If Int32.Parse(dropCommentView.SelectedItem.Value) = 0 Then
                ctlCommentList.DataSource = colComments
            Else
                ctlCommentList.DataSource = colComments.GetChildren(ContentPageID)
            End If
            Me.DataBind()

        Else
            pnlComments.Visible = False
        End If
    End Sub 'BindComments


    '*********************************************************************
    '
    ' Repeater_ItemDataBoundFlat Method
    '
    ' Displays comments using the flat view.
    '
    '*********************************************************************
    Private Sub Repeater_ItemDataBoundFlat(ByVal s As [Object], ByVal e As ContentListItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
        End If
    End Sub 'Repeater_ItemDataBoundFlat




    '*********************************************************************
    '
    ' Repeater_ItemDataBoundThreaded Method
    '
    ' Displays comments using the threaded view.
    '
    '*********************************************************************
    Private Sub Repeater_ItemDataBoundThreaded(ByVal s As [Object], ByVal e As ContentListItemEventArgs)
        Dim nestedComments As ContentList
        Dim colNestedComments As CommentCollection
        Dim commentID As Integer

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            ' Display Nested Comments
            commentID = CType(e.Item.DataItem, CommentInfo).ContentPageID
            colNestedComments = colComments.GetChildren(commentID)

            ' Bind to Nested Repeater
            If colNestedComments.Count > 0 Then
                nestedComments = CType(e.Item.Controls(0).FindControl("ThreadedComments"), ContentList)
                nestedComments.ItemTemplate = LoadTemplate("Comments_DisplayComments-ItemTemplate-Threaded.ascx")
                AddHandler nestedComments.ItemDataBound, AddressOf Repeater_ItemDataBoundThreaded
                nestedComments.SeparatorTemplate = LoadTemplate("Comments_DisplayComments-SeparatorTemplate-Flat.ascx")
                nestedComments.DataSource = colNestedComments
                nestedComments.DataBind()
            End If
        End If
    End Sub 'Repeater_ItemDataBoundThreaded



    '*********************************************************************
    '
    ' Repeater_ItemDataBoundNested Method
    '
    ' Displays comments using the nested view.
    '
    '*********************************************************************
    Private Sub Repeater_ItemDataBoundNested(ByVal s As [Object], ByVal e As ContentListItemEventArgs)
        Dim nestedComments As ContentList
        Dim colNestedComments As CommentCollection
        Dim commentID As Integer


        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            ' Display Nested Comments
            commentID = CType(e.Item.DataItem, CommentInfo).ContentPageID
            colNestedComments = colComments.GetChildren(commentID)

            ' Bind to Nested Repeater
            If colNestedComments.Count > 0 Then
                nestedComments = CType(e.Item.Controls(0).FindControl("NestedComments"), ContentList)
                nestedComments.ItemTemplate = LoadTemplate((_itemTemplate + "-Nested.ascx"))
                AddHandler nestedComments.ItemDataBound, AddressOf Repeater_ItemDataBoundNested
                nestedComments.SeparatorTemplate = LoadTemplate("Comments_DisplayComments-SeparatorTemplate-Flat.ascx")
                nestedComments.DataSource = colNestedComments
                nestedComments.DataBind()
            End If
        End If
    End Sub 'Repeater_ItemDataBoundNested


    Public Shared Function GetComments(ByVal username As String, ByVal contentPageID As Integer, ByVal orderBy As Integer) As CommentCollection
        Dim conPortal As New SqlConnection("Server=(local);Trusted_Connection=true;database=CommunityStarterKit")
        Dim cmdGet As New SqlCommand("Community_CommentsGetComments", conPortal)
        cmdGet.CommandType = CommandType.StoredProcedure

        ' Add Parameters
        cmdGet.Parameters.AddWithValue("@communityID", 1)
        cmdGet.Parameters.AddWithValue("@username", username)
        cmdGet.Parameters.AddWithValue("@contentPageID", contentPageID)
        cmdGet.Parameters.AddWithValue("@orderBy", orderBy)

        Dim colComments As New CommentCollection()

        conPortal.Open()
        Dim dr As SqlDataReader = cmdGet.ExecuteReader()
        While dr.Read()
            colComments.Add(New CommentInfo(dr))
        End While
        conPortal.Close()
        Return colComments
    End Function 'GetComments


    Public Shared Function GetPostInfo(ByVal username As String, ByVal contentPageID As Integer) As ContentInfo
        ' needs help
        Dim _postInfo As PostInfo = Nothing

        Dim conPortal As New SqlConnection("Server=(local);Trusted_Connection=true;database=CommunityStarterKit")
        Dim cmdGet As New SqlCommand("Community_DiscussGetPost", conPortal)
        cmdGet.CommandType = CommandType.StoredProcedure
        cmdGet.Parameters.AddWithValue("@communityID", 1)
        cmdGet.Parameters.AddWithValue("@username", username)
        cmdGet.Parameters.AddWithValue("@contentPageID", contentPageID)
        conPortal.Open()
        Dim dr As SqlDataReader = cmdGet.ExecuteReader()
        If dr.Read() Then
            _postInfo = New PostInfo(dr)
        End If
        conPortal.Close()
        Return CType(_postInfo, ContentInfo)
    End Function 'GetPostInfo

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Visible AndAlso ContentPageID <> -1 Then
            EnsureChildControls()

            ' determine comment view
            If Not Page.IsPostBack AndAlso Not (Context.Session("CommentView") Is Nothing) Then
                commentView = Fix(Context.Session("CommentView"))
                dropCommentView.SelectedIndex = -1
                Dim item As ListItem = dropCommentView.Items.FindByValue(commentView.ToString())
                If Not (item Is Nothing) Then
                    item.Selected = True
                End If
            Else
                'commentView = Int32.Parse(dropCommentView.SelectedItem.Value)

                commentView = Comments_DisplayComments1.dropCommentViewSelectedItemValue
            End If

            InitializeHandlers()
            InitializeTemplates()
            BindComments()
        End If

    End Sub
End Class