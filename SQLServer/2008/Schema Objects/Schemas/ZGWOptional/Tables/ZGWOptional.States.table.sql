CREATE TABLE [ZGWOptional].[States] (
    [State]        CHAR (2)   NOT NULL,
    [Description]  VARCHAR (128) NULL,
    [Status_SeqID] INT           NULL,
    [Added_By]     INT           NOT NULL,
    [Added_Date]   DATETIME      NOT NULL,
    [Updated_By]   INT           NULL,
    [Updated_Date] DATETIME      NULL
);

