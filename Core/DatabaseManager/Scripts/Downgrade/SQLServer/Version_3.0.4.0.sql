-- Downgrade
SET NOCOUNT ON;

/****** Start:  Procedure [ZGWSystem].[Get_Calendar_Data] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWOptional].[Get_Calendar_Data]') AND type in (N'P', N'PC'))
	BEGIN
		DROP PROCEDURE ZGWOptional.Get_Calendar_Data;
	END
--End If

-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '3.0.1.0',
    [Updated_By] = null,
    [Updated_Date] = null