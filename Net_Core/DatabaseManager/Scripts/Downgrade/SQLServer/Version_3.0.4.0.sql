-- Downgrade
USE [YourDatabaseName];
GO
SET NOCOUNT ON;

/****** Start: Procedure [ZGWSystem].[Get_Calendar_Events] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWOptional].[Get_Calendar_Events]') AND type in (N'P', N'PC'))
	BEGIN
		DROP PROCEDURE ZGWOptional.Get_Calendar_Events;
	END
--End If
/****** End: Procedure [ZGWSystem].[Get_Calendar_Events] ******/

/****** Start: Procedure [ZGWSystem].[Set_Calendar_Event] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWOptional].[Set_Calendar_Event]') AND type in (N'P', N'PC'))
	BEGIN
		DROP PROCEDURE ZGWOptional.Set_Calendar_Event;
	END
--End If
/****** End: Procedure [ZGWSystem].[Set_Calendar_Event] ******/

/****** Start: Procedure [ZGWSystem].[Get_Calendar_Event] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWOptional].[Get_Calendar_Event]') AND type in (N'P', N'PC'))
	BEGIN
		DROP PROCEDURE ZGWOptional.Get_Calendar_Event;
	END
--End If
/****** End: Procedure [ZGWSystem].[Get_Calendar_Event] ******/

/****** Start: Procedure [ZGWSystem].[Delete_Calendar_Event] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWOptional].[Delete_Calendar_Event]') AND type in (N'P', N'PC'))
	BEGIN
		DROP PROCEDURE ZGWOptional.Delete_Calendar_Event;
	END
--End If
/****** End: Procedure [ZGWSystem].[Delete_Calendar_Event] ******/

/****** Start: Procedure [ZGWOptional].[Set_Calendar] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWOptional].[Set_Calendar]') AND type in (N'P', N'PC'))
	BEGIN
		DROP PROCEDURE ZGWOptional.Set_Calendar;
	END
--End If
/****** End: Procedure [ZGWOptional].[Set_Calendar] ******/

/****** Start: Procedure [ZGWOptional].[Get_Calendar] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWOptional].[Get_Calendar]') AND type in (N'P', N'PC'))
	BEGIN
		DROP PROCEDURE ZGWOptional.Get_Calendar;
	END
--End If
/****** End: Procedure [ZGWOptional].[Get_Calendar] ******/

/****** Start: Procedure [ZGWOptional].[Delete_Calendar] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWOptional].[Delete_Calendar]') AND type in (N'P', N'PC'))
	BEGIN
		DROP PROCEDURE ZGWOptional.Delete_Calendar;
	END
--End If
/****** End: Procedure [ZGWOptional].[Delete_Calendar] ******/

/****** Start: Drop [ZGWOptional].[Calendar_Events] ******/
IF OBJECT_ID(N'ZGWOptional.Calendar_Events', N'U') IS NOT NULL  
   DROP TABLE [ZGWOptional].[Calendar_Events];  
GO
/****** End: Drop [ZGWOptional].[Calendar_Events] ******/

/****** Start: Drop columns to [ZGWOptional].[Calendars] ******/
IF COL_LENGTH('ZGWOptional.Calendars','CalendarSeqId') IS NOT NULL
  BEGIN
	CREATE TABLE [ZGWOptional].[Calendars2](
		[SecurityEntitySeqId] [int] NOT NULL,
		[Calendar_Name] [varchar](50) NOT NULL,
		[Entry_Date] [smalldatetime] NOT NULL,
		[Comment] [varchar](100) NOT NULL,
		[Active] [int] NOT NULL,
		[Added_By] [int] NOT NULL,
		[Added_Date] [datetime] NOT NULL,
		[Updated_By] [int] NULL,
		[Updated_Date] [datetime] NULL
	) ON [PRIMARY];

    DROP TABLE [ZGWOptional].[Calendars];

    EXEC sp_rename 'ZGWOptional.Calendars2', 'Calendars';

	ALTER TABLE [ZGWOptional].[Calendars] ADD  CONSTRAINT [DF_ZGWOptional_Calendar_Added_By]  DEFAULT ((1)) FOR [Added_By]

	ALTER TABLE [ZGWOptional].[Calendars] ADD  CONSTRAINT [DF_ZGWOptional_Calendar_Added_Date]  DEFAULT (getdate()) FOR [Added_Date]

	ALTER TABLE [ZGWOptional].[Calendars]  WITH CHECK ADD  CONSTRAINT [FK_ZGWOptional_Calendar_ZGWSecurity_Entities] FOREIGN KEY([SecurityEntitySeqId])
	REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId])
	ON UPDATE CASCADE
	ON DELETE CASCADE

	ALTER TABLE [ZGWOptional].[Calendars] CHECK CONSTRAINT [FK_ZGWOptional_Calendar_ZGWSecurity_Entities]
  END
--END IF
/****** End: Drop columns to [ZGWOptional].[Calendars] ******/

-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '3.0.1.0',
    [Updated_By] = null,
    [Updated_Date] = null