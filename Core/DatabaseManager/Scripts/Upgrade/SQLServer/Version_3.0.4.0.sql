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

	CREATE UNIQUE NONCLUSTERED INDEX [UX_Calendars_SecurityEntitySeqId_Name] ON [ZGWOptional].[Calendars]
	(
		[SecurityEntitySeqId] ASC,
		[Calendar_Name] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY];

	ALTER TABLE [ZGWOptional].[Calendars]  WITH CHECK ADD  CONSTRAINT [FK_ZGWSecurity_Entities_ZGWOptional_Calendars] FOREIGN KEY([SecurityEntitySeqId])
	REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId])
	ON UPDATE CASCADE
	ON DELETE CASCADE;

	ALTER TABLE [ZGWOptional].[Calendars] CHECK CONSTRAINT [FK_ZGWSecurity_Entities_ZGWOptional_Calendars];

  END
--END IF
/****** End: Add columns to [ZGWOptional].[Calendars] ******/

/****** Start: Procedure [ZGWOptional].[Set_Calendar] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWOptional].[Set_Calendar]') AND type in (N'P', N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWOptional].[Set_Calendar] AS'
	END
--End If

GO
/*
Usage:
	DECLARE 
	  @P_CalendarSeqId INT  = -1,
	  @P_SecurityEntitySeqId [int] = (SELECT TOP(1) [SecurityEntitySeqId] FROM [ZGWSecurity].[Security_Entities] WHERE [Name] = 'System'),
	  @P_Calendar_Name [varchar](50) = 'Community Calendar',
	  @P_Comment [varchar](100) = 'Created for the community calendar',
	  @P_Active [int] = 1,
	  @P_Added_Updated_By [int] = (SELECT TOP(1) [AccountSeqId] FROM [ZGWSecurity].[Accounts] WHERE [Account] = 'System');

	exec ZGWOptional.Set_Calendar
	  @P_CalendarSeqId OUTPUT,
	  @P_SecurityEntitySeqId,
	  @P_Calendar_Name,
	  @P_Comment,
	  @P_Active,
	  @P_Added_Updated_By;

    PRINT @P_CalendarSeqId;
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 03/01/2024
-- Description:	Calendar Data
-- =============================================
ALTER PROCEDURE [ZGWOptional].[Set_Calendar]
    @P_CalendarSeqId INT OUTPUT,
    @P_SecurityEntitySeqId [int],
    @P_Calendar_Name [varchar](50),
    @P_Comment [varchar](100),
    @P_Active [int],
    @P_Added_Updated_By [int]
AS
	SET NOCOUNT ON;
	IF @P_CalendarSeqId > 0
		BEGIN
			UPDATE [ZGWOptional].[Calendars] SET
			  [Calendar_Name] = @P_Calendar_Name
			, [Comment] = @P_Comment
			, [Active] = @P_Active
			, [Updated_By] = @P_Added_Updated_By
			, [Updated_Date] = GETDATE()
			WHERE [CalendarSeqId] = @P_CalendarSeqId;
		END
	ELSE
		BEGIN
			INSERT INTO [ZGWOptional].[Calendars] (
				  [SecurityEntitySeqId]
				, [Calendar_Name]
				, [Comment]
				, [Active]
				, [Added_By]
				, [Added_Date]
			) VALUES (
				  @P_SecurityEntitySeqId
				, @P_Calendar_Name
				, @P_Comment
				, @P_Active
				, @P_Added_Updated_By
				, GETDATE()
			);
			SELECT @P_CalendarSeqId=SCOPE_IDENTITY();
		END
	--END IF

	SET NOCOUNT OFF;

RETURN 0
GO
/****** End: Procedure [ZGWOptional].[Set_Calendar] ******/

/****** Start: Procedure [ZGWOptional].[Get_Calendar] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWOptional].[Get_Calendar]') AND type in (N'P', N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWOptional].[Get_Calendar] AS'
	END
--End If

GO
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
ALTER PROCEDURE [ZGWOptional].[Get_Calendar]
    @P_CalendarSeqId INT
AS
	SET NOCOUNT ON;
	IF @P_CalendarSeqId > 0
		BEGIN
			SELECT 
				 [CalendarSeqId]
				,[SecurityEntitySeqId]
				,[Calendar_Name]
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
/****** End: Procedure [ZGWOptional].[Get_Calendar] ******/

