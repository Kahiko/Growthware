-- Downgrade

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
ALTER PROCEDURE [ZGWSecurity].[Get_Menu_Data]
	@P_SecurityEntitySeqId INT,
	@P_Navigation_Types_NVP_DetailSeqId INT,
	@P_Account VARCHAR(128),
	@P_Debug INT = 1
AS
	SET NOCOUNT ON
	DECLARE @V_Permission_Id INT
	SET @V_Permission_Id = ZGWSecurity.Get_View_PermissionSeqId()
	DECLARE @V_AvalibleItems TABLE ([ID] INT,
	Title VARCHAR(30),
	[Description] VARCHAR(256),
	URL VARCHAR(256),
	Parent INT,
	Sort_Order INT,
	[Role] VARCHAR(50),
	FunctionTypeSeqId INT)
	INSERT INTO @V_AvalibleItems
	SELECT -- Menu items via roles
		[FUNCTIONS].FunctionSeqId AS [ID],
		[FUNCTIONS].[Name] AS Title,
		[FUNCTIONS].[Description],
		[FUNCTIONS].[Action] AS URL,
		[FUNCTIONS].ParentSeqId AS Parent,
		[FUNCTIONS].Sort_Order AS Sort_Order,
		ROLES.[Name] AS ROLE,
		[FUNCTIONS].FunctionTypeSeqId
	FROM
		ZGWSecurity.Roles_Security_Entities SE_ROLES WITH(NOLOCK),
		ZGWSecurity.Roles ROLES WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities_Functions [SECURITY] WITH(NOLOCK),
		ZGWSecurity.Functions [FUNCTIONS] WITH(NOLOCK),
		ZGWSecurity.[Permissions] [Permissions] WITH(NOLOCK)
	WHERE
			SE_ROLES.RoleSeqId = ROLES.RoleSeqId
		AND [SECURITY].RolesSecurityEntitiesSeqId = SE_ROLES.RolesSecurityEntitiesSeqId
		AND [SECURITY].FunctionSeqId = [FUNCTIONS].FunctionSeqId
		AND [Permissions].NVP_DetailSeqId = SECURITY.PermissionsNVPDetailSeqId
		AND [Permissions].NVP_DetailSeqId = @V_Permission_Id
		AND [FUNCTIONS].Navigation_Types_NVP_DetailSeqId = @P_Navigation_Types_NVP_DetailSeqId
		AND [FUNCTIONS].Is_Nav = 1
		AND SE_ROLES.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
		FROM ZGWSecurity.Get_Entity_Parents(1,@P_SecurityEntitySeqId))
UNION ALL
	SELECT -- Menu items via groups
		[FUNCTIONS].FunctionSeqId AS [ID],
		[FUNCTIONS].[Name] AS Title,
		[FUNCTIONS].[Description],
		[FUNCTIONS].[Action] AS URL,
		[FUNCTIONS].ParentSeqId AS Parent,
		[FUNCTIONS].Sort_Order AS Sort_Order,
		ROLES.[Name] AS ROLE,
		[FUNCTIONS].FunctionTypeSeqId
	FROM
		ZGWSecurity.Groups_Security_Entities_Functions WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles ROLES WITH(NOLOCK),
		ZGWSecurity.Functions [FUNCTIONS] WITH(NOLOCK),
		ZGWSecurity.[Permissions] [Permissions] WITH(NOLOCK)
	WHERE
			ZGWSecurity.Groups_Security_Entities_Functions.FunctionSeqId = [FUNCTIONS].FunctionSeqId
		AND ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Functions.GroupsSecurityEntitiesSeqId
		AND ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId
		AND ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.RolesSecurityEntitiesSeqId
		AND ROLES.RoleSeqId = ZGWSecurity.Roles_Security_Entities.RoleSeqId
		AND [Permissions].NVP_DetailSeqId = ZGWSecurity.Groups_Security_Entities_Functions.PermissionsNVPDetailSeqId
		AND [Permissions].NVP_DetailSeqId = @V_Permission_Id
		AND [FUNCTIONS].Navigation_Types_NVP_DetailSeqId = @P_Navigation_Types_NVP_DetailSeqId
		AND [FUNCTIONS].Is_Nav = 1
		AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
		FROM ZGWSecurity.Get_Entity_Parents(1,@P_SecurityEntitySeqId))

	--SELECT * FROM @V_AvalibleMenuItems -- DEBUG

	DECLARE @V_AccountRoles TABLE (Roles VARCHAR(30)) -- Roles belonging to the account
	INSERT INTO @V_AccountRoles
