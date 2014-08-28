Imports GrowthWare.Framework.Model.Profiles
Imports System.IO
Imports System.Web.Caching
Imports System.Web
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Common
Imports System.Globalization

''' <summary>
''' Facade for System.Web.Caching
''' </summary>
Public Class CacheController
    Private Shared s_CacheDirectory As String = HttpContext.Current.Server.MapPath("~\") & "CacheDependency\"

    ''' <summary>
    '''	AddToCacheDependency function Adds an object to the
    ''' cache as well as creates/re-writes the cacheDependency file
    ''' based on the appropriate application variable
    ''' For each cache dependency file a corresponding application variable
    ''' is created, this is done to better track when a file needs
    ''' to be changed.
    ''' 
    ''' The core of the cachecontroler relys on the application
    ''' running within a web farm and gives the ability
    ''' to keep cached objects syncronized between the servers.
    ''' When there is a change to one of the cache dependency objects
    ''' the cache object is re-created in the servers memory.
    ''' Should part of the application remove a cache object from memory
    ''' the corresponding file is altered, file replication occures
    ''' and the others servers will then update their in memory cache
    ''' objects the next time the cache objected is requested.
    ''' </summary>
    ''' <param name="key">
    '''		String representation of the cached object as
    ''' the corresponding cache file name "myKey.txt".
    ''' </param>
    ''' <param name="value">
    '''		Object being placed into cache.
    ''' </param>
    ''' <returns>
    '''		Boolean
    ''' </returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ReganM1]	12/15/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function AddToCacheDependency(ByVal key As String, ByVal value As Object) As Boolean
        Dim retVal As Boolean = False
        If Not ConfigSettings.CentralManagement And ConfigSettings.EnableCache Then
            Dim fileStream As FileStream = Nothing
            Dim fileName As String
            fileName = s_CacheDirectory & key & ".txt"
            ' ensure the file exists if not then create one
            If Not File.Exists(fileName) Then
                Try
                    File.Create(fileName).Close()
                Catch ex As Exception
                    Dim DirectoryProfile As New MDirectoryProfile
                    FileUtility.CreateDirectory(HttpContext.Current.Server.MapPath("~\"), "CacheDependency", DirectoryProfile)
                    File.Create(fileName).Close()
                End Try
                HttpContext.Current.Application.Lock()
                HttpContext.Current.Application(key & "WriteCache") = True
                HttpContext.Current.Application.UnLock()
            End If
            ' re-write the dependancy file based on the application variable
            ' file replication will cause the other servers to remove their cache item
            If Convert.ToBoolean(HttpContext.Current.Application(key & "WriteCache"), CultureInfo.InvariantCulture) Then
                Try
                    fileStream = New FileStream(fileName, FileMode.Truncate)
                    Using writer As New StreamWriter(fileStream)
                        writer.WriteLine(Now.TimeOfDay)
                    End Using
                    HttpContext.Current.Application.Lock()
                    HttpContext.Current.Application(key & "WriteCache") = False
                    HttpContext.Current.Application.UnLock()
                Catch ex As Exception
                    Throw ex
                Finally
                    If Not fileStream Is Nothing Then fileStream.Dispose()
                End Try
            End If
            ' cache it for future use
            Dim onCacheRemove As CacheItemRemovedCallback = Nothing
            Dim mCacheDependency As CacheDependency = Nothing
            Try
                onCacheRemove = New CacheItemRemovedCallback(AddressOf CheckCallback)
                mCacheDependency = New CacheDependency(fileName)
                If Not value Is Nothing Then HttpContext.Current.Cache.Add(key, value, mCacheDependency, Caching.Cache.NoAbsoluteExpiration, Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheRemove)
            Catch ex As Exception
                Throw ex
            Finally
                If Not mCacheDependency Is Nothing Then mCacheDependency.Dispose()
            End Try
            If Err.Number = 0 Then retVal = True
        Else
            retVal = True
        End If
        Return retVal
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Remove a cache item from the servers memory.
    ''' </summary>
    ''' <param name="cacheName"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ReganM1]	12/15/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Sub RemoveFromCache(ByVal cacheName As String)
        Dim fileName As String
        fileName = s_CacheDirectory & cacheName & ".txt"
        If File.Exists(fileName) Then
            File.Delete(fileName)
        End If
        HttpContext.Current.Cache.Remove(cacheName)
    End Sub

    ''' <summary>
    ''' Catches when a cache item has been removed from the servers memory then, 
    ''' sets the corresponding application "WriteCache" variable to true, 
    ''' forcing the server to write to the associated cache file upon the next request 
    ''' for the cached item.
    ''' </summary>
    ''' <param name="key">Name of the cached item.</param>
    ''' <param name="value">Generally a object being placed into cache.</param>
    ''' <param name="reason">The reason the item was removed from cache.</param>
    ''' <remarks></remarks>
    Public Shared Sub CheckCallback(ByVal key As String, ByVal value As Object, ByVal reason As CacheItemRemovedReason)
        'rebuild cache and file to sync cached objects if necessary
        If Not key Is Nothing Then
            If reason = CacheItemRemovedReason.Removed Then
                Dim log As Logger = Logger.Instance()
                Try
                    If Not HttpContext.Current Is Nothing Then
                        HttpContext.Current.Application(key & "WriteCache") = True
                    End If
                    log.Info("CheckCallback() :: " + key + " is no longer in cache")
                Catch ex As NullReferenceException

                    log.Info("CheckCallback() :: NullReferenceException was encountered." + System.Environment.NewLine + ex.Message.ToString())
                    ' do nothing
                Catch ex As Exception
                    log.Error("CheckCallback() :: Unexpected was encountered." + System.Environment.NewLine + ex.Message.ToString())
                    Throw
                End Try
            End If
        End If
    End Sub

    ''' <summary>
    ''' Removes all cache items from member by removing all files from the cache dependency directory.
    ''' </summary>
    Public Shared Sub RemoveAllCache()
        Dim DirectoryFiles As New DirectoryInfo(s_CacheDirectory)
        For Each directoryFile As FileInfo In DirectoryFiles.GetFiles("*.*")
            File.Delete(DirectoryFiles.FullName & directoryFile.Name)
        Next
    End Sub

End Class
