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
		  [AllDay]
		, [CalendarEventSeqId]
		, [CalendarSeqId]
		, [Color]
		, [Description]
		, [End]
		, [Link]
		, [Owner] = (SELECT TOP(1) [Owner] = SUBSTRING ([First_Name], 1, 1) + '. ' + [Last_Name] FROM [ZGWSecurity].[Accounts] WHERE [AccountSeqId] = [Added_By])
		, [Title]
		, [Start]
		, [Location]
		, [Added_By]
		, [Added_Date]
		, [Updated_By]
		, [Updated_Date]
	FROM 
		[ZGWOptional].[Calendar_Events]
	WHERE 
		[CalendarEventSeqId] = @P_CalendarEventSeqId
	SET NOCOUNT OFF;

RETURN 0
GO