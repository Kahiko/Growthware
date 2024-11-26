CREATE VIEW ZGWOptional.vwHistoryFeedback AS
    SELECT 
          [FeedbackId]
        , [AssigneeId] = [Assignee]
        , [Assignee] = (SELECT [Account] FROM [ZGWSecurity].[Accounts] WHERE [AccountSeqId] = [Assignee])
        , [Details]
        , [FoundInVersion]
        , [Notes]
        , [Severity]
        , [Status]
        , [TargetVersion]
        , [Type]
        , [VerifiedById] = [VerifiedBy]
        , [VerifiedBy] = (SELECT [Account] FROM [ZGWSecurity].[Accounts] WHERE [AccountSeqId] = [VerifiedBy])
        , [Start_Date]
        , [End_Date]
    FROM 
        [ZGWOptional].[Feedbacks];