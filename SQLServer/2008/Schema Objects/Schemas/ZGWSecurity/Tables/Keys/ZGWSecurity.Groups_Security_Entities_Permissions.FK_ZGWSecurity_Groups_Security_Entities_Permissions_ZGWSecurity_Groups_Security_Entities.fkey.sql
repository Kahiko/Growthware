ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Permissions]
    ADD CONSTRAINT [FK_ZGWSecurity_Groups_Security_Entities_Permissions_ZGWSecurity_Groups_Security_Entities] FOREIGN KEY ([Groups_Security_Entities_SeqID]) REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([Groups_Security_Entities_SeqID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

