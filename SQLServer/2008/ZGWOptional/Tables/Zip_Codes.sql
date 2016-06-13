CREATE TABLE [ZGWOptional].[Zip_Codes] (
    [State]     CHAR (2)      NOT NULL,
    [Zip_Code]  INT           NOT NULL,
    [Area_Code] INT           NOT NULL,
    [City]      VARCHAR (255) NULL,
    [Time_Zone] VARCHAR (255) NULL,
    CONSTRAINT [FK_ZGWOptional_Zip_Codes_ZGWOptional_States] FOREIGN KEY ([State]) REFERENCES [ZGWOptional].[States] ([State]),
    CONSTRAINT [UK_ZGWOptional_Zip_Codes] UNIQUE NONCLUSTERED ([State] ASC, [Zip_Code] ASC, [City] ASC)
);

