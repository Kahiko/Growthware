CREATE TABLE [ZGWSecurity].[Roles_Security_Entities_Accounts] (
    [RolesSecurityEntitiesSeqId] INT      NOT NULL,
    [AccountSeqId]               INT      NOT NULL,
    [Added_By]                   INT      CONSTRAINT [DF_ZGWSecurity_Roles_Security_Entities_Accounts_Added_By] DEFAULT ((1)) NOT NULL,
    [Added_Date]                 DATETIME CONSTRAINT [DF_ZGWSecurity_Roles_Security_Entities_Accounts_ADDED_DATE] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [FK_Roles_Security_Entities_Accounts_Accounts] FOREIGN KEY ([AccountSeqId]) REFERENCES [ZGWSecurity].[Accounts] ([AccountSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Roles_Security_Entities_Accounts_Roles_Security_Entities] FOREIGN KEY ([RolesSecurityEntitiesSeqId]) REFERENCES [ZGWSecurity].[Roles_Security_Entities] ([RolesSecurityEntitiesSeqId]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

