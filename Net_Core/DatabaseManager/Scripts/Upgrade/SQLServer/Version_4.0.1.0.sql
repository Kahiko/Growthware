-- Upgrade script for version 4.0.1.0
SET NOCOUNT ON;

DECLARE @V_MessageSeqId INT = (SELECT [MessageSeqId] from [ZGWCoreWeb].[Messages] where [Name] = 'RequestNewPassword');

ALTER TABLE [ZGWSecurity].[Accounts] ALTER COLUMN [ResetToken] VARCHAR(256);

IF NOT EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [Action] = '/accounts/forgot-password')
BEGIN
	PRINT 'Adding Forgot Password';

	DECLARE @V_FunctionSeqId INT = - 1
		,@V_Name VARCHAR(30) = 'Forgot Password'
		,@V_Description VARCHAR(512) = 'Forgot Password'
		,@V_FunctionTypeSeqId INT = 1
		,@V_Source VARCHAR(512) = ''
		,@V_Controller VARCHAR(512) = ''
		,@V_Resolve VARCHAR(MAX) = ''
		,@V_Enable_View_State INT = 0
		,@V_Enable_Notifications INT = 0
		,@V_Redirect_On_Timeout INT = 0
		,@V_Is_Nav INT = 0
		,@V_Link_Behavior INT = 1
		,@V_NO_UI INT = 0
		,@V_NAV_TYPE_ID INT = 1
		,@V_Action VARCHAR(256) = '/accounts/forgot-password'
		,@V_Meta_Key_Words VARCHAR(512) = ''
		,@V_ParentSeqId INT = 1
		,@V_Notes VARCHAR(512) = ''
		,@V_Debug INT = 0
		,@V_SystemID INT = 1
		,@V_ViewPermission INT;

	EXEC ZGWSecurity.Set_Function @V_FunctionSeqId
		,@V_Name
		,@V_Description
		,@V_FunctionTypeSeqId
		,@V_Source
		,@V_Controller
		,@V_Resolve
		,@V_Enable_View_State
		,@V_Enable_Notifications
		,@V_Redirect_On_Timeout
		,@V_Is_Nav
		,@V_Link_Behavior
		,@V_NO_UI
		,@V_NAV_TYPE_ID
		,@V_Action
		,@V_Meta_Key_Words
		,@V_ParentSeqId
		,@V_Notes
		,@V_SystemID
		,@V_Debug;

	SET @V_FunctionSeqId = (
			SELECT FunctionSeqId
			FROM ZGWSecurity.Functions
			WHERE action = @V_Action
		);
	SET @V_ViewPermission = (
			SELECT NVP_DetailSeqId
			FROM ZGWSecurity.Permissions
			WHERE NVP_Detail_Value = 'View'
		);

	EXEC ZGWSecurity.Set_Function_Roles @V_FunctionSeqId
		,1
		,'Anonymous'
		,@V_ViewPermission
		,@V_SystemID
		,@V_Debug;
END
--END IF

-- Update the message
UPDATE [ZGWCoreWeb].[Messages] SET [Body] = 'Dear <FullName>,
<br><br>
There has been a request for a password change: 
<br><br>
	Please Use the following link to logon and change your password: <a href="<Server>accounts/logon?resetToken=<ResetToken>">Change Password</a>
<br>
<br>
If you did not request a password change, please ignore this email, no changes have been made to your account.
<br>
<br>
<b>Please note once you have logged on using this link you will only be able to change our password.</b>'
WHERE [MessageSeqId] = @V_MessageSeqId;

/****** Start: Procedure [ZGWSecurity].[Get_Function_Sort] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'ZGWSecurity.Get_Function_Sort') AND type IN ( N'P' ,N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Function_Sort] AS'
	END
--End If
GO

/*
Usage:
	DECLARE 
		@P_FunctionSeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Function_Sort
		@P_FunctionSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/15/2011
-- Description:	Returns sorted function information
--	for related functions given the funtionSeqId
-- =============================================
-- Author:		Michael Regan
-- Create date: 05/03/2024
-- Description:	Fixed but where nothing is returned if the ParentSeqId <> 1
--  this should have been ParentSeqId = @V_Parent_ID
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Function_Sort] @P_FunctionSeqId INT
	,@P_Debug INT = 0
AS
SET NOCOUNT ON;

DECLARE @V_Parent_ID INT
	,@V_NAV_TYPE_ID INT;

SET @V_Parent_ID = (
		SELECT ParentSeqId
		FROM ZGWSecurity.Functions
		WHERE FunctionSeqId = @P_FunctionSeqId
	);
SET @V_NAV_TYPE_ID = (
		SELECT Navigation_Types_NVP_DetailSeqId
		FROM ZGWSecurity.Functions
		WHERE FunctionSeqId = @P_FunctionSeqId
	);

SELECT 
	 [FunctionSeqId] AS [FUNCTION_SEQ_ID]
	,[Name]
	,[Action]
	,[Sort_Order]
FROM [ZGWSecurity].[Functions] WITH (NOLOCK)
WHERE [ParentSeqId] = @V_PARENT_ID
	AND [Is_Nav] = 1
	AND [Navigation_Types_NVP_DetailSeqId] = @V_NAV_TYPE_ID
	AND [ParentSeqId] = @V_Parent_ID
ORDER BY [Sort_Order] ASC;

SET NOCOUNT OFF;
GO

/****** End: Procedure [ZGWSecurity].[Get_Function_Sort] ******/
/****** Start: Procedure [ZGWSecurity].[Set_Account] ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Account]') AND type IN (N'P' ,N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Account] AS'
	END
--END IF
GO

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
		@P_ResetToken VARCHAR(256) = NULL,
		@P_ResetTokenExpires datetime = NULL,
		@P_Failed_Attempts int = 0,
		@P_Added_Updated_By int = 1,
		@P_Last_Login datetime = GETDATE(),
		@P_Time_Zone int = -5,
		@P_Location VARCHAR(50) = 'desk',
		@P_Enable_Notifications int = 0,
		@P_Is_System_Admin int = 0,
		@P_Debug INT = 1
--Insert new
	EXEC ZGWSecurity.Set_Account 
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
		@P_ResetToken VARCHAR(256),
		@P_ResetTokenExpires datetime,
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
	EXEC ZGWSecurity.Set_Account
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
		@P_ResetToken,
		@P_ResetTokenExpires,
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
ALTER PROCEDURE [ZGWSecurity].[Set_Account] @P_AccountSeqId INT OUTPUT
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
/****** End: Procedure [ZGWSecurity].[Set_Account] ******/

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '4.0.1.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;
