ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Permissions]
    ADD CONSTRAINT [PK_ZGWSecurity_Roles_Security_Entities_Permissions] PRIMARY KEY NONCLUSTERED ([NVP_SeqID] ASC, [Roles_Security_Entities_SeqID] ASC, [Permissions_NVP_Detail_SeqID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF) ON [PRIMARY];

