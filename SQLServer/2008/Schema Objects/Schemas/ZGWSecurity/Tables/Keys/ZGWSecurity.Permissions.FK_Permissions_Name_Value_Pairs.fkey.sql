ALTER TABLE [ZGWSecurity].[Permissions]
    ADD CONSTRAINT [FK_Permissions_Name_Value_Pairs] FOREIGN KEY ([NVP_SeqID]) REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVP_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE;

