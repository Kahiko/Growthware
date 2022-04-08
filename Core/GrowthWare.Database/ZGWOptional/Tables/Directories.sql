CREATE TABLE [ZGWOptional].[Directories] (
    [Function_SeqID]         INT           NOT NULL,
    [Directory]              VARCHAR (255) NOT NULL,
    [Impersonate]            INT           NOT NULL,
    [Impersonating_Account]  VARCHAR (50)  NULL,
    [Impersonating_Password] VARCHAR (50)  NULL,
    [Added_By]               INT           NOT NULL,
    [Added_Date]             DATETIME      DEFAULT (getdate()) NOT NULL,
    [Updated_By]             INT           NULL,
    [Updated_Date]           DATETIME      NULL,
    CONSTRAINT [PK_Directories] PRIMARY KEY CLUSTERED ([Function_SeqID] ASC),
    CONSTRAINT [FK_Directories_Functions] FOREIGN KEY ([Function_SeqID]) REFERENCES [ZGWSecurity].[Functions] ([Function_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

