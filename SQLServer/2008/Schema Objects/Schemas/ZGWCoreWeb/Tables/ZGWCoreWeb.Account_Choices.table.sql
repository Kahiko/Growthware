CREATE TABLE [ZGWCoreWeb].[Account_Choices] (
    [Account]          VARCHAR (128)  NOT NULL,
    [SE_SEQ_ID]        INT			  NULL,
    [SE_NAME]          VARCHAR (256)  NULL,
    [Back_Color]       VARCHAR (15)   NULL,
    [Left_Color]       VARCHAR (15)   NULL,
    [Head_Color]       VARCHAR (15)   NULL,
    [Sub_Head_Color]   VARCHAR (15)   NULL,
    [Color_Scheme]     VARCHAR (15)   NULL,
    [Favoriate_Action] VARCHAR (50)   NULL,
    [Thin_Actions]     VARCHAR (4000) NULL,
    [Wide_Actions]     VARCHAR (4000) NULL,
    [Records_Per_Page] INT NULL
);

