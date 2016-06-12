ALTER TABLE [ZGWSystem].[Data_Errors]
    ADD CONSTRAINT [DF_ZGWSystem.Data_Errors_ErrorDate] DEFAULT (getdate()) FOR [ErrorDate];

