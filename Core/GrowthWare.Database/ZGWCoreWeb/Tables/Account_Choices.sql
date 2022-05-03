CREATE TABLE [ZGWCoreWeb].[Account_Choices] (
    [Account]                  VARCHAR (128)  NOT NULL,
    [SecurityEntityID]                INT            NULL,
    [SecurityEntityName]                  VARCHAR (256)  NULL,
    [BackColor]               VARCHAR (15)   NULL,
    [LeftColor]               VARCHAR (15)   NULL,
    [HeadColor]               VARCHAR (15)   NULL,
    [SubHeadColor]           VARCHAR (15)   NULL,
    [ColorScheme]             VARCHAR (15)   NULL,
    [FavoriteAction]          VARCHAR (50)   NULL,
    [Records_Per_Page]         INT            NULL,
    [Row_BackColor]            VARCHAR (15)   NULL,
    [AlternatingRow_BackColor] VARCHAR (15)   NULL,
    [Header_ForeColor]         VARCHAR (15)   NULL,
    CONSTRAINT [FK_ZGWCore_Account_Choices_ZGWSecurity_Security_Accounts] FOREIGN KEY ([Account]) REFERENCES [ZGWSecurity].[Accounts] ([Account]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UK_ZGWCore_Account_Choices] UNIQUE NONCLUSTERED ([Account] ASC)
);


GO

