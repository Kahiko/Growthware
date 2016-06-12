ALTER TABLE [ZGWSecurity].[Roles_Security_Entities_Functions]
    ADD CONSTRAINT [DF_ZGWSecurity_Roles_Security_Entities_Functions_Added_Date] DEFAULT (getdate()) FOR [Added_Date];

