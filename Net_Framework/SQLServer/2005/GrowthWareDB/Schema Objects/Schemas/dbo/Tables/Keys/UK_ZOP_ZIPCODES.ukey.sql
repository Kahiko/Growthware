﻿ALTER TABLE [dbo].[ZOP_ZIPCODES]
    ADD CONSTRAINT [UK_ZOP_ZIPCODES] UNIQUE NONCLUSTERED ([STATE] ASC, [ZIP_CODE] ASC, [CITY] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF) ON [PRIMARY];
