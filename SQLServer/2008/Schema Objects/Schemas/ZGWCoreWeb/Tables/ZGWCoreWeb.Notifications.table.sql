CREATE TABLE [ZGWCoreWeb].[Notifications] (
    [Notification_SeqID]    INT      IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Security_Entity_SeqID] INT      NOT NULL,
    [Function_SeqID]        INT      NOT NULL,
    [Added_By]              INT      NOT NULL,
    [Added_Date]            DATETIME NOT NULL
);

