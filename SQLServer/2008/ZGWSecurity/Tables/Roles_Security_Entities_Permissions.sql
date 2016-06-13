CREATE TABLE [ZGWSecurity].[Roles_Security_Entities_Permissions] (
    [NVP_SeqID]                     INT      NOT NULL,
    [Roles_Security_Entities_SeqID] INT      NOT NULL,
    [Permissions_NVP_Detail_SeqID]  INT      NOT NULL,
    [Added_By]                      INT      NOT NULL,
    [Added_Date]                    DATETIME NOT NULL,
    CONSTRAINT [FK_Roles_Security_Entities_Permissions_Permissions] FOREIGN KEY ([Permissions_NVP_Detail_SeqID]) REFERENCES [ZGWSecurity].[Permissions] ([NVP_Detail_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Roles_Security_Entities_Permissions_Roles_Security_Entities] FOREIGN KEY ([Roles_Security_Entities_SeqID]) REFERENCES [ZGWSecurity].[Roles_Security_Entities] ([Roles_Security_Entities_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
CREATE UNIQUE CLUSTERED INDEX [UIX_Roles_Entites_SeqID_Permissions_NVP_Detail_SeqID]
    ON [ZGWSecurity].[Roles_Security_Entities_Permissions]([NVP_SeqID] ASC, [Roles_Security_Entities_SeqID] ASC, [Permissions_NVP_Detail_SeqID] ASC);

