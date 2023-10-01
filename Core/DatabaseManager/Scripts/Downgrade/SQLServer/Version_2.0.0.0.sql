-- Downgrade

/****** Object:  StoredProcedure [ZGWCoreWeb].[Set_Message]    Script Date: 9/24/2023 5:49:41 AM ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('ZGWCoreWeb.Set_Message'))
   exec('CREATE PROCEDURE [ZGWSecurity].[Get_Menu_Data] AS BEGIN SET NOCOUNT ON; END')
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
Usage:
	DECLARE 
		@P_MessageSeqId INT = 1,
		@P_SecurityEntitySeqId INT = 2,
		@P_Name VARCHAR(50) 'Test',
		@P_Title VARCHAR(100) = 'Just Testing',
		@P_Description VARCHAR(512) = 'Some description',
		@P_Body VARCHAR(MAX) = 'The body',
		@P_Format_As_HTML INT = 0,
		@P_Added_Updated_By INT = 1,
		@P_Primary_Key int,
		@P_Debug INT = 1

	exec ZGWCoreWeb.Set_Message
		@P_MessageSeqId,
		@P_SecurityEntitySeqId,
		@P_Name,
		@P_Title,
		@P_Description,
		@P_Body,
		@P_Format_As_HTML,
		@P_Added_Updated_By,
		@P_Primary_Key OUT,
		@P_Debug

	PRINT 'Primay key is: ' + CONVERT(VARCHAR(30),@P_Primary_Key)
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/02/2011
-- Description:	Inserts or updates ZGWCoreWeb.[Messages]
-- =============================================
ALTER PROCEDURE [ZGWCoreWeb].[Set_Message]
	@P_MessageSeqId INT,
	@P_SecurityEntitySeqId INT,
	@P_Name VARCHAR(50),
	@P_Title VARCHAR(100),
	@P_Description VARCHAR(512),
	@P_Body VARCHAR(MAX),
	@P_Format_As_HTML INT,
	@P_Added_Updated_By INT,
	@P_Primary_Key int OUTPUT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Message'
	DECLARE @V_Now DATETIME = GETDATE()

	IF @P_MessageSeqId > -1
		BEGIN
	-- UPDATE PROFILE
	-- CHECK FOR DUPLICATE Name BEFORE INSERTING
	IF EXISTS( SELECT [Name]
	FROM ZGWCoreWeb.[Messages]
	WHERE [Name] = @P_Name AND
		SecurityEntitySeqId = @P_SecurityEntitySeqId
			)
				BEGIN
		UPDATE ZGWCoreWeb.[Messages]
					SET
						SecurityEntitySeqId = @P_SecurityEntitySeqId,
						[Name] = @P_Name,
						Title = @P_Title,
						[Description] = @P_Description,
						Format_As_HTML = @P_Format_As_HTML,
						Body = @P_Body,
						Updated_By = @P_Added_Updated_By,
						Updated_Date = GETDATE()
					WHERE
						MessageSeqId = @P_MessageSeqId
			AND SecurityEntitySeqId = @P_SecurityEntitySeqId

		SELECT @P_Primary_Key = @P_MessageSeqId
	-- set the output id just in case.
	END
			ELSE
				BEGIN
		INSERT ZGWCoreWeb.[Messages]
			(
			SecurityEntitySeqId,
			[Name],
			Title,
			[Description],
			BODY,
			Format_As_HTML,
			Added_By,
			Added_Date
			)
		VALUES
			(
				@P_SecurityEntitySeqId,
				@P_Name,
				@P_Title,
				@P_Description,
				@P_Body,
				@P_Format_As_HTML,
				@P_Added_Updated_By,
				@V_Now
					)
		SELECT @P_Primary_Key = SCOPE_IDENTITY()
	-- Get the IDENTITY value for the row just inserted.
	END
END
	ELSE
		BEGIN
	-- INSERT a new row in the table.

	-- CHECK FOR DUPLICATE Name BEFORE INSERTING
	IF EXISTS( SELECT [Name]
	FROM ZGWCoreWeb.[Messages]
	WHERE [Name] = @P_Name AND
		SecurityEntitySeqId = @P_SecurityEntitySeqId
			)
			BEGIN
		RAISERROR ('The message you entered already exists in the database.',16,1)
		RETURN
	END

	INSERT ZGWCoreWeb.[Messages]
		(
		SecurityEntitySeqId,
		[Name],
		Title,
		[Description],
		Body,
		Format_As_HTML,
		Added_By,
		Added_Date
		)
	VALUES
		(
			@P_SecurityEntitySeqId,
			@P_Name,
			@P_Title,
			@P_Description,
			@P_Body,
			@P_Format_As_HTML,
			@P_Added_Updated_By,
			@V_Now
			)
	SELECT @P_Primary_Key = SCOPE_IDENTITY()
-- Get the IDENTITY value for the row just inserted.
END
	-- END IF
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Set_Message'
RETURN 0
GO

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
		[Added_By] = (SELECT Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = G.Added_By),
		G.Added_Date,
		[Updated_By] = (SELECT Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = G.Updated_By),
		G.[Updated_Date],
		RSE.SecurityEntitySeqId
	FROM
		ZGWSecurity.Groups G WITH(NOLOCK)
		INNER JOIN ZGWSecurity.Groups_Security_Entities RSE WITH(NOLOCK)
			ON G.GroupSeqId = RSE.GroupSeqId
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [ZGWSecurity].[vwSearchRoles] AS 
	SELECT
		R.[RoleSeqId] AS ROLE_SEQ_ID,
		R.[Name],
		R.[Description],
		R.[Is_System],
		R.[Is_System_Only],
		Added_By = (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = R.Added_By),
		R.Added_Date,
		[Updated_By] = (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = R.Updated_By),
		R.[Updated_Date],
		RSE.SecurityEntitySeqId
	FROM
		ZGWSecurity.Roles R WITH(NOLOCK)
		INNER JOIN ZGWSecurity.Roles_Security_Entities RSE WITH(NOLOCK)
			ON R.RoleSeqId = RSE.RoleSeqId
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
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'Logoff' WHERE [Action] = '/accounts/logout';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'Logon' WHERE [Action] = '/accounts/logon';
UPDATE [ZGWSecurity].[Functions] SET [Link_Behavior] = 1 WHERE [Action] = '/accounts/logon';

DELETE FROM [ZGWSecurity].[Functions] WHERE [Action] = 'SaveAccount'

DECLARE 
	@V_FunctionSeqId int = 4,
	@V_SecurityEntitySeqId	INT = 1,
	@V_AddPermission    INT = (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Add'),
	@V_EditPermission   INT = (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Edit'),
	@V_DeletePermission INT	= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Delete'),
	@V_ErrorCode int,
	@V_Debug INT = 0;

SET @V_FunctionSeqId = (SELECT FunctionSeqId from ZGWSecurity.Functions WHERE action='EditMessage');
EXEC ZGWSecurity.Delete_Function_Roles @V_FunctionSeqId, @V_SecurityEntitySeqId, @V_AddPermission, @V_ErrorCode OUT;
EXEC ZGWSecurity.Delete_Function_Roles @V_FunctionSeqId, @V_SecurityEntitySeqId, @V_EditPermission, @V_ErrorCode OUT;

SET @V_FunctionSeqId = (SELECT FunctionSeqId from ZGWSecurity.Functions WHERE action='FunctionSecurity');
EXEC ZGWSecurity.Delete_Function_Roles @V_FunctionSeqId, @V_SecurityEntitySeqId, @V_AddPermission, @V_ErrorCode OUT;
EXEC ZGWSecurity.Delete_Function_Roles @V_FunctionSeqId, @V_SecurityEntitySeqId, @V_EditPermission, @V_ErrorCode OUT;
EXEC ZGWSecurity.Delete_Function_Roles @V_FunctionSeqId, @V_SecurityEntitySeqId, @V_DeletePermission, @V_ErrorCode OUT;

UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '1.0.0.0',
    [Updated_By] = null,
    [Updated_Date] = null
WHERE [Version] = '2.0.0.0'