Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Common.Cache
Imports ApplicationBase.Model.Directories
Imports ApplicationBase.Common.Security
#Region " Notes "
' The DirectoryInfoUtility class aids in managing the DirectoryInfo object.
#End Region
Public Class DirectoryInfoUtility
	Public Shared ReadOnly DirectoryInfoCachedCollection As String = "DirectoryInfoCollection"

    Public Shared Function getDirectoryInfo(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As MDirectoryProfileInformation
        On Error Resume Next
        Dim myCryptoUtil As New CryptoUtil
        Dim directoryCollection As MDirectoryProfileInfoCollection = CType(HttpContext.Current.Cache.Item(DirectoryInfoCachedCollection), MDirectoryProfileInfoCollection)
        If directoryCollection Is Nothing Then
            directoryCollection = BDirectoryInfo.GetAllEnabledDirectories()
            CacheControler.AddToCacheDependency(DirectoryInfoCachedCollection, directoryCollection)
        End If
        Dim retDirectoryInfo As MDirectoryProfileInformation
        retDirectoryInfo = directoryCollection.GetDirectoryByBusinessUnitID(BUSINESS_UNIT_SEQ_ID)
        ' the directory information in the DB is encrypted
        ' so we need to attempt to decrypt it
        retDirectoryInfo.Directory = myCryptoUtil.DecryptTripleDES(retDirectoryInfo.Directory)
        retDirectoryInfo.Impersonate_Account = myCryptoUtil.DecryptTripleDES(retDirectoryInfo.Impersonate_Account)
        retDirectoryInfo.Impersonate_PWD = myCryptoUtil.DecryptTripleDES(retDirectoryInfo.Impersonate_PWD)
        If retDirectoryInfo Is Nothing Then
            retDirectoryInfo = New MDirectoryProfileInformation
            retDirectoryInfo.BUSINESS_UNIT_SEQ_ID = BUSINESS_UNIT_SEQ_ID
        End If
        Return retDirectoryInfo
    End Function
End Class