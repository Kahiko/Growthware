CREATE TABLE [ZGWOptional].[Calendars] (
    [CalendarSeqId]       INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SecurityEntitySeqId] INT           NOT NULL,
    [FunctionSeqId]       INT           NOT NULL,
    [Calendar_Name]       VARCHAR (50)  NOT NULL,
    [Entry_Date]          SMALLDATETIME NOT NULL,
    [Comment]             VARCHAR (100) NOT NULL,
    [Active]              INT           NOT NULL,
    [Added_By]            INT           CONSTRAINT [DF_ZGWOptional_Calendar_Added_By] DEFAULT ((1)) NOT NULL,
    [Added_Date]          DATETIME      CONSTRAINT [DF_ZGWOptional_Calendar_Added_Date] DEFAULT (getdate()) NOT NULL,
    [Updated_By]          INT           NULL,
    [Updated_Date]        DATETIME      NULL,
    CONSTRAINT [PK_Calendars] PRIMARY KEY CLUSTERED ([CalendarSeqId] ASC),
    CONSTRAINT [FK_ZGWSecurity_Entities_ZGWOptional_Calendars] FOREIGN KEY ([SecurityEntitySeqId]) REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ZGWSecurity_Functions_ZGWOptional_Calendars] FOREIGN KEY ([FunctionSeqId]) REFERENCES [ZGWSecurity].[Functions] ([FunctionSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
);

GO
