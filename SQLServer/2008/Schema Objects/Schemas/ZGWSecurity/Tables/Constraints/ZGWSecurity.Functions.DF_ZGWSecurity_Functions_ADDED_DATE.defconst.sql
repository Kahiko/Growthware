ALTER TABLE [ZGWSecurity].[Functions]
    ADD CONSTRAINT [DF_ZGWSecurity_Functions_ADDED_DATE] DEFAULT (getdate()) FOR [Added_Date];

