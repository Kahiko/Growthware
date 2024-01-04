-- Upgrade
SET NOCOUNT ON;

-- Add functions
DECLARE @V_Now DATETIME						= GETDATE()
		, @V_SystemID INT 					= (SELECT AccountSeqId FROM ZGWSecurity.Accounts WHERE Account = 'System')
		, @V_SecurityEntitySeqId INT 		= (SELECT SecurityEntitySeqId FROM ZGWSecurity.Security_Entities WHERE [Name]='System')
		, @V_EVERYONE_ID INT 				= (SELECT GroupSeqId FROM ZGWSecurity.Groups WHERE [Name]='Everyone')
		, @V_MyAction VARCHAR(256)			= ''
		, @V_EnableViewStateTrue INT		= 1 -- 0 = FALSE 1 = TRUE
		, @V_EnableViewStateFalse INT 		= 0 -- 0 = FALSE 1 = TRUE
		, @V_EnableNotificationsTrue INT	= 1 -- 0 = FALSE 1 = TRUE
		, @V_EnableNotificationsFalse INT	= 0 -- 0 = FALSE 1 = TRUE
		, @V_IsNavTrue INT 					= 1 -- 0 = FALSE 1 = TRUE
		, @V_IsNavFalse INT 				= 0 -- 0 = FALSE 1 = TRUE
		, @V_LinkBehaviorInternal int		= (SELECT NVP_DetailSeqId FROM ZGWCoreWeb.Link_Behaviors WHERE NVP_Detail_Value = 'Internal')
		, @V_LinkBehaviorPopup int			= (SELECT NVP_DetailSeqId FROM ZGWCoreWeb.Link_Behaviors WHERE NVP_Detail_Value = 'Popup')
		, @V_LinkBehaviorExternal	int		= (SELECT NVP_DetailSeqId FROM ZGWCoreWeb.Link_Behaviors WHERE NVP_Detail_Value = 'External')
		, @V_LinkBehaviorNewPage int		= (SELECT NVP_DetailSeqId FROM ZGWCoreWeb.Link_Behaviors WHERE NVP_Detail_Value = 'NewPage')
		, @V_NO_UITrue int					= 1 -- TRUE
		, @V_NO_UIFalse int					= 0 -- FALSE
		, @V_ParentID INT
		, @V_FunctionID INT		
		, @V_ViewPermission INT 			= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'View')
		, @V_AddPermission INT 				= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Add')
		, @V_EditPermission INT 			= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Edit')
		, @V_DeletePermission INT 			= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Delete')
		, @V_NAV_TYPE_Hierarchical INT		= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Navigation_Types WHERE NVP_Detail_Value = 'Hierarchical')
		, @V_NAV_TYPE_Vertical INT 			= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Navigation_Types WHERE NVP_Detail_Value = 'Vertical')
		, @V_NAV_TYPE_Horizontal INT 		= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Navigation_Types WHERE NVP_Detail_Value = 'Horizontal')
		, @V_META_KEY_WORDS VARCHAR(512)	= ''
		, @V_CHANGE_PASSWORD INT 			= (select StatusSeqId from ZGWSystem.Statuses where [Name] = 'ChangePassword')
		, @V_INACTIVE INT 					= (select StatusSeqId from ZGWSystem.Statuses where [Name] = 'Inactive')
		, @V_ACTIVE INT 					= (select StatusSeqId from ZGWSystem.Statuses where [Name] = 'Active')
		, @V_ALLOW_HTML_INPUT INT			= 0 -- FALSE
		, @V_ALLOW_COMMENT_HTML_INPUT INT	= 0 -- FALSE
		, @V_IS_CONTENT INT					= 0 -- FALSE
		, @V_FORMAT_AS_HTML_TRUE INT		= 1 -- TRUE
		, @V_FORMAT_AS_HTML_FALSE INT		= 0 -- FALSE
		, @V_PRIMARY_KEY INT				= NULL -- Not needed when setup up the database
		, @V_ErrorCode INT					= NULL -- Not needed when setup up the database
		, @V_FunctionTypeSeqId INT
		, @V_ENCRYPTION_TYPE INT			= 3 -- Aes
		, @V_ENABLE_INHERITANCE INT			= 1 -- 0 = FALSE 1 = TRUE
		, @V_Sort_Order INT 				= 0
		, @V_Redirect_On_Timeout INT		= 1 -- TRUE
		, @V_Added_Updated_By INT			= 1
		, @V_Added_Updated_Date DATETIME	= GETDATE()
		, @V_Debug INT						= 0

SET @V_MyAction = 'SwaggerAPI'
IF NOT EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [Action] = @V_MyAction)
	BEGIN
		PRINT 'Adding Swagger API'
		SET @V_FunctionTypeSeqId 	= (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
		SET @V_ParentID 			= (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
		EXEC ZGWSecurity.Set_Function -1,'API','Displays the Swagger UI.',@V_FunctionTypeSeqId,'https://localhost:44455/swagger/index.html','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorExternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Will need to change the source for your implementation.', @V_SystemID, @V_Debug
		SET @V_FunctionID 			= (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
		EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
	END
--END IF
SET @V_MyAction = 'RevokeToken'
IF NOT EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [Action] = @V_MyAction)
	BEGIN
		PRINT 'Adding RevokeToken'
		SET @V_FunctionTypeSeqId 	= (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Security')
		SET @V_ParentID 			= (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
		EXEC ZGWSecurity.Set_Function -1,'RevokeToken','Revokes a token',@V_FunctionTypeSeqId,'https://localhost:44455/swagger/index.html','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorExternal,@V_NO_UITrue,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to enforce secutiry when attempting to revoke a token.', @V_SystemID, @V_Debug
		SET @V_FunctionID 			= (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
		EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
	END
--END IF
-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '3.0.0.1',
    [Updated_By] = 3,
    [Updated_Date] = getdate()