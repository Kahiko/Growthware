CREATE TABLE [ZGWCoreWeb].[Link_Behaviors] (
    [NVP_DetailSeqId] INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [NVPSeqId]        INT           NOT NULL,
    [NVP_Detail_Name]  VARCHAR (50)  NOT NULL,
    [NVP_Detail_Value] VARCHAR (300) NOT NULL,
    [StatusSeqId]     INT           NOT NULL,
    [Sort_Order]       INT           NOT NULL,
    [Added_By]         INT           NOT NULL,
    [Added_DATE]       DATETIME      NOT NULL,
    [Updated_By]       INT           NULL,
    [UPDATED_DATE]     DATETIME      NULL,
    CONSTRAINT [PK_Link_Behaviors] PRIMARY KEY CLUSTERED ([NVP_DetailSeqId] ASC),
    CONSTRAINT [FK_ZGWCoreWeb_Link_Behaviors_ZGWSystem_Name_Value_Pairs] FOREIGN KEY ([NVPSeqId]) REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVPSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ZGWCoreWeb_Link_Behaviors_ZGWSystem_Statuses] FOREIGN KEY ([StatusSeqId]) REFERENCES [ZGWSystem].[Statuses] ([StatusSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UK_Link_Behaviors] UNIQUE NONCLUSTERED ([NVP_Detail_Name] ASC, [NVP_Detail_Value] ASC)
);


GO

