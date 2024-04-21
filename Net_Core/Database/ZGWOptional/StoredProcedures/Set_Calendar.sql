/*
Usage:
	DECLARE 
	  @P_CalendarSeqId INT  = -1,
	  @P_SecurityEntitySeqId [int] = (SELECT TOP(1) [SecurityEntitySeqId] FROM [ZGWSecurity].[Security_Entities] WHERE [Name] = 'System'),
	  @P_FunctionSeqId [int] = (SELECT TOP(1) FROM [ZGWSecurity].[Functions] WHERE [Action] = 'CommunityCalendar')
	  @P_Comment [varchar](100) = 'Created for the community calendar',
	  @P_Active [int] = 1,
	  @P_Added_Updated_By [int] = (SELECT TOP(1) [AccountSeqId] FROM [ZGWSecurity].[Accounts] WHERE [Account] = 'System');

	exec ZGWOptional.Set_Calendar
	  @P_CalendarSeqId OUTPUT,
	  @P_SecurityEntitySeqId,
	  @P_FunctionSeqId,
	  @P_Comment,
	  @P_Active,
	  @P_Added_Updated_By;

    PRINT @P_CalendarSeqId;
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 03/01/2024
-- Description:	Calendar Data
-- =============================================
CREATE PROCEDURE [ZGWOptional].[Set_Calendar]
    @P_CalendarSeqId INT OUTPUT,
    @P_SecurityEntitySeqId [int],
    @P_FunctionSeqId [int],
    @P_Comment [varchar](100),
    @P_Active [int],
    @P_Added_Updated_By [int]
AS
	SET NOCOUNT ON;
	IF @P_CalendarSeqId > 0
		BEGIN
			UPDATE [ZGWOptional].[Calendars] SET
			  [Comment] = @P_Comment
			, [Active] = @P_Active
			, [Updated_By] = @P_Added_Updated_By
			, [Updated_Date] = GETDATE()
			WHERE [CalendarSeqId] = @P_CalendarSeqId;
		END
	ELSE
		BEGIN
			INSERT INTO [ZGWOptional].[Calendars] (
				  [SecurityEntitySeqId]
				, [FunctionSeqId]
				, [Comment]
				, [Active]
				, [Added_By]
				, [Added_Date]
			) VALUES (
				  @P_SecurityEntitySeqId
				, @P_FunctionSeqId
				, @P_Comment
				, @P_Active
				, @P_Added_Updated_By
				, GETDATE()
			);
			SELECT @P_CalendarSeqId=SCOPE_IDENTITY();
		END
	--END IF

	SET NOCOUNT OFF;

RETURN 0
GO