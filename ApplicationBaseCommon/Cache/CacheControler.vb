Imports System
Imports System.IO
Imports System.Web
Imports System.Web.Caching

Namespace Cache
    Public Class CacheControler
        ''' <summary>
        '''		addToCacheDependency function Adds an object to the
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
        ''' <param name="Key">
        '''		String representation of the cached object as
        ''' the corresponding cache file name "myKey.txt".
        ''' </param>
        ''' <param name="Value">
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
        Public Shared Function AddToCacheDependency(ByVal Key As String, ByVal Value As Object) As Boolean
            Dim retVal As Boolean = False
            Dim fileStream As FileStream
            Dim writer As StreamWriter
            Dim fileName As String
            fileName = HttpContext.Current.Server.MapPath("~\") & "CacheDependency\" & Key & ".txt"
            ' ensure the file exists if not then create one
            If Not File.Exists(fileName) Then
                File.Create(fileName).Close()
                HttpContext.Current.Application.Lock()
                HttpContext.Current.Application(Key & "WriteCache") = True
                HttpContext.Current.Application.UnLock()
            End If
            ' re-write the dependancy file based on the application variable
            ' file replication will cause the other servers to remove their cache item
            If HttpContext.Current.Application(Key & "WriteCache") Then
                fileStream = New FileStream(fileName, FileMode.Truncate)
                writer = New StreamWriter(fileStream)
                writer.WriteLine(Now.TimeOfDay)
                writer.Close()
                fileStream.Close()
                HttpContext.Current.Application.Lock()
                HttpContext.Current.Application(Key & "WriteCache") = False
                HttpContext.Current.Application.UnLock()
            End If
            ' cache it for future use
            Dim onCacheRemove As CacheItemRemovedCallback
            onCacheRemove = New CacheItemRemovedCallback(AddressOf CheckCallback)
            HttpContext.Current.Cache.Add(Key, Value, New CacheDependency(fileName), Caching.Cache.NoAbsoluteExpiration, Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheRemove)
            If Err.Number = 0 Then retVal = True
            Return retVal
        End Function
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="CacheName"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ReganM1]	12/15/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Sub RemoveFromCache(ByVal CacheName As String)
            HttpContext.Current.Cache.Remove(CacheName)
        End Sub

        Public Shared Sub CheckCallback(ByVal Key As String, ByVal Value As Object, ByVal reason As CacheItemRemovedReason)
            'rebuild cache and file to sync cached objects if necessary
            If Not Key Is Nothing Then
                If reason = CacheItemRemovedReason.Removed Then
                    Try
                        HttpContext.Current.Application(Key & "WriteCache") = True
                    Catch ex As NullReferenceException
                    Catch ex As Exception
                        Throw ex
                    End Try
                End If
            End If
        End Sub
    End Class
End Namespace