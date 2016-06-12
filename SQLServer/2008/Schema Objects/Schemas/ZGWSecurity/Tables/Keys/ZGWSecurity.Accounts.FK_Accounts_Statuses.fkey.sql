ALTER TABLE [ZGWSecurity].[Accounts]
    ADD CONSTRAINT [FK_Accounts_Statuses] FOREIGN KEY ([Status_SeqID]) REFERENCES [ZGWSystem].[Statuses] ([Status_SeqID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

