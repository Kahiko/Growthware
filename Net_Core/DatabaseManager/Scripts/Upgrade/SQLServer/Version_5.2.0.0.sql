-- Upgrade from 5.1.0.0 to 5.2.0.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
GO

DECLARE 
    @P_RoleSeqId INT = -1,
    @P_Name VARCHAR(50) = 'QA',
    @P_Description VARCHAR(128) = 'Quality Assurance Role, used when editing a Feedback to populate Verified By.',
    @P_Is_System INT = 1,
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
        [AssigneeId] [INT] NOT NULL,
        [Date_Closed] DATETIME NULL,
        [Date_Opened] DATETIME NOT NULL,
        [Details] [NVARCHAR](MAX) NOT NULL,
        [Found_In_Version] VARCHAR(32) NOT NULL,
        [FunctionSeqId] INT NOT NULL,
        [Notes] [NVARCHAR](MAX) NULL,
        [Severity] VARCHAR(32) NULL,
        [Status] VARCHAR(32) NULL,
        [SubmittedById] INT NOT NULL,
        [TargetVersion] VARCHAR(32) NULL,
        [Type] [NVARCHAR](128) NULL,
        [UpdatedById] INT NOT NULL,
        [VerifiedById] INT NULL,
		[Start_Date] DATETIME NOT NULL,
		[End_Date] DATETIME NULL,
        CONSTRAINT [UI_CLX_Feedback] UNIQUE CLUSTERED ([FeedbackId] ASC, [Start_Date] DESC, [End_Date] DESC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	) ON [PRIMARY];
GO
IF NOT EXISTS (SELECT 1
            FROM sys.indexes I
                INNER JOIN sys.tables T
                    ON I.object_id = T.object_id
                INNER JOIN sys.schemas S
                    ON S.schema_id = T.schema_id
            WHERE I.Name = 'UIX_FeedbackId_End_Date' -- Index name
                AND T.Name = 'Feedbacks' -- Table name
                AND S.Name = 'ZGWOptional') --Schema Name
    BEGIN
        CREATE UNIQUE NONCLUSTERED INDEX [UIX_FeedbackId_End_Date] ON [ZGWOptional].[Feedbacks]
        (
            [FeedbackId] ASC,
            [End_Date] ASC
        )
        WHERE End_Date IS NULL
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
    END
-- END IF
-- Add constraints
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'D' AND [name] LIKE 'DF_ZGWOptional_Feedbacks_Date_Opened')
    BEGIN
        ALTER TABLE [ZGWOptional].[Feedbacks] ADD CONSTRAINT [DF_ZGWOptional_Feedbacks_Date_Opened] DEFAULT (getdate()) FOR [Date_Opened]
    END
--END IF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'D' AND [name] LIKE 'DF_ZGWOptional_Feedbacks_Start_Date')
    BEGIN
        ALTER TABLE [ZGWOptional].[Feedbacks] ADD CONSTRAINT [DF_ZGWOptional_Feedbacks_Start_Date] DEFAULT (getdate()) FOR [Start_Date]
    END
