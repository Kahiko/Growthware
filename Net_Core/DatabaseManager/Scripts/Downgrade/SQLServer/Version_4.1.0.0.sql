-- Downgrade from 4.1.0.0 to 4.0.1.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '4.0.1.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;
