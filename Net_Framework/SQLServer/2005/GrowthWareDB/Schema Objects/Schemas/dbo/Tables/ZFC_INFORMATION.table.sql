CREATE TABLE [dbo].[ZFC_INFORMATION] (
    [Information_SEQ_ID] INT          IDENTITY (1, 1) NOT NULL,
    [Version]            VARCHAR (50) NOT NULL,
    [Enable_Inheritance] INT          NOT NULL,
    [ADDED_BY]           INT          DEFAULT ((1)) NOT NULL,
    [ADDED_DATE]         DATETIME     DEFAULT (getdate()) NOT NULL,
    [UPDATED_BY]         INT          DEFAULT ((1)) NOT NULL,
    [UPDATED_DATE]       DATETIME     DEFAULT (getdate()) NOT NULL
);

