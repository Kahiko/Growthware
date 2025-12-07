-- Downgrade from 6.0.1.0 to 6.0.0.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
GO

/****** Start: [ZGWSecurity].[Get_Account] ******/

/*
Usage:
	DECLARE 
		@P_Is_System_Admin bit = 1,
		@P_Account VARCHAR(128) = 'Developer',
		@P_SecurityEntitySeqId INT = 1,
		@P_Debug INT = 1

	exec  ZGWSecurity.Get_Account
		@P_Is_System_Admin,
		@P_Account,
		@P_SecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/08/2011
-- Description:	Selects 1 or all records from ZGWSecurity.Get_Account
--	from ZGWSecurity.Accounts
-- =============================================
CREATE OR ALTER PROCEDURE [ZGWSecurity].[Get_Account]
	@P_Is_System_Admin Bit,
	@P_Account VARCHAR(128),
	@P_SecurityEntitySeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	-- SELECT all rows from the table.
	IF LEN(RTRIM(LTRIM(@P_Account))) = 0
		BEGIN
	IF @P_Is_System_Admin = 1
				BEGIN
		IF @P_Debug = 1 PRINT 'Selecting all accounts'
		SELECT
			AccountSeqId AS ACCT_SEQ_ID
						, Account AS ACCT
						, Email
						, Enable_Notifications
						, Is_System_Admin
						, StatusSeqId AS STATUS_SEQ_ID
						, Password_Last_Set
						, [Password] AS PWD
						, Failed_Attempts
						, First_Name
						, Last_Login
						, Last_Name
						, Location
						, Middle_Name
						, Preferred_Name
						, Time_Zone
						, Added_By
						, Added_Date
						, Updated_By
						, Updated_Date
		FROM
			[ZGWSecurity].[Accounts] WITH(NOLOCK)
		ORDER BY 
						[Account] ASC
	END
			ELSE
				BEGIN
		IF @P_Debug = 1 PRINT 'Selecting all accounts for Entity ' + CONVERT(VARCHAR(MAX),@P_SecurityEntitySeqId)
		DECLARE @V_Accounts TABLE (
			AccountSeqId INT
						,
			Account VARCHAR(100)
						,
			Email VARCHAR(100)
						,
			Enable_Notifications BIT
						,
			Is_System_Admin INT
						,
			StatusSeqId INT
						,
			Password_Last_Set DATETIME
						,
			[Password] VARCHAR(256)
						,
			Failed_Attempts INT
						,
			First_Name VARCHAR(30)
						,
			Last_Login DATETIME
						,
			Last_Name VARCHAR(30)
						,
			Location VARCHAR(100)
						,
			Middle_Name VARCHAR(30)
						,
			Preferred_Name VARCHAR(100)
						,
			Time_Zone INT
						,
			Added_By INT
						,
			Added_Date DATETIME
						,
			Updated_By INT
						,
			Updated_Date DATETIME)
		INSERT INTO @V_Accounts
					SELECT -- Roles via roles
				Accounts.AccountSeqId
						, Accounts.Account
						, Accounts.Email
						, Accounts.Enable_Notifications
						, Accounts.Is_System_Admin
						, Accounts.StatusSeqId
						, Accounts.Password_Last_Set
						, Accounts.[Password]
						, Accounts.Failed_Attempts
						, Accounts.First_Name
						, Accounts.Last_Login
						, Accounts.Last_Name
						, Accounts.Location
						, Accounts.Middle_Name
						, Accounts.Preferred_Name
						, Accounts.Time_Zone
						, Accounts.Added_By
						, Accounts.Added_Date
						, Accounts.Updated_By
						, Accounts.Updated_Date
			FROM
				ZGWSecurity.Accounts AS Accounts WITH(NOLOCK),
				ZGWSecurity.Roles_Security_Entities_Accounts WITH(NOLOCK),
				ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
				ZGWSecurity.Roles WITH(NOLOCK)
			WHERE
						Roles_Security_Entities_Accounts.AccountSeqId = Accounts.AccountSeqId
				AND Roles_Security_Entities_Accounts.RolesSecurityEntitiesSeqId = Roles_Security_Entities.RolesSecurityEntitiesSeqId
				AND Roles_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
				FROM ZGWSecurity.Get_Entity_Parents(1,@P_SecurityEntitySeqId))
				AND Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
		UNION
			SELECT -- Roles via groups
				Accounts.AccountSeqId
						, Accounts.Account
						, Accounts.Email
						, Accounts.Enable_Notifications
						, Accounts.Is_System_Admin
						, Accounts.StatusSeqId
						, Accounts.Password_Last_Set
						, Accounts.[Password]
						, Accounts.Failed_Attempts
						, Accounts.First_Name
						, Accounts.Last_Login
						, Accounts.Last_Name
						, Accounts.Location
						, Accounts.Middle_Name
						, Accounts.Preferred_Name
						, Accounts.Time_Zone
						, Accounts.Added_By
						, Accounts.Added_Date
						, Accounts.Updated_By
						, Accounts.Updated_Date
			FROM
				ZGWSecurity.Accounts AS Accounts WITH(NOLOCK),
				ZGWSecurity.Groups_Security_Entities_Accounts WITH(NOLOCK),
				ZGWSecurity.Groups_Security_Entities WITH(NOLOCK),
				ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities WITH(NOLOCK),
				ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
				ZGWSecurity.Roles WITH(NOLOCK)
			WHERE
						ZGWSecurity.Groups_Security_Entities_Accounts.AccountSeqId = Accounts.AccountSeqId
				AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId
				FROM ZGWSecurity.Get_Entity_Parents(1,@P_SecurityEntitySeqId))
				AND ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.GroupsSecurityEntitiesSeqId
				AND Roles_Security_Entities.RolesSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.RolesSecurityEntitiesSeqId
				AND Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId

		SELECT DISTINCT
			AccountSeqId AS ACCT_SEQ_ID
						, Account AS ACCT
						, Email
						, Enable_Notifications
						, Is_System_Admin
						, StatusSeqId AS STATUS_SEQ_ID
						, Password_Last_Set
						, [Password] AS PWD
						, Failed_Attempts
						, First_Name
						, Last_Login
						, Last_Name
						, Location
						, Middle_Name
						, Preferred_Name
						, Time_Zone
						, Added_By
						, Added_Date
						, Updated_By
						, Updated_Date
		FROM
			@V_Accounts
		ORDER BY
						Account
	END
-- END IF
END
	ELSE
		BEGIN
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Selecting single account'
	-- SELECT an existing row from the table.
	SELECT
		AccountSeqId AS ACCT_SEQ_ID
				, Account AS ACCT
				, Email
				, Enable_Notifications
				, Is_System_Admin
				, StatusSeqId AS STATUS_SEQ_ID
				, Password_Last_Set
				, [Password] AS PWD
				, Failed_Attempts
				, First_Name
				, Last_Login
				, Last_Name
				, Location
				, Middle_Name
				, Preferred_Name
				, Time_Zone
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM ZGWSecurity.Accounts WITH(NOLOCK)
	WHERE
				[Account] = @P_Account
END
	-- END IF
RETURN 0
GO

/****** End: [ZGWSecurity].[Get_Account] ******/

