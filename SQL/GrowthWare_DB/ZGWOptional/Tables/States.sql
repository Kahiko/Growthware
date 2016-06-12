CREATE TABLE [ZGWOptional].[States] (
    [State]        CHAR (2)      NOT NULL,
    [Description]  VARCHAR (128) NULL,
    [Status_SeqID] INT           NULL,
    [Added_By]     INT           CONSTRAINT [DF_ZGWOptional_States_Added_By] DEFAULT ((1)) NOT NULL,
    [Added_Date]   DATETIME      CONSTRAINT [DF_ZGWOptional_States_Added_Date] DEFAULT (getdate()) NOT NULL,
    [Updated_By]   INT           CONSTRAINT [DF_ZGWOptional_States_Updated_By] DEFAULT ((1)) NULL,
    [Updated_Date] DATETIME      CONSTRAINT [DF_ZGWOptional_States_Updated_Date] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_ZGWOptional_States] PRIMARY KEY CLUSTERED ([State] ASC),
    CONSTRAINT [FK_ZGWOptional_States_ZGWSystem_Statuses] FOREIGN KEY ([Status_SeqID]) REFERENCES [ZGWSystem].[Statuses] ([Status_SeqID])
);