--END IF
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[ZGWOptional].[FK_Accounts_Feedbacks_AssigneeId]') AND parent_object_id = OBJECT_ID(N'[ZGWOptional].[Feedbacks]'))
    BEGIN
        ALTER TABLE [ZGWOptional].[Feedbacks] WITH CHECK ADD CONSTRAINT [FK_Accounts_Feedbacks_AssigneeId] FOREIGN KEY([AssigneeId])
        REFERENCES [ZGWSecurity].[Accounts] ([AccountSeqId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
    END
--END IF
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[ZGWOptional].[FK_Accounts_Feedbacks_UpdatedById]') AND parent_object_id = OBJECT_ID(N'[ZGWOptional].[Feedbacks]'))
    BEGIN
        ALTER TABLE [ZGWOptional].[Feedbacks] WITH CHECK ADD CONSTRAINT [FK_Accounts_Feedbacks_UpdatedById] FOREIGN KEY([UpdatedById])
        REFERENCES [ZGWSecurity].[Accounts] ([AccountSeqId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
    END
--END IF
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[ZGWOptional].[FK_Accounts_Feedbacks_SubmittedById]') AND parent_object_id = OBJECT_ID(N'[ZGWOptional].[Feedbacks]'))
    BEGIN
        ALTER TABLE [ZGWOptional].[Feedbacks] WITH CHECK ADD CONSTRAINT [FK_Accounts_Feedbacks_SubmittedById] FOREIGN KEY([SubmittedById])
        REFERENCES [ZGWSecurity].[Accounts] ([AccountSeqId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
    END
--END IF
GO
-- Add extended properties
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = 0)
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'An "inline" history table of feedbacks/issues. The row idenifier for the table is the combination of FeedbackId + Start_Date + End_Date!  The reason for this table is to provide an example of an "inline" history table, not, to provide a comprehensive feedback/issue tracking system.  It best if working with the data you use the stored procedures ([ZGWOptional].[Get_Feedback] and [ZGWOptional].[Set_Feedback])!' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'FeedbackId' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'A "unique" identifier for the feedback/issue (not the row).' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'FeedbackId';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'AssigneeId' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'A FK from [ZGWSecurity].[Accounts].[AccountSeqId].' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'AssigneeId';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Details' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The details of the feedback/issue from the submitter' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Details';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Date_Closed' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The date the feedback/issue was closed' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Date_Closed';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Date_Opened' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The date the feedback/issue was opened' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Date_Opened';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Found_In_Version' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The version of the system the feedback/issue was found in' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Found_In_Version';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'FunctionSeqId' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The area the feedback/issue was found FK from [ZGWSecurity].[Functions].[FunctionSeqId]' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'FunctionSeqId';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Notes' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'Notes from the developer' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Notes';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Severity' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The severity of the feedback/issue' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Severity';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Status' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The status of the feedback/issue, submitted/open/open-in progress/closed/closed-cannot reproduce/closed-works as designed' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Status';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'SubmittedById' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The person submitting the feedback/issue' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'SubmittedById';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'TargetVersion' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The version of the system the feedback/issue was released in' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'TargetVersion';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Type' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The type of issue/feedback, bug/feature-change/feature-request.  Where bugs are issues that cause a problem, feature-change is when the request is to change an existing feature, and feature-request is when the request is to add a new feature' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Type';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'UpdatedById' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'A FK from [ZGWSecurity].[Accounts].[AccountSeqId].' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'UpdatedById';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'VerifiedById' AND [object_id] = OBJECT_ID('[ZGWOptional].[Feedbacks]')))
    EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'A FK from [ZGWSecurity].[Accounts].[AccountSeqId].' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'VerifiedById';
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
        , [Action] = (SELECT TOP(1) [Action] FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = [FunctionSeqId])
        , [Assignee] = (SELECT TOP(1) [Account] FROM [ZGWSecurity].[Accounts] WHERE [AccountSeqId] = [AssigneeId])
        , [AssigneeId]
        , [Date_Closed]
        , [Date_Opened]
        , [Details]
        , [Found_In_Version]
        , [FunctionSeqId]
        , [Notes]
        , [Severity]
        , [Status]
        , [SubmittedBy] = (SELECT TOP(1) [Account] FROM [ZGWSecurity].[Accounts] WHERE [AccountSeqId] = [SubmittedById])
        , [SubmittedById]
        , [TargetVersion]
        , [Type]
        , [UpdatedBy] = (SELECT TOP(1) [Account] FROM [ZGWSecurity].[Accounts] WHERE [AccountSeqId] = [UpdatedById])
        , [UpdatedById]
        , [VerifiedBy] = (SELECT TOP(1) [Account] FROM [ZGWSecurity].[Accounts] WHERE [AccountSeqId] = [VerifiedById])
        , [VerifiedById]
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
          FB.[FeedbackId]
        , [Action] = FUN.[Action]
        , [Assignee] = [Assignee].[Account]
        , FB.[AssigneeId]
        , FB.[Date_Closed]
        , FB.[Date_Opened]
        , FB.[Details]
        , FB.[Found_In_Version]
        , FB.[FunctionSeqId]
        , FB.[Notes]
        , FB.[Severity]
        , FB.[Status]
        , [SubmittedBy] = [Submitted].[Account]
        , FB.[SubmittedById]
        , FB.[TargetVersion]
        , FB.[Type]
        , [UpdatedBy] = [Updated].[Account]
        , FB.[UpdatedById]
        , [VerifiedBy] = [Verified].[Account]
        , FB.[VerifiedById]
    FROM 
        [ZGWOptional].[Feedbacks] AS FB
		LEFT JOIN [ZGWSecurity].[Functions] AS FUN ON
			FB.[FunctionSeqId] = FUN.[FunctionSeqId]

		LEFT JOIN [ZGWSecurity].[Accounts] AS Assignee ON
			FB.[AssigneeId] = [Assignee].[AccountSeqId]

		LEFT JOIN [ZGWSecurity].[Accounts] AS Submitted ON
			FB.[SubmittedById] = [Submitted].[AccountSeqId]

		LEFT JOIN [ZGWSecurity].[Accounts] AS Updated ON
			FB.[UpdatedById] = [Updated].[AccountSeqId]

		LEFT JOIN [ZGWSecurity].[Accounts] AS Verified ON
			FB.[VerifiedById] = [Verified].[AccountSeqId]
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
        @P_FeedbackId INT = -1,
        @P_AssigneeId INT = (SELECT AccountSeqId FROM [ZGWSecurity].[Accounts] WHERE [Account] = 'System'),
        @P_Date_Closed [DATETIME] = NULL,
        @P_Date_Opened [DATETIME] = GETDATE(),
        @P_Details NVARCHAR(MAX) = 'The details',
        @P_Found_In_Version VARCHAR(32) = 'x.x.x.x',
        @P_FunctionSeqId INT = 4,
        @P_Notes NVARCHAR(MAX) = null,
        @P_Severity VARCHAR(32) = 'Needs Classification', -- ???
        @P_Status VARCHAR(32) = 'Submitted', -- Under Review, In Progress, Closed, Closed-Not Fixed, Closed-Could not Reproduce
		@P_SubmittedById INT = 1,
        @P_TargetVersion VARCHAR(32) = NULL,
        @P_Type [NVARCHAR](128) = 'Needs Classification', -- Feature Request, Bug, General, Needs Classification
        @P_UpdatedById INT = 1,
        @P_VerifiedById INT = (SELECT AccountSeqId FROM [ZGWSecurity].[Accounts] WHERE [Account] = 'Anonymous'),
		@P_Primary_Key INT = null,
        @P_Debug INT = 1;
    
    EXECUTE [ZGWOptional].[Set_Feedback]
		 @P_FeedbackId
		,@P_AssigneeId
		,@P_Date_Closed
		,@P_Date_Opened
		,@P_Details
		,@P_Found_In_Version
		,@P_FunctionSeqId
		,@P_Notes
		,@P_Severity
		,@P_Status
		,@P_SubmittedById
		,@P_TargetVersion
		,@P_Type
		,@P_UpdatedById
		,@P_VerifiedById
		,@P_Primary_Key OUTPUT
		,@P_Debug;

	SET @P_FeedbackId = @P_Primary_Key;
	PRINT '@P_FeedbackId: ' + CONVERT(VARCHAR(MAX), @P_FeedbackId);
    SET @P_FeedbackId = @P_Primary_Key
	SET @P_AssigneeId = (SELECT AccountSeqId FROM [ZGWSecurity].[Accounts] WHERE [Account] = 'Developer');
    SET @P_Details = 'Changed details';
    SET @P_Found_In_Version = '5.2.0.1023';
    SET @P_Notes = 'Notes from the developer';
    SET @P_Severity = '???';
    SET @P_Status = 'Closed-Could not Reproduce';
    SET @P_TargetVersion = '5.2.0';
    SET @P_Type = 'Feature request';
    SET @P_UpdatedById =(SELECT AccountSeqId FROM [ZGWSecurity].[Accounts] WHERE [Account] = 'Mike');
    SET @P_VerifiedById = (SELECT AccountSeqId FROM [ZGWSecurity].[Accounts] WHERE [Account] = 'Mike');

    EXECUTE [ZGWOptional].[Set_Feedback]
		 @P_FeedbackId
		,@P_AssigneeId
		,@P_Date_Closed
		,@P_Date_Opened
		,@P_Details
		,@P_Found_In_Version
		,@P_FunctionSeqId
		,@P_Notes
		,@P_Severity
		,@P_Status
		,@P_SubmittedById
		,@P_TargetVersion
		,@P_Type
		,@P_UpdatedById
		,@P_VerifiedById
		,@P_Primary_Key OUTPUT
		,@P_Debug;
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 11/21/2024
-- Description:	Inserts or updates [ZGWOptional].[Set_Feedback]
-- =============================================
ALTER PROCEDURE [ZGWOptional].[Set_Feedback]
     @P_FeedbackId INT
    ,@P_AssigneeId INT = -1
    ,@P_Date_Closed DATETIME
    ,@P_Date_Opened DATETIME
    ,@P_Details NVARCHAR(MAX)
    ,@P_Found_In_Version VARCHAR(32)
    ,@P_FunctionSeqId INT
    ,@P_Notes NVARCHAR(MAX)
    ,@P_Severity VARCHAR(32) = 'Needs Classification'
    ,@P_Status VARCHAR(32) = 'Unassigned'
    ,@P_SubmittedById INT
    ,@P_TargetVersion VARCHAR(32) = NULL
    ,@P_Type NVARCHAR(128) = 'Needs Classification'
    ,@P_UpdatedById INT
    ,@P_VerifiedById INT = 1
	,@P_Primary_Key INT OUTPUT
    ,@P_Debug INT = 0
AS
	SET NOCOUNT ON;
	IF @P_Debug = 1 PRINT 'Starting ZGWOptional.Set_Feedback';
    IF @P_VerifiedById = -1 SET @P_VerifiedById = (SELECT AccountSeqId FROM [ZGWSecurity].[Accounts] WHERE [Account] = 'Anonymous')
    DECLARE @V_Now DATETIME = GETDATE()
            , @V_FeedbackId INT = @P_FeedbackId
            , @V_AnonymousSeqId INT = (SELECT AccountSeqId FROM [ZGWSecurity].[Accounts] WHERE [Account] = 'Anonymous');

    IF @P_AssigneeId = -1 SET @P_AssigneeId = @V_AnonymousSeqId;
    IF @P_VerifiedById = -1 SET @P_VerifiedById = @V_AnonymousSeqId;
	IF CONVERT(DATETIME, '1/1/1900 12:00:00 AM') = @P_Date_Closed
		BEGIN
			SET @P_Date_Closed = NULL;
		END
	--END IF
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
        [FeedbackId],
        [AssigneeId],
        [Date_Closed],
        [Date_Opened],
        [Details],
        [Found_In_Version],
        [FunctionSeqId],
        [Notes],
        [Severity],
        [Status],
        [SubmittedById],
        [TargetVersion],
        [Type],
        [UpdatedById],
        [VerifiedById],
		[Start_Date],
		[End_Date]
    ) VALUES (
        @V_FeedbackId,
        @P_AssigneeId,
        @P_Date_Closed,
        @P_Date_Opened,
        @P_Details,
        @P_Found_In_Version,
        @P_FunctionSeqId,
        @P_Notes,
        @P_Severity,
        @P_Status,
        @P_SubmittedById,
        @P_TargetVersion,
        @P_Type,
        @P_UpdatedById,
        @P_VerifiedById,
        @V_Now,
        NULL
    )
	-- Set the return primary key
	SET @P_Primary_Key = @V_FeedbackId;
    -- Return the current record..
    SELECT
        [FeedbackId]
        ,[Action]
        ,[Assignee]
        ,[AssigneeId]
        ,[Date_Closed]
        ,[Date_Opened]
        ,[Details]
        ,[Found_In_Version]
        ,[FunctionSeqId]
        ,[Notes]
        ,[Severity]
        ,[Status]
        ,[SubmittedBy]
        ,[SubmittedById]
        ,[TargetVersion]
        ,[Type]
        ,[UpdatedBy]
        ,[UpdatedById]
        ,[VerifiedBy]
        ,[VerifiedById]
    FROM [ZGWOptional].[vwCurrentFeedbacks]
    WHERE [FeedbackId] = @V_FeedbackId

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
                [FeedbackId],
                [AssigneeId],
                [Date_Closed],
                [Date_Opened],
                [Details],
                [Found_In_Version],
                [FunctionSeqId],
                [Notes],
                [Severity],
                [Status],
                [SubmittedById],
                [TargetVersion],
                [Type],
                [UpdatedById],
                [VerifiedById],
                [Start_Date],
                [End_Date]
            )
            SELECT 
                FBs.[FeedbackId],
                FBs.[AssigneeId],
                FBs.[Date_Closed],
                FBs.[Date_Opened],
                FBs.[Details],
                FBs.[Found_In_Version],
                FBs.[FunctionSeqId],
                FBs.[Notes],
                FBs.[Severity],
                FBs.[Status],
                FBs.[SubmittedById],
                FBs.[TargetVersion],
                FBs.[Type],
                FBs.[UpdatedById],
                FBs.[VerifiedById],
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
                [FeedbackId]
                ,[Action]
                ,[Assignee]
                ,[AssigneeId]
                ,[Date_Closed]
                ,[Date_Opened]
                ,[Details]
                ,[Found_In_Version]
                ,[FunctionSeqId]
                ,[Notes]
                ,[Severity]
                ,[Status]
                ,[SubmittedBy]
                ,[SubmittedById]
                ,[TargetVersion]
                ,[Type]
                ,[UpdatedBy]
                ,[UpdatedById]
                ,[VerifiedBy]
                ,[VerifiedById]
            FROM [ZGWOptional].[vwCurrentFeedbacks]
        END
    ELSE
        BEGIN
            IF @P_Debug = 1 PRINT 'Return one record';
            SELECT
                [FeedbackId]
                ,[Action]
                ,[Assignee]
                ,[AssigneeId]
                ,[Date_Closed]
                ,[Date_Opened]
                ,[Details]
                ,[Found_In_Version]
                ,[FunctionSeqId]
                ,[Notes]
                ,[Severity]
                ,[Status]
                ,[SubmittedBy]
                ,[SubmittedById]
                ,[TargetVersion]
                ,[Type]
                ,[UpdatedBy]
                ,[UpdatedById]
                ,[VerifiedBy]
                ,[VerifiedById]
            FROM [ZGWOptional].[vwCurrentFeedbacks]
            WHERE FeedbackId = @P_FeedbackId
        END
    --END IF

RETURN 0

GO
/****** End: Procedure [ZGWOptional].[Get_Feedback] ******/
/****** Start: Adding Feedback Actions ******/
DECLARE @V_FunctionSeqId INT = -1
    ,@V_Name VARCHAR(30) = 'Manage Feedbacks'
    ,@V_Description VARCHAR(512) = 'Search feedbacks/issues for editing'
    ,@V_FunctionTypeSeqId INT = 1
    ,@V_Source VARCHAR(512) = ''
    ,@V_Controller VARCHAR(512) = ''
    ,@V_Resolve VARCHAR(MAX) = ''
    ,@V_Enable_View_State INT = 0
    ,@V_Enable_Notifications INT = 0
    ,@V_Redirect_On_Timeout INT = 0
    ,@V_Is_Nav INT = 1
    ,@V_Link_Behavior INT = 1
    ,@V_NO_UI INT = 0
    ,@V_NAV_TYPE_ID INT = 3
    ,@V_Action VARCHAR(256) = 'feedbacks'
    ,@V_Meta_Key_Words VARCHAR(512) = ''
    ,@V_ParentSeqId INT = (SELECT TOP(1) [FunctionSeqId] FROM [ZGWSecurity].[Functions] WHERE [Action] = 'SystemAdministration')
    ,@V_Notes VARCHAR(512) = 'Used to search feedbacks/issues for editing'
    ,@V_Debug INT = 0
    ,@V_SystemID INT = 1
    ,@V_ViewPermission INT;

IF NOT EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [Action] = 'feedbacks')
    BEGIN
        PRINT 'Adding Manage Feedbacks';

        EXEC ZGWSecurity.Set_Function 
              @V_FunctionSeqId       -- FunctionSeqId (-1 indicates new record)
            , @V_Name                -- Name
            , @V_Description         -- Description
            , @V_FunctionTypeSeqId   -- FunctionTypeSeqId
            , @V_Source              -- Source
            , @V_Controller          -- Controller
            , NULL                   -- Resolve
            , 0                      -- Enable_View_State
            , 0                      -- Enable_Notifications
            , 0                      -- Redirect_On_Timeout
            , 1                      -- Is_Nav
            , 1                      -- Link_Behavior (Internal)
            , 0                      -- NO_UI
            , 3                      -- NVP_DetailSeqId (Hierarchical)
            , @V_Action              -- Action
            , @V_META_KEY_WORDS      -- Meta_Key_Words
            , @V_ParentSeqId         -- ParentSeqId
            , @V_Notes               -- Notes
            , @V_SystemID            -- Added_By
            , @V_Debug               -- Debug flag
        
        UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 5 WHERE [Action] = @V_Action;

        SET @V_FunctionSeqId = (SELECT FunctionSeqId FROM [ZGWSecurity].[Functions] WHERE action = @V_Action);

        SET @V_ViewPermission = (SELECT NVP_DetailSeqId FROM [ZGWSecurity].[Permissions] WHERE NVP_Detail_Value = 'View');

        EXEC [ZGWSecurity].[Set_Function_Roles] 
            @V_FunctionSeqId   -- FunctionSeqId
            ,1                  -- SecurityEntitySeqId
            ,'Developer'        -- Roles
            ,@V_ViewPermission  -- PermissionsNVPDetailSeqId
            ,@V_SystemID        -- AccountSeqId for the 'System Administrator'
            ,@V_Debug;          -- Debug flag
    END
