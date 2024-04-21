CREATE TABLE [dbo].[ZFO_MESSAGES] (
    [MESSAGE_SEQ_ID] INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SE_SEQ_ID]      INT           NOT NULL,
    [NAME]           VARCHAR (50)  NOT NULL,
    [TITLE]          VARCHAR (100) NOT NULL,
    [DESCRIPTION]    VARCHAR (128) NOT NULL,
    [FORMAT_AS_HTML] INT           NOT NULL,
    [BODY]           NTEXT         NOT NULL,
    [ADDED_BY]       INT           NOT NULL,
    [ADDED_DATE]     DATETIME      DEFAULT (getdate()) NOT NULL,
    [UPDATED_BY]     INT           NULL,
    [UPDATED_DATE]   DATETIME      DEFAULT (getdate()) NULL
);

