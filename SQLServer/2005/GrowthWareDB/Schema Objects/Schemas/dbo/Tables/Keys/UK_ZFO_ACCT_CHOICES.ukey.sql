﻿ALTER TABLE [dbo].[ZFO_ACCT_CHOICES]
    ADD CONSTRAINT [UK_ZFO_ACCT_CHOICES] UNIQUE NONCLUSTERED ([ACCT] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF) ON [PRIMARY];

