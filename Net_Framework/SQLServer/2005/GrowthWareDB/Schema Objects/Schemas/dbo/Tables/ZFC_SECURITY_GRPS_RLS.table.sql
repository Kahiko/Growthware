﻿CREATE TABLE [dbo].[ZFC_SECURITY_GRPS_RLS] (
    [RLS_SE_SEQ_ID]  INT      NOT NULL,
    [GRPS_SE_SEQ_ID] INT      NOT NULL,
    [ADDED_BY]       INT      NOT NULL,
    [ADDED_DATE]     DATETIME DEFAULT (getdate()) NOT NULL
);

