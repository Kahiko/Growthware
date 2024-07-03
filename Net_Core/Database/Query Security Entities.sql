SELECT TOP (1000) 
	 [SecurityEntitySeqId]
	,[Name]
	,[Description]
	,[URL]
	,[StatusSeqId]
	,[DAL]
	,[DAL_Name]
	,[DAL_Name_Space]
	,[DAL_String]
	,[Skin]
	,[Style]
	,[Encryption_Type]
	,[ParentSecurityEntitySeqId]
	,[Added_By]
	,[Added_Date]
	,[Updated_By]
	,[Updated_Date]
FROM [GrowthWare].[ZGWSecurity].[Security_Entities]

/*
UPDATE [ZGWSecurity].[Security_Entities] SET [URL] = 'https://localhost' WHERE [SecurityEntitySeqId] = 1;
UPDATE [ZGWSecurity].[Security_Entities] SET [URL] = 'no url' WHERE [SecurityEntitySeqId] = 1;
*/
