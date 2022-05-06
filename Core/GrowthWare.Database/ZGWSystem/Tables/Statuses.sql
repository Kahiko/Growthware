CREATE TABLE [ZGWSystem].[Statuses] (
    [StatusSeqId] INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]         CHAR (25)     NOT NULL,
    [Description]  VARCHAR (512) NULL,
    [Added_By]     INT           NULL,
    [Added_Date]   DATETIME      CONSTRAINT [DF_ZGWSystem_Statuses_Added_Date] DEFAULT (getdate()) NOT NULL,
    [Updated_By]   INT           NULL,
    [Updated_Date] DATETIME      NULL,
    CONSTRAINT [PK_ZGWSystem_Statuses] PRIMARY KEY CLUSTERED ([StatusSeqId] ASC)
);


GO

