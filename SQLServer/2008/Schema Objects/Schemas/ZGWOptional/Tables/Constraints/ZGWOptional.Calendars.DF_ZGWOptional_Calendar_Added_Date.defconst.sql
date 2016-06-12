ALTER TABLE [ZGWOptional].[Calendars]
    ADD CONSTRAINT [DF_ZGWOptional_Calendar_Added_Date] DEFAULT (getdate()) FOR [Added_Date];