EXEC ZGWSecurity.Get_Account_Security @P_Account, @P_SecurityEntitySeqId, @P_Debug

	--SELECT * FROM @V_AccountRoles -- DEBUG
	DECLARE @V_AllMenuItems TABLE ([ID] INT,
	Title VARCHAR(30),
	[Description] VARCHAR(256),
	URL VARCHAR(256),
	Parent INT,
	Sort_Order INT,
	ROLE VARCHAR(50),
	FunctionTypeSeqId INT)
	INSERT INTO @V_AllMenuItems
SELECT -- Last but not least get the menu items when there are matching account roles.
	[ID],
	Title,
	[Description],
	URL,
	Parent,
	Sort_Order,
	[Role],
	FunctionTypeSeqId
FROM
	@V_AvalibleItems
WHERE
			ROLE IN (SELECT DISTINCT Roles
FROM @V_AccountRoles)

	DECLARE @V_DistinctItems TABLE ([ID] INT,
	TITLE VARCHAR(30),
	[Description] VARCHAR(256),
	URL VARCHAR(256),
	Parent INT,
	Sort_Order INT,
	FunctionTypeSeqId INT)
	INSERT INTO @V_DistinctItems
SELECT DISTINCT
	[ID],
	Title,
	[Description],
	URL,
	Parent,
	Sort_Order,
	FunctionTypeSeqId
FROM
	@V_AllMenuItems
	IF EXISTS (SELECT TOP(1)
	1
FROM @V_DistinctItems
WHERE [TITLE] = 'Favorite')
		BEGIN
	DECLARE @V_FavoriteAction VARCHAR(256)
	SET @V_FavoriteAction = (SELECT [FavoriteAction]
	FROM [ZGWCoreWeb].[Account_Choices]
	WHERE [Account] = @P_Account);
	IF @V_FavoriteAction IS NOT NULL
				BEGIN
		UPDATE @V_DistinctItems SET [URL] = @V_FavoriteAction WHERE [TITLE] = 'Favorite';
	END
--END IF
END
	--END IF

	SELECT
	ID as MenuID,
	TITLE AS Title,
	[Description],
	URL,
	Parent as ParentID,
	Sort_Order,
	FunctionTypeSeqId as Function_Type_Seq_ID
FROM
	@V_DistinctItems
ORDER BY
		Parent,
		Sort_Order,
		Title,
		ID


RETURN 0

GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [ZGWSecurity].[vwSearchGroups] AS
	SELECT
		G.[GroupSeqId] AS Group_SEQ_ID,
		G.[Name],
		G.[Description],
		Added_By = (SELECT Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = G.Added_By),
		Added_Date = (SELECT Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = G.Added_Date),
		G.[Updated_By],
		G.[Updated_Date],
		RSE.SecurityEntitySeqId
	FROM
		ZGWSecurity.Groups G WITH(NOLOCK)
		INNER JOIN ZGWSecurity.Groups_Security_Entities RSE WITH(NOLOCK)
			ON G.GroupSeqId = RSE.GroupSeqId
GO
-- Update ZGWSecurity.Functions data
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'Search_Accounts' WHERE [Action] = 'accounts';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'Search_Functions' WHERE [Action] = 'functions';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'CopyFunctionSecurity' WHERE [Action] = '/functions/copyfunctionsecurity';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'EditAccount' WHERE [Action] = '/accounts/Edit-My-Account';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'ChangePassword' WHERE [Action] = '/accounts/change-password';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'SelectPreferences' WHERE [Action] = '/accounts/selectpreferences';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'UpdateAnonymousProfile' WHERE [Action] = '/accounts/updateanonymousprofile';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'RandomNumbers' WHERE [Action] = '/security/random-numbers';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'guidhelper' WHERE [Action] = '/security/guid_helper';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'Encryption_Helper' WHERE [Action] = 'security';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'Search_Security_Entities' WHERE [Action] = 'search_security_entities';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'LineCount' WHERE [Action] = '/sys_admin/linecount';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'EditDBInformation' WHERE [Action] = '/sys_admin/editdbinformation';

DELETE FROM [ZGWSecurity].[Functions] WHERE [Action] = 'SaveAccount'

UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '1.0.0.0',
    [Updated_By] = null,
    [Updated_Date] = null
WHERE [Version] = '2.0.0.0'