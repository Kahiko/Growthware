ALTER TABLE [ZGWSecurity].[Functions]
    ADD CONSTRAINT [DF_ZGWSecurity_Functions_REDIRECT_ON_TIMEOUT] DEFAULT ((1)) FOR [Redirect_On_Timeout];

