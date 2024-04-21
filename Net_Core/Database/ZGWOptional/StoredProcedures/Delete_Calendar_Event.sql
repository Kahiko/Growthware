/*
USAGE:
	DECLARE
		  @P_CalendarEventSeqId	INT = 1;

	EXEC ZGWOptional.Delete_Calendar_Event
		  @P_CalendarEventSeqId;
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 03/06/2024
-- Description:	Delete a calendar event
-- =============================================
CREATE PROCEDURE [ZGWOptional].[Delete_Calendar_Event]
	@P_CalendarEventSeqId	INT
AS
	SET NOCOUNT ON;
	IF EXISTS (SELECT TOP(1) 1 FROM [ZGWOptional].[Calendar_Events] WHERE [CalendarEventSeqId] = @P_CalendarEventSeqId)
		BEGIN
			DELETE FROM [ZGWOptional].[Calendar_Events] WHERE [CalendarEventSeqId] = @P_CalendarEventSeqId;
		END
	--END IF
	SET NOCOUNT OFF;

RETURN 0
GO