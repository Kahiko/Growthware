/*
Usage:
	DECLARE 
        @P_CalendarEventSeqId INT = 1

	exec ZGWOptional.Get_Calendar_Event
		@P_CalendarSeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 03/11/2024
-- Description:	Calendar Data
-- =============================================
CREATE PROCEDURE [ZGWOptional].[Get_Calendar_Event]
    @P_CalendarEventSeqId INT
AS
	SET NOCOUNT ON;
	SELECT
		  CE.[AllDay]
		, CE.[CalendarEventSeqId]
		, CE.[CalendarSeqId]
		, CE.[Color]
		, CE.[Description]
		, CE.[End]
		, CE.[Link]
		, [Owner] = SUBSTRING ([ACCT].[First_Name], 1, 1) + '. ' + [ACCT].[Last_Name]
		, CE.[Title]
		, CE.[Start]
		, CE.[Location]
		, CE.[Added_By]
		, CE.[Added_Date]
		, CE.[Updated_By]
		, CE.[Updated_Date]
	FROM 
		[ZGWOptional].[Calendar_Events] CE
		LEFT JOIN [ZGWSecurity].[Accounts] ACCT ON 
			ACCT.[AccountSeqId] = CE.[Added_By]
	WHERE 
		[CalendarEventSeqId] = @P_CalendarEventSeqId
	SET NOCOUNT OFF;

RETURN 0
GO