/****** Start: [ZGWSecurity].[Get_Account_By_Verification_Token] ******/
SET QUOTED_IDENTIFIER OFF
GO

/*
Usage:
	DECLARE 
		@P_VerificationToken NVARCHAR(MAX) = '',
		@P_Debug INT = 1

	EXEC ZGWSecurity.Get_Account_By_Verification_Token
		@P_VerificationToken,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/07/2024
-- Description:	Selects a single account given the VerificationToekn
-- =============================================
CREATE OR ALTER PROCEDURE [ZGWSecurity].[Get_Account_By_Verification_Token]
	@P_VerificationToken NVARCHAR(MAX),
	@P_Debug INT = 0
AS
BEGIN
	SET NOCOUNT ON
	SELECT TOP (1) 
		 [ACCT_SEQ_ID] = ACCT.[AccountSeqId]
		,[ACCT] = ACCT.[Account]
		,ACCT.[Email]
		,ACCT.[Enable_Notifications]
		,ACCT.[Is_System_Admin]
		,ACCT.[StatusSeqId]
		,ACCT.[Password_Last_Set]
		,ACCT.[Password]
		,ACCT.[ResetToken]
		,ACCT.[ResetTokenExpires]
		,ACCT.[Failed_Attempts]
		,ACCT.[First_Name]
		,ACCT.[Last_Login]
		,ACCT.[Last_Name]
		,ACCT.[Location]
		,ACCT.[Middle_Name]
		,ACCT.[Preferred_Name]
		,ACCT.[Time_Zone]
		,ACCT.[VerificationToken]
		,ACCT.[Added_By]
		,ACCT.[Added_Date]
		,ACCT.[Updated_By]
		,ACCT.[Updated_Date]
	FROM [ZGWSecurity].[Accounts] ACCT
    WHERE
        ACCT.[VerificationToken] = @P_VerificationToken;
	RETURN 0
END
GO
/****** End: [ZGWSecurity].[Get_Account_By_Verification_Token] ******/

