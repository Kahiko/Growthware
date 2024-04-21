CREATE PROCEDURE [ZFP_POP_BASE_DATA]
	@P_ErrorCode int OUTPUT
AS
SET NOCOUNT ON
DECLARE @now datetime
set @now = getdate()
/* */
SET NOCOUNT ON
DECLARE @V_DeveloperID INT
DECLARE @V_SE_SEQ_ID INT
DECLARE @V_EVERYONE_ID INT
DECLARE @V_MyAction VARCHAR(256)
DECLARE @V_FUNCTION_TYPE INT
DECLARE @V_EnableViewStateTrue INT
DECLARE @V_EnableViewStateFalse INT
DECLARE @V_IsNavTrue INT
DECLARE @V_IsNavFalse INT
DECLARE @V_NAV_TYPE INT
DECLARE @V_ParentID INT
DECLARE @V_FunctionID INT
DECLARE @V_ViewPermission INT
DECLARE @V_AddPermission INT
DECLARE @V_EditPermission INT
DECLARE @V_DeletePermission INT
DECLARE @V_NAV_TYPE_Hierarchical INT
DECLARE @V_NAV_TYPE_Vertical INT
DECLARE @V_NAV_TYPE_Horizontal INT
DECLARE @V_CHANGE_PASSWORD INT
DECLARE @V_INACTIVE INT
DECLARE @V_ACTIVE INT
DECLARE @V_ALLOW_HTML_INPUT INT
DECLARE @V_ALLOW_COMMENT_HTML_INPUT INT
DECLARE @V_IS_CONTENT INT
DECLARE @V_FORMAT_AS_HTML_TRUE INT
DECLARE @V_FORMAT_AS_HTML_FALSE INT
DECLARE @V_PRIMARY_KEY INT
DECLARE @V_ErrorCode INT
DECLARE @V_FUNCTION_SEQ_ID INT
DECLARE @V_ENCRYPTION_TYPE INT
DECLARE @V_ENABLE_INHERITANCE INT

