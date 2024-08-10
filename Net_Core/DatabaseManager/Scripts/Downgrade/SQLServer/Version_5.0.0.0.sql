-- Downgrade from 5.0.0.0 to 4.1.0.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
GO


-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '4.1.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;