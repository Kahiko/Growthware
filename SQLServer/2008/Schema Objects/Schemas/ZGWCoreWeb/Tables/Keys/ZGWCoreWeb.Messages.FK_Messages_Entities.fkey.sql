ALTER TABLE [ZGWCoreWeb].[Messages]
    ADD CONSTRAINT [FK_Messages_Entities] FOREIGN KEY ([Security_Entity_SeqID]) REFERENCES [ZGWSecurity].[Security_Entities] ([Security_Entity_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE;

