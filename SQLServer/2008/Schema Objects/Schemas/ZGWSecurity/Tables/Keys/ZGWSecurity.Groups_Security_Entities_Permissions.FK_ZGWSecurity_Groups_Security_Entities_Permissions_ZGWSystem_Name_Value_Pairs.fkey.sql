ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Permissions]
    ADD CONSTRAINT [FK_ZGWSecurity_Groups_Security_Entities_Permissions_ZGWSystem_Name_Value_Pairs] FOREIGN KEY ([NVP_SeqID]) REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVP_SeqID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

