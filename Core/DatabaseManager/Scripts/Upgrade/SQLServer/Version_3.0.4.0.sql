-- Upgrade
SET NOCOUNT ON;

/****** Start: Procedure [ZGWOptional].[Get_Calendar_Data] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWOptional].[Get_Calendar_Data]') AND type in (N'P', N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWOptional].[Get_Calendar_Data] AS'
	END
--End If

GO

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
ALTER PROCEDURE [ZGWOptional].[Get_Calendar_Data]
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
/****** End: Procedure [ZGWOptional].[Get_Calendar_Data] ******/

-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '3.0.4.0',
    [Updated_By] = 3,
    [Updated_Date] = getdate()