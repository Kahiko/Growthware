CREATE TABLE [ZGWCoreWeb].[Messages] (
    [Message_SeqID]         INT                  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Security_Entity_SeqID] INT                  NOT NULL,
    [Name]                  VARCHAR (50)         NOT NULL,
    [Title]                 VARCHAR (100)        NOT NULL,
    [Description]           VARCHAR (512) SPARSE NULL,
    [Format_As_HTML]        INT                  CONSTRAINT [DF_ZGWCoreWeb_Messages_Format_As_HTML] DEFAULT ((0)) NOT NULL,
    [Body]                  VARCHAR (MAX)        NOT NULL,
    [Added_By]              INT                  NOT NULL,
    [Added_Date]            DATETIME             DEFAULT (getdate()) NOT NULL,
    [Updated_By]            INT                  NULL,
    [Updated_Date]          DATETIME             NULL,
    CONSTRAINT [PK_ZFO_Messages] PRIMARY KEY CLUSTERED ([Message_SeqID] ASC),
    CONSTRAINT [FK_Messages_Entities] FOREIGN KEY ([Security_Entity_SeqID]) REFERENCES [ZGWSecurity].[Security_Entities] ([Security_Entity_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [FK_IX_Security_Entity_SeqID]
    ON [ZGWCoreWeb].[Messages]([Security_Entity_SeqID] ASC);

