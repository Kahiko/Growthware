﻿ALTER TABLE [dbo].[ZFO_WORK_FLOWS]
    ADD CONSTRAINT [FK_ZFO_WORK_FLOWS_ZFC_NVP] FOREIGN KEY ([NVP_SEQ_ID]) REFERENCES [dbo].[ZFC_NVP] ([NVP_SEQ_ID]) ON DELETE CASCADE ON UPDATE CASCADE;

