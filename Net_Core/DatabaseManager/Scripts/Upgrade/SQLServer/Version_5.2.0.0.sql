-- Upgrade from 5.1.0.0 to 5.2.0.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
GO

/****** Start: Sequence [ZGWOptional].[FeedbackCounter] ******/
IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ZGWOptional].[FeedbackCounter]') AND type = 'SO')
    CREATE SEQUENCE [ZGWOptional].[FeedbackCounter] AS INT START WITH 1 INCREMENT BY 1;
--END IF
GO
/****** End: Sequence [ZGWOptional].[FeedbackCounter] ******/
/****** Start: Table [ZGWOptional].[Feedbacks] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ZGWOptional].[Feedbacks]') AND type in (N'U'))
	CREATE TABLE [ZGWOptional].[Feedbacks] (
        [FeedbackId] [INT] NOT NULL,
        [Assignee] [INT] NOT NULL,
        [Details] [NVARCHAR](MAX) NOT NULL,
        [FoundInVersion] VARCHAR(32) NOT NULL,
        [Notes] [NVARCHAR](MAX) NULL,
        [Severity] VARCHAR(32) NULL,
        [Status] VARCHAR(32) NULL,
        [TargetVersion] VARCHAR(32) NULL,
        [Type] [NVARCHAR](128) NULL,
        [VerifiedBy] INT NULL,
		[Start_Date] DATETIME NOT NULL,
		[End_Date] DATETIME NULL,
        CONSTRAINT [UK_Feedback] UNIQUE CLUSTERED ([Start_Date] DESC, [End_Date] DESC, [FeedbackId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	) ON [PRIMARY];
GO
-- Add constraints
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'D' AND [name] LIKE 'DF_ZGWOptional_Feedbacks_Start_Date')
    BEGIN
        ALTER TABLE [ZGWOptional].[Feedbacks] ADD CONSTRAINT [DF_ZGWOptional_Feedbacks_Start_Date] DEFAULT (getdate()) FOR [Start_Date]
    END
--END IF
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[ZGWOptional].[FK_Accounts_Feedbacks_Asignee]') AND parent_object_id = OBJECT_ID(N'[ZGWOptional].[Feedbacks]'))
    BEGIN
        ALTER TABLE [ZGWOptional].[Feedbacks] WITH CHECK ADD CONSTRAINT [FK_Accounts_Feedbacks_Asignee] FOREIGN KEY([Assignee])
        REFERENCES [ZGWSecurity].[Accounts] ([AccountSeqId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
    END
--END IF
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[ZGWOptional].[FK_Accounts_Feedbacks_VerifiedBy]') AND parent_object_id = OBJECT_ID(N'[ZGWOptional].[Feedbacks]'))
    BEGIN
        ALTER TABLE [ZGWOptional].[Feedbacks] WITH CHECK ADD CONSTRAINT [FK_Accounts_Feedbacks_VerifiedBy] FOREIGN KEY([VerifiedBy])
        REFERENCES [ZGWSecurity].[Accounts] ([AccountSeqId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
    END
--END IF
GO
-- Add extended properties
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = 0)
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'An "inline" history table of feedbacks/issues. The row idenifier for the table is the combination of FeedbackId + Start_Date + End_Date!' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'FeedbackId' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'A unique identifier for the feedback/issue (not the row).' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'FeedbackId';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Assignee' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'A FK from [ZGWSecurity].[Accounts].[AccountSeqId].' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Assignee';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Details' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The details of the feedback/issue from the submitter' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Details';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'FoundInVersion' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The version of the system the feedback/issue was found in' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'FoundInVersion';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Notes' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'Notes from the developer' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Notes';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Severity' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'TheDescriptionHere' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Severity';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Status' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The status of the feedback/issue, submitted/open/open-in progress/closed/closed-cannot reproduce/closed-works as designed' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Status';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'TargetVersion' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The version of the system the feedback/issue was released in' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'TargetVersion';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Type' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The type of issue/feedback, bug/feature-change/feature-request.  Where bugs are issues that cause a problem, feature-change is when the request is to change an existing feature, and feature-request is when the request is to add a new feature' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Type';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'VerifiedBy' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'A FK from [ZGWSecurity].[Accounts].[AccountSeqId].' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'VerifiedBy';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Start_Date' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The date the record was created in the database.  Used in conjunction with End_Date and FeedbackId in order to maintain an "inline" history. The oldest Start_Date is when the feedback/issue was created.' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Start_Date';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'End_Date' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The date the record became "history".  Used in conjunction with Start_Date and FeedbackId in order to maintain an "inline" history.  When the End_Date is null the record is current.  When the End_Date is not null the record is either a historical record or the feedback/issue has been "deleted" (the Start_Date should be the same value).' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'End_Date';
GO
/****** End: Table [ZGWOptional].[Feedbacks] ******/
/****** Start: View [ZGWOptional].[vwHistoryFeedback] ******/
IF NOT EXISTS (SELECT * FROM sys.views WHERE [schema_id] = SCHEMA_ID('ZGWOptional') AND [name] = 'vwHistoryFeedback')
    BEGIN
        EXEC dbo.sp_executesql @statement = N'CREATE VIEW [ZGWOptional].[vwHistoryFeedback] AS SELECT * FROM [ZGWOptional].[Feedbacks]';
    END
