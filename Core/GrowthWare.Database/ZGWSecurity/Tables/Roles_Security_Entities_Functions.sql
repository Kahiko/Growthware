CREATE TABLE [ZGWSecurity].[Roles_Security_Entities_Functions] (
    [Roles_Security_Entities_SeqID] INT      NOT NULL,
    [FunctionSeqId]                INT      NOT NULL,
    [Permissions_NVP_Detail_SeqID]  INT      NOT NULL,
    [Added_By]                      INT      NOT NULL,
    [Added_Date]                    DATETIME CONSTRAINT [DF_ZGWSecurity_Roles_Security_Entities_Functions_Added_Date] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [FK_Roles_Security_Entities_Functions_Functions] FOREIGN KEY ([FunctionSeqId]) REFERENCES [ZGWSecurity].[Functions] ([FunctionSeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Roles_Security_Entities_Functions_Permissions] FOREIGN KEY ([Permissions_NVP_Detail_SeqID]) REFERENCES [ZGWSecurity].[Permissions] ([NVP_Detail_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Roles_Security_Entities_Functions_Roles_Security_Entities] FOREIGN KEY ([Roles_Security_Entities_SeqID]) REFERENCES [ZGWSecurity].[Roles_Security_Entities] ([Roles_Security_Entities_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UK_ZGWSecurity_Permissions_Roles_Security_Entities_Functions] UNIQUE NONCLUSTERED ([Permissions_NVP_Detail_SeqID] ASC, [Roles_Security_Entities_SeqID] ASC, [FunctionSeqId] ASC)
);


GO

