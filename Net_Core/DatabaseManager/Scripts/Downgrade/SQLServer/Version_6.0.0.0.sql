-- Downgrade from 6.0.0.0 to 5.2.0.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
GO

DECLARE 
      @V_FunctionSeqId INT = -1
    , @V_Action VARCHAR(256) = '/sys_admin/searchDBLogs'
	, @P_ErrorCode BIT = 0;

IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [Action] = @V_Action)
	BEGIN
		PRINT 'Deleting ' + CONVERT(VARCHAR(MAX), @V_Action);
		SET @V_FunctionSeqId = (SELECT TOP(1) [FunctionSeqId] FROM [ZGWSecurity].[Functions] WHERE [Action] =  @V_Action);	
		EXEC [ZGWSecurity].[Delete_Function]
			@V_FunctionSeqId ,
			@P_ErrorCode;
	END
--END IF

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '5.2.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;