--END IF
GO
ALTER VIEW ZGWOptional.vwHistoryFeedback AS
    SELECT 
          [FeedbackId]
        , [Assignee]
        , [Details]
        , [FoundInVersion]
        , [Notes]
        , [Severity]
        , [Status]
        , [TargetVersion]
        , [Type]
        , [VerifiedBy]
        , [Start_Date]
        , [End_Date]
    FROM 
        [ZGWOptional].[Feedbacks];
GO
/****** Start: View [ZGWOptional].[vwHistoryFeedback] ******/
/****** Start: View [ZGWOptional].[vwCurrentFeedbacks] ******/
IF NOT EXISTS (SELECT * FROM sys.views WHERE [schema_id] = SCHEMA_ID('ZGWOptional') AND [name] = 'vwCurrentFeedbacks')
    BEGIN
        EXEC dbo.sp_executesql @statement = N'CREATE VIEW [ZGWOptional].[vwCurrentFeedbacks] AS SELECT * FROM [ZGWOptional].[Feedbacks]';
    END
--END IF
GO
ALTER VIEW ZGWOptional.vwCurrentFeedbacks AS
    SELECT 
          [FeedbackId]
        , [Assignee]
        , [Details]
        , [FoundInVersion]
        , [Notes]
        , [Severity]
        , [Status]
        , [TargetVersion]
        , [Type]
        , [VerifiedBy]
        , [Start_Date]
        , [End_Date]
    FROM 
        [ZGWOptional].[Feedbacks]
    WHERE 
        [End_Date] IS NULL;
