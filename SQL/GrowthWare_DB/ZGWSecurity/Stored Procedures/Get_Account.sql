/*
Usage:
	DECLARE 
		@P_Is_System_Admin bit = 1,
		@P_Account VARCHAR(128) = 'Developer',
		@P_Security_Entity_SeqID INT = 1,
		@P_Debug INT = 1

	exec  ZGWSecurity.Get_Account
		@P_Is_System_Admin,
		@P_Account,
		@P_Security_Entity_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/08/2011
-- Description:	Selects 1 or all records from ZGWSecurity.Get_Account
--	from ZGWSecurity.Accounts
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Account]
	@P_Is_System_Admin Bit,
	@P_Account VARCHAR(128),
	@P_Security_Entity_SeqID INT,
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
						Account_SeqID AS ACCT_SEQ_ID
						, Account AS ACCT
						, Email
						, Enable_Notifications
						, Is_System_Admin
						, Status_SeqID AS STATUS_SEQ_ID
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
					IF @P_Debug = 1 PRINT 'Selecting all accounts for Entity ' + CONVERT(VARCHAR(MAX),@P_Security_Entity_SeqID)
					DECLARE @V_Accounts TABLE (
						Account_SeqID INT
						, Account VARCHAR(100)
						, Email VARCHAR(100)
						, Enable_Notifications BIT
						, Is_System_Admin INT
						, Status_SeqID INT
						, Password_Last_Set DATETIME
						, [Password] VARCHAR(256)
						, Failed_Attempts INT
						, First_Name VARCHAR(30)
						, Last_Login DATETIME
						, Last_Name VARCHAR(30)
						, Location VARCHAR(100)
						, Middle_Name VARCHAR(30)
						, Preferred_Name VARCHAR(100)
						, Time_Zone INT
						, Added_By INT
						, Added_Date DATETIME
						, Updated_By INT
						, Updated_Date DATETIME)
					INSERT INTO @V_Accounts
					SELECT -- Roles via roles
						Accounts.Account_SeqID
						, Accounts.Account
						, Accounts.Email
						, Accounts.Enable_Notifications
						, Accounts.Is_System_Admin
						, Accounts.Status_SeqID
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
						Roles_Security_Entities_Accounts.Account_SeqID = Accounts.Account_SeqID
						AND Roles_Security_Entities_Accounts.Roles_Security_Entities_SeqID = Roles_Security_Entities.Roles_Security_Entities_SeqID
						AND Roles_Security_Entities.Security_Entity_SeqID IN (SELECT Security_Entity_SeqID FROM ZGWSecurity.Get_Entity_Parents(1,@P_Security_Entity_SeqID))
						AND Roles_Security_Entities.Role_SeqID = ZGWSecurity.Roles.Role_SeqID
					UNION
					SELECT -- Roles via groups
						Accounts.Account_SeqID
						, Accounts.Account
						, Accounts.Email
						, Accounts.Enable_Notifications
						, Accounts.Is_System_Admin
						, Accounts.Status_SeqID
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
						ZGWSecurity.Groups_Security_Entities_Accounts.Account_SeqID = Accounts.Account_SeqID
						AND ZGWSecurity.Groups_Security_Entities.Security_Entity_SeqID IN (SELECT Security_Entity_SeqID FROM ZGWSecurity.Get_Entity_Parents(1,@P_Security_Entity_SeqID))
						AND ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Groups_Security_Entities_SeqID
						AND Roles_Security_Entities.Roles_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Roles_Security_Entities_SeqID
						AND Roles_Security_Entities.Role_SeqID = ZGWSecurity.Roles.Role_SeqID
				
					SELECT DISTINCT
						Account_SeqID AS ACCT_SEQ_ID
						, Account AS ACCT
						, Email
						, Enable_Notifications
						, Is_System_Admin
						, Status_SeqID AS STATUS_SEQ_ID
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
				Account_SeqID AS ACCT_SEQ_ID
				, Account AS ACCT
				, Email
				, Enable_Notifications
				, Is_System_Admin
				, Status_SeqID AS STATUS_SEQ_ID
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