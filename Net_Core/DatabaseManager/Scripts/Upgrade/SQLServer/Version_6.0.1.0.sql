-- Upgrade from 6.0.0.0 to 6.0.1.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
GO

/*
Usage:
DECLARE 
	@P_Is_System_Admin bit = 1,
	@P_Account VARCHAR(128) = 'Developer',
	@P_SecurityEntitySeqId INT = 1,
	@P_Debug INT = 1

EXEC  ZGWSecurity.Get_Account
	@P_Is_System_Admin,
	@P_Account,
	@P_SecurityEntitySeqId,
	@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/08/2011
-- Description:	Selects 1 or all records from [ZGWSecurity].[Get_Account]
--	from ZGWSecurity.Accounts
-- =============================================
-- Author:		Michael Regan
-- Create date: 05/25/2025
-- Description:	Now returns multiple tables when requesting a 
-- single acccount from:
--	[ZGWSecurity].[RefreshTokens]
--	[ZGWSecurity].[Get_Account_Roles]
--	[ZGWSecurity].[Get_Account_Groups]
--	[ZGWSecurity].[Get_Account_Security]
-- =============================================
CREATE OR ALTER PROCEDURE [ZGWSecurity].[Get_Account] 
	 @P_Is_System_Admin BIT
	,@P_Account VARCHAR(128)
	,@P_SecurityEntitySeqId INT
	,@P_Debug INT = 0
AS
SET NOCOUNT ON;

-- SELECT all rows from the table.
IF LEN(RTRIM(LTRIM(@P_Account))) = 0
	BEGIN
		IF @P_Is_System_Admin = 1
			BEGIN
				IF @P_Debug = 1 PRINT 'Selecting all accounts'
				SELECT 
					 [AccountSeqId] AS [ACCT_SEQ_ID]
					,[Account] AS [ACCT]
					,[Email]
					,[Enable_Notifications]
					,[Is_System_Admin]
					,[StatusSeqId] AS [STATUS_SEQ_ID]
					,[Password_Last_Set]
					,[Password] AS [PWD]
					,[Failed_Attempts]
					,[First_Name]
					,[Last_Login]
					,[Last_Name]
					,[Location]
					,[Middle_Name]
					,[Preferred_Name]
					,[Time_Zone]
					,[Added_By]
					,[Added_Date]
					,[Updated_By]
					,[Updated_Date]
				FROM [ZGWSecurity].[Accounts] WITH (NOLOCK)
				ORDER BY [Account] ASC
			END
		ELSE
			BEGIN
				IF @P_Debug = 1 PRINT 'Selecting all accounts for Entity ' + CONVERT(VARCHAR(MAX), @P_SecurityEntitySeqId)
				DECLARE @V_Accounts TABLE (
					 [AccountSeqId] INT
					,[Account] VARCHAR(100)
					,[Email] VARCHAR(100)
					,[Enable_Notifications] BIT
					,[Is_System_Admin] INT
					,[StatusSeqId] INT
					,[Password_Last_Set] DATETIME
					,[Password] VARCHAR(256)
					,[Failed_Attempts] INT
					,[First_Name] VARCHAR(30)
					,[Last_Login] DATETIME
					,[Last_Name] VARCHAR(30)
					,[Location] VARCHAR(100)
					,[Middle_Name] VARCHAR(30)
					,[Preferred_Name] VARCHAR(100)
					,[Time_Zone] INT
					,[Added_By] INT
					,[Added_Date] DATETIME
					,[Updated_By] INT
					,[Updated_Date] DATETIME
				)

				INSERT INTO @V_Accounts
				SELECT -- Roles via roles
					 [Accounts].AccountSeqId
					,[Accounts].[Account]
					,[Accounts].[Email]
					,[Accounts].[Enable_Notifications]
					,[Accounts].[Is_System_Admin]
					,[Accounts].[StatusSeqId]
					,[Accounts].[Password_Last_Set]
					,[Accounts].[Password]
					,[Accounts].[Failed_Attempts]
					,[Accounts].[First_Name]
					,[Accounts].[Last_Login]
					,[Accounts].[Last_Name]
					,[Accounts].[Location]
					,[Accounts].[Middle_Name]
					,[Accounts].[Preferred_Name]
					,[Accounts].[Time_Zone]
					,[Accounts].[Added_By]
					,[Accounts].[Added_Date]
					,[Accounts].[Updated_By]
					,[Accounts].[Updated_Date]
				FROM [ZGWSecurity].[Accounts] AS [Accounts] WITH (NOLOCK)
					,[ZGWSecurity].[Roles_Security_Entities_Accounts] AS [RSEA] WITH (NOLOCK)
					,[ZGWSecurity].[Roles_Security_Entities] AS [RSE] WITH (NOLOCK)
					,[ZGWSecurity].[Roles] WITH  (NOLOCK)
				WHERE [RSEA].AccountSeqId = [Accounts].[AccountSeqId]
					AND [RSEA].RolesSecurityEntitiesSeqId = [RSE].[RolesSecurityEntitiesSeqId]
					AND [RSE].SecurityEntitySeqId IN (
						SELECT SecurityEntitySeqId
						FROM [ZGWSecurity].[Get_Entity_Parents](1, @P_SecurityEntitySeqId)
					)
					AND [RSE].[RoleSeqId] = [ZGWSecurity].[Roles].[RoleSeqId]
				
				UNION
				
				SELECT -- Roles via groups
					 [Accounts].AccountSeqId
					,[Accounts].[Account]
					,[Accounts].[Email]
					,[Accounts].[Enable_Notifications]
					,[Accounts].[Is_System_Admin]
					,[Accounts].[StatusSeqId]
					,[Accounts].[Password_Last_Set]
					,[Accounts].[Password]
					,[Accounts].[Failed_Attempts]
					,[Accounts].[First_Name]
					,[Accounts].[Last_Login]
					,[Accounts].[Last_Name]
					,[Accounts].[Location]
					,[Accounts].[Middle_Name]
					,[Accounts].[Preferred_Name]
					,[Accounts].[Time_Zone]
					,[Accounts].[Added_By]
					,[Accounts].[Added_Date]
					,[Accounts].[Updated_By]
					,[Accounts].[Updated_Date]
				FROM [ZGWSecurity].[Accounts] AS [Accounts] WITH (NOLOCK)
					,[ZGWSecurity].[Groups_Security_Entities_Accounts] AS [GSEA] WITH (NOLOCK)
					,[ZGWSecurity].[Groups_Security_Entities] AS [GSE] WITH (NOLOCK)
					,[ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities] AS [GSERSE] WITH (NOLOCK)
					,[ZGWSecurity].[Roles_Security_Entities] AS [RSE] WITH (NOLOCK)
					,[ZGWSecurity].[Roles] WITH (NOLOCK)
				WHERE [GSEA].AccountSeqId = [Accounts].[AccountSeqId]
					AND [GSE].SecurityEntitySeqId IN (
						SELECT [SecurityEntitySeqId]
						FROM [ZGWSecurity].[Get_Entity_Parents](1, @P_SecurityEntitySeqId)
					)
					AND [GSE].[GroupsSecurityEntitiesSeqId] = [GSERSE].[GroupsSecurityEntitiesSeqId]
					AND [RSE].[RolesSecurityEntitiesSeqId] = [GSERSE].[RolesSecurityEntitiesSeqId]
					AND [RSE].[RoleSeqId] = [ZGWSecurity].[Roles].[RoleSeqId]

				SELECT DISTINCT 
					 [AccountSeqId] AS [ACCT_SEQ_ID]
					,[Account] AS [ACCT]
					,[Email]
					,[Enable_Notifications]
					,[Is_System_Admin]
					,[StatusSeqId] AS [STATUS_SEQ_ID]
					,[Password_Last_Set]
					,[Password] AS [PWD]
					,[Failed_Attempts]
					,[First_Name]
					,[Last_Login]
					,[Last_Name]
					,[Location]
					,[Middle_Name]
					,[Preferred_Name]
					,[Time_Zone]
					,[Added_By]
					,[Added_Date]
					,[Updated_By]
					,[Updated_Date]
				FROM @V_Accounts
				ORDER BY Account
			END
		-- END IF
	END
ELSE
	BEGIN
		IF @P_Debug = 1 PRINT 'Selecting single account'

		-- SELECT an existing row from the table.
		SELECT 
			 AccountSeqId AS ACCT_SEQ_ID
			,Account AS ACCT
			,Email
			,Enable_Notifications
			,Is_System_Admin
			,StatusSeqId AS STATUS_SEQ_ID
			,Password_Last_Set
			,[Password] AS PWD
			,Failed_Attempts
			,First_Name
			,Last_Login
			,Last_Name
			,Location
			,Middle_Name
			,Preferred_Name
			,Time_Zone
			,Added_By
			,Added_Date
			,Updated_By
			,Updated_Date
		FROM ZGWSecurity.Accounts WITH (NOLOCK)
		WHERE [Account] = @P_Account
        -- [ZGWSecurity].[RefreshTokens]
		SELECT 
			  RT.[RefreshTokenId]
			, RT.[AccountSeqId]
			, RT.[Token]
			, RT.[Expires]
			, RT.[Created]
			, RT.[CreatedByIp]
			, RT.[Revoked]
			, RT.[RevokedByIp]
			, RT.[ReplacedByToken]
			, RT.[ReasonRevoked]
        FROM 
			[ZGWSecurity].[RefreshTokens] RT
        	INNER JOIN [ZGWSecurity].[Accounts] ACCT 
				ON ACCT.[Account] = @P_Account AND RT.AccountSeqId = ACCT.[AccountSeqId]
        ORDER BY [Created] ASC;
		-- [ZGWSecurity].[Get_Account_Roles]
		EXEC [ZGWSecurity].[Get_Account_Roles] @P_Account, @P_SecurityEntitySeqId
		-- [ZGWSecurity].[Get_Account_Groups]
		EXEC [ZGWSecurity].[Get_Account_Groups] @P_Account, @P_SecurityEntitySeqId
		-- [ZGWSecurity].[Get_Account_Security]
		EXEC [ZGWSecurity].[Get_Account_Security] @P_Account, @P_SecurityEntitySeqId
	END
-- END IF
RETURN 0
GO

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '6.0.1.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;