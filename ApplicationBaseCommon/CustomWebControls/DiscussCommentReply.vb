Imports System
Imports System.Web.UI
Imports ApplicationBase.Model
Imports ApplicationBase.Model.Discuss
Imports ApplicationBase.Model.Accounts.Security
Imports ApplicationBase.Model.Modules

Namespace CustomWebControls
    '*********************************************************************
    '
    ' DiscussCommentReply
    '
    ' We need to override the base CommentReply class to prevent users from
    ' replying to announcements.
    '
    '*********************************************************************

    Public Class DiscussCommentReply
        Inherits CommentReply

        '*********************************************************************
        '
        ' Render Method
        '
        ' Here's where we check add permissions
        '
        '*********************************************************************
        Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
            'Dim objUserInfo As UserInfo = CType(Context.Items("UserInfo"), UserInfo)
            'Dim objSectionInfo As SectionInfo = CType(Context.Items("SectionInfo"), SectionInfo)
            'Dim objPostInfo As PostInfo = CType(Context.Items("ContentInfo"), PostInfo)
            'If objUserInfo.MayComment OrElse Array.IndexOf(objSectionInfo.CommentRoles, "Community-Authenticated") <> -1 Then
            '    If objPostInfo.IsLocked = False Then
            '        MyBase.Render(writer)
            '    End If
            'End If

            Dim objModuleProfileInfo As MModuleProfileInfo = CType(Context.Items("ModuleProfileInfo"), MModuleProfileInfo)
            Dim objSecurityProfile As MAccountSecurityInfo = New MAccountSecurityInfo(objModuleProfileInfo)
            Dim objPostInfo As PostInfo = CType(Context.Items("ContentInfo"), PostInfo)
            If objSecurityProfile.MayEdit OrElse Array.IndexOf(objModuleProfileInfo.EditRoles, "Authenticated") <> -1 Then
                If objPostInfo.IsLocked = False Then
                    MyBase.Render(writer)
                End If
            End If
        End Sub 'Render 
    End Class 'DiscussCommentReply 
End Namespace
