-- Upgrade
-- 0 = FALSE 1 = TRUE
DECLARE @V_Action VARCHAR(256)          = ''
    , @V_Debug int                      = 0
	, @V_Description VARCHAR(512)		= ''
	, @V_Controller VARCHAR(512)		= ''
	, @V_EnableNotificationsFalse INT	= 0
	, @V_EnableViewStateFalse INT		= 0
    , @V_FunctionID INT                 = -1
    , @V_FunctionTypeSeqId INT			= (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Security')
	, @V_IsNavFalse INT					= 0
	, @V_LinkBehaviorInternal INT		= (SELECT [NVP_DetailSeqId] FROM [ZGWCoreWeb].[Link_Behaviors] WHERE [NVP_Detail_Value] = 'Internal')
	, @V_META_KEY_WORDS VARCHAR(512)	= ''
	, @V_Name VARCHAR(30)				= ''
	, @V_NAV_TYPE INT					= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Navigation_Types WHERE NVP_Detail_Value = 'Hierarchical')
	, @V_Notes VARCHAR(512)				= ''
	, @V_NO_UI INT						= 1
    , @V_ParentID INT					= NULL
	, @V_Redirect_On_Timeout INT		= 0
	, @V_Resolve VARCHAR(MAX)			= NULL
	, @V_Source VARCHAR(512)			= 'None'
    , @V_SystemID INT					= (SELECT AccountSeqId from ZGWSecurity.Accounts where Account = 'System')
    , @V_Permission_Add INT				= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Add')
    , @V_Permission_Delete INT			= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Delete')
    , @V_Permission_Edit INT			= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Edit')
    , @V_Permission_View INT			= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'View')

-- Setup the values for the new/updated Function
PRINT 'Adding SaveAccount'
SET @V_FunctionID = -1;
SET @V_Description = 'A security entry to determine who can Save and Account (not their own)';
SET @V_Action = 'SaveAccount';
SET @V_Name = 'Save Account';
SET @V_Notes = 'Used as a security holder to determine who can Save and Account (not their own).';
EXEC ZGWSecurity.Set_Function @V_FunctionID, @V_Name, @V_Description, @V_FunctionTypeSeqId, @V_Source, @V_Controller, @V_Resolve, @V_EnableViewStateFalse, @V_EnableNotificationsFalse, @V_Redirect_On_Timeout, @V_IsNavFalse, @V_LinkBehaviorInternal, @V_NO_UI, @V_NAV_TYPE, @V_Action, @V_META_KEY_WORDS, @V_ParentID, @V_Notes, @V_SystemID, @V_Debug;
-- Set the Security for the new/updated Function
SET @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_Action)
EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID, 1, 'Developer', @V_Permission_Add, @V_SystemID, @V_Debug
EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID, 1, 'Developer', @V_Permission_Edit, @V_SystemID, @V_Debug

-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '1.0.0.1',
    [Updated_By] = 3,
    [Updated_Date] = getdate()
WHERE [Version] = '1.0.0.0'
