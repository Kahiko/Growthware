-- Upgrade from 5.2.0.0 to 6.0.0.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
GO

DECLARE 
      @V_FunctionSeqId INT = -1
    , @V_Name VARCHAR(30) = 'Search DB Logs'
    , @V_Description VARCHAR(512) = 'Search the logging message in the [ZGWSystem].[Logging] table in the database'
    , @V_FunctionTypeSeqId INT = 1
    , @V_Source VARCHAR(512) = ''
    , @V_Controller VARCHAR(512) = ''
    , @V_Resolve VARCHAR(MAX) = ''
    , @V_Enable_View_State INT = 0
    , @V_Enable_Notifications INT = 0
    , @V_Redirect_On_Timeout INT = 0
    , @V_Is_Nav INT = 1
    , @V_Link_Behavior INT = 1
    , @V_NO_UI INT = 0
    , @V_NAV_TYPE_ID INT = 3
    , @V_Action VARCHAR(256) = '/sys_admin/searchDBLogs'
    , @V_Meta_Key_Words VARCHAR(512) = ''
    , @V_ParentSeqId INT = (SELECT TOP(1) [FunctionSeqId] FROM [ZGWSecurity].[Functions] WHERE [Action] = 'SystemAdministration')
    , @V_Notes VARCHAR(512) = 'Used to search '
    , @V_Debug INT = 0
    , @V_SystemID INT = 1
    , @V_ViewPermission INT;

/****** Start: Adding Search DB Logs Actions ******/
IF NOT EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [Action] = @V_Action)
    BEGIN
        PRINT 'Adding ' + CONVERT(VARCHAR(MAX), @V_Name);

        EXEC ZGWSecurity.Set_Function 
              @V_FunctionSeqId       -- FunctionSeqId (-1 indicates new record)
            , @V_Name                -- Name
            , @V_Description         -- Description
            , @V_FunctionTypeSeqId   -- FunctionTypeSeqId
            , @V_Source              -- Source
            , @V_Controller          -- Controller
            , NULL                   -- Resolve
            , 0                      -- Enable_View_State
            , 0                      -- Enable_Notifications
            , 0                      -- Redirect_On_Timeout
            , 1                      -- Is_Nav
            , 1                      -- Link_Behavior (Internal)
            , 0                      -- NO_UI
            , 3                      -- NVP_DetailSeqId (Hierarchical)
            , @V_Action              -- Action
            , @V_META_KEY_WORDS      -- Meta_Key_Words
            , @V_ParentSeqId         -- ParentSeqId
            , @V_Notes               -- Notes
            , @V_SystemID            -- Added_By
            , @V_Debug               -- Debug flag
        
        UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = 12 WHERE [Action] = @V_Action;

        SET @V_FunctionSeqId = (SELECT FunctionSeqId FROM [ZGWSecurity].[Functions] WHERE action = @V_Action);

        SET @V_ViewPermission = (SELECT NVP_DetailSeqId FROM [ZGWSecurity].[Permissions] WHERE NVP_Detail_Value = 'View');

        EXEC [ZGWSecurity].[Set_Function_Roles] 
            @V_FunctionSeqId   -- FunctionSeqId
            ,1                  -- SecurityEntitySeqId
            ,'Developer'        -- Roles
            ,@V_ViewPermission  -- PermissionsNVPDetailSeqId
            ,@V_SystemID        -- AccountSeqId for the 'System Administrator'
            ,@V_Debug;          -- Debug flag
    END
--END IF
/****** End: Adding Search DB Logs Actions ******/

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '6.0.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;