﻿ALTER TABLE [dbo].[ZFC_SECURITY_ENTITIES]
    ADD CONSTRAINT [FK_ZFC_SECURITY_ENTITIES_ZFC_SECURITY_ENTITIES] FOREIGN KEY ([PARENT_SE_SEQ_ID]) REFERENCES [dbo].[ZFC_SECURITY_ENTITIES] ([SE_SEQ_ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