/****** Start: [ZGWSecurity].[Get_Account_By_Reset_Token] ******/
SET QUOTED_IDENTIFIER ON;
GO
/*
Usage:
	DECLARE 
		@P_ResetToken NVARCHAR(MAX) = '',
		@P_Debug INT = 1

	exec  ZGWSecurity.Get_Account_By_Reset_Token
		@P_ResetToken,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 11/12/2022
-- Description:	Selects a single account given the ResetToken has not expired
-- =============================================
-- Author:			Michael Regan
-- Modified date: 	05/21/2024
-- Description:		Changed ACCT.[Account] to [ACCT] = ACCT.[Account]
-- 					to match the C# code
-- =============================================
CREATE OR ALTER PROCEDURE [ZGWSecurity].[Get_Account_By_Reset_Token]
	@P_ResetToken NVARCHAR(MAX),
	@P_Debug INT = 0
AS
BEGIN
	SET NOCOUNT ON
	SELECT TOP (1) 
		 [ACCT_SEQ_ID] = ACCT.[AccountSeqId]
		,[ACCT] = ACCT.[Account]
		,ACCT.[Email]
		,ACCT.[Enable_Notifications]
		,ACCT.[Is_System_Admin]
		,ACCT.[StatusSeqId]
		,ACCT.[Password_Last_Set]
		,ACCT.[Password]
		,ACCT.[ResetToken]
		,ACCT.[ResetTokenExpires]
		,ACCT.[Failed_Attempts]
		,ACCT.[First_Name]
		,ACCT.[Last_Login]
		,ACCT.[Last_Name]
		,ACCT.[Location]
		,ACCT.[Middle_Name]
		,ACCT.[Preferred_Name]
		,ACCT.[Time_Zone]
		,ACCT.[Added_By]
		,ACCT.[Added_Date]
		,ACCT.[Updated_By]
		,ACCT.[Updated_Date]
	FROM [ZGWSecurity].[Accounts] ACCT
-- var account = _context.Accounts.SingleOrDefault(x => x.ResetToken == token && x.ResetTokenExpires > DateTime.UtcNow);
    WHERE
        ACCT.[ResetToken] = @P_ResetToken
        AND ResetTokenExpires > GETDATE();
	RETURN 0
END
GO
/****** End: [ZGWSecurity].[Get_Account_By_Reset_Token] ******/

