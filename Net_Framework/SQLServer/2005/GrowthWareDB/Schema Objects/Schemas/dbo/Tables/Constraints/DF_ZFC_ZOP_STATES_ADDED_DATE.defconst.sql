﻿ALTER TABLE [dbo].[ZOP_STATES]
    ADD CONSTRAINT [DF_ZFC_ZOP_STATES_ADDED_DATE] DEFAULT (getdate()) FOR [ADDED_DATE];

