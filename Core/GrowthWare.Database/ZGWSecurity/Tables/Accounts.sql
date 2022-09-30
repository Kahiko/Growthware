CREATE TABLE [ZGWSecurity].[Accounts] (
    [AccountSeqId]         INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Account]              VARCHAR (128) NOT NULL,
    [Email]                VARCHAR (128) NULL,
    [Enable_Notifications] INT           NULL,
    [Is_System_Admin]      INT           CONSTRAINT [DF_Accounts_IS_SYSTEM_ADMIN] DEFAULT ((0)) NOT NULL,
    [StatusSeqId]          INT           NOT NULL,
    [Password_Last_Set]    DATETIME      CONSTRAINT [DF_Accounts_PASSWORD_LAST_SET] DEFAULT (getdate()) NOT NULL,
    [Password]             VARCHAR (256) NOT NULL,
    [Failed_Attempts]      INT           NOT NULL,
    [First_Name]           VARCHAR (35)  NOT NULL,
    [Last_Login]           DATETIME      CONSTRAINT [DF_Accounts_LAST_LOGIN] DEFAULT (getdate()) NULL,
    [Last_Name]            VARCHAR (35)  NOT NULL,
    [Location]             VARCHAR (128) NULL,
    [Middle_Name]          VARCHAR (35)  NULL,
    [Preferred_Name]       VARCHAR (50)  NULL,
    [Time_Zone]            INT           NULL,
    [Added_By]             INT           CONSTRAINT [DF_Accounts_Added_By] DEFAULT ((1)) NOT NULL,
    [Added_Date]           DATETIME      CONSTRAINT [DF_Accounts_ADDED_DATE] DEFAULT (getdate()) NOT NULL,
    [Updated_By]           INT           NULL,
    [Updated_Date]         DATETIME      NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED ([AccountSeqId] ASC),
    CONSTRAINT [FK_Accounts_Statuses] FOREIGN KEY ([StatusSeqId]) REFERENCES [ZGWSystem].[Statuses] ([StatusSeqId]),
    CONSTRAINT [UK_Accounts] UNIQUE NONCLUSTERED ([Account] ASC)
);


GO

