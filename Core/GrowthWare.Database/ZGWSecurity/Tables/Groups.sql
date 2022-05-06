CREATE TABLE [ZGWSecurity].[Groups] (
    [GroupSeqId]  INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]         VARCHAR (128) NOT NULL,
    [Description]  VARCHAR (512) NOT NULL,
    [Added_By]     INT           NOT NULL,
    [Added_Date]   DATETIME      DEFAULT (getdate()) NOT NULL,
    [Updated_By]   INT           NULL,
    [Updated_Date] DATETIME      NULL,
    CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED ([GroupSeqId] ASC),
    CONSTRAINT [UK_ZGWSecurity_Groups] UNIQUE NONCLUSTERED ([Name] ASC)
);


GO

