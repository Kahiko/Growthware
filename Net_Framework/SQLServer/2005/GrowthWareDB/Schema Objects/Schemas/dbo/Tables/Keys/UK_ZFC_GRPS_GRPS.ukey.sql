﻿ALTER TABLE [dbo].[ZFC_SECURITY_GRPS_GRPS]
    ADD CONSTRAINT [UK_ZFC_GRPS_GRPS] UNIQUE NONCLUSTERED ([GRPS_SE_SEQ_ID] ASC, [GROUP_SEQ_ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF) ON [PRIMARY];

