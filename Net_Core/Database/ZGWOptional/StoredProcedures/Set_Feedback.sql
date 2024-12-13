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
CREATE PROCEDURE [ZGWOptional].[Set_Feedback]
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