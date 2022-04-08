CREATE TABLE [ZGWSecurity].[Groups_Security_Entities_Permissions] (
    [Groups_Security_Entities_SeqID] INT      NOT NULL,
    [NVP_SeqID]                      INT      NOT NULL,
    [Permissions_NVP_Detail_SeqID]   INT      NOT NULL,
    [Added_By]                       INT      NOT NULL,
    [Added_Date]                     DATETIME CONSTRAINT [DF_ZGWSecurity_Groups_Security_Entities_Permissions_Added_Date] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [FK_ZFC_SECURITY_NVP_GRPS_ZFC_PERMISSIONS] FOREIGN KEY ([Permissions_NVP_Detail_SeqID]) REFERENCES [ZGWSecurity].[Permissions] ([NVP_Detail_SeqID]),
    CONSTRAINT [FK_ZGWSecurity_Groups_Security_Entities_Permissions_ZGWSecurity_Groups_Security_Entities] FOREIGN KEY ([Groups_Security_Entities_SeqID]) REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([Groups_Security_Entities_SeqID]),
    CONSTRAINT [FK_ZGWSecurity_Groups_Security_Entities_Permissions_ZGWSystem_Name_Value_Pairs] FOREIGN KEY ([NVP_SeqID]) REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVP_SeqID])
);


GO

