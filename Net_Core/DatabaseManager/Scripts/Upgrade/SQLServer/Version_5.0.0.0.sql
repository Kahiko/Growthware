-- Upgrade from 4.1.0.0 to 5.0.0.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
GO

UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [Action] = 'accounts'
UPDATE [ZGWSecurity].[Functions] SET [Is_Nav] = 0 WHERE [FunctionSeqId] = 76

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '5.0.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;