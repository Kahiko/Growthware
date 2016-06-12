/*
Usage:
	DECLARE 
		@P_Security_Entity_SeqID AS INT,
		@P_Navigation_Types_NVP_Detail_SeqID AS INT,
		@P_Account VARCHAR(128) = 'Developer',
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Menu_Data
		@P_Security_Entity_SeqID,
		@P_Navigation_Types_NVP_Detail_SeqID,
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
	@P_Security_Entity_SeqID INT,
	@P_Navigation_Types_NVP_Detail_SeqID INT,
	@P_Account VARCHAR(128),
	@P_Debug INT = 1
AS
	SET NOCOUNT ON
	DECLARE @V_PERMISSION_ID INT
	SET @V_PERMISSION_ID = ZGWSecurity.Get_View_Permission_SeqID()
	DECLARE @V_AvalibleItems TABLE ([ID] INT, 
									TITLE VARCHAR(30), 
									[DESCRIPTION] VARCHAR(256), 
									URL VARCHAR(256), 
									PARENT INT, 
									SORT_ORDER INT, 
									ROLE VARCHAR(50),
									Function_Type_SeqID INT)
	INSERT INTO @V_AvalibleItems
		SELECT -- Menu items via roles
			[FUNCTIONS].Function_SeqID AS [ID],
			[FUNCTIONS].[NAME] AS TITLE,
			[FUNCTIONS].[DESCRIPTION],
			[FUNCTIONS].[ACTION] AS URL,
			[FUNCTIONS].Parent_SeqID AS PARENT,
			[FUNCTIONS].SORT_ORDER AS SORT_ORDER,
			ROLES.[NAME] AS ROLE,
			[FUNCTIONS].Function_Type_SeqID
		FROM
			ZGWSecurity.Roles_Security_Entities SE_ROLES WITH(NOLOCK),
			ZGWSecurity.Roles ROLES WITH(NOLOCK),
			ZGWSecurity.Roles_Security_Entities_Functions [SECURITY] WITH(NOLOCK),
			ZGWSecurity.Functions [FUNCTIONS] WITH(NOLOCK),
			ZGWSecurity.Permissions [Permissions] WITH(NOLOCK)
		WHERE
			SE_ROLES.Role_SeqID = ROLES.Role_SeqID
			AND [SECURITY].Roles_Security_Entities_SeqID = SE_ROLES.Roles_Security_Entities_SeqID
			AND [SECURITY].Function_SeqID = [FUNCTIONS].Function_SeqID
			AND [Permissions].NVP_Detail_SeqID = SECURITY.Permissions_NVP_Detail_SeqID
			AND [Permissions].NVP_Detail_SeqID = @V_PERMISSION_ID
			AND [FUNCTIONS].Navigation_Types_NVP_Detail_SeqID = @P_Navigation_Types_NVP_Detail_SeqID
			AND [FUNCTIONS].IS_NAV = 1
			AND SE_ROLES.Security_Entity_SeqID IN (SELECT Security_Entity_SeqID FROM ZGWSecurity.Get_Entity_Parents(1,@P_Security_Entity_SeqID))
		UNION ALL
		SELECT -- Menu items via groups
			[FUNCTIONS].Function_SeqID AS [ID],
			[FUNCTIONS].[NAME] AS TITLE,
			[FUNCTIONS].[DESCRIPTION],
			[FUNCTIONS].[ACTION] AS URL,
			[FUNCTIONS].Parent_SeqID AS PARENT,
			[FUNCTIONS].SORT_ORDER AS SORT_ORDER,
			ROLES.[NAME] AS ROLE,
			[FUNCTIONS].Function_Type_SeqID
		FROM
			ZGWSecurity.Groups_Security_Entities_Functions WITH(NOLOCK),
			ZGWSecurity.Groups_Security_Entities WITH(NOLOCK),
			ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities WITH(NOLOCK),
			ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
			ZGWSecurity.Roles ROLES WITH(NOLOCK),
			ZGWSecurity.Functions [FUNCTIONS] WITH(NOLOCK),
			ZGWSecurity.[Permissions] [Permissions] WITH(NOLOCK)
		WHERE
			ZGWSecurity.Groups_Security_Entities_Functions.Function_SeqID = [FUNCTIONS].Function_SeqID
			AND ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Functions.Groups_Security_Entities_SeqID
			AND ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID
			AND ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Roles_Security_Entities_SeqID
			AND ROLES.Role_SeqID = ZGWSecurity.Roles_Security_Entities.Role_SeqID
			AND [Permissions].NVP_Detail_SeqID = ZGWSecurity.Groups_Security_Entities_Functions.Permissions_NVP_Detail_SeqID
			AND [Permissions].NVP_Detail_SeqID = @V_PERMISSION_ID
			AND [FUNCTIONS].Navigation_Types_NVP_Detail_SeqID = @P_Navigation_Types_NVP_Detail_SeqID
			AND [FUNCTIONS].IS_NAV = 1
			AND ZGWSecurity.Groups_Security_Entities.Security_Entity_SeqID IN (SELECT Security_Entity_SeqID FROM ZGWSecurity.Get_Entity_Parents(1,@P_Security_Entity_SeqID))

	--SELECT * FROM @V_AvalibleMenuItems -- DEBUG

	DECLARE @V_AccountRoles TABLE (Roles VARCHAR(30)) -- Roles belonging to the account
	INSERT INTO @V_AccountRoles
		SELECT -- Roles via roles
			ZGWSecurity.Roles.[NAME] AS Roles
		FROM
			ZGWSecurity.Accounts WITH(NOLOCK),
			ZGWSecurity.Roles_Security_Entities_Accounts WITH(NOLOCK),
			ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
			ZGWSecurity.Roles WITH(NOLOCK)
		WHERE
			ZGWSecurity.Accounts.Account = @P_Account
			AND ZGWSecurity.Roles_Security_Entities_Accounts.Account_SeqID = ZGWSecurity.Accounts.Account_SeqID
			AND ZGWSecurity.Roles_Security_Entities_Accounts.Roles_Security_Entities_SeqID = ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID
			AND ZGWSecurity.Roles_Security_Entities.Security_Entity_SeqID IN (SELECT Security_Entity_SeqID FROM ZGWSecurity.Get_Entity_Parents(1,@P_Security_Entity_SeqID))
			AND ZGWSecurity.Roles_Security_Entities.Role_SeqID = ZGWSecurity.Roles.Role_SeqID
		UNION
		SELECT -- Roles via groups
			ZGWSecurity.Roles.[NAME] AS Roles
		FROM
			ZGWSecurity.Accounts WITH(NOLOCK),
			ZGWSecurity.Groups_Security_Entities_Accounts WITH(NOLOCK),
			ZGWSecurity.Groups_Security_Entities WITH(NOLOCK),
			ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities WITH(NOLOCK),
			ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
			ZGWSecurity.Roles WITH(NOLOCK)
		WHERE
			ZGWSecurity.Accounts.Account = @P_Account
			AND ZGWSecurity.Groups_Security_Entities_Accounts.Account_SeqID = ZGWSecurity.Accounts.Account_SeqID
			AND ZGWSecurity.Groups_Security_Entities.Security_Entity_SeqID IN (SELECT Security_Entity_SeqID FROM ZGWSecurity.Get_Entity_Parents(1,@P_Security_Entity_SeqID))
			AND ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Groups_Security_Entities_SeqID
			AND ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Roles_Security_Entities_SeqID
			AND ZGWSecurity.Roles_Security_Entities.Role_SeqID = ZGWSecurity.Roles.Role_SeqID

	--SELECT * FROM @V_AccountRoles -- DEBUG
	DECLARE @V_AllMenuItems TABLE ([ID] INT, TITLE VARCHAR(30), [DESCRIPTION] VARCHAR(256), URL VARCHAR(256), PARENT INT, SORT_ORDER INT, ROLE VARCHAR(50),Function_Type_SeqID INT)
	INSERT INTO @V_AllMenuItems
		SELECT -- Last but not least get the menu items when there are matching account roles.
			[ID],
			TITLE,
			[DESCRIPTION],
			URL,
			PARENT,
			SORT_ORDER,
			ROLE,
			Function_Type_SeqID
		FROM 
			@V_AvalibleItems
		WHERE
			ROLE IN (SELECT DISTINCT Roles FROM @V_AccountRoles)

	DECLARE @V_DistinctItems TABLE ([ID] INT, TITLE VARCHAR(30), [DESCRIPTION] VARCHAR(256), URL VARCHAR(256), PARENT INT, SORT_ORDER INT, Function_Type_SeqID INT)
	INSERT INTO @V_DistinctItems
		SELECT DISTINCT
			[ID],
			Title,
			[Description],
			URL,
			Parent,
			Sort_Order,
			Function_Type_SeqID
		FROM
			@V_AllMenuItems

	SELECT
		ID as MenuID,
		TITLE,
		[DESCRIPTION],
		URL,                                                                                                                                                                                                                                                              
		PARENT as ParentID,
		SORT_ORDER,
		Function_Type_SeqID as FUNCTION_TYPE_SEQ_ID
	FROM
		@V_DistinctItems
	ORDER BY
		Sort_Order


RETURN 0