Begin
	SET @V_FORMAT_AS_HTML_TRUE = 1 -- FALSE
	SET @V_FORMAT_AS_HTML_FALSE = 0 -- FALSE
	SET @V_ALLOW_HTML_INPUT = 0 -- FALSE
	SET @V_ALLOW_COMMENT_HTML_INPUT = 0 -- FALSE
	SET @V_IS_CONTENT = 0 -- FALSE
	SET @V_PRIMARY_KEY = NULL -- Not needed when setup up the database
	SET @V_ErrorCode = NULL -- Not needed when setup up the database
	SET @V_ENCRYPTION_TYPE = 1 -- TripleDES
	SET @V_ENABLE_INHERITANCE = 1 -- 0 = FALSE 1 = TRUE

	-- Setup ZFC_SYSTEM_STATUS
	Print 'Adding System Status'
	exec ZFP_SET_SYSTEM_STATUS -1,'Active',1,@now,1,@now,@V_PRIMARY_KEY,@V_ErrorCode
	exec ZFP_SET_SYSTEM_STATUS -1,'Inactive',1,@now,1,@now,@V_PRIMARY_KEY,@V_ErrorCode
	exec ZFP_SET_SYSTEM_STATUS -1,'Disabled',1,@now,1,@now,@V_PRIMARY_KEY,@V_ErrorCode
	exec ZFP_SET_SYSTEM_STATUS -1,'ChangePassword',1,@now,1,@now,@V_PRIMARY_KEY,@V_ErrorCode
	SET @V_CHANGE_PASSWORD = (select STATUS_SEQ_ID from ZFC_SYSTEM_STATUS where [DESCRIPTION] = 'ChangePassword')
	SET @V_INACTIVE = (select STATUS_SEQ_ID from ZFC_SYSTEM_STATUS where [DESCRIPTION] = 'Inactive')
	SET @V_ACTIVE = (select STATUS_SEQ_ID from ZFC_SYSTEM_STATUS where [DESCRIPTION] = 'Active')
	--
	Print 'Adding Accounts'
	-- Add the anonymous account
	exec ZFP_SET_ACCOUNT -1,1,'Anonymous','Anonymous','Anonymous','','Anonymous-Account','me@me.com','none',@now,0,1,@now,@now,-5,'none',0,0,0,@now,@V_PRIMARY_KEY,@V_ErrorCode
	-- BEFORE ADDING ANY MORE ACCOUNTS SETUP ZF_ACCT_CHOICES
	exec ZFP_SET_ACCT_CHOICES 'Anonymous',1,'System','#ffffff','#eeeeee','#6699cc','#b6cbeb','Blue','FavoriateAction','ThinActions','WideActions',5
	-- Add the system administrator account
	exec ZFP_SET_ACCOUNT -1,@V_CHANGE_PASSWORD,'Developer','System','Developer','','System-Developer','michael.regan@verizon.net','none',@now,0,1,@now,@now,-5,'none',0,1,1,@now,@V_PRIMARY_KEY,@V_ErrorCode
	-- testing account
	exec ZFP_SET_ACCOUNT -1,@V_CHANGE_PASSWORD,'Mike','System','Tester','','System-Tester','michael.regan@verizon.net','none',@now,0,0,@now,@now,-5,'none',0,0,1,@now,@V_PRIMARY_KEY,@V_ErrorCode
	set @V_DeveloperID = (select ACCT_SEQ_ID from ZFC_ACCTS where ACCT = 'Developer')

	Print 'Adding DB Information'
	exec ZFP_SET_INFORMATION -1,'1.0',@V_ENABLE_INHERITANCE,@V_DeveloperID, NULL,NULL

	Print 'Adding Function types'
	-- Setup ZFC_FUNCTION_TYPES
	exec ZFP_SET_FUNCTION_TYPES -1,'Module','used for modules','','0',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_FUNCTION_TYPES -1,'Security','used as a container for security.','none','0',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_FUNCTION_TYPES -1,'Menu Item','designates entry is a menu item.','none','0',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_FUNCTION_TYPES -1,'File Manager','Used for managing files and directories','Modules\System\FileManagement\FileManagerControl.ascx','0',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_FUNCTION_TYPES -1,'Articles','Contains articles that can represent news articles or other content','Modules\System\Content\ContentControl.ascx','1',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_FUNCTION_TYPES -1,'Links','Contains internal and external links','Modules\System\Content\ContentControl.ascx','1',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_FUNCTION_TYPES -1,'Downloads','Allows downloading of files','Modules\System\Content\ContentControl.ascx','1',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_FUNCTION_TYPES -1,'Photo Gallery','Enables you to display a gallery of images','Modules\System\Content\ContentControl.ascx','1',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_FUNCTION_TYPES -1,'Books','Contains book listings','Modules\System\Content\ContentControl.ascx','1',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_FUNCTION_TYPES -1,'Events','Contains event listings','Modules\System\Content\ContentControl.ascx','1',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_FUNCTION_TYPES -1,'HTML Page','Contains a single editable HTML page','Modules\System\Content\ContentControl.ascx','1',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_FUNCTION_TYPES -1,'Discuss','Contains a discussion area in which users can add posts','Modules\System\Content\Discussions.ascx','1',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode

	Print 'Adding navigation types'
	-- Setup ZFC_NAVIGATION_TYPES
	--exec ZFP_SET_NAVIGATION_TYPES -1,'Horizontal',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	--exec ZFP_SET_NAVIGATION_TYPES -1,'Vertical',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	--exec ZFP_SET_NAVIGATION_TYPES -1,'Hierarchical',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode

	--SET @V_NAV_TYPE_Hierarchical = (SELECT NAV_TYPE_SEQ_ID FROM ZFC_NAVIGATION_TYPES WHERE [DESCRIPTION] = 'Hierarchical')
	--SET @V_NAV_TYPE_Vertical = (SELECT NAV_TYPE_SEQ_ID FROM ZFC_NAVIGATION_TYPES WHERE [DESCRIPTION] = 'Vertical')
	--SET @V_NAV_TYPE_Horizontal = (SELECT NAV_TYPE_SEQ_ID FROM ZFC_NAVIGATION_TYPES WHERE [DESCRIPTION] = 'Horizontal')

	Print 'Adding permissions'
	-- Setup ZFC_PERMISSIONS
