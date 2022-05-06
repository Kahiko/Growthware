USE [YourDatabaseName];
GO
SET NOCOUNT ON

DECLARE @V_Now datetime,
		@V_SystemID INT,
		@VSecurityEntitySeqId INT,
		@V_EVERYONE_ID INT,
		@V_MyAction VARCHAR(256),
		@V_EnableViewStateTrue INT,
		@V_EnableViewStateFalse INT,
		@V_EnableNotificationsTrue INT,
		@V_EnableNotificationsFalse INT,
		@V_IsNavTrue INT,
		@V_IsNavFalse INT,
		@V_LinkBehaviorInternal int = 1,
		@V_LinkBehaviorPopup int = 2,
		@V_LinkBehaviorExternal int = 3,
		@V_LinkBehaviorNewPage int = 3,
		@V_NO_UITrue int,
		@V_NO_UIFalse int,
		@V_NAV_TYPE INT,
		@V_ParentID INT,
		@V_FunctionID INT,
		@V_ViewPermission INT,
		@V_AddPermission INT,
		@V_EditPermission INT,
		@V_DeletePermission INT,
		@V_NAV_TYPE_Hierarchical INT,
		@V_NAV_TYPE_Vertical INT,
		@V_NAV_TYPE_Horizontal INT,
		@V_META_KEY_WORDS VARCHAR(512),
		@V_CHANGE_PASSWORD INT,
		@V_INACTIVE INT,
		@V_ACTIVE INT,
		@V_ALLOW_HTML_INPUT INT,
		@V_ALLOW_COMMENT_HTML_INPUT INT,
		@V_IS_CONTENT INT,
		@V_FORMAT_AS_HTML_TRUE INT,
		@V_FORMAT_AS_HTML_FALSE INT,
		@V_PRIMARY_KEY INT,
		@V_ErrorCode INT,
		@V_FunctionTypeSeqId INT,
		@V_ENCRYPTION_TYPE INT,
		@V_ENABLE_INHERITANCE INT,
		@V_NVPSeqId INT,
		@V_Sort_Order INT,
		@V_Redirect_On_Timeout INT,
		@V_Added_Updated_By INT,
		@V_Added_Updated_Date datetime,
		@V_Debug int = 0
SET @V_Now = getdate()
SET @V_Redirect_On_Timeout = 1 -- TRUE
SET @V_FORMAT_AS_HTML_TRUE = 1 -- TRUE
SET @V_FORMAT_AS_HTML_FALSE = 0 -- FALSE
SET @V_ALLOW_HTML_INPUT = 0 -- FALSE
SET @V_ALLOW_COMMENT_HTML_INPUT = 0 -- FALSE
SET @V_IS_CONTENT = 0 -- FALSE
SET @V_NO_UITrue = 1 -- True
SET @V_NO_UIFalse = 0 -- True
SET @V_PRIMARY_KEY = NULL -- Not needed when setup up the database
SET @V_ErrorCode = NULL -- Not needed when setup up the database
SET @V_ENCRYPTION_TYPE = 1 -- TripleDES
SET @V_ENABLE_INHERITANCE = 1 -- 0 = FALSE 1 = TRUE
SET @V_Sort_Order = 0
SET @V_META_KEY_WORDS = ''
SET @V_EnableNotificationsTrue = 1 -- 0 = FALSE 1 = TRUE
SET @V_EnableNotificationsFalse = 0  -- 0 = FALSE 1 = TRUE
SET @V_Added_Updated_Date = GETDATE()
SET @V_Added_Updated_By = 1
-- Setup ZFC_SYSTEM_STATUS
Print 'Adding System Status'
exec [ZGWSystem].[Set_System_Status] -1,'Active','Active Status',@V_Added_Updated_By,@V_PRIMARY_KEY,@V_ErrorCode
exec [ZGWSystem].[Set_System_Status] -1,'Inactive','Inactive Status',@V_Added_Updated_By,@V_PRIMARY_KEY,@V_ErrorCode
exec [ZGWSystem].[Set_System_Status] -1,'Disabled','Disabled Status',@V_Added_Updated_By,@V_PRIMARY_KEY,@V_ErrorCode
exec [ZGWSystem].[Set_System_Status] -1,'ChangePassword','ChangePassword Status used by the CoreWebApplication',@V_Added_Updated_By,@V_PRIMARY_KEY,@V_ErrorCode
exec [ZGWSystem].[Set_System_Status] -1,'SetAccountDetails','Please enter your account details',@V_Added_Updated_By,@V_PRIMARY_KEY,@V_ErrorCode
--************************

SET @V_CHANGE_PASSWORD = (select StatusSeqId from ZGWSystem.Statuses where [Name] = 'ChangePassword')
SET @V_INACTIVE = (select StatusSeqId from ZGWSystem.Statuses where [Name] = 'Inactive')
SET @V_ACTIVE = (select StatusSeqId from ZGWSystem.Statuses where [Name] = 'Active')
Print 'Adding Accounts'
-- Add the anonymous account
exec ZGWSecurity.Set_Account -1,1,'Anonymous','Anonymous','Anonymous','','Anonymous-Account','me@me.com','none',@V_Now,0,1,@V_Now,-5,'none',0,0, @V_Debug
-- BEFORE ADDING ANY MORE ACCOUNTS SETUP ZF_ACCT_CHOICES
EXEC ZGWCoreWeb.Set_Account_Choices @P_ACCT = N'Anonymous',	@P_SecurityEntityID = 1, @P_SecurityEntityName = N'System',@P_BackColor = N'#ffffff'
	,@P_LeftColor = N'#eeeeee',@P_HeadColor = N'#C7C7C7',@P_HeaderForeColor = N'Black',@P_SubHeadColor = N'#b6cbeb'
	,@P_RowBackColor = N'#b6cbeb',@P_AlternatingRowBackColor = N'#6699cc',@P_ColorScheme = N'Blue',@P_FavoriteAction = N'Home'
	,@P_recordsPerPage = 10
-- Add the system administrator account
exec ZGWSecurity.Set_Account -1,@V_CHANGE_PASSWORD,'System','System','System','','System','michael.regan@verizon.net','none',@V_Now,0,1,@V_Now,-5,'none',0,1, @V_Debug
-- Add the system Developer account
exec ZGWSecurity.Set_Account -1,@V_CHANGE_PASSWORD,'Developer','System','Developer','','System-Developer','michael.regan@verizon.net','none',@V_Now,0,1,@V_Now,-5,'none',0,1, @V_Debug
-- testing account
exec ZGWSecurity.Set_Account -1,@V_CHANGE_PASSWORD,'Mike','System','Tester','','System-Tester','michael.regan@verizon.net','none',@V_Now,0,0,@V_Now,-5,'none',0,0, @V_Debug
set @V_SystemID = (select AccountSeqId from ZGWSecurity.Accounts where Account = 'System')
--
Print 'Adding NVP tables'
exec ZGWSystem.Set_Name_Value_Pair -1,'ZGWSecurity','Navigation_Types','Navigation Types','Navigation Types',1,1,null,null
exec ZGWSystem.Set_Name_Value_Pair -1,'ZGWSecurity','Permissions','Permissions','Permissions',1,1,null,null
exec ZGWSystem.Set_Name_Value_Pair -1,'ZGWCoreWeb','Link_Behaviors','Link Behaviors','Link Behaviors',1,1,null,null
exec ZGWSystem.Set_Name_Value_Pair -1,'ZGWCoreWeb','Work_Flows','Work Flows','Work Flows',1,1,null,null

