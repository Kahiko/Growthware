ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Functions]
    ADD CONSTRAINT [FK_Groups_Security_Entities_Functions_Functions] FOREIGN KEY ([Function_SeqID]) REFERENCES [ZGWSecurity].[Functions] ([Function_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE;

