ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Accounts]
    ADD CONSTRAINT [FK_Roles_Security_Entities_Accounts_Roles_Security_Entities] FOREIGN KEY ([Roles_Security_Entities_SeqID]) REFERENCES [ZGWSecurity].[Roles_Security_Entities] ([Roles_Security_Entities_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE;

