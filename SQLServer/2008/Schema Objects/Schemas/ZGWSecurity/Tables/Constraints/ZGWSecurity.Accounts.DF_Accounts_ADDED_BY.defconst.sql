ALTER TABLE [ZGWSecurity].[Accounts]
    ADD CONSTRAINT [DF_Accounts_ADDED_BY] DEFAULT ((1)) FOR [Added_By];