/****** Start: Procedure [ZGWOptional].[Delete_Calendar] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWOptional].[Delete_Calendar]') AND type in (N'P', N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWOptional].[Delete_Calendar] AS'
	END
--End If

GO
/*
Usage:
	DECLARE 
		@P_CalendarSeqId INT = 1

	exec ZGWOptional.Delete_Calendar
		@P_CalendarSeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 03/01/2024
-- Description:	Calendar Data
-- =============================================
ALTER PROCEDURE [ZGWOptional].[Delete_Calendar]
      @P_CalendarSeqId INT
AS
	SET NOCOUNT ON;
	IF EXISTS (SELECT 1 FROM [ZGWOptional].[Calendars] WHERE [CalendarSeqId] = @P_CalendarSeqId)
		BEGIN
			DELETE FROM [ZGWOptional].[Calendars] WHERE [CalendarSeqId] = @P_CalendarSeqId;
		END
	ELSE
		BEGIN
			PRINT 'Not found'
		END
	--END IF
	SET NOCOUNT OFF;

RETURN 0
GO
/****** End: Procedure [ZGWOptional].[Delete_Calendar] ******/

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

/****** Start: Procedure [ZGWOptional].[Get_Calendar_Events] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWOptional].[Get_Calendar_Events]') AND type in (N'P', N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWOptional].[Get_Calendar_Events] AS'
	END
--End If

GO

/*
Usage:
	DECLARE 
		@P_CalendarSeqId INT = 1
	  , @P_Start_Date SMALLDATETIME = CONVERT(VARCHAR, '2/2/24 00:00', 108)
	  , @P_End_Date SMALLDATETIME = CONVERT(VARCHAR, '2/29/24 00:00', 108)

	exec ZGWOptional.Get_Calendar_Events
		@P_CalendarSeqId
	  , @P_Start_Date
	  , @P_End_Date
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 02/26/2024
-- Description:	Calendar Data
-- =============================================
ALTER PROCEDURE [ZGWOptional].[Get_Calendar_Events]
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
/****** End: Procedure [ZGWOptional].[Get_Calendar_Events] ******/

/****** Start: Procedure [ZGWOptional].[Set_Calendar_Event] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWOptional].[Set_Calendar_Event]') AND type in (N'P', N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWOptional].[Set_Calendar_Event] AS'
	END
--End If
GO
/*
USAGE:
	DECLARE
		  @P_CalendarEventSeqId	INT				= -1
		, @P_CalendarSeqId		INT				= 1
		, @P_Title				VARCHAR(255)	= 'Fake meeting with me ;-)'
		, @P_Start				DATETIME		= CONVERT(VARCHAR, '2/2/24 00:00', 108)
		, @P_End				DATETIME		= CONVERT(VARCHAR, '2/2/24 00:00', 108)
		, @P_AllDay				BIT				= 0
		, @P_Description		VARCHAR(512)	= 'Fake meeting with me ;-) so I can test'
		, @P_Color				VARCHAR(20)		= '#ff0000' -- red
		, @P_Link				VARCHAR(255)	= ''
		, @P_Location			VARCHAR(255)	= 'My office?'
		, @P_Added_Updated_By	INT				= 3

	exec ZGWOptional.Set_Calendar_Event
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
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 03/06/2024
-- Description:	Calendar Data
-- =============================================
ALTER PROCEDURE [ZGWOptional].[Set_Calendar_Event]
	  @P_CalendarEventSeqId	INT OUTPUT
	, @P_CalendarSeqId		INT
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
				  @P_CalendarSeqId
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
			WHERE [CalendarSeqId] = @P_CalendarSeqId;
		END
	--END IF
	SET NOCOUNT OFF;

RETURN 0
GO
/****** End: Procedure [ZGWOptional].[Set_Calendar_Event] ******/

/****** Adding new Calendar ******/
	DECLARE 
	  @P_CalendarSeqId INT  = -1,
	  @P_SecurityEntitySeqId [int] = (SELECT TOP(1) [SecurityEntitySeqId] FROM [ZGWSecurity].[Security_Entities] WHERE [Name] = 'System'),
	  @P_Calendar_Name [varchar](50) = 'Community Calendar',
	  @P_Comment [varchar](100) = 'Created for the community calendar',
	  @P_Active [int] = 1,
	  @P_Added_Updated_By [int] = (SELECT TOP(1) [AccountSeqId] FROM [ZGWSecurity].[Accounts] WHERE [Account] = 'System');

	EXEC ZGWOptional.Set_Calendar
	  @P_CalendarSeqId OUTPUT,
	  @P_SecurityEntitySeqId,
	  @P_Calendar_Name,
	  @P_Comment,
	  @P_Active,
	  @P_Added_Updated_By;
/****** Done adding new Calendar ******/
-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '3.0.4.0',
    [Updated_By] = 3,
    [Updated_Date] = getdate()