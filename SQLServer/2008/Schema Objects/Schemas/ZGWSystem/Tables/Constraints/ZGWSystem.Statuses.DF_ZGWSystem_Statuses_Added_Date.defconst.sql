ALTER TABLE [ZGWSystem].[Statuses]
    ADD CONSTRAINT [DF_ZGWSystem_Statuses_Added_Date] DEFAULT (getdate()) FOR [Added_Date];

