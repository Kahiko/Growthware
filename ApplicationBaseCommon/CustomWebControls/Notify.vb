Imports System
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports ApplicationBase.Model.Modules
Imports ApplicationBase.Model.Accounts

Namespace CustomWebControls
    '*********************************************************************
    '
    ' Notify Class
    '
    ' WebControl that displays a checkbox indicating whether or not
    ' user has requested a notification email when new items are listed. 
    '
    '*********************************************************************

    Public Class Notify
        Inherits CheckBox

        Private objModuleProfileInfo As MModuleProfileInfo
        'Private objPageInfo As PageInfo
        Private objAccountProfileInfo As MAccountProfileInfo

        '*********************************************************************
        '
        ' Notify Constructor
        '
        ' Retrieves the UserInfo, SectionInfo, and PageInfo objects
        ' from context. This control hides itself if notification
        ' is not enabled for this section.
        '
        '*********************************************************************
        Public Sub New()
            If Not (Context Is Nothing) Then
                objModuleProfileInfo = CType(Context.Items("ModuleProfileInfo"), MModuleProfileInfo)
                'objPageInfo = CType(Context.Items("PageInfo"), PageInfo)
                objAccountProfileInfo = CType(Context.Session("AccountProfileInfo"), MAccountProfileInfo)

            End If
            ' Enable autopostback                
            AutoPostBack = True
        End Sub 'New

        '*********************************************************************
        '
        ' OnLoad Method
        '
        ' Checks the checkbox when the user has asked to be notified.
        '
        '*********************************************************************
        Protected Overrides Sub OnLoad(ByVal e As EventArgs)
            If Not Page.IsPostBack Then
                'Checked = NotifyUtility.GetNotificationStatus(objPageInfo.ID, objSectionInfo.ID, objUserInfo.Username)
            End If
            ' call base method
            MyBase.OnLoad(e)
        End Sub 'OnLoad

        '*********************************************************************
        '
        ' OnCheckedChanged Method
        '
        ' Changes notification status for user in database.
        ' If the user is not authenticated, redirects to login page.
        '
        '*********************************************************************
        Protected Overrides Sub OnCheckedChanged(ByVal e As EventArgs)
            ' Check if authenticated
            If Not objAccountProfileInfo.IsAuthenticated Then
                'CommunityGlobals.ForceLogin()
            End If
            ' Update Database
            'NotifyUtility.UpdateNotificationStatus(objPageInfo.ID, objSectionInfo.ID, objUserInfo.Username, Checked)


            ' call base method
            MyBase.OnCheckedChanged(e)
        End Sub 'OnCheckedChanged
    End Class 'Notify 
End Namespace