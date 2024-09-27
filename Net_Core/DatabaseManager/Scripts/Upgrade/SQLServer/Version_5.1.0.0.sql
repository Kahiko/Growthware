-- Upgrade from 5.0.0.0 to 5.1.0.0
--USE [YourDatabaseName];
GO
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
GO

/****** Start: [ZGWCoreWeb].[Account_Choices2] ******/
IF OBJECT_ID(N'ZGWCoreWeb.Account_Choices2', N'U') IS NOT NULL DROP TABLE [ZGWCoreWeb].[Account_Choices2];
GO
IF OBJECT_ID(N'ZGWCoreWeb.Account_Choices', N'U') IS NOT NULL
    BEGIN
        SELECT * INTO [ZGWCoreWeb].[Account_Choices2]
        FROM (
            SELECT
                  [Account] = [Account]
                , [SecurityEntityId] = [SecurityEntityID]
                , [SecurityEntityName] = [SecurityEntityName]
                , [FavoriteAction] = [FavoriteAction]
                , [RecordsPerPage] = [recordsPerPage]
                , [ColorScheme] = [ColorScheme]
                , [EvenRow] = [AlternatingRowBackColor]
                , [EvenFont] = [HeaderForeColor]
                , [OddRow] = [RowBackColor]
                , [OddFont] = [HeaderForeColor]
                , [HeaderRow] = [HeadColor]
                , [HeaderFont] = [SubHeadColor]
                , [Background] = [BackColor]
            FROM
                [ZGWCoreWeb].[Account_Choices]
        ) as OriginalData;
        -- Drop the original table
        DROP TABLE [ZGWCoreWeb].[Account_Choices];
        SELECT * INTO [ZGWCoreWeb].[Account_Choices] FROM (SELECT * FROM [ZGWCoreWeb].[Account_Choices2]) AS NewData
        -- Create Constraints
        ALTER TABLE [ZGWCoreWeb].[Account_Choices]
            ADD CONSTRAINT [FK_ZGWCore_Account_Choices_ZGWSecurity_Security_Accounts]
                FOREIGN KEY (Account) REFERENCES [ZGWSecurity].[Accounts](Account) ON DELETE CASCADE ON UPDATE CASCADE;
        ALTER TABLE [ZGWCoreWeb].[Account_Choices]
            ADD CONSTRAINT [UK_ZGWCore_Account_Choices] 
                UNIQUE NONCLUSTERED ([Account] ASC);
        IF OBJECT_ID(N'ZGWCoreWeb.Account_Choices2', N'U') IS NOT NULL DROP TABLE [ZGWCoreWeb].[Account_Choices2];
    END
