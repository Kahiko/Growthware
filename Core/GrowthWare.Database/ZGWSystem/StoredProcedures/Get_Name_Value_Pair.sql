
/*
Usage:
	DECLARE
		@P_NVPSeqId int = 1,
		@P_AccountSeqId int = 2,
		@PSecurityEntitySeqId int = 1,
		@P_Debug INT = 1

	exec ZGWSystem.Get_Name_Value_Pair
		@P_NVPSeqId,
		@P_AccountSeqId,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns name value pairs 
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Get_Name_Value_Pair]
	@P_NVPSeqId int,
	@P_AccountSeqId int,
	@PSecurityEntitySeqId int,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSystem.Get_Name_Value_Pair'
	IF @P_NVPSeqId > -1
		BEGIN
			SELECT
				NVPSeqId as NVP_SEQ_ID
				, [Schema_Name]
				, Static_Name
				, Display
				, [Description]
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
			FROM
				ZGWSystem.Name_Value_Pairs
			WHERE
				ZGWSystem.Name_Value_Pairs.NVPSeqId = @P_NVPSeqId
			ORDER BY
				Static_Name
		END
	ELSE
		BEGIN
			IF @P_AccountSeqId > -1
				BEGIN -- get only valid NVP for the given account
					IF @P_Debug = 1 PRINT 'get only valid NVP for the given account'
					DECLARE @V_Permission_Id INT
					SET @V_Permission_Id = ZGWSecurity.Get_View_PermissionSeqId()
					DECLARE @V_AvalibleItems TABLE ([NVPSeqId] int,
													[Schema_Name] varchar(30),
													[Static_Name] varchar(30),
													[Display] varchar(128),
													[Description] varchar(256),
													[StatusSeqId] int,
													[Added_By] int,
													[Added_Date] datetime,
													[Updated_By] int,
													[Updated_Date] datetime,
													[Role] VARCHAR(50))
					IF @P_Debug = 1 PRINT 'Geting items via roles'
					INSERT INTO @V_AvalibleItems
					SELECT -- Items via roles
						ZGWSystem.Name_Value_Pairs.NVPSeqId,
						ZGWSystem.Name_Value_Pairs.[Schema_Name],
						ZGWSystem.Name_Value_Pairs.Static_Name,
						ZGWSystem.Name_Value_Pairs.Display,
						ZGWSystem.Name_Value_Pairs.[Description],
						ZGWSystem.Name_Value_Pairs.StatusSeqId,
						ZGWSystem.Name_Value_Pairs.Added_By,
						ZGWSystem.Name_Value_Pairs.Added_Date,
						ZGWSystem.Name_Value_Pairs.Updated_By,
						ZGWSystem.Name_Value_Pairs.Updated_Date,
						ROLES.Name AS [Role]
					FROM
						ZGWSecurity.Roles_Security_Entities SE_ROLES,
						ZGWSecurity.Roles ROLES,
						ZGWSecurity.Roles_Security_Entities_Permissions [SECURITY],
						ZGWSystem.Name_Value_Pairs,
						ZGWSecurity.[Permissions] [Permissions]
					WHERE
						SE_ROLES.RoleSeqId = ROLES.RoleSeqId
						AND SECURITY.Roles_Security_EntitiesSeqId = SE_ROLES.Roles_Security_EntitiesSeqId
						AND SECURITY.NVPSeqId = ZGWSystem.Name_Value_Pairs.NVPSeqId
						AND [Permissions].NVP_DetailSeqId = SECURITY.Permissions_NVP_DetailSeqId
						AND [Permissions].NVP_DetailSeqId = @V_Permission_Id
						AND SE_ROLES.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))
					IF @P_Debug = 1 PRINT 'Getting items via groups'
					INSERT INTO @V_AvalibleItems
					SELECT -- Items via groups
						ZGWSystem.Name_Value_Pairs.NVPSeqId
						, ZGWSystem.Name_Value_Pairs.[Schema_Name]
						, ZGWSystem.Name_Value_Pairs.Static_Name
						, ZGWSystem.Name_Value_Pairs.Display
						, ZGWSystem.Name_Value_Pairs.[Description]
						, ZGWSystem.Name_Value_Pairs.StatusSeqId
						, ZGWSystem.Name_Value_Pairs.Added_By
						, ZGWSystem.Name_Value_Pairs.Added_Date
						, ZGWSystem.Name_Value_Pairs.Updated_By
						, ZGWSystem.Name_Value_Pairs.Updated_Date
						, ROLES.[Name] AS [Role]
					FROM
						ZGWSecurity.Groups_Security_Entities_Permissions,
						ZGWSecurity.Groups_Security_Entities,
						ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities,
						ZGWSecurity.Roles_Security_Entities,
						ZGWSecurity.Roles ROLES,
						ZGWSystem.Name_Value_Pairs,
						ZGWSecurity.[Permissions] [Permissions]
					WHERE
						ZGWSecurity.Groups_Security_Entities_Permissions.NVPSeqId = ZGWSystem.Name_Value_Pairs.NVPSeqId
						AND ZGWSecurity.Groups_Security_Entities.Groups_Security_EntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Permissions.Groups_Security_EntitiesSeqId
						AND ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Groups_Security_EntitiesSeqId = ZGWSecurity.Groups_Security_Entities.Groups_Security_EntitiesSeqId
						AND ZGWSecurity.Roles_Security_Entities.Roles_Security_EntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Roles_Security_EntitiesSeqId
						AND ROLES.RoleSeqId = ZGWSecurity.Roles_Security_Entities.RoleSeqId
						AND [Permissions].NVP_DetailSeqId = ZGWSecurity.Groups_Security_Entities_Permissions.Permissions_NVP_DetailSeqId
						AND [Permissions].NVP_DetailSeqId = @V_Permission_Id
						AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))

					DECLARE @V_AccountRoles TABLE (Roles VARCHAR(30)) -- Roles belonging to the account
					IF @P_Debug = 1 PRINT 'Getting roles for account and roles via groups'
					INSERT INTO @V_AccountRoles
					SELECT -- Roles via roles
						ZGWSecurity.Roles.[Name] AS Roles
					FROM
						ZGWSecurity.Accounts,
						ZGWSecurity.Roles_Security_Entities_Accounts,
						ZGWSecurity.Roles_Security_Entities,
						ZGWSecurity.Roles
					WHERE
						ZGWSecurity.Roles_Security_Entities_Accounts.AccountSeqId = @P_AccountSeqId
						AND ZGWSecurity.Roles_Security_Entities_Accounts.Roles_Security_EntitiesSeqId = ZGWSecurity.Roles_Security_Entities.Roles_Security_EntitiesSeqId
						AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))
						AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
					UNION
					SELECT -- Roles via groups
						ZGWSecurity.Roles.[Name] AS Roles
					FROM
						ZGWSecurity.Accounts,
						ZGWSecurity.Groups_Security_Entities_Accounts,
						ZGWSecurity.Groups_Security_Entities,
						ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities,
						ZGWSecurity.Roles_Security_Entities,
						ZGWSecurity.Roles
					WHERE
						ZGWSecurity.Groups_Security_Entities_Accounts.AccountSeqId = @P_AccountSeqId
						AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))
						AND ZGWSecurity.Groups_Security_Entities.Groups_Security_EntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Groups_Security_EntitiesSeqId
						AND ZGWSecurity.Roles_Security_Entities.Roles_Security_EntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Roles_Security_EntitiesSeqId
						AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId

					DECLARE @V_AllItems TABLE ([NVPSeqId] int, 
												[Schema_Name] varchar(30),
												[Static_Name] varchar(30),
												[Display] varchar(128),
												[Description] varchar(256),
												[Added_By] int,
												[Added_Date] datetime,
												[Updated_By] int,
												[Updated_Date] datetime)
					IF @P_Debug = 1 PRINT 'Putting all items into tabable variable'
					INSERT INTO @V_AllItems
						SELECT -- Last but not least get the menu items when there are matching account roles.
							NVPSeqId
							, [Schema_Name]
							, Static_Name
							, Display
							, [Description]
							, Added_By
							, Added_Date
							, Updated_By
							, Updated_Date
						FROM 
							@V_AvalibleItems
						WHERE
							[Role] IN (SELECT DISTINCT * FROM @V_AccountRoles)

					DECLARE @V_DistinctItems TABLE ([NVPSeqId] int, 
													[Schema_Name] varchar(30),
													[Static_Name] varchar(30),
													[Display] varchar(128),
													[Description] varchar(256),
													[Added_By] int,
													[Added_Date] datetime,
													[Updated_By] int,
													[Updated_Date] datetime)
					IF @P_Debug = 1 PRINT 'Getting disting items into table variable'
					INSERT INTO @V_DistinctItems
						SELECT DISTINCT
							NVPSeqId,
							[Schema_Name],
							Static_Name,
							Display,
							[Description],
							Added_By,
							Added_Date,
							Updated_By,
							Updated_Date
						FROM
							@V_AllItems

					IF @P_Debug = 1 PRINT 'Selecting all distint items for account'
					SELECT
						NVPSeqId as NVP_SEQ_ID
						, [Schema_Name]
						, Static_Name
						, Display
						, [Description]
						, Added_By
						, Added_Date
						, Updated_By
						, Updated_Date
					FROM
						@V_DistinctItems
					ORDER BY
						Static_Name
				END
			ELSE
				BEGIN -- get only valid NVP for the given account
					IF @P_Debug = 1 PRINT 'get only valid NVP for the given account'
					SELECT
						NVPSeqId as NVP_SEQ_ID
						, [Schema_Name]
						, Static_Name
						, Display
						, [Description]
						, Added_By
						, Added_Date
						, Updated_By
						, Updated_Date
					FROM
						ZGWSystem.Name_Value_Pairs
					ORDER BY
						Static_Name
				END
		END
		IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Function_Roles'
RETURN 0

GO

