CREATE TABLE [ZGWSystem].[Logging] (
    [Account]    VARCHAR (128)  NOT NULL,
    [Component]  VARCHAR (50)   NOT NULL,
    [ClassName]  VARCHAR (50)   NOT NULL,
    [Level]      VARCHAR (5)    NOT NULL,
    [LogDate]    DATETIME       CONSTRAINT [DF_ZGWSystem.Logging_LogDate] DEFAULT (getdate()) NULL,
    [LogSeqId]   INT            IDENTITY (1, 1) NOT NULL,
    [MethodName] VARCHAR (50)   NOT NULL,
    [Msg]        NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [CI_ZGWSystem.Logging] UNIQUE CLUSTERED ([LogDate] ASC)
);


GO

CREATE NONCLUSTERED INDEX [NC_ZGWSystem_Logging_LogDate_Level]
    ON [ZGWSystem].[Logging]([LogDate] ASC, [Level] ASC);


GO

