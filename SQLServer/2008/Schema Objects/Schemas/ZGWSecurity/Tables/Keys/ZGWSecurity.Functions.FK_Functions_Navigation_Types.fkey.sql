ALTER TABLE [ZGWSecurity].[Functions]
    ADD CONSTRAINT [FK_Functions_Navigation_Types] FOREIGN KEY ([Navigation_Types_NVP_Detail_SeqID]) REFERENCES [ZGWSecurity].[Navigation_Types] ([NVP_Detail_SeqID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

