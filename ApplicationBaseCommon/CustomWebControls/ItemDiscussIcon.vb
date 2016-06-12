Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports ApplicationBase.Model.Discuss
Imports ApplicationBase.Common.Globals

Namespace CustomWebControls
    '*********************************************************************
    '
    ' ItemDiscussIcon Class
    '
    ' Displays different images indicating the type of post:
    '   
    '  * Pinned 
    '  * Announcement
    '  * Popular
    '  * Locked
    '
    '*********************************************************************
    Public Class ItemDiscussIcon
        Inherits WebControl

        Private _popularCount As Integer = 0
        Private _ImagePath As String = BaseSettings.imagePath
        Private _popularImage As String = _ImagePath & "HasRead/Popular.Gif"

        Private _readImage As String = _ImagePath & "HasRead/Read.Gif"
        Private _notReadImage As String = _ImagePath & "HasRead/NotRead.Gif"

        Private _announcementReadImage As String = _ImagePath & "Discuss/Announce_Read.Gif"
        Private _announcementNotReadImage As String = _ImagePath & "Discuss/Announce_NotRead.Gif"

        Private _pinnedReadImage As String = _ImagePath & "Discuss/Pinned_Read.Gif"
        Private _pinnedNotReadImage As String = _ImagePath & "Discuss/Pinned_NotRead.Gif"

        Private _pinnedLockedReadImage As String = _ImagePath & "Discuss/PinnedLocked_Read.Gif"
        Private _pinnedLockedNotReadImage As String = _ImagePath & "Discuss/PinnedLocked_NotRead.Gif"

        Private _lockedReadImage As String = _ImagePath & "Discuss/Locked_Read.Gif"
        Private _lockedNotReadImage As String = _ImagePath & "Discuss/Locked_NotRead.Gif"


        '*********************************************************************
        '
        ' ReadImage Property
        '
        ' The path to the image that is displayed when user has read item
        '
        '*********************************************************************

        Public Property ReadImage() As String
            Get
                Return _readImage
            End Get
            Set(ByVal value As String)
                _readImage = value
            End Set
        End Property



        '*********************************************************************
        '
        ' NotReadImage Property
        '
        ' The path to the image that is displayed when user has not read item
        '
        '*********************************************************************

        Public Property NotReadImage() As String
            Get
                Return _notReadImage
            End Get
            Set(ByVal value As String)
                _notReadImage = value
            End Set
        End Property



        '*********************************************************************
        '
        ' PopularImage Property
        '
        ' The path to the image that is displayed when item is popular
        '
        '*********************************************************************

        Public Property PopularImage() As String
            Get
                Return _popularImage
            End Get
            Set(ByVal value As String)
                _popularImage = value
            End Set
        End Property



        '*********************************************************************
        '
        ' PopularCount Property
        '
        ' How many users must read to make this item popular
        '
        '*********************************************************************

        Public Property PopularCount() As Integer
            Get
                Return _popularCount
            End Get
            Set(ByVal value As Integer)
                _popularCount = value
            End Set
        End Property


        '*********************************************************************
        '
        ' HasRead Property
        '
        ' Store the HasRead property in View State
        '
        '*********************************************************************

        Public Property HasRead() As Boolean
            Get
                If ViewState("HasRead") Is Nothing Then
                    Return False
                Else
                    Return CBool(ViewState("HasRead"))
                End If
            End Get
            Set(ByVal value As Boolean)
                ViewState("HasRead") = value
            End Set
        End Property

        '*********************************************************************
        '
        ' ViewCount Property
        '
        ' Store the ViewCount property in View State
        '
        '*********************************************************************

        Public Property ViewCount() As Integer
            Get
                If ViewState("ViewCount") Is Nothing Then
                    Return 0
                Else
                    Return Fix(ViewState("ViewCount"))
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState("ViewCount") = value
            End Set
        End Property

        '*********************************************************************
        '
        ' Pinned Property
        '
        ' Store the Pinned property in View State
        '
        '*********************************************************************

        Public Property Pinned() As Boolean
            Get
                If ViewState("Pinned") Is Nothing Then
                    Return False
                Else
                    Return CBool(ViewState("Pinned"))
                End If
            End Get
            Set(ByVal value As Boolean)
                ViewState("Pinned") = value
            End Set
        End Property
        '*********************************************************************
        '
        ' Announcement Property
        '
        ' Store the Announcement property in View State
        '
        '*********************************************************************

        Public Property Announcement() As Boolean
            Get
                If ViewState("Announcement") Is Nothing Then
                    Return False
                Else
                    Return CBool(ViewState("Announcement"))
                End If
            End Get
            Set(ByVal value As Boolean)
                ViewState("Announcement") = value
            End Set
        End Property


        '*********************************************************************
        '
        ' Locked Property
        '
        ' Store the Locked property in View State
        '
        '*********************************************************************

        Public Property Locked() As Boolean
            Get
                If ViewState("Locked") Is Nothing Then
                    Return False
                Else
                    Return CBool(ViewState("Locked"))
                End If
            End Get
            Set(ByVal value As Boolean)
                ViewState("Locked") = value
            End Set
        End Property



        '*********************************************************************
        '
        ' ItemDiscussIcon Constructor
        '
        ' Assign a default css style (the user can override)
        '
        '*********************************************************************
        Public Sub New()
            CssClass = "itemDiscussIcon"
            EnableViewState = False
        End Sub 'New



        '*********************************************************************
        '
        ' OnDataBinding Method
        '
        ' Get the read status from the container's DataItem property
        '
        '*********************************************************************
        Protected Overrides Sub OnDataBinding(ByVal e As EventArgs)
            Dim item As ContentItem

            If TypeOf NamingContainer Is ContentItem Then
                item = CType(NamingContainer, ContentItem)
            Else
                item = CType(NamingContainer.NamingContainer, ContentItem)
            End If

            Dim objPostInfo As PostInfo = CType(item.DataItem, PostInfo)
            HasRead = objPostInfo.HasRead
            ViewCount = objPostInfo.ViewCount
            Announcement = objPostInfo.IsAnnouncement
            Pinned = objPostInfo.IsPinned
            Locked = objPostInfo.IsLocked
        End Sub 'OnDataBinding




        '*********************************************************************
        '
        ' RenderContents Method
        '
        ' Display the HasRead image
        '
        '*********************************************************************
        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            ' if popular, than just show that
            If _popularCount > 0 AndAlso ViewCount > _popularCount Then
                writer.Write(String.Format("<img src=""{0}"">", Page.ResolveUrl(_popularImage)))
                Return
            End If


            ' Otherwise, show Pinned and Locked
            If Pinned AndAlso Locked Then
                If HasRead Then
                    writer.Write(String.Format("<img src=""{0}"">", Page.ResolveUrl(_pinnedLockedReadImage)))
                Else
                    writer.Write(String.Format("<img src=""{0}"">", Page.ResolveUrl(_pinnedLockedNotReadImage)))
                End If
                Return
            End If


            ' Otherwise, show Pinned
            If Pinned Then
                If HasRead Then
                    writer.Write(String.Format("<img src=""{0}"">", Page.ResolveUrl(_pinnedReadImage)))
                Else
                    writer.Write(String.Format("<img src=""{0}"">", Page.ResolveUrl(_pinnedNotReadImage)))
                End If
                Return
            End If

            ' Otherwise, show announcement
            If Announcement Then
                If HasRead Then
                    writer.Write(String.Format("<img src=""{0}"">", Page.ResolveUrl(_announcementReadImage)))
                Else
                    writer.Write(String.Format("<img src=""{0}"">", Page.ResolveUrl(_announcementNotReadImage)))
                End If
                Return
            End If


            ' Otherwise, show locked
            If Locked Then
                If HasRead Then
                    writer.Write(String.Format("<img src=""{0}"">", Page.ResolveUrl(_lockedReadImage)))
                Else
                    writer.Write(String.Format("<img src=""{0}"">", Page.ResolveUrl(_lockedNotReadImage)))
                End If
                Return
            End If



            ' Finally, just show Read status
            If HasRead Then
                writer.Write(String.Format("<img src=""{0}"">", Page.ResolveUrl(_readImage)))
            Else
                writer.Write(String.Format("<img src=""{0}"">", Page.ResolveUrl(_notReadImage)))
            End If
        End Sub 'RenderContents 
    End Class 'ItemDiscussIcon 
End Namespace
