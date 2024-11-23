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
-- Add constraints
ALTER TABLE [ZGWOptional].[Feedbacks] ADD CONSTRAINT [DF_ZGWOptional_Feedbacks_Start_Date] DEFAULT (getdate()) FOR [Start_Date]
ALTER TABLE [ZGWOptional].[Feedbacks] WITH CHECK ADD CONSTRAINT [FK_Accounts_Feedbacks_Asignee] FOREIGN KEY([Assignee])
    REFERENCES [ZGWSecurity].[Accounts] ([AccountSeqId])
    ON UPDATE NO ACTION
    ON DELETE NO ACTION

ALTER TABLE [ZGWOptional].[Feedbacks] WITH CHECK ADD CONSTRAINT [FK_Accounts_Feedbacks_VerifiedBy] FOREIGN KEY([VerifiedBy])
    REFERENCES [ZGWSecurity].[Accounts] ([AccountSeqId])
    ON UPDATE NO ACTION
    ON DELETE NO ACTION

-- Add extended properties
EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'An "inline" history table of feedbacks/issues. The row idenifier for the table is the combination of FeedbackId + Start_Date + End_Date!  The reason for this table is to provide an example of an "inline" history table, not, to provide a comprehensive feedback/issue tracking system' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks';
EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'A unique identifier for the feedback/issue (not the row).' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'FeedbackId';
EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'A FK from [ZGWSecurity].[Accounts].[AccountSeqId].' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Assignee';
EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The details of the feedback/issue from the submitter' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Details';
EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The version of the system the feedback/issue was found in' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'FoundInVersion';
EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'Notes from the developer' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Notes';
EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'TheDescriptionHere' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Severity';
EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The status of the feedback/issue, submitted/open/open-in progress/closed/closed-cannot reproduce/closed-works as designed' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Status';
EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The version of the system the feedback/issue was released in' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'TargetVersion';
EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The type of issue/feedback, bug/feature-change/feature-request.  Where bugs are issues that cause a problem, feature-change is when the request is to change an existing feature, and feature-request is when the request is to add a new feature' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Type';
EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'A FK from [ZGWSecurity].[Accounts].[AccountSeqId].' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'VerifiedBy';
EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The date the record was created in the database.  Used in conjunction with End_Date and FeedbackId in order to maintain an "inline" history. The oldest Start_Date is when the feedback/issue was created.' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'Start_Date';
EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The date the record became "history".  Used in conjunction with Start_Date and FeedbackId in order to maintain an "inline" history.  When the End_Date is null the record is current.  When the End_Date is not null the record is either a historical record or the feedback/issue has been "deleted" (the Start_Date should be the same value).' , @level0type=N'SCHEMA',@level0name=N'ZGWOptional', @level1type=N'TABLE',@level1name=N'Feedbacks', @level2type=N'COLUMN',@level2name=N'End_Date';