--END IF
IF NOT EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [Action] LIKE 'feedbacks/edit')
    BEGIN
        PRINT 'Edit Feedback/Issue';

        SET @V_FunctionSeqId = -1;
        SET @V_Name = 'Edit Feedback';
        SET @V_Description = 'Edit Feedback/Issue';
        SET @V_FunctionTypeSeqId = 1;
        SET @V_Is_Nav = 0;
        SET @V_Action = 'feedbacks/edit';
        SET @V_ParentSeqId = (SELECT TOP(1) [FunctionSeqId] FROM [ZGWSecurity].[Functions] WHERE [Action] = 'feedbacks');
        SET @V_Notes = 'Used to edit feedbacks/issues';
        SET @V_Debug = 0;

        EXEC ZGWSecurity.Set_Function 
              @V_FunctionSeqId       -- FunctionSeqId (-1 indicates new record)
            , @V_Name                -- Name
            , @V_Description         -- Description
            , @V_FunctionTypeSeqId   -- FunctionTypeSeqId
            , @V_Source              -- Source
            , @V_Controller          -- Controller
            , NULL                   -- Resolve
            , 0                      -- Enable_View_State
            , 0                      -- Enable_Notifications
            , 0                      -- Redirect_On_Timeout
            , 1                      -- Is_Nav
            , 1                      -- Link_Behavior (Internal)
            , 0                      -- NO_UI
            , 3                      -- NVP_DetailSeqId (Hierarchical)
            , @V_Action              -- Action
            , @V_META_KEY_WORDS      -- Meta_Key_Words
            , @V_ParentSeqId         -- ParentSeqId
            , @V_Notes               -- Notes
            , @V_SystemID            -- Added_By
            , @V_Debug               -- Debug flag

        SET @V_FunctionSeqId = (SELECT FunctionSeqId FROM [ZGWSecurity].[Functions] WHERE action = @V_Action);

        SET @V_ViewPermission = (SELECT NVP_DetailSeqId FROM [ZGWSecurity].[Permissions] WHERE NVP_Detail_Value = 'View');

        EXEC [ZGWSecurity].[Set_Function_Roles] 
            @V_FunctionSeqId   -- FunctionSeqId
            ,1                  -- SecurityEntitySeqId
            ,'Anonymous'        -- Roles
            ,@V_ViewPermission  -- PermissionsNVPDetailSeqId
            ,@V_SystemID        -- AccountSeqId for the 'System Administrator'
            ,@V_Debug;          -- Debug flag
    END
