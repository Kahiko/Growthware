ALTER TABLE [ZGWOptional].[Zip_Codes]
    ADD CONSTRAINT [FK_ZGWOptional_Zip_Codes_ZGWOptional_States] FOREIGN KEY ([State]) REFERENCES [ZGWOptional].[States] ([State]) ON DELETE NO ACTION ON UPDATE NO ACTION;

