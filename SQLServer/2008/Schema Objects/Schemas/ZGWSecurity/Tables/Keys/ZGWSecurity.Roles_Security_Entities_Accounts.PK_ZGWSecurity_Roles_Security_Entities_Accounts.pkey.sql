ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Accounts]
    ADD CONSTRAINT [PK_ZGWSecurity_Roles_Security_Entities_Accounts] PRIMARY KEY CLUSTERED ([Roles_Security_Entities_SeqID] ASC, [Account_SeqID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

