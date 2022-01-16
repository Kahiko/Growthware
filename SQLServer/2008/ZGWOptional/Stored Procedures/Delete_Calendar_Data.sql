/*
Usage:
	DECLARE 
		  @P_Security_Entity_SeqID	INT				= 1
		, @P_Calendar_Name			NVARCHAR(50)	= 'community calendar'
		, @P_Comment				NVARCHAR(100)	= 'sysadmin test'
		, @P_EntryDate 				SMALLDATETIME	= CONVERT(SMALLDATETIME, (SELECT FORMAT(GETDATE(), 'yyyyMMdd')) + ' 00:00:00')
		, @P_AddUpd_By				INT				= 3 -- Developer

	exec ZGWOptional.Delete_Calendar_Data
		  @P_Security_Entity_SeqID
		, @P_Comment
		, @P_EntryDate
		, @P_AddUpd_By
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/25/2011
-- Description:	Inserts or updates ZGWOptional.Calendars
CREATE PROCEDURE [ZGWOptional].[Delete_Calendar_Data]
	@P_Security_Entity_SeqID	INT,
	@P_Calendar_Name			NVARCHAR(50),
	@P_Comment					NVARCHAR(100),
	@P_EntryDate 				SMALLDATETIME,
	@P_AddUpd_By				INT
AS
BEGIN
	UPDATE
		[ZGWOptional].[Calendars]
	SET
		  ACTIVE = 0
		, [Updated_By] = @P_AddUpd_By
		, [Updated_Date] = GETDATE()
	WHERE
			Security_Entity_SeqID = @P_Security_Entity_SeqID
		AND [Calendar_Name] = @P_Calendar_Name
		AND [Comment] = @P_Comment
		AND [Entry_Date] = @P_EntryDate
END