/****** Start: [ZGWSecurity].[Get_Account_By_Refresh_Token] ******/
SET QUOTED_IDENTIFIER ON;
GO
/*
Usage:
	DECLARE 
		@P_Token NVARCHAR(MAX) = 'Developer',
		@P_Debug INT = 1

	exec  ZGWSecurity.Get_Account
		@P_Account,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 11/11/2022
-- Description:	Selects a single account given the Token
-- =============================================
CREATE OR ALTER PROCEDURE [ZGWSecurity].[Get_Account_By_Refresh_Token]
	@P_Token NVARCHAR(MAX),
	@P_Debug INT = 0
AS
BEGIN
	SET NOCOUNT ON
	SELECT TOP (1) 
		  ACCT.[AccountSeqId] AS ACCT_SEQ_ID
		, ACCT.[Account] AS ACCT
		, ACCT.[Email]
		, ACCT.[Enable_Notifications]
		, ACCT.[Is_System_Admin]
		, ACCT.[StatusSeqId] AS STATUS_SEQ_ID
		, ACCT.[Password_Last_Set]
		, ACCT.[Password] AS PWD
		, ACCT.[Failed_Attempts]
		, ACCT.[First_Name]
		, ACCT.[Last_Login]
		, ACCT.[Last_Name]
		, ACCT.[Location]
		, ACCT.[Middle_Name]
		, ACCT.[Preferred_Name]
		, ACCT.[Time_Zone]
		, ACCT.[VerificationToken]
		, ACCT.[Added_By]
		, ACCT.[Added_Date]
		, ACCT.[Updated_By]
		, ACCT.[Updated_Date]
	FROM [YourDatabaseName].[ZGWSecurity].[Accounts] ACCT
		INNER JOIN [ZGWSecurity].[RefreshTokens] RT ON
			RT.[Token] = @P_Token
			AND ACCT.[AccountSeqId] = RT.[AccountSeqId]
	RETURN 0
END
GO
/****** End: [ZGWSecurity].[Get_Account_By_Refresh_Token] ******/

/****** Start: [ZGWSecurity].[Get_Function] ******/
SET QUOTED_IDENTIFIER ON;
GO
/*
Usage:

DECLARE 
	 @P_FunctionSeqId INT = 1
	,@P_Debug INT = 1

EXEC [ZGWSecurity].[Get_Function]
	 @P_FunctionSeqId
	,@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/12/2011
-- Description:	Selects function given
--	the FunctionSeqId. When FunctionSeqId = -1
--	all rows in the table are retruned.
-- =============================================
CREATE OR ALTER PROCEDURE [ZGWSecurity].[Get_Function] 
	 @P_FunctionSeqId INT
	,@P_SecurityEntitySeqId INT
	,@P_Debug INT = 0
AS
SET NOCOUNT ON;
IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Function'

IF @P_FunctionSeqId <> - 1
	BEGIN
		-- SELECT an existing row from the table.
		IF @P_Debug = 1 PRINT 'Selecting single record'

		SELECT 
			 [Functions].[FunctionSeqId] AS [FUNCTION_SEQ_ID]
			,[Functions].[Name]
			,[Functions].[Description]
			,[Functions].[FunctionTypeSeqId] AS [FUNCTION_TYPE_SEQ_ID]
			,[Functions].[Source]
			,[Functions].[Controller]
			,[Functions].[Resolve]
			,[Functions].[Enable_View_State]
			,[Functions].[Enable_Notifications]
			,[Functions].[Redirect_On_Timeout]
			,[Functions].[Is_Nav]
			,[Functions].[Link_Behavior]
			,[Functions].[No_UI]
			,[Functions].[Navigation_Types_NVP_DetailSeqId] AS [NAVIGATION_NVP_SEQ_DET_ID]
			,[Functions].[Meta_Key_Words]
			,[Functions].[Action]
			,[Functions].[ParentSeqId] AS [PARENT_FUNCTION_SEQ_ID]
			,[Functions].[Notes]
			,[Functions].[Sort_Order]
			,[Functions].[Added_By]
			,[Functions].[Added_Date]
			,[Functions].[Updated_By]
			,[Functions].[Updated_Date]
		FROM [ZGWSecurity].[Functions] AS [Functions] WITH (NOLOCK)
		WHERE [Functions].[FunctionSeqId] = @P_FunctionSeqId
		ORDER BY [Functions].[Name] ASC
	END
ELSE
	BEGIN
		IF @P_Debug = 1
			PRINT 'Selecting all records'

		SELECT 
			 [Functions].[FunctionSeqId] AS [FUNCTION_SEQ_ID]
			,[Functions].[Name]
			,[Functions].[Description]
			,[Functions].[FunctionTypeSeqId] AS [FUNCTION_TYPE_SEQ_ID]
			,[Functions].[Source]
			,[Functions].[Controller]
			,[Functions].[Resolve]
			,[Functions].[Enable_View_State]
			,[Functions].[Enable_Notifications]
			,[Functions].[Redirect_On_Timeout]
			,[Functions].[Is_Nav]
			,[Functions].[Link_Behavior]
			,[Functions].[No_UI]
			,[Functions].[Navigation_Types_NVP_DetailSeqId] AS [NAVIGATION_NVP_SEQ_DET_ID]
			,[Functions].[Meta_Key_Words]
			,[Functions].[Action]
			,[Functions].[ParentSeqId] AS [PARENT_FUNCTION_SEQ_ID]
			,[Functions].[Notes]
			,[Functions].[Sort_Order]
			,[Functions].[Added_By]
			,[Functions].[Added_Date]
			,[Functions].[Updated_By]
			,[Functions].[Updated_Date]
		FROM [ZGWSecurity].[Functions] AS [Functions] WITH (NOLOCK)
		ORDER BY [Functions].[Name] ASC;

	END
-- END IF
IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Function'

RETURN 0
GO
/****** End: [ZGWSecurity].[Get_Function] ******/

