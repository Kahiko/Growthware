ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Functions]
    ADD CONSTRAINT [FK_Roles_Security_Entities_Functions_Permissions] FOREIGN KEY ([Permissions_NVP_Detail_SeqID]) REFERENCES [ZGWSecurity].[Permissions] ([NVP_Detail_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE;

