CREATE TABLE [ZGWSecurity].[Functions] (
    [Function_SeqID]                    INT                  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]                              VARCHAR (30)         NOT NULL,
    [Description]                       VARCHAR (512)        NOT NULL,
    [Function_Type_SeqID]               INT                  NULL,
    [Source]                            VARCHAR (512) SPARSE NULL,
    [Enable_View_State]                 INT                  NOT NULL,
    [Enable_Notifications]              INT                  NOT NULL,
    [Redirect_On_Timeout]               INT                  NOT NULL,
    [Is_Nav]                            INT                  NOT NULL,
	[Link_Behavior]						INT					 NOT NULL,
    [No_UI]                             INT                  NOT NULL,
    [Navigation_Types_NVP_Detail_SeqID] INT                  NOT NULL,
    [Meta_Key_Words]                    VARCHAR (512) SPARSE NULL,
    [Action]                            VARCHAR (256)        NOT NULL,
    [Parent_SeqID]                      INT                  NULL,
    [Notes]                             VARCHAR (512) SPARSE NULL,
    [Sort_Order]                        INT                  NOT NULL,
    [Added_By]                          INT                  NOT NULL,
    [Added_Date]                        DATETIME             NOT NULL,
    [Updated_By]                        INT                  NULL,
    [Updated_Date]                      DATETIME             NULL
);

