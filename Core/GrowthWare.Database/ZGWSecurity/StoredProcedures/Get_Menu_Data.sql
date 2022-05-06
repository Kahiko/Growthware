
/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId AS INT = 1,
		@P_Navigation_Types_NVP_DetailSeqId AS INT = 3,
		@P_Account VARCHAR(128) = 'Developer',
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Menu_Data
		@PSecurityEntitySeqId,
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
CREATE PROCEDURE [ZGWSecurity].[Get_Menu_Data]
	@PSecurityEntitySeqId INT,
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
			AND [SECURITY].Roles_Security_EntitiesSeqId = SE_ROLES.Roles_Security_EntitiesSeqId
			AND [SECURITY].FunctionSeqId = [FUNCTIONS].FunctionSeqId
			AND [Permissions].NVP_DetailSeqId = SECURITY.Permissions_NVP_DetailSeqId
			AND [Permissions].NVP_DetailSeqId = @V_Permission_Id
			AND [FUNCTIONS].Navigation_Types_NVP_DetailSeqId = @P_Navigation_Types_NVP_DetailSeqId
			AND [FUNCTIONS].Is_Nav = 1
			AND SE_ROLES.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))
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
			AND ZGWSecurity.Groups_Security_Entities.Groups_Security_EntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Functions.Groups_Security_EntitiesSeqId
			AND ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Groups_Security_EntitiesSeqId = ZGWSecurity.Groups_Security_Entities.Groups_Security_EntitiesSeqId
			AND ZGWSecurity.Roles_Security_Entities.Roles_Security_EntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Roles_Security_EntitiesSeqId
			AND ROLES.RoleSeqId = ZGWSecurity.Roles_Security_Entities.RoleSeqId
			AND [Permissions].NVP_DetailSeqId = ZGWSecurity.Groups_Security_Entities_Functions.Permissions_NVP_DetailSeqId
			AND [Permissions].NVP_DetailSeqId = @V_Permission_Id
			AND [FUNCTIONS].Navigation_Types_NVP_DetailSeqId = @P_Navigation_Types_NVP_DetailSeqId
			AND [FUNCTIONS].Is_Nav = 1
			AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))

	--SELECT * FROM @V_AvalibleMenuItems -- DEBUG

	DECLARE @V_AccountRoles TABLE (Roles VARCHAR(30)) -- Roles belonging to the account
	INSERT INTO @V_AccountRoles
		EXEC ZGWSecurity.Get_Account_Security @P_Account, @PSecurityEntitySeqId, @P_Debug

	--SELECT * FROM @V_AccountRoles -- DEBUG
	DECLARE @V_AllMenuItems TABLE ([ID] INT, Title VARCHAR(30), [Description] VARCHAR(256), URL VARCHAR(256), Parent INT, Sort_Order INT, ROLE VARCHAR(50),FunctionTypeSeqId INT)
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
			ROLE IN (SELECT DISTINCT Roles FROM @V_AccountRoles)

	DECLARE @V_DistinctItems TABLE ([ID] INT, TITLE VARCHAR(30), [Description] VARCHAR(256), URL VARCHAR(256), Parent INT, Sort_Order INT, FunctionTypeSeqId INT)
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
	IF EXISTS (SELECT TOP(1) 1 FROM @V_DistinctItems WHERE [TITLE] = 'Favorite')
		BEGIN
			DECLARE @V_FavoriteAction VARCHAR(256)
			SET @V_FavoriteAction = (SELECT [FavoriteAction] FROM [ZGWCoreWeb].[Account_Choices] WHERE [Account] = @P_Account);
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

