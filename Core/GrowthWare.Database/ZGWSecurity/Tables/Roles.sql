CREATE TABLE [ZGWSecurity].[Roles] (
    [Role_SeqID]     INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]           VARCHAR (50)  NOT NULL,
    [Description]    VARCHAR (128) NOT NULL,
    [Is_System]      INT           NOT NULL,
    [Is_System_Only] INT           CONSTRAINT [DF_ZGWSecurity_Roles_IS_SYSTEM_ONLY] DEFAULT ((0)) NOT NULL,
    [Added_By]       INT           CONSTRAINT [DF_ZGWSecurity_Roles_Added_By] DEFAULT ((2)) NOT NULL,
    [Added_Date]     DATETIME      CONSTRAINT [DF_ZGWSecurity_Roles_ADDED_DATE] DEFAULT (getdate()) NOT NULL,
    [Updated_By]     INT           NULL,
    [Updated_Date]   DATETIME      NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([Role_SeqID] ASC),
    CONSTRAINT [UK_ZGWSecurity_Roles] UNIQUE NONCLUSTERED ([Name] ASC)
);


GO

