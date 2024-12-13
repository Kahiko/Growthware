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
CREATE PROCEDURE [ZGWOptional].[Delete_Feedback]
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