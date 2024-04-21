CREATE TABLE [ZGWSecurity].[Groups_Security_Entities_Accounts] (
    [GroupsSecurityEntitiesSeqId] INT      NOT NULL,
    [AccountSeqId]                INT      NOT NULL,
    [Added_By]                    INT      CONSTRAINT [DF_ZGWSecurity_Groups_Security_Entities_Accounts_Added_By] DEFAULT ((1)) NOT NULL,
    [Added_Date]                  DATETIME CONSTRAINT [DF_ZGWSecurity_Groups_Security_Entities_Accounts_Added_Date] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [FK_Groups_Security_Entities_Accounts_Accounts] FOREIGN KEY ([AccountSeqId]) REFERENCES [ZGWSecurity].[Accounts] ([AccountSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Groups_Security_Entities_Accounts_Groups_Security_Entities] FOREIGN KEY ([GroupsSecurityEntitiesSeqId]) REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([GroupsSecurityEntitiesSeqId]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

