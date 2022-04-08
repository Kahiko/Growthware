CREATE TABLE [ZGWCoreWeb].[Link_Behaviors] (
    [NVP_Detail_SeqID] INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [NVP_SeqID]        INT           NOT NULL,
    [NVP_Detail_Name]  VARCHAR (50)  NOT NULL,
    [NVP_Detail_Value] VARCHAR (300) NOT NULL,
    [Status_SeqID]     INT           NOT NULL,
    [Sort_Order]       INT           NOT NULL,
    [Added_By]         INT           NOT NULL,
    [Added_DATE]       DATETIME      NOT NULL,
    [Updated_By]       INT           NULL,
    [UPDATED_DATE]     DATETIME      NULL,
    CONSTRAINT [PK_Link_Behaviors] PRIMARY KEY CLUSTERED ([NVP_Detail_SeqID] ASC),
    CONSTRAINT [FK_ZGWCoreWeb_Link_Behaviors_ZGWSystem_Name_Value_Pairs] FOREIGN KEY ([NVP_SeqID]) REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVP_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ZGWCoreWeb_Link_Behaviors_ZGWSystem_Statuses] FOREIGN KEY ([Status_SeqID]) REFERENCES [ZGWSystem].[Statuses] ([Status_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UK_Link_Behaviors] UNIQUE NONCLUSTERED ([NVP_Detail_Name] ASC, [NVP_Detail_Value] ASC)
);


GO

