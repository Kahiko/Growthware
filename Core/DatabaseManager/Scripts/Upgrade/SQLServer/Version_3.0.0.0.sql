-- Upgrade
/****** Object:  Table [ZGWSystem].[Logging]    Script Date: 7/4/2022 10:50:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSystem].[Logging]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSystem].[Logging]
	(
		[Account] [varchar](128) NOT NULL,
		[Component] [varchar](50) NOT NULL,
		[ClassName] [varchar](50) NOT NULL,
		[Level] [varchar](5) NOT NULL,
		[LogDate] [datetime] NULL,
		[LogSeqId] [int] IDENTITY(1,1) NOT NULL,
		[MethodName] [varchar](50) NOT NULL,
		[Msg] [nvarchar](max) NOT NULL,
		CONSTRAINT [CI_ZGWSystem.Logging] UNIQUE CLUSTERED ( [LogSeqId] ASC )
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
-- 0 = FALSE 1 = TRUE
DECLARE @V_Action VARCHAR(256)          = ''
    , @V_Debug int                      = 0
	, @V_Description VARCHAR(512)		= ''
	, @V_Controller VARCHAR(512)		= ''
	, @V_EnableNotificationsFalse INT	= 0
	, @V_EnableViewStateFalse INT		= 0
    , @V_FunctionID INT                 = -1
    , @V_FunctionTypeSeqId INT			= (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Security')
	, @V_IsNavFalse INT					= 0
	, @V_LinkBehaviorInternal INT		= (SELECT [NVP_DetailSeqId] FROM [ZGWCoreWeb].[Link_Behaviors] WHERE [NVP_Detail_Value] = 'Internal')
	, @V_META_KEY_WORDS VARCHAR(512)	= ''
	, @V_Name VARCHAR(30)				= ''
	, @V_NAV_TYPE INT					= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Navigation_Types WHERE NVP_Detail_Value = 'Hierarchical')
	, @V_Notes VARCHAR(512)				= ''
	, @V_NO_UI INT						= 1
    , @V_ParentID INT					= NULL
	, @V_Redirect_On_Timeout INT		= 0
	, @V_Resolve VARCHAR(MAX)			= NULL
	, @V_Source VARCHAR(512)			= 'None'
    , @V_SystemID INT					= (SELECT AccountSeqId from ZGWSecurity.Accounts where Account = 'System')
    , @V_Permission_Add INT				= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Add')
    , @V_Permission_Delete INT			= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Delete')
    , @V_Permission_Edit INT			= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Edit')
    , @V_Permission_View INT			= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'View')

-- Setup the values for the new/updated Function
PRINT 'Adding SaveAccount'
IF NOT EXISTS (SELECT TOP(1) 1 FROM [ZGWSecurity].[Functions] WHERE [Action] = 'SaveAccount')
	BEGIN
		SET @V_FunctionID = -1;
		SET @V_Description = 'A security entry to determine who can Save and Account (not their own)';
		SET @V_Action = 'SaveAccount';
		SET @V_Name = 'Save Account';
		SET @V_Notes = 'Used as a security holder to determine who can Save and Account (not their own).';
		EXEC ZGWSecurity.Set_Function @V_FunctionID, @V_Name, @V_Description, @V_FunctionTypeSeqId, @V_Source, @V_Controller, @V_Resolve, @V_EnableViewStateFalse, @V_EnableNotificationsFalse, @V_Redirect_On_Timeout, @V_IsNavFalse, @V_LinkBehaviorInternal, @V_NO_UI, @V_NAV_TYPE, @V_Action, @V_META_KEY_WORDS, @V_ParentID, @V_Notes, @V_SystemID, @V_Debug;
		-- Set the Security for the new/updated Function
		SET @V_FunctionID = (select FunctionSeqId from ZGWSecurity.Functions where action=@V_Action)
		EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID, 1, 'Developer', @V_Permission_Add, @V_SystemID, @V_Debug
		EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID, 1, 'Developer', @V_Permission_Edit, @V_SystemID, @V_Debug
	END
--END IF

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('ZGWSecurity.Get_Menu_Data'))
   exec('CREATE PROCEDURE [ZGWSecurity].[Get_Menu_Data] AS BEGIN SET NOCOUNT ON; END')
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [ZGWSecurity].[vwSearchGroups] AS
	SELECT
		G.[GroupSeqId],
		G.[Name],
		G.[Description],
		Added_By = (SELECT Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = G.Added_By),
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
		R.[RoleSeqId],
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
-- Description:	Added Link_Behavior and Source to the output
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Menu_Data] @P_SecurityEntitySeqId INT
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
	,[Source] VARCHAR(512)
);

INSERT INTO @V_AvalibleItems
SELECT -- Menu items via roles
	[FUNCTIONS].FunctionSeqId AS [ID]
	,[FUNCTIONS].[Name] AS [Title]
	,[FUNCTIONS].[Description]
	,[FUNCTIONS].[Action] AS [URL]
	,[FUNCTIONS].[ParentSeqId] AS [Parent_Id]
	,[FUNCTIONS].[Sort_Order] AS [Sort_Order]
	,[ROLES].[Name] AS [ROLE]
	,[FUNCTIONS].[FunctionTypeSeqId]
	,[FUNCTIONS].[Link_Behavior]
	,[FUNCTIONS].[Source]
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
	,[FUNCTIONS].[Name] AS [Title]
	,[FUNCTIONS].[Description]
	,[FUNCTIONS].[Action] AS URL
	,[FUNCTIONS].[ParentSeqId] AS [Parent_Id]
	,[FUNCTIONS].[Sort_Order] AS [Sort_Order]
	,ROLES.[Name] AS [ROLE]
	,[FUNCTIONS].[FunctionTypeSeqId]
	,[FUNCTIONS].[Link_Behavior]
	,[FUNCTIONS].[Source]
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
	,[Source] VARCHAR(512)
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
	,[Source]
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
	,[Source] VARCHAR(512)
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
	,[Source]
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
	,[Source] AS [Link]
FROM @V_DistinctItems
ORDER BY Parent_Id
	,Sort_Order
	,Title
	,ID

GO

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
		@P_Name VARCHAR(50) = 'Test',
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
-- Author:		Michael Regan
-- Modified date: 09/24/2023
-- Description:	Was not return the correct value for P_Primary_Key when updating
-- =============================================
ALTER PROCEDURE [ZGWCoreWeb].[Set_Message] @P_MessageSeqId INT
	,@P_SecurityEntitySeqId INT
	,@P_Name VARCHAR(50)
	,@P_Title VARCHAR(100)
	,@P_Description VARCHAR(512)
	,@P_Body VARCHAR(MAX)
	,@P_Format_As_HTML INT
	,@P_Added_Updated_By INT
	,@P_Primary_Key INT OUTPUT
	,@P_Debug INT = 0
AS
IF @P_Debug = 1
	PRINT 'Starting ZGWSecurity.Set_Message'

DECLARE @V_Now DATETIME = GETDATE()

IF @P_MessageSeqId > - 1
BEGIN
	-- UPDATE PROFILE
	-- CHECK FOR DUPLICATE Name BEFORE INSERTING
	IF EXISTS (SELECT [Name] FROM ZGWCoreWeb.[Messages] WHERE [Name] = @P_Name AND SecurityEntitySeqId = @P_SecurityEntitySeqId)
		BEGIN
			UPDATE ZGWCoreWeb.[Messages]
				SET SecurityEntitySeqId = @P_SecurityEntitySeqId
					,[Name] = @P_Name
					,Title = @P_Title
					,[Description] = @P_Description
					,Format_As_HTML = @P_Format_As_HTML
					,Body = @P_Body
					,Updated_By = @P_Added_Updated_By
					,Updated_Date = GETDATE()
				WHERE MessageSeqId = @P_MessageSeqId
					AND SecurityEntitySeqId = @P_SecurityEntitySeqId
			-- set the output id just in case.
			SELECT 
				@P_Primary_Key = [MessageSeqId]
			FROM 
				ZGWCoreWeb.[Messages] 
			WHERE 
				[Name] = @P_Name AND SecurityEntitySeqId = @P_SecurityEntitySeqId
		END
	ELSE
		BEGIN
			INSERT ZGWCoreWeb.[Messages] (
					SecurityEntitySeqId
				,[Name]
				,Title
				,[Description]
				,Body
				,Format_As_HTML
				,Added_By
				,Added_Date
			) VALUES (
					@P_SecurityEntitySeqId
				,@P_Name
				,@P_Title
				,@P_Description
				,@P_Body
				,@P_Format_As_HTML
				,@P_Added_Updated_By
				,@V_Now
			);
			-- Get the IDENTITY value for the row just inserted.
			SELECT @P_Primary_Key = SCOPE_IDENTITY()
		END
	END
ELSE
	BEGIN
		-- INSERT a new row in the table.
		-- CHECK FOR DUPLICATE Name BEFORE INSERTING
		IF EXISTS (SELECT [Name] FROM ZGWCoreWeb.[Messages] WHERE [Name] = @P_Name AND SecurityEntitySeqId = @P_SecurityEntitySeqId)
			BEGIN
				RAISERROR ('The message you entered already exists in the database.',16,1);
				RETURN
			END
		-- END IF
		INSERT ZGWCoreWeb.[Messages] (
			 SecurityEntitySeqId
			,[Name]
			,Title
			,[Description]
			,Body
			,Format_As_HTML
			,Added_By
			,Added_Date
		) VALUES (
			 @P_SecurityEntitySeqId
			,@P_Name
			,@P_Title
			,@P_Description
			,@P_Body
			,@P_Format_As_HTML
			,@P_Added_Updated_By
			,@V_Now
			);

		SELECT @P_Primary_Key = SCOPE_IDENTITY()
			-- Get the IDENTITY value for the row just inserted.
	END
-- END IF
IF @P_Debug = 1
	PRINT 'Ending ZGWSecurity.Set_Message'

GO
/****** Start:  UserDefinedFunction [ZGWSecurity].[udfSplit] ******/
IF EXISTS (
		SELECT *
		FROM sys.objects
		WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[udfSplit]')
			AND type IN (N'FN' ,N'IF' ,N'TF' ,N'FS' ,N'FT')
	)
	BEGIN
		DROP FUNCTION [ZGWSecurity].[udfSplit];
	END
