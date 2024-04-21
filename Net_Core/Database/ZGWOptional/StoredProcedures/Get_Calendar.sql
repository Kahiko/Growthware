/*
Usage:
	DECLARE 
	  @P_CalendarSeqId INT  = 1

	exec ZGWOptional.Get_Calendar
	  @P_CalendarSeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 03/06/2024
-- Description:	Calendar Data
-- =============================================
CREATE PROCEDURE [ZGWOptional].[Get_Calendar]
    @P_CalendarSeqId INT
AS
	SET NOCOUNT ON;
	IF @P_CalendarSeqId > 0
		BEGIN
			SELECT 
				 [CalendarSeqId]
				,[SecurityEntitySeqId]
				,[FunctionSeqId]
				,[Comment]
				,[Active]
				,[Added_By]
				,[Added_Date]
				,[Updated_By]
				,[Updated_Date]
			FROM [ZGWOptional].[Calendars] WHERE [CalendarSeqId] = @P_CalendarSeqId;
		END
	ELSE
		BEGIN
			PRINT 'Not implemented as of yet, unsure if we need, should be using search';
		END
	--END IF

	SET NOCOUNT OFF;

RETURN 0
GO