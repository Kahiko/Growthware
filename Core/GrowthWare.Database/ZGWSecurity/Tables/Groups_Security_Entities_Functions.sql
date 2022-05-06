CREATE TABLE [ZGWSecurity].[Groups_Security_Entities_Functions] (
    [GroupsSecurityEntitiesSeqId] INT      NOT NULL,
    [FunctionSeqId]                 INT      NOT NULL,
    [PermissionsNVPDetailSeqId]   INT      NOT NULL,
    [Added_By]                       INT      NOT NULL,
    [Added_Date]                     DATETIME NOT NULL,
    CONSTRAINT [FK_Groups_Security_Entities_Functions_Functions] FOREIGN KEY ([FunctionSeqId]) REFERENCES [ZGWSecurity].[Functions] ([FunctionSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Groups_Security_Entities_Functions_Groups_Security_Entities] FOREIGN KEY ([GroupsSecurityEntitiesSeqId]) REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([GroupsSecurityEntitiesSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Groups_Security_Entities_Functions_Permissions] FOREIGN KEY ([PermissionsNVPDetailSeqId]) REFERENCES [ZGWSecurity].[Permissions] ([NVP_DetailSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UK_ZFC_FUNCT_PER_GRPS] UNIQUE NONCLUSTERED ([PermissionsNVPDetailSeqId] ASC, [GroupsSecurityEntitiesSeqId] ASC, [FunctionSeqId] ASC)
);


GO

