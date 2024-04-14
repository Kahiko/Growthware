-- Upgrade
--USE [YourDatabaseName];
GO

SET NOCOUNT ON;

/****** Start: Procedure [ZGWSystem].[Get_Name_Value_Pair] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWSystem].[Get_Name_Value_Pair]') AND type IN (N'P' ,N'PC'))
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
-- Author:		Michael Regan
-- Create date: 04/12/2024
-- Description:	Added 
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Get_Name_Value_Pair] 
	 @P_NVPSeqId INT
	,@P_AccountSeqId INT
	,@P_SecurityEntitySeqId INT
	,@P_Debug INT = 0
AS
SET NOCOUNT ON

IF @P_Debug = 1
	PRINT 'Starting ZGWSystem.Get_Name_Value_Pair'

IF @P_NVPSeqId > - 1
BEGIN
	SELECT NVPSeqId AS NVP_SEQ_ID
		,[Schema_Name]
		,Static_Name
		,Display
		,[Description]
		,[STATUS_SEQ_ID] = [StatusSeqId]
		,Added_By
		,Added_Date
		,Updated_By
		,Updated_Date
	FROM ZGWSystem.Name_Value_Pairs
	WHERE ZGWSystem.Name_Value_Pairs.NVPSeqId = @P_NVPSeqId
	ORDER BY Static_Name
END
ELSE
BEGIN
	IF @P_AccountSeqId > - 1
	BEGIN
		-- get only valid NVP for the given account
		IF @P_Debug = 1
			PRINT 'get only valid NVP for the given account'

		DECLARE @V_Permission_Id INT

		SET @V_Permission_Id = ZGWSecurity.Get_View_PermissionSeqId()

		DECLARE @V_AvalibleItems TABLE (
			[NVPSeqId] INT
			,[Schema_Name] VARCHAR(30)
			,[Static_Name] VARCHAR(30)
			,[Display] VARCHAR(128)
			,[Description] VARCHAR(256)
			,[StatusSeqId] INT
			,[Added_By] INT
			,[Added_Date] DATETIME
			,[Updated_By] INT
			,[Updated_Date] DATETIME
			,[Role] VARCHAR(50)
			)

		IF @P_Debug = 1
			PRINT 'Geting items via roles'

		INSERT INTO @V_AvalibleItems
		SELECT -- Items via roles
			 ZGWSystem.Name_Value_Pairs.[NVPSeqId]
			,ZGWSystem.Name_Value_Pairs.[Schema_Name]
			,ZGWSystem.Name_Value_Pairs.[Static_Name]
			,ZGWSystem.Name_Value_Pairs.[Display]
			,ZGWSystem.Name_Value_Pairs.[Description]
			,ZGWSystem.Name_Value_Pairs.[StatusSeqId]
			,ZGWSystem.Name_Value_Pairs.[Added_By]
			,ZGWSystem.Name_Value_Pairs.[Added_Date]
			,ZGWSystem.Name_Value_Pairs.[Updated_By]
			,ZGWSystem.Name_Value_Pairs.[Updated_Date]
			,ROLES.Name AS [Role]
		FROM ZGWSecurity.Roles_Security_Entities SE_ROLES
			,ZGWSecurity.Roles ROLES
			,ZGWSecurity.Roles_Security_Entities_Permissions [SECURITY]
			,ZGWSystem.Name_Value_Pairs
			,ZGWSecurity.[Permissions] [Permissions]
		WHERE SE_ROLES.RoleSeqId = ROLES.RoleSeqId
			AND SECURITY.RolesSecurityEntitiesSeqId = SE_ROLES.RolesSecurityEntitiesSeqId
			AND SECURITY.NVPSeqId = ZGWSystem.Name_Value_Pairs.NVPSeqId
			AND [Permissions].NVP_DetailSeqId = SECURITY.PermissionsNVPDetailSeqId
			AND [Permissions].NVP_DetailSeqId = @V_Permission_Id
			AND SE_ROLES.SecurityEntitySeqId IN (
				SELECT SecurityEntitySeqId
				FROM ZGWSecurity.Get_Entity_Parents(1, @P_SecurityEntitySeqId)
				)

		IF @P_Debug = 1
			PRINT 'Getting items via groups'

		INSERT INTO @V_AvalibleItems
		SELECT -- Items via groups
			ZGWSystem.Name_Value_Pairs.NVPSeqId
			,ZGWSystem.Name_Value_Pairs.[Schema_Name]
			,ZGWSystem.Name_Value_Pairs.Static_Name
			,ZGWSystem.Name_Value_Pairs.Display
			,ZGWSystem.Name_Value_Pairs.[Description]
			,ZGWSystem.Name_Value_Pairs.StatusSeqId
			,ZGWSystem.Name_Value_Pairs.Added_By
			,ZGWSystem.Name_Value_Pairs.Added_Date
			,ZGWSystem.Name_Value_Pairs.Updated_By
			,ZGWSystem.Name_Value_Pairs.Updated_Date
			,ROLES.[Name] AS [Role]
		FROM ZGWSecurity.Groups_Security_Entities_Permissions
			,ZGWSecurity.Groups_Security_Entities
			,ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
			,ZGWSecurity.Roles_Security_Entities
			,ZGWSecurity.Roles ROLES
			,ZGWSystem.Name_Value_Pairs
			,ZGWSecurity.[Permissions] [Permissions]
		WHERE ZGWSecurity.Groups_Security_Entities_Permissions.NVPSeqId = ZGWSystem.Name_Value_Pairs.NVPSeqId
			AND ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Permissions.GroupsSecurityEntitiesSeqId
			AND ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId
			AND ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.RolesSecurityEntitiesSeqId
			AND ROLES.RoleSeqId = ZGWSecurity.Roles_Security_Entities.RoleSeqId
			AND [Permissions].NVP_DetailSeqId = ZGWSecurity.Groups_Security_Entities_Permissions.PermissionsNVPDetailSeqId
			AND [Permissions].NVP_DetailSeqId = @V_Permission_Id
			AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (
				SELECT SecurityEntitySeqId
				FROM ZGWSecurity.Get_Entity_Parents(1, @P_SecurityEntitySeqId)
				)

		DECLARE @V_AccountRoles TABLE (Roles VARCHAR(30))

		-- Roles belonging to the account
		IF @P_Debug = 1
			PRINT 'Getting roles for account and roles via groups'

		INSERT INTO @V_AccountRoles
		SELECT -- Roles via roles
			ZGWSecurity.Roles.[Name] AS Roles
		FROM ZGWSecurity.Accounts
			,ZGWSecurity.Roles_Security_Entities_Accounts
			,ZGWSecurity.Roles_Security_Entities
			,ZGWSecurity.Roles
		WHERE ZGWSecurity.Roles_Security_Entities_Accounts.AccountSeqId = @P_AccountSeqId
			AND ZGWSecurity.Roles_Security_Entities_Accounts.RolesSecurityEntitiesSeqId = ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId
			AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId IN (
				SELECT SecurityEntitySeqId
				FROM ZGWSecurity.Get_Entity_Parents(1, @P_SecurityEntitySeqId)
				)
			AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
		
		UNION
		
		SELECT -- Roles via groups
			ZGWSecurity.Roles.[Name] AS Roles
		FROM ZGWSecurity.Accounts
			,ZGWSecurity.Groups_Security_Entities_Accounts
			,ZGWSecurity.Groups_Security_Entities
			,ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
			,ZGWSecurity.Roles_Security_Entities
			,ZGWSecurity.Roles
		WHERE ZGWSecurity.Groups_Security_Entities_Accounts.AccountSeqId = @P_AccountSeqId
			AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (
				SELECT SecurityEntitySeqId
				FROM ZGWSecurity.Get_Entity_Parents(1, @P_SecurityEntitySeqId)
				)
			AND ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.GroupsSecurityEntitiesSeqId
			AND ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.RolesSecurityEntitiesSeqId
			AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId

		DECLARE @V_AllItems TABLE (
			[NVPSeqId] INT
			,[Schema_Name] VARCHAR(30)
			,[Static_Name] VARCHAR(30)
			,[Display] VARCHAR(128)
			,[Description] VARCHAR(256)
			,[StatusSeqId] INT
			,[Added_By] INT
			,[Added_Date] DATETIME
			,[Updated_By] INT
			,[Updated_Date] DATETIME
			)

		IF @P_Debug = 1
			PRINT 'Putting all items into tabable variable'

		INSERT INTO @V_AllItems
		SELECT -- Last but not least get the menu items when there are matching account roles.
			NVPSeqId
			,[Schema_Name]
			,Static_Name
			,Display
			,[Description]
			,[STATUS_SEQ_ID] = [StatusSeqId]
			,Added_By
			,Added_Date
			,Updated_By
			,Updated_Date
		FROM @V_AvalibleItems
		WHERE [Role] IN (
				SELECT DISTINCT *
				FROM @V_AccountRoles
				)

		DECLARE @V_DistinctItems TABLE (
			[NVPSeqId] INT
			,[Schema_Name] VARCHAR(30)
			,[Static_Name] VARCHAR(30)
			,[Display] VARCHAR(128)
			,[Description] VARCHAR(256)
			,[StatusSeqId] INT
			,[Added_By] INT
			,[Added_Date] DATETIME
			,[Updated_By] INT
			,[Updated_Date] DATETIME
			)

		IF @P_Debug = 1
			PRINT 'Getting disting items into table variable'

		INSERT INTO @V_DistinctItems
		SELECT DISTINCT 
			 [NVPSeqId]
			,[Schema_Name]
			,[Static_Name]
			,[Display]
			,[Description]
			,[StatusSeqId]
			,[Added_By]
			,[Added_Date]
			,[Updated_By]
			,[Updated_Date]
		FROM @V_AllItems

		IF @P_Debug = 1
			PRINT 'Selecting all distint items for account'

		SELECT NVPSeqId AS NVP_SEQ_ID
			,[Schema_Name]
			,Static_Name
			,Display
			,[Description]
			,[STATUS_SEQ_ID] = [StatusSeqId]
			,Added_By
			,Added_Date
			,Updated_By
			,Updated_Date
		FROM @V_DistinctItems
		ORDER BY Static_Name
	END
	ELSE
	BEGIN
		-- get only valid NVP for the given account
		IF @P_Debug = 1
			PRINT 'get only valid NVP for the given account'

		SELECT [NVPSeqId] AS NVP_SEQ_ID
			,[Schema_Name]
			,Static_Name
			,Display
			,[Description]
			,[STATUS_SEQ_ID] = [StatusSeqId]
			,Added_By
			,Added_Date
			,Updated_By
			,Updated_Date
		FROM ZGWSystem.Name_Value_Pairs
		ORDER BY Static_Name
	END
END

IF @P_Debug = 1
	PRINT 'Ending ZGWSecurity.Get_Function_Roles'

RETURN 0
GO

/****** End: Procedure [ZGWSystem].[Get_Name_Value_Pair] ******/

