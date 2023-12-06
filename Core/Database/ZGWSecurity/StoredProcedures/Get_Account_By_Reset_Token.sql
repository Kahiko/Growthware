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
CREATE OR ALTER PROCEDURE [ZGWSecurity].[Get_Account_By_Reset_Token]
	@P_ResetToken NVARCHAR(MAX),
	@P_Debug INT = 0
AS
BEGIN
	SET NOCOUNT ON
	SELECT TOP (1) 
		 ACCT.[AccountSeqId]
		,ACCT.[Account]
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
-- var account = _context.Accounts.SingleOrDefault(x => x.ResetToken == token && x.ResetTokenExpires > DateTime.UtcNow);
    WHERE
        ACCT.[ResetToken] = @P_ResetToken
        AND ResetTokenExpires > GETDATE();
	RETURN 0
END
GO