Print 'Adding DB Information'
exec ZGWSystem.Set_DataBase_Information -1,'1.0.0.0',@V_ENABLE_INHERITANCE,@V_SystemID, NULL, @V_Debug

Print 'Adding Function types'
-- Setup Functions Types
exec ZGWSecurity.Set_Function_Types -1,'Module','used for modules','','0',@V_SystemID,@V_PRIMARY_KEY, @V_ErrorCode
exec ZGWSecurity.Set_Function_Types -1,'Security','used as a container for security.','none','0',@V_SystemID,@V_PRIMARY_KEY, @V_ErrorCode
exec ZGWSecurity.Set_Function_Types -1,'Menu Item','designates entry is a menu item.','none','0',@V_SystemID,@V_PRIMARY_KEY, @V_ErrorCode
exec ZGWSecurity.Set_Function_Types -1,'Calendar','Used for managing files and Calendars','Functions/System/Calendar/CommunityCalendar.aspx','1',@V_SystemID,@V_PRIMARY_KEY, @V_ErrorCode
exec ZGWSecurity.Set_Function_Types -1,'File Manager','Used for managing files and directories','Functions/System/FileManagement/FileManager.aspx','0',@V_SystemID,@V_PRIMARY_KEY, @V_ErrorCode

Print 'Adding navigation types'
-- Setup Navagation types
SET @V_NVPSeqId = (SELECT NVPSeqId FROM ZGWSystem.Name_Value_Pairs WHERE Static_Name = 'Navigation_Types')
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVPSeqId,'Horizontal','Horizontal',@V_ACTIVE, @V_Sort_Order,@V_SystemID,@V_Primary_Key,@V_ErrorCode
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVPSeqId,'Vertical','Vertical',@V_ACTIVE, @V_Sort_Order,@V_SystemID,@V_Primary_Key,@V_ErrorCode
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVPSeqId,'Hierarchical','Hierarchical',@V_ACTIVE, @V_Sort_Order,@V_SystemID,@V_Primary_Key,@V_ErrorCode

SET @V_NAV_TYPE_Hierarchical = (SELECT NVP_DetailSeqId FROM ZGWSecurity.Navigation_Types WHERE NVP_Detail_Value = 'Hierarchical')
SET @V_NAV_TYPE_Vertical = (SELECT NVP_DetailSeqId FROM ZGWSecurity.Navigation_Types WHERE NVP_Detail_Value = 'Vertical')
SET @V_NAV_TYPE_Horizontal = (SELECT NVP_DetailSeqId FROM ZGWSecurity.Navigation_Types WHERE NVP_Detail_Value = 'Horizontal')

Print 'Adding permissions'
SET @V_NVPSeqId = (SELECT NVPSeqId FROM ZGWSystem.Name_Value_Pairs WHERE Static_Name = 'Permissions')
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVPSeqId,'View','View',@V_ACTIVE, @V_Sort_Order,@V_SystemID,@V_Primary_Key,@V_ErrorCode
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVPSeqId,'Edit','Edit',@V_ACTIVE, @V_Sort_Order,@V_SystemID,@V_Primary_Key,@V_ErrorCode
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVPSeqId,'Add','Add',@V_ACTIVE, @V_Sort_Order,@V_SystemID,@V_Primary_Key,@V_ErrorCode
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVPSeqId,'Delete','Delete',@V_ACTIVE, @V_Sort_Order,@V_SystemID,@V_Primary_Key,@V_ErrorCode

