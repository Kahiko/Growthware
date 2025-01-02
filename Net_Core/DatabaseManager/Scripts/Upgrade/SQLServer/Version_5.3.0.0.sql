-- Upgrade from 5.2.0.0 to 5.3.0.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
GO

	DECLARE 
		@P_RoleSeqId INT = -1,
		@P_Name VARCHAR(50) = 'QA',
		@P_Description VARCHAR(128) = 'Quality Assurance Role',
		@P_Is_System INT = 0,
		@P_Is_System_Only INT = 0,
		@P_SecurityEntitySeqId INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Primary_Key int,
		@P_Debug INT = 1

	EXEC [ZGWSecurity].[Set_Role]
		@P_RoleSeqId,
		@P_Name,
		@P_Description,
		@P_Is_System,
		@P_Is_System_Only,
		@P_SecurityEntitySeqId,
		@P_Added_Updated_By,
		@P_Primary_Key OUT,
		@P_Debug

	DECLARE 
		@P_Account VARCHAR(128) = 'Mike';

	EXEC [ZGWSecurity].[Set_Role_Accounts]
		@P_Primary_Key,
		@P_SecurityEntitySeqId,
		@P_Account,
		@P_Added_Updated_By,
		@P_Debug


-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '5.3.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;