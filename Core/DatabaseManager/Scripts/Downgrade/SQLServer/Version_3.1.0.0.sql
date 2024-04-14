-- Downgrade
--USE [YourDatabaseName];
GO
SET NOCOUNT ON;

/****** Start: Procedure [ZGWSystem].[Get_Name_Value_Pair] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWSystem].[Get_Name_Value_Pair]') AND type in (N'P', N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Get_Name_Value_Pair] AS'
	END
--End If

GO
/*
Usage:
	DECLARE
		@P_NVPSeqId int = 1,
		@P_AccountSeqId int = 2,
		@P_SecurityEntitySeqId int = 1,
		@P_Debug INT = 1

	exec ZGWSystem.Get_Name_Value_Pair
		@P_NVPSeqId,
		@P_AccountSeqId,
		@P_SecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns name value pairs 
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Get_Name_Value_Pair]
	@P_NVPSeqId int,
	@P_AccountSeqId int,
	@P_SecurityEntitySeqId int,
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
				BEGIN
		-- get only valid NVP for the given account
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
			AND SECURITY.RolesSecurityEntitiesSeqId = SE_ROLES.RolesSecurityEntitiesSeqId
			AND SECURITY.NVPSeqId = ZGWSystem.Name_Value_Pairs.NVPSeqId
			AND [Permissions].NVP_DetailSeqId = SECURITY.PermissionsNVPDetailSeqId
			AND [Permissions].NVP_DetailSeqId = @V_Permission_Id
			AND SE_ROLES.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
			FROM ZGWSecurity.Get_Entity_Parents(1,@P_SecurityEntitySeqId))
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
			AND ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Permissions.GroupsSecurityEntitiesSeqId
			AND ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId
			AND ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.RolesSecurityEntitiesSeqId
			AND ROLES.RoleSeqId = ZGWSecurity.Roles_Security_Entities.RoleSeqId
			AND [Permissions].NVP_DetailSeqId = ZGWSecurity.Groups_Security_Entities_Permissions.PermissionsNVPDetailSeqId
			AND [Permissions].NVP_DetailSeqId = @V_Permission_Id
			AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
			FROM ZGWSecurity.Get_Entity_Parents(1,@P_SecurityEntitySeqId))

		DECLARE @V_AccountRoles TABLE (Roles VARCHAR(30))
		-- Roles belonging to the account
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
				AND ZGWSecurity.Roles_Security_Entities_Accounts.RolesSecurityEntitiesSeqId = ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId
				AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
				FROM ZGWSecurity.Get_Entity_Parents(1,@P_SecurityEntitySeqId))
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
				AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
				FROM ZGWSecurity.Get_Entity_Parents(1,@P_SecurityEntitySeqId))
				AND ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.GroupsSecurityEntitiesSeqId
				AND ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.RolesSecurityEntitiesSeqId
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
							[Role] IN (SELECT DISTINCT *
		FROM @V_AccountRoles)

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
				BEGIN
		-- get only valid NVP for the given account
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
/****** End: Procedure [ZGWSystem].[Get_Name_Value_Pair] ******/

