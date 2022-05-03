CREATE TABLE [ZGWCoreWeb].[Account_Choices] (
    [Account]                  VARCHAR (128)  NOT NULL,
    [securityEntityID]                INT            NULL,
    [SE_NAME]                  VARCHAR (256)  NULL,
    [Back_Color]               VARCHAR (15)   NULL,
    [Left_Color]               VARCHAR (15)   NULL,
    [Head_Color]               VARCHAR (15)   NULL,
    [Sub_Head_Color]           VARCHAR (15)   NULL,
    [Color_Scheme]             VARCHAR (15)   NULL,
    [Favorite_Action]          VARCHAR (50)   NULL,
    [Records_Per_Page]         INT            NULL,
    [Row_BackColor]            VARCHAR (15)   NULL,
    [AlternatingRow_BackColor] VARCHAR (15)   NULL,
    [Header_ForeColor]         VARCHAR (15)   NULL,
    CONSTRAINT [FK_ZGWCore_Account_Choices_ZGWSecurity_Security_Accounts] FOREIGN KEY ([Account]) REFERENCES [ZGWSecurity].[Accounts] ([Account]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UK_ZGWCore_Account_Choices] UNIQUE NONCLUSTERED ([Account] ASC)
);


GO

