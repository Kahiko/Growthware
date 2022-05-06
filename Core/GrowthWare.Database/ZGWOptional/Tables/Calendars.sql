CREATE TABLE [ZGWOptional].[Calendars] (
    [SecurityEntitySeqId] INT           NOT NULL,
    [Calendar_Name]         VARCHAR (50)  NOT NULL,
    [Entry_Date]            SMALLDATETIME NOT NULL,
    [Comment]               VARCHAR (100) NOT NULL,
    [Active]                INT           NOT NULL,
    [Added_By]              INT           CONSTRAINT [DF_ZGWOptional_Calendar_Added_By] DEFAULT ((1)) NOT NULL,
    [Added_Date]            DATETIME      CONSTRAINT [DF_ZGWOptional_Calendar_Added_Date] DEFAULT (getdate()) NOT NULL,
    [Updated_By]            INT           NULL,
    [Updated_Date]          DATETIME      NULL,
    CONSTRAINT [FK_ZGWOptional_Calendar_ZGWSecurity_Entities] FOREIGN KEY ([SecurityEntitySeqId]) REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

