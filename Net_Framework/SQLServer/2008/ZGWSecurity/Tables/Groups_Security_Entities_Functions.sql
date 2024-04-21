CREATE TABLE [ZGWSecurity].[Groups_Security_Entities_Functions] (
    [Groups_Security_Entities_SeqID] INT      NOT NULL,
    [Function_SeqID]                 INT      NOT NULL,
    [Permissions_NVP_Detail_SeqID]   INT      NOT NULL,
    [Added_By]                       INT      NOT NULL,
    [Added_Date]                     DATETIME NOT NULL,
    CONSTRAINT [FK_Groups_Security_Entities_Functions_Functions] FOREIGN KEY ([Function_SeqID]) REFERENCES [ZGWSecurity].[Functions] ([Function_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Groups_Security_Entities_Functions_Groups_Security_Entities] FOREIGN KEY ([Groups_Security_Entities_SeqID]) REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([Groups_Security_Entities_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Groups_Security_Entities_Functions_Permissions] FOREIGN KEY ([Permissions_NVP_Detail_SeqID]) REFERENCES [ZGWSecurity].[Permissions] ([NVP_Detail_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UK_ZFC_FUNCT_PER_GRPS] UNIQUE NONCLUSTERED ([Permissions_NVP_Detail_SeqID] ASC, [Groups_Security_Entities_SeqID] ASC, [Function_SeqID] ASC)
);