SET @V_ViewPermission = (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'View')
SET @V_AddPermission = (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Add')
SET @V_EditPermission = (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Edit')
SET @V_DeletePermission = (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Delete')

Print 'Adding Link Behaviors'
SET @V_NVPSeqId = (SELECT NVPSeqId FROM ZGWSystem.Name_Value_Pairs WHERE Static_Name = 'Link_Behaviors')
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVPSeqId,'Internal','Internal',@V_ACTIVE, @V_Sort_Order,@V_SystemID,@V_Primary_Key,@V_ErrorCode
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVPSeqId,'Popup','Popup',@V_ACTIVE, @V_Sort_Order,@V_SystemID,@V_Primary_Key,@V_ErrorCode
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVPSeqId,'External','External',@V_ACTIVE, @V_Sort_Order,@V_SystemID,@V_Primary_Key,@V_ErrorCode
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVPSeqId,'NewPage','NewPage',@V_ACTIVE, @V_Sort_Order,@V_SystemID,@V_Primary_Key,@V_ErrorCode

SET @V_LinkBehaviorInternal = (SELECT NVP_DetailSeqId FROM ZGWCoreWeb.Link_Behaviors WHERE NVP_Detail_Value = 'Internal')
SET @V_LinkBehaviorPopup = (SELECT NVP_DetailSeqId FROM ZGWCoreWeb.Link_Behaviors WHERE NVP_Detail_Value = 'Popup')
SET @V_LinkBehaviorExternal = (SELECT NVP_DetailSeqId FROM ZGWCoreWeb.Link_Behaviors WHERE NVP_Detail_Value = 'External')
SET @V_LinkBehaviorNewPage = (SELECT NVP_DetailSeqId FROM ZGWCoreWeb.Link_Behaviors WHERE NVP_Detail_Value = 'NewPage')

Print 'Adding Security Entity'
exec ZGWSecurity.Set_Security_Entity -1,'System','The default Security Entity, needed by the system.','no url',1,'SQLServer','YourDatabaseName.Framework.BusinessData','YourDatabaseName.Framework.BusinessData.DataAccessLayer.SQLServer.V2008','server=(local);Integrated Security=SSPI;database=GW2013Development;connection reset=false;connection lifetime=5;enlist=true;min pool size=1;max pool size=50','Default','Default',@V_Encryption_Type,-1,@V_SystemID, @V_PRIMARY_KEY, @V_Debug
SET @VSecurityEntitySeqId = (SELECT SecurityEntitySeqId FROM ZGWSecurity.Security_Entities WHERE [Name]='System')
exec ZGWSecurity.Set_Security_Entity -1,'Blue Arrow','The Security Entity to test the Blue Arrow skin.','no url',1,'SQLServer','YourDatabaseName.Framework.BusinessData','YourDatabaseName.Framework.BusinessData.DataAccessLayer.SQLServer.V2008','server=(local);Integrated Security=SSPI;database=GW2013Development;connection reset=false;connection lifetime=5;enlist=true;min pool size=1;max pool size=50','Blue Arrow','Blue Arrow',@V_Encryption_Type,@VSecurityEntitySeqId,@V_SystemID, @V_PRIMARY_KEY, @V_Debug
exec ZGWSecurity.Set_Security_Entity -1,'Default Black','The Security Entity to test the Default Black skin.','no url',1,'SQLServer','YourDatabaseName.Framework.BusinessData','YourDatabaseName.Framework.BusinessData.DataAccessLayer.SQLServer.V2008','server=(local);Integrated Security=SSPI;database=GW2013Development;connection reset=false;connection lifetime=5;enlist=true;min pool size=1;max pool size=50','Default Black','Default',@V_Encryption_Type,@VSecurityEntitySeqId,@V_SystemID, @V_PRIMARY_KEY, @V_Debug
exec ZGWSecurity.Set_Security_Entity -1,'DevOps','The Security Entity to test the DevOps skin.','no url',1,'SQLServer','YourDatabaseName.Framework.BusinessData','YourDatabaseName.Framework.BusinessData.DataAccessLayer.SQLServer.V2008','server=(local);Integrated Security=SSPI;database=GW2013Development;connection reset=false;connection lifetime=5;enlist=true;min pool size=1;max pool size=50','DevOps','Default',@V_Encryption_Type,@VSecurityEntitySeqId,@V_SystemID, @V_PRIMARY_KEY, @V_Debug
exec ZGWSecurity.Set_Security_Entity -1,'Professional','The Security Entity to test the Professional skin.','no url',1,'SQLServer','YourDatabaseName.Framework.BusinessData','YourDatabaseName.Framework.BusinessData.DataAccessLayer.SQLServer.V2008','server=(local);Integrated Security=SSPI;database=GW2013Development;connection reset=false;connection lifetime=5;enlist=true;min pool size=1;max pool size=50','Professional','Default',@V_Encryption_Type,@VSecurityEntitySeqId,@V_SystemID, @V_PRIMARY_KEY, @V_Debug
exec ZGWSecurity.Set_Security_Entity -1,'Arc','The Security Entity to test the Arc skin.','no url',1,'SQLServer','YourDatabaseName.Framework.BusinessData','YourDatabaseName.Framework.BusinessData.DataAccessLayer.SQLServer.V2008','server=(local);Integrated Security=SSPI;database=GW2013Development;connection reset=false;connection lifetime=5;enlist=true;min pool size=1;max pool size=50','Arc','Arc',@V_Encryption_Type,@VSecurityEntitySeqId,@V_SystemID, @V_PRIMARY_KEY, @V_Debug
exec ZGWSecurity.Set_Security_Entity -1,'Dashboard','The Security Entity to test the Dashboard skin.','no url',1,'SQLServer','YourDatabaseName.Framework.BusinessData','YourDatabaseName.Framework.BusinessData.DataAccessLayer.SQLServer.V2008','server=(local);Integrated Security=SSPI;database=GW2013Development;connection reset=false;connection lifetime=5;enlist=true;min pool size=1;max pool size=50','Dashboard','Dashboard',@V_Encryption_Type,@VSecurityEntitySeqId,@V_SystemID, @V_PRIMARY_KEY, @V_Debug

Print 'Adding roles'
-- Setup ZF_RLS
exec ZGWSecurity.Set_Role -1,'Anonymous','The anonymous role.',1,0,@VSecurityEntitySeqId, @V_SystemID, @V_PRIMARY_KEY, @V_Debug
exec ZGWSecurity.Set_Role -1,'Authenticated','The authenticated role.',1,0,@VSecurityEntitySeqId, @V_SystemID, @V_PRIMARY_KEY, @V_Debug
exec ZGWSecurity.Set_Role -1,'Developer','The developer role.',1,0,@VSecurityEntitySeqId, @V_SystemID, @V_PRIMARY_KEY, @V_Debug
exec ZGWSecurity.Set_Role -1,'AlwaysLogon','Assign this role to allow logon when the system is under maintance.',1,0,@VSecurityEntitySeqId, @V_SystemID, @V_PRIMARY_KEY, @V_Debug

Print 'Adding Groups'
-- group id,group name,group description,Security Entity,added by,added date,updated by,updated date
exec ZGWSecurity.Set_Group -1,'Everyone','Group representing both the authenticated and the anonymous roles.', @VSecurityEntitySeqId, @V_SystemID, @V_PRIMARY_KEY, @V_Debug

SET @V_EVERYONE_ID = (SELECT GroupSeqId FROM ZGWSecurity.Groups WHERE [Name]='Everyone')
-- group id, Security Entity,comma sep roles,added by,ErrorCode
EXEC ZGWSecurity.Set_Group_Roles @V_EVERYONE_ID,@VSecurityEntitySeqId,'Authenticated,Anonymous',@V_SystemID, @V_Debug
								 
Print 'Adding account security'
-- Setup the security
-- Setup the account security
exec ZGWSecurity.Set_Account_Roles 'Anonymous',@VSecurityEntitySeqId,'Anonymous', @V_SystemID, @V_Debug
exec ZGWSecurity.Set_Account_Roles 'Developer',@VSecurityEntitySeqId,'Developer,Authenticated,AlwaysLogon', @V_SystemID, @V_Debug
exec ZGWSecurity.Set_Account_Roles 'mike',@VSecurityEntitySeqId,'Authenticated', @V_SystemID, @V_Debug

Print 'Adding NVP Roles'
SET @V_NVPSeqId = (select NVPSeqId FROM ZGWSystem.Name_Value_Pairs where Static_Name = 'Navigation_Types')
exec ZGWSecurity.Set_Name_Value_Pair_Roles @V_NVPSeqId, @VSecurityEntitySeqId, 'Developer', @V_ViewPermission, @V_SystemID, @V_Debug
SET @V_NVPSeqId = (select NVPSeqId FROM ZGWSystem.Name_Value_Pairs where Static_Name = 'Permissions')
exec ZGWSecurity.Set_Name_Value_Pair_Roles @V_NVPSeqId, @VSecurityEntitySeqId, 'Developer', @V_ViewPermission, @V_SystemID, @V_Debug

Print 'Adding functions'
-- Add functions

SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_EnableViewStateFalse = 1
set @V_EnableViewStateFalse = 0
set @V_IsNavTrue = 1
set @V_IsNavFalse = 0

Print 'Adding Root Menu'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Menu Item')
set @V_MyAction = 'RootMenu'
exec ZGWSecurity.Set_Function -1,'Root Menu','Place Holer',@V_FunctionTypeSeqId,'none','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,1,'RootMenu', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
Print 'Adding GenericHome'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'RootMenu')
set @V_MyAction = 'Generic_Home'
exec ZGWSecurity.Set_Function -1,'Home','Home',@V_FunctionTypeSeqId,'Functions/System/Home/GenericHome.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,1,'Shown when not authenticated', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous',@V_ViewPermission,@V_SystemID,@V_Debug
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = @V_MyAction)

Print 'Adding Home'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'RootMenu')
set @V_MyAction = 'Home'
exec ZGWSecurity.Set_Function -1,'Home','Home',@V_FunctionTypeSeqId,'Functions/System/Home/Home.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Shown when authenticated', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID,@V_Debug

