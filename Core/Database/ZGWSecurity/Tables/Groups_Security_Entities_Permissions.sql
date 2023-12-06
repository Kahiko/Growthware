CREATE TABLE [ZGWSecurity].[Groups_Security_Entities_Permissions] (
    [GroupsSecurityEntitiesSeqId] INT      NOT NULL,
    [NVPSeqId]                    INT      NOT NULL,
    [PermissionsNVPDetailSeqId]   INT      NOT NULL,
    [Added_By]                    INT      NOT NULL,
    [Added_Date]                  DATETIME CONSTRAINT [DF_ZGWSecurity_Groups_Security_Entities_Permissions_Added_Date] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [FK_ZFC_SECURITY_NVP_GRPS_ZFC_PERMISSIONS] FOREIGN KEY ([PermissionsNVPDetailSeqId]) REFERENCES [ZGWSecurity].[Permissions] ([NVP_DetailSeqId]),
    CONSTRAINT [FK_ZGWSecurity_Groups_Security_Entities_Permissions_ZGWSecurity_Groups_Security_Entities] FOREIGN KEY ([GroupsSecurityEntitiesSeqId]) REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([GroupsSecurityEntitiesSeqId]),
    CONSTRAINT [FK_ZGWSecurity_Groups_Security_Entities_Permissions_ZGWSystem_Name_Value_Pairs] FOREIGN KEY ([NVPSeqId]) REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVPSeqId])
);


GO

