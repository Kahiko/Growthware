﻿ALTER TABLE [dbo].[ZFC_ACCTS]
    ADD CONSTRAINT [PK_ZFC_ACCTS] PRIMARY KEY CLUSTERED ([ACCT_SEQ_ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);
