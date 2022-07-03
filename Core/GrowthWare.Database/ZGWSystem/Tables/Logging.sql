CREATE TABLE [ZGWSystem].[Logging] (
    [LogDate]       DATETIME        CONSTRAINT [DF_ZGWSystem.Logging_LogDate] DEFAULT (GETDATE()),
    [Level]         VARCHAR (5)     NOT NULL,
    [Account]       VARCHAR (128)   NOT NULL,
    [Component]     VARCHAR (50)    NOT NULL,
    [ClassName]     VARCHAR (50)    NOT NULL,
    [MethodName]    VARCHAR (50)    NOT NULL,
    [Msg]           VARCHAR (MAX)   NOT NULL,
    CONSTRAINT [CI_ZGWSystem.Logging] UNIQUE CLUSTERED ([LogDate] ASC)
);

CREATE NONCLUSTERED INDEX NC_ZGWSystem_Logging_LogDate_Level
ON [ZGWSystem].[Logging](LogDate, Level);

GO
