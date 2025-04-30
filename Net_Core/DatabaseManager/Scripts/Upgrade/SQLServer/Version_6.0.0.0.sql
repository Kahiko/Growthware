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
/****** Start: [ZGWCoreWeb].[Get_Messages] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'ZGWCoreWeb.Get_Messages') AND type IN ( N'P' ,N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWCoreWeb].[Get_Messages] AS'
	END
--End If
GO
/*
Usage:

DECLARE @P_MessageSeqId INT
	,@P_SecurityEntitySeqId INT = 1
	,@P_Debug INT = 1

EXEC [ZGWCoreWeb].[Get_Messages] @P_MessageSeqId
	,@P_SecurityEntitySeqId
	,@P_Debug

*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns messages from ZGWCoreWeb.Messages
--	given the MessageSeqId.  If MessageSeqId = -1
--	all messages are returned.
-- =============================================
-- Author:		Michael Regan
-- Create date: 04/28/2025
-- Description:	Changed so that the data is always returned
-- =============================================
ALTER PROCEDURE [ZGWCoreWeb].[Get_Messages] @P_MessageSeqId INT
	,@P_SecurityEntitySeqId INT
	,@P_Debug INT = 0
AS
SET NOCOUNT ON

IF @P_Debug = 1
	PRINT 'Starting ZGWCoreWeb.Get_Messages'

DECLARE @V_DefaultSecurityEntitySeqId INT = ZGWSecurity.Get_Default_Entity_ID()
-- If messages do not exist for the requested SecurityEntitySeqId, add them if it is a valid SecurityEntitySeqId
IF NOT EXISTS (SELECT TOP(1) 1 FROM [ZGWCoreWeb].[Messages] WHERE [SecurityEntitySeqId] = @P_SecurityEntitySeqId)
	BEGIN
		IF EXISTS (SELECT TOP(1) 1 FROM [ZGWSecurity].[Security_Entities] WHERE [SecurityEntitySeqId] =  @P_SecurityEntitySeqId )
			BEGIN
				INSERT INTO [ZGWCoreWeb].[Messages]
				SELECT 
					  @P_SecurityEntitySeqId
					, [Name]
					, [Title]
					, [Description]
					, [Format_As_HTML]
					, [Body]
					, [Added_By]
					, [Added_Date]
					, [Updated_By]
					, [Updated_Date]
				FROM [ZGWCoreWeb].[Messages]
				WHERE [SecurityEntitySeqId] = @V_DefaultSecurityEntitySeqId;

				IF @P_Debug = 1 PRINT 'Needed to add entries for all message for the requested Security_Entity';
			END
		ELSE
			BEGIN
				IF @P_Debug = 1 PRINT 'There are no message as of yet stop trying to get them!';
				RAISERROR ('The requested SecurityEntitySeqId does not exist.',16,1);
				RETURN 1;
			END
		--END IF
	END
--END IF

IF @P_MessageSeqId <> - 1
	BEGIN
		IF @P_Debug = 1 PRINT 'Getting single message';
		SELECT 
			 [MessageSeqId] AS MESSAGE_SEQ_ID
			,[SecurityEntitySeqId] AS SecurityEntityID
			,[NAME]
			,[TITLE]
			,[Description]
			,[Format_As_HTML]
			,[BODY]
			,[Added_By]
			,[Added_Date]
			,[Updated_By]
			,[Updated_Date]
		FROM [ZGWCoreWeb].[Messages]
		WHERE 
			[MessageSeqId] = @P_MessageSeqId;
	END
ELSE
	BEGIN
		IF @P_Debug = 1 PRINT 'Getting all messages';
		SELECT 
			 [MessageSeqId] AS MESSAGE_SEQ_ID
			,[SecurityEntitySeqId] AS SecurityEntityID
			,[NAME]
			,[TITLE]
			,[Description]
			,[Format_As_HTML]
			,[BODY]
			,[Added_By]
			,[Added_Date]
			,[Updated_By]
			,[Updated_Date]
		FROM 
			[ZGWCoreWeb].[Messages]
		WHERE
			[SecurityEntitySeqId] = @P_SecurityEntitySeqId
		ORDER BY [Name];
	END
--END IF
RETURN 0
GO
/****** End: [ZGWCoreWeb].[Get_Messages] ******/

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '6.0.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;