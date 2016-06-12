Imports System
Imports System.ComponentModel

Namespace CustomWebControls
    <Designer(GetType(CustomWebControls.CustomDesigner))> _
    Public Class DiscussEditContent
        Inherits EditContent

        Public Sub New()
            If Not (Context Is Nothing) Then
                ' needs help
                'Dim _pageInfo As PageInfo = CType(Context.Items("PageInfo"), PageInfo)
                'Dim contentPageID As Integer = _pageInfo.ID

                Dim contentPageID As Integer = 1
                AddUrl = "Discuss_AddPost.aspx"
                EditUrl = String.Format("Discuss_EditPost.aspx?id={0}", contentPageID)
                DeleteUrl = String.Format("ContentPages_DeleteContentPage.aspx?id={0}", contentPageID)
                MoveUrl = String.Format("ContentPages_MoveContentPage.aspx?id={0}", contentPageID)
                CommentUrl = String.Format("Comments_AddComment.aspx?id={0}", contentPageID)
                ModerateUrl = "Moderation_ModerateSection.aspx"
            End If
        End Sub 'New
    End Class 'DiscussEditContent
End Namespace