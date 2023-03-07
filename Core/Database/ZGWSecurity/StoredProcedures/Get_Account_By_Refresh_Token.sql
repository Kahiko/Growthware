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
	FROM [Growthware].[ZGWSecurity].[Accounts] ACCT
		INNER JOIN [ZGWSecurity].[RefreshTokens] RT ON
			RT.[Token] = @P_Token
			AND ACCT.[AccountSeqId] = RT.[AccountSeqId]
	RETURN 0
END
GO