GO
/****** End: View [ZGWOptional].[vwCurrentFeedbacks] ******/
/****** Start: Procedure [ZGWOptional].[Set_Feedback] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'ZGWOptional.Set_Feedback') AND type IN ( N'P' ,N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWOptional].[Set_Feedback] AS'
	END
--End If
GO
/*
Usage:
	DECLARE 
        @P_FeedbackId int = -1,
        @P_Assignee int = (SELECT AccountSeqId FROM [ZGWSecurity].[Accounts] WHERE [Account] = 'System'),
        @P_Details NVARCHAR(MAX) = 'The details',
        @P_FoundInVersion VARCHAR(32) = 'x.x.x.x',
        @P_Notes NVARCHAR(MAX) = null,
        @P_Severity VARCHAR(32) = 'Needs Classification', -- ???
        @P_Status VARCHAR(32) = 'Open', -- Under Review, In Progress, Closed, Closed-Not Fixed, Closed-Could not Reproduce
        @P_TargetVersion VARCHAR(32) = NULL,
        @P_Type [NVARCHAR](128) = 'Needs Classification', -- Feature Request, Bug, General, Needs Classification
        @P_VerifiedBy int = (SELECT AccountSeqId FROM [ZGWSecurity].[Accounts] WHERE [Account] = 'Anonymous'),
        @P_Primary_Key INT = null,
        @P_Debug INT = 0
    
    EXECUTE [ZGWOptional].[Set_Feedback]
        @P_FeedbackId,
        @P_Assignee,
        @P_Details,
        @P_FoundInVersion,
        @P_Notes,
        @P_Severity,
        @P_Status,
        @P_TargetVersion,
        @P_Type,
        @P_VerifiedBy,
        @P_Primary_Key,
        @P_Debug

    SET @P_FeedbackId = @P_Primary_Key;
    SET @P_Assignee = (SELECT AccountSeqId FROM [ZGWSecurity].[Accounts] WHERE [Account] = 'Assignee');
    SET @P_Details = 'Changed details';
    SET @P_FoundInVersion = '5.2.0.1023';
    SET @P_Notes = 'Notes from the developer';
    SET @P_Severity = '???';
    SET @P_Status = 'Closed-Could not Reproduce';
    SET @P_TargetVersion = '5.2.0';
    SET @P_Type = 'Feature request';
    SET @P_VerifiedBy = (SELECT AccountSeqId FROM [ZGWSecurity].[Accounts] WHERE [Account] = 'Mike');

    EXECUTE [ZGWOptional].[Set_Feedback]
        @P_FeedbackId,
        @P_Assignee,
        @P_Details,
        @P_FoundInVersion,
        @P_Notes,
        @P_Severity,
        @P_Status,
        @P_TargetVersion,
        @P_Type,
        @P_VerifiedBy,
        @P_Primary_Key,
        @P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 11/21/2024
-- Description:	Inserts or updates [ZGWOptional].[Set_Feedback]
-- =============================================
ALTER PROCEDURE [ZGWOptional].[Set_Feedback]
     @P_FeedbackId int
    ,@P_Assignee int
    ,@P_Details NVARCHAR(MAX)
    ,@P_FoundInVersion VARCHAR(32)
    ,@P_Notes NVARCHAR(MAX)
    ,@P_Severity VARCHAR(32) = 'Needs Classification'
    ,@P_Status VARCHAR(32) = 'Unassigned'
    ,@P_TargetVersion VARCHAR(32) = NULL
    ,@P_Type [NVARCHAR](128) = 'Needs Classification'
    ,@P_VerifiedBy int = -1
    ,@P_Primary_Key INT OUTPUT
    ,@P_Debug INT = 0
AS
	SET NOCOUNT ON;
	IF @P_Debug = 1 PRINT 'Starting ZGWOptional.Set_Feedback';
    IF @P_VerifiedBy = -1 SET @P_VerifiedBy = (SELECT AccountSeqId FROM [ZGWSecurity].[Accounts] WHERE [Account] = 'Anonymous')
    DECLARE @V_Now DATETIME = GETDATE()
            , @V_FeedbackId INT = @P_FeedbackId;

    IF EXISTS (SELECT TOP(1) NULL FROM [ZGWOptional].[Feedbacks] WHERE FeedbackId = @P_FeedbackId)
        BEGIN -- Update the End_Date for the "current" feedback record
            PRINT 'Updating feedback';
            IF EXISTS (SELECT TOP(1) NULL FROM [ZGWOptional].[Feedbacks] WHERE FeedbackId = @P_FeedbackId AND [End_Date] IS NULL)
                BEGIN
                    UPDATE [ZGWOptional].[Feedbacks] SET [End_Date] = @V_Now WHERE FeedbackId = @P_FeedbackId AND [End_Date] IS NULL;
                END
            --END IF
        END
    ELSE
        BEGIN
            PRINT 'Adding new feedback';
            SET @V_FeedbackId = NEXT VALUE FOR [ZGWOptional].[FeedbackCounter]
        END
    --END IF
    -- We are always creating a new record with a null End_Date (null End_Date indicaes it is the current record)
    INSERT INTO [ZGWOptional].[Feedbacks] (
        [Assignee],
        [Details],
        [FoundInVersion],
        [Notes],
        [Severity],
        [Status],
        [TargetVersion],
        [Type],
        [VerifiedBy],
        [Start_Date],
        [End_Date]
    ) VALUES (
        @P_Assignee,
        @P_Details,
        @P_FoundInVersion,
        @P_Notes,
        @P_Severity,
        @P_Status,
        @P_TargetVersion,
        @P_Type,
        @P_VerifiedBy,
        @V_Now,
        NULL
    )
    -- Set the return "primary" key value.
    SELECT @P_Primary_Key = @V_FeedbackId;

    IF @P_Debug = 1 PRINT 'Ending ZGWOptional.Set_Feedback';
RETURN 0

GO
/****** End: Procedure [ZGWOptional].[Set_Feedback] ******/
/****** Start: Procedure [ZGWOptional].[Delete_Feedback] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'ZGWOptional.Delete_Feedback') AND type IN ( N'P' ,N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWOptional].[Delete_Feedback] AS'
	END
--End If
GO
/*
Usage:
	DECLARE 
        @P_FeedbackId int = 1,
        @P_Debug INT = 0
    
    EXECUTE [ZGWOptional].[Delete_Feedback]
        @P_FeedbackId,
        @P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 11/21/2024
-- Description:	"Deletes" a Feedback from table [ZGWOptional].[Feedbacks] by setting the End_Date to now and 
--      inserting a new record to identify the feedback/issue has been deleted.  After the End_Date is set 
--      to now, there is nolonger a "current" record and we need to create a "history" record to 
--      indicate that the feedback/issue was deleted.  In order to track the deletion we inset a new record where
--      all the values are the same as the record we just updated except for the Start_Date and End_Date.  The
--      Start_Date and End_Date are both set to now, leaving a record we can use to track the deletion. A query
--      with a predicate of Start_Date = End_Date will return all the "deleted" feedback/issue records.
-- =============================================
ALTER PROCEDURE [ZGWOptional].[Delete_Feedback]
	@P_FeedbackId int,
	@P_Debug INT = 0
AS
    SET NOCOUNT ON;
    DECLARE @V_Now DATETIME = GETDATE();
    IF @P_Debug = 1 PRINT 'Starting ZGWOptional.Delete_Feedback';
    IF EXISTS (SELECT TOP(1) NULL FROM [ZGWOptional].[Feedbacks] WHERE FeedbackId = @P_FeedbackId AND [End_Date] IS NULL)
        BEGIN
            IF @P_Debug = 1 PRINT 'Setting the End_Date for the "current" feedback record in ZGWOptional.Delete_Feedback';
            /*
             * Update the End_Date for the "current" feedback record and insert a new record to indicate the feedback/issue was deleted
             * If your table design uses the "identifing key" is used in other tables it is here where you need
             * to handle those tables manually here.
            */
            UPDATE [ZGWOptional].[Feedbacks] SET [End_Date] = @V_Now WHERE FeedbackId = @P_FeedbackId AND [End_Date] IS NULL;
            INSERT INTO [ZGWOptional].[Feedbacks] ( 
                [Assignee],
                [Details],
                [FoundInVersion],
                [Notes],
                [Severity],
                [Status],
                [TargetVersion],
                [Type],
                [VerifiedBy],
                [Start_Date],
                [End_Date]
            )
            SELECT 
                FBs.[Assignee],
                FBs.[Details],
                FBs.[FoundInVersion],
                FBs.[Notes],
                FBs.[Severity],
                FBs.[Status],
                FBs.[TargetVersion],
                FBs.[Type],
                FBs.[VerifiedBy],
                @V_Now,
                @V_Now
            FROM
                [ZGWOptional].[Feedbacks] AS FBs
            WHERE 1=1
                AND FBs.[FeedbackId] = @P_FeedbackId 
                AND FBs.[End_Date] = @V_Now;
        End
    --END IF
    IF @P_Debug = 1 PRINT 'Ending ZGWOptional.Delete_Feedback';

