CREATE TABLE [ZGWSecurity].[Groups_Security_Entities_Permissions] (
    [Groups_Security_EntitiesSeqId] INT      NOT NULL,
    [NVPSeqId]                      INT      NOT NULL,
    [Permissions_NVP_DetailSeqId]   INT      NOT NULL,
    [Added_By]                       INT      NOT NULL,
    [Added_Date]                     DATETIME CONSTRAINT [DF_ZGWSecurity_Groups_Security_Entities_Permissions_Added_Date] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [FK_ZFC_SECURITY_NVP_GRPS_ZFC_PERMISSIONS] FOREIGN KEY ([Permissions_NVP_DetailSeqId]) REFERENCES [ZGWSecurity].[Permissions] ([NVP_DetailSeqId]),
    CONSTRAINT [FK_ZGWSecurity_Groups_Security_Entities_Permissions_ZGWSecurity_Groups_Security_Entities] FOREIGN KEY ([Groups_Security_EntitiesSeqId]) REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([Groups_Security_EntitiesSeqId]),
    CONSTRAINT [FK_ZGWSecurity_Groups_Security_Entities_Permissions_ZGWSystem_Name_Value_Pairs] FOREIGN KEY ([NVPSeqId]) REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVPSeqId])
);


GO