Print 'Adding Logon'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'RootMenu')
set @V_MyAction = 'Logon'
exec ZGWSecurity.Set_Function -1,'Logon','Logon',@V_FunctionTypeSeqId,'Functions/System/Accounts/Logon.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Loggs on an account', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Developer',@V_ViewPermission,@V_SystemID,@V_Debug

Print 'Adding Favorite'
set @V_MyAction = 'Favorite'
exec ZGWSecurity.Set_Function -1,'Favorite','Favorite',@V_FunctionTypeSeqId,'Functions/System/Accounts/Favorite.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Allows client to set a Favorite action.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID,@V_Debug

Print 'Adding Natural Sort'
set @V_MyAction = 'NaturalSort'
exec ZGWSecurity.Set_Function -1,'Natural Sort','Natural Sort',@V_FunctionTypeSeqId,'Functions/System/TestNaturalSort.aspx','TestNaturalSortController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Vertical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Shows natural sort order vs	ANSI', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_SystemID,@V_Debug

Print 'Adding Logoff'
set @V_MyAction = 'Logoff'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Menu Item')
exec ZGWSecurity.Set_Function -1,'Logoff','Logoff',@V_FunctionTypeSeqId,'Functions/System/Accounts/Logoff.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Loggs off the system.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID,@V_Debug

Print 'Adding Admin'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Menu Item')
set @V_MyAction = 'Admin'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'RootMenu')
exec ZGWSecurity.Set_Function -1,'Admin','Administration',@V_FunctionTypeSeqId,'none','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Administration tasks.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

Print 'Adding Calendars'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Menu Item')
set @V_MyAction = 'Calendars'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'RootMenu')
exec ZGWSecurity.Set_Function -1,'Calendars','Calendars',@V_FunctionTypeSeqId,'none','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Access to the calendar.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated,Developer',@V_ViewPermission,@V_SystemID,@V_Debug

SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Menu Item')
set @V_MyAction = 'Reports'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'RootMenu')
exec ZGWSecurity.Set_Function -1,'Reports','Reports',@V_FunctionTypeSeqId,'none','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Access to the reports.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Menu Item')
set @V_MyAction = 'MyProfile'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'RootMenu')
exec ZGWSecurity.Set_Function -1,'My Profile','My Profile',@V_FunctionTypeSeqId,'none','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Access to profile information.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated,Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding System Administrator menu'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Menu Item')
set @V_MyAction = 'SystemAdministration'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'RootMenu')
exec ZGWSecurity.Set_Function -1,'SysAdmin','System Administration',@V_FunctionTypeSeqId,'none','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Serves as the root menu item for the hierarchical menus.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'RootMenu')
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding ManageFunctions'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Menu Item')
set @V_MyAction = 'ManageFunctions'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'Manage Functions','Manage Functions',@V_FunctionTypeSeqId,'none','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Menu item for functions.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Add Functions'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'AddFunctions'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'ManageFunctions')
exec ZGWSecurity.Set_Function -1,'Add Functions','Add Functions',@V_FunctionTypeSeqId,'Functions/System/Administration/Functions/AddEditFunction.aspx','AddEditFunctionController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Adds a function to the system.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Copy Function Security'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'CopyFunctionSecurity'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'ManageFunctions')
exec ZGWSecurity.Set_Function -1,'Copy Function Security','Copy Function Security',@V_FunctionTypeSeqId,'Functions/System/Administration/Functions/CopyFunctionSecurity.aspx','CopyFunctionSecurityController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Adds a function to the system.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

-- Search Security Entitys
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'Search_Security_Entities'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'Manage Security Entities','Search Security Entities',@V_FunctionTypeSeqId,'Functions/System/Administration/SecurityEntities/SearchSecurityEntities.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to search a Security Entity.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_AddPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_DeletePermission,@V_SystemID, @V_Debug

print 'File Management menu'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Menu Item')
set @V_MyAction = 'ManageFiles'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'Manage Files','Manage Files',@V_FunctionTypeSeqId,'none','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to manage files.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'cache directory management'
-- Add module
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'File Manager')
SET @V_MyAction = 'Manage_Cache_Dependency'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'ManageFiles')
EXEC ZGWSecurity.Set_Function -1,'Manage Cachedependency','Manage Cachedependency',@V_FunctionTypeSeqId,'Functions/System/FileManagement/FileManager.aspx','FileManagerController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to manage the cache dependency direcory.', @V_SystemID, @V_Debug
-- Set security
SET @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_AddPermission,@V_SystemID, @V_Debug
EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID, @V_Debug
EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_DeletePermission,@V_SystemID, @V_Debug
PRINT 'Adding cache directory'
-- Add directory information
EXEC ZGWOptional.Set_Directory @V_FunctionID ,'D:\Development\YourDatabaseName\VB\YourDatabaseName.WebAngularJS\CacheDependency',0,'','',@V_SystemID,@V_PRIMARY_KEY, @V_Debug

PRINT 'cache directory management'
-- Add module
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'File Manager')
SET @V_MyAction = 'Manage_Logs'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'ManageFiles')
EXEC ZGWSecurity.Set_Function -1,'Manage Logs','Manage Logs',@V_FunctionTypeSeqId,'Functions/System/FileManagement/FileManager.aspx','FileManagerController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to manage the logs direcory.', @V_SystemID, @V_Debug
SET @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
-- Set security
EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_AddPermission,@V_SystemID, @V_Debug
EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID, @V_Debug
EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_DeletePermission,@V_SystemID, @V_Debug
PRINT 'Adding log log'
-- Add directory information
EXEC ZGWOptional.Set_Directory @V_FunctionID ,'D:\Development\YourDatabaseName\VB\YourDatabaseName.WebAngularJS\Logs',0,'','',@V_SystemID,@V_PRIMARY_KEY, @V_Debug

print 'Adding Manage Name/Value Pairs'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Menu Item')
set @V_MyAction = 'Manage_Name_Value_Pairs'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'Manage Name/Value Pairs','Manage Name/Value Pairs',@V_FunctionTypeSeqId,'none','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Menu item for name/value pairs.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Add Edit Groups'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'AddEditGroups'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Add Edit Groups','Add Edit Groups',@V_FunctionTypeSeqId,'Functions/System/Administration/Groups/AddEditGroups.aspx','AddEditGroupController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorPopup,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add or edit roles.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_AddPermission,@V_SystemID,@V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID,@V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_DeletePermission,@V_SystemID,@V_Debug

print 'Adding Manage Groups'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'Manage_Groups'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Manage Groups','Manage Groups',@V_FunctionTypeSeqId,'Functions/System/Administration/Groups/SearchGroups.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add or edit roles.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_AddPermission,@V_SystemID,@V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID,@V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_DeletePermission,@V_SystemID,@V_Debug

