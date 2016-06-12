CREATE TABLE [ZGWSecurity].[Groups_Security_Entities] (
    [Groups_Security_Entities_SeqID] INT      IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Security_Entity_SeqID]          INT      NOT NULL,
    [Group_SeqID]                    INT      NOT NULL,
    [ADDED_BY]                       INT      NOT NULL,
    [ADDED_DATE]                     DATETIME DEFAULT (getdate()) NOT NULL
);

