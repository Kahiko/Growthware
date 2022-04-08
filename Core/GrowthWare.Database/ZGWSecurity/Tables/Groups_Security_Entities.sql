CREATE TABLE [ZGWSecurity].[Groups_Security_Entities] (
    [Groups_Security_Entities_SeqID] INT      IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Security_Entity_SeqID]          INT      NOT NULL,
    [Group_SeqID]                    INT      NOT NULL,
    [Added_By]                       INT      NOT NULL,
    [Added_Date]                     DATETIME DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ZFC_GRPS_SE] PRIMARY KEY CLUSTERED ([Groups_Security_Entities_SeqID] ASC),
    CONSTRAINT [FK_Groups_Security_Entities_Entities] FOREIGN KEY ([Security_Entity_SeqID]) REFERENCES [ZGWSecurity].[Security_Entities] ([Security_Entity_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ZGWSecurity_Groups_Security_Entities_ZGWSecurity_Groups] FOREIGN KEY ([Group_SeqID]) REFERENCES [ZGWSecurity].[Groups] ([Group_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UK_ZFC_GRPS_SE] UNIQUE NONCLUSTERED ([Security_Entity_SeqID] ASC, [Group_SeqID] ASC)
);


GO

