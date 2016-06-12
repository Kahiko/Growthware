ALTER TABLE [ZGWSecurity].[Groups_Security_Entities_Permissions]
    ADD CONSTRAINT [DF_ZGWSecurity_Groups_Security_Entities_Permissions_Added_Date] DEFAULT (getdate()) FOR [Added_Date];

