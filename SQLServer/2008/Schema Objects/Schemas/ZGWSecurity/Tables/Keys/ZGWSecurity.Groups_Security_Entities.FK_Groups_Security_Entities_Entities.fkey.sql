ALTER TABLE [ZGWSecurity].[Groups_Security_Entities]
    ADD CONSTRAINT [FK_Groups_Security_Entities_Entities] FOREIGN KEY ([Security_Entity_SeqID]) REFERENCES [ZGWSecurity].[Security_Entities] ([Security_Entity_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE;

