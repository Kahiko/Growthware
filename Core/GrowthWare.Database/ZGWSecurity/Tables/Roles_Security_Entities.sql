CREATE TABLE [ZGWSecurity].[Roles_Security_Entities] (
    [Roles_Security_Entities_SeqID] INT      IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Security_Entity_SeqID]         INT      NOT NULL,
    [Role_SeqID]                    INT      NOT NULL,
    [Added_By]                      INT      NOT NULL,
    [Added_Date]                    DATETIME NOT NULL,
    CONSTRAINT [PK_Roles_Security_Entities] PRIMARY KEY CLUSTERED ([Roles_Security_Entities_SeqID] ASC),
    CONSTRAINT [FK_Roles_Security_Entities_Entities] FOREIGN KEY ([Security_Entity_SeqID]) REFERENCES [ZGWSecurity].[Security_Entities] ([Security_Entity_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Roles_Security_Entities_Roles] FOREIGN KEY ([Role_SeqID]) REFERENCES [ZGWSecurity].[Roles] ([Role_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

