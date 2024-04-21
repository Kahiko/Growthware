CREATE TABLE [ZGWCoreWeb].[Work_Flows] (
    [NVP_Detail_SeqID] INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [NVP_SeqID]        INT           NOT NULL,
    [NVP_Detail_Name]  VARCHAR (50)  NOT NULL,
    [NVP_Detail_Value] VARCHAR (300) NOT NULL,
    [Status_SeqID]     INT           NOT NULL,
    [Sort_Order]       INT           NOT NULL,
    [Added_By]         INT           NOT NULL,
    [Added_Date]       DATETIME      NOT NULL,
    [Updated_By]       INT           NULL,
    [Updated_Date]     DATETIME      NULL,
    CONSTRAINT [PK_ZGWCoreWeb_Work_Flows] PRIMARY KEY CLUSTERED ([NVP_Detail_SeqID] ASC),
    CONSTRAINT [FK_ZGWCoreWeb_Work_Flows_ZGWSystem_NVP] FOREIGN KEY ([NVP_SeqID]) REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVP_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ZGWCoreWeb_Work_Flows_ZGWSystem_Statuses] FOREIGN KEY ([Status_SeqID]) REFERENCES [ZGWSystem].[Statuses] ([Status_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UK_ZGWCoreWeb_Work_Flows] UNIQUE NONCLUSTERED ([NVP_Detail_Name] ASC, [NVP_Detail_Value] ASC)
);


GO
CREATE NONCLUSTERED INDEX [FX_IX_Work_Flows]
    ON [ZGWCoreWeb].[Work_Flows]([NVP_SeqID] ASC, [Status_SeqID] ASC);

