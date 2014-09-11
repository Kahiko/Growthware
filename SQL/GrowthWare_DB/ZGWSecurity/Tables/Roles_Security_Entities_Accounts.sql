CREATE TABLE [ZGWSecurity].[Roles_Security_Entities_Accounts] (
    [Roles_Security_Entities_SeqID] INT      NOT NULL,
    [Account_SeqID]                 INT      NOT NULL,
    [Added_By]                      INT      CONSTRAINT [DF_ZGWSecurity_Roles_Security_Entities_Accounts_ADDED_BY] DEFAULT ((1)) NOT NULL,
    [Added_Date]                    DATETIME CONSTRAINT [DF_ZGWSecurity_Roles_Security_Entities_Accounts_ADDED_DATE] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [FK_Roles_Security_Entities_Accounts_Accounts] FOREIGN KEY ([Account_SeqID]) REFERENCES [ZGWSecurity].[Accounts] ([Account_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Roles_Security_Entities_Accounts_Roles_Security_Entities] FOREIGN KEY ([Roles_Security_Entities_SeqID]) REFERENCES [ZGWSecurity].[Roles_Security_Entities] ([Roles_Security_Entities_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE
);

