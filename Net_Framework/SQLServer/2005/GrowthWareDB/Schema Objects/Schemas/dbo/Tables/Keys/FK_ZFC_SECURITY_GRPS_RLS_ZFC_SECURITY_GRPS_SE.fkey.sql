﻿ALTER TABLE [dbo].[ZFC_SECURITY_GRPS_RLS]
    ADD CONSTRAINT [FK_ZFC_SECURITY_GRPS_RLS_ZFC_SECURITY_GRPS_SE] FOREIGN KEY ([GRPS_SE_SEQ_ID]) REFERENCES [dbo].[ZFC_SECURITY_GRPS_SE] ([GRPS_SE_SEQ_ID]) ON DELETE CASCADE ON UPDATE CASCADE;
