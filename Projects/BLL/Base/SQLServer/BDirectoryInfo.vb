Imports DALFactory.Base.Application
Imports DALModel.Base.Directories
Imports DALInterface.Base.Interfaces
Imports Common.Security.BaseSecurity
Imports System.Runtime.InteropServices

Namespace Base.SQLServer
    Public Class BDirectoryInfo
		'Private Shared iBaseDAL As IDirectoryInfo = FDirectoryInfo.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"))
		Private Shared iBaseDAL As IDirectoryInfo = AbstractFactory.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"), "DDirectoryInfo")

        Public Shared Function addUpdateDirectoryInfo(ByVal directoryInfo As MDirectoryProfileInformation, Optional ByVal Account_Seq_id As Integer = 1) As Boolean
            Dim myCryptoUtil As New CryptoUtil
            directoryInfo.Directory = myCryptoUtil.EncryptTripleDES(directoryInfo.Directory)
            If directoryInfo.Impersonate_Account <> "" Then directoryInfo.Impersonate_Account = myCryptoUtil.EncryptTripleDES(directoryInfo.Impersonate_Account)
            If directoryInfo.Impersonate_PWD <> "" Then directoryInfo.Impersonate_PWD = myCryptoUtil.EncryptTripleDES(directoryInfo.Impersonate_PWD)
			Return iBaseDAL.addUpdateDirectoryInfo(directoryInfo, Account_Seq_id)
        End Function

        Public Shared Function GetAllEnabledDirectories() As MDirectoryProfileInfoCollection
			Return iBaseDAL.GetDirectoryCollectionFromDB
        End Function 'GetAllEnabledSections
    End Class
End Namespace