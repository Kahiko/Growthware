/*
Usage:
	DECLARE 
          @P_SecurityEntitySeqId INT = 1
        , @P_Calendar_Name VARCHAR(50) = ''
        , @P_Start_Date SMALLDATETIME = CONVERT(VARCHAR, '2/2/24 00:00', 108)
        , @P_End_Date SMALLDATETIME = CONVERT(VARCHAR, '2/29/24 00:00', 108)

	exec ZGWOptional.Get_Calendar_Data
          @P_SecurityEntitySeqId
        , @P_Calendar_Name
        , @P_Start_Date
        , @P_End_Date
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 02/26/2024
-- Description:	Calendar Data
-- =============================================
CREATE PROCEDURE [ZGWOptional].[Get_Calendar_Data]
      @P_SecurityEntitySeqId INT
    , @P_Calendar_Name VARCHAR(50)
    , @P_Start_Date SMALLDATETIME
    , @P_End_Date SMALLDATETIME
AS
	SET NOCOUNT ON;
    SELECT
        [SecurityEntitySeqId]
        ,[Calendar_Name]
        ,[Entry_Date]
        ,[Comment]
        ,[Active]
        ,[Added_By]
        ,[Added_Date]
        ,[Updated_By]
        ,[Updated_Date]
    FROM
        [ZGWOptional].[Calendars]
    WHERE
        [SecurityEntitySeqId] = @P_SecurityEntitySeqId
        AND [Calendar_Name] = @P_Calendar_Name
        AND [Entry_Date] BETWEEN @P_Start_Date AND @P_End_Date;
	SET NOCOUNT OFF;

RETURN 0

GO
