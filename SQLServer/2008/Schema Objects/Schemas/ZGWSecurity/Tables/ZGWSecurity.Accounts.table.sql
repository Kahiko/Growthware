CREATE TABLE [ZGWSecurity].[Accounts] (
    [Account_SeqID]        INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Account]              VARCHAR (128) NOT NULL,
    [Email]                VARCHAR (128) NULL,
    [Enable_Notifications] INT           NULL,
    [Is_System_Admin]      INT           NOT NULL,
    [Status_SeqID]         INT           NOT NULL,
    [Password_Last_Set]    DATETIME      NOT NULL,
    [Password]             VARCHAR (256) NOT NULL,
    [Failed_Attempts]      INT           NOT NULL,
    [First_Name]           VARCHAR (15)  NOT NULL,
    [Last_Login]           DATETIME      NOT NULL,
    [Last_Name]            VARCHAR (15)  NOT NULL,
    [Location]             VARCHAR (128) NULL,
    [Middle_Name]          VARCHAR (15)  NULL,
    [Prefered_Name]        VARCHAR (50)  NULL,
    [Time_Zone]            INT           NULL,
    [Added_By]             INT           NOT NULL,
    [Added_Date]           DATETIME      NOT NULL,
    [Updated_By]           INT           NULL,
    [Updated_Date]         DATETIME      NULL
);

