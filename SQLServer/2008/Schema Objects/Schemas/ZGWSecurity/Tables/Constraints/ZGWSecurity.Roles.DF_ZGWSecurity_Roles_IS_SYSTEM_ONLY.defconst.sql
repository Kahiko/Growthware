ALTER TABLE [ZGWSecurity].[Roles]
    ADD CONSTRAINT [DF_ZGWSecurity_Roles_IS_SYSTEM_ONLY] DEFAULT ((0)) FOR [Is_System_Only];