--END IF
IF NOT EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [Action] = 'feedbacks/feedback')
    BEGIN
        PRINT 'Add a Feedback/Issue';

        SET @V_FunctionSeqId = -1;
        SET @V_Name = 'Feedback';
        SET @V_Description = 'Add a Feedback/Issue';
        SET @V_FunctionTypeSeqId = 1;
        SET @V_Is_Nav = 1;
        SET @V_Action = 'feedbacks/feedback';
        SET @V_ParentSeqId = 1;
        SET @V_Notes = 'Add a Feedback/Issue';
        SET @V_Debug = 0;

        EXEC ZGWSecurity.Set_Function 
              @V_FunctionSeqId       -- FunctionSeqId (-1 indicates new record)
            , @V_Name                -- Name
            , @V_Description         -- Description
            , @V_FunctionTypeSeqId   -- FunctionTypeSeqId
            , @V_Source              -- Source
            , @V_Controller          -- Controller
            , NULL                   -- Resolve
            , 0                      -- Enable_View_State
            , 0                      -- Enable_Notifications
            , 0                      -- Redirect_On_Timeout
            , 1                      -- Is_Nav
            , 1                      -- Link_Behavior (Internal)
            , 0                      -- NO_UI
            , 1                      -- NVP_DetailSeqId (Horizontal)
            , @V_Action              -- Action
            , @V_META_KEY_WORDS      -- Meta_Key_Words
            , @V_ParentSeqId         -- ParentSeqId
            , @V_Notes               -- Notes
            , @V_SystemID            -- Added_By
            , @V_Debug               -- Debug flag

        SET @V_FunctionSeqId = (SELECT FunctionSeqId FROM [ZGWSecurity].[Functions] WHERE action = @V_Action);

        SET @V_ViewPermission = (SELECT NVP_DetailSeqId FROM [ZGWSecurity].[Permissions] WHERE NVP_Detail_Value = 'View');

        EXEC [ZGWSecurity].[Set_Function_Roles] 
            @V_FunctionSeqId   -- FunctionSeqId
            ,1                  -- SecurityEntitySeqId
            ,'Authenticated'    -- Roles
            ,@V_ViewPermission  -- PermissionsNVPDetailSeqId
            ,@V_SystemID        -- AccountSeqId for the 'System Administrator'
            ,@V_Debug;          -- Debug flag
    END
