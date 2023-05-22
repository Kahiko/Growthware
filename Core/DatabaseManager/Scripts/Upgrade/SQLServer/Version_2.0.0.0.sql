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
IF NOT EXISTS (SELECT TOP(1) 1 FROM [ZGWSecurity].[Functions] WHERE [Action] = 'SaveAccount')
	BEGIN
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
	END
--END IF

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('ZGWSecurity.Get_Menu_Data'))
   exec('CREATE PROCEDURE [ZGWSecurity].[Get_Menu_Data] AS BEGIN SET NOCOUNT ON; END')
GO

/*
Usage:
	DECLARE 
		@P_SecurityEntitySeqId AS INT = 1,
		@P_Navigation_Types_NVP_DetailSeqId AS INT = 3,
		@P_Account VARCHAR(128) = 'Developer',
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Menu_Data
		@P_SecurityEntitySeqId,
		@P_Navigation_Types_NVP_DetailSeqId,
		@P_Account,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description:	Retrieves menu data given the
--	Account, Security Entity ID and the Navigation type.
-- =============================================
-- Author:		Michael Regan
-- Create date: 05/15/2023
-- Description:	Changed returned columns
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Menu_Data] @P_SecurityEntitySeqId INT
	,@P_Navigation_Types_NVP_DetailSeqId INT
	,@P_Account VARCHAR(128)
	,@P_Debug INT = 1
AS
SET NOCOUNT ON

DECLARE @V_Permission_Id INT

SET @V_Permission_Id = ZGWSecurity.Get_View_PermissionSeqId()

DECLARE @V_AvalibleItems TABLE (
	[ID] INT
	,Title VARCHAR(30)
	,[Description] VARCHAR(256)
	,URL VARCHAR(256)
	,Parent_Id INT
	,Sort_Order INT
	,[Role] VARCHAR(50)
	,FunctionTypeSeqId INT
	)

INSERT INTO @V_AvalibleItems
	SELECT -- Menu items via roles
		[FUNCTIONS].FunctionSeqId AS [ID]
		,[FUNCTIONS].[Name] AS Title
		,[FUNCTIONS].[Description]
		,[FUNCTIONS].[Action] AS URL
		,[FUNCTIONS].ParentSeqId AS Parent_Id
		,[FUNCTIONS].Sort_Order AS Sort_Order
		,ROLES.[Name] AS ROLE
		,[FUNCTIONS].FunctionTypeSeqId
	FROM ZGWSecurity.Roles_Security_Entities SE_ROLES WITH (NOLOCK)
		,ZGWSecurity.Roles ROLES WITH (NOLOCK)
		,ZGWSecurity.Roles_Security_Entities_Functions [SECURITY] WITH (NOLOCK)
		,ZGWSecurity.Functions [FUNCTIONS] WITH (NOLOCK)
		,ZGWSecurity.[Permissions] [Permissions] WITH (NOLOCK)
	WHERE SE_ROLES.RoleSeqId = ROLES.RoleSeqId
		AND [SECURITY].RolesSecurityEntitiesSeqId = SE_ROLES.RolesSecurityEntitiesSeqId
		AND [SECURITY].FunctionSeqId = [FUNCTIONS].FunctionSeqId
		AND [Permissions].NVP_DetailSeqId = SECURITY.PermissionsNVPDetailSeqId
		AND [Permissions].NVP_DetailSeqId = @V_Permission_Id
		AND [FUNCTIONS].Navigation_Types_NVP_DetailSeqId = @P_Navigation_Types_NVP_DetailSeqId
		AND [FUNCTIONS].Is_Nav = 1
		AND SE_ROLES.SecurityEntitySeqId IN (
			SELECT SecurityEntitySeqId
			FROM ZGWSecurity.Get_Entity_Parents(1, @P_SecurityEntitySeqId)
		)

UNION ALL
	SELECT -- Menu items via groups
		[FUNCTIONS].FunctionSeqId AS [ID]
		,[FUNCTIONS].[Name] AS Title
		,[FUNCTIONS].[Description]
		,[FUNCTIONS].[Action] AS URL
		,[FUNCTIONS].ParentSeqId AS Parent_Id
		,[FUNCTIONS].Sort_Order AS Sort_Order
		,ROLES.[Name] AS ROLE
		,[FUNCTIONS].FunctionTypeSeqId
	FROM ZGWSecurity.Groups_Security_Entities_Functions WITH (NOLOCK)
		,ZGWSecurity.Groups_Security_Entities WITH (NOLOCK)
		,ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities WITH (NOLOCK)
		,ZGWSecurity.Roles_Security_Entities WITH (NOLOCK)
		,ZGWSecurity.Roles ROLES WITH (NOLOCK)
		,ZGWSecurity.Functions [FUNCTIONS] WITH (NOLOCK)
		,ZGWSecurity.[Permissions] [Permissions] WITH (NOLOCK)
	WHERE ZGWSecurity.Groups_Security_Entities_Functions.FunctionSeqId = [FUNCTIONS].FunctionSeqId
		AND ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Functions.GroupsSecurityEntitiesSeqId
		AND ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId
		AND ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.RolesSecurityEntitiesSeqId
		AND ROLES.RoleSeqId = ZGWSecurity.Roles_Security_Entities.RoleSeqId
		AND [Permissions].NVP_DetailSeqId = ZGWSecurity.Groups_Security_Entities_Functions.PermissionsNVPDetailSeqId
		AND [Permissions].NVP_DetailSeqId = @V_Permission_Id
		AND [FUNCTIONS].Navigation_Types_NVP_DetailSeqId = @P_Navigation_Types_NVP_DetailSeqId
		AND [FUNCTIONS].Is_Nav = 1
		AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (
			SELECT SecurityEntitySeqId
			FROM ZGWSecurity.Get_Entity_Parents(1, @P_SecurityEntitySeqId)
		)

--SELECT * FROM @V_AvalibleMenuItems -- DEBUG
DECLARE @V_AccountRoles TABLE (Roles VARCHAR(30)) -- Roles belonging to the account

INSERT INTO @V_AccountRoles
EXEC ZGWSecurity.Get_Account_Security @P_Account
	,@P_SecurityEntitySeqId
	,@P_Debug

--SELECT * FROM @V_AccountRoles -- DEBUG
DECLARE @V_AllMenuItems TABLE (
	[ID] INT
	,Title VARCHAR(30)
	,[Description] VARCHAR(256)
	,URL VARCHAR(256)
	,Parent_Id INT
	,Sort_Order INT
	,ROLE VARCHAR(50)
	,FunctionTypeSeqId INT
	)

INSERT INTO @V_AllMenuItems
SELECT -- Last but not least get the menu items when there are matching account roles.
	[ID]
	,Title
	,[Description]
	,URL
	,Parent_Id
	,Sort_Order
	,[Role]
	,FunctionTypeSeqId
FROM @V_AvalibleItems
WHERE ROLE IN (
		SELECT DISTINCT Roles
		FROM @V_AccountRoles
		)

DECLARE @V_DistinctItems TABLE (
	[ID] INT
	,TITLE VARCHAR(30)
	,[Description] VARCHAR(256)
	,URL VARCHAR(256)
	,Parent_Id INT
	,Sort_Order INT
	,FunctionTypeSeqId INT
	)

INSERT INTO @V_DistinctItems
SELECT DISTINCT [ID]
	,Title
	,[Description]
	,URL
	,Parent_Id
	,Sort_Order
	,FunctionTypeSeqId
FROM @V_AllMenuItems

IF EXISTS (
		SELECT TOP (1) 1
		FROM @V_DistinctItems
		WHERE [TITLE] = 'Favorite'
		)
BEGIN
	DECLARE @V_FavoriteAction VARCHAR(256)

	SET @V_FavoriteAction = (
			SELECT [FavoriteAction]
			FROM [ZGWCoreWeb].[Account_Choices]
			WHERE [Account] = @P_Account
			);

	IF @V_FavoriteAction IS NOT NULL
	BEGIN
		UPDATE @V_DistinctItems
		SET [URL] = @V_FavoriteAction
		WHERE [TITLE] = 'Favorite';
	END
			--END IF
END

--END IF
SELECT 
	 [Id]
	,[Title]
	,[Description]
	,[URL] = LOWER([URL])
	,[ParentId] = Parent_Id
	,Sort_Order
	,FunctionTypeSeqId AS Function_Type_Seq_ID
FROM @V_DistinctItems
ORDER BY Parent_Id
	,Sort_Order
	,Title
	,ID

RETURN 0
GO

-- Update ZGWSecurity.Functions data
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'accounts' WHERE [Action] = 'Search_Accounts';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'functions' WHERE [Action] = 'Search_Functions';
UPDATE [ZGWSecurity].[Functions] SET [Action] = '/accounts/Edit-My-Account' WHERE [Action] = 'EditAccount';
UPDATE [ZGWSecurity].[Functions] SET [Action] = '/accounts/change-password' WHERE [Action] = 'changepassword';
UPDATE [ZGWSecurity].[Functions] SET [Action] = '/security/random-numbers' WHERE [Action] = 'RandomNumbers';
UPDATE [ZGWSecurity].[Functions] SET [Action] = '/security/guid_helper' WHERE [Action] = 'guidhelper';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'security' WHERE [Action] = 'Encryption_Helper';

-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '2.0.0.0',
    [Updated_By] = 3,
    [Updated_Date] = getdate()
WHERE [Version] = '1.0.0.0'
