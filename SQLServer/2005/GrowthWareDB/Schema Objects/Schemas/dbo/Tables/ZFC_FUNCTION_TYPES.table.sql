CREATE TABLE [dbo].[ZFC_FUNCTION_TYPES] (
    [FUNCTION_TYPE_SEQ_ID] INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [NAME]                 VARCHAR (50)  NOT NULL,
    [DESCRIPTION]          VARCHAR (128) NOT NULL,
    [TEMPLATE]             VARCHAR (512) NULL,
    [IS_CONTENT]           INT           NOT NULL,
    [ADDED_BY]             INT           NOT NULL,
    [ADDED_DATE]           DATETIME      NOT NULL,
    [UPDATED_BY]           INT           NOT NULL,
    [UPDATED_DATE]         DATETIME      NOT NULL
);

