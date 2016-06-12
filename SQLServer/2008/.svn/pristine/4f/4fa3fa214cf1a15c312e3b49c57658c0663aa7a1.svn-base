/*
Usage:
	DECLARE
		@P_NVP_SeqID int = 1,
		@P_Account_SeqID int = 2,
		@P_Security_Entity_SeqID int = 1,
		@P_Debug INT = 1

	exec ZGWSystem.Get_Name_Value_Pair
		@P_NVP_SeqID,
		@P_Account_SeqID,
		@P_Security_Entity_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns name value pairs 
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Get_Name_Value_Pair]
	@P_NVP_SeqID int,
	@P_Account_SeqID int,
	@P_Security_Entity_SeqID int,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSystem.Get_Name_Value_Pair'
	IF @P_NVP_SeqID > -1
		BEGIN
			SELECT
				NVP_SeqID as NVP_SEQ_ID
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
				ZGWSystem.Name_Value_Pairs.NVP_SeqID = @P_NVP_SeqID
			ORDER BY
				Static_Name
		END
	ELSE
		BEGIN
			IF @P_Account_SeqID > -1
				BEGIN -- get only valid NVP for the given account
					IF @P_Debug = 1 PRINT 'get only valid NVP for the given account'
					DECLARE @V_PERMISSION_ID INT
					SET @V_PERMISSION_ID = ZGWSecurity.Get_View_Permission_SeqID()
					DECLARE @V_AvalibleItems TABLE ([NVP_SeqID] int,
													[Schema_Name] varchar(30),
													[Static_Name] varchar(30),
													[Display] varchar(128),
													[Description] varchar(256),
													[Status_SeqID] int,
													[Added_By] int,
													[Added_Date] datetime,
													[Updated_By] int,
													[Updated_Date] datetime,
													[ROLE] VARCHAR(50))
					IF @P_Debug = 1 PRINT 'Geting items via roles'
					INSERT INTO @V_AvalibleItems
					SELECT -- Items via roles
						ZGWSystem.Name_Value_Pairs.NVP_SeqID,
						ZGWSystem.Name_Value_Pairs.[Schema_Name],
						ZGWSystem.Name_Value_Pairs.Static_Name,
						ZGWSystem.Name_Value_Pairs.Display,
						ZGWSystem.Name_Value_Pairs.[Description],
						ZGWSystem.Name_Value_Pairs.Status_SeqID,
						ZGWSystem.Name_Value_Pairs.Added_By,
						ZGWSystem.Name_Value_Pairs.Added_Date,
						ZGWSystem.Name_Value_Pairs.Updated_By,
						ZGWSystem.Name_Value_Pairs.Updated_Date,
						ROLES.NAME AS [ROLE]
					FROM
						ZGWSecurity.Roles_Security_Entities SE_ROLES,
						ZGWSecurity.Roles ROLES,
						ZGWSecurity.Roles_Security_Entities_Permissions [SECURITY],
						ZGWSystem.Name_Value_Pairs,
						ZGWSecurity.[Permissions] [Permissions]
					WHERE
						SE_ROLES.Role_SeqID = ROLES.Role_SeqID
						AND SECURITY.Roles_Security_Entities_SeqID = SE_ROLES.Roles_Security_Entities_SeqID
						AND SECURITY.NVP_SeqID = ZGWSystem.Name_Value_Pairs.NVP_SeqID
						AND [Permissions].NVP_Detail_SeqID = SECURITY.Permissions_NVP_Detail_SeqID
						AND [Permissions].NVP_Detail_SeqID = @V_PERMISSION_ID
						AND SE_ROLES.Security_Entity_SeqID IN (SELECT Security_Entity_SeqID FROM ZGWSecurity.Get_Entity_Parents(1,@P_Security_Entity_SeqID))
					IF @P_Debug = 1 PRINT 'Getting items via groups'
					INSERT INTO @V_AvalibleItems
					SELECT -- Items via groups
						ZGWSystem.Name_Value_Pairs.NVP_SeqID
						, ZGWSystem.Name_Value_Pairs.[Schema_Name]
						, ZGWSystem.Name_Value_Pairs.Static_Name
						, ZGWSystem.Name_Value_Pairs.Display
						, ZGWSystem.Name_Value_Pairs.[Description]
						, ZGWSystem.Name_Value_Pairs.Status_SeqID
						, ZGWSystem.Name_Value_Pairs.Added_By
						, ZGWSystem.Name_Value_Pairs.Added_Date
						, ZGWSystem.Name_Value_Pairs.Updated_By
						, ZGWSystem.Name_Value_Pairs.Updated_Date
						, ROLES.NAME AS [ROLE]
					FROM
						ZGWSecurity.Groups_Security_Entities_Permissions,
						ZGWSecurity.Groups_Security_Entities,
						ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities,
						ZGWSecurity.Roles_Security_Entities,
						ZGWSecurity.Roles ROLES,
						ZGWSystem.Name_Value_Pairs,
						ZGWSecurity.[Permissions] [Permissions]
					WHERE
						ZGWSecurity.Groups_Security_Entities_Permissions.NVP_SeqID = ZGWSystem.Name_Value_Pairs.NVP_SeqID
						AND ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Permissions.Groups_Security_Entities_SeqID
						AND ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID
						AND ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Roles_Security_Entities_SeqID
						AND ROLES.Role_SeqID = ZGWSecurity.Roles_Security_Entities.Role_SeqID
						AND [Permissions].NVP_Detail_SeqID = ZGWSecurity.Groups_Security_Entities_Permissions.Permissions_NVP_Detail_SeqID
						AND [Permissions].NVP_Detail_SeqID = @V_PERMISSION_ID
						AND ZGWSecurity.Groups_Security_Entities.Security_Entity_SeqID IN (SELECT Security_Entity_SeqID FROM ZGWSecurity.Get_Entity_Parents(1,@P_Security_Entity_SeqID))

					DECLARE @V_AccountRoles TABLE (Roles VARCHAR(30)) -- Roles belonging to the account
					IF @P_Debug = 1 PRINT 'Getting roles for account and roles via groups'
					INSERT INTO @V_AccountRoles
					SELECT -- Roles via roles
						ZGWSecurity.Roles.[NAME] AS Roles
					FROM
						ZGWSecurity.Accounts,
						ZGWSecurity.Roles_Security_Entities_Accounts,
						ZGWSecurity.Roles_Security_Entities,
						ZGWSecurity.Roles
					WHERE
						ZGWSecurity.Roles_Security_Entities_Accounts.Account_SeqID = @P_Account_SeqID
						AND ZGWSecurity.Roles_Security_Entities_Accounts.Roles_Security_Entities_SeqID = ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID
						AND ZGWSecurity.Roles_Security_Entities.Security_Entity_SeqID IN (SELECT Security_Entity_SeqID FROM ZGWSecurity.Get_Entity_Parents(1,@P_Security_Entity_SeqID))
						AND ZGWSecurity.Roles_Security_Entities.Role_SeqID = ZGWSecurity.Roles.Role_SeqID
					UNION
					SELECT -- Roles via groups
						ZGWSecurity.Roles.[NAME] AS Roles
					FROM
						ZGWSecurity.Accounts,
						ZGWSecurity.Groups_Security_Entities_Accounts,
						ZGWSecurity.Groups_Security_Entities,
						ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities,
						ZGWSecurity.Roles_Security_Entities,
						ZGWSecurity.Roles
					WHERE
						ZGWSecurity.Groups_Security_Entities_Accounts.Account_SeqID = @P_Account_SeqID
						AND ZGWSecurity.Groups_Security_Entities.Security_Entity_SeqID IN (SELECT Security_Entity_SeqID FROM ZGWSecurity.Get_Entity_Parents(1,@P_Security_Entity_SeqID))
						AND ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Groups_Security_Entities_SeqID
						AND ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Roles_Security_Entities_SeqID
						AND ZGWSecurity.Roles_Security_Entities.Role_SeqID = ZGWSecurity.Roles.Role_SeqID

					DECLARE @V_AllItems TABLE ([NVP_SeqID] int, 
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
							NVP_SeqID
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
							ROLE IN (SELECT DISTINCT * FROM @V_AccountRoles)

					DECLARE @V_DistinctItems TABLE ([NVP_SeqID] int, 
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
							NVP_SeqID,
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
						NVP_SeqID as NVP_SEQ_ID
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
						NVP_SeqID as NVP_SEQ_ID
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