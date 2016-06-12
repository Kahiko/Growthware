ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Groups]
    ADD CONSTRAINT [FK_Groups_Security_Entities_Groups_Groups_Security_Entities] FOREIGN KEY ([Groups_Security_Entities_SeqID]) REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([Groups_Security_Entities_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE;

