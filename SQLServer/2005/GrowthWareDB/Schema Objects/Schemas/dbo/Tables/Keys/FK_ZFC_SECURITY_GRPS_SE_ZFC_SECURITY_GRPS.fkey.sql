﻿ALTER TABLE [dbo].[ZFC_SECURITY_GRPS_SE]
    ADD CONSTRAINT [FK_ZFC_SECURITY_GRPS_SE_ZFC_SECURITY_GRPS] FOREIGN KEY ([GROUP_SEQ_ID]) REFERENCES [dbo].[ZFC_SECURITY_GRPS] ([GROUP_SEQ_ID]) ON DELETE CASCADE ON UPDATE CASCADE;

