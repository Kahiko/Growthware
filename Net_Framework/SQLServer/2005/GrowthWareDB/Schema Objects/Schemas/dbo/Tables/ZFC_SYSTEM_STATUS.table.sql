﻿CREATE TABLE [dbo].[ZFC_SYSTEM_STATUS] (
    [STATUS_SEQ_ID] INT       IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DESCRIPTION]   CHAR (25) NOT NULL,
    [ADDED_BY]      INT       NULL,
    [ADDED_DATE]    DATETIME  NOT NULL,
    [UPDATED_BY]    INT       NULL,
    [UPDATED_DATE]  DATETIME  NOT NULL
);
