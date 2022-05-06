CREATE TABLE [ZGWSecurity].[Roles_Security_Entities_Functions] (
    [RolesSecurityEntitiesSeqId] INT      NOT NULL,
    [FunctionSeqId]                INT      NOT NULL,
    [PermissionsNVPDetailSeqId]  INT      NOT NULL,
    [Added_By]                      INT      NOT NULL,
    [Added_Date]                    DATETIME CONSTRAINT [DF_ZGWSecurity_Roles_Security_Entities_Functions_Added_Date] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [FK_Roles_Security_Entities_Functions_Functions] FOREIGN KEY ([FunctionSeqId]) REFERENCES [ZGWSecurity].[Functions] ([FunctionSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Roles_Security_Entities_Functions_Permissions] FOREIGN KEY ([PermissionsNVPDetailSeqId]) REFERENCES [ZGWSecurity].[Permissions] ([NVP_DetailSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Roles_Security_Entities_Functions_Roles_Security_Entities] FOREIGN KEY ([RolesSecurityEntitiesSeqId]) REFERENCES [ZGWSecurity].[Roles_Security_Entities] ([RolesSecurityEntitiesSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UK_ZGWSecurity_Permissions_Roles_Security_Entities_Functions] UNIQUE NONCLUSTERED ([PermissionsNVPDetailSeqId] ASC, [RolesSecurityEntitiesSeqId] ASC, [FunctionSeqId] ASC)
);


GO

