CREATE TABLE [ZGWCoreWeb].[Notifications] (
    [Notification_SeqID]    INT      IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Security_Entity_SeqID] INT      NOT NULL,
    [Function_SeqID]        INT      NOT NULL,
    [Added_By]              INT      NOT NULL,
    [Added_Date]            DATETIME CONSTRAINT [DF_ZGWCoreWeb_NOTIFICATIONS_ADDED_DATE] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ZGWCoreWeb_NOTIFICATIONS] PRIMARY KEY CLUSTERED ([Notification_SeqID] ASC),
    CONSTRAINT [FK_Notifications_Entities] FOREIGN KEY ([Security_Entity_SeqID]) REFERENCES [ZGWSecurity].[Security_Entities] ([Security_Entity_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Notifications_Functions] FOREIGN KEY ([Function_SeqID]) REFERENCES [ZGWSecurity].[Functions] ([Function_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [FK_IX_Notification_Entity_Function]
    ON [ZGWCoreWeb].[Notifications]([Notification_SeqID] ASC, [Security_Entity_SeqID] ASC, [Function_SeqID] ASC);

