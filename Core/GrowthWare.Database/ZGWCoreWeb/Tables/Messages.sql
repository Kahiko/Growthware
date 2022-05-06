CREATE TABLE [ZGWCoreWeb].[Messages] (
    [MessageSeqId]         INT                  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SecurityEntitySeqId] INT                  NOT NULL,
    [Name]                  VARCHAR (50)         NOT NULL,
    [Title]                 VARCHAR (100)        NOT NULL,
    [Description]           VARCHAR (512) SPARSE NULL,
    [Format_As_HTML]        INT                  CONSTRAINT [DF_ZGWCoreWeb_Messages_Format_As_HTML] DEFAULT ((0)) NOT NULL,
    [Body]                  VARCHAR (MAX)        NOT NULL,
    [Added_By]              INT                  NOT NULL,
    [Added_Date]            DATETIME             DEFAULT (getdate()) NOT NULL,
    [Updated_By]            INT                  NULL,
    [Updated_Date]          DATETIME             NULL,
    CONSTRAINT [PK_ZFO_Messages] PRIMARY KEY CLUSTERED ([MessageSeqId] ASC),
    CONSTRAINT [FK_Messages_Entities] FOREIGN KEY ([SecurityEntitySeqId]) REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [FK_IXSecurityEntitySeqId]
    ON [ZGWCoreWeb].[Messages]([SecurityEntitySeqId] ASC);


GO

