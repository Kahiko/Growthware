﻿ALTER TABLE [dbo].[ZFC_SECURITY_NVP_GRPS]
    ADD CONSTRAINT [FK_ZFC_NVP_GRPS_SEC_ZFC_GRPS_SE] FOREIGN KEY ([GRPS_SE_SEQ_ID]) REFERENCES [dbo].[ZFC_SECURITY_GRPS_SE] ([GRPS_SE_SEQ_ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
