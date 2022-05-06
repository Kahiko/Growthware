CREATE TABLE [ZGWSecurity].[Roles_Security_Entities_Permissions] (
    [NVPSeqId]                     INT      NOT NULL,
    [Roles_Security_EntitiesSeqId] INT      NOT NULL,
    [Permissions_NVP_DetailSeqId]  INT      NOT NULL,
    [Added_By]                      INT      NOT NULL,
    [Added_Date]                    DATETIME NOT NULL,
    CONSTRAINT [FK_Roles_Security_Entities_Permissions_Permissions] FOREIGN KEY ([Permissions_NVP_DetailSeqId]) REFERENCES [ZGWSecurity].[Permissions] ([NVP_DetailSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Roles_Security_Entities_Permissions_Roles_Security_Entities] FOREIGN KEY ([Roles_Security_EntitiesSeqId]) REFERENCES [ZGWSecurity].[Roles_Security_Entities] ([Roles_Security_EntitiesSeqId]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

CREATE UNIQUE CLUSTERED INDEX [UIX_Roles_EntitesSeqId_Permissions_NVP_DetailSeqId]
    ON [ZGWSecurity].[Roles_Security_Entities_Permissions]([NVPSeqId] ASC, [Roles_Security_EntitiesSeqId] ASC, [Permissions_NVP_DetailSeqId] ASC);


GO

