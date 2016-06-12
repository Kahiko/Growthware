CREATE TABLE [ZGWCoreWeb].[Messages] (
    [Message_SeqID]         INT                  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Security_Entity_SeqID] INT                  NOT NULL,
    [Name]                  VARCHAR (50)         NOT NULL,
    [Title]                 VARCHAR (100)        NOT NULL,
    [Description]           VARCHAR (512) SPARSE NULL,
    [Format_As_HTML]        INT                  NOT NULL,
    [Body]                  VARCHAR (MAX)        NOT NULL,
    [Added_By]              INT                  NOT NULL,
    [Added_Date]            DATETIME             DEFAULT (getdate()) NOT NULL,
    [Updated_By]            INT                  NULL,
    [Updated_Date]          DATETIME             NULL
);

