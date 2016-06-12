CREATE TABLE [dbo].[ZFC_SECURITY_ENTITIES] (
    [SE_SEQ_ID]        INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [NAME]             VARCHAR (256) NOT NULL,
    [DESCRIPTION]      VARCHAR (512) NULL,
    [URL]              VARCHAR (128) NULL,
    [STATUS_SEQ_ID]    INT           NOT NULL,
    [DAL]              NCHAR (50)    NOT NULL,
    [DAL_NAME]         NCHAR (50)    NOT NULL,
    [DAL_NAME_SPACE]   VARCHAR (256) NOT NULL,
    [DAL_STRING]       VARCHAR (512) NOT NULL,
    [SKIN]             NCHAR (25)    NOT NULL,
    [STYLE]            VARCHAR (25)  NOT NULL,
    [ENCRYPTION_TYPE]  INT           NOT NULL,
    [PARENT_SE_SEQ_ID] INT           NULL,
    [ADDED_BY]         INT           NOT NULL,
    [ADDED_DATE]       DATETIME      NOT NULL,
    [UPDATED_BY]       INT           NOT NULL,
    [UPDATED_DATE]     DATETIME      NOT NULL
);