--print 'Adding Manage Messages'
--SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Menu Item')
--set @V_MyAction = 'ManageMessages'
--SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
--exec ZGWSecurity.Set_Function -1,'Manage Messages','Manage Messages',@V_FunctionTypeSeqId,'none','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Menu item for messages.', @V_SystemID, @V_Debug
--set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
--exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Manage States'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Menu Item')
set @V_MyAction = 'ManageStates'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'Manage States','Manage States',@V_FunctionTypeSeqId,'none','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Menu item for states.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Manage Work Flows'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Menu Item')
set @V_MyAction = 'WorkFlows'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'Manage Work Flows','Manage Work Flows',@V_FunctionTypeSeqId,'none','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Menu item for work flows.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Encryption Helper'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'Encryption_Helper'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'Encryption Helper','Encryption Helper',@V_FunctionTypeSeqId,'Functions/System/Administration/Encrypt/EncryptDecrypt.aspx','EncryptDecryptController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Menu item for work flows.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding GUID Helper'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'GuidHelper'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'GUID Helper','Displays''s a GUID',@V_FunctionTypeSeqId,'Functions/System/Administration/Encrypt/GUIDHelper.aspx','GUIDHelperController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Displays a GUID may be necessary if you need to change the GUID in your project files.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Random Number'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'RandomNumbers'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'Random Numbers','Displays''s a set of randomly generated number''s',@V_FunctionTypeSeqId,'Functions/System/Administration/Encrypt/RandomNumbers.aspx','RandomNumbersController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Displays''s a set of randomly generated number''s.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Set Log Level'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'SetLogLevel'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'Set Log Level','Set Log Level',@V_FunctionTypeSeqId,'Functions/System/Administration/Logs/SetLogLevel.aspx','SetLogLevelController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to set the log level of the application ... Debug, Error, Warn, Fatal.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Update Anonymous Profile'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'UpdateAnonymousProfile'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'Update Anonymous Profile','Update Anonymous Profile',@V_FunctionTypeSeqId,'Functions/System/Administration/AnonymousAccount/UpdateAnonymousCache.aspx','UpdateAnonymousCacheController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Remove any cached information for the anonymous account.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Search Functions'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'Search_Functions'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'ManageFunctions')
exec ZGWSecurity.Set_Function -1,'Search Functions','Search Functions',@V_FunctionTypeSeqId,'Functions/System/Administration/Functions/SearchFunctions.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Searches for functions in the system.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_DeletePermission,@V_SystemID,@V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_AddPermission,@V_SystemID,@V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID,@V_Debug

print 'Adding Edit Functions'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'EditFunctions'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'Edit Functions','Edit Functions',@V_FunctionTypeSeqId,'Functions/System/Administration/Functions/AddEditFunction.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Edits a function in the system.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Function Security'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'FunctionSecurity'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Reports')
exec ZGWSecurity.Set_Function -1,'Function Security','Function Security',@V_FunctionTypeSeqId,'Functions/System/Reports/FunctionSecurity.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Displays a report for function security.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Security By Role'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'SecurityByRole'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Reports')
exec ZGWSecurity.Set_Function -1,'Security By Role','Security By Role',@V_FunctionTypeSeqId,'Functions/System/Reports/SecurityByRole.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Displays a report for security by role.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Change Password'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'ChangePassword'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'MyProfile')
exec ZGWSecurity.Set_Function -1,'Change Password','Change Password',@V_FunctionTypeSeqId,'Functions/System/Accounts/ChangePassword.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to change an accounts password.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID,@V_Debug

--print 'Adding Change Colors'
--SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
--set @V_MyAction = 'ChangeColors'
--SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'MyProfile')
--exec ZGWSecurity.Set_Function -1,'Change Colors','Change Colors',@V_FunctionTypeSeqId,'Functions/System/Accounts/ChangeColors.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to change an accounts color scheme.', @V_SystemID, @V_Debug
--set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
--exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Select Preferences'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'SelectPreferences'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'MyProfile')
exec ZGWSecurity.Set_Function -1,'Select Preferences','Select Preferences',@V_FunctionTypeSeqId,'Functions/System/Accounts/SelectPreferences.aspx','SelectPreferencesController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to select preference for an account, records per page etc.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Edit Account'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'EditAccount'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'MyProfile')
exec ZGWSecurity.Set_Function -1,'Edit Account','Edit Account',@V_FunctionTypeSeqId,'Functions/System/Administration/Accounts/AddEditAccount.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to edit an account profile.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID,@V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_EditPermission,@V_SystemID,@V_Debug

print 'Adding Edit Other Account'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'EditOtherAccount'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'MyProfile')
exec ZGWSecurity.Set_Function -1,'Edit Other Account','Edit Other Account',@V_FunctionTypeSeqId,'Functions/System/Administration/Accounts/AddEditAccount.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to edit anothers account profile.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID,@V_Debug

print 'Adding Community Calendar'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'CommunityCalendar'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Calendars')
exec ZGWSecurity.Set_Function -1,'Community Calendar','Community Calendar',@V_FunctionTypeSeqId,'Functions/System/Calendar/CommunityCalendar.aspx','CommunityCalendarController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to show calendar data.  Created as an example module.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Add Account'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'AddAccount'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Add Account','Add Account',@V_FunctionTypeSeqId,'Functions/System/Administration/Accounts/AddEditAccount.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add an accounts password.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

Print 'Adding Register'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'RootMenu')
set @V_MyAction = 'Register'
exec ZGWSecurity.Set_Function -1,'Register','Register accounts for the system',@V_FunctionTypeSeqId,'Functions/System/Administration/Accounts/AddEditAccount.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Registers an account.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous',@V_ViewPermission,@V_SystemID,@V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous',@V_AddPermission,@V_SystemID,@V_Debug

Print 'Adding Open Auth Provider Logon'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'RootMenu')
set @V_MyAction = 'OpenAuthProviderLogon'
exec ZGWSecurity.Set_Function -1,'Open Auth Provider Logon','Open Auth Provider Logon bounce page to redirect to the appropriate provider.',@V_FunctionTypeSeqId,'Functions/System/ExternalAuth/OpenAuthProviderLogon.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Open Auth Provider Logon.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous',@V_ViewPermission,@V_SystemID,@V_Debug

Print 'Adding Register External Login'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'RootMenu')
set @V_MyAction = 'RegisterExternalLogin'
exec ZGWSecurity.Set_Function -1,'Register External Login','Register External Login accounts for the system',@V_FunctionTypeSeqId,'Functions/System/ExternalAuth/RegisterExternalLogin.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Register External Login.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous',@V_ViewPermission,@V_SystemID,@V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous',@V_AddPermission,@V_SystemID,@V_Debug

print 'Adding Add Edit Roles'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'AddEditRoles'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Add Edit Roles','Add Edit Roles',@V_FunctionTypeSeqId,'Functions/System/Administration/Roles/AddEditRole.aspx','AddEditRoleController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorPopup,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add or edit roles.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_AddPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_DeletePermission,@V_SystemID, @V_Debug

