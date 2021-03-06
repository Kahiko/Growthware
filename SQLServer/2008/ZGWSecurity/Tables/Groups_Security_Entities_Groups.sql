﻿CREATE TABLE [ZGWSecurity].[Groups_Security_Entities_Groups] (
    [Groups_Security_Entities_SeqID] INT      NOT NULL,
    [Group_SeqID]                    INT      NOT NULL,
    [Added_By]                       INT      NOT NULL,
    [Added_Date]                     DATETIME CONSTRAINT [DF_Groups_Security_Entities_Groups_Added_Date] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [FK_Groups_Security_Entities_Groups_Groups] FOREIGN KEY ([Group_SeqID]) REFERENCES [ZGWSecurity].[Groups] ([Group_SeqID]),
    CONSTRAINT [FK_Groups_Security_Entities_Groups_Groups_Security_Entities] FOREIGN KEY ([Groups_Security_Entities_SeqID]) REFERENCES [ZGWSecurity].[Groups_Security_Entities] ([Groups_Security_Entities_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UK_Groups_Security_Entities_Groups] UNIQUE NONCLUSTERED ([Groups_Security_Entities_SeqID] ASC, [Group_SeqID] ASC)
);

