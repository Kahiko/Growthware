CREATE VIEW ZGWOptional.vwCurrentFeedbacks AS
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