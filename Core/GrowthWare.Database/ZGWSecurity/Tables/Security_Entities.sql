CREATE TABLE [ZGWSecurity].[Security_Entities] (
    [Security_Entity_SeqID]        INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]                         VARCHAR (256) NOT NULL,
    [Description]                  VARCHAR (512) NULL,
    [URL]                          VARCHAR (128) NULL,
    [Status_SeqID]                 INT           NOT NULL,
    [DAL]                          NCHAR (50)    NOT NULL,
    [DAL_Name]                     NCHAR (50)    NOT NULL,
    [DAL_Name_Space]               VARCHAR (256) NOT NULL,
    [DAL_String]                   VARCHAR (512) NOT NULL,
    [Skin]                         NCHAR (25)    NOT NULL,
    [Style]                        VARCHAR (25)  NOT NULL,
    [Encryption_Type]              INT           NOT NULL,
    [Parent_Security_Entity_SeqID] INT           NULL,
    [Added_By]                     INT           NOT NULL,
    [Added_Date]                   DATETIME      NOT NULL,
    [Updated_By]                   INT           NULL,
    [Updated_Date]                 DATETIME      NULL,
    CONSTRAINT [PK_Entities] PRIMARY KEY CLUSTERED ([Security_Entity_SeqID] ASC),
    CONSTRAINT [FK_Entities_Entities] FOREIGN KEY ([Parent_Security_Entity_SeqID]) REFERENCES [ZGWSecurity].[Security_Entities] ([Security_Entity_SeqID]),
    CONSTRAINT [FK_Entities_Statuses] FOREIGN KEY ([Status_SeqID]) REFERENCES [ZGWSystem].[Statuses] ([Status_SeqID])
);


GO

