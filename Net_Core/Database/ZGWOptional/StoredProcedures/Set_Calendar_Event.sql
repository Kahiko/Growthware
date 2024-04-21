/*
USAGE:
	DECLARE
		  @P_CalendarEventSeqId	INT				= -1
		, @P_FunctionSeqId		INT				= 1
		, @P_Title				VARCHAR(255)	= 'Fake meeting with me ;-)'
		, @P_Start				DATETIME		= CONVERT(VARCHAR, '2/2/24 00:00', 108)
		, @P_End				DATETIME		= CONVERT(VARCHAR, '2/2/24 00:00', 108)
		, @P_AllDay				BIT				= 0
		, @P_Description		VARCHAR(512)	= 'Fake meeting with me ;-) so I can test'
		, @P_Color				VARCHAR(20)		= '#ff0000' -- red
		, @P_Link				VARCHAR(255)	= ''
		, @P_Location			VARCHAR(255)	= 'My office?'
		, @P_Added_Updated_By	INT				= 3

	EXEC ZGWOptional.Set_Calendar_Event
		  @P_CalendarEventSeqId
		, @P_FunctionSeqId
		, @P_Title
		, @P_Start
		, @P_End
		, @P_AllDay
		, @P_Description
		, @P_Color
		, @P_Link
		, @P_Location
		, @P_Added_Updated_By
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 03/06/2024
-- Description:	Calendar Data
-- =============================================
CREATE PROCEDURE [ZGWOptional].[Set_Calendar_Event]
	  @P_CalendarEventSeqId	INT
	, @P_FunctionSeqId		INT
	, @P_Title				VARCHAR(255)
	, @P_Start				DATETIME
	, @P_End				DATETIME
	, @P_AllDay				BIT
	, @P_Description		VARCHAR(512)
	, @P_Color				VARCHAR(20)
	, @P_Link				VARCHAR(255)
	, @P_Location			VARCHAR(255)
	, @P_Added_Updated_By	INT
AS
	SET NOCOUNT ON;
	DECLARE @V_CalendarSeqId INT = (SELECT [CalendarSeqId] FROM [ZGWOptional].[Calendars] WHERE [FunctionSeqId] = @P_FunctionSeqId);
	IF @P_CalendarEventSeqId = -1
		BEGIN
			-- PRINT 'Insert new';
			INSERT INTO [ZGWOptional].[Calendar_Events] (
				 [CalendarSeqId]
				,[Title]
				,[Start]
				,[End]
				,[AllDay]
				,[Description]
				,[Color]
				,[Link]
				,[Location]
				,[Added_By]
				,[Added_Date]
			) VALUES (
				  @V_CalendarSeqId
				, @P_Title
				, @P_Start
				, @P_End
				, @P_AllDay
				, @P_Description
				, @P_Color
				, @P_Link
				, @P_Location
				, @P_Added_Updated_By
				, GETDATE()
			);
			SELECT @P_CalendarEventSeqId=SCOPE_IDENTITY();
		END
	ELSE
		BEGIN
			-- PRINT 'Update existing';
			UPDATE [ZGWOptional].[Calendar_Events] SET
				  [Title] 		= @P_Title
				, [Start]		= @P_Start
				, [End]			= @P_End
				, [AllDay]		= @P_AllDay
				, [Description]	= @P_Description
				, [Color]		= @P_Color
				, [Link]		= @P_Link
				, [Location] 	= @P_Location
				, [Updated_By] 	= @P_Added_Updated_By
				, [Updated_Date] = GETDATE()
			WHERE [CalendarEventSeqId] = @P_CalendarEventSeqId;
		END
	--END IF
	EXEC ZGWOptional.Get_Calendar_Event @P_CalendarEventSeqId
	SET NOCOUNT OFF;

RETURN 0
GO