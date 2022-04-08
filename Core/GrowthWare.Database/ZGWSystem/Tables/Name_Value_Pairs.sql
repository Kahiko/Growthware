CREATE TABLE [ZGWSystem].[Name_Value_Pairs] (
    [NVP_SeqID]    INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Schema_Name]  VARCHAR (30)  NOT NULL,
    [Static_Name]  VARCHAR (30)  NOT NULL,
    [Display]      VARCHAR (128) NOT NULL,
    [Description]  VARCHAR (256) NOT NULL,
    [Status_SeqID] INT           NOT NULL,
    [Added_By]     INT           NOT NULL,
    [Added_Date]   DATETIME      DEFAULT (getdate()) NOT NULL,
    [Updated_By]   INT           NULL,
    [Updated_Date] DATETIME      NULL,
    CONSTRAINT [PK_ZGWSystem_Name_Value_Pairs] PRIMARY KEY CLUSTERED ([NVP_SeqID] ASC),
    CONSTRAINT [FK_Name_Value_Pairs_Statuses] FOREIGN KEY ([Status_SeqID]) REFERENCES [ZGWSystem].[Statuses] ([Status_SeqID])
);


GO

