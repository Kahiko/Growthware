Imports ApplicationBase.Model.Directories

Public Interface IDirectoryInfo
	Function addUpdateDirectoryInfo(ByVal directoryInfo As MDirectoryProfileInformation, Optional ByVal Account_Seq_id As Integer = 1) As Boolean
	Function GetDirectoryCollectionFromDB() As MDirectoryProfileInfoCollection
End Interface
