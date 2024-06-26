CREATE TABLE [ZGWSystem].[Data_Errors] (
    [ErrorSeqId]     INT                  IDENTITY (1, 1) NOT NULL,
    [ErrorNumber]    INT                  NULL,
    [ErrorSeverity]  INT                  NULL,
    [ErrorState]     INT                  NULL,
    [ErrorProcedure] VARCHAR (MAX)        NULL,
    [ErrorLine]      INT                  NULL,
    [ErrorMessage]   VARCHAR (MAX) SPARSE NULL,
    [ErrorDate]      DATETIME             CONSTRAINT [DF_ZGWSystem.Data_Errors_ErrorDate] DEFAULT (getdate()) NOT NULL,
    [Parameters]     VARCHAR (MAX) SPARSE NULL
);


GO