--END IF
/****** Done: Adding Feedback Actions ******/
/****** Start: Adding Test Modal Action ******/
IF NOT EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [Action] = 'testing/modal')
    BEGIN
        PRINT 'Testing Modal';

        SET @V_FunctionSeqId = -1;
        SET @V_Name = 'Test Modal';
        SET @V_Description = 'Used for testing the modal service';
        SET @V_FunctionTypeSeqId = 1;
        SET @V_Is_Nav = 1;
        SET @V_Action = '/testing/modal';
        SET @V_ParentSeqId = 12;
        SET @V_Notes = 'Testing the Modal Feature';
        SET @V_Debug = 0;

        EXEC ZGWSecurity.Set_Function 
              @V_FunctionSeqId       -- FunctionSeqId (-1 indicates new record)
            , @V_Name                -- Name
            , @V_Description         -- Description
            , @V_FunctionTypeSeqId   -- FunctionTypeSeqId
            , @V_Source              -- Source
            , @V_Controller          -- Controller
            , NULL                   -- Resolve
            , 0                      -- Enable_View_State
            , 0                      -- Enable_Notifications
            , 0                      -- Redirect_On_Timeout
            , @V_Is_Nav              -- Is_Nav
            , 1                      -- Link_Behavior (Internal)
            , 0                      -- NO_UI
            , 3                      -- NVP_DetailSeqId (Hierarchical)
            , @V_Action              -- Action
            , @V_META_KEY_WORDS      -- Meta_Key_Words
            , @V_ParentSeqId         -- ParentSeqId
            , @V_Notes               -- Notes
            , @V_SystemID            -- Added_By
            , @V_Debug               -- Debug flag

        SET @V_FunctionSeqId = (SELECT FunctionSeqId FROM [ZGWSecurity].[Functions] WHERE action = @V_Action);

        UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 13 WHERE [FunctionSeqId] = @V_FunctionSeqId

        SET @V_ViewPermission = (SELECT NVP_DetailSeqId FROM [ZGWSecurity].[Permissions] WHERE NVP_Detail_Value = 'View');

        EXEC [ZGWSecurity].[Set_Function_Roles] 
            @V_FunctionSeqId   -- FunctionSeqId
            ,1                  -- SecurityEntitySeqId
            ,'Developer'        -- Roles
            ,@V_ViewPermission  -- PermissionsNVPDetailSeqId
            ,@V_SystemID        -- AccountSeqId for the 'System Administrator'
            ,@V_Debug;          -- Debug flag
    END
