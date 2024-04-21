﻿CREATE TABLE [dbo].[ZOP_STATES] (
    [STATE]         VARCHAR (2)   NOT NULL,
    [DESCRIPTION]   VARCHAR (128) NULL,
    [STATUS_SEQ_ID] INT           NULL,
    [ADDED_BY]      INT           NOT NULL,
    [ADDED_DATE]    DATETIME      NOT NULL,
    [UPDATED_BY]    INT           NOT NULL,
    [UPDATED_DATE]  DATETIME      NOT NULL
);

