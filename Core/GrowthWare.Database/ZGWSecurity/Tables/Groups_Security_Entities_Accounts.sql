CREATE TABLE [ZGWSecurity].[Groups_Security_Entities_Accounts] (
    [Groups_Security_Entities_SeqID] INT      NOT NULL,
    [Account_SeqID]                  INT      NOT NULL,
    [Added_By]                       INT      CONSTRAINT [DF_ZGWSecurity_Groups_Security_Entities_Accounts_Added_By] DEFAULT ((1)) NOT NULL,
    [Added_Date]                     DATETIME CONSTRAINT [DF_ZGWSecurity_Groups_Security_Entities_Accounts_Added_Date] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [FK_Groups_Security_Entities_Accounts_Accounts] FOREIGN KEY ([Account_SeqID]) REFERENCES [ZGWSecurity].[Accounts] ([Account_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Groups_Security_Entities_Accounts_Groups_Security_Entities] FOREIGN KEY ([Groups_Security_Entities_SeqID]) REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([Groups_Security_Entities_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

