CREATE VIEW ZGWOptional.vwCurrentFeedbacks AS
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
    FROM 
        [ZGWOptional].[Feedbacks]
    WHERE 
        [End_Date] IS NULL;