CREATE TABLE [ZGWSecurity].[Roles_Security_Entities] (
    [RolesSecurityEntitiesSeqId] INT      IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SecurityEntitySeqId]        INT      NOT NULL,
    [RoleSeqId]                  INT      NOT NULL,
    [Added_By]                   INT      NOT NULL,
    [Added_Date]                 DATETIME NOT NULL,
    CONSTRAINT [PK_Roles_Security_Entities] PRIMARY KEY CLUSTERED ([RolesSecurityEntitiesSeqId] ASC),
    CONSTRAINT [FK_Roles_Security_Entities_Entities] FOREIGN KEY ([SecurityEntitySeqId]) REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Roles_Security_Entities_Roles] FOREIGN KEY ([RoleSeqId]) REFERENCES [ZGWSecurity].[Roles] ([RoleSeqId]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

