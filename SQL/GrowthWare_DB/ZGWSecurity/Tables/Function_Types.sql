CREATE TABLE [ZGWSecurity].[Function_Types] (
    [Function_Type_SeqID] INT                  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]                VARCHAR (50)         NOT NULL,
    [Description]         VARCHAR (512)        NOT NULL,
    [Template]            VARCHAR (512) SPARSE NULL,
    [Is_Content]          INT                  NOT NULL,
    [Added_By]            INT                  NOT NULL,
    [Added_Date]          DATETIME             CONSTRAINT [DF_ZGWCoreWeb_Function_Types_Added_Date] DEFAULT (getdate()) NOT NULL,
    [Updated_By]          INT                  NULL,
    [Updated_Date]        DATETIME             NULL,
    CONSTRAINT [PK_ZGWCoreWeb_Function_Types] PRIMARY KEY CLUSTERED ([Function_Type_SeqID] ASC),
    CONSTRAINT [UK_ZGWCoreWeb_Function_Types] UNIQUE NONCLUSTERED ([Name] ASC)
);