--END IF
GO

CREATE FUNCTION [ZGWSecurity].[udfSplit] (
	@P_Text VARCHAR(MAX)
	,@P_Delimiter NCHAR(1)
	)
RETURNS @V_TblSplitValues TABLE (
	[Id] INT
	,[Data] VARCHAR(50)
	)
AS
BEGIN
	DECLARE @V_AuxString VARCHAR(MAX);

	SET @V_AuxString = REPLACE(@P_Text, @P_Delimiter, '~');

	WITH Split (
		stpos
		,endpos
		)
	AS (
		SELECT 0 AS stpos
			,CHARINDEX('~', @V_AuxString) AS endpos
		
		UNION ALL
		
		SELECT CAST(endpos AS INT) + 1
			,CHARINDEX('~', @V_AuxString, endpos + 1)
		FROM Split
		WHERE endpos > 0
		)
	INSERT @V_TblSplitValues
	SELECT [Id] = ROW_NUMBER() OVER (
			ORDER BY (
					SELECT 1
					)
			)
		,[Data] = SUBSTRING(@V_AuxString, stpos, COALESCE(NULLIF(endpos, 0), LEN(@V_AuxString) + 1) - stpos)
	FROM Split;

	RETURN;
END
GO
/****** End:  UserDefinedFunction [ZGWSecurity]].[udfSplit] ******/
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