print 'Adding Manage Roles'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'Search_Roles'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Manage Roles','Manage Roles',@V_FunctionTypeSeqId,'Functions/System/Administration/Roles/SearchRoles.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add or edit roles.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_AddPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_DeletePermission,@V_SystemID, @V_Debug

print 'Adding Add Edit Name Value Pairs Details'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'AddEditNameValuePairDetails'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Add Edit Name Value Pairs','Add Edit Name Value Pairs',@V_FunctionTypeSeqId,'Functions/System/Administration/NVP/AddEditNVPDetails.aspx','AddEditNVPDetailController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add or edit a list of value details.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding ViewAccountRoleTab'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Security')
set @V_MyAction = 'View_Account_Role_Tab'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'ViewAccountRoleTab','View Accounts Roles Tab',@V_FunctionTypeSeqId,'None','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used as a security holder for roles that can view the accounts role tab.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding ViewFunctionRoleTab'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Security')
set @V_MyAction = 'View_Function_Role_Tab'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'ViewFunctionRoleTab','View Functions Roles Tab',@V_FunctionTypeSeqId,'None','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used as a security holder for roles that can view the functions role tab.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding ViewAccountGroupTab'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Security')
set @V_MyAction = 'View_Account_Group_Tab'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'ViewAccountGroupTab','View Accounts Groups Tab',@V_FunctionTypeSeqId,'None','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used as a security holder for groups that can view the accounts group tab.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding ViewFunctionGroupTab'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Security')
set @V_MyAction = 'View_Function_Group_Tab'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'ViewFunctionGroupTab','View Function Groups Tab',@V_FunctionTypeSeqId,'None','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used as a security holder for groups that can view the functions group tab.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Search Accounts'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'Search_Accounts'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Manage Accounts','Manage Accounts',@V_FunctionTypeSeqId,'Functions/System/Administration/Accounts/SearchAccounts.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to select accounts for edit.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_AddPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_DeletePermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Edit Role Members'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'Edit_Role_Members'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Edit Role Members','Edit Role Members',@V_FunctionTypeSeqId,'Functions/System/Administration/Roles/EditRoleMembers.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UITrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add or remove members of a role.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID, @V_Debug

print 'Adding Add Role'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'AddRole'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Add Role','Add Role',@V_FunctionTypeSeqId,'Functions/System/Administration/Roles/AddRole.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UITrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Adds a role.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID, @V_Debug

print 'Adding Edit Group Members'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'Edit_Group_Members'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Edit Group Members','Edit Group Members',@V_FunctionTypeSeqId,'Functions/System/Administration/Groups/EditGroupMembers.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UITrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add or remove members of a role.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Add A Group'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'AddGroup'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Add A Group','Add A Group',@V_FunctionTypeSeqId,'Functions/System/Administration/Groups/AddGroup.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UITrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add or remove members of a role.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Not Avalible'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'NotAvalible'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Not Avalible','Not Avalible',@V_FunctionTypeSeqId,'Functions/System/Errors/NotAvailable.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Error page when the action is not avalible.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug
--AccessDenied
print 'Adding Access Denied'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'AccessDenied'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Access Denied','Access Denied',@V_FunctionTypeSeqId,'Functions/System/Errors/AccessDenied.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Error page when the account being used does not have sufficient access to the view permission.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug
--Adding Error
print 'Adding Error'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'DisplayError'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Display Error','Display Error',@V_FunctionTypeSeqId,'Functions/System/Errors/DisplayError.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsTrue,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Error page when unknown or unexpected error occurs.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug

--Adding Error
print 'Adding Unknown Action Error'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'UnknownAction'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Unknown Action','Unknown Action',@V_FunctionTypeSeqId,'Functions/System/Errors/UnknownAction.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsTrue,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Error page when unknown action is attempted occurs.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Horizontal Hierarchical menu'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'Horizontal_Hierarchical_Menu'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Horizontal Hierarchical menu','Horizontal Hierarchical menu',@V_FunctionTypeSeqId,'Functions/System/Menus/HHMenu.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsTrue,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Display''s Horizontal Hierarchical menu.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Vertical Hierarchical menu'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'Vertical_Hierarchical_Menu'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Vertical Hierarchical menu','Vertical Hierarchical menu',@V_FunctionTypeSeqId,'Functions/System/Menus/VHMenu.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsTrue,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Display''s Vertical Hierarchical menu.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Vertical menu'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'Vertical_Menu'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Vertical menu','Vertical menu',@V_FunctionTypeSeqId,'Functions/System/Menus/VMenu.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsTrue,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Display''s Vertical menu.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Horizontal menu'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'Horizontal_Menu'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Horizontal menu','Horizontal menu',@V_FunctionTypeSeqId,'Functions/System/Menus/HMenu.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsTrue,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Display''s Horizontal menu.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug


--Select A Security Entity
print 'Adding Select A Security Entity'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'SelectASecurityEntity'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Select A Security Entity','Select A Security Entity',@V_FunctionTypeSeqId,'Functions/System/SecurityEntities/SelectSecurityEntity.aspx','SecurityEntityController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Vertical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to select a Security Entity.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug
---- Web configuration
--print 'Adding Web configuration'
--SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
--set @V_MyAction = 'WebConfig'
--SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
--exec ZGWSecurity.Set_Function -1,'Web Config','Web Config',@V_FunctionTypeSeqId,'Functions/System/Administration/Configuration/AddEditWebConfig.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Adds or edits web.config file settings.', @V_SystemID, @V_Debug
--set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
--exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Line Count
print 'Adding Line Count'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'LineCount'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'Line Count','Line Count',@V_FunctionTypeSeqId,'Functions/System/LineCount.aspx','LineCountController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Utility to count the lines of code.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Add a Security Entity
print 'Adding Add Security Entitys'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'AddSecurityEntities'
--SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Search_Security_Entities')
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'RootMenu')
exec ZGWSecurity.Set_Function -1,'Add Security Entitys','Add Security Entitys',@V_FunctionTypeSeqId,'Functions/System/Administration/SecurityEntities/AddEditSecurityEntities.aspx','AddEditSecurityEntityController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add a Security Entity.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Edit a Security Entity
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'EditASecurityEntity'
--SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Search_Security_Entities')
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'RootMenu')
exec ZGWSecurity.Set_Function -1,'Edit a Security Entity','Edit a Security Entity',@V_FunctionTypeSeqId,'Functions/System/Administration/SecurityEntities/AddEditSecurityEntities.aspx','AddEditSecurityEntityController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to edit a Security Entity.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

-- Search Name Value Pairs
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'Search_Name_Value_Pairs'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'Manage Name/Value Pairs','Search Name Value Pairs',@V_FunctionTypeSeqId,'Functions/System/Administration/NVP/SearchNVP.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to search a name/value pair.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
UPDATE ZGWSecurity.Functions SET Sort_Order = 5 WHERE FunctionSeqId = @V_FunctionID
-- Add a Name Value Pair
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'AddNameValuePairs'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SearchNameValuePairs')
exec ZGWSecurity.Set_Function -1,'Add Name Value Pairs','Add Name Value Pairs',@V_FunctionTypeSeqId,'Functions/System/Administration/NVP/AddEditNVP.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add a name/value pair.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Edit a Name Value Pair
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'EditNameValuePairs'
exec ZGWSecurity.Set_Function -1,'Edit a Name Value Pair','Edit a Name Value Pair',@V_FunctionTypeSeqId,'Functions/System/Administration/NVP/AddEditNVP.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to edit a name/value pair.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

