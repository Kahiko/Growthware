CREATE TABLE [dbo].[ZFC_ACCTS] (
    [ACCT_SEQ_ID]          INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ACCT]                 VARCHAR (128) NOT NULL,
    [EMAIL]                VARCHAR (128) NULL,
    [ENABLE_NOTIFICATIONS] INT           NULL,
    [IS_SYSTEM_ADMIN]      INT           NOT NULL,
    [STATUS_SEQ_ID]        INT           NOT NULL,
    [PASSWORD_LAST_SET]    DATETIME      NOT NULL,
    [PWD]                  VARCHAR (256) NOT NULL,
    [FAILED_ATTEMPTS]      INT           NOT NULL,
    [FIRST_NAME]           VARCHAR (15)  NOT NULL,
    [LAST_LOGIN]           DATETIME      NOT NULL,
    [LAST_NAME]            VARCHAR (15)  NOT NULL,
    [LOCATION]             VARCHAR (128) NULL,
    [MIDDLE_NAME]          VARCHAR (15)  NULL,
    [PREFERED_NAME]        VARCHAR (50)  NULL,
    [TIME_ZONE]            INT           NULL,
    [ADDED_BY]             INT           NOT NULL,
    [ADDED_DATE]           DATETIME      NOT NULL,
    [UPDATED_BY]           INT           NOT NULL,
    [UPDATED_DATE]         DATETIME      NOT NULL
);

