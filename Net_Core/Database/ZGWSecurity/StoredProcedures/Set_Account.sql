
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
CREATE PROCEDURE [ZGWSecurity].[Set_Account] @P_AccountSeqId INT OUTPUT
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
	,@P_Debug INT = 0
AS
SET NOCOUNT ON;
IF @P_Debug = 1 PRINT 'Start Set_Account';
IF ISDATE(@P_Password_Last_Set) = 0 SET @P_Password_Last_Set = GETDATE();
IF ISDATE(@P_Last_Login) = 0 SET @P_Last_Login = NULL;

DECLARE @VSecurityEntitySeqId VARCHAR(1)
	,@V_SecurityEntityName VARCHAR(50)
	,@V_BackColor VARCHAR(15)
	,@V_LeftColor VARCHAR(15)
	,@V_HeadColor VARCHAR(15)
	,@V_HeaderForeColor VARCHAR(15)
	,@V_SubHeadColor VARCHAR(15)
	,@V_RowBackColor VARCHAR(15)
	,@V_AlternatingRowBackColor VARCHAR(15)
	,@V_ColorScheme VARCHAR(15)
	,@V_FavoriteAction VARCHAR(25)
	,@V_recordsPerPage VARCHAR(1000)
	,@V_Default_Account VARCHAR(50)
	,@V_Now DATETIME = GETDATE()

IF @P_AccountSeqId > - 1
BEGIN
	-- UPDATE PROFILE
	IF @P_Debug = 1 PRINT 'UPDATE [ZGWSecurity].[Accounts]';

	UPDATE [ZGWSecurity].[Accounts]
	SET StatusSeqId = @P_StatusSeqId
		,Account = @P_Account
		,First_Name = @P_First_Name
		,Last_Name = @P_Last_Name
		,Middle_Name = @P_Middle_Name
		,Preferred_Name = @P_Preferred_Name
		,Email = @P_Email
		,Password_Last_Set = @P_Password_Last_Set
		,[Password] = @P_Password
		,[ResetToken] = @P_ResetToken
		,[ResetTokenExpires] = @P_ResetTokenExpires
		,Failed_Attempts = @P_Failed_Attempts
		,Last_Login = @P_Last_Login
		,Time_Zone = @P_Time_Zone
		,Location = @P_Location
		,Is_System_Admin = @P_Is_System_Admin
		,Enable_Notifications = @P_Enable_Notifications
		,Updated_By = @P_Added_Updated_By
		,Updated_Date = @V_Now
	WHERE AccountSeqId = @P_AccountSeqId
END
ELSE
BEGIN
	-- INSERT a new row in the table.
	SET NOCOUNT ON

	IF @P_Debug = 1 PRINT 'INSERT [ZGWSecurity].[Accounts]';

	INSERT [ZGWSecurity].[Accounts] (
		 StatusSeqId
		,Account
		,First_Name
		,Last_Name
		,Middle_Name
		,Preferred_Name
		,Email
		,Password_Last_Set
		,[Password]
		,[ResetToken]
		,[ResetTokenExpires]
		,FAILED_ATTEMPTS
		,IS_SYSTEM_ADMIN
		,Added_By
		,Added_Date
		,LAST_LOGIN
		,TIME_ZONE
		,Location
		,Enable_Notifications
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
					@VSecurityEntitySeqId = SecurityEntityID
					,@V_SecurityEntityName = SecurityEntityName
					,@V_BackColor = BackColor
					,@V_LeftColor = LeftColor
					,@V_HeadColor = HeadColor
					,@V_HeaderForeColor = HeaderForeColor
					,@V_SubHeadColor = SubHeadColor
					,@V_RowBackColor = RowBackColor
					,@V_AlternatingRowBackColor = AlternatingRowBackColor
					,@V_ColorScheme = ColorScheme
					,@V_FavoriteAction = FavoriteAction
					,@V_recordsPerPage = recordsPerPage
				FROM [ZGWCoreWeb].Account_Choices
				WHERE Account = @V_Default_Account
			END
			ELSE
			BEGIN
				IF @P_Debug = 1
					PRINT 'Populating default values minimum values'

				SET @VSecurityEntitySeqId = (
						SELECT MIN(SecurityEntitySeqId)
						FROM ZGWSecurity.Security_Entities
					);
				SET @V_SecurityEntityName = (
						SELECT [Name]
						FROM ZGWSecurity.Security_Entities
						WHERE SecurityEntitySeqId = @VSecurityEntitySeqId
					);

				IF @VSecurityEntitySeqId = NULL
					SET @VSecurityEntitySeqId = 1

				IF @V_SecurityEntityName = NULL
					SET @V_SecurityEntityName = 'System'
			END
			--END IF

			IF @P_Debug = 1
				PRINT 'Executing ZGWCoreWeb.Set_Account_Choices'

			EXEC ZGWCoreWeb.Set_Account_Choices @P_Account
				,@VSecurityEntitySeqId
				,@V_SecurityEntityName
				,@V_BackColor
				,@V_LeftColor
				,@V_HeadColor
				,@V_HeaderForeColor
				,@V_SubHeadColor
				,@V_RowBackColor
				,@V_AlternatingRowBackColor
				,@V_ColorScheme
				,@V_FavoriteAction
				,@V_recordsPerPage
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