/****** Start:  StoredProcedure [ZGWSecurity].[Set_Function_Sort] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('ZGWSecurity.Set_Function_Sort'))
   exec('CREATE PROCEDURE [ZGWSecurity].[Set_Function_Sort] AS BEGIN SET NOCOUNT ON; END')
GO
/*
Usage:
	DECLARE 
		@P_Commaseparated_Ids VARCHAR(50) = '5,4,3,2,1',
		@P_Added_Updated_By INT = 2,
		@P_Primary_Key int,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Function_Sort
		@P_Commaseparated_Ids,
		@P_Added_Updated_By,
		@P_Primary_Key,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/02/2011
-- Description:	Updates ZGWSecurity.Functions Sort_Order column
-- =============================================
-- Author:		Michael Regan
-- Create date: 10/04/2023
-- Description:	Changed to accept all functions at once in order to support a drag and drop frontend
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Function_Sort]
	@P_Commaseparated_Ids VARCHAR(50),
	@P_Added_Updated_By INT,
	@P_Primary_Key INT OUTPUT,
	@P_Debug INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @V_TblSplitValues TABLE (
		[Id] INT
		,[Data] VARCHAR(50)
		,[Processed] BIT
	);
	DECLARE  @V_FunctionSeqId INT
			,@V_Id int;

	INSERT INTO @V_TblSplitValues SELECT [Id], [Data], 0  FROM [ZGWSecurity].[udfSplit](@P_Commaseparated_Ids, ',');
	--SELECT * FROM @V_TblSplitValues;
	WHILE (SELECT COUNT(*) FROM @V_TblSplitValues WHERE Processed = 0) > 0
		BEGIN
			SELECT TOP(1)
				@V_Id = [Id]
				, @V_FunctionSeqId = CONVERT(INT, [Data])
			FROM @V_TblSplitValues WHERE [Processed] = 0 ORDER BY [Id];
			-- PRINT 'FunctionSeqId ' + CONVERT(VARCHAR(10), @V_FunctionSeqId) + ' Sort = ' + CONVERT(VARCHAR(10), @V_Id - 1);
			UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = @V_Id - 1 WHERE [FunctionSeqId] = @V_FunctionSeqId;
			UPDATE @V_TblSplitValues SET [Processed] = 1 WHERE [Id] = @V_Id;
		END
	--END WHILE
END
GO
/****** Start:  Stored Procedure [ZGWSecurity].[Set_Function_Sort] ******/
/****** Start:  StoredProcedure [ZGWSecurity].[Copy_Function_Security] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('ZGWSecurity.Copy_Function_Security'))
   exec('CREATE PROCEDURE [ZGWSecurity].[Copy_Function_Security] AS BEGIN SET NOCOUNT ON; END')
GO
/*
Usage:
	DECLARE 
        @P_Source INT = 1
	  , @P_Target INT = 8
	  , @P_Added_Updated_By INT = 4;

	EXEC [ZGWSecurity].[Copy_Function_Security]
        @P_Source
	  , @P_Target
	  , @P_Added_Updated_By
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 10/22/2023
-- Description:	"Copies the group and role security for all functions"
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Copy_Function_Security]
      @P_Source INT
    , @P_Target INT
    , @P_Added_Updated_By INT
    , @P_Debug INT = 0
AS
BEGIN
    SET NOCOUNT ON;
-- Delete any Roles and Groups associated with the target (rely's on FK settings to delete from subsequent tables
    -- Groups_Security_Entities_Roles_Security_Entities does not have a FK cascade delete so we need to manually delete the records
	DELETE FROM 
        [ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities]
    WHERE 
        [RolesSecurityEntitiesSeqId] IN (SELECT [RolesSecurityEntitiesSeqId] FROM [ZGWSecurity].[Roles_Security_Entities] WHERE [SecurityEntitySeqId] =  @p_Target)

    DELETE FROM [ZGWSecurity].[Roles_Security_Entities] WHERE [SecurityEntitySeqId] =  @p_Target
    DELETE FROM [ZGWSecurity].[Groups_Security_Entities] WHERE [SecurityEntitySeqId] =  @p_Target
-- Associate Roles with the target
	INSERT INTO [ZGWSecurity].[Roles_Security_Entities]
	SELECT 
		  @P_Target
		, [RoleSeqId]
		, [Added_By]
		, GETDATE()
	FROM [ZGWSecurity].[Roles_Security_Entities] WITH(NOLOCK)
	WHERE [SecurityEntitySeqId] = @P_Source
-- Associate Groups with the target
	INSERT INTO [ZGWSecurity].[Groups_Security_Entities]
	SELECT 
		  @P_Target
		, [GroupsSecurityEntitiesSeqId]
		, [Added_By]
		, GETDATE()
	FROM [ZGWSecurity].[Groups_Security_Entities] WITH(NOLOCK)
	WHERE [SecurityEntitySeqId] = @P_Source
-- Associatet Roles with the Functions
	INSERT INTO [ZGWSecurity].[Roles_Security_Entities_Functions]
	SELECT
		 [RolesSecurityEntitiesSeqId] = (SELECT [RolesSecurityEntitiesSeqId] FROM [ZGWSecurity].[Roles_Security_Entities] WHERE [SecurityEntitySeqId] = @P_Target AND [RoleSeqId] = RSE.[RoleSeqId])
		,RSEF.[FunctionSeqId]
		,RSEF.[PermissionsNVPDetailSeqId]
		,@P_Added_Updated_By
		,GETDATE()
	FROM [ZGWSecurity].[Roles_Security_Entities] RSE
		INNER JOIN [ZGWSecurity].[Roles_Security_Entities_Functions] RSEF ON 1=1
			AND RSE.[SecurityEntitySeqId] = @P_Source
			AND RSEF.[RolesSecurityEntitiesSeqId] = RSE.[RolesSecurityEntitiesSeqId]
-- Associate Groups with the Functions
	INSERT INTO [ZGWSecurity].[Groups_Security_Entities_Functions]
	SELECT
		 [GroupsSecurityEntitiesSeqId] = (SELECT [GroupsSecurityEntitiesSeqId] FROM [ZGWSecurity].[Groups_Security_Entities_Functions] WHERE [SecurityEntitySeqId] = @P_Target AND [GroupSeqId] = GSE.[GroupSeqId])
		,GSEF.[FunctionSeqId]
		,GSEF.[PermissionsNVPDetailSeqId]
		,@P_Added_Updated_By
		,GETDATE()
	FROM [ZGWSecurity].[Groups_Security_Entities] GSE
		INNER JOIN [ZGWSecurity].[Groups_Security_Entities_Functions] GSEF ON 1=1
			AND GSE.[SecurityEntitySeqId] = @P_Source
			AND GSEF.[GroupsSecurityEntitiesSeqId] = GSE.[GroupsSecurityEntitiesSeqId]
END

GO
/****** End:  	StoredProcedure [ZGWSecurity].[Copy_Function_Security] ******/
DECLARE @V_Now datetime,
		@V_SystemID INT = (SELECT AccountSeqId FROM ZGWSecurity.Accounts where Account = 'System'),
		@V_MyAction VARCHAR(256),
		@V_FunctionID INT,
		@V_ViewPermission   INT = (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'View'),
		@V_AddPermission    INT = (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Add'),
		@V_EditPermission   INT = (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Edit'),
		@V_DeletePermission INT	= (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'Delete'),
		@V_Debug int = 0;

SET @V_FunctionID = (SELECT FunctionTypeSeqId FROM ZGWSecurity.Function_Types WHERE [Name] = 'Module');

SET @V_MyAction = 'EditMessage';
SET @V_FunctionID = (SELECT FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction);

EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_AddPermission,@V_SystemID, @V_Debug;
EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID, @V_Debug;

SET @V_MyAction = 'FunctionSecurity';
SET @V_FunctionID = (SELECT FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction);
EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_AddPermission,@V_SystemID, @V_Debug;
EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_EditPermission,@V_SystemID, @V_Debug;
EXEC ZGWSecurity.Set_Function_Roles @V_FunctionID,1,'Developer',@V_DeletePermission,@V_SystemID, @V_Debug;

-- Update ZGWSecurity.Functions data
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'accounts' WHERE [Action] = 'Search_Accounts';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'functions' WHERE [Action] = 'Search_Functions';
UPDATE [ZGWSecurity].[Functions] SET [Action] = '/functions/copyfunctionsecurity' WHERE [Action] = 'CopyFunctionSecurity';
UPDATE [ZGWSecurity].[Functions] SET [Action] = '/accounts/Edit-My-Account' WHERE [Action] = 'EditAccount';
UPDATE [ZGWSecurity].[Functions] SET [Action] = '/accounts/change-password' WHERE [Action] = 'changepassword';
UPDATE [ZGWSecurity].[Functions] SET [Action] = '/accounts/selectpreferences' WHERE [Action] = 'SelectPreferences';
UPDATE [ZGWSecurity].[Functions] SET [Action] = '/accounts/updateanonymousprofile' WHERE [Action] = 'UpdateAnonymousProfile';
UPDATE [ZGWSecurity].[Functions] SET [Action] = '/security/random-numbers' WHERE [Action] = 'RandomNumbers';
UPDATE [ZGWSecurity].[Functions] SET [Action] = '/security/guid_helper' WHERE [Action] = 'guidhelper';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'security' WHERE [Action] = 'Encryption_Helper';
UPDATE [ZGWSecurity].[Functions] SET [Action] = 'search_security_entities' WHERE [Action] = 'Search_Security_Entities';
UPDATE [ZGWSecurity].[Functions] SET [Action] = '/sys_admin/linecount' WHERE [Action] = 'LineCount';
UPDATE [ZGWSecurity].[Functions] SET [Action] = '/sys_admin/editdbinformation' WHERE [Action] = 'EditDBInformation';
UPDATE [ZGWSecurity].[Functions] SET [Action] = '/accounts/logout' WHERE [Action] = 'Logoff';
UPDATE [ZGWSecurity].[Functions] SET [Action] = '/accounts/logon' WHERE [Action] = 'Logon';
UPDATE [ZGWSecurity].[Functions] SET [Action] = '/accounts/register' WHERE [Action] = 'Register';
UPDATE [ZGWSecurity].[Functions] SET [Link_Behavior] = 2 WHERE [Action] = 'Logon';
UPDATE [ZGWSecurity].[Functions] SET [Source] = 'functions' WHERE [Action] = 'MSNewPage';
UPDATE [GrowthWare].[ZGWCoreWeb].[Account_Choices] SET [FavoriteAction] = 'generic_home' WHERE [Account] = 'Anonymous'
UPDATE [ZGWSecurity].[Functions] SET [Is_Nav] = 0 WHERE [Action] = 'WorkFlows';

-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '3.0.0.0',
    [Updated_By] = 3,
    [Updated_Date] = getdate()
--WHERE [Version] = '2.0.0.0'

