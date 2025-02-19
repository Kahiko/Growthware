-- Downgrade from 5.2.0.0 to 5.1.0.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
GO

-- Remove the Role
DECLARE 
	@P_Name AS VARCHAR(50) = 'QA',
	@P_SecurityEntitySeqId AS INT = 1

EXEC [ZGWSecurity].[Delete_Role]
	@P_Name,
	@P_SecurityEntitySeqId

-- Remove the functions
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [Action] LIKE 'feedbacks/edit')
	BEGIN
		DELETE FROM [ZGWSecurity].[Functions] WHERE [Action] LIKE 'feedbacks/edit';
	END
--END IF
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [Action] = 'feedbacks')
	BEGIN
		DELETE FROM [ZGWSecurity].[Functions] WHERE [Action] = 'feedbacks';
	END
--END IF
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [Action] LIKE 'feedbacks/feedback')
	BEGIN
		DELETE FROM [ZGWSecurity].[Functions] WHERE [Action] LIKE 'feedbacks/feedback';
	END
--END IF

/****** Start: View [ZGWOptional].[vwHistoryFeedback] ******/
IF EXISTS (SELECT * FROM sys.views WHERE [name] = 'ZGWOptional.vwHistoryFeedback')
    BEGIN
        DROP VIEW [ZGWOptional].[vwHistoryFeedback];
    END
--END IF
GO
/****** Start: View [ZGWOptional].[vwHistoryFeedback] ******/
IF EXISTS (SELECT * FROM sys.views WHERE [name] = 'ZGWOptional.vwCurrentFeedbacks')
    BEGIN
        DROP VIEW [ZGWOptional].[vwCurrentFeedbacks];
    END
--END IF
GO
/****** End: View [ZGWOptional].[vwCurrentFeedbacks] ******/
/****** Start: Procedure [ZGWOptional].[Get_Feedback] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'ZGWOptional.Get_Feedback') AND type IN ( N'P' ,N'PC'))
	BEGIN
		DROP PROCEDURE [ZGWOptional].[Get_Feedback];
	END
--End If
/****** End: Procedure [ZGWOptional].[Get_Feedback] ******/
/****** Start: Procedure [ZGWOptional].[Delete_Feedback] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'ZGWOptional.Delete_Feedback') AND type IN ( N'P' ,N'PC'))
	BEGIN
		DROP PROCEDURE [ZGWOptional].[Delete_Feedback];
	END
--End If
/****** End: Procedure [ZGWOptional].[Delete_Feedback] ******/
/****** Start: Procedure [ZGWOptional].[Set_Feedback] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'ZGWOptional.Set_Feedback') AND type IN ( N'P' ,N'PC'))
	BEGIN
		DROP PROCEDURE [ZGWOptional].[Set_Feedback];
	END
--End If
/****** End: Procedure [ZGWOptional].[Set_Feedback] ******/
-- Drop the table
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ZGWOptional].[Feedbacks]') AND type in (N'U'))
	DROP TABLE [ZGWOptional].[Feedbacks];
GO
/****** Start: Sequence [ZGWOptional].[FeedbackCounter] ******/
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ZGWOptional].[FeedbackCounter]') AND type = 'SO')
    DROP SEQUENCE [ZGWOptional].[FeedbackCounter];
--END IF
GO
/****** End: Sequence [ZGWOptional].[FeedbackCounter] ******/
-- Put the sort order back to the way it was before the upgrade
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 13)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 0 WHERE [FunctionSeqId] = 13;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 16)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 16;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 24)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 24;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 17)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 3 WHERE [FunctionSeqId] = 17;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 23)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 4 WHERE [FunctionSeqId] = 23;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 67)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 5 WHERE [FunctionSeqId] = 67;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 20)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 6 WHERE [FunctionSeqId] = 20;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 76)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 76;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 77)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 77;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 78)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 78;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 79)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 79;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 85)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 85;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 88)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 88;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 24)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 25;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 26)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 26;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 27)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 27;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 28)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 28;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 31)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 31;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 64)
	UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 64;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 28)
    UPDATE [ZGWSecurity].[Functions] SET [Action] = 'SetLogLevel' WHERE FunctionSeqId = 28

IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 16)
    UPDATE [ZGWSecurity].[Functions] SET [Action] = 'search_security_entities' WHERE FunctionSeqId = 16
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 63)
    UPDATE [ZGWSecurity].[Functions] SET [Action] = 'SelectASecurityEntity' WHERE FunctionSeqId = 63

UPDATE 
	[ZGWSecurity].[Functions] 
SET [Source] = 'https://localhost:44455/swagger/index.html' 
WHERE [FunctionSeqId] = (SELECT FunctionSeqId FROM [ZGWSecurity].[Functions] WHERE [Action] = 'SwaggerAPI')

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '5.1.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;