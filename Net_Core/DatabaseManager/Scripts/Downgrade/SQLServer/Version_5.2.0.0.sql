-- Downgrade from 5.2.0.0 to 5.1.0.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
GO

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

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '5.1.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;