CREATE TABLE [ZGWSecurity].[Security_Entities] (
    [SecurityEntitySeqId]       INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]                      VARCHAR (256) NOT NULL,
    [Description]               VARCHAR (512) NULL,
    [URL]                       VARCHAR (128) NULL,
    [StatusSeqId]               INT           NOT NULL,
    [DAL]                       NCHAR (50)    NOT NULL,
    [DAL_Name]                  NCHAR (50)    NOT NULL,
    [DAL_Name_Space]            VARCHAR (256) NOT NULL,
    [DAL_String]                VARCHAR (512) NOT NULL,
    [Skin]                      NCHAR (25)    NOT NULL,
    [Style]                     VARCHAR (25)  NOT NULL,
    [Encryption_Type]           INT           NOT NULL,
    [ParentSecurityEntitySeqId] INT           NULL,
    [Added_By]                  INT           NOT NULL,
    [Added_Date]                DATETIME      NOT NULL,
    [Updated_By]                INT           NULL,
    [Updated_Date]              DATETIME      NULL,
    CONSTRAINT [PK_Entities] PRIMARY KEY CLUSTERED ([SecurityEntitySeqId] ASC),
    CONSTRAINT [FK_Entities_Entities] FOREIGN KEY ([ParentSecurityEntitySeqId]) REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId]),
    CONSTRAINT [FK_Entities_Statuses] FOREIGN KEY ([StatusSeqId]) REFERENCES [ZGWSystem].[Statuses] ([StatusSeqId])
);


GO

