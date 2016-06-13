CREATE TABLE [ZGWSecurity].[Navigation_Types] (
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
    CONSTRAINT [PK_ZGWSecurity_Navigation_Types] PRIMARY KEY CLUSTERED ([NVP_Detail_SeqID] ASC),
    CONSTRAINT [FK_ZGWSecurity_Navigation_Types_ZGWSystem_Name_Value_Pairs] FOREIGN KEY ([NVP_SeqID]) REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVP_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ZGWSecurity_Navigation_Types_ZGWSystem_Statuses] FOREIGN KEY ([Status_SeqID]) REFERENCES [ZGWSystem].[Statuses] ([Status_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UK_ZGWSecurity_Navigation_Types] UNIQUE NONCLUSTERED ([NVP_Detail_Name] ASC, [NVP_Detail_Value] ASC)
);

