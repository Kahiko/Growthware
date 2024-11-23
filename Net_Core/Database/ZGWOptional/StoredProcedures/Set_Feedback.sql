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
CREATE PROCEDURE [ZGWOptional].[Set_Feedback]
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