--	exec ZFP_SET_PERMISSIONS -1,'View',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY,@V_ErrorCode
--	exec ZFP_SET_PERMISSIONS -1,'Edit',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY,@V_ErrorCode
--	exec ZFP_SET_PERMISSIONS -1,'Add',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY,@V_ErrorCode
--	exec ZFP_SET_PERMISSIONS -1,'Delete',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY,@V_ErrorCode
--	set @V_ViewPermission = (select PERMISSIONS_SEQ_ID from ZFC_PERMISSIONS where DESCRIPTION='View')
--	set @V_AddPermission = (select PERMISSIONS_SEQ_ID from ZFC_PERMISSIONS where DESCRIPTION='Add')
--	set @V_EditPermission = (select PERMISSIONS_SEQ_ID from ZFC_PERMISSIONS where DESCRIPTION='Edit')
--	set @V_DeletePermission = (select PERMISSIONS_SEQ_ID from ZFC_PERMISSIONS where DESCRIPTION='Delete')

	Print 'Adding Security Entity'
	exec ZFP_SET_SECURITY_ENTITIES -1,	'System','The default Security Entity, needed by the system.','no url',1,'SQLServer','FoundationFramework','Foundation.Framework.SQLServer','server=(local);Integrated Security=SSPI;database=Foundation;connection reset=false;connection lifetime=5;enlist=true;min pool size=1;max pool size=50','Blue Arrow','Default',@V_ENCRYPTION_TYPE,-1,@V_PRIMARY_KEY,@V_ErrorCode
	SET @V_SE_SEQ_ID = (SELECT SE_SEQ_ID FROM ZFC_SECURITY_ENTITIES WHERE [NAME]='System')
	Print 'Adding roles'
	-- Setup ZF_RLS
	exec ZFP_SET_ROLE -1,'Anonymous','The anonymous role.',1,0,1,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY,@V_ErrorCode
	exec ZFP_SET_ROLE -1,'Authenticated','The authenticated role.',1,1,1,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY,@V_ErrorCode
	exec ZFP_SET_ROLE -1,'Developer','The developer role.',1,1,1,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY,@V_ErrorCode
	exec ZFP_SET_ROLE -1,'AlwaysLogon','Assign this role to allow logon when the system is under maintance.',1,0,1,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY,@V_ErrorCode

	Print 'Adding Groups'
	-- group id,group name,group description,Security Entity,added by,added date,updated by,updated date
	exec ZFP_SET_GROUP -1,'Everyone','Group representing both the authenticated and the anonymous roles.',1,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY,@V_ErrorCode
	SET @V_EVERYONE_ID = (SELECT GROUP_SEQ_ID FROM ZFC_SECURITY_GRPS WHERE [NAME]='Everyone')
	-- group id, Security Entity,comma sep roles,added by,ErrorCode
	EXEC ZFP_SET_GROUP_RLS @V_EVERYONE_ID,@V_SE_SEQ_ID,'Authenticated,Anonymous',@V_DeveloperID,NULL
	Print 'Adding account security'
	-- Setup the security
	-- Setup the account security
	exec ZFP_SET_ACCT_RLS 'Anonymous',1,'Anonymous',@V_DeveloperID,@V_ErrorCode
	exec ZFP_SET_ACCT_RLS 'Developer',1,'Developer,Authenticated,AlwaysLogon',@V_DeveloperID,@V_ErrorCode
	exec ZFP_SET_ACCT_RLS 'mike',1,'Authenticated',@V_DeveloperID,@V_ErrorCode

	Print 'Adding functions'
	-- Add functions

	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_EnableViewStateTrue = 1
	set @V_EnableViewStateFalse = 0
	set @V_IsNavTrue = 1
	set @V_IsNavFalse = 0

	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Menu Item')
	set @V_MyAction = 'mnuRootMenu'
	exec ZFP_SET_FUNCTION -1,'Root Menu','Place Holer',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'none',@V_EnableViewStateFalse,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,0,'mnuRootHolder',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	set @V_MyAction = 'GenericHome'
	exec ZFP_SET_FUNCTION -1,'Home','Generic Home',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Home\GenericHome.ascx',@V_EnableViewStateFalse,@V_IsNavTrue,@V_NAV_TYPE_Horizontal,'GenericHome',1,'Shown when not authenticated',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY,@V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Anonymous',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = @V_MyAction)

	set @V_MyAction = 'Home'
	exec ZFP_SET_FUNCTION -1,'Home','Home',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Home\Home.ascx',@V_EnableViewStateFalse,@V_IsNavTrue,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_ParentID,'Shown when authenticated',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	set @V_MyAction = 'Logon'
	exec ZFP_SET_FUNCTION -1,'Logon','Logon',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Accounts\Logon.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Vertical,@V_MyAction,@V_ParentID,'Loggs on an account',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Anonymous,Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	set @V_MyAction = 'Natural Sort'
	exec ZFP_SET_FUNCTION -1,'Natural Sort','Natural Sort',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\TestNaturalSort.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Vertical,@V_MyAction,@V_ParentID,'Shows natural sort order vs	ANSI',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	set @V_MyAction = 'Logoff'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Menu Item')
	exec ZFP_SET_FUNCTION -1,'Logoff','Logoff',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Accounts\Logoff.ascx',@V_EnableViewStateFalse,@V_IsNavTrue,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_ParentID,'Loggs off the system.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Menu Item')
	set @V_MyAction = 'mnuAdmin'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuRootMenu')
	exec ZFP_SET_FUNCTION -1,'Admin','Administration',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'none',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Administration tasks.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Menu Item')
	set @V_MyAction = 'mnuCalendars'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuRootMenu')
	exec ZFP_SET_FUNCTION -1,'Calendars','Calendars',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'none',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Access to the calendar.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Authenticated,Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Menu Item')
	set @V_MyAction = 'mnuReports'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuRootMenu')
	exec ZFP_SET_FUNCTION -1,'Reports','Reports',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'none',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Access to the reports.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Menu Item')
	set @V_MyAction = 'mnuMyProfile'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuRootMenu')
	exec ZFP_SET_FUNCTION -1,'My Profile','My Profile',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'none',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Access to profile information.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Authenticated,Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding System Administrator menu'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Menu Item')
	set @V_MyAction = 'mnuSystem Administration'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuRootMenu')
	exec ZFP_SET_FUNCTION -1,'SysAdmin','System Administration',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'none',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Serves as the root menu item for the hierarchical menus.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuRootMenu')
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Manage Functions'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Menu Item')
	set @V_MyAction = 'mnuManage Functions'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'Manage Functions','Manage Functions',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'none',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Menu item for functions.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Add Functions'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'AddFunctions'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Functions')
	exec ZFP_SET_FUNCTION -1,'Add Functions','Add Functions',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Functions\AddEditFunctions.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Adds a function to the system.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Copy Function Security'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Copy Function Security'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Functions')
	exec ZFP_SET_FUNCTION -1,'Copy Function Security','Copy Function Security',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Functions\CopyFunctionSecurity.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Adds a function to the system.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Manage Security Entitys'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Menu Item')
	set @V_MyAction = 'mnuManage Security Entitys'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'Manage Security Entitys','Manage Security Entitys',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'none',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Menu item for Security Entitys.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'File Management menu'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Menu Item')
	set @V_MyAction = 'mnuManageFiles'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'Manage Files','Manage Files',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'none',@V_EnableViewStateFalse,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to manage files.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode

	print 'cache directory management'
	-- Add module
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'File Manager')
	SET @V_MyAction = 'Manage Cachedependency'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManageFiles')
	EXEC ZFP_SET_FUNCTION -1,'Manage Cachedependency','Manage Cachedependency',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\FileManagement\FileManagerControl.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to manage the cache dependency direcory.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	-- Set security
	SET @V_FUNCTION_SEQ_ID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	EXEC ZFP_SET_FUNCTION_RLS @V_FUNCTION_SEQ_ID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	EXEC ZFP_SET_FUNCTION_RLS @V_FUNCTION_SEQ_ID,1,'Developer',@V_AddPermission,@V_DeveloperID, @V_ErrorCode
	EXEC ZFP_SET_FUNCTION_RLS @V_FUNCTION_SEQ_ID,1,'Developer',@V_EditPermission,@V_DeveloperID, @V_ErrorCode
	EXEC ZFP_SET_FUNCTION_RLS @V_FUNCTION_SEQ_ID,1,'Developer',@V_DeletePermission,@V_DeveloperID, @V_ErrorCode
	PRINT 'Adding cache directory'
	-- Add directory information
	SET @V_FUNCTION_SEQ_ID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = @V_MyAction)
	EXEC ZFP_SET_DIRECTORY @V_FUNCTION_SEQ_ID,'D:\WebProjects\2005\Foundation\FoundationProjects\FoundationWeb\CacheDependency',0,'','',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode

	PRINT 'cache directory management'
	-- Add module
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'File Manager')
	SET @V_MyAction = 'Manage Logs'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManageFiles')
	EXEC ZFP_SET_FUNCTION -1,'Manage Logs','Manage Logs',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\FileManagement\FileManagerControl.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to manage the logs direcory.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	SET @V_FUNCTION_SEQ_ID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	-- Set security
	EXEC ZFP_SET_FUNCTION_RLS @V_FUNCTION_SEQ_ID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	EXEC ZFP_SET_FUNCTION_RLS @V_FUNCTION_SEQ_ID,1,'Developer',@V_AddPermission,@V_DeveloperID, @V_ErrorCode
	EXEC ZFP_SET_FUNCTION_RLS @V_FUNCTION_SEQ_ID,1,'Developer',@V_EditPermission,@V_DeveloperID, @V_ErrorCode
	EXEC ZFP_SET_FUNCTION_RLS @V_FUNCTION_SEQ_ID,1,'Developer',@V_DeletePermission,@V_DeveloperID, @V_ErrorCode
	PRINT 'Adding log log'
	-- Add directory information
	SET @V_FUNCTION_SEQ_ID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = @V_MyAction)
	EXEC ZFP_SET_DIRECTORY @V_FUNCTION_SEQ_ID,'D:\WebProjects\2005\Foundation\FoundationProjects\FoundationWeb\Logs',0,'','',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode

	print 'Adding Manage Name/Value Pairs'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Menu Item')
	set @V_MyAction = 'mnuManage Name Value Pairs'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'Manage Name/Value Pairs','Manage Name/Value Pairs',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'none',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Menu item for name/value pairs.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Add Edit Groups'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Add Edit Groups'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuAdmin')
	exec ZFP_SET_FUNCTION -1,'Add Edit Groups','Add Edit Groups',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Groups\AddEditGroups.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to add or edit roles.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_AddPermission,@V_DeveloperID,@V_ErrorCode
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_EditPermission,@V_DeveloperID,@V_ErrorCode
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_DeletePermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Manage Messages'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Menu Item')
	set @V_MyAction = 'mnuManage Messages'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuAdmin')
	exec ZFP_SET_FUNCTION -1,'Manage Messages','Manage Messages',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'none',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Menu item for messages.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Manage States'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Menu Item')
	set @V_MyAction = 'mnuManage States'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'Manage States','Manage States',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'none',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Menu item for states.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Manage Work Flows'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Menu Item')
	set @V_MyAction = 'mnuWork Flows'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'Manage Work Flows','Manage Work Flows',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'none',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Menu item for work flows.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Encryption Helper'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'EncryptionHelper'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'Encryption Helper','Encryption Helper',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Encrypt\EncryptDecrypt.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Menu item for work flows.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding GUID Helper'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'GUIDHelper'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'GUID Helper','Displays''s a GUID',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Encrypt\GUIDHelper.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Displays a GUID may be necessary if you need to change the GUID in your project files.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Random Number'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Random Numbers'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'Random Numbers','Displays''s a set of randomly generated number''s',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Encrypt\RandomNumbers.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Displays''s a set of randomly generated number''s.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Set Log Level'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'SetLogLevel'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'Set Log Level','Set Log Level',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Logs\SetLogLevel.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to set the log level of the application ... Debug, Error, Warn, Fatal.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Update Anonymous Profile'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'UpdateAnonymousProfile'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'Update Anonymous Profile','Update Anonymous Profile',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\AnonymousAccount\UpdateAnonymousCache.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Remove any cached information for the anonymous account.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Search Functions'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'SearchFunctions'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Functions')
	exec ZFP_SET_FUNCTION -1,'Search Functions','Search Functions',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Functions\SearchFunctions.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Searches for functions in the system.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_DeletePermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Edit Functions'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'EditFunctions'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'Edit Functions','Edit Functions',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Functions\AddEditFunctions.ascx',@V_EnableViewStateTrue,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Edits a function in the system.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Function Security'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Function Security'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuReports')
	exec ZFP_SET_FUNCTION -1,'Function Security','Function Security',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Reports\FunctionSecurity.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Displays a report for function security.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Security By Role'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Security By Role'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuReports')
	exec ZFP_SET_FUNCTION -1,'Security By Role','Security By Role',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Reports\SecurityByRole.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Displays a report for security by role.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Change Password'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Change Password'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuMyProfile')
	exec ZFP_SET_FUNCTION -1,'Change Password','Change Password',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Accounts\ChangePassword.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to change an accounts password.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Change Colors'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Change Colors'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuMyProfile')
	exec ZFP_SET_FUNCTION -1,'Change Colors','Change Colors',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Accounts\ChangeColors.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to change an accounts color scheme.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Select Preferences'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Select Preferences'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuMyProfile')
	exec ZFP_SET_FUNCTION -1,'Select Preferences','Select Preferences',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Accounts\SelectPreferences.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to select preference for an account, records per page etc.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Edit Account'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Edit Account'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuMyProfile')
	exec ZFP_SET_FUNCTION -1,'Edit Account','Edit Account',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Accounts\AddEditAccount.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to edit an account profile.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Edit Other Account'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Edit Other Account'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuMyProfile')
	exec ZFP_SET_FUNCTION -1,'Edit Other Account','Edit Other Account',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Accounts\AddEditAccount.ascx',@V_EnableViewStateTrue,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to edit anothers account profile.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID,@V_ErrorCode

	print 'Adding Community Calendar'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Community Calendar'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuCalendars')
	exec ZFP_SET_FUNCTION -1,'Community Calendar','Community Calendar',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Calendar\CommunityCalendar.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to show calendar data.  Created as an example module.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode

	print 'Adding Add Account'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Add Account'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuAdmin')
	exec ZFP_SET_FUNCTION -1,'Add Account','Add Account',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Accounts\AddEditAccount.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to add an accounts password.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode

	print 'Adding Add Edit Roles'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Add Edit Roles'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuAdmin')
	exec ZFP_SET_FUNCTION -1,'Add Edit Roles','Add Edit Roles',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Roles\AddEditRoles.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to add or edit roles.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_AddPermission,@V_DeveloperID, @V_ErrorCode
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_EditPermission,@V_DeveloperID, @V_ErrorCode
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_DeletePermission,@V_DeveloperID, @V_ErrorCode


	print 'Adding Add Edit Name Value Pairs Details'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Add Edit List of values'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuAdmin')
	exec ZFP_SET_FUNCTION -1,'Add Edit List of Values','Add Edit List of Values',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\LOV\AddEditLOV.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to add or edit a list of value details.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode

	print 'Adding ViewAccountRoleTab'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Security')
	set @V_MyAction = 'ViewAccountRoleTab'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuAdmin')
	exec ZFP_SET_FUNCTION -1,'ViewAccountRoleTab','View Accounts Roles Tab',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'None',@V_EnableViewStateFalse,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used as a security holder for roles that can view the accounts role tab.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode

	print 'Adding ViewFunctionRoleTab'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Security')
	set @V_MyAction = 'ViewFunctionRoleTab'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuAdmin')
	exec ZFP_SET_FUNCTION -1,'ViewFunctionRoleTab','View Functions Roles Tab',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'None',@V_EnableViewStateFalse,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used as a security holder for roles that can view the functions role tab.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode

	print 'Adding ViewAccountGroupTab'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Security')
	set @V_MyAction = 'ViewAccountGroupTab'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuAdmin')
	exec ZFP_SET_FUNCTION -1,'ViewAccountGroupTab','View Accounts Groups Tab',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'None',@V_EnableViewStateFalse,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used as a security holder for groups that can view the accounts group tab.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode

	print 'Adding ViewFunctionGroupTab'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Security')
	set @V_MyAction = 'ViewFunctionGroupTab'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuAdmin')
	exec ZFP_SET_FUNCTION -1,'ViewFunctionGroupTab','View Function Groups Tab',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'None',@V_EnableViewStateFalse,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used as a security holder for groups that can view the functions group tab.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode

	print 'Adding Search Accounts'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Search Accounts'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuAdmin')
	exec ZFP_SET_FUNCTION -1,'Search Accounts','Search Accounts',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Accounts\SearchAccounts.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to select accounts for edit.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode

	print 'Adding Edit Role Members'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Edit Role Members'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuAdmin')
	exec ZFP_SET_FUNCTION -1,'Edit Role Members','Edit Role Members',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Roles\EditRoleMembers.ascx',@V_EnableViewStateTrue,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to add or remove members of a role.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_EditPermission,@V_DeveloperID, @V_ErrorCode

	print 'Adding Edit Group Members'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Edit Group Members'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuAdmin')
	exec ZFP_SET_FUNCTION -1,'Edit Group Members','Edit Group Members',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Groups\EditGroupMembers.ascx',@V_EnableViewStateTrue,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to add or remove members of a role.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode

	print 'Adding Navigation'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'VerticalMenu'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuAdmin')
	exec ZFP_SET_FUNCTION -1,'Navigation','Navigation',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Navigation\VerticalMenuUserControl.ascx',@V_EnableViewStateTrue,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Contains link items for the vertical menus.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode

	print 'Adding Not Avalible'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'NotAvalible'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuAdmin')
	exec ZFP_SET_FUNCTION -1,'Not Avalible','Not Avalible',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Errors\NotAvailable.ascx',@V_EnableViewStateTrue,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Error page when the action is not avalible.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	--AccessDenied
	print 'Adding Access Denied'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'AccessDenied'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuAdmin')
	exec ZFP_SET_FUNCTION -1,'Access Denied','Access Denied',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Errors\AccessDenied.ascx',@V_EnableViewStateTrue,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Error page when the account being used does not have sufficient access to the view permission.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	--Adding Error
	print 'Adding Error'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'DisplayError'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuAdmin')
	exec ZFP_SET_FUNCTION -1,'Display Error','Display Error',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Errors\DisplayError.ascx',@V_EnableViewStateTrue,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Error page when unknown or unexpected error occurs.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode

	--Select A Security Entity
	print 'Adding Select A Security Entity'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Select A Security Entity'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuAdmin')
	exec ZFP_SET_FUNCTION -1,'Select A Security Entity','Select A Security Entity',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\SecurityEntities\SelectSecurityEntity.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Vertical,@V_MyAction,@V_ParentID,'Used to select a Security Entity.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	-- Web configuration
	print 'Adding Web configuration'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Web Config'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'Web Config','Web Config',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Configuration\AddEditWebConfig.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Adds or edits web.config file settings.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	-- Line Count
	print 'Adding Line Count'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Line Count'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'Line Count','Line Count',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\LineCount.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Utility to count the lines of code.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	-- Add a Security Entity
	print 'Adding Add Security Entitys'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Add Security Entitys'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
	exec ZFP_SET_FUNCTION -1,'Add Security Entitys','Add Security Entitys',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\SecurityEntities\AddEditSecurityEntities.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to add a Security Entity.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	-- Edit a Security Entity
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Edit a Security Entity'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
	exec ZFP_SET_FUNCTION -1,'Edit a Security Entity','Edit a Security Entity',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\SecurityEntities\AddEditSecurityEntities.ascx',@V_EnableViewStateTrue,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to edit a Security Entity.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	-- Search Security Entitys
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Search Security Entitys'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
	exec ZFP_SET_FUNCTION -1,'Search Security Entitys','Search Security Entitys',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\SecurityEntities\SearchSecurityEntities.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to search a Security Entity.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode

	-- Add a Name Value Pair
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Add Name Value Pairs'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Name Value Pairs')
	exec ZFP_SET_FUNCTION -1,'Add Name Value Pairs','Add Security Entitys',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\NVP\AddEditNVP.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to add a name/value pair.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	-- Edit a Name Value Pair
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Edit Name Value Pairs'
	--SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
	exec ZFP_SET_FUNCTION -1,'Edit a Name Value Pair','Edit a Name Value Pair',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\NVP\AddEditNVP.ascx',@V_EnableViewStateTrue,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to edit a name/value pair.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	-- Search Name Value Pairs
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Search Name Value Pairs'
	--SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
	exec ZFP_SET_FUNCTION -1,'Search Name Value Pairs','Search Name Value Pairs',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\NVP\SearchNVP.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to search a name/value pair.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	-- Add a Message
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Add Message'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Messages')
	exec ZFP_SET_FUNCTION -1,'Add Message','Add Message',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Messages\AddEditMessage.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to add a message.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	-- Edit a Message
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Edit Message'
	--SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
	exec ZFP_SET_FUNCTION -1,'Edit a Message','Edit a Message',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Messages\AddEditMessage.ascx',@V_EnableViewStateTrue,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to edit a Message.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	-- Search Message
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Search Messages'
	--SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
	exec ZFP_SET_FUNCTION -1,'Search Messages','Search Messages',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Messages\SearchMessages.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to search a Message.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode

	-- Edit a State
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Edit State'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage States')
	exec ZFP_SET_FUNCTION -1,'Edit a State','Edit a State',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\States\AddEditStates.ascx',@V_EnableViewStateTrue,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to edit a State.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	-- Search State
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Search States'
	--SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
	exec ZFP_SET_FUNCTION -1,'Search States','Search States',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\States\SearchStates.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to search a State.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	-- Add a Workflows
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Add a Workflow'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuWork Flows')
	exec ZFP_SET_FUNCTION -1,'Add a Workflow','Add a Workflow',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Messages\AddEditMessage.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to add a Workflow.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	-- Edit a Workflows
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Edit a Workflow'
	--SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
	exec ZFP_SET_FUNCTION -1,'Edit a Workflow','Edit a Workflow',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Messages\AddEditMessage.ascx',@V_EnableViewStateTrue,@V_IsNavFalse,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to edit a Workflow.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	-- Search Workflows
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Search Workflows'
	--SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
	exec ZFP_SET_FUNCTION -1,'Search Workflows','Search Workflows',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Messages\SearchMessages.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to search a Workflows.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	-- Update Session
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Update'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'Update','Update',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Accounts\UpdateSession.ascx',@V_EnableViewStateFalse,@V_IsNavTrue,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_ParentID,'Used to update the session menus and roles.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Authenticated',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	-- Under Maintance
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'UnderMaintance'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'Under Maintance','Under Maintance',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\UnderMaintance.ascx',@V_EnableViewStateFalse,@V_IsNavFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_ParentID,'Used to update the session menus and roles.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Anonymous,Authenticated',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	print 'Adding AlwaysLogon'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Security')
	set @V_MyAction = 'AlwaysLogon'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'Always Logon','Always Logon',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'none',@V_EnableViewStateFalse,@V_IsNavFalse,@V_NAV_TYPE_Horizontal,@V_MyAction,@V_ParentID,'Used to update the session menus and roles.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'AlwaysLogon',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode
	print 'Adding Edit DB Information'
	SET @V_FUNCTION_TYPE = (SELECT FUNCTION_TYPE_SEQ_ID FROM ZFC_FUNCTION_TYPES WHERE [NAME] = 'Module')
	set @V_MyAction = 'Edit DB Information'
	SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuSystem Administration')
	exec ZFP_SET_FUNCTION -1,'Edit DB Information','Edit DB Information',@V_FUNCTION_TYPE,@V_ALLOW_HTML_INPUT,@V_ALLOW_COMMENT_HTML_INPUT,'Modules\System\Administration\Configuration\AddEditDBInformation.ascx',@V_EnableViewStateTrue,@V_IsNavTrue,@V_NAV_TYPE_Hierarchical,@V_MyAction,@V_ParentID,'Used to update the ZF_Information table, enable inheritence.',@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	set @V_FunctionID = (select function_seq_id from ZFC_FUNCTIONS where action=@V_MyAction)
	exec ZFP_SET_FUNCTION_RLS @V_FunctionID,1,'Developer',@V_ViewPermission,@V_DeveloperID, @V_ErrorCode


	PRINT 'Adding messages'
	exec ZFP_SET_MESSAGE -1,@V_SE_SEQ_ID,'Logon Error','Logon Error','Displayed when logon fails','<b>Invalid Account or Password!</b>',@V_FORMAT_AS_HTML_TRUE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_MESSAGE -1,@V_SE_SEQ_ID,'New Account','New Account','Message sent when an account is created.','Dear <FullName>,

	There has been a request for a new account: 

		Please Use this link to logon:
	 <Server>Default.aspx?Action=Logon&Account=<AccountName>&Password=<Password>

	<b>Please note once you have logged on using this link you will only be able to change our password.</b>',@V_FORMAT_AS_HTML_FALSE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode

	exec ZFP_SET_MESSAGE -1,@V_SE_SEQ_ID,'Request Password Reset UI','Request Password Reset UI','Displayed when new password is requested','<b>An EMail has been send to your account with instructions!</b>',@V_FORMAT_AS_HTML_TRUE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_MESSAGE -1,@V_SE_SEQ_ID,'RequestNewPassword','Request New Password','Request New Password','Dear <FullName>,

	There has been a request for a password change: 

		Please Use this link to logon:
	 <Server>Default.aspx?Action=Logon&Account=<AccountName>&Password=<Password>

	<b>Please note once you have logged on using this link you will only be able to change our password.</b>',@V_FORMAT_AS_HTML_TRUE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_MESSAGE -1,1,'WebConfigNotSaved','Blank Environment Text','Blank Environment Text','Settings have not been saved.',@V_FORMAT_AS_HTML_FALSE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_MESSAGE -1,1,'WebConfigIsLocked','Web Config Is Locked','Web Config Is Locked','Configuration Section is locked. Unable to modify.',@V_FORMAT_AS_HTML_FALSE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_MESSAGE -1,1,'WebConfigEnvironmentRequired','Web Config Environment Required','Web Config Environment Required','You have selected a new environment but did not give the name.',@V_FORMAT_AS_HTML_FALSE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_MESSAGE -1,1,'ErrorAccountDetails','Error Account Details','Error Account Details','Could not set account details.',@V_FORMAT_AS_HTML_FALSE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_MESSAGE -1,1,'PasswordSendMailError','Password Send Mail Error','Password Send Mail Error','The password was reset, but, an email could not be sent.',@V_FORMAT_AS_HTML_FALSE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_MESSAGE -1,1,'DisabledAccount','Disabled Account','Disabled Account','This account is disabled.',@V_FORMAT_AS_HTML_FALSE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_MESSAGE -1,1,'SuccessChangePassword','Success Change Password','Success Change Password','Your password has been changed.',@V_FORMAT_AS_HTML_FALSE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_MESSAGE -1,1,'UnSuccessChangePassword','UnSuccess Change Password','UnSuccess ChangePassword','Your password has NOT been changed.',@V_FORMAT_AS_HTML_FALSE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_MESSAGE -1,1,'PasswordNotMatched','Password Not Matched','Password Not Matched','The OLD password did not match your current password.',@V_FORMAT_AS_HTML_FALSE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_MESSAGE -1,1,'UnderMaintance','Under Maintance','Under Maintance','The system is currently under maintance and logons have been limited.',@V_FORMAT_AS_HTML_FALSE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_MESSAGE -1,1,'UnderConstruction','Under Construction','Under Construction','The system is currently under construction.',@V_FORMAT_AS_HTML_FALSE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_MESSAGE -1,1,'NoDataFound','No Data Found','No Data Found','No Data Found.',@V_FORMAT_AS_HTML_FALSE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_MESSAGE -1,1,'ChangedSelectedSecurityEntity','Changed Selected Security Entity','Message for when a account changes the selected Security Entity.','You have changed your selected Security Entity.',@V_FORMAT_AS_HTML_FALSE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode
	exec ZFP_SET_MESSAGE -1,1,'SameAccountChangeAccount','Same Account Change Account','Message for when a account changes their own account.','showMSG("If you change your account the system will need to log you off.")',@V_FORMAT_AS_HTML_FALSE,@V_DeveloperID,@now,@V_DeveloperID,@now,@V_PRIMARY_KEY, @V_ErrorCode

	-- Insert States
	Print 'Adding States'
	DELETE from ZOP_STATES
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('AA','Armed Forces Americas',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('AE','Armed Forces Africa',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('AK','Alaska',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('AL','Alabama',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('AP','Armed Forces Pacific',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('AR','Arkansas',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('AS','American Samoa',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('AZ','Arizona',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('CA','California',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('CO','Colorado',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('CT','Connecticut',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('DC','District Of Columbia',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('DE','Delaware',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('FL','Florida',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('FM','Federated States of Micro',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('GA','Georgia',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('GU','Gaum',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('HI','Hawaii',@V_ACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('IA','Iowa',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('ID','Idaho',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('IL','Illinois',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('IN','Indiana',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('KS','Kansas',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('KY','Kentucky',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('LA','Louisiana',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('MA','Massachusetts',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('MD','Maryland',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('ME','Maine',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('MH','Marshall Islands',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('MI','Michigan',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('MN','Minnesota',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('MO','Missouri',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('MP','Northern Mariana Islands',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('MS','Mississippi',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('MT','Montana',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('NC','North Carolina',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('ND','North Dakota',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('NE','Nebraska',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('NH','New Hampshire',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('NJ','New Jersey',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('NM','New Mexico',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('NV','Nevada',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('NY','New York',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('OH','Ohio',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('OK','Oklahoma',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('OR','Oregon',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('PA','Pennsylvania',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('PR','Puerto Rico',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('PW','Palau',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('RI','Rhode Island',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('SC','South Carolina',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('SD','South Dakota',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('TN','Tennessee',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('TX','Texas',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('UT','Utah',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('VA','Virginia',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('VI','Virgin Islands',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('VT','Vermont',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('WA','Washington',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('WI','Wisconsin',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('WV','West Virginia',@V_INACTIVE)
	insert ZOP_STATES([STATE],[DESCRIPTION],STATUS_SEQ_ID) values('WY','Wyoming',@V_INACTIVE)
	update statistics ZOP_STATES

End

-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR