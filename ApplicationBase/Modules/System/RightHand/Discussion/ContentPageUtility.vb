Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Collections
Imports System.Web
Imports System.Text.RegularExpressions
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model

'*********************************************************************
'
' ContentPageUtility Class
'
' Contains static methods for working with content pages.
'
'*********************************************************************
Public Class ContentPageUtility
    '*********************************************************************
    '
    ' GetTotalRecords Method
    '
    ' Returns the total number of visible content pages in a 
    ' particular section. 
    '
    '*********************************************************************
    Public Shared Function GetTotalRecords(ByVal sectionID As Integer) As Integer
        Dim conPortal As New SqlConnection("Server=(local);Trusted_Connection=true;database=CommunityStarterKit")
        Dim cmdGetTotal As New SqlCommand("Community_ContentPagesGetTotalRecords", conPortal)
        cmdGetTotal.CommandType = CommandType.StoredProcedure
        cmdGetTotal.Parameters.AddWithValue("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
        cmdGetTotal.Parameters.AddWithValue("@sectionID", sectionID)
        conPortal.Open()
        cmdGetTotal.ExecuteNonQuery()
        Dim totalRecords As Integer = Fix(cmdGetTotal.Parameters("@RETURN_VALUE").Value)
        conPortal.Close()

        Return totalRecords
    End Function 'GetTotalRecords

    '*********************************************************************
    '
    ' GetTotalRecordsWithInvisible Method
    '
    ' Returns the total number of visible and invisible content pages in a 
    ' particular section. 
    '
    '*********************************************************************
    Public Shared Function GetTotalRecordsWithInvisible(ByVal sectionID As Integer) As Integer
        Dim conPortal As New SqlConnection("Server=(local);Trusted_Connection=true;database=CommunityStarterKit")
        Dim cmdGetTotal As New SqlCommand("Community_ContentPagesGetTotalRecordsWithInvisible", conPortal)
        cmdGetTotal.CommandType = CommandType.StoredProcedure
        cmdGetTotal.Parameters.AddWithValue("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
        cmdGetTotal.Parameters.AddWithValue("@sectionID", sectionID)
        conPortal.Open()
        cmdGetTotal.ExecuteNonQuery()
        Dim totalRecords As Integer = Fix(cmdGetTotal.Parameters("@RETURN_VALUE").Value)
        conPortal.Close()

        Return totalRecords
    End Function 'GetTotalRecordsWithInvisible

    '*********************************************************************
    '
    ' GetContentPageInfoFromDB Method
    '
    ' Returns a content page from the database given a 
    ' particular ID. 
    '
    '*********************************************************************
    Public Shared Function GetContentPageInfoFromDB(ByVal contentID As Integer, ByVal sectionID As Integer) As ContentPageInfo
        Dim _contentPageInfo As ContentPageInfo = Nothing

        Dim conPortal As New SqlConnection("Server=(local);Trusted_Connection=true;database=CommunityStarterKit")
        Dim cmdGet As New SqlCommand("Community_ContentPagesGetContentPage", conPortal)
        cmdGet.CommandType = CommandType.StoredProcedure
        cmdGet.Parameters.AddWithValue("@communityID", BaseSettings.defaultBusinessUnitID)
        cmdGet.Parameters.AddWithValue("@sectionID", sectionID)
        cmdGet.Parameters.AddWithValue("@contentPageID", contentID)

        conPortal.Open()
        Dim dr As SqlDataReader = cmdGet.ExecuteReader()
        If dr.Read() Then
            _contentPageInfo = PopulateContentPageInfoFromSqlDataReader(dr)
        End If
        conPortal.Close()

        Return _contentPageInfo
    End Function 'GetContentPageInfoFromDB




    '*********************************************************************
    '
    ' PopulateContentPageInfoFromSqlDataReader Method
    '
    ' Generates a ContentPageInfo object given a SqlDataReader.
    '
    '*********************************************************************
    Public Shared Function PopulateContentPageInfoFromSqlDataReader(ByVal dr As SqlDataReader) As ContentPageInfo
        Dim _contentPageInfo As New ContentPageInfo()

        _contentPageInfo.ContentPageID = Fix(dr("contentPage_id"))
        _contentPageInfo.ParentID = Fix(dr("contentPage_parentID"))
        _contentPageInfo.SectionID = Fix(dr("contentPage_sectionID"))
        _contentPageInfo.PageType = Fix(dr("contentPage_pageType"))
        _contentPageInfo.Title = CStr(dr("contentPage_title"))
        _contentPageInfo.Description = CStr(dr("contentPage_description"))
        _contentPageInfo.MetaDescription = CStr(dr("contentPage_metaDesc"))
        _contentPageInfo.MetaKeys = CStr(dr("contentPage_metaKeys"))
        _contentPageInfo.PageContent = CStr(dr("PageType_pageContent"))
        _contentPageInfo.ModerationStatus = CType(dr("contentPage_moderationStatus"), ModerationStatus)

        Return _contentPageInfo
    End Function 'PopulateContentPageInfoFromSqlDataReader




    '*********************************************************************
    '
    ' DeleteContentPage Method
    '
    ' Deletes a content page from the database.
    '
    '*********************************************************************
    Public Shared Sub DeleteContentPage(ByVal contentPageID As Integer, ByVal sectionID As Integer)
        Dim conPortal As New SqlConnection("Server=(local);Trusted_Connection=true;database=CommunityStarterKit")
        Dim cmd As New SqlCommand("Community_ContentPagesDeleteContentPage", conPortal)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@communityID", BaseSettings.defaultBusinessUnitID)
        cmd.Parameters.AddWithValue("@sectionID", sectionID)
        cmd.Parameters.AddWithValue("@contentPageID", contentPageID)

        conPortal.Open()
        cmd.ExecuteNonQuery()
        conPortal.Close()
    End Sub 'DeleteContentPage




    '*********************************************************************
    '
    ' MoveContentPage Method
    '
    ' Moves a content page from one section to another.
    '
    '*********************************************************************
    Public Shared Sub MoveContentPage(ByVal contentPageID As Integer, ByVal moderationStatus As ModerationStatus, ByVal sectionID As Integer)
        Dim conPortal As New SqlConnection("Server=(local);Trusted_Connection=true;database=CommunityStarterKit")
        Dim cmd As New SqlCommand("Community_ContentPagesMoveContentPage", conPortal)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@communityID", BaseSettings.defaultBusinessUnitID)
        cmd.Parameters.AddWithValue("@contentPageID", contentPageID)
        cmd.Parameters.AddWithValue("@moderationStatus", Fix(moderationStatus))
        cmd.Parameters.AddWithValue("@sectionID", sectionID)

        conPortal.Open()
        cmd.ExecuteNonQuery()
        conPortal.Close()
    End Sub 'MoveContentPage




    '*********************************************************************
    '
    ' CalculateMetaDescription Method
    '
    ' Truncates description to 250 characters.
    '
    '*********************************************************************
    Public Shared Function CalculateMetaDescription(ByVal description As String) As String
        Return BaseHelperOld.Truncate(HttpUtility.HtmlEncode(description), 250)
    End Function 'CalculateMetaDescription



    '*********************************************************************
    '
    ' CalculateMetaKeys Method
    '
    ' Truncates description to 200 characters and splits into separate
    ' words.
    '
    '*********************************************************************
    Public Shared Function CalculateMetaKeys(ByVal metaKeys As String) As String
        Dim keys As String = BaseHelperOld.Truncate(HttpUtility.HtmlEncode(metaKeys), 200)
        Dim words As MatchCollection = Regex.Matches(keys, "\w+")

        Dim joinKeys As String = String.Empty
        Dim word As Match
        For Each word In words
            joinKeys += word.Value + ","
        Next word
        joinKeys.TrimEnd(","c)
        Return joinKeys
    End Function 'CalculateMetaKeys




    '*********************************************************************
    '
    ' CalculateContentPath Method
    '
    ' This method returns the relative path of a content page 
    ' (including the section path). This method is used, for example,
    ' by the Topic page to generate the path to a content page.
    '
    '*********************************************************************
    Public Shared Function CalculateContentPath(ByVal sectionID As Integer, ByVal contentID As Integer) As String
        'Dim path As String = SectionUtility.GetSectionPath(sectionID)
        Dim path As String = BaseSettings.rootSite
        path = path.Remove(path.LastIndexOf("/"), path.Length - path.LastIndexOf("/"))
        Return String.Format("{0}/{1}.aspx", path, contentID)
    End Function 'CalculateContentPath



    '*********************************************************************
    '
    ' CalculateFullContentPath Method
    '
    ' This method generates the absolute path to a content page. It
    ' includes the domain information for the path.
    '
    '*********************************************************************
    Public Shared Function CalculateFullContentPath(ByVal sectionID As Integer, ByVal contentID As Integer) As String
        Dim path As String = CalculateContentPath(sectionID, contentID)
        Return BaseSettings.FQDNPage
    End Function 'CalculateFullContentPath





    '*********************************************************************
    '
    ' GetNewContent Method
    '
    ' Retrieves the latest content added to this community
    ' of a particular type.
    '
    '*********************************************************************
    Public Shared Function GetNewContent(ByVal pageTypeName As String) As ArrayList
        Dim colContent As New ArrayList()

        Dim conPortal As New SqlConnection("Server=(local);Trusted_Connection=true;database=CommunityStarterKit")
        Dim cmdGet As New SqlCommand("Community_GetNewContent", conPortal)
        cmdGet.CommandType = CommandType.StoredProcedure
        cmdGet.Parameters.AddWithValue("@communityID", BaseSettings.defaultBusinessUnitID)
        cmdGet.Parameters.AddWithValue("@pageTypeName", pageTypeName)
        conPortal.Open()

        Dim dr As SqlDataReader = cmdGet.ExecuteReader()
        While dr.Read()
            colContent.Add(New ContentInfo(dr))
        End While
        conPortal.Close()

        Return colContent
    End Function 'GetNewContent




    '*********************************************************************
    '
    ' GetPopularContent Method
    '
    ' Retrieves the most viewed content added to this community
    ' of a particular type.
    '
    '*********************************************************************
    Public Shared Function GetPopularContent(ByVal pageTypeName As String) As ArrayList
        Dim colContent As New ArrayList()

        Dim conPortal As New SqlConnection("Server=(local);Trusted_Connection=true;database=CommunityStarterKit")
        Dim cmdGet As New SqlCommand("Community_GetPopularContent", conPortal)
        cmdGet.CommandType = CommandType.StoredProcedure
        cmdGet.Parameters.AddWithValue("@communityID", BaseSettings.defaultBusinessUnitID)
        cmdGet.Parameters.AddWithValue("@pageTypeName", pageTypeName)
        conPortal.Open()

        Dim dr As SqlDataReader = cmdGet.ExecuteReader()
        While dr.Read()
            colContent.Add(New ContentInfo(dr))
        End While
        conPortal.Close()

        Return colContent
    End Function 'GetPopularContent

    '*********************************************************************
    '
    ' MoveContentUp Method
    '
    ' Used when sorting content to move content up. 
    '
    '*********************************************************************
    Public Shared Sub MoveContentUp(ByVal contentPageID As Integer)
        Dim conPortal As New SqlConnection("Server=(local);Trusted_Connection=true;database=CommunityStarterKit")
        Dim cmd As New SqlCommand("Community_ContentPagesMoveContentUp", conPortal)
        cmd.CommandType = CommandType.StoredProcedure

        cmd.Parameters.AddWithValue("@contentPageID", contentPageID)
        conPortal.Open()
        cmd.ExecuteNonQuery()
        conPortal.Close()
    End Sub 'MoveContentUp


    '*********************************************************************
    '
    ' MoveContentDown Method
    '
    ' Used when sorting content to move content down. 
    '
    '*********************************************************************
    Public Shared Sub MoveContentDown(ByVal contentPageID As Integer)
        Dim conPortal As New SqlConnection("Server=(local);Trusted_Connection=true;database=CommunityStarterKit")
        Dim cmd As New SqlCommand("Community_ContentPagesMoveContentDown", conPortal)
        cmd.CommandType = CommandType.StoredProcedure

        cmd.Parameters.AddWithValue("@contentPageID", contentPageID)

        conPortal.Open()
        cmd.ExecuteNonQuery()
        conPortal.Close()
    End Sub 'MoveContentDown
End Class 'ContentPageUtility 


