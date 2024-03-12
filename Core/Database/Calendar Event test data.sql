DECLARE
	  @P_CalendarEventSeqId	INT				= -1
	, @P_CalendarSeqId		INT				= 1
	, @P_Title				VARCHAR(255)	= 'Fake meeting with me ;-)'
	, @P_Start				DATETIME		= CONVERT(VARCHAR, '3/3/24 08:00', 108)
	, @P_End				DATETIME		= CONVERT(VARCHAR, '3/3/24 09:00', 108)
	, @P_AllDay				BIT				= 0
	, @P_Description		VARCHAR(512)	= 'Fake meeting with me ;-) so I can test'
	, @P_Color				VARCHAR(20)		= '#ff0000' -- red
	, @P_Link				VARCHAR(255)	= ''
	, @P_Location			VARCHAR(255)	= 'My office?'
	, @P_Added_Updated_By	INT				= 4

EXEC ZGWOptional.Set_Calendar_Event
	  @P_CalendarEventSeqId
	, @P_CalendarSeqId
	, @P_Title
	, @P_Start
	, @P_End
	, @P_AllDay
	, @P_Description
	, @P_Color
	, @P_Link
	, @P_Location
	, @P_Added_Updated_By

SET @P_Start	= CONVERT(VARCHAR, '3/3/24 09:00', 108)
SET @P_End		= CONVERT(VARCHAR, '3/3/24 10:00', 108)
SET @P_Color	= '#6495ED' -- CornflowerBlue
SET @P_Added_Updated_By = 3;
EXEC ZGWOptional.Set_Calendar_Event @P_CalendarEventSeqId, @P_CalendarSeqId, @P_Title, @P_Start, @P_End, @P_AllDay, @P_Description, @P_Color, @P_Link, @P_Location, @P_Added_Updated_By;
SET @P_Start	= CONVERT(VARCHAR, '3/3/24 10:00', 108)
SET @P_End		= CONVERT(VARCHAR, '3/3/24 11:00', 108)
SET @P_Color	= '#6495ED' -- CornflowerBlue
EXEC ZGWOptional.Set_Calendar_Event @P_CalendarEventSeqId, @P_CalendarSeqId, @P_Title, @P_Start, @P_End, @P_AllDay, @P_Description, @P_Color, @P_Link, @P_Location, @P_Added_Updated_By;
SET @P_Start	= CONVERT(VARCHAR, '3/3/24 11:00', 108)
SET @P_End		= CONVERT(VARCHAR, '3/3/24 12:00', 108)
SET @P_Color	= '#ff0000' -- red
EXEC ZGWOptional.Set_Calendar_Event @P_CalendarEventSeqId, @P_CalendarSeqId, @P_Title, @P_Start, @P_End, @P_AllDay, @P_Description, @P_Color, @P_Link, @P_Location, @P_Added_Updated_By;
SET @P_Start	= CONVERT(VARCHAR, '3/3/24 13:00', 108)
SET @P_End		= CONVERT(VARCHAR, '3/3/24 14:00', 108)
SET @P_Color	= '#6495ED' -- CornflowerBlue
EXEC ZGWOptional.Set_Calendar_Event @P_CalendarEventSeqId, @P_CalendarSeqId, @P_Title, @P_Start, @P_End, @P_AllDay, @P_Description, @P_Color, @P_Link, @P_Location, @P_Added_Updated_By;
SET @P_Start	= CONVERT(VARCHAR, '3/3/24 15:00', 108)
SET @P_End		= CONVERT(VARCHAR, '3/3/24 16:00', 108)
SET @P_Color	= '#6495ED' -- CornflowerBlue
EXEC ZGWOptional.Set_Calendar_Event @P_CalendarEventSeqId, @P_CalendarSeqId, @P_Title, @P_Start, @P_End, @P_AllDay, @P_Description, @P_Color, @P_Link, @P_Location, @P_Added_Updated_By;
SET @P_Start	= CONVERT(VARCHAR, '3/3/24 16:00', 108)
SET @P_End		= CONVERT(VARCHAR, '3/3/24 17:00', 108)
SET @P_Color	= '#6495ED' -- CornflowerBlue
EXEC ZGWOptional.Set_Calendar_Event @P_CalendarEventSeqId, @P_CalendarSeqId, @P_Title, @P_Start, @P_End, @P_AllDay, @P_Description, @P_Color, @P_Link, @P_Location, @P_Added_Updated_By;
