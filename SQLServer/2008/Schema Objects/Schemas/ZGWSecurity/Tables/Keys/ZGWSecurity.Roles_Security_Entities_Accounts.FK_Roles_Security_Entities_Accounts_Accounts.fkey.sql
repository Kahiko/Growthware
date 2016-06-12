ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Accounts]
    ADD CONSTRAINT [FK_Roles_Security_Entities_Accounts_Accounts] FOREIGN KEY ([Account_SeqID]) REFERENCES [ZGWSecurity].[Accounts] ([Account_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE;