-- Add a Message
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'AddMessage'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'ManageMessages')
exec ZGWSecurity.Set_Function -1,'Add Message','Add Message',@V_FunctionTypeSeqId,'Functions/System/Administration/Messages/AddEditMessage.aspx','AddEditMessageController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add a message.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Edit a Message
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'EditMessage'
exec ZGWSecurity.Set_Function -1,'Edit a Message','Edit a Message',@V_FunctionTypeSeqId,'Functions/System/Administration/Messages/AddEditMessage.aspx','AddEditMessageController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to edit a Message.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Search Message
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'Search_Messages'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Manage Messages','Manage Messages',@V_FunctionTypeSeqId,'Functions/System/Administration/Messages/SearchMessages.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to search a Message.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
--exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_AddPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_DeletePermission,@V_SystemID, @V_Debug


-- Edit a State
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'EditState'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'ManageStates')
exec ZGWSecurity.Set_Function -1,'Edit a State','Edit a State',@V_FunctionTypeSeqId,'Functions/System/Administration/States/AddEditStates.aspx','AddEditStateController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to edit a State.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Search State
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'Search_States'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'ManageStates')
exec ZGWSecurity.Set_Function -1,'Search States','Search States',@V_FunctionTypeSeqId,'Functions/System/Administration/States/SearchStates.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to search a State.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Add Edit Workflows
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'AddEditWorkflow'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'WorkFlows')
exec ZGWSecurity.Set_Function -1,'Add/Edit Workflows','Add/Edit Workflows',@V_FunctionTypeSeqId,'Functions/System/Administration/WorkFlow/AddEditWorkFlow.aspx','AddEditWorkFlowController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add or edit a Workflow.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Update Session
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'Update'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'Update','Update',@V_FunctionTypeSeqId,'Functions/System/Accounts/UpdateSession.aspx','UpdateSessionController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to update the session menus and roles.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug
-- Under Maintance
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'UnderMaintance'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'Under Maintance','Under Maintance',@V_FunctionTypeSeqId,'Functions/System/Administration/UnderMaintance.aspx','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to update the session menus and roles.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug
print 'Adding AlwaysLogon'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Security')
set @V_MyAction = 'AlwaysLogon'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'Always Logon','Always Logon',@V_FunctionTypeSeqId,'none','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavFalse,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to update the session menus and roles.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'AlwaysLogon',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Edit DB Information'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'EditDBInformation'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'SystemAdministration')
exec ZGWSecurity.Set_Function -1,'Edit DB Information','Edit DB Information',@V_FunctionTypeSeqId,'Functions/System/Administration/Configuration/AddEditDBInformation.aspx','AddEditDBInformationController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to update the ZF_Information table, enable inheritance.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

-- @V_LinkBehaviorInternal
-- @V_LinkBehaviorPopup
-- @V_LinkBehaviorExternal
-- @V_LinkBehaviorNewPage
print 'Adding LinkBehavior tests'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Menu Item')
set @V_MyAction = 'TestLinkBehavior'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'RootMenu')
exec ZGWSecurity.Set_Function -1,'Link Behaviors','Testing Link Behaviors',@V_FunctionTypeSeqId,'none','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Access to the calendar.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding MS Popup'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'MSPopup'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'TestLinkBehavior')
exec ZGWSecurity.Set_Function -1,'MS Popup','MS Popup',@V_FunctionTypeSeqId,'http://www.microsoft.com/en-us/','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorPopup,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to edit a State.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding MS External'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'MSExternal'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'TestLinkBehavior')
exec ZGWSecurity.Set_Function -1,'MS External','MS External',@V_FunctionTypeSeqId,'http://www.microsoft.com/en-us/','', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorExternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to edit a State.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding MS NewPage'
SET @V_FunctionTypeSeqId = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module')
set @V_MyAction = 'MSNewPage'
SET @V_ParentID = (SELECT FunctionSeqId FROM ZGWSecurity.Functions WHERE [Action] = 'TestLinkBehavior')
exec ZGWSecurity.Set_Function -1,'MS NewPage','MS NewPage',@V_FunctionTypeSeqId,'Functions/System/TestNaturalSort.aspx','TestNaturalSortController', NULL,@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorNewPage,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to edit a State.', @V_SystemID, @V_Debug
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

--Print 'Adding work flow details'
---- Setup Navigation_Types
SET @V_NVPSeqId = (SELECT NVPSeqId FROM ZGWSystem.Name_Value_Pairs WHERE Static_Name = 'Work_Flows')
SET @V_MyAction = 'Change_Password'
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
SET @V_Sort_Order = 1
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVPSeqId,'Change Password',@V_FunctionID,@V_ACTIVE, @V_Sort_Order,@V_SystemID, @V_PRIMARY_KEY, NULL, @v_Debug
SET @V_MyAction = 'Home'
set @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction)
SET @V_Sort_Order = 2
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVPSeqId,'Change Password',@V_FunctionID,@V_ACTIVE, @V_Sort_Order,@V_SystemID, @V_PRIMARY_KEY, NULL, @v_Debug
SET @V_Sort_Order = 0

PRINT 'Adding messages'
exec ZGWCoreWeb.Set_Message -1,@VSecurityEntitySeqId,'Logon Error','Logon Error','Displayed when logon fails','<b>Invalid Account or Password!</b>',@V_FORMAT_AS_HTML_TRUE,@V_SystemID, @V_Primary_Key, @V_Debug
				   
exec ZGWCoreWeb.Set_Message -1,@VSecurityEntitySeqId,'New Account','New Account','Message sent when an account is created.','Dear <FullName>,

There has been a request for a new account: 

	Please Use this link to logon:
 <Server>Default.aspx?Action=Logon&Account=<AccountName>&Password=<Password>

<b>Please note once you have logged on using this link you will only be able to change our password.</b>',@V_FORMAT_AS_HTML_FALSE,@V_SystemID, @V_Primary_Key, @V_Debug

exec ZGWCoreWeb.Set_Message -1,@VSecurityEntitySeqId,'Request Password Reset UI','Request Password Reset UI','Displayed when new password is requested','<b>An EMail has been send to your account with instructions!</b>',@V_FORMAT_AS_HTML_TRUE,@V_SystemID, @V_Primary_Key, @V_Debug
exec ZGWCoreWeb.Set_Message -1,@VSecurityEntitySeqId,'RequestNewPassword','Request New Password','Request New Password','Dear <FullName>,

There has been a request for a password change: 

	Please Use this link to logon:
 <Server>Default.aspx?Action=Logon&Account=<AccountName>&Password=<Password>

