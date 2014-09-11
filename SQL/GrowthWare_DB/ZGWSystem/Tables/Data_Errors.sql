CREATE TABLE [ZGWSystem].[Data_Errors] (
    [Error_SeqID]    INT                  IDENTITY (1, 1) NOT NULL,
    [ErrorNumber]    INT                  NOT NULL,
    [ErrorSeverity]  INT                  NOT NULL,
    [ErrorState]     INT                  NOT NULL,
    [ErrorProcedure] VARCHAR (MAX)        NULL,
    [ErrorLine]      INT                  NULL,
    [ErrorMessage]   VARCHAR (MAX) SPARSE NULL,
    [ErrorDate]      DATETIME             CONSTRAINT [DF_ZGWSystem.Data_Errors_ErrorDate] DEFAULT (getdate()) NOT NULL,
    [Parameters]     VARCHAR (MAX) SPARSE NULL
);