/****** Start: Procedure [ZGWSystem].[Set_Name_Value_Pair] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWSystem].[Set_Name_Value_Pair]') AND type in (N'P', N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSystem].[Name_ValuSet_Name_Value_Paire_Pairs] AS'
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
-- Author:		Michael Regan
-- Create date: 04/12/2024
-- Description:	Changed to return a data row
-- =============================================
ALTER PROCEDURE [ZGWSystem].[Set_Name_Value_Pair]
	@P_NVPSeqId int,
	@P_Schema_Name VARCHAR(30),
	@P_Static_Name VARCHAR(30),
	@P_Display VARCHAR(128),
	@P_Description VARCHAR(256),
	@P_StatusSeqId INT,
	@P_Added_Updated_By INT,
	@P_ErrorCode int OUTPUT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting [ZGWSystem].[Set_Name_Value_Pair]'
	DECLARE @V_Now DATETIME = GETDATE()
			, @V_PrimaryKey int = -1;
	SET @V_PrimaryKey = @P_NVPSeqId;
	IF @P_NVPSeqId > -1
		BEGIN
	-- UPDATE PROFILE
	UPDATE ZGWSystem.Name_Value_Pairs
		SET 
			[Display] = @P_Display,
			[Description] = @P_Description,
			[StatusSeqId] = @P_StatusSeqId,
			[Updated_By] = @P_Added_Updated_By,
			[Updated_Date] = @V_Now
		WHERE
			[NVPSeqId] = @P_NVPSeqId
END
	ELSE
	BEGIN
	-- INSERT a new row in the table.

	-- CHECK FOR DUPLICATE NAME BEFORE INSERTING
	IF EXISTS(SELECT Static_Name FROM [ZGWSystem].[Name_Value_Pairs] WHERE Static_Name = @P_Static_Name)
		BEGIN
			RAISERROR ('The name value pair already exists in the database.',16,1)
			SELECT @P_ErrorCode=1
			RETURN
		END
	-- END IF
	IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID('[' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR(MAX),@P_Static_Name) + ']') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
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
	-- END IF
	INSERT ZGWSystem.Name_Value_Pairs(
		[Schema_Name],
		[Static_Name],
		[Display],
		[Description],
		[StatusSeqId],
		[Added_By],
		[Added_Date]
	) VALUES (
		@P_Schema_Name,
		@P_Static_Name,
		@P_Display,
		@P_Description,
		@P_StatusSeqId,
		@P_Added_Updated_By,
		@V_Now
	)
	SELECT @V_PrimaryKey=SCOPE_IDENTITY()
-- Get the IDENTITY value for the row just inserted.
END
-- Get the Error Code for the statement just executed.
SELECT
	  [nvpSeqId] = [NVPSeqId]
	, [schemaName] = [Schema_Name]
	, [staticName] = [Static_Name]
	, [display]
	, [description]
	, [StatusSeqId]
	, [Added_By]
	, [Added_Date]
	, [Updated_By]
	, [Updated_Date]
FROM
	[ZGWSystem].[Name_Value_Pairs]
WHERE
	[NVPSeqId] = @V_PrimaryKey
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
	SET @V_TableName = (
		SELECT [Schema_Name] + '.' + Static_Name
		FROM ZGWSystem.Name_Value_Pairs
		WHERE NVPSeqId = @P_NVPSeqId
	)
	SET @V_Statement = '
SELECT 
	  [NVP_DetailSeqId] AS [NVP_SEQ_DET_ID]
	, [NVPSeqId] as NVP_SEQ_ID
	, [NVP_Detail_Name] AS [NVP_DET_TEXT]
	, [NVP_Detail_Value] AS [NVP_DET_VALUE]
	, [StatusSeqId] AS [STATUS_SEQ_ID]
	, [Sort_Order] AS [SORT_ORDER]
	, [Added_By]
	, [Added_Date]
	, [Updated_By]
	, [Updated_Date] 
FROM ' + CONVERT(VARCHAR,@V_TableName) + '
WHERE
	NVP_DetailSeqId = ' + CONVERT(VARCHAR,@P_NVP_DetailSeqId)
	IF @P_Debug = 1 PRINT @V_Statement;
	EXECUTE dbo.sp_executesql @statement = @V_Statement
RETURN 0

GO
/****** END: StoredProcedure [ZGWSystem].[Get_Name_Value_Pair_Detail] ******/

-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '3.1.0.0',
    [Updated_By] = 3,
    [Updated_Date] = getdate()