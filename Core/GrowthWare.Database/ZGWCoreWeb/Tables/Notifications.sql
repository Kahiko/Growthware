CREATE TABLE [ZGWCoreWeb].[Notifications] (
    [NotificationSeqId]   INT      IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SecurityEntitySeqId] INT      NOT NULL,
    [FunctionSeqId]       INT      NOT NULL,
    [Added_By]            INT      NOT NULL,
    [Added_Date]          DATETIME CONSTRAINT [DF_ZGWCoreWeb_NOTIFICATIONS_ADDED_DATE] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ZGWCoreWeb_NOTIFICATIONS] PRIMARY KEY CLUSTERED ([NotificationSeqId] ASC),
    CONSTRAINT [FK_Notifications_Entities] FOREIGN KEY ([SecurityEntitySeqId]) REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Notifications_Functions] FOREIGN KEY ([FunctionSeqId]) REFERENCES [ZGWSecurity].[Functions] ([FunctionSeqId]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

CREATE NONCLUSTERED INDEX [FK_IX_Notification_Entity_Function]
    ON [ZGWCoreWeb].[Notifications]([NotificationSeqId] ASC, [SecurityEntitySeqId] ASC, [FunctionSeqId] ASC);


GO

