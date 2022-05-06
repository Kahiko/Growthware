CREATE TABLE [ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities] (
    [GroupsSecurityEntitiesSeqId] INT      NOT NULL,
    [RolesSecurityEntitiesSeqId]  INT      NOT NULL,
    [Added_By]                       INT      NOT NULL,
    [Added_Date]                     DATETIME DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [FK_Groups_Security_Entities_Roles_Security_Entities_Groups_Security_Entities] FOREIGN KEY ([GroupsSecurityEntitiesSeqId]) REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([GroupsSecurityEntitiesSeqId]),
    CONSTRAINT [FK_Groups_Security_Entities_Roles_Security_Entities_Roles_Security_Entities] FOREIGN KEY ([RolesSecurityEntitiesSeqId]) REFERENCES [ZGWSecurity].[Roles_Security_Entities] ([RolesSecurityEntitiesSeqId])
);


GO

