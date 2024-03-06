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
		  [CalendarEventSeqId]
		, [CalendarSeqId]
		, [Title]
		, [Start]
		, [End]
		, [AllDay]
		, [Description]
		, [Color]
		, [Link]
		, [Location]
		, [AddedBy] = (SELECT TOP(1) [FullName] = [First_Name] + ' ' + [Last_Name] FROM [ZGWSecurity].[Accounts] WHERE [AccountSeqId] = [Added_By])
		, [Added_Date]
		, [UpdatedBy] = (SELECT TOP(1) [FullName] = [First_Name] + ' ' + [Last_Name] FROM [ZGWSecurity].[Accounts] WHERE [AccountSeqId] = [Updated_By])
		, [Updated_Date]
	FROM 
		[ZGWOptional].[Calendar_Events]
	WHERE 
		[CalendarSeqId] = @P_CalendarSeqId
		AND [Start] >= @P_Start_Date 
		AND [End] <= @P_End_Date;

	SET NOCOUNT OFF;

RETURN 0
GO