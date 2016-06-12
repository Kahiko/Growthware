Imports System
Imports System.Data
Imports System.Data.SqlClient

'*********************************************************************
'
' ContentInfo Class
'
' Represents all information about a particular content item.
' This is the base class for the BookInfo, DownloadInfo, PhotoInfo
' ArticleInfo, etc. classes. 
'
'*********************************************************************

Public Class ContentInfo

    Private _contentPageID As Integer = -1
    Private _title As String = String.Empty
    Private _briefDescription As String = String.Empty
    Private _dateCreated As DateTime
    Private _dateUpdated As DateTime
    Private _dateCommented As DateTime
    Private _sectionID As Integer = -1
    Private _author As String = String.Empty
    Private _topicID As Integer = -1
    Private _topicName As String = String.Empty
    Private _topicImage As String = String.Empty
    Private _yourRating As Integer = -1
    Private _averageRating As Integer = -1
    Private _viewCount As Integer = -1
    Private _commentCount As Integer = -1
    Private _hasRead As Boolean = False
    Private _remoteAuthor As String = String.Empty



    '*********************************************************************
    '
    ' ContentInfo Constructor
    '
    ' Initializes content information. 
    '
    '*********************************************************************
    Public Sub New(ByVal dr As SqlDataReader)
        ' Required fields
        _contentPageID = Fix(dr("ContentPage_ID"))
        _title = CStr(dr("ContentPage_Title"))
        _briefDescription = CStr(dr("ContentPage_Description"))
        _dateCreated = CType(dr("ContentPage_DateCreated"), DateTime)
        _sectionID = Fix(dr("ContentPage_SectionID"))
        _viewCount = Fix(dr("ContentPage_ViewCount"))
        _commentCount = Fix(dr("CommentCount"))
        _hasRead = CBool(dr("HasRead"))
        _remoteAuthor = CStr(dr("RemoteUsername"))

        ' Optional fields
        If Not IsDBNull(dr("ContentPage_DateUpdated")) Then
            _dateUpdated = CType(dr("ContentPage_DateUpdated"), DateTime)
        End If
        If Not IsDBNull(dr("ContentPage_DateCommented")) Then
            _dateCommented = CType(dr("ContentPage_DateCommented"), DateTime)
        End If
        If Not IsDBNull(dr("Username")) Then
            _author = CStr(dr("Username"))
        End If
        If Not IsDBNull(dr("Topic_ID")) Then
            _topicID = Fix(dr("Topic_ID"))
        End If
        If Not IsDBNull(dr("Topic_Name")) Then
            _topicName = CStr(dr("Topic_Name"))
        End If
        If Not IsDBNull(dr("Topic_Image")) Then
            _topicImage = CStr(dr("Topic_Image"))
        End If
        If Not IsDBNull(dr("YourRating")) Then
            _yourRating = Fix(dr("YourRating"))
        End If
        If Not IsDBNull(dr("AverageRating")) Then
            _averageRating = Fix(dr("AverageRating"))
        End If
    End Sub 'New


    Public Sub New()
    End Sub 'New


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
    ' Title Property
    '
    ' Represents the content page Title. 
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
    ' BriefDescription Property
    '
    ' Represents the content page brief description. 
    '
    '*********************************************************************

    Public Property BriefDescription() As String
        Get
            Return _briefDescription
        End Get
        Set(ByVal value As String)
            _briefDescription = value
        End Set
    End Property

    '*********************************************************************
    '
    ' DateCreated Property
    '
    ' Represents the date that a content page was added to
    ' the database. 
    '
    '*********************************************************************

    Public Property DateCreated() As DateTime
        Get
            Return _dateCreated
        End Get
        Set(ByVal value As DateTime)
            _dateCreated = value
        End Set
    End Property

    '*********************************************************************
    '
    ' DateUpdated Property
    '
    ' Represents the date that a content page was last updated
    '
    '*********************************************************************

    Public Property DateUpdated() As DateTime
        Get
            Return _dateUpdated
        End Get
        Set(ByVal value As DateTime)
            _dateUpdated = value
        End Set
    End Property

    '*********************************************************************
    '
    ' DateCommented Property
    '
    ' Represents the date that a content page last had a comment added
    '
    '*********************************************************************

    Public Property DateCommented() As DateTime
        Get
            Return _dateCommented
        End Get
        Set(ByVal value As DateTime)
            _dateCommented = value
        End Set
    End Property

    '*********************************************************************
    '
    ' SectionID Property
    '
    ' Represents the section that contains this content page. 
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
    ' Author Property
    '
    ' Represents the username of the author of this content page. 
    '
    '*********************************************************************

    Public Property Author() As String
        Get
            Return _author
        End Get
        Set(ByVal value As String)
            _author = value
        End Set
    End Property

    '*********************************************************************
    '
    ' TopicID Property
    '
    ' Represents the content page topic ID. 
    '
    '*********************************************************************

    Public Property TopicID() As Integer
        Get
            Return _topicID
        End Get
        Set(ByVal value As Integer)
            _topicID = value
        End Set
    End Property

    '*********************************************************************
    '
    ' TopicName Property
    '
    ' Represents the content page topic name. 
    '
    '*********************************************************************

    Public Property TopicName() As String
        Get
            Return _topicName
        End Get
        Set(ByVal value As String)
            _topicName = value
        End Set
    End Property

    '*********************************************************************
    '
    ' TopicImage Property
    '
    ' Represents the content page topic image. 
    '
    '*********************************************************************

    Public Property TopicImage() As String
        Get
            Return _topicImage
        End Get
        Set(ByVal value As String)
            _topicImage = value
        End Set
    End Property

    '*********************************************************************
    '
    ' YourRating Property
    '
    ' Represents the current user's rating of the content page. 
    '
    '*********************************************************************

    Public Property YourRating() As Integer
        Get
            Return _yourRating
        End Get
        Set(ByVal value As Integer)
            _yourRating = value
        End Set
    End Property

    '*********************************************************************
    '
    ' AverageRating Property
    '
    ' Represents the average rating of the content page. 
    '
    '*********************************************************************

    Public Property AverageRating() As Integer
        Get
            Return _averageRating
        End Get
        Set(ByVal value As Integer)
            _averageRating = value
        End Set
    End Property

    '*********************************************************************
    '
    ' ViewCount Property
    '
    ' Represents the number of times the content page
    ' has been requested. 
    '
    '*********************************************************************

    Public Property ViewCount() As Integer
        Get
            Return _viewCount
        End Get
        Set(ByVal value As Integer)
            _viewCount = value
        End Set
    End Property

    '*********************************************************************
    '
    ' CommentCount Property
    '
    ' Represents the number of comments that have been added to
    ' the content page. 
    '
    '*********************************************************************

    Public Property CommentCount() As Integer
        Get
            Return _commentCount
        End Get
        Set(ByVal value As Integer)
            _commentCount = value
        End Set
    End Property

    '*********************************************************************
    '
    ' HasRead Property
    '
    ' True, if the user has previously viewed content. 
    '
    '*********************************************************************

    Public Property HasRead() As Boolean
        Get
            Return _hasRead
        End Get
        Set(ByVal value As Boolean)
            _hasRead = value
        End Set
    End Property


    '*********************************************************************
    '
    ' RemoteAuthor Property
    '
    ' The name of the name remote author of this content. 
    '
    '*********************************************************************

    Public Property RemoteAuthor() As String
        Get
            Return _remoteAuthor
        End Get
        Set(ByVal value As String)
            _remoteAuthor = value
        End Set
    End Property
End Class 'ContentInfo 
