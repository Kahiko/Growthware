﻿ALTER TABLE [dbo].[ZFC_SECURITY_ACCTS_GRPS]
    ADD CONSTRAINT [DF_ZFC_ACCTS_GRPS_ADDED_BY] DEFAULT ((1)) FOR [ADDED_BY];
