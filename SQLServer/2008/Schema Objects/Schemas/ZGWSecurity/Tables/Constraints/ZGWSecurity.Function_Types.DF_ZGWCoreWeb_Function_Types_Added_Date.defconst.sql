ALTER TABLE [ZGWSecurity].[Function_Types]
    ADD CONSTRAINT [DF_ZGWCoreWeb_Function_Types_Added_Date] DEFAULT (getdate()) FOR [Added_Date];

