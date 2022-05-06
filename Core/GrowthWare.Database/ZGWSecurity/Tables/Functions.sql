CREATE TABLE [ZGWSecurity].[Functions] (
    [FunctionSeqId]                    INT                  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Action]                            VARCHAR (256)        NOT NULL,
    [Added_By]                          INT                  NOT NULL,
    [Added_Date]                        DATETIME             CONSTRAINT [DF_ZGWSecurity_Functions_ADDED_DATE] DEFAULT (getdate()) NOT NULL,
    [Controller]                        VARCHAR (512) SPARSE NULL,
    [Description]                       VARCHAR (512)        NOT NULL,
    [Enable_Notifications]              INT                  CONSTRAINT [DF_ZGWSecurity_Functions_ENABLE_NOTIFICATIONS] DEFAULT ((0)) NOT NULL,
    [Enable_View_State]                 INT                  NOT NULL,
    [FunctionTypeSeqId]               INT                  NULL,
    [Is_Nav]                            INT                  NOT NULL,
    [Link_Behavior]                     INT                  NOT NULL,
    [Meta_Key_Words]                    VARCHAR (512) SPARSE NULL,
    [Name]                              VARCHAR (30)         NOT NULL,
    [Navigation_Types_NVP_Detail_SeqID] INT                  NOT NULL,
    [Notes]                             VARCHAR (512) SPARSE NULL,
    [No_UI]                             INT                  CONSTRAINT [DF_ZGWSecurity_Functions_NO_UI] DEFAULT ((0)) NOT NULL,
    [Parent_SeqID]                      INT                  NULL,
    [Redirect_On_Timeout]               INT                  CONSTRAINT [DF_ZGWSecurity_Functions_REDIRECT_ON_TIMEOUT] DEFAULT ((1)) NOT NULL,
    [Resolve]                           VARCHAR (MAX) SPARSE NULL,
    [Sort_Order]                        INT                  CONSTRAINT [DF_ZGWSecurity_Functions_Sort_Order] DEFAULT ((0)) NOT NULL,
    [Source]                            VARCHAR (512) SPARSE NULL,
    [Updated_By]                        INT                  NULL,
    [Updated_Date]                      DATETIME             NULL,
    CONSTRAINT [PK_Functions] PRIMARY KEY CLUSTERED ([FunctionSeqId] ASC),
    CONSTRAINT [FK_Functions_Function_Types] FOREIGN KEY ([FunctionTypeSeqId]) REFERENCES [ZGWSecurity].[Function_Types] ([FunctionTypeSeqId]),
    CONSTRAINT [FK_Functions_Functions] FOREIGN KEY ([Parent_SeqID]) REFERENCES [ZGWSecurity].[Functions] ([FunctionSeqId]),
    CONSTRAINT [FK_Functions_Navigation_Types] FOREIGN KEY ([Navigation_Types_NVP_Detail_SeqID]) REFERENCES [ZGWSecurity].[Navigation_Types] ([NVP_Detail_SeqID]),
    CONSTRAINT [UK_ZGWSecurity_Functions] UNIQUE NONCLUSTERED ([Action] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Used by AnjularJs for building routes', @level0type = N'SCHEMA', @level0name = N'ZGWSecurity', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'Resolve';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Used by AnjularJs for building routes', @level0type = N'SCHEMA', @level0name = N'ZGWSecurity', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'Controller';


GO

