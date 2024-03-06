/*
Usage:
	DECLARE 
		@P_CalendarSeqId INT = 1

	exec ZGWOptional.Delete_Calendar
		@P_CalendarSeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 03/01/2024
-- Description:	Calendar Data
-- =============================================
CREATE PROCEDURE [ZGWOptional].[Delete_Calendar]
      @P_CalendarSeqId INT
AS
	SET NOCOUNT ON;
	IF EXISTS (SELECT 1 FROM [ZGWOptional].[Calendars] WHERE [CalendarSeqId] = @P_CalendarSeqId)
		BEGIN
			DELETE FROM [ZGWOptional].[Calendars] WHERE [CalendarSeqId] = @P_CalendarSeqId;
		END
	ELSE
		BEGIN
			PRINT 'Not found'
		END
	--END IF
	SET NOCOUNT OFF;

RETURN 0
GO