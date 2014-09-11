CREATE TABLE [ZGWSecurity].[Permissions] (
    [NVP_Detail_SeqID] INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [NVP_SeqID]        INT           NOT NULL,
    [NVP_Detail_Name]  VARCHAR (50)  NOT NULL,
    [NVP_Detail_Value] VARCHAR (300) NOT NULL,
    [Status_SeqID]     INT           NOT NULL,
    [SORT_ORDER]       INT           NOT NULL,
    [ADDED_BY]         INT           NOT NULL,
    [ADDED_DATE]       DATETIME      NOT NULL,
    [UPDATED_BY]       INT           NULL,
    [UPDATED_DATE]     DATETIME      NULL,
    CONSTRAINT [PK_ZGWSecurity_Permissions] PRIMARY KEY CLUSTERED ([NVP_Detail_SeqID] ASC),
    CONSTRAINT [FK_Permissions_Name_Value_Pairs] FOREIGN KEY ([NVP_SeqID]) REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVP_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ZGWSecurity_Permissions_ZGWSystem_Statuses] FOREIGN KEY ([Status_SeqID]) REFERENCES [ZGWSystem].[Statuses] ([Status_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UK_ZGWSecurity_Permissions] UNIQUE NONCLUSTERED ([NVP_Detail_Name] ASC, [NVP_Detail_Value] ASC)
);