<b>Please note once you have logged on using this link you will only be able to change our password.</b>',@V_FORMAT_AS_HTML_TRUE,@V_SystemID, @V_Primary_Key, @V_Debug
exec ZGWCoreWeb.Set_Message -1,1,'UnhandledException','Unhandled Exception','Unhandled Exception','Unhandled Exception',@V_FORMAT_AS_HTML_True,@V_SystemID, @V_Primary_Key, @V_Debug
exec ZGWCoreWeb.Set_Message -1,1,'WebConfigNotSaved','Web Config has not be saved','Web Config has not be saved','Settings have not been saved.',@V_FORMAT_AS_HTML_FALSE,@V_SystemID, @V_Primary_Key, @V_Debug
exec ZGWCoreWeb.Set_Message -1,1,'WebConfigIsLocked','Web Config Is Locked','Web Config Is Locked','Configuration Section is locked. Unable to modify.',@V_FORMAT_AS_HTML_FALSE,@V_SystemID, @V_Primary_Key, @V_Debug
exec ZGWCoreWeb.Set_Message -1,1,'WebConfigEnvironmentRequired','Web Config Environment Required','Web Config Environment Required','You have selected a new environment but did not give the name.',@V_FORMAT_AS_HTML_FALSE,@V_SystemID, @V_Primary_Key, @V_Debug
exec ZGWCoreWeb.Set_Message -1,1,'ErrorAccountDetails','Error Account Details','Error Account Details','Could not set account details.',@V_FORMAT_AS_HTML_FALSE,@V_SystemID, @V_Primary_Key, @V_Debug
exec ZGWCoreWeb.Set_Message -1,1,'PasswordSendMailError','Password Send Mail Error','Password Send Mail Error','The password was reset, but, an email could not be sent.',@V_FORMAT_AS_HTML_FALSE,@V_SystemID, @V_Primary_Key, @V_Debug
exec ZGWCoreWeb.Set_Message -1,1,'DisabledAccount','Disabled Account','Disabled Account','This account is disabled.',@V_FORMAT_AS_HTML_FALSE,@V_SystemID, @V_Primary_Key, @V_Debug
exec ZGWCoreWeb.Set_Message -1,1,'SuccessChangePassword','Success Change Password','Success Change Password','Your password has been changed.',@V_FORMAT_AS_HTML_FALSE,@V_SystemID, @V_Primary_Key, @V_Debug
exec ZGWCoreWeb.Set_Message -1,1,'UnSuccessChangePassword','UnSuccess Change Password','UnSuccess ChangePassword','Your password has NOT been changed.',@V_FORMAT_AS_HTML_FALSE,@V_SystemID, @V_Primary_Key, @V_Debug
exec ZGWCoreWeb.Set_Message -1,1,'PasswordNotMatched','Password Not Matched','Password Not Matched','The OLD password did not match your current password.',@V_FORMAT_AS_HTML_FALSE,@V_SystemID, @V_Primary_Key, @V_Debug
exec ZGWCoreWeb.Set_Message -1,1,'UnderMaintance','Under Maintance','Under Maintance','The system is currently under maintance and logons have been limited.',@V_FORMAT_AS_HTML_FALSE,@V_SystemID, @V_Primary_Key, @V_Debug
exec ZGWCoreWeb.Set_Message -1,1,'UnderConstruction','Under Construction','Under Construction','The system is currently under construction.',@V_FORMAT_AS_HTML_FALSE,@V_SystemID, @V_Primary_Key, @V_Debug
exec ZGWCoreWeb.Set_Message -1,1,'NoDataFound','No Data Found','No Data Found','No Data Found.',@V_FORMAT_AS_HTML_FALSE,@V_SystemID, @V_Primary_Key, @V_Debug
exec ZGWCoreWeb.Set_Message -1,1,'ChangedSelectedSecurityEntity','Changed Selected Security Entity','Message for when a account changes the selected Security Entity.','You have changed your selected Security Entity.',@V_FORMAT_AS_HTML_FALSE,@V_SystemID, @V_Primary_Key, @V_Debug
exec ZGWCoreWeb.Set_Message -1,1,'SameAccountChangeAccount','Same Account Change Account','Message for when a account changes their own account.','showMSG("If you change your account the system will need to log you off.")',@V_FORMAT_AS_HTML_FALSE,@V_SystemID, @V_Primary_Key, @V_Debug

---- Insert States
Print 'Adding States'
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('AA',2,2,'Armed Forces Americas',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('AE',2,2,'Armed Forces Africa',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('AK',2,2,'Alaska',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('AL',2,2,'Alabama',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('AP',2,2,'Armed Forces Pacific',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('AR',2,2,'Arkansas',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('AS',2,2,'American Samoa',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('AZ',2,2,'Arizona',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('CA',2,2,'California',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('CO',2,2,'Colorado',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('CT',2,2,'Connecticut',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('DC',2,2,'District Of Columbia',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('DE',2,2,'Delaware',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('FL',2,2,'Florida',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('FM',2,2,'Federated States of Micronesia',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('GA',2,2,'Georgia',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('GU',2,2,'Gaum',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('HI',2,2,'Hawaii',@V_ACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('IA',2,2,'Iowa',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('ID',2,2,'Idaho',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('IL',2,2,'Illinois',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('IN',2,2,'Indiana',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('KS',2,2,'Kansas',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('KY',2,2,'Kentucky',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('LA',2,2,'Louisiana',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('MA',2,2,'Massachusetts',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('MD',2,2,'Maryland',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('ME',2,2,'Maine',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('MH',2,2,'Marshall Islands',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('MI',2,2,'Michigan',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('MN',2,2,'Minnesota',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('MO',2,2,'Missouri',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('MP',2,2,'Northern Mariana Islands',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('MS',2,2,'Mississippi',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('MT',2,2,'Montana',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('NC',2,2,'North Carolina',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('ND',2,2,'North Dakota',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('NE',2,2,'Nebraska',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('NH',2,2,'New Hampshire',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('NJ',2,2,'New Jersey',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('NM',2,2,'New Mexico',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('NV',2,2,'Nevada',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('NY',2,2,'New York',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('OH',2,2,'Ohio',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('OK',2,2,'Oklahoma',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('OR',2,2,'Oregon',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('PA',2,2,'Pennsylvania',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('PR',2,2,'Puerto Rico',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('PW',2,2,'Palau',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('RI',2,2,'Rhode Island',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('SC',2,2,'South Carolina',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('SD',2,2,'South Dakota',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('TN',2,2,'Tennessee',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('TX',2,2,'Texas',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('UT',2,2,'Utah',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('VA',2,2,'Virginia',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('VI',2,2,'Virgin Islands',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('VT',2,2,'Vermont',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('WA',2,2,'Washington',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('WI',2,2,'Wisconsin',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('WV',2,2,'West Virginia',@V_INACTIVE)
insert ZGWOptional.States([State],Added_By,Updated_By,[Description],StatusSeqId) values('WY',2,2,'Wyoming',@V_INACTIVE)
update statistics ZGWOptional.States