/****** Start: Procedure [ZGWSystem].[Set_Name_Value_Pair] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWSystem].[Set_Name_Value_Pair]') AND type in (N'P', N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Set_Name_Value_Pair] AS'
	END
--End If

GO
/*
Usage:
	DECLARE 
		@P_NVPSeqId int = -1,
		@P_Schema_Name VARCHAR(30) = 'dbo',
		@P_Static_Name VARCHAR(30) = 'Testing',
		@P_Display VARCHAR(128) = 'TestingNVP',
		@P_Description VARCHAR(256) = 'Just Testing the Name value Pair',
		@P_StatusSeqId INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Primary_Key INT = null,
		@P_ErrorCode int = null,
		@P_Debug bit = 1

	exec ZGWSystem.Set_Name_Value_Pair
		@P_NVPSeqId,
		@P_Schema_Name,
		@P_Static_Name,
		@P_Display,
		@P_Description,
		@P_StatusSeqId,
		@P_Added_Updated_By,
		@P_Primary_Key,
		@P_ErrorCode,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates ZGWCoreWeb.Account_Choices based on @P_Account
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Set_Name_Value_Pair]
	@P_NVPSeqId int,
	@P_Schema_Name VARCHAR(30),
	@P_Static_Name VARCHAR(30),
	@P_Display VARCHAR(128),
	@P_Description VARCHAR(256),
	@P_StatusSeqId INT,
	@P_Added_Updated_By INT,
	@P_Primary_Key INT OUTPUT,
	@P_ErrorCode int OUTPUT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting [ZGWSystem].[Set_Name_Value_Pair]'
	DECLARE @V_Now DATETIME = GETDATE()
	IF @P_NVPSeqId > -1
		BEGIN
	-- UPDATE PROFILE
	UPDATE ZGWSystem.Name_Value_Pairs
			SET 
				[Display] = @P_Display,
				[Description] = @P_Description,
				StatusSeqId = @P_StatusSeqId,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = @V_Now
			WHERE
				NVPSeqId = @P_NVPSeqId

	SELECT @P_Primary_Key = @P_NVPSeqId
END
	ELSE
	BEGIN
	-- INSERT a new row in the table.

	-- CHECK FOR DUPLICATE NAME BEFORE INSERTING
	IF EXISTS(SELECT Static_Name
	FROM ZGWSystem.Name_Value_Pairs
	WHERE Static_Name = @P_Static_Name)
				BEGIN
		RAISERROR ('The name value pair already exists in the database.',16,1)
		SELECT @P_ErrorCode=1
		RETURN
	END
	IF NOT EXISTS (SELECT *
	FROM dbo.sysobjects
	WHERE id = OBJECT_ID('[' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR(MAX),@P_Static_Name) + ']') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
			BEGIN
		-- Create the new table to hold the details for the name value pair
		DECLARE @V_Statement nvarchar(4000)

		set @V_Statement = 'CREATE TABLE [' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR,@P_Static_Name) + '](
					[NVP_DetailSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
					[NVPSeqId] [int] NOT NULL,
					[NVP_Detail_Name] [varchar](50) NOT NULL,
					[NVP_Detail_Value] [varchar](300) NOT NULL,
					[StatusSeqId] [int] NOT NULL,
					[Sort_Order] [int] NOT NULL,
					[Added_By] [int] NOT NULL,
					[Added_DATE] [datetime] NOT NULL,
					[Updated_By] [int] NULL,
					[Updated_Date] [datetime] NULL,
					 CONSTRAINT [PK_' + CONVERT(VARCHAR,@P_Static_Name) + '] PRIMARY KEY CLUSTERED 
					(
						[NVP_DetailSeqId] ASC
					)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
					 CONSTRAINT [UK_' + CONVERT(VARCHAR,@P_Static_Name) + '] UNIQUE NONCLUSTERED 
					(
						[NVP_Detail_Name] ASC,	
						[NVP_Detail_Value] ASC
					)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
					) ON [PRIMARY]
					ALTER TABLE [' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR,@P_Static_Name) + '] WITH CHECK ADD CONSTRAINT [FK_' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '_' + CONVERT(VARCHAR,@P_Static_Name) + '_ZGWSystem_Statuses] FOREIGN KEY([StatusSeqId])
					REFERENCES [ZGWSystem].[Statuses] ([StatusSeqId])
					ON UPDATE CASCADE
					ON DELETE CASCADE
					ALTER TABLE [' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR,@P_Static_Name) + '] CHECK CONSTRAINT [FK_' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '_' + CONVERT(VARCHAR,@P_Static_Name) + '_ZGWSystem_Statuses]
					ALTER TABLE[' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR,@P_Static_Name) + ']  WITH CHECK ADD  CONSTRAINT [FK_' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '_' + CONVERT(VARCHAR,@P_Static_Name) + '_ZGWSystem_Name_Value_Pairs] FOREIGN KEY([NVPSeqId])
					REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVPSeqId])
					ON UPDATE CASCADE
					ON DELETE CASCADE
					ALTER TABLE [' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR,@P_Static_Name) + '] CHECK CONSTRAINT [FK_' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '_' + CONVERT(VARCHAR,@P_Static_Name) + '_ZGWSystem_Name_Value_Pairs]
					'
		IF @P_Debug = 1 PRINT  @V_Statement
		EXECUTE dbo.sp_executesql @statement = @V_Statement

	END
	INSERT ZGWSystem.Name_Value_Pairs
		(
		[Schema_Name],
		Static_Name,
		[Display],
		[Description],
		StatusSeqId,
		Added_By,
		Added_Date
		)
	VALUES
		(
			@P_Schema_Name,
			@P_Static_Name,
			@P_Display,
			@P_Description,
			@P_StatusSeqId,
			@P_Added_Updated_By,
			@V_Now
			)
	SELECT @P_Primary_Key=SCOPE_IDENTITY()
-- Get the IDENTITY value for the row just inserted.
END
-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR
IF @P_Debug = 1 PRINT 'End [ZGWSystem].[Set_Name_Value_Pair]'

GO
/****** End: Procedure [ZGWSystem].[Set_Name_Value_Pair] ******/
/****** Start: StoredProcedure [ZGWSystem].[Get_Name_Value_Pair_Detail] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Get_Name_Value_Pair_Detail]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Get_Name_Value_Pair_Detail] AS'
END
GO
/*
Usage:
	DECLARE
		@P_NVPSeqId INT = 1,
		@P_NVP_DetailSeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSystem.Get_Name_Value_Pair_Detail
		@P_NVPSeqId,
		@P_NVP_DetailSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/11/2011
-- Description:	Returns name value pair detail
-- Note:
--	This not the most effecient however this should
--	not be called very often ... it is intended for the
--	front end to cache the information and only get called
--	when needed
-- =============================================
-- Author:		Michael Regan
-- Create date: 11/03/2023
-- Description:	Fixed the returned columns
-- =============================================
-- Author:		Michael Regan
-- Create date: 04/14/2024
-- Description:	Fixed error near ","
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Get_Name_Value_Pair_Detail]
	@P_NVPSeqId INT,
	@P_NVP_DetailSeqId INT,
	@P_Debug INT = 0
AS
	DECLARE @V_TableName VARCHAR(30)
	DECLARE @V_Statement nvarchar(4000)
	SET @V_TableName = (SELECT [Schema_Name] + '.' + Static_Name
FROM ZGWSystem.Name_Value_Pairs
WHERE NVPSeqId = @P_NVPSeqId)
	SET @V_Statement = '[NVP_DetailSeqId], [NVPSeqId] as NVP_SEQ_ID, [NVP_Detail_Name], [NVP_Detail_Value], [StatusSeqId], [Sort_Order], [Added_By], [Added_Date], [Updated_By], [Updated_Date] FROM ' + CONVERT(VARCHAR,@V_TableName) + '
	WHERE
		NVP_DetailSeqId = ' + CONVERT(VARCHAR,@P_NVP_DetailSeqId) + ' ORDER BY Static_Name'

	EXECUTE dbo.sp_executesql @statement = @V_Statement
RETURN 0

GO
/****** END: StoredProcedure [ZGWSystem].[Get_Name_Value_Pair_Detail] ******/
/****** Start: StoredProcedure [ZGWSystem].[Set_Name_Value_Pair_Detail] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Set_Name_Value_Pair_Detail]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Set_Name_Value_Pair_Detail] AS'
END
GO

/*
Usage:
	DECLARE 
		@V_NVP_DetailSeqId INT = -1,
		@V_NVPSeqId int = (SELECT NVPSeqId FROM ZGWSystem.Name_Value_Pairs WHERE STATIC_NAME = 'Navigation_Types') ,
		@V_NVP_Detail_Name VARCHAR(50) = 'Test',
		@V_NVP_Detail_Value VARCHAR(300) = 'Test value',
		@V_StatusSeqId INT = 1,
		@V_Sort_Order INT = 1,
		@V_Added_Updated_BY INT = 1,
		@V_Primary_Key INT = null,
		@V_ErrorCode int = null,
		@V_Debug bit = 1

	exec ZGWSystem.Set_Name_Value_Pair_Detail
		@V_NVP_DetailSeqId,
		@V_NVPSeqId,
		@V_NVP_Detail_Name,
		@V_NVP_Detail_Value,
		@V_StatusSeqId,
		@V_Sort_Order,
		@V_Added_Updated_BY,
		@V_Primary_Key,
		@V_ErrorCode,
		@V_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates ZGWCoreWeb.Account_Choices based on @P_Account
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Set_Name_Value_Pair_Detail]
	@P_NVP_DetailSeqId INT,
	@P_NVPSeqId int,
	@P_NVP_Detail_Name VARCHAR(50),
	@P_NVP_Detail_Value VARCHAR(300),
	@P_StatusSeqId INT,
	@P_Sort_Order INT,
	@P_Added_Updated_By INT,
	@P_Primary_Key INT OUTPUT,
	@P_ErrorCode int OUTPUT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting [ZGWSystem].[Set_Name_Value_Pair_Detail]'
	DECLARE 
		@V_Static_Name VARCHAR(30) = (SELECT Static_Name
FROM ZGWSystem.Name_Value_Pairs
WHERE NVPSeqId = @P_NVPSeqId)
		,@V_Schema_Name VARCHAR(30) = (SELECT [Schema_Name]
FROM ZGWSystem.Name_Value_Pairs
WHERE NVPSeqId = @P_NVPSeqId)
		,@V_Statement NVARCHAR(4000)
		,@V_Now DATETIME = GETDATE()

	IF @P_NVP_DetailSeqId > -1
		BEGIN
	-- UPDATE PROFILE
	SET @V_Statement = 'UPDATE ' + CONVERT(VARCHAR,@V_Schema_Name) + '.' + CONVERT(VARCHAR,@V_Static_Name) + '
			SET 
				NVP_Detail_Name = ''' + CONVERT(VARCHAR,@P_NVP_Detail_Name) + ''',
				NVP_Detail_Value = ''' + CONVERT(VARCHAR,@P_NVP_Detail_Value) + ''',
				StatusSeqId = ' + CONVERT(VARCHAR,@P_StatusSeqId) + ',
				Sort_Order = ' + CONVERT(VARCHAR,@P_Sort_Order) + ',
				Updated_By = ' + CONVERT(VARCHAR,@P_Added_Updated_By) + ',
				UPDATED_DATE = ''' + CONVERT(VARCHAR,@V_Now) + '''
			WHERE
				NVP_DetailSeqId = ' + CONVERT(VARCHAR,@P_NVP_DetailSeqId)
	IF @P_Debug = 1 PRINT @V_Statement
	EXECUTE dbo.sp_executesql @statement = @V_Statement
	SELECT @P_Primary_Key = @P_NVPSeqId
END
	ELSE
		BEGIN
	-- INSERT a new row in the table.
	-- CHECK FOR DUPLICATE NAME BEFORE INSERTING
	DECLARE @V_COUNT INT
	SET @V_Statement= 'SET @V_COUNT = (SELECT COUNT(*)
				   FROM ' + CONVERT(VARCHAR,@V_Schema_Name) + '.' + CONVERT(VARCHAR,@V_Static_Name) + '
				   WHERE NVP_Detail_Value = ''' + CONVERT(VARCHAR,@P_NVP_Detail_Value) + ''' AND NVP_Detail_Name = ''' + CONVERT(VARCHAR,@P_NVP_Detail_Name) + ''')'
	IF @P_Debug = 1 PRINT @V_Statement
	EXECUTE sp_executesql @V_Statement,N'@V_COUNT int output',@V_COUNT output
	IF @V_COUNT > 0
				BEGIN
		RAISERROR ('The entry already exists in the database.',16,1)
		RETURN
	END
	SET @V_Statement = 'INSERT INTO ' + CONVERT(VARCHAR,@V_Schema_Name) + '.' + CONVERT(VARCHAR,@V_Static_Name) + '(
					NVPSeqId,
					NVP_Detail_Name,
					NVP_Detail_Value,
					StatusSeqId,
					Sort_Order,
					Added_By,
					ADDED_DATE
				)
				VALUES
				(
					' + CONVERT(VARCHAR,@P_NVPSeqId) + ',
					''' + CONVERT(VARCHAR,@P_NVP_Detail_Name) + ''',
					''' + CONVERT(VARCHAR,@P_NVP_Detail_Value) + ''',
					' + CONVERT(VARCHAR,@P_StatusSeqId) + ',
					' + CONVERT(VARCHAR,@P_Sort_Order) + ',
					' + CONVERT(VARCHAR,@P_Added_Updated_By) + ',
					''' + CONVERT(VARCHAR,@V_Now) + '''
				)'
	IF @P_Debug = 1 PRINT @V_Statement
	EXECUTE dbo.sp_executesql @statement = @V_Statement
	SELECT @P_Primary_Key=SCOPE_IDENTITY()
-- Get the IDENTITY value for the row just inserted.
--PRINT 'DONE ADDING'
END
-- Get the Error Code for the statement just executed.
--PRINT 'SETTING ERROR CODE'
SELECT @P_ErrorCode=@@ERROR
RETURN 0

GO
/****** END: StoredProcedure [ZGWSystem].[Set_Name_Value_Pair_Detail] ******/
/****** Start: StoredProcedure [ZGWSystem].[Get_Name_Value_Pair_Details] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Get_Name_Value_Pair_Details]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Get_Name_Value_Pair_Details] AS'
END
GO
/*
Usage:
	DECLARE
		@P_NVPSeqId int = 1,
		@P_Debug INT = 1

	exec ZGWSystem.Get_Name_Value_Pair_Details
		@P_NVPSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns name value pair details 
-- Note:
--	This not the most effecient however this should
--	not be called very often ... it is intended for the
--	front end to cache the information and only get called
--	when needed
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Get_Name_Value_Pair_Details] @P_NVPSeqId INT
	,@P_Debug INT = 0
AS
SET NOCOUNT ON

IF @P_Debug = 1
	PRINT 'Starting ZGWSecurity.Get_Name_Value_Pair_Details'

CREATE TABLE #NVP_DETAILS (
	 NVP_DetailSeqId INT
	,NVPSeqId INT
	,NVP_Detail_Name VARCHAR(50)
	,NVP_Detail_Value VARCHAR(300)
	,StatusSeqId INT
	,Sort_Order INT
	,Added_By INT
	,Added_Date DATETIME
	,Updated_By INT
	,Updated_Date DATETIME
	)

DECLARE @V_NVPSeqId INT
	,@V_Static_Name VARCHAR(30)
	,@V_Schema_Name VARCHAR(30)
	,@V_Statement NVARCHAR(max)

SET @V_Statement = 'SELECT * FROM '

DECLARE V_Name_Value_Pairs CURSOR STATIC LOCAL
FOR
SELECT NVPSeqId
	,Static_Name
	,[Schema_Name]
FROM ZGWSystem.Name_Value_Pairs

OPEN V_Name_Value_Pairs

FETCH NEXT
FROM V_Name_Value_Pairs
INTO @V_NVPSeqId
	,@V_Static_Name
	,@V_Schema_Name

WHILE (@@FETCH_STATUS = 0)
BEGIN
	SET @V_Statement = @V_Statement + CONVERT(VARCHAR, @V_Schema_Name) + '.' + CONVERT(VARCHAR, @V_Static_Name) + ' UNION ALL SELECT * FROM '

	FETCH NEXT
	FROM V_Name_Value_Pairs
	INTO @V_NVPSeqId
		,@V_Static_Name
		,@V_Schema_Name
END

CLOSE V_Name_Value_Pairs

DEALLOCATE V_Name_Value_Pairs

SET @V_Statement = SUBSTRING(@V_Statement, 0, LEN(@V_Statement) - 23)

IF @P_Debug = 1
	PRINT @V_Statement

INSERT INTO #NVP_DETAILS
EXECUTE dbo.sp_executesql @statement = @V_Statement

IF @P_NVPSeqId = - 1
	SELECT #NVP_DETAILS.NVP_DetailSeqId AS NVP_SEQ_DET_ID
		,#NVP_DETAILS.NVPSeqId AS NVP_SEQ_ID
		,ZGWSystem.Name_Value_Pairs.[Schema_Name] + '.' + ZGWSystem.Name_Value_Pairs.Static_Name AS [Table_Name]
		,#NVP_DETAILS.NVP_Detail_Name AS NVP_DET_VALUE
		,#NVP_DETAILS.NVP_Detail_Value AS NVP_DET_TEXT
		,#NVP_DETAILS.StatusSeqId AS STATUS_SEQ_ID
		,#NVP_DETAILS.Sort_Order
		,(
			SELECT TOP (1) Account
			FROM ZGWSecurity.Accounts
			WHERE AccountSeqId = #NVP_DETAILS.Added_By
			) AS Added_By
		,#NVP_DETAILS.Added_Date
		,(
			SELECT TOP (1) Account
			FROM ZGWSecurity.Accounts
			WHERE AccountSeqId = #NVP_DETAILS.Updated_By
			) AS Updated_By
		,#NVP_DETAILS.Updated_Date
	FROM #NVP_DETAILS
		,ZGWSystem.Name_Value_Pairs
	WHERE #NVP_DETAILS.NVPSeqId = ZGWSystem.Name_Value_Pairs.NVPSeqId
	ORDER BY ZGWSystem.Name_Value_Pairs.Static_Name
		,#NVP_DETAILS.NVP_Detail_Value
ELSE
	SELECT #NVP_DETAILS.NVP_DetailSeqId AS NVP_SEQ_DET_ID
		,#NVP_DETAILS.NVPSeqId AS NVP_SEQ_ID
		,ZGWSystem.Name_Value_Pairs.[Schema_Name] + '.' + ZGWSystem.Name_Value_Pairs.Static_Name AS [Table_Name]
		,#NVP_DETAILS.NVP_Detail_Name AS NVP_DET_VALUE
		,#NVP_DETAILS.NVP_Detail_Value AS NVP_DET_TEXT
		,#NVP_DETAILS.StatusSeqId AS STATUS_SEQ_ID
		,#NVP_DETAILS.Sort_Order
		,(
			SELECT TOP (1) Account
			FROM ZGWSecurity.Accounts
			WHERE AccountSeqId = #NVP_DETAILS.Added_By
			) AS Added_By
		,#NVP_DETAILS.Added_Date
		,(
			SELECT TOP (1) Account
			FROM ZGWSecurity.Accounts
			WHERE AccountSeqId = #NVP_DETAILS.Updated_By
			) AS Updated_By
		,#NVP_DETAILS.Updated_Date
	FROM #NVP_DETAILS
		,ZGWSystem.Name_Value_Pairs
	WHERE #NVP_DETAILS.NVPSeqId = ZGWSystem.Name_Value_Pairs.NVPSeqId
		AND ZGWSystem.Name_Value_Pairs.NVPSeqId = @P_NVPSeqId
	ORDER BY ZGWSystem.Name_Value_Pairs.Static_Name
		,#NVP_DETAILS.NVP_Detail_Value

DROP TABLE #NVP_DETAILS

RETURN 0

GO
/****** End: StoredProcedure [ZGWSystem].[Get_Name_Value_Pair_Details] ******/

-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '3.0.4.0',
    [Updated_By] = null,
    [Updated_Date] = null