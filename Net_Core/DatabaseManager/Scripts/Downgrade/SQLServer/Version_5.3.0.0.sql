-- Downgrade from 5.3.0.0 to 5.2.0.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
GO

	DECLARE 
		@P_Name AS VARCHAR(50) = 'QA',
		@P_SecurityEntitySeqId AS INT = 1

	EXEC [ZGWSecurity].[Delete_Role]
		@P_Name,
		@P_SecurityEntitySeqId

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '5.2.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;