-- Downgrade from 5.0.0.0 to 4.1.0.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'ZGWCoreWebApplication')
	BEGIN
		EXEC sys.sp_executesql N'CREATE SCHEMA [ZGWCoreWebApplication]';
	END
--END IF
GO

UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 11 WHERE [Action] = 'accounts'
UPDATE [ZGWSecurity].[Functions] SET [Is_Nav] = 1 WHERE [FunctionSeqId] = 76

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '4.1.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;