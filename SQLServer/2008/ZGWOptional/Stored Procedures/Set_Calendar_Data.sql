/*
Usage:
	DECLARE 
		  @P_Security_Entity_SeqID	INT				= 1
		, @P_Calendar_Name			NVARCHAR(50)	= 'community calendar'
		, @P_Comment				NVARCHAR(100)	= 'sysadmin test'
		, @P_EntryDate 				SMALLDATETIME	= CONVERT(SMALLDATETIME, (SELECT FORMAT(GETDATE(), 'yyyyMMdd')) + ' 00:00:00')
		, @P_AddUpd_By				INT				= 3 -- Developer
		, @P_Debug					INT = 0

	exec ZGWOptional.Set_Calendar_Data
		  @P_Security_Entity_SeqID
		, @P_Calendar_Name
		, @P_Comment
		, @P_EntryDate
		, @P_AddUpd_By
		, @P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/25/2011
-- Description:	Inserts or updates ZGWOptional.Calendars
CREATE PROCEDURE [ZGWOptional].[Set_Calendar_Data]
	  @P_Security_Entity_SeqID	INT
	, @P_Calendar_Name			NVARCHAR(50)
	, @P_Comment				NVARCHAR(100)
	, @P_EntryDate 				SMALLDATETIME
	, @P_AddUpd_By				INT
	, @P_Debug					INT = 0 
AS
BEGIN
	DECLARE @V_Active AS BIT
	SELECT @V_Active = 1
	IF (SELECT COUNT(*) FROM [ZGWOptional].[Calendars]
		WHERE 
			[Security_Entity_SeqID] = @P_Security_Entity_SeqID AND
			[Calendar_Name] = @P_Calendar_Name AND
			[Entry_Date] = @P_EntryDate AND
			[Comment] = @P_Comment
	) > 0
		BEGIN -- DO AN UPDATE
			IF @P_Debug = 1 PRINT 'Updating'
			UPDATE
				[ZGWOptional].[Calendars]
			SET
				  [Active] = @V_Active
				, [Updated_By] =  @P_AddUpd_By
				, [Updated_Date] = GETDATE()
			WHERE
				[Security_Entity_SeqID] = @P_Security_Entity_SeqID
				AND [Calendar_Name]		= @P_Calendar_Name
				AND [Entry_Date]		= @P_EntryDate
				AND [Comment]			= @P_Comment
		END
	ELSE
		BEGIN -- TRY TO INSERT
			IF @P_Debug = 1 PRINT 'Inserting'
			INSERT INTO [ZGWOptional].[Calendars](
				[Security_Entity_SeqID],
				[Calendar_Name],
				[Entry_Date],
				[Comment],
				[Active],
				[Added_By],
				[Added_Date]
			)
			VALUES(
				@P_Security_Entity_SeqID,
				@P_Calendar_Name,
				@P_EntryDate,
				@P_Comment,
				@V_Active,
				@P_AddUpd_By,
				GETDATE()
			)
		END
	IF @@ERROR = 0 RETURN 1 ELSE RETURN @@ERROR
END