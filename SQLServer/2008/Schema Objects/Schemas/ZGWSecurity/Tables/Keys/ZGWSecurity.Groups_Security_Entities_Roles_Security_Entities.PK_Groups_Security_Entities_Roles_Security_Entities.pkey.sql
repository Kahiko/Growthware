ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities]
    ADD CONSTRAINT [PK_Groups_Security_Entities_Roles_Security_Entities] PRIMARY KEY CLUSTERED ([Groups_Security_Entities_SeqID] ASC, [Roles_Security_Entities_SeqID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

