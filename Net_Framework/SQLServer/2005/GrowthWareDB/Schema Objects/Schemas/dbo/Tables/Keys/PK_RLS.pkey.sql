﻿ALTER TABLE [dbo].[ZFC_SECURITY_RLS]
    ADD CONSTRAINT [PK_RLS] PRIMARY KEY CLUSTERED ([ROLE_SEQ_ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