--END IF
GO
--************ [ZGWCoreWeb].[Set_Account_Choice] ************
/*
Usage:
	EXEC ZGWCoreWeb.Set_Account_Choices
		@P_ACCT = N'Anonymous',
		@P_SecurityEntityId = 1,
		@P_SecurityEntityName = 'System',
        @P_ColorScheme = 'Blue',
        @P_EvenRow = '#6699cc',
        @P_EvenFont = 'White',
        @P_OddRow = '#b6cbeb',
        @P_OddFont = 'Black',
        @P_HeaderRow = '#C7C7C7',
        @P_HeaderFont = 'Black',
        @P_Background = '#ffffff',
		@P_FavoriteAction = 'Home',
		@P_recordsPerPage = 5
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates ZGWCoreWeb.Account_Choices based on @P_ACCT
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/26/2024
-- Description: Updated to match [ZGWCoreWeb].[Account_Choices] changes
-- =============================================
CREATE OR ALTER PROCEDURE [ZGWCoreWeb].[Set_Account_Choices]
	@P_Account VARCHAR(128),
	@P_SecurityEntityId int,
	@P_SecurityEntityName VARCHAR(256),
    @P_ColorScheme VARCHAR(15),
    @P_EvenRow VARCHAR(15),
    @P_EvenFont VARCHAR(15),
    @P_OddRow VARCHAR(15),
    @P_OddFont VARCHAR(15),
    @P_HeaderRow VARCHAR(15),
    @P_HeaderFont VARCHAR(15),
    @P_Background VARCHAR(15),
	@P_FavoriteAction VARCHAR(50),
	@P_RecordsPerPage int
AS
-- INSERT a new row in the table.
	IF(SELECT COUNT(*) FROM [ZGWCoreWeb].[Account_Choices] WHERE Account = @P_Account) <= 0
		BEGIN
			INSERT [ZGWCoreWeb].[Account_Choices] (
                  [Account]
                , [SecurityEntityId]
                , [SecurityEntityName]
                , [FavoriteAction]
                , [RecordsPerPage]
                , [ColorScheme]
                , [EvenRow]
                , [EvenFont]
                , [OddRow]
                , [OddFont]
                , [HeaderRow]
                , [HeaderFont]
                , [Background]
			) VALUES (
                  @P_Account
                , @P_SecurityEntityId
                , @P_SecurityEntityName
                , @P_FavoriteAction
                , @P_RecordsPerPage
                , @P_ColorScheme
                , @P_EvenRow
                , @P_EvenFont
                , @P_OddRow
                , @P_OddFont
                , @P_HeaderRow
                , @P_HeaderFont
                , @P_Background
			);
		END
	ELSE
		BEGIN
		UPDATE [ZGWCoreWeb].[Account_Choices]
			SET
                  SecurityEntityId = @P_SecurityEntityId
                , SecurityEntityName = @P_SecurityEntityName
                , FavoriteAction = @P_FavoriteAction
                , RecordsPerPage = @P_RecordsPerPage
                , ColorScheme = @P_ColorScheme
                , EvenRow = @P_EvenRow
                , EvenFont = @P_EvenFont
                , OddRow = @P_OddRow
                , OddFont = @P_OddFont
                , HeaderRow = @P_HeaderRow
                , HeaderFont = @P_HeaderFont
                , Background = @P_Background
			WHERE
				Account=@P_Account;
		END
	-- END IF
-- Get the Error Code for the statement just executed.
--SELECT @P_ErrorCode=@@ERROR
GO
--************ [ZGWCoreWeb].[Get_Account_Choice] ************
/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'Developer',
		@P_Debug INT = 0

	exec ZGWCoreWeb.Get_Account_Choice
		@P_Account ,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/08/2011
-- Description: Gets a record from xx
--	given the Account
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/26/2024
-- Description: Updated to match [ZGWCoreWeb].[Account_Choices] changes
-- =============================================
CREATE OR ALTER PROCEDURE [ZGWCoreWeb].[Get_Account_Choice]
	@P_Account VARCHAR(128),
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF EXISTS(SELECT Account FROM [ZGWCoreWeb].[Account_Choices] WHERE Account = @P_Account)
		BEGIN
            IF @P_Debug = 1 PRINT 'Selecting client choices for ' + CONVERT(VARCHAR(25),@P_Account)
            SELECT
                  [Account]
                , [SecurityEntityId]
                , [SecurityEntityName]
                , [FavoriteAction]
                , [RecordsPerPage]
                , [ColorScheme]
                , [EvenRow]
                , [EvenFont]
                , [OddRow]
                , [OddFont]
                , [HeaderRow]
                , [HeaderFont]
                , [Background]
            FROM 
                [ZGWCoreWeb].[Account_Choices]
            WHERE
                [Account] = @P_Account
        END
	ELSE
		BEGIN
            IF @P_Debug = 1 PRINT 'Selecting client choices for the Anonymous account'
            SELECT
                  [Account]
                , [SecurityEntityId]
                , [SecurityEntityName]
                , [FavoriteAction]
                , [RecordsPerPage]
                , [ColorScheme]
                , [EvenRow]
                , [EvenFont]
                , [OddRow]
                , [OddFont]
                , [HeaderRow]
                , [HeaderFont]
                , [Background]
            FROM 
                [ZGWCoreWeb].[Account_Choices]
            WHERE
                [Account] = 'Anonymous';
        END
    -- END IF

RETURN 0
GO
--************ [ZGWSecurity].[Set_Account] ************
/*
Usage:
	DECLARE 
		@P_AccountSeqId int = -1,
		@P_StatusSeqId int = 1,
		@P_Account VARCHAR(128) = 'test',
		@P_First_Name VARCHAR(15) = 'test',
		@P_Last_Name VARCHAR(15) = 'test',
		@P_Middle_Name VARCHAR(15) = 'test',
		@P_Preferred_Name VARCHAR(50) = 'test',
		@P_Email VARCHAR(128) = 'test@test.com',
		@P_Password VARCHAR(256) = 'test',
		@P_Password_Last_Set datetime = GETDATE(),
		@P_Failed_Attempts int = 0,
		@P_Added_Updated_By int = 1,
		@P_Last_Login datetime = GETDATE(),
		@P_Time_Zone int = -5,
		@P_Location VARCHAR(50) = 'desk',
		@P_Enable_Notifications int = 0,
		@P_Is_System_Admin int = 0,
		@P_VerificationToken VARCHAR(256) = NULL,
		@P_Debug INT = 1
--Insert new
	exec ZGWSecurity.Set_Account 
		@P_AccountSeqId,
		@P_StatusSeqId,
		@P_Account,
		@P_First_Name,
		@P_Last_Name,
		@P_Middle_Name,
		@P_Preferred_Name,
		@P_Email,
		@P_Password,
		@P_Password_Last_Set,
		@P_Failed_Attempts,
		@P_Added_Updated_By,
		@P_Last_Login,
		@P_Time_Zone,
		@P_Location,
		@P_Enable_Notifications,
		@P_Is_System_Admin,
		@P_VerificationToken,
		@P_Debug
--Update
	SET @P_AccountSeqId = (SELECT AccountSeqId FROM ZGWSecurity.Accounts WHERE Account = 'test')
	exec ZGWSecurity.Set_Account
		@P_AccountSeqId,
		@P_StatusSeqId,
		@P_Account,
		@P_First_Name,
		@P_Last_Name,
		@P_Middle_Name,
		@P_Preferred_Name,
		@P_Email,
		@P_Password,
		@P_Password_Last_Set,
		@P_Failed_Attempts,
		@P_Added_Updated_By,
		@P_Last_Login,
		@P_Time_Zone,
		@P_Location,
		@P_Enable_Notifications,
		@P_Is_System_Admin,
		@P_VerificationToken,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates [ZGWSystem].[Account]
--	@P_StatusSeqId's value determines insert/update
--	a value of -1 is insert > -1 performs update
-- =============================================
-- Author:		Michael Regan
-- Create date: 05/10/2024
-- Description:	Adding @P_ResetToken and @P_ResetTokenExpires
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/01/2024
-- Description:	Added check for @P_Password_Last_Set fixes 
--'SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM' error
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/07/2024
-- Description:	Added @P_VerificationToken
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/26/2024
-- Description: Updated to match [ZGWCoreWeb].[Account_Choices] changes
-- =============================================
CREATE OR ALTER PROCEDURE [ZGWSecurity].[Set_Account] @P_AccountSeqId INT OUTPUT
	,@P_StatusSeqId INT
	,@P_Account VARCHAR(128)
	,@P_First_Name VARCHAR(35)
	,@P_Last_Name VARCHAR(35)
	,@P_Middle_Name VARCHAR(35)
	,@P_Preferred_Name VARCHAR(50)
	,@P_Email VARCHAR(128)
	,@P_Password VARCHAR(256)
	,@P_Password_Last_Set DATETIME
	,@P_ResetToken VARCHAR(256)
	,@P_ResetTokenExpires DATETIME
	,@P_Failed_Attempts INT
	,@P_Added_Updated_By INT
	,@P_Last_Login DATETIME
	,@P_Time_Zone INT
	,@P_Location VARCHAR(50)
	,@P_Enable_Notifications INT
	,@P_Is_System_Admin INT
	,@P_VerificationToken VARCHAR (MAX)
	,@P_Debug INT = 0
AS
SET NOCOUNT ON;
IF @P_Debug = 1 PRINT 'Start Set_Account';
IF ISDATE(@P_Password_Last_Set) = 0 SET @P_Password_Last_Set = GETDATE();
IF ISDATE(@P_Last_Login) = 0 SET @P_Last_Login = NULL;

DECLARE @V_SecurityEntityId int
		, @V_SecurityEntityName VARCHAR(256)
		, @V_ColorScheme VARCHAR(15)
		, @V_EvenRow VARCHAR(15)
		, @V_EvenFont VARCHAR(15)
		, @V_OddRow VARCHAR(15)
		, @V_OddFont VARCHAR(15)
		, @V_HeaderRow VARCHAR(15)
		, @V_HeaderFont VARCHAR(15)
		, @V_Background VARCHAR(15)
		, @V_FavoriteAction VARCHAR(50)
		, @V_RecordsPerPage int
		, @V_Default_Account VARCHAR(50)
		, @V_Now DATETIME = GETDATE()

IF @P_AccountSeqId > - 1
BEGIN
	-- UPDATE PROFILE
	IF @P_Debug = 1 PRINT 'UPDATE [ZGWSecurity].[Accounts]';

	UPDATE [ZGWSecurity].[Accounts]
	SET 
		 [Account] = @P_Account
		,[Email] = @P_Email
		,[Enable_Notifications] = @P_Enable_Notifications
		,[Failed_Attempts] = @P_Failed_Attempts
		,[First_Name] = @P_First_Name
		,[Is_System_Admin] = @P_Is_System_Admin
		,[Last_Login] = @P_Last_Login
		,[Last_Name] = @P_Last_Name
		,[Location] = @P_Location
		,[Middle_Name] = @P_Middle_Name
		,[Preferred_Name] = @P_Preferred_Name
		,[Password_Last_Set] = @P_Password_Last_Set
		,[Password] = @P_Password
		,[ResetToken] = @P_ResetToken
		,[ResetTokenExpires] = @P_ResetTokenExpires
		,[StatusSeqId] = @P_StatusSeqId
		,[Time_Zone] = @P_Time_Zone
		,[VerificationToken] = @P_VerificationToken
		,[Updated_By] = @P_Added_Updated_By
		,[Updated_Date] = @V_Now
	WHERE [AccountSeqId] = @P_AccountSeqId
END
ELSE
BEGIN
	-- INSERT a new row in the table.
	SET NOCOUNT ON

	IF @P_Debug = 1 PRINT 'INSERT [ZGWSecurity].[Accounts]';

	INSERT [ZGWSecurity].[Accounts] (
		 [StatusSeqId]
		,[Account]
		,[First_Name]
		,[Last_Name]
		,[Middle_Name]
		,[Preferred_Name]
		,[Email]
		,[Password_Last_Set]
		,[Password]
		,[ResetToken]
		,[ResetTokenExpires]
		,[FAILED_ATTEMPTS]
		,[IS_SYSTEM_ADMIN]
		,[Added_By]
		,[Added_Date]
		,[LAST_LOGIN]
		,[TIME_ZONE]
		,[VerificationToken]
		,[Location]
		,[Enable_Notifications]
	) VALUES (
		@P_StatusSeqId
		,@P_Account
		,@P_First_Name
		,@P_Last_Name
		,@P_Middle_Name
		,@P_Preferred_Name
		,@P_Email
		,@P_Password_Last_Set
		,@P_Password
		,NULL
		,NULL
		,@P_Failed_Attempts
		,@P_Is_System_Admin
		,@P_Added_Updated_By
		,@V_Now
		,@P_Last_Login
		,@P_Time_Zone
		,@P_VerificationToken
		,@P_Location
		,@P_Enable_Notifications
	);

	SET @P_AccountSeqId = SCOPE_IDENTITY()

	IF EXISTS (SELECT TOP(1) 1 FROM [ZGWSecurity].[Accounts] WHERE AccountSeqId = @P_AccountSeqId)
	BEGIN
		-- The Authenticated role is always added execpte for the Anonymous account
		IF(UPPER(@P_Account) <> 'ANONYMOUS')
			EXEC ZGWSecurity.Set_Account_Roles @P_Account ,1 ,'Authenticated' ,@P_Added_Updated_By ,@P_Debug;

		/*add an entry to account choice table*/
		IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Account_Choices' AND TABLE_SCHEMA = 'ZGWCoreWeb')
		BEGIN
			SELECT @V_Default_Account = Account
			FROM ZGWSecurity.Accounts
			WHERE AccountSeqId = @P_Added_Updated_By

			IF @V_Default_Account = NULL
				SET @V_Default_Account = 'ANONYMOUS'

			IF EXISTS (SELECT 1 FROM [ZGWCoreWeb].Account_Choices WHERE Account = @V_Default_Account)
			BEGIN
				-- Populate values from Account_Choices from the Anonymous account
				IF @P_Debug = 1
					PRINT 'Populating default values from the database for account ' + CONVERT(VARCHAR(MAX), @V_Default_Account)

				SELECT -- FILL THE DEFAULT VALUES
					  @V_SecurityEntityId = [SecurityEntityId]
					, @V_SecurityEntityName = [SecurityEntityName]
					, @V_ColorScheme = [ColorScheme]
					, @V_EvenRow = [EvenRow]
					, @V_EvenFont = [EvenFont]
					, @V_OddRow = [OddRow]
					, @V_OddFont = [OddFont]
					, @V_HeaderRow = [HeaderRow]
					, @V_HeaderFont = [HeaderFont]
					, @V_Background = [Background]
					, @V_FavoriteAction = [FavoriteAction]
					, @V_RecordsPerPage = [RecordsPerPage]
				FROM [ZGWCoreWeb].[Account_Choices]
				WHERE Account = @V_Default_Account
			END
			ELSE
			BEGIN
				IF @P_Debug = 1
					PRINT 'Populating default values minimum values'

				SET @V_SecurityEntityId = (
						SELECT MIN(SecurityEntitySeqId)
						FROM [ZGWSecurity].[Security_Entities]
					);
				SET @V_SecurityEntityName = (
						SELECT [Name]
						FROM [ZGWSecurity].[Security_Entities]
						WHERE SecurityEntitySeqId = @V_SecurityEntityId
					);

				IF @V_SecurityEntityId = NULL
					SET @V_SecurityEntityId = 1

				IF @V_SecurityEntityName = NULL
					SET @V_SecurityEntityName = 'System'
			END
			--END IF

			IF @P_Debug = 1
				PRINT 'Executing ZGWCoreWeb.Set_Account_Choices'

			EXEC ZGWCoreWeb.Set_Account_Choices @P_Account
				, @V_SecurityEntityId
				, @V_SecurityEntityName
				, @V_ColorScheme
				, @V_EvenRow
				, @V_EvenFont
				, @V_OddRow
				, @V_OddFont
				, @V_HeaderRow
				, @V_HeaderFont
				, @V_Background
				, @V_FavoriteAction
				, @V_RecordsPerPage
		END
		--END IF
	END
END -- Get the Error Code for the statement just executed.

IF @P_Debug = 1
	PRINT '@P_AccountSeqId = '

IF @P_Debug = 1
	PRINT @P_AccountSeqId

/* -- GOING BACK TO USING AN OUTPUT PARAMETER.
	SELECT
		  AccountSeqId
		, Account
		, Email
		, Enable_Notifications
		, Is_System_Admin
		, StatusSeqId
		, Password_Last_Set
		, Password
		, ResetToken
		, ResetTokenExpires
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
		[ZGWSecurity].[Accounts] 
	WHERE 
		AccountSeqId = @P_AccountSeqId
*/
SET NOCOUNT OFF;
IF @P_Debug = 1 PRINT 'End Set_Account';
GO
/****** Done: [ZGWCoreWeb].[Account_Choices2] ******/

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '5.1.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;