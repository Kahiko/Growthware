Imports System.IO
Imports System.Web
Imports System.Web.Caching
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Web.Utilities

Namespace Controllers
	''' <summary>
	''' Facade for System.Web.Caching
	''' </summary>
	Public Class CacheController
		Private Shared mCacheDirectory As String = HttpContext.Current.Server.MapPath("~\") & "CacheDependency\"

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
			If Not WebConfigSettings.CentralManagement And WebConfigSettings.EnableCache Then
				Dim fileStream As FileStream
				Dim writer As StreamWriter
				Dim fileName As String
				fileName = mCacheDirectory & Key & ".txt"
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
				If Not Value Is Nothing Then HttpContext.Current.Cache.Add(Key, Value, New CacheDependency(fileName), Caching.Cache.NoAbsoluteExpiration, Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheRemove)
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
		''' <param name="CacheName"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[ReganM1]	12/15/2006	Created
		''' </history>
		''' -----------------------------------------------------------------------------
		Public Shared Sub RemoveFromCache(ByVal CacheName As String)
			Dim fileName As String
			fileName = mCacheDirectory & CacheName & ".txt"
			If File.Exists(fileName) Then
				File.Delete(fileName)
			End If
			HttpContext.Current.Cache.Remove(CacheName)
		End Sub

		''' <summary>
		''' Catches when a cache item has been removed from the servers memory then, 
		''' sets the corresponding application "WriteCache" variable to true, 
		''' forcing the server to write to the associated cache file upon the next request 
		''' for the cached item.
		''' </summary>
		''' <param name="Key">Name of the cached item.</param>
		''' <param name="Value">Generally a object being placed into cache.</param>
		''' <param name="reason">The reason the item was removed from cache.</param>
		''' <remarks></remarks>
		Public Shared Sub CheckCallback(ByVal Key As String, ByVal Value As Object, ByVal reason As CacheItemRemovedReason)
			'rebuild cache and file to sync cached objects if necessary
			If Not Key Is Nothing Then
				If reason = CacheItemRemovedReason.Removed Then
					Try
						HttpContext.Current.Application(Key & "WriteCache") = True
					Catch ex As NullReferenceException
						Dim log As LogUtility = LogUtility.GetInstance()
						log.Error("CheckCallback() :: NullReferenceException was encountered." + Environment.NewLine + ex.Message.ToString())
						' do nothing
					Catch ex As Exception
						Throw ex
					End Try
				End If
			End If
		End Sub

		''' <summary>
		''' Removes all cache items from member by removing all files from the cache dependency directory.
		''' </summary>
		Public Shared Sub RemoveAllCache()
			Dim directoryInfo As New MDirectoryProfile
			Dim DirectoryFiles As New DirectoryInfo(mCacheDirectory)
			Dim directoryFile As FileInfo
			For Each directoryFile In DirectoryFiles.GetFiles("*.*")
				File.Delete(DirectoryFiles.FullName & directoryFile.Name)
			Next
		End Sub
	End Class
End Namespace
