﻿ALTER TABLE [dbo].[ZFC_SECURITY_RLS_SE]
    ADD CONSTRAINT [DF_ZFC_RLS_SE_ADDED_DATE] DEFAULT (getdate()) FOR [ADDED_DATE];

