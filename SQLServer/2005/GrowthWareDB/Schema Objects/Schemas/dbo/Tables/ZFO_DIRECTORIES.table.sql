CREATE TABLE [dbo].[ZFO_DIRECTORIES] (
    [FUNCTION_SEQ_ID]     INT           NOT NULL,
    [DIRECTORY]           VARCHAR (255) NOT NULL,
    [IMPERSONATE]         INT           NOT NULL,
    [IMPERSONATE_ACCOUNT] VARCHAR (50)  NULL,
    [IMPERSONATE_PWD]     VARCHAR (50)  NULL,
    [ADDED_BY]            INT           NOT NULL,
    [ADDED_DATE]          DATETIME      DEFAULT (getdate()) NOT NULL,
    [UPDATED_BY]          INT           NOT NULL,
    [UPDATED_DATE]        DATETIME      DEFAULT (getdate()) NOT NULL
);

