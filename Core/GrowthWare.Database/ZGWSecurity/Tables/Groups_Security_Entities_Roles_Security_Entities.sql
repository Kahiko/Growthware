CREATE TABLE [ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities] (
    [Groups_Security_EntitiesSeqId] INT      NOT NULL,
    [Roles_Security_EntitiesSeqId]  INT      NOT NULL,
    [Added_By]                       INT      NOT NULL,
    [Added_Date]                     DATETIME DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [FK_Groups_Security_Entities_Roles_Security_Entities_Groups_Security_Entities] FOREIGN KEY ([Groups_Security_EntitiesSeqId]) REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([Groups_Security_EntitiesSeqId]),
    CONSTRAINT [FK_Groups_Security_Entities_Roles_Security_Entities_Roles_Security_Entities] FOREIGN KEY ([Roles_Security_EntitiesSeqId]) REFERENCES [ZGWSecurity].[Roles_Security_Entities] ([Roles_Security_EntitiesSeqId])
);


GO

