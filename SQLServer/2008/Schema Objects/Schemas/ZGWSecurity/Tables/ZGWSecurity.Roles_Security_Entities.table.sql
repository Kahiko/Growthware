CREATE TABLE [ZGWSecurity].[Roles_Security_Entities] (
    [Roles_Security_Entities_SeqID] INT      IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Security_Entity_SeqID]         INT      NOT NULL,
    [Role_SeqID]                    INT      NOT NULL,
    [Added_By]                      INT      NOT NULL,
    [Added_Date]                    DATETIME NOT NULL
);

