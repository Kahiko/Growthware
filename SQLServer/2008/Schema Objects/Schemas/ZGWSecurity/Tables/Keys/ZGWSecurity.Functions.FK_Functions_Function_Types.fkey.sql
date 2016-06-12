ALTER TABLE [ZGWSecurity].[Functions]
    ADD CONSTRAINT [FK_Functions_Function_Types] FOREIGN KEY ([Function_Type_SeqID]) REFERENCES [ZGWSecurity].[Function_Types] ([Function_Type_SeqID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

