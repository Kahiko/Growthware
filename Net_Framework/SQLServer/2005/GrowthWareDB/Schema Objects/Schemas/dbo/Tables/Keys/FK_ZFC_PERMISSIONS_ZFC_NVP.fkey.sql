﻿ALTER TABLE [dbo].[ZFC_PERMISSIONS]
    ADD CONSTRAINT [FK_ZFC_PERMISSIONS_ZFC_NVP] FOREIGN KEY ([NVP_SEQ_ID]) REFERENCES [dbo].[ZFC_NVP] ([NVP_SEQ_ID]) ON DELETE CASCADE ON UPDATE CASCADE;
