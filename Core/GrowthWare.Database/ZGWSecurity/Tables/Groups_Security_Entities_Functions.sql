CREATE TABLE [ZGWSecurity].[Groups_Security_Entities_Functions] (
    [Groups_Security_EntitiesSeqId] INT      NOT NULL,
    [FunctionSeqId]                 INT      NOT NULL,
    [Permissions_NVP_DetailSeqId]   INT      NOT NULL,
    [Added_By]                       INT      NOT NULL,
    [Added_Date]                     DATETIME NOT NULL,
    CONSTRAINT [FK_Groups_Security_Entities_Functions_Functions] FOREIGN KEY ([FunctionSeqId]) REFERENCES [ZGWSecurity].[Functions] ([FunctionSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Groups_Security_Entities_Functions_Groups_Security_Entities] FOREIGN KEY ([Groups_Security_EntitiesSeqId]) REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([Groups_Security_EntitiesSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Groups_Security_Entities_Functions_Permissions] FOREIGN KEY ([Permissions_NVP_DetailSeqId]) REFERENCES [ZGWSecurity].[Permissions] ([NVP_DetailSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UK_ZFC_FUNCT_PER_GRPS] UNIQUE NONCLUSTERED ([Permissions_NVP_DetailSeqId] ASC, [Groups_Security_EntitiesSeqId] ASC, [FunctionSeqId] ASC)
);


GO

