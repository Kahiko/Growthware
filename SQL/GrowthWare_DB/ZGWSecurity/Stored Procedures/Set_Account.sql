/*
Usage:
	DECLARE 
		@P_Account_SeqID int = -1,
		@P_Status_SeqID int = 1,
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
		@P_Account_SeqID,
		@P_Status_SeqID,
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
	SET @P_Account_SeqID = (SELECT Account_SeqID FROM ZGWSecurity.Accounts WHERE Account = 'test')
	exec ZGWSecurity.Set_Account
		@P_Account_SeqID,
		@P_Status_SeqID,
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
--	@P_Status_SeqID's value determines insert/update
--	a value of -1 is insert > -1 performs update
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Account]
	@P_Account_SeqID int output,
	@P_Status_SeqID int,
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
	DECLARE @V_Security_Entity_SeqID VARCHAR(1),
		@V_SE_NAME VARCHAR(50),
		@V_Back_Color VARCHAR(15),
		@V_Left_Color VARCHAR(15),
		@V_Head_Color VARCHAR(15),
		@V_Header_ForeColor VARCHAR(15),
		@V_Sub_Head_Color VARCHAR(15),
		@V_Row_BackColor VARCHAR(15),
		@V_AlternatingRow_BackColor VARCHAR(15),
		@V_Color_Scheme VARCHAR(15),
		@V_Thin_Actions VARCHAR(256),
		@V_Wide_Actions VARCHAR(256),
		@V_Favorite_Action VARCHAR(25),
		@V_Records_Per_Page VARCHAR(1000),
		@V_Default_Account VARCHAR(50),
		@V_Now DATETIME = GETDATE()
	
	
	IF @P_Account_SeqID > -1
		BEGIN -- UPDATE PROFILE
			IF @P_Debug = 1 PRINT 'UPDATE [ZGWSecurity].[Accounts]'
			UPDATE [ZGWSecurity].[Accounts]
			SET 
				Status_SeqID = @P_Status_SeqID,
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
				Account_SeqID = @P_Account_SeqID

		END
	ELSE
		BEGIN -- INSERT a new row in the table.
			SET NOCOUNT ON
			IF @P_Debug = 1 PRINT 'INSERT [ZGWSecurity].[Accounts]'
			INSERT [ZGWSecurity].[Accounts]
			(
				Status_SeqID,
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
				@P_Status_SeqID,
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
			SET @P_Account_SeqID = SCOPE_IDENTITY()
			IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Accounts] WHERE Account_SeqID = @P_Account_SeqID)

			exec ZGWSecurity.Set_Account_Roles
				@P_Account,
				1,
				'Authenticated',
				@P_Added_Updated_By,
				@P_Debug

			BEGIN
				/*add an entry to account choice table*/
				IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Account_Choices' AND TABLE_SCHEMA = 'ZGWCoreWeb')		
					BEGIN
						SELECT @V_Default_Account=Account FROM ZGWSecurity.Accounts WHERE Account_SeqID = @P_Added_Updated_By
						
						IF @V_Default_Account = NULL SET @V_Default_Account = 'ANONYMOUS'
						
						IF EXISTS (SELECT 1 FROM [ZGWCoreWeb].Account_Choices WHERE Account = @V_Default_Account)
							BEGIN -- Populate values from Account_Choices from the Anonymous account
								IF @P_Debug = 1 PRINT 'Populating default values from the database for account ' + CONVERT(VARCHAR(MAX),@V_Default_Account)
								SELECT -- FILL THE DEFAULT VALUES
									@V_Security_Entity_SeqID = SE_SEQ_ID,
									@V_SE_NAME = SE_NAME,
									@V_Back_Color = Back_Color,
									@V_Left_Color = Left_Color,
									@V_Head_Color = Head_Color,
									@V_Header_ForeColor = Header_ForeColor,
									@V_Sub_Head_Color = Sub_Head_Color,
									@V_Row_BackColor = Row_BackColor,
									@V_AlternatingRow_BackColor = AlternatingRow_BackColor,
									@V_Color_Scheme = Color_Scheme,
									@V_Favorite_Action = Favorite_Action,
									@V_Thin_Actions = Thin_Actions,
									@V_Wide_Actions = Wide_Actions,
									@V_Records_Per_Page = Records_Per_Page
								FROM
									[ZGWCoreWeb].Account_Choices
								WHERE 
									Account = @V_Default_Account
							END
						ELSE
							BEGIN
								IF @P_Debug = 1 PRINT 'Populating default values minimum values'
								SET @V_Security_Entity_SeqID = (SELECT MIN(Security_Entity_SeqID) FROM ZGWSecurity.Security_Entities)
								SET @V_SE_NAME = (SELECT [Name] FROM ZGWSecurity.Security_Entities WHERE Security_Entity_SeqID = @V_Security_Entity_SeqID)
								IF @V_Security_Entity_SeqID = NULL SET @V_Security_Entity_SeqID = 1
								IF @V_SE_NAME = NULL SET @V_SE_NAME = 'System'
							END
						--END IF
						IF @P_Debug = 1 PRINT 'Executing ZGWCoreWeb.Set_Account_Choices'
						EXEC ZGWCoreWeb.Set_Account_Choices
							@P_Account,
							@V_Security_Entity_SeqID,
							@V_SE_NAME,
							@V_Back_Color,
							@V_Left_Color,
							@V_Head_Color,
							@V_Header_ForeColor,
							@V_Sub_Head_Color,
							@V_Row_BackColor,
							@V_AlternatingRow_BackColor,
							@V_Color_Scheme ,
							@V_Favorite_Action,
							@V_Thin_Actions,
							@V_Wide_Actions,
							@V_Records_Per_Page	
					END
				--END IF
			END
		END-- Get the Error Code for the statement just executed.
	IF @P_Debug = 1 PRINT '@P_Account_SeqID = '
	IF @P_Debug = 1 PRINT @P_Account_SeqID
/* -- GOING BACK TO USING AN OUTPUT PARAMETER.
	SELECT
		Account_SeqID
		, Account
		, Email
		, Enable_Notifications
		, Is_System_Admin
		, Status_SeqID
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
		Account_SeqID = @P_Account_SeqID
*/
	IF @P_Debug = 1 PRINT 'End Set_Account'
