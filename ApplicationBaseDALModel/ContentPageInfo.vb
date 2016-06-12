Imports System

'*********************************************************************
'
' ContentPageInfo Class
'
' Represents all information about a particular content page. This
' class differs from the content info class in that it does not
' represent information about the content of a content page. It
' represents only the information needed to create the page shell
' in the community default page.
'
'*********************************************************************
Public Class ContentPageInfo

    Private _contentPageID As Integer
    Private _parentID As Integer
    Private _sectionID As Integer
    Private _pageType As Integer
    Private _title As String
    Private _description As String
    Private _metaDescription As String
    Private _metaKeys As String
    Private _pageContent As String
    Private _moderationStatus As ModerationStatus


    '*********************************************************************
    '
    ' ContentPageID Property
    '
    ' Represents the content page ID. 
    '
    '*********************************************************************

    Public Property ContentPageID() As Integer
        Get
            Return _contentPageID
        End Get
        Set(ByVal value As Integer)
            _contentPageID = value
        End Set
    End Property


    '*********************************************************************
    '
    ' ParentID Property
    '
    ' Represents the parent page of this page. 
    '
    '*********************************************************************

    Public Property ParentID() As Integer
        Get
            Return _parentID
        End Get
        Set(ByVal value As Integer)
            _parentID = value
        End Set
    End Property

    '*********************************************************************
    '
    ' SectionID Property
    '
    ' Represents the section associated with this page. 
    '
    '*********************************************************************

    Public Property SectionID() As Integer
        Get
            Return _sectionID
        End Get
        Set(ByVal value As Integer)
            _sectionID = value
        End Set
    End Property


    '*********************************************************************
    '
    ' PageType Property
    '
    ' Represents the ID of the type of content represented by
    ' this page. 
    '
    '*********************************************************************

    Public Property PageType() As Integer
        Get
            Return _pageType
        End Get
        Set(ByVal value As Integer)
            _pageType = value
        End Set
    End Property

    '*********************************************************************
    '
    ' Title Property
    '
    ' Represents the title of this page. 
    '
    '*********************************************************************

    Public Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
        End Set
    End Property

    '*********************************************************************
    '
    ' Description Property
    '
    ' Represents the description of this page. 
    '
    '*********************************************************************

    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property

    '*********************************************************************
    '
    ' MetaDescription Property
    '
    ' Represents the content page's meta description. 
    '
    '*********************************************************************

    Public Property MetaDescription() As String
        Get
            Return _metaDescription
        End Get
        Set(ByVal value As String)
            _metaDescription = value
        End Set
    End Property

    '*********************************************************************
    '
    ' MetaKeys Property
    '
    ' Represents the content page's meta keys. 
    '
    '*********************************************************************

    Public Property MetaKeys() As String
        Get
            Return _metaKeys
        End Get
        Set(ByVal value As String)
            _metaKeys = value
        End Set
    End Property

    '*********************************************************************
    '
    ' PageContent Property
    '
    ' Represents the type of content contained in the content page. 
    '
    '*********************************************************************

    Public Property PageContent() As String
        Get
            Return _pageContent
        End Get
        Set(ByVal value As String)
            _pageContent = value
        End Set
    End Property


    '*********************************************************************
    '
    ' ModerationStatus Property
    '
    ' Represents the current moderation status of the page. 
    '
    '*********************************************************************

    Public Property ModerationStatus() As ModerationStatus
        Get
            Return _moderationStatus
        End Get
        Set(ByVal value As ModerationStatus)
            _moderationStatus = value
        End Set
    End Property


    '*********************************************************************
    '
    ' ContentPageInfo Constructor
    '
    ' Initializes the ContentPageInfo class. 
    '
    '*********************************************************************
    Public Sub New()
    End Sub 'New 
End Class 'ContentPageInfo