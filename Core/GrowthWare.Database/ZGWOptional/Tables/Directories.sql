CREATE TABLE [ZGWOptional].[Directories] (
    [FunctionSeqId]         INT           NOT NULL,
    [Directory]              VARCHAR (255) NOT NULL,
    [Impersonate]            INT           NOT NULL,
    [Impersonating_Account]  VARCHAR (50)  NULL,
    [Impersonating_Password] VARCHAR (50)  NULL,
    [Added_By]               INT           NOT NULL,
    [Added_Date]             DATETIME      DEFAULT (getdate()) NOT NULL,
    [Updated_By]             INT           NULL,
    [Updated_Date]           DATETIME      NULL,
    CONSTRAINT [PK_Directories] PRIMARY KEY CLUSTERED ([FunctionSeqId] ASC),
    CONSTRAINT [FK_Directories_Functions] FOREIGN KEY ([FunctionSeqId]) REFERENCES [ZGWSecurity].[Functions] ([FunctionSeqId]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

