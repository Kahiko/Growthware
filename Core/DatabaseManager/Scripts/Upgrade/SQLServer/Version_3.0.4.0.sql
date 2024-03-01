-- Upgrade
--USE [YourDatabaseName];
GO
SET NOCOUNT ON;

/****** Start: Add columns to [ZGWOptional].[Calendars] ******/
IF COL_LENGTH('ZGWOptional.Calendars','CalendarSeqId') IS NULL
  BEGIN
    CREATE TABLE [ZGWOptional].[Calendars2] (
        [CalendarSeqId]       INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
        [SecurityEntitySeqId] INT           NOT NULL,
        [FunctionSeqId]       INT           NOT NULL,
        [Calendar_Name]       VARCHAR (50)  NOT NULL,
        [Comment]             VARCHAR (100) NOT NULL,
        [Active]              INT           NOT NULL,
        [Added_By]            INT           NOT NULL,
        [Added_Date]          DATETIME      NOT NULL,
        [Updated_By]          INT           NULL,
        [Updated_Date]        DATETIME      NULL,
		CONSTRAINT [PK_Calendars] PRIMARY KEY CLUSTERED ([CalendarSeqId] ASC),
    );

    -- TODO: We should copy the data from [ZGWOptional].[Calendars] to [ZGWOptional].[Calendars2], but,
    --       this feature has not been implemented in a very long time and I don't want to spend time on it right now.
    -- INSERT INTO [ZGWOptional].[Calendars2] SELECT * FROM [ZGWOptional].[Calendars];
    DROP TABLE [ZGWOptional].[Calendars];

	EXEC sp_rename 'ZGWOptional.Calendars2', 'Calendars';

	ALTER TABLE [ZGWOptional].[Calendars]
    ADD CONSTRAINT [FK_ZGWSecurity_Entities_ZGWOptional_Calendars] FOREIGN KEY ([SecurityEntitySeqId]) REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId]) ON DELETE CASCADE ON UPDATE CASCADE;

	ALTER TABLE [ZGWOptional].[Calendars]
    ADD CONSTRAINT [FK_ZGWSecurity_Functions_ZGWOptional_Calendars] FOREIGN KEY ([FunctionSeqId]) REFERENCES [ZGWSecurity].[Functions] ([FunctionSeqId]) ON DELETE CASCADE ON UPDATE CASCADE;

  END
--END IF
/****** End: Add columns to [ZGWOptional].[Calendars] ******/

/****** Start: Create [ZGWOptional].[Calendar_Events] ******/
IF OBJECT_ID(N'ZGWOptional.Calendar_Events', N'U') IS NULL  
  CREATE TABLE [ZGWOptional].[Calendar_Events] (
    [CalendarEventSeqId]  INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CalendarSeqId]       INT           NOT NULL,
    [Title]               VARCHAR(255)  NOT NULL,
    [Start]               DATETIME      CONSTRAINT [DF_ZGWOptional_Calendar_Events_Start_Date] DEFAULT (getdate()) NOT NULL,
    [End]                 DATETIME      CONSTRAINT [DF_ZGWOptional_Calendar_Events_End_Date] DEFAULT (getdate()) NOT NULL,
    [AllDay]              BIT           CONSTRAINT [DF_ZGWOptional_Calendar_Events_All_Day] DEFAULT (0) NOT NULL,
    [Description]         VARCHAR(512)  NULL,
    [Color]               VARCHAR(20)   NOT NULL,
    [Link]                VARCHAR(255)  NULL,
    [Location]            VARCHAR(255)  NULL,
    [Added_By]            INT           CONSTRAINT [DF_ZGWOptional_Calendar_Events_Added_By] DEFAULT ((1)) NOT NULL,
    [Added_Date]          DATETIME      CONSTRAINT [DF_ZGWOptional_Calendar_Events_Added_Date] DEFAULT (getdate()) NOT NULL,
    [Updated_By]          INT           NULL,
    [Updated_Date]        DATETIME      NULL,
    CONSTRAINT [PK_Calendar_Events] PRIMARY KEY CLUSTERED ([CalendarEventSeqId] ASC),
    CONSTRAINT [FK_ZGWOptional_Calendars_Calendar_Events] FOREIGN KEY ([CalendarSeqId]) REFERENCES [ZGWOptional].[Calendars] ([CalendarSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
  );
GO
/****** End: Create [ZGWOptional].[Calendar_Events] ******/

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
		@P_CalendarSeqId INT = 1
	  , @P_Start_Date SMALLDATETIME = CONVERT(VARCHAR, '2/2/24 00:00', 108)
	  , @P_End_Date SMALLDATETIME = CONVERT(VARCHAR, '2/29/24 00:00', 108)

	exec ZGWOptional.Get_Calendar_Data
		@P_CalendarSeqId
	  , @P_Start_Date
	  , @P_End_Date
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 02/26/2024
-- Description:	Calendar Data
-- =============================================
ALTER PROCEDURE [ZGWOptional].[Get_Calendar_Data]
      @P_CalendarSeqId INT
    , @P_Start_Date SMALLDATETIME
    , @P_End_Date SMALLDATETIME
AS
	SET NOCOUNT ON;
	SELECT
		  [CalendarEventSeqId]
		, [CalendarSeqId]
		, [Title]
		, [Start]
		, [End]
		, [AllDay]
		, [Description]
		, [Color]
		, [Link]
		, [Location]
		, [AddedBy] = (SELECT TOP(1) [FullName] = [First_Name] + ' ' + [Last_Name] FROM [ZGWSecurity].[Accounts] WHERE [AccountSeqId] = [Added_By])
		, [Added_Date]
		, [UpdatedBy] = (SELECT TOP(1) [FullName] = [First_Name] + ' ' + [Last_Name] FROM [ZGWSecurity].[Accounts] WHERE [AccountSeqId] = [Updated_By])
		, [Updated_Date]
	FROM 
		[ZGWOptional].[Calendar_Events]
	WHERE 
		[CalendarSeqId] = @P_CalendarSeqId
		AND [Start] >= @P_Start_Date 
		AND [End] <= @P_End_Date;

	SET NOCOUNT OFF;

RETURN 0
GO
/****** End: Procedure [ZGWOptional].[Get_Calendar_Data] ******/

-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '3.0.4.0',
    [Updated_By] = 3,
    [Updated_Date] = getdate()