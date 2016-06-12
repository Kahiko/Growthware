ALTER TABLE [ZGWSecurity].[Security_Entities]
    ADD CONSTRAINT [FK_Entities_Statuses] FOREIGN KEY ([Status_SeqID]) REFERENCES [ZGWSystem].[Statuses] ([Status_SeqID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

