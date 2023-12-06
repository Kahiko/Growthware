CREATE TABLE [ZGWSecurity].[Roles_Security_Entities_Permissions] (
    [NVPSeqId]                   INT      NOT NULL,
    [RolesSecurityEntitiesSeqId] INT      NOT NULL,
    [PermissionsNVPDetailSeqId]  INT      NOT NULL,
    [Added_By]                   INT      NOT NULL,
    [Added_Date]                 DATETIME NOT NULL,
    CONSTRAINT [FK_Roles_Security_Entities_Permissions_Permissions] FOREIGN KEY ([PermissionsNVPDetailSeqId]) REFERENCES [ZGWSecurity].[Permissions] ([NVP_DetailSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Roles_Security_Entities_Permissions_Roles_Security_Entities] FOREIGN KEY ([RolesSecurityEntitiesSeqId]) REFERENCES [ZGWSecurity].[Roles_Security_Entities] ([RolesSecurityEntitiesSeqId]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

CREATE UNIQUE CLUSTERED INDEX [UIX_Roles_EntitesSeqId_PermissionsNVPDetailSeqId]
    ON [ZGWSecurity].[Roles_Security_Entities_Permissions]([NVPSeqId] ASC, [RolesSecurityEntitiesSeqId] ASC, [PermissionsNVPDetailSeqId] ASC);


GO

