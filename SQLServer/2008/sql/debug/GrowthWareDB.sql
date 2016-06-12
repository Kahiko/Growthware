/*
Deployment script for GWDevelopment
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar Path1 "D:\Data\SQL\"
:setvar Path2 "D:\Data\SQL\"
:setvar DatabaseName "GWDevelopment"
:setvar DefaultDataPath "D:\Data\SQL\2008R2\"
:setvar DefaultLogPath "D:\Data\SQL\2008R2\"

GO
:on error exit
GO
USE [master]
GO
IF (DB_ID(N'$(DatabaseName)') IS NOT NULL
    AND DATABASEPROPERTYEX(N'$(DatabaseName)','Status') <> N'ONLINE')
BEGIN
    RAISERROR(N'The state of the target database, %s, is not set to ONLINE. To deploy to this database, its state must be set to ONLINE.', 16, 127,N'$(DatabaseName)') WITH NOWAIT
    RETURN
END

GO

IF NOT EXISTS (SELECT 1 FROM [master].[dbo].[sysdatabases] WHERE [name] = N'$(DatabaseName)')
BEGIN
    RAISERROR(N'You cannot deploy this update script to target ASUS_G74S\SQL2008R2. The database for which this script was built, GWDevelopment, does not exist on this server.', 16, 127) WITH NOWAIT
    RETURN
END

GO

IF (@@servername != 'ASUS_G74S\SQL2008R2')
BEGIN
    RAISERROR(N'The server name in the build script %s does not match the name of the target server %s. Verify whether your database project settings are correct and whether your build script is up to date.', 16, 127,N'ASUS_G74S\SQL2008R2',@@servername) WITH NOWAIT
    RETURN
END

GO

IF CAST(DATABASEPROPERTY(N'$(DatabaseName)','IsReadOnly') as bit) = 1
BEGIN
    RAISERROR(N'You cannot deploy this update script because the database for which it was built, %s , is set to READ_ONLY.', 16, 127, N'$(DatabaseName)') WITH NOWAIT
    RETURN
END

GO
USE [$(DatabaseName)]
GO
/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
			   SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
SET NOCOUNT ON
DECLARE @V_Now datetime,
		@V_SystemID INT,
		@V_Security_Entity_SeqID INT,
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
		@V_Function_Type_SeqID INT,
		@V_ENCRYPTION_TYPE INT,
		@V_ENABLE_INHERITANCE INT,
		@V_NVP_SeqID INT,
		@V_SORT_ORDER INT,
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
SET @V_SORT_ORDER = 0
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
--************************

SET @V_CHANGE_PASSWORD = (select Status_SeqID from ZGWSystem.Statuses where [Name] = 'ChangePassword')
SET @V_INACTIVE = (select Status_SeqID from ZGWSystem.Statuses where [Name] = 'Inactive')
SET @V_ACTIVE = (select Status_SeqID from ZGWSystem.Statuses where [Name] = 'Active')
Print 'Adding Accounts'
-- Add the anonymous account
exec ZGWSecurity.Set_Account -1,1,'Anonymous','Anonymous','Anonymous','','Anonymous-Account','me@me.com','none',@V_Now,0,1,@V_Now,-5,'none',0,0, @V_Debug
-- BEFORE ADDING ANY MORE ACCOUNTS SETUP ZF_ACCT_CHOICES
exec ZGWCoreWeb.Set_Account_Choices 'Anonymous',1,'System','#ffffff','#eeeeee','#6699cc','#b6cbeb','Blue','Home','ThinActions','WideActions',5
-- Add the system administrator account
exec ZGWSecurity.Set_Account -1,@V_CHANGE_PASSWORD,'System','System','System','','System','michael.regan@verizon.net','none',@V_Now,0,1,@V_Now,-5,'none',0,1, @V_Debug
-- Add the system Developer account
exec ZGWSecurity.Set_Account -1,@V_CHANGE_PASSWORD,'Developer','System','Developer','','System-Developer','michael.regan@verizon.net','none',@V_Now,0,1,@V_Now,-5,'none',0,1, @V_Debug
-- testing account
exec ZGWSecurity.Set_Account -1,@V_CHANGE_PASSWORD,'Mike','System','Tester','','System-Tester','michael.regan@verizon.net','none',@V_Now,0,0,@V_Now,-5,'none',0,0, @V_Debug
set @V_SystemID = (select Account_SeqID from ZGWSecurity.Accounts where Account = 'System')
--
Print 'Adding NVP tables'
exec ZGWSystem.Set_Name_Value_Pair -1,'ZGWSecurity','Navigation_Types','Navigation Types','Navigation Types',1,1,null,null
exec ZGWSystem.Set_Name_Value_Pair -1,'ZGWSecurity','Permissions','Permissions','Permissions',1,1,null,null
exec ZGWSystem.Set_Name_Value_Pair -1,'ZGWCoreWeb','Link_Behaviors','Link Behaviors','Link Behaviors',1,1,null,null
exec ZGWSystem.Set_Name_Value_Pair -1,'ZGWCoreWeb','Work_Flows','Work Flows','Work Flows',1,1,null,null

Print 'Adding DB Information'
exec ZGWSystem.Set_Datase_Information -1,'3.0',@V_ENABLE_INHERITANCE,@V_SystemID, NULL, @V_Debug

Print 'Adding Function types'
-- Setup Functions Types
exec ZGWSecurity.Set_Function_Types -1,'Module','used for modules','','0',@V_SystemID,@V_PRIMARY_KEY, @V_ErrorCode
exec ZGWSecurity.Set_Function_Types -1,'Security','used as a container for security.','none','0',@V_SystemID,@V_PRIMARY_KEY, @V_ErrorCode
exec ZGWSecurity.Set_Function_Types -1,'Menu Item','designates entry is a menu item.','none','0',@V_SystemID,@V_PRIMARY_KEY, @V_ErrorCode
exec ZGWSecurity.Set_Function_Types -1,'Calendar','Used for managing files and Calendars','Modules\System\Calendar\CommunityCalendar.ascx','1',@V_SystemID,@V_PRIMARY_KEY, @V_ErrorCode
exec ZGWSecurity.Set_Function_Types -1,'File Manager','Used for managing files and directories','Modules\System\FileManagement\FileManagerControl.ascx','0',@V_SystemID,@V_PRIMARY_KEY, @V_ErrorCode

Print 'Adding navigation types'
-- Setup Navagation types
SET @V_NVP_SeqID = (SELECT NVP_SeqID FROM ZGWSystem.Name_Value_Pairs WHERE Static_Name = 'Navigation_Types')
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVP_SeqID,'Horizontal','Horizontal',@V_ACTIVE, @V_SORT_ORDER,@V_SystemID,@V_Primary_Key,@V_ErrorCode
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVP_SeqID,'Vertical','Vertical',@V_ACTIVE, @V_SORT_ORDER,@V_SystemID,@V_Primary_Key,@V_ErrorCode
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVP_SeqID,'Hierarchical','Hierarchical',@V_ACTIVE, @V_SORT_ORDER,@V_SystemID,@V_Primary_Key,@V_ErrorCode

SET @V_NAV_TYPE_Hierarchical = (SELECT NVP_Detail_SeqID FROM ZGWSecurity.Navigation_Types WHERE NVP_Detail_Value = 'Hierarchical')
SET @V_NAV_TYPE_Vertical = (SELECT NVP_Detail_SeqID FROM ZGWSecurity.Navigation_Types WHERE NVP_Detail_Value = 'Vertical')
SET @V_NAV_TYPE_Horizontal = (SELECT NVP_Detail_SeqID FROM ZGWSecurity.Navigation_Types WHERE NVP_Detail_Value = 'Horizontal')

Print 'Adding permissions'
SET @V_NVP_SeqID = (SELECT NVP_SeqID FROM ZGWSystem.Name_Value_Pairs WHERE Static_Name = 'Permissions')
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVP_SeqID,'View','View',@V_ACTIVE, @V_SORT_ORDER,@V_SystemID,@V_Primary_Key,@V_ErrorCode
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVP_SeqID,'Edit','Edit',@V_ACTIVE, @V_SORT_ORDER,@V_SystemID,@V_Primary_Key,@V_ErrorCode
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVP_SeqID,'Add','Add',@V_ACTIVE, @V_SORT_ORDER,@V_SystemID,@V_Primary_Key,@V_ErrorCode
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVP_SeqID,'Delete','Delete',@V_ACTIVE, @V_SORT_ORDER,@V_SystemID,@V_Primary_Key,@V_ErrorCode

SET @V_ViewPermission = (SELECT NVP_Detail_SeqID FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'View')
SET @V_AddPermission = (SELECT NVP_Detail_SeqID FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Add')
SET @V_EditPermission = (SELECT NVP_Detail_SeqID FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Edit')
SET @V_DeletePermission = (SELECT NVP_Detail_SeqID FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Delete')

Print 'Adding Link Behaviors'
SET @V_NVP_SeqID = (SELECT NVP_SeqID FROM ZGWSystem.Name_Value_Pairs WHERE Static_Name = 'Link_Behaviors')
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVP_SeqID,'Internal','Internal',@V_ACTIVE, @V_SORT_ORDER,@V_SystemID,@V_Primary_Key,@V_ErrorCode
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVP_SeqID,'Popup','Popup',@V_ACTIVE, @V_SORT_ORDER,@V_SystemID,@V_Primary_Key,@V_ErrorCode
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVP_SeqID,'External','External',@V_ACTIVE, @V_SORT_ORDER,@V_SystemID,@V_Primary_Key,@V_ErrorCode

SET @V_LinkBehaviorInternal = (SELECT NVP_Detail_SeqID FROM ZGWCoreWeb.Link_Behaviors WHERE NVP_Detail_Value = 'Internal')
SET @V_LinkBehaviorPopup = (SELECT NVP_Detail_SeqID FROM ZGWCoreWeb.Link_Behaviors WHERE NVP_Detail_Value = 'Popup')
SET @V_LinkBehaviorExternal = (SELECT NVP_Detail_SeqID FROM ZGWCoreWeb.Link_Behaviors WHERE NVP_Detail_Value = 'External')

Print 'Adding Security Entity'
exec ZGWSecurity.Set_Security_Entity -1,	'System','The default Security Entity, needed by the system.','no url',1,'SQLServer','GrowthWareFramework','GrowthWare.Framework.DataAccessLayer.SQLServer.V2008','server=(local)\SQL2008R2;Integrated Security=SSPI;database=GWDevelopment;connection reset=false;connection lifetime=5;enlist=true;min pool size=1;max pool size=50','Blue Arrow','Default',@V_Encryption_Type,-1,@V_SystemID, @V_PRIMARY_KEY, @V_Debug
SET @V_Security_Entity_SeqID = (SELECT Security_Entity_SeqID FROM ZGWSecurity.Security_Entities WHERE [NAME]='System')
Print 'Adding roles'
-- Setup ZF_RLS
exec ZGWSecurity.Set_Role -1,'Anonymous','The anonymous role.',1,0,@V_Security_Entity_SeqID, @V_SystemID, @V_PRIMARY_KEY, @V_Debug
exec ZGWSecurity.Set_Role -1,'Authenticated','The authenticated role.',1,0,@V_Security_Entity_SeqID, @V_SystemID, @V_PRIMARY_KEY, @V_Debug
exec ZGWSecurity.Set_Role -1,'Developer','The developer role.',1,0,@V_Security_Entity_SeqID, @V_SystemID, @V_PRIMARY_KEY, @V_Debug
exec ZGWSecurity.Set_Role -1,'AlwaysLogon','Assign this role to allow logon when the system is under maintance.',1,0,@V_Security_Entity_SeqID, @V_SystemID, @V_PRIMARY_KEY, @V_Debug

Print 'Adding Groups'
-- group id,group name,group description,Security Entity,added by,added date,updated by,updated date
exec ZGWSecurity.Set_Group -1,'Everyone','Group representing both the authenticated and the anonymous roles.', @V_Security_Entity_SeqID, @V_SystemID, @V_PRIMARY_KEY, @V_Debug

SET @V_EVERYONE_ID = (SELECT Group_SeqID FROM ZGWSecurity.Groups WHERE [NAME]='Everyone')
-- group id, Security Entity,comma sep roles,added by,ErrorCode
EXEC ZGWSecurity.Set_Group_Roles @V_EVERYONE_ID,@V_Security_Entity_SeqID,'Authenticated,Anonymous',@V_SystemID, @V_Debug
								 
Print 'Adding account security'
-- Setup the security
-- Setup the account security
exec ZGWSecurity.Set_Account_Roles 'Anonymous',@V_Security_Entity_SeqID,'Anonymous', @V_SystemID, @V_Debug
exec ZGWSecurity.Set_Account_Roles 'Developer',@V_Security_Entity_SeqID,'Developer,Authenticated,AlwaysLogon', @V_SystemID, @V_Debug
exec ZGWSecurity.Set_Account_Roles 'mike',@V_Security_Entity_SeqID,'Authenticated', @V_SystemID, @V_Debug

Print 'Adding NVP Roles'
SET @V_NVP_SeqID = (select NVP_SeqID FROM ZGWSystem.Name_Value_Pairs where Static_Name = 'Navigation_Types')
exec ZGWSecurity.Set_Name_Value_Pair_Roles @V_NVP_SeqID, @V_Security_Entity_SeqID, 'Developer', @V_ViewPermission, @V_SystemID, @V_Debug
SET @V_NVP_SeqID = (select NVP_SeqID FROM ZGWSystem.Name_Value_Pairs where Static_Name = 'Permissions')
exec ZGWSecurity.Set_Name_Value_Pair_Roles @V_NVP_SeqID, @V_Security_Entity_SeqID, 'Developer', @V_ViewPermission, @V_SystemID, @V_Debug

Print 'Adding functions'
-- Add functions

SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_EnableViewStateTrue = 1
set @V_EnableViewStateFalse = 0
set @V_IsNavTrue = 1
set @V_IsNavFalse = 0

Print 'Adding Root Menu'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Menu Item')
set @V_MyAction = 'Root_Menu'
exec ZGWSecurity.Set_Function -1,'Root Menu','Place Holer',@V_Function_Type_SeqID,'none',@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,1,'Root_Menu', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
Print 'Adding GenericHome'
set @V_MyAction = 'Generic_Home'
exec ZGWSecurity.Set_Function -1,'Home','Generic Home',@V_Function_Type_SeqID,'Modules\System\Home\GenericHome.ascx',@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,1,'Shown when not authenticated', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous',@V_ViewPermission,@V_SystemID,@V_Debug
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = @V_MyAction)

Print 'Adding Home'
set @V_MyAction = 'Home'
exec ZGWSecurity.Set_Function -1,'Home','Home',@V_Function_Type_SeqID,'Modules\System\Home\Home.ascx',@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Shown when authenticated', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID,@V_Debug

Print 'Adding Logon'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Root_Menu')
set @V_MyAction = 'Logon'
exec ZGWSecurity.Set_Function -1,'Logon','Logon',@V_Function_Type_SeqID,'Modules\System\Logon\Logon.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Vertical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Loggs on an account', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Developer',@V_ViewPermission,@V_SystemID,@V_Debug

Print 'Adding Favorite'
set @V_MyAction = 'Favorite'
exec ZGWSecurity.Set_Function -1,'Favorite','Favorite',@V_Function_Type_SeqID,'Modules\System\Accounts\Favorite.ascx',@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Allows client to set a Favorite action.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID,@V_Debug

Print 'Adding Natural Sort'
set @V_MyAction = 'Natural_Sort'
exec ZGWSecurity.Set_Function -1,'Natural Sort','Natural Sort',@V_Function_Type_SeqID,'Modules\System\TestNaturalSort.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Vertical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Shows natural sort order vs	ANSI', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_SystemID,@V_Debug

Print 'Adding Logoff'
set @V_MyAction = 'Logoff'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Menu Item')
exec ZGWSecurity.Set_Function -1,'Logoff','Logoff',@V_Function_Type_SeqID,'Modules\System\Accounts\Logoff.ascx',@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Loggs off the system.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID,@V_Debug

Print 'Adding Admin'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Menu Item')
set @V_MyAction = 'Admin'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Root_Menu')
exec ZGWSecurity.Set_Function -1,'Admin','Administration',@V_Function_Type_SeqID,'none',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Administration tasks.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

Print 'Adding Calendars'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Menu Item')
set @V_MyAction = 'Calendars'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Root_Menu')
exec ZGWSecurity.Set_Function -1,'Calendars','Calendars',@V_Function_Type_SeqID,'none',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Access to the calendar.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated,Developer',@V_ViewPermission,@V_SystemID,@V_Debug

SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Menu Item')
set @V_MyAction = 'Reports'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Root_Menu')
exec ZGWSecurity.Set_Function -1,'Reports','Reports',@V_Function_Type_SeqID,'none',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Access to the reports.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Menu Item')
set @V_MyAction = 'My_Profile'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Root_Menu')
exec ZGWSecurity.Set_Function -1,'My Profile','My Profile',@V_Function_Type_SeqID,'none',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Access to profile information.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated,Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding System Administrator menu'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Menu Item')
set @V_MyAction = 'System_Administration'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Root_Menu')
exec ZGWSecurity.Set_Function -1,'SysAdmin','System Administration',@V_Function_Type_SeqID,'none',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Serves as the root menu item for the hierarchical menus.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Root_Menu')
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Manage Functions'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Menu Item')
set @V_MyAction = 'Manage_Functions'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'Manage Functions','Manage Functions',@V_Function_Type_SeqID,'none',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Menu item for functions.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Add Functions'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Add_Functions'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Manage_Functions')
exec ZGWSecurity.Set_Function -1,'Add Functions','Add Functions',@V_Function_Type_SeqID,'Modules\System\Administration\Functions\AddEditFunctions.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Adds a function to the system.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Copy Function Security'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Copy_Function_Security'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Manage_Functions')
exec ZGWSecurity.Set_Function -1,'Copy Function Security','Copy Function Security',@V_Function_Type_SeqID,'Modules\System\Administration\Functions\CopyFunctionSecurity.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Adds a function to the system.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Manage Security Entitys'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Menu Item')
set @V_MyAction = 'Manage_SECURITY_ENTITIES'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'Manage Security Entitys','Manage Security Entitys',@V_Function_Type_SeqID,'none',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Menu item for Security Entitys.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'File Management menu'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Menu Item')
set @V_MyAction = 'Manage_Files'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'Manage Files','Manage Files',@V_Function_Type_SeqID,'none',@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to manage files.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'cache directory management'
-- Add module
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'File Manager')
SET @V_MyAction = 'Manage_Cache_Dependency'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Manage_Files')
EXEC ZGWSecurity.Set_Function -1,'Manage Cachedependency','Manage Cachedependency',@V_Function_Type_SeqID,'Modules\System\FileManagement\FileManagerControl.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to manage the cache dependency direcory.', @V_SystemID, @V_Debug
-- Set security
SET @V_Function_Type_SeqID = (select Function_Type_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
EXEC ZGWSecurity.Set_Function_Roles @V_Function_Type_SeqID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
EXEC ZGWSecurity.Set_Function_Roles @V_Function_Type_SeqID,1,'Developer',@V_AddPermission,@V_SystemID, @V_Debug
EXEC ZGWSecurity.Set_Function_Roles @V_Function_Type_SeqID,1,'Developer',@V_EditPermission,@V_SystemID, @V_Debug
EXEC ZGWSecurity.Set_Function_Roles @V_Function_Type_SeqID,1,'Developer',@V_DeletePermission,@V_SystemID, @V_Debug
PRINT 'Adding cache directory'
-- Add directory information
SET @V_Function_Type_SeqID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = @V_MyAction)
EXEC ZGWOptional.Set_Directory @V_Function_Type_SeqID,'D:\WebProjects\2008\WWWGrowthWare\CacheDependency',0,'','',@V_SystemID,@V_PRIMARY_KEY, @V_Debug

PRINT 'cache directory management'
-- Add module
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'File Manager')
SET @V_MyAction = 'Manage_Logs'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Manage_Files')
EXEC ZGWSecurity.Set_Function -1,'Manage Logs','Manage Logs',@V_Function_Type_SeqID,'Modules\System\FileManagement\FileManagerControl.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to manage the logs direcory.', @V_SystemID, @V_Debug
SET @V_Function_Type_SeqID = (select Function_Type_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
-- Set security
EXEC ZGWSecurity.Set_Function_Roles @V_Function_Type_SeqID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
EXEC ZGWSecurity.Set_Function_Roles @V_Function_Type_SeqID,1,'Developer',@V_AddPermission,@V_SystemID, @V_Debug
EXEC ZGWSecurity.Set_Function_Roles @V_Function_Type_SeqID,1,'Developer',@V_EditPermission,@V_SystemID, @V_Debug
EXEC ZGWSecurity.Set_Function_Roles @V_Function_Type_SeqID,1,'Developer',@V_DeletePermission,@V_SystemID, @V_Debug
PRINT 'Adding log log'
-- Add directory information
SET @V_Function_Type_SeqID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = @V_MyAction)
EXEC ZGWOptional.Set_Directory @V_Function_Type_SeqID,'D:\WebProjects\2008\WWWGrowthWare\Logs',0,'','',@V_SystemID,@V_PRIMARY_KEY, @V_Debug

print 'Adding Manage Name/Value Pairs'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Menu Item')
set @V_MyAction = 'Manage_Name_Value_Pairs'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'Manage Name/Value Pairs','Manage Name/Value Pairs',@V_Function_Type_SeqID,'none',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Menu item for name/value pairs.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Add Edit Groups'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Add_Edit_Groups'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Add Edit Groups','Add Edit Groups',@V_Function_Type_SeqID,'Modules\System\Administration\Groups\AddEditGroups.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorPopup,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add or edit roles.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_AddPermission,@V_SystemID,@V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID,@V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_DeletePermission,@V_SystemID,@V_Debug

print 'Adding Manage Messages'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Menu Item')
set @V_MyAction = 'Manage_Messages'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Manage Messages','Manage Messages',@V_Function_Type_SeqID,'none',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Menu item for messages.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Manage States'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Menu Item')
set @V_MyAction = 'Manage_States'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'Manage States','Manage States',@V_Function_Type_SeqID,'none',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Menu item for states.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Manage Work Flows'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Menu Item')
set @V_MyAction = 'Work_Flows'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'Manage Work Flows','Manage Work Flows',@V_Function_Type_SeqID,'none',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Menu item for work flows.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Encryption Helper'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Encryption_Helper'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'Encryption Helper','Encryption Helper',@V_Function_Type_SeqID,'Modules\System\Administration\Encrypt\EncryptDecrypt.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Menu item for work flows.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding GUID Helper'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'GUID_Helper'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'GUID Helper','Displays''s a GUID',@V_Function_Type_SeqID,'Modules\System\Administration\Encrypt\GUIDHelper.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Displays a GUID may be necessary if you need to change the GUID in your project files.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Random Number'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Random_Numbers'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'Random Numbers','Displays''s a set of randomly generated number''s',@V_Function_Type_SeqID,'Modules\System\Administration\Encrypt\RandomNumbers.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Displays''s a set of randomly generated number''s.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Set Log Level'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Set_Log_Level'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'Set Log Level','Set Log Level',@V_Function_Type_SeqID,'Modules\System\Administration\Logs\SetLogLevel.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to set the log level of the application ... Debug, Error, Warn, Fatal.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Update Anonymous Profile'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Update_Anonymous_Profile'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'Update Anonymous Profile','Update Anonymous Profile',@V_Function_Type_SeqID,'Modules\System\Administration\AnonymousAccount\UpdateAnonymousCache.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Remove any cached information for the anonymous account.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Search Functions'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Search_Functions'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Manage_Functions')
exec ZGWSecurity.Set_Function -1,'Search Functions','Search Functions',@V_Function_Type_SeqID,'Modules\System\Administration\Functions\SearchFunctions.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Searches for functions in the system.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_DeletePermission,@V_SystemID,@V_Debug

print 'Adding Edit Functions'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Edit_Functions'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'Edit Functions','Edit Functions',@V_Function_Type_SeqID,'Modules\System\Administration\Functions\AddEditFunctions.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Edits a function in the system.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Function Security'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Function_Security'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Reports')
exec ZGWSecurity.Set_Function -1,'Function Security','Function Security',@V_Function_Type_SeqID,'Modules\System\Reports\FunctionSecurity.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Displays a report for function security.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Security By Role'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Security_By_Role'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Reports')
exec ZGWSecurity.Set_Function -1,'Security By Role','Security By Role',@V_Function_Type_SeqID,'Modules\System\Reports\SecurityByRole.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Displays a report for security by role.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Change Password'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Change_Password'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'My_Profile')
exec ZGWSecurity.Set_Function -1,'Change Password','Change Password',@V_Function_Type_SeqID,'Modules\System\Accounts\ChangePassword.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to change an accounts password.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Change Colors'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Change_Colors'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'My_Profile')
exec ZGWSecurity.Set_Function -1,'Change Colors','Change Colors',@V_Function_Type_SeqID,'Modules\System\Accounts\ChangeColors.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to change an accounts color scheme.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Select Preferences'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Select_Preferences'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'My_Profile')
exec ZGWSecurity.Set_Function -1,'Select Preferences','Select Preferences',@V_Function_Type_SeqID,'Modules\System\Accounts\SelectPreferences.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to select preference for an account, records per page etc.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Edit Account'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Edit_Account'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'My_Profile')
exec ZGWSecurity.Set_Function -1,'Edit Account','Edit Account',@V_Function_Type_SeqID,'Modules\System\Administration\Accounts\AddEditAccount.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to edit an account profile.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Edit Other Account'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Edit_Other_Account'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'My_Profile')
exec ZGWSecurity.Set_Function -1,'Edit Other Account','Edit Other Account',@V_Function_Type_SeqID,'Modules\System\Administration\Accounts\AddEditAccount.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to edit anothers account profile.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID,@V_Debug

print 'Adding Community Calendar'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Community_Calendar'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Calendars')
exec ZGWSecurity.Set_Function -1,'Community Calendar','Community Calendar',@V_Function_Type_SeqID,'Modules\System\Calendar\CommunityCalendar.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to show calendar data.  Created as an example module.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Add Account'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Add_Account'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Add Account','Add Account',@V_Function_Type_SeqID,'Modules\System\Administration\Accounts\AddEditAccount.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add an accounts password.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

Print 'Adding Register Account'
set @V_MyAction = 'Register_Account'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Root_Menu')
exec ZGWSecurity.Set_Function -1,'Register Account','Register Account',@V_Function_Type_SeqID,'Modules\System\Administration\Accounts\AddEditAccount.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Vertical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add an accounts password.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
--exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Groups @V_FunctionID,1,'Everyone',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Add Edit Roles'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Add_Edit_Roles'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Add Edit Roles','Add Edit Roles',@V_Function_Type_SeqID,'Modules\System\Administration\Roles\AddEditRoles.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorPopup,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add or edit roles.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_AddPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_DeletePermission,@V_SystemID, @V_Debug

print 'Adding Add Edit Name Value Pairs Details'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Add_Edit_Name_Value_Pair_Details'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Add Edit Name Value Pairs','Add Edit Name Value Pairs',@V_Function_Type_SeqID,'Modules\System\Administration\NVP\AddEditNVPDetails.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add or edit a list of value details.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding ViewAccountRoleTab'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Security')
set @V_MyAction = 'View_Account_Role_Tab'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'ViewAccountRoleTab','View Accounts Roles Tab',@V_Function_Type_SeqID,'None',@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used as a security holder for roles that can view the accounts role tab.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding ViewFunctionRoleTab'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Security')
set @V_MyAction = 'View_Function_Role_Tab'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'ViewFunctionRoleTab','View Functions Roles Tab',@V_Function_Type_SeqID,'None',@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used as a security holder for roles that can view the functions role tab.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding ViewAccountGroupTab'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Security')
set @V_MyAction = 'View_Account_Group_Tab'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'ViewAccountGroupTab','View Accounts Groups Tab',@V_Function_Type_SeqID,'None',@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used as a security holder for groups that can view the accounts group tab.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding ViewFunctionGroupTab'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Security')
set @V_MyAction = 'View_Function_Group_Tab'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'ViewFunctionGroupTab','View Function Groups Tab',@V_Function_Type_SeqID,'None',@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used as a security holder for groups that can view the functions group tab.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Search Accounts'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Search_Accounts'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Search Accounts','Search Accounts',@V_Function_Type_SeqID,'Modules\System\Administration\Accounts\SearchAccounts.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to select accounts for edit.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Edit Role Members'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Edit_Role_Members'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Edit Role Members','Edit Role Members',@V_Function_Type_SeqID,'Modules\System\Administration\Roles\EditRoleMembers.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UITrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add or remove members of a role.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID, @V_Debug

print 'Adding Add Role'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Add_Role'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Add Role','Add Role',@V_Function_Type_SeqID,'Modules\System\Administration\Roles\AddRole.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UITrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Adds a role.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID, @V_Debug

print 'Adding Edit Group Members'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Edit_Group_Members'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Edit Group Members','Edit Group Members',@V_Function_Type_SeqID,'Modules\System\Administration\Groups\EditGroupMembers.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UITrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add or remove members of a role.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Add A Group'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Add_Group'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Add A Group','Add A Group',@V_Function_Type_SeqID,'Modules\System\Administration\Groups\AddGroup.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UITrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add or remove members of a role.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Navigation'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Vertical_Menu'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Navigation','Navigation',@V_Function_Type_SeqID,'Modules\System\Navigation\VerticalMenuUserControl.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Contains link items for the vertical menus.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Not Avalible'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Not_Avalible'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Not Avalible','Not Avalible',@V_Function_Type_SeqID,'Modules\System\Errors\NotAvailable.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Error page when the action is not avalible.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug
--AccessDenied
print 'Adding Access Denied'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Access_Denied'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Access Denied','Access Denied',@V_Function_Type_SeqID,'Modules\System\Errors\AccessDenied.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Error page when the account being used does not have sufficient access to the view permission.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug
--Adding Error
print 'Adding Error'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Display_Error'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Display Error','Display Error',@V_Function_Type_SeqID,'Modules\System\Errors\DisplayError.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsTrue,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Error page when unknown or unexpected error occurs.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug

--Select A Security Entity
print 'Adding Select A Security Entity'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Select_A_Security_Entity'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Admin')
exec ZGWSecurity.Set_Function -1,'Select A Security Entity','Select A Security Entity',@V_Function_Type_SeqID,'Modules\System\SecurityEntities\SelectSecurityEntity.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Vertical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to select a Security Entity.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug
-- Web configuration
print 'Adding Web configuration'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Web_Config'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'Web Config','Web Config',@V_Function_Type_SeqID,'Modules\System\Administration\Configuration\AddEditWebConfig.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Adds or edits web.config file settings.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Line Count
print 'Adding Line Count'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Line_Count'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'Line Count','Line Count',@V_Function_Type_SeqID,'Modules\System\LineCount.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Utility to count the lines of code.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Add a Security Entity
print 'Adding Add Security Entitys'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Add_SECURITY_ENTITIES'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Manage_SECURITY_ENTITIES')
exec ZGWSecurity.Set_Function -1,'Add Security Entitys','Add Security Entitys',@V_Function_Type_SeqID,'Modules\System\Administration\SecurityEntities\AddEditSecurityEntities.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add a Security Entity.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Edit a Security Entity
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Edit_A_Security_Entity'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Manage_SECURITY_ENTITIES')
exec ZGWSecurity.Set_Function -1,'Edit a Security Entity','Edit a Security Entity',@V_Function_Type_SeqID,'Modules\System\Administration\SecurityEntities\AddEditSecurityEntities.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to edit a Security Entity.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Search Security Entitys
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Search_SECURITY_ENTITIES'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Manage_SECURITY_ENTITIES')
exec ZGWSecurity.Set_Function -1,'Search Security Entitys','Search Security Entitys',@V_Function_Type_SeqID,'Modules\System\Administration\SecurityEntities\SearchSecurityEntities.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to search a Security Entity.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

-- Add a Name Value Pair
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Add_Name_Value_Pairs'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Manage_Name_Value_Pairs')
exec ZGWSecurity.Set_Function -1,'Add Name Value Pairs','Add Security Entitys',@V_Function_Type_SeqID,'Modules\System\Administration\NVP\AddEditNVP.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add a name/value pair.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Edit a Name Value Pair
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Edit_Name_Value_Pairs'
--SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Manage_SECURITY_ENTITIES')
exec ZGWSecurity.Set_Function -1,'Edit a Name Value Pair','Edit a Name Value Pair',@V_Function_Type_SeqID,'Modules\System\Administration\NVP\AddEditNVP.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to edit a name/value pair.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Search Name Value Pairs
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Search_Name_Value_Pairs'
--SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Manage_SECURITY_ENTITIES')
exec ZGWSecurity.Set_Function -1,'Search Name Value Pairs','Search Name Value Pairs',@V_Function_Type_SeqID,'Modules\System\Administration\NVP\SearchNVP.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to search a name/value pair.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Add a Message
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Add_Message'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Manage_Messages')
exec ZGWSecurity.Set_Function -1,'Add Message','Add Message',@V_Function_Type_SeqID,'Modules\System\Administration\Messages\AddEditMessage.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add a message.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Edit a Message
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Edit_Message'
--SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Manage_SECURITY_ENTITIES')
exec ZGWSecurity.Set_Function -1,'Edit a Message','Edit a Message',@V_Function_Type_SeqID,'Modules\System\Administration\Messages\AddEditMessage.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to edit a Message.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Search Message
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Search_Messages'
--SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Manage_SECURITY_ENTITIES')
exec ZGWSecurity.Set_Function -1,'Search Messages','Search Messages',@V_Function_Type_SeqID,'Modules\System\Administration\Messages\SearchMessages.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to search a Message.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug

-- Edit a State
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Edit_State'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Manage_States')
exec ZGWSecurity.Set_Function -1,'Edit a State','Edit a State',@V_Function_Type_SeqID,'Modules\System\Administration\States\AddEditStates.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to edit a State.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Search State
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Search_States'
--SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Manage_SECURITY_ENTITIES')
exec ZGWSecurity.Set_Function -1,'Search States','Search States',@V_Function_Type_SeqID,'Modules\System\Administration\States\SearchStates.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to search a State.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Add Edit Workflows
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Add_Edit_Workflow'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Work_Flows')
--SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'Manage_SECURITY_ENTITIES')
exec ZGWSecurity.Set_Function -1,'Add/Edit Workflows','Add/Edit Workflows',@V_Function_Type_SeqID,'Modules\System\Administration\WorkFlow\AddEditWorkFlow.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to add or edit a Workflow.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug
-- Update Session
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Update'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'Update','Update',@V_Function_Type_SeqID,'Modules\System\Accounts\UpdateSession.ascx',@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to update the session menus and roles.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug
-- Under Maintance
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Under_Maintance'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'Under Maintance','Under Maintance',@V_Function_Type_SeqID,'Modules\System\Administration\UnderMaintance.ascx',@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to update the session menus and roles.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_SystemID, @V_Debug
print 'Adding AlwaysLogon'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Security')
set @V_MyAction = 'Always_Logon'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'Always Logon','Always Logon',@V_Function_Type_SeqID,'none',@V_EnableViewStateFalse,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to update the session menus and roles.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'AlwaysLogon',@V_ViewPermission,@V_SystemID, @V_Debug

print 'Adding Edit DB Information'
SET @V_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM ZGWSecurity.Function_Types WHERE [NAME] = 'Module')
set @V_MyAction = 'Edit_DB_Information'
SET @V_ParentID = (SELECT Function_SeqID FROM ZGWSecurity.Functions WHERE [ACTION] = 'System_Administration')
exec ZGWSecurity.Set_Function -1,'Edit DB Information','Edit DB Information',@V_Function_Type_SeqID,'Modules\System\Administration\Configuration\AddEditDBInformation.ascx',@V_EnableViewStateTrue,@V_EnableNotificationsFalse,@V_Redirect_On_Timeout,@V_IsNavTrue,@V_LinkBehaviorInternal,@V_NO_UIFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_META_KEY_WORDS,@V_ParentID,'Used to update the ZF_Information table, enable inheritence.', @V_SystemID, @V_Debug
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
exec ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_ViewPermission,@V_SystemID, @V_Debug


--Print 'Adding work flow details'
---- Setup Navigation_Types
SET @V_NVP_SeqID = (SELECT NVP_SeqID FROM ZGWSystem.Name_Value_Pairs WHERE Static_Name = 'Work_Flows')
SET @V_MyAction = 'Change_Password'
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
SET @V_SORT_ORDER = 1
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVP_SeqID,'Change Password',@V_FunctionID,@V_ACTIVE, @V_SORT_ORDER,@V_SystemID, @V_PRIMARY_KEY, NULL, @v_Debug
SET @V_MyAction = 'Home'
set @V_FunctionID = (select Function_SeqID from ZGWSecurity.Functions where action=@V_MyAction)
SET @V_SORT_ORDER = 2
exec ZGWSystem.Set_Name_Value_Pair_Detail -1,@V_NVP_SeqID,'Change Password',@V_FunctionID,@V_ACTIVE, @V_SORT_ORDER,@V_SystemID, @V_PRIMARY_KEY, NULL, @v_Debug
SET @V_SORT_ORDER = 0

PRINT 'Adding messages'
exec ZGWCoreWeb.Set_Message -1,@V_Security_Entity_SeqID,'Logon Error','Logon Error','Displayed when logon fails','<b>Invalid Account or Password!</b>',@V_FORMAT_AS_HTML_TRUE,@V_SystemID, @V_Primary_Key, @V_Debug
				   
exec ZGWCoreWeb.Set_Message -1,@V_Security_Entity_SeqID,'New Account','New Account','Message sent when an account is created.','Dear <FullName>,

There has been a request for a new account: 

	Please Use this link to logon:
 <Server>Default.aspx?Action=Logon&Account=<AccountName>&Password=<Password>

<b>Please note once you have logged on using this link you will only be able to change our password.</b>',@V_FORMAT_AS_HTML_FALSE,@V_SystemID, @V_Primary_Key, @V_Debug

exec ZGWCoreWeb.Set_Message -1,@V_Security_Entity_SeqID,'Request Password Reset UI','Request Password Reset UI','Displayed when new password is requested','<b>An EMail has been send to your account with instructions!</b>',@V_FORMAT_AS_HTML_TRUE,@V_SystemID, @V_Primary_Key, @V_Debug
exec ZGWCoreWeb.Set_Message -1,@V_Security_Entity_SeqID,'RequestNewPassword','Request New Password','Request New Password','Dear <FullName>,

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
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('AA','Armed Forces Americas',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('AE','Armed Forces Africa',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('AK','Alaska',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('AL','Alabama',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('AP','Armed Forces Pacific',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('AR','Arkansas',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('AS','American Samoa',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('AZ','Arizona',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('CA','California',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('CO','Colorado',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('CT','Connecticut',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('DC','District Of Columbia',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('DE','Delaware',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('FL','Florida',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('FM','Federated States of Micro',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('GA','Georgia',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('GU','Gaum',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('HI','Hawaii',@V_ACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('IA','Iowa',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('ID','Idaho',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('IL','Illinois',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('IN','Indiana',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('KS','Kansas',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('KY','Kentucky',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('LA','Louisiana',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('MA','Massachusetts',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('MD','Maryland',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('ME','Maine',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('MH','Marshall Islands',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('MI','Michigan',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('MN','Minnesota',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('MO','Missouri',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('MP','Northern Mariana Islands',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('MS','Mississippi',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('MT','Montana',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('NC','North Carolina',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('ND','North Dakota',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('NE','Nebraska',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('NH','New Hampshire',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('NJ','New Jersey',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('NM','New Mexico',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('NV','Nevada',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('NY','New York',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('OH','Ohio',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('OK','Oklahoma',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('OR','Oregon',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('PA','Pennsylvania',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('PR','Puerto Rico',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('PW','Palau',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('RI','Rhode Island',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('SC','South Carolina',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('SD','South Dakota',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('TN','Tennessee',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('TX','Texas',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('UT','Utah',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('VA','Virginia',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('VI','Virgin Islands',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('VT','Vermont',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('WA','Washington',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('WI','Wisconsin',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('WV','West Virginia',@V_INACTIVE)
insert ZGWOptional.States([State],[DESCRIPTION],Status_SeqID) values('WY','Wyoming',@V_INACTIVE)
update statistics ZGWOptional.States
*/

GO
