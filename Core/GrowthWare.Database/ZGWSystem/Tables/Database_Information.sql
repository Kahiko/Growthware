CREATE TABLE [ZGWSystem].[Database_Information] (
    [Database_InformationSeqId] INT          IDENTITY (1, 1) NOT NULL,
    [Version]                   VARCHAR (50) NOT NULL,
    [Enable_Inheritance]        INT          NOT NULL,
    [Added_By]                  INT          DEFAULT ((1)) NOT NULL,
    [Added_Date]                DATETIME     NOT NULL,
    [Updated_By]                INT          NULL,
    [Updated_Date]              DATETIME     NULL,
    CONSTRAINT [PK_ZGWSystem_Database_Information] PRIMARY KEY CLUSTERED ([Database_InformationSeqId] ASC)
);


GO

