Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model
Imports ApplicationBase.Model.Discuss

Namespace Discuss
    '*********************************************************************
    '
    ' DiscussUtility Class
    '
    ' Contains static methods for working with threads and posts in the
    ' discuss module. 
    '
    '*********************************************************************

    Public Class DiscussUtility
        '*********************************************************************
        '
        ' AddPost Method
        '
        ' Adds a new post to the database. 
        '
        '*********************************************************************
        Public Shared Function AddPost(ByVal username As String, ByVal sectionID As Integer, ByVal topicID As Integer, ByVal title As String, ByVal moderationStatus As Integer, ByVal isPinned As Boolean, ByVal isAnnouncement As Boolean, ByVal isLocked As Boolean, ByVal bodyText As String) As Integer
            ' Create brief description
            Dim briefDescription As String = BaseHelperOld.TruncateWithEllipsis(BaseHelperOld.StripTags(bodyText), 500)

            Dim conPortal As New SqlConnection("Server=(local);Trusted_Connection=true;database=CommunityStarterKit")
            Dim cmdAdd As New SqlCommand("Community_DiscussAddPost", conPortal)
            cmdAdd.CommandType = CommandType.StoredProcedure

            cmdAdd.Parameters.AddWithValue("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            cmdAdd.Parameters.AddWithValue("@communityID", BaseSettings.defaultBusinessUnitID)
            cmdAdd.Parameters.AddWithValue("@sectionID", sectionID)
            cmdAdd.Parameters.AddWithValue("@username", username)
            cmdAdd.Parameters.AddWithValue("@topicID", topicID)
            cmdAdd.Parameters.AddWithValue("@title", title)
            cmdAdd.Parameters.AddWithValue("@briefDescription", briefDescription)
            cmdAdd.Parameters.AddWithValue("@metaDescription", ContentPageUtility.CalculateMetaDescription(briefDescription))
            cmdAdd.Parameters.AddWithValue("@metaKeys", ContentPageUtility.CalculateMetaKeys(briefDescription))
            cmdAdd.Parameters.AddWithValue("@moderationStatus", moderationStatus)
            cmdAdd.Parameters.AddWithValue("@isPinned", isPinned)
            cmdAdd.Parameters.AddWithValue("@isAnnouncement", isAnnouncement)
            cmdAdd.Parameters.AddWithValue("@isLocked", isLocked)
            cmdAdd.Parameters.AddWithValue("@bodyText", SqlDbType.NText)
            cmdAdd.Parameters("@bodyText").Value = bodyText

            conPortal.Open()
            cmdAdd.ExecuteNonQuery()
            Dim result As Integer = Fix(cmdAdd.Parameters("@RETURN_VALUE").Value)

            ' Add Search Keys
            'SearchUtility.AddSearchKeys(conPortal, sectionID, result, title, briefDescription)

            conPortal.Close()

            Return result
        End Function 'AddPost

        '*********************************************************************
        '
        ' AddPostReply Method
        '
        ' Adds a new comment to the database. 
        '
        '*********************************************************************
        Public Shared Function AddPostReply(ByVal contentPageID As Integer, ByVal moderationStatus As ModerationStatus, ByVal username As String, ByVal title As String, ByVal body As String) As Integer
            Dim conPortal As New SqlConnection("Server=(local);Trusted_Connection=true;database=CommunityStarterKit")
            Dim cmdAdd As New SqlCommand("Community_DiscussAddReply", conPortal)
            cmdAdd.CommandType = CommandType.StoredProcedure

            ' Add Parameters
            cmdAdd.Parameters.AddWithValue("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            cmdAdd.Parameters.AddWithValue("@communityID", BaseSettings.defaultBusinessUnitID)
            cmdAdd.Parameters.AddWithValue("@contentPageID", contentPageID)
            cmdAdd.Parameters.AddWithValue("@moderationStatus", moderationStatus)
            cmdAdd.Parameters.AddWithValue("@username", username)
            cmdAdd.Parameters.AddWithValue("@title", title)
            cmdAdd.Parameters.AddWithValue("@metaDescription", ContentPageUtility.CalculateMetaDescription(body))
            cmdAdd.Parameters.AddWithValue("@metaKeys", ContentPageUtility.CalculateMetaKeys(body))
            cmdAdd.Parameters.AddWithValue("@body", SqlDbType.NText)
            cmdAdd.Parameters("@body").Value = body


            conPortal.Open()
            cmdAdd.ExecuteNonQuery()
            Dim result As Integer = Fix(cmdAdd.Parameters("@RETURN_VALUE").Value)
            conPortal.Close()

            Return result
        End Function 'AddPostReply

        '*********************************************************************
        '
        ' EditPost Method
        '
        ' Edits an existing post in the database. 
        '
        '*********************************************************************
        Public Shared Sub EditPost(ByVal contentPageID As Integer, ByVal username As String, ByVal sectionID As Integer, ByVal topicID As Integer, ByVal title As String, ByVal isPinned As Boolean, ByVal isAnnouncement As Boolean, ByVal isLocked As Boolean, ByVal bodyText As String)
            ' Create brief description
            Dim briefDescription As String = BaseHelperOld.TruncateWithEllipsis(BaseHelperOld.StripTags(bodyText), 500)

            Dim conPortal As New SqlConnection("Server=(local);Trusted_Connection=true;database=CommunityStarterKit")
            Dim cmdAdd As New SqlCommand("Community_DiscussEditPost", conPortal)
            cmdAdd.CommandType = CommandType.StoredProcedure

            cmdAdd.Parameters.AddWithValue("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            cmdAdd.Parameters.AddWithValue("@communityID", BaseSettings.defaultBusinessUnitID)
            cmdAdd.Parameters.AddWithValue("@contentPageID", contentPageID)
            cmdAdd.Parameters.AddWithValue("@username", username)
            cmdAdd.Parameters.AddWithValue("@topicID", topicID)
            cmdAdd.Parameters.AddWithValue("@title", title)
            cmdAdd.Parameters.AddWithValue("@briefDescription", briefDescription)
            cmdAdd.Parameters.AddWithValue("@metaDescription", ContentPageUtility.CalculateMetaDescription(briefDescription))
            cmdAdd.Parameters.AddWithValue("@metaKeys", ContentPageUtility.CalculateMetaKeys(briefDescription))
            cmdAdd.Parameters.AddWithValue("@isPinned", isPinned)
            cmdAdd.Parameters.AddWithValue("@isAnnouncement", isAnnouncement)
            cmdAdd.Parameters.AddWithValue("@isLocked", isLocked)
            cmdAdd.Parameters.AddWithValue("@bodyText", SqlDbType.NText)
            cmdAdd.Parameters("@bodyText").Value = bodyText

            conPortal.Open()
            cmdAdd.ExecuteNonQuery()

            ' Edit Search Keys
            'SearchUtility.EditSearchKeys(conPortal, sectionID, contentPageID, title, briefDescription)

            conPortal.Close()
        End Sub 'EditPost

        '*********************************************************************
        '
        ' GetPosts Method
        '
        ' Gets posts for this section. 
        '
        '*********************************************************************
        Public Shared Function GetPosts(ByVal username As String, ByVal sectionID As Integer, ByVal pageSize As Integer, ByVal pageIndex As Integer, ByVal sortOrder As String) As ArrayList
            Dim conPortal As New SqlConnection("Server=(local);Trusted_Connection=true;database=CommunityStarterKit")
            Dim cmdGet As New SqlCommand("Community_DiscussGetPosts", conPortal)
            cmdGet.CommandType = CommandType.StoredProcedure
            cmdGet.Parameters.AddWithValue("@communityID", BaseSettings.defaultBusinessUnitID)
            cmdGet.Parameters.AddWithValue("@username", username)
            cmdGet.Parameters.AddWithValue("@sectionID", sectionID)
            cmdGet.Parameters.AddWithValue("@pageSize", pageSize)
            cmdGet.Parameters.AddWithValue("@pageIndex", pageIndex)
            cmdGet.Parameters.AddWithValue("@sortOrder", sortOrder)

            Dim colPosts As New ArrayList()

            conPortal.Open()
            Dim dr As SqlDataReader = cmdGet.ExecuteReader()
            While dr.Read()
                colPosts.Add(New PostInfo(dr))
            End While
            conPortal.Close()
            Return colPosts
        End Function 'GetPosts

        '*********************************************************************
        '
        ' GetPostInfo Method
        '
        ' Gets a particular post from the database. 
        '
        '*********************************************************************
        Public Shared Function GetPostInfo(ByVal username As String, ByVal contentPageID As Integer) As ContentInfo
            Dim _postInfo As PostInfo = Nothing

            Dim conPortal As New SqlConnection("Server=(local);Trusted_Connection=true;database=CommunityStarterKit")
            Dim cmdGet As New SqlCommand("Community_DiscussGetPost", conPortal)
            cmdGet.CommandType = CommandType.StoredProcedure
            cmdGet.Parameters.AddWithValue("@communityID", BaseSettings.defaultBusinessUnitID)
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

        Private Sub New()
        End Sub 'New
    End Class 'DiscussUtility
End Namespace