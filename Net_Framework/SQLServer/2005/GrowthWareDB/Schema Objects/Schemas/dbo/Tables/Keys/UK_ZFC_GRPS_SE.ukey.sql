﻿ALTER TABLE [dbo].[ZFC_SECURITY_GRPS_SE]
    ADD CONSTRAINT [UK_ZFC_GRPS_SE] UNIQUE NONCLUSTERED ([SE_SEQ_ID] ASC, [GROUP_SEQ_ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF) ON [PRIMARY];

