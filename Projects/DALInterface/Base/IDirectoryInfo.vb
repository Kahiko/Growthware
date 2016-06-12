Imports DALModel.Base.Directories

Namespace Base.Interfaces
	Public Interface IDirectoryInfo
		Function addUpdateDirectoryInfo(ByVal directoryInfo As MDirectoryProfileInformation, Optional ByVal Account_Seq_id As Integer = 1) As Boolean
		Function GetDirectoryCollectionFromDB() As MDirectoryProfileInfoCollection
	End Interface
End Namespace