RETURN 0

GO
/****** End: Procedure [ZGWOptional].[Delete_Feedback] ******/
/****** Start: Procedure [ZGWOptional].[Get_Feedback] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'ZGWOptional.Get_Feedback') AND type IN ( N'P' ,N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWOptional].[Get_Feedback] AS'
	END
--End If
GO
/*
Usage:
	DECLARE 
        @P_FeedbackId int = 1,
        @P_Debug INT = 0
    
    EXECUTE [ZGWOptional].[Get_Feedback]
        @P_FeedbackId,
        @P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 11/21/2024
-- Description:	Gets 1 or more Feedbacks from table [ZGWOptional].[Feedbacks]
--  The UI tends to use [ZGWSystem].[Get_Paginated_Data] to retrieve multiple rows of data
-- =============================================
ALTER PROCEDURE [ZGWOptional].[Get_Feedback]
	@P_FeedbackId int,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON;
	IF @P_Debug = 1 PRINT 'Starting ZGWOptional.Get_Feedback';
    DECLARE @V_Now DATETIME = GETDATE();
    IF NOT EXISTS (SELECT NULL FROM [ZGWOptional].[Feedbacks] WHERE FeedbackId = @P_FeedbackId)
        BEGIN
            IF @P_Debug = 1 PRINT 'Return all records';
            SELECT
                *
            FROM 
                [ZGWOptional].[Feedbacks]
        END
    ELSE
        BEGIN
            IF @P_Debug = 1 PRINT 'Return one record';
            SELECT
                *
            FROM 
                [ZGWOptional].[Feedbacks]
            WHERE FeedbackId = @P_FeedbackId
        END
    --END IF

RETURN 0

GO
/****** End: Procedure [ZGWOptional].[Get_Feedback] ******/


-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '5.2.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;