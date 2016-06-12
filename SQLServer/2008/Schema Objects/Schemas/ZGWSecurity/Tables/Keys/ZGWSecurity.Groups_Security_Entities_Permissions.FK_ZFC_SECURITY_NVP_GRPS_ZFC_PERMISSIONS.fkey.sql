ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Permissions]
    ADD CONSTRAINT [FK_ZFC_SECURITY_NVP_GRPS_ZFC_PERMISSIONS] FOREIGN KEY ([Permissions_NVP_Detail_SeqID]) REFERENCES [ZGWSecurity].[Permissions] ([NVP_Detail_SeqID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

