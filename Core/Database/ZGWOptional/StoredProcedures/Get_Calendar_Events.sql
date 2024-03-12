/*
Usage:
	DECLARE 
		@P_CalendarSeqId INT = 1
	  , @P_Start_Date SMALLDATETIME = CONVERT(VARCHAR, '2/2/24 00:00', 108)
	  , @P_End_Date SMALLDATETIME = CONVERT(VARCHAR, '2/29/24 00:00', 108)

	exec ZGWOptional.Get_Calendar_Events
		@P_CalendarSeqId
	  , @P_Start_Date
	  , @P_End_Date
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 02/26/2024
-- Description:	Calendar Data
-- =============================================
CREATE PROCEDURE [ZGWOptional].[Get_Calendar_Events]
      @P_CalendarSeqId INT
    , @P_Start_Date SMALLDATETIME
    , @P_End_Date SMALLDATETIME
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
		, [Owner] = (SELECT SUBSTRING (ACCTS.[First_Name], 1, 1) + '. ' + ACCTS.[Last_Name])
		, CE.[Title]
		, CE.[Start]
		, CE.[Location]
		, CE.[Added_By]
		, CE.[Added_Date]
		, CE.[Updated_By]
		, CE.[Updated_Date]
	FROM 
		[ZGWOptional].[Calendar_Events] CE
		LEFT JOIN [ZGWSecurity].[Accounts] ACCTS ON
			CE.[Added_By] = ACCTS.[AccountSeqId]
	WHERE 
		[CalendarSeqId] = @P_CalendarSeqId
		AND [Start] >= @P_Start_Date 
		AND [End] <= @P_End_Date;

	SET NOCOUNT OFF;

RETURN 0
GO