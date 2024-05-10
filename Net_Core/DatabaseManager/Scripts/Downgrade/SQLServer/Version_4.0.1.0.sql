-- Downgrade script for version 4.0.1.0

ALTER TABLE [ZGWSecurity].[Accounts] ALTER COLUMN [ResetToken] VARCHAR(MAX);

DECLARE 
	@V_FunctionSeqId int,
	@V_MyAction VARCHAR(256) = '/accounts/forgot-password',
	@V_ErrorCode int;

SET @V_FunctionSeqId = (SELECT FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction);
EXEC ZGWSecurity.Delete_Function
	@V_FunctionSeqId ,
	@V_ErrorCode;

/****** Start: Procedure [ZGWSecurity].[Get_Function_Sort] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Function_Sort]') AND type in (N'P', N'PC'))
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
ALTER PROCEDURE [ZGWSecurity].[Get_Function_Sort]
	@P_FunctionSeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	DECLARE @V_Parent_ID INT
	DECLARE @V_NAV_TYPE_ID INT
	SET @V_Parent_ID = (SELECT ParentSeqId
FROM ZGWSecurity.Functions
WHERE FunctionSeqId = @P_FunctionSeqId)
	SET @V_NAV_TYPE_ID = (SELECT Navigation_Types_NVP_DetailSeqId
FROM ZGWSecurity.Functions
WHERE FunctionSeqId = @P_FunctionSeqId)
	SELECT
	FunctionSeqId as FUNCTION_SEQ_ID,
	[Name],
	[Action],
	Sort_Order
FROM
	ZGWSecurity.Functions WITH(NOLOCK)
WHERE
		ParentSeqId = @V_PARENT_ID
	AND Is_Nav = 1
	AND Navigation_Types_NVP_DetailSeqId = @V_NAV_TYPE_ID
	AND ParentSeqId <> 1
ORDER BY
		Sort_Order ASC

RETURN 0

GO
/****** End: Procedure [ZGWSecurity].[Get_Function_Sort] ******/
/****** Start: Procedure [ZGWSecurity].[Set_Account] ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Account]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Account] AS'
END
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
ALTER PROCEDURE [ZGWSecurity].[Set_Account]
	@P_AccountSeqId int output,
	@P_StatusSeqId int,
	@P_Account VARCHAR(128),
	@P_First_Name VARCHAR(35),
	@P_Last_Name VARCHAR(35),
	@P_Middle_Name VARCHAR(35),
	@P_Preferred_Name VARCHAR(50),
	@P_Email VARCHAR(128),
	@P_Password VARCHAR(256),
	@P_Password_Last_Set datetime,
	@P_Failed_Attempts int,
	@P_Added_Updated_By int,
	@P_Last_Login datetime,
	@P_Time_Zone int,
	@P_Location VARCHAR(50),
	@P_Enable_Notifications int,
	@P_Is_System_Admin int,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Start Set_Account'
	DECLARE @VSecurityEntitySeqId VARCHAR(1),
		@V_SecurityEntityName VARCHAR(50),
		@V_BackColor VARCHAR(15),
		@V_LeftColor VARCHAR(15),
		@V_HeadColor VARCHAR(15),
		@V_HeaderForeColor VARCHAR(15),
		@V_SubHeadColor VARCHAR(15),
		@V_RowBackColor VARCHAR(15),
		@V_AlternatingRowBackColor VARCHAR(15),
		@V_ColorScheme VARCHAR(15),
		@V_FavoriteAction VARCHAR(25),
		@V_recordsPerPage VARCHAR(1000),
		@V_Default_Account VARCHAR(50),
		@V_Now DATETIME = GETDATE()
	
	
	IF @P_AccountSeqId > -1
		BEGIN
	-- UPDATE PROFILE
	IF @P_Debug = 1 PRINT 'UPDATE [ZGWSecurity].[Accounts]'
	UPDATE [ZGWSecurity].[Accounts]
			SET 
				StatusSeqId = @P_StatusSeqId,
				Account = @P_Account,
				First_Name = @P_First_Name,
				Last_Name = @P_Last_Name,
				Middle_Name = @P_Middle_Name,
				Preferred_Name = @P_Preferred_Name,
				Email = @P_Email,
				Password_Last_Set = @P_Password_Last_Set,
				[Password] = @P_Password,
				Failed_Attempts = @P_Failed_Attempts,
				Last_Login = @P_Last_Login,
				Time_Zone = @P_Time_Zone,
				Location = @P_Location,
				Is_System_Admin = @P_Is_System_Admin,
				Enable_Notifications = @P_Enable_Notifications,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = @V_Now
			WHERE
				AccountSeqId = @P_AccountSeqId

END
	ELSE
		BEGIN
	-- INSERT a new row in the table.
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'INSERT [ZGWSecurity].[Accounts]'
	INSERT [ZGWSecurity].[Accounts]
		(
		StatusSeqId,
		Account,
		First_Name,
		Last_Name,
		Middle_Name,
		Preferred_Name,
		Email,
		Password_Last_Set,
		[Password],
		FAILED_ATTEMPTS,
		IS_SYSTEM_ADMIN,
		Added_By,
		Added_Date,
		LAST_LOGIN,
		TIME_ZONE,
		Location,
		Enable_Notifications
		)
	VALUES
		(
			@P_StatusSeqId,
			@P_Account,
			@P_First_Name,
			@P_Last_Name,
			@P_Middle_Name,
			@P_Preferred_Name,
			@P_Email,
			@P_Password_Last_Set,
			@P_Password,
			@P_Failed_Attempts,
			@P_Is_System_Admin,
			@P_Added_Updated_By,
			@V_Now,
			@P_Last_Login,
			@P_Time_Zone,
			@P_Location,
			@P_Enable_Notifications
			)
	SET @P_AccountSeqId = SCOPE_IDENTITY()
	IF EXISTS (SELECT 1
	FROM [ZGWSecurity].[Accounts]
	WHERE AccountSeqId = @P_AccountSeqId)

			exec ZGWSecurity.Set_Account_Roles
				@P_Account,
				1,
				'Authenticated',
				@P_Added_Updated_By,
				@P_Debug

	BEGIN
		/*add an entry to account choice table*/
		IF  EXISTS (SELECT 1
		FROM INFORMATION_SCHEMA.TABLES
		WHERE TABLE_NAME = 'Account_Choices' AND TABLE_SCHEMA = 'ZGWCoreWeb')		
					BEGIN
			SELECT @V_Default_Account=Account
			FROM ZGWSecurity.Accounts
			WHERE AccountSeqId = @P_Added_Updated_By

			IF @V_Default_Account = NULL SET @V_Default_Account = 'ANONYMOUS'

			IF EXISTS (SELECT 1
			FROM [ZGWCoreWeb].Account_Choices
			WHERE Account = @V_Default_Account)
							BEGIN
				-- Populate values from Account_Choices from the Anonymous account
				IF @P_Debug = 1 PRINT 'Populating default values from the database for account ' + CONVERT(VARCHAR(MAX),@V_Default_Account)
				SELECT -- FILL THE DEFAULT VALUES
					@VSecurityEntitySeqId = SecurityEntityID,
					@V_SecurityEntityName = SecurityEntityName,
					@V_BackColor = BackColor,
					@V_LeftColor = LeftColor,
					@V_HeadColor = HeadColor,
					@V_HeaderForeColor = HeaderForeColor,
					@V_SubHeadColor = SubHeadColor,
					@V_RowBackColor = RowBackColor,
					@V_AlternatingRowBackColor = AlternatingRowBackColor,
					@V_ColorScheme = ColorScheme,
					@V_FavoriteAction = FavoriteAction,
					@V_recordsPerPage = recordsPerPage
				FROM
					[ZGWCoreWeb].Account_Choices
				WHERE 
									Account = @V_Default_Account
			END
						ELSE
							BEGIN
				IF @P_Debug = 1 PRINT 'Populating default values minimum values'
				SET @VSecurityEntitySeqId = (SELECT MIN(SecurityEntitySeqId)
				FROM ZGWSecurity.Security_Entities)
				SET @V_SecurityEntityName = (SELECT [Name]
				FROM ZGWSecurity.Security_Entities
				WHERE SecurityEntitySeqId = @VSecurityEntitySeqId)
				IF @VSecurityEntitySeqId = NULL SET @VSecurityEntitySeqId = 1
				IF @V_SecurityEntityName = NULL SET @V_SecurityEntityName = 'System'
			END
			--END IF
			IF @P_Debug = 1 PRINT 'Executing ZGWCoreWeb.Set_Account_Choices'
			EXEC ZGWCoreWeb.Set_Account_Choices
							@P_Account,
							@VSecurityEntitySeqId,
							@V_SecurityEntityName,
							@V_BackColor,
							@V_LeftColor,
							@V_HeadColor,
							@V_HeaderForeColor,
							@V_SubHeadColor,
							@V_RowBackColor,
							@V_AlternatingRowBackColor,
							@V_ColorScheme ,
							@V_FavoriteAction,
							@V_recordsPerPage
		END
	--END IF
	END
END-- Get the Error Code for the statement just executed.
	IF @P_Debug = 1 PRINT '@P_AccountSeqId = '
	IF @P_Debug = 1 PRINT @P_AccountSeqId
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
	IF @P_Debug = 1 PRINT 'End Set_Account'
GO
/****** End: Procedure [ZGWSecurity].[Set_Account] ******/

-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '4.0.0.0',
    [Updated_By] = 3,
    [Updated_Date] = getdate()