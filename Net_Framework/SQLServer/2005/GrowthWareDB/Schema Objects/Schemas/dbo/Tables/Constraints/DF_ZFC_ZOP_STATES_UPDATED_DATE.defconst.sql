﻿ALTER TABLE [dbo].[ZOP_STATES]
    ADD CONSTRAINT [DF_ZFC_ZOP_STATES_UPDATED_DATE] DEFAULT (getdate()) FOR [UPDATED_DATE];