--END IF
/****** Done: Adding Test Modal Action ******/
-- Update the sort order
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 13)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 0 WHERE [FunctionSeqId] = 13;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 24)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 24;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 31)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 2 WHERE [FunctionSeqId] = 31;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 76)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 3 WHERE [FunctionSeqId] = 76;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 77)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 77;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 78)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 78;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 85)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 1 WHERE [FunctionSeqId] = 85;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 17)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 2 WHERE [FunctionSeqId] = 17;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 23)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 3 WHERE [FunctionSeqId] = 23;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 20)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 4 WHERE [FunctionSeqId] = 20;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 67)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 5 WHERE [FunctionSeqId] = 67;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 16)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 6 WHERE [FunctionSeqId] = 16;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 79)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 7 WHERE [FunctionSeqId] = 79;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 25)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 8 WHERE [FunctionSeqId] = 25;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 26)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 9 WHERE [FunctionSeqId] = 26;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 64)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 10 WHERE [FunctionSeqId] = 64;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 27)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 11 WHERE [FunctionSeqId] = 27;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 28)
    UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 12 WHERE [FunctionSeqId] = 28;
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 88)
    UPDATE [ZGWSecurity].[Functions] SET 
        [Action] = '/testing/logging',
        [Sort_Order] = 13
    WHERE [FunctionSeqId] = 88;

IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 28)
    UPDATE [ZGWSecurity].[Functions] SET [Action] = '/logging/SetLogLevel' WHERE FunctionSeqId = 28

IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 16)
    UPDATE [ZGWSecurity].[Functions] SET [Action] = 'securityEntity' WHERE FunctionSeqId = 16
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 63)
    UPDATE [ZGWSecurity].[Functions] SET [Action] = '/securityEntity/SelectASecurityEntity' WHERE FunctionSeqId = 63

UPDATE 
	[ZGWSecurity].[Functions] 
SET [Source] = 'https://127.0.0.1:44455/swagger/index.html' 
WHERE [FunctionSeqId] = (SELECT FunctionSeqId FROM [ZGWSecurity].[Functions] WHERE [Action] = 'SwaggerAPI')

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '5.2.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;