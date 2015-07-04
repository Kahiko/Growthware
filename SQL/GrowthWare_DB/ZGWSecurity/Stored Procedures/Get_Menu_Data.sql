/*
Usage:
	DECLARE 
		@P_Security_Entity_SeqID AS INT = 1,
		@P_Navigation_Types_NVP_Detail_SeqID AS INT = 3,
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
	DECLARE @V_Permission_Id INT
	SET @V_Permission_Id = ZGWSecurity.Get_View_Permission_SeqID()
	DECLARE @V_AvalibleItems TABLE ([ID] INT, 
									Title VARCHAR(30), 
									[Description] VARCHAR(256), 
									URL VARCHAR(256), 
									Parent INT, 
									Sort_Order INT, 
									[Role] VARCHAR(50),
									Function_Type_SeqID INT)
	INSERT INTO @V_AvalibleItems
		SELECT -- Menu items via roles
			[FUNCTIONS].Function_SeqID AS [ID],
			[FUNCTIONS].[Name] AS Title,
			[FUNCTIONS].[Description],
			[FUNCTIONS].[Action] AS URL,
			[FUNCTIONS].Parent_SeqID AS Parent,
			[FUNCTIONS].Sort_Order AS Sort_Order,
			ROLES.[Name] AS ROLE,
			[FUNCTIONS].Function_Type_SeqID
		FROM
			ZGWSecurity.Roles_Security_Entities SE_ROLES WITH(NOLOCK),
			ZGWSecurity.Roles ROLES WITH(NOLOCK),
			ZGWSecurity.Roles_Security_Entities_Functions [SECURITY] WITH(NOLOCK),
			ZGWSecurity.Functions [FUNCTIONS] WITH(NOLOCK),
			ZGWSecurity.[Permissions] [Permissions] WITH(NOLOCK)
		WHERE
			SE_ROLES.Role_SeqID = ROLES.Role_SeqID
			AND [SECURITY].Roles_Security_Entities_SeqID = SE_ROLES.Roles_Security_Entities_SeqID
			AND [SECURITY].Function_SeqID = [FUNCTIONS].Function_SeqID
			AND [Permissions].NVP_Detail_SeqID = SECURITY.Permissions_NVP_Detail_SeqID
			AND [Permissions].NVP_Detail_SeqID = @V_Permission_Id
			AND [FUNCTIONS].Navigation_Types_NVP_Detail_SeqID = @P_Navigation_Types_NVP_Detail_SeqID
			AND [FUNCTIONS].Is_Nav = 1
			AND SE_ROLES.Security_Entity_SeqID IN (SELECT Security_Entity_SeqID FROM ZGWSecurity.Get_Entity_Parents(1,@P_Security_Entity_SeqID))
		UNION ALL
		SELECT -- Menu items via groups
			[FUNCTIONS].Function_SeqID AS [ID],
			[FUNCTIONS].[Name] AS Title,
			[FUNCTIONS].[Description],
			[FUNCTIONS].[Action] AS URL,
			[FUNCTIONS].Parent_SeqID AS Parent,
			[FUNCTIONS].Sort_Order AS Sort_Order,
			ROLES.[Name] AS ROLE,
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
			AND [Permissions].NVP_Detail_SeqID = @V_Permission_Id
			AND [FUNCTIONS].Navigation_Types_NVP_Detail_SeqID = @P_Navigation_Types_NVP_Detail_SeqID
			AND [FUNCTIONS].Is_Nav = 1
			AND ZGWSecurity.Groups_Security_Entities.Security_Entity_SeqID IN (SELECT Security_Entity_SeqID FROM ZGWSecurity.Get_Entity_Parents(1,@P_Security_Entity_SeqID))

	--SELECT * FROM @V_AvalibleMenuItems -- DEBUG

	DECLARE @V_AccountRoles TABLE (Roles VARCHAR(30)) -- Roles belonging to the account
	INSERT INTO @V_AccountRoles
		EXEC ZGWSecurity.Get_Account_Security @P_Account, @P_Security_Entity_SeqID, @P_Debug

	--SELECT * FROM @V_AccountRoles -- DEBUG
	DECLARE @V_AllMenuItems TABLE ([ID] INT, Title VARCHAR(30), [Description] VARCHAR(256), URL VARCHAR(256), Parent INT, Sort_Order INT, ROLE VARCHAR(50),Function_Type_SeqID INT)
	INSERT INTO @V_AllMenuItems
		SELECT -- Last but not least get the menu items when there are matching account roles.
			[ID],
			Title,
			[Description],
			URL,
			Parent,
			Sort_Order,
			[Role],
			Function_Type_SeqID
		FROM 
			@V_AvalibleItems
		WHERE
			ROLE IN (SELECT DISTINCT Roles FROM @V_AccountRoles)

	DECLARE @V_DistinctItems TABLE ([ID] INT, TITLE VARCHAR(30), [Description] VARCHAR(256), URL VARCHAR(256), Parent INT, Sort_Order INT, Function_Type_SeqID INT)
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
		TITLE AS Title,
		[Description],
		URL,                                                                                                                                                                                                                                                              
		Parent as ParentID,
		Sort_Order,
		Function_Type_SeqID as Function_Type_Seq_ID
	FROM
		@V_DistinctItems
	ORDER BY
		Parent,
		Sort_Order,
		Title,
		ID


RETURN 0