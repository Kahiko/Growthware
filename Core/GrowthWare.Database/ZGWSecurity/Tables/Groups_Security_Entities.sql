CREATE TABLE [ZGWSecurity].[Groups_Security_Entities] (
    [GroupsSecurityEntitiesSeqId] INT      IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SecurityEntitySeqId]          INT      NOT NULL,
    [GroupSeqId]                    INT      NOT NULL,
    [Added_By]                       INT      NOT NULL,
    [Added_Date]                     DATETIME DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ZFC_GRPS_SE] PRIMARY KEY CLUSTERED ([GroupsSecurityEntitiesSeqId] ASC),
    CONSTRAINT [FK_Groups_Security_Entities_Entities] FOREIGN KEY ([SecurityEntitySeqId]) REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ZGWSecurity_Groups_Security_Entities_ZGWSecurity_Groups] FOREIGN KEY ([GroupSeqId]) REFERENCES [ZGWSecurity].[Groups] ([GroupSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UK_ZFC_GRPS_SE] UNIQUE NONCLUSTERED ([SecurityEntitySeqId] ASC, [GroupSeqId] ASC)
);


GO

