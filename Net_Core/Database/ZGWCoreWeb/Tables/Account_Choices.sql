CREATE TABLE [ZGWCoreWeb].[Account_Choices] (
    [Account]                 VARCHAR (128) NOT NULL,
    [SecurityEntityId]        INT           NULL,
    [SecurityEntityName]      VARCHAR (256) NULL,
    [FavoriteAction]          VARCHAR (15)  NULL,
    [RecordsPerPage]          VARCHAR (15)  NULL,
    [ColorScheme]             VARCHAR (15)  NULL,
    [EvenRow]                 VARCHAR (15)  NULL,
    [EvenFont]                VARCHAR (15)  NULL,
    [OddRow]                  VARCHAR (15)  NULL,
    [OddFont]                 VARCHAR (15)  NULL,
    [HeaderRow]               VARCHAR (15)  NULL,
    [HeaderFont]              VARCHAR (15)  NULL,
    [Background]              VARCHAR (15)  NULL,
    CONSTRAINT [FK_ZGWCore_Account_Choices_ZGWSecurity_Security_Accounts] FOREIGN KEY ([Account]) REFERENCES [ZGWSecurity].[Accounts] ([Account]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UK_ZGWCore_Account_Choices] UNIQUE NONCLUSTERED ([Account] ASC)
);

GO
