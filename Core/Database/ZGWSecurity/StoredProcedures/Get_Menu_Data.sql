
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
-- Author:		Michael Regan
-- Create date: 10/01/2023
-- Description:	Added Link_Behavior
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Menu_Data] @P_SecurityEntitySeqId INT
	,@P_Navigation_Types_NVP_DetailSeqId INT
	,@P_Account VARCHAR(128)
	,@P_Debug INT = 1
AS
SET NOCOUNT ON

DECLARE @V_Permission_Id INT

SET @V_Permission_Id = ZGWSecurity.Get_View_PermissionSeqId()

DECLARE @V_AvalibleItems TABLE (
	 [ID] INT
	,[Title] VARCHAR(30)
	,[Description] VARCHAR(256)
	,[URL] VARCHAR(256)
	,[Parent_Id] INT
	,[Sort_Order] INT
	,[Role] VARCHAR(50)
	,[FunctionTypeSeqId] INT
	,[Link_Behavior] INT
);

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
	,[FUNCTIONS].Link_Behavior
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
	,[FUNCTIONS].Link_Behavior
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
	);

--SELECT * FROM @V_AvalibleMenuItems -- DEBUG
DECLARE @V_AccountRoles TABLE (Roles VARCHAR(30)) -- Roles belonging to the account

INSERT INTO @V_AccountRoles
EXEC ZGWSecurity.Get_Account_Security @P_Account
	,@P_SecurityEntitySeqId
	,@P_Debug

--SELECT * FROM @V_AccountRoles -- DEBUG
DECLARE @V_AllMenuItems TABLE (
	 [ID] INT
	,[Title] VARCHAR(30)
	,[Description] VARCHAR(256)
	,[URL] VARCHAR(256)
	,[Parent_Id] INT
	,[Sort_Order] INT
	,[ROLE] VARCHAR(50)
	,[FunctionTypeSeqId] INT
	,[Link_Behavior] INT
);

INSERT INTO @V_AllMenuItems
SELECT -- Last but not least get the menu items when there are matching account roles.
	 [ID]
	,[Title]
	,[Description]
	,[URL]
	,[Parent_Id]
	,[Sort_Order]
	,[Role]
	,[FunctionTypeSeqId]
	,[Link_Behavior]
FROM @V_AvalibleItems
WHERE ROLE IN (
		SELECT DISTINCT Roles
		FROM @V_AccountRoles
	);

DECLARE @V_DistinctItems TABLE (
	 [ID] INT
	,[TITLE] VARCHAR(30)
	,[Description] VARCHAR(256)
	,[URL] VARCHAR(256)
	,[Parent_Id] INT
	,[Sort_Order] INT
	,[FunctionTypeSeqId] INT
	,[Link_Behavior] INT
);

INSERT INTO @V_DistinctItems
SELECT DISTINCT [ID]
	,Title
	,[Description]
	,URL
	,Parent_Id
	,Sort_Order
	,FunctionTypeSeqId
	,[Link_Behavior]
FROM @V_AllMenuItems

IF EXISTS (SELECT TOP (1) 1 FROM @V_DistinctItems WHERE [TITLE] = 'Favorite')
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
	,[Title] AS Title
	,[Description]
	,[URL]
	,[Parent_Id] AS [ParentId]
	,[Sort_Order]
	,[FunctionTypeSeqId] AS [Function_Type_Seq_ID]
	,[Link_Behavior] AS [LinkBehavior]
FROM @V_DistinctItems
ORDER BY Parent_Id
	,Sort_Order
	,Title
	,ID

RETURN 0

GO

