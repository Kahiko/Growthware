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
    [Added_By]            INT           CONSTRAINT [DF_ZGWOptional_Calendar_Added_By] DEFAULT ((1)) NOT NULL,
    [Added_Date]          DATETIME      CONSTRAINT [DF_ZGWOptional_Calendar_Added_Date] DEFAULT (getdate()) NOT NULL,
    [Updated_By]          INT           NULL,
    [Updated_Date]        DATETIME      NULL,
    CONSTRAINT [PK_Calendar_Events] PRIMARY KEY CLUSTERED ([CalendarEventSeqId] ASC),
    CONSTRAINT [FK_ZGWOptional_Calendars_Calendar_Events] FOREIGN KEY ([CalendarSeqId]) REFERENCES [ZGWOptional].[Calendars] ([CalendarSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
);


GO