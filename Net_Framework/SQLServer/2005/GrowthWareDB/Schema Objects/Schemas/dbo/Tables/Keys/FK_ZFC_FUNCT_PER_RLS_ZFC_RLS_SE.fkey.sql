﻿ALTER TABLE [dbo].[ZFC_SECURITY_FUNCT_RLS]
    ADD CONSTRAINT [FK_ZFC_FUNCT_PER_RLS_ZFC_RLS_SE] FOREIGN KEY ([RLS_SE_SEQ_ID]) REFERENCES [dbo].[ZFC_SECURITY_RLS_SE] ([RLS_SE_SEQ_ID]) ON DELETE CASCADE ON UPDATE CASCADE;
