CREATE TABLE [ZGWSecurity].[Groups_Security_Entities_Groups] (
    [GroupsSecurityEntitiesSeqId] INT      NOT NULL,
    [GroupSeqId]                    INT      NOT NULL,
    [Added_By]                       INT      NOT NULL,
    [Added_Date]                     DATETIME CONSTRAINT [DF_Groups_Security_Entities_Groups_Added_Date] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [FK_Groups_Security_Entities_Groups_Groups] FOREIGN KEY ([GroupSeqId]) REFERENCES [ZGWSecurity].[Groups] ([GroupSeqId]),
    CONSTRAINT [FK_Groups_Security_Entities_Groups_Groups_Security_Entities] FOREIGN KEY ([GroupsSecurityEntitiesSeqId]) REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([GroupsSecurityEntitiesSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UK_Groups_Security_Entities_Groups] UNIQUE NONCLUSTERED ([GroupsSecurityEntitiesSeqId] ASC, [GroupSeqId] ASC)
);


GO

