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
CREATE PROCEDURE [ZGWOptional].[Get_Feedback]
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