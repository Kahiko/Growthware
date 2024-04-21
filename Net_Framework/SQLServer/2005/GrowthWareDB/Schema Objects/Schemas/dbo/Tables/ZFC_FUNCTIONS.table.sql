CREATE TABLE [dbo].[ZFC_FUNCTIONS] (
    [FUNCTION_SEQ_ID]           INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [NAME]                      VARCHAR (30)  NOT NULL,
    [DESCRIPTION]               VARCHAR (512) NOT NULL,
    [FUNCTION_TYPE_SEQ_ID]      INT           NULL,
    [SOURCE]                    VARCHAR (512) NULL,
    [ENABLE_VIEW_STATE]         INT           NOT NULL,
    [ENABLE_NOTIFICATIONS]      INT           NOT NULL,
    [REDIRECT_ON_TIMEOUT]       INT           NOT NULL,
    [IS_NAV]                    INT           NOT NULL,
    [NO_UI]                     INT           NOT NULL,
    [NAVIGATION_NVP_SEQ_DET_ID] INT           NOT NULL,
    [META_KEY_WORDS]            VARCHAR (512) NULL,
    [ACTION]                    VARCHAR (256) NOT NULL,
    [PARENT_FUNCTION_SEQ_ID]    INT           NULL,
    [NOTES]                     VARCHAR (512) NULL,
    [SORT_ORDER]                INT           NOT NULL,
    [ADDED_BY]                  INT           NOT NULL,
    [ADDED_DATE]                DATETIME      NOT NULL,
    [UPDATED_BY]                INT           NOT NULL,
    [UPDATED_DATE]              DATETIME      NOT NULL
);

