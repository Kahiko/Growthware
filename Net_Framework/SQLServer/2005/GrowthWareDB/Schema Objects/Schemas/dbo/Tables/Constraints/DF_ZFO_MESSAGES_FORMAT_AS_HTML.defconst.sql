﻿ALTER TABLE [dbo].[ZFO_MESSAGES]
    ADD CONSTRAINT [DF_ZFO_MESSAGES_FORMAT_AS_HTML] DEFAULT ((0)) FOR [FORMAT_AS_HTML];

