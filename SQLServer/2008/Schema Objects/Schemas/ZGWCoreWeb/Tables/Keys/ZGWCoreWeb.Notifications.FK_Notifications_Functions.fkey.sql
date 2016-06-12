ALTER TABLE [ZGWCoreWeb].[Notifications]
    ADD CONSTRAINT [FK_Notifications_Functions] FOREIGN KEY ([Function_SeqID]) REFERENCES [ZGWSecurity].[Functions] ([Function_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE;

