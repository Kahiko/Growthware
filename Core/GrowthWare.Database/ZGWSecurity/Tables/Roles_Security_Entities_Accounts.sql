CREATE TABLE [ZGWSecurity].[Roles_Security_Entities_Accounts] (
    [Roles_Security_EntitiesSeqId] INT      NOT NULL,
    [AccountSeqId]                 INT      NOT NULL,
    [Added_By]                      INT      CONSTRAINT [DF_ZGWSecurity_Roles_Security_Entities_Accounts_Added_By] DEFAULT ((1)) NOT NULL,
    [Added_Date]                    DATETIME CONSTRAINT [DF_ZGWSecurity_Roles_Security_Entities_Accounts_ADDED_DATE] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [FK_Roles_Security_Entities_Accounts_Accounts] FOREIGN KEY ([AccountSeqId]) REFERENCES [ZGWSecurity].[Accounts] ([AccountSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Roles_Security_Entities_Accounts_Roles_Security_Entities] FOREIGN KEY ([Roles_Security_EntitiesSeqId]) REFERENCES [ZGWSecurity].[Roles_Security_Entities] ([Roles_Security_EntitiesSeqId]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

