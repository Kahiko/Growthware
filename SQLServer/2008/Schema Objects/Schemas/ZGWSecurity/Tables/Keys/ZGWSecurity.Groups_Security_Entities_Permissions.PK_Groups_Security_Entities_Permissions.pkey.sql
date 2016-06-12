ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Permissions]
    ADD CONSTRAINT [PK_Groups_Security_Entities_Permissions] PRIMARY KEY CLUSTERED ([Groups_Security_Entities_SeqID] ASC, [NVP_SeqID] ASC, [Permissions_NVP_Detail_SeqID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

