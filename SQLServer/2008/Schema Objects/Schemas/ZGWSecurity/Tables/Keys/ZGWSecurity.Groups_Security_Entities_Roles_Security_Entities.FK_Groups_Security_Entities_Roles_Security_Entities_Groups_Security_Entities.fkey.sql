ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities]
    ADD CONSTRAINT [FK_Groups_Security_Entities_Roles_Security_Entities_Groups_Security_Entities] FOREIGN KEY ([Groups_Security_Entities_SeqID]) REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([Groups_Security_Entities_SeqID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