/****** Start: [ZGWSecurity].[Get_Role] ******/
SET QUOTED_IDENTIFIER ON;
GO
/*
Usage:
	DECLARE 
		@P_RoleSeqId AS INT = -1,
		@P_SecurityEntitySeqId AS INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Role
		@P_RoleSeqId,
		@P_SecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description:	Retrieves roles given the
--	the RoleSeqId and SecurityEntitySeqId
-- Note:
--	RoleSeqId of -1 returns all roles.
-- =============================================
CREATE OR ALTER PROCEDURE [ZGWSecurity].[Get_Role]
	@P_RoleSeqId INT,
	@P_SecurityEntitySeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Start ZGWSecurity.Get_Role and SELECT an existing row from the table.'
	IF @P_RoleSeqId > -1 -- SELECT an existing row from the table.
		SELECT
			ZGWSecurity.Roles.[RoleSeqId] AS ROLE_SEQ_ID,
			ZGWSecurity.Roles.[Name],
			ZGWSecurity.Roles.[Description],
			ZGWSecurity.Roles.[Is_System],
			ZGWSecurity.Roles.[Is_System_Only],
			ZGWSecurity.Roles.[Added_By],
			ZGWSecurity.Roles.[Added_Date],
			ZGWSecurity.Roles.[Updated_By],
			ZGWSecurity.Roles.[Updated_Date]
		FROM
			ZGWSecurity.Roles
		WHERE
			RoleSeqId = @P_RoleSeqId
	ELSE -- GET ALL ROLES FOR A GIVEN Security Entity
		IF @P_Debug = 1 PRINT 'GET ALL ROLES FOR A GIVEN Security Entity.'
		SELECT
			ZGWSecurity.Roles.[RoleSeqId] AS ROLE_SEQ_ID,
			ZGWSecurity.Roles.[Name],
			ZGWSecurity.Roles.[Description],
			ZGWSecurity.Roles.[Is_System],
			ZGWSecurity.Roles.[Is_System_Only],
			ZGWSecurity.Roles.[Added_By],
			ZGWSecurity.Roles.[Added_Date],
			ZGWSecurity.Roles.[Updated_By],
			ZGWSecurity.Roles.[Updated_Date]
		FROM
			ZGWSecurity.Roles,
			ZGWSecurity.Roles_Security_Entities
		WHERE
			ZGWSecurity.Roles.RoleSeqId = ZGWSecurity.Roles_Security_Entities.RoleSeqId
			AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId = @P_SecurityEntitySeqId
ORDER BY
			ZGWSecurity.Roles.[Name]
	-- END IF		
	IF @P_Debug = 1 PRINT 'End ZGWSecurity.Get_Role'
RETURN 0
GO
/****** End: [ZGWSecurity].[Get_Role] ******/

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '6.0.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;