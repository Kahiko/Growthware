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
-- Author:		Michael Regan
-- Create date: 08/07/2024
-- Description:	Now returns multiple tables given the VerificationToken tables are data from:
--	[ZGWSecurity].[RefreshTokens]
--	[ZGWSecurity].[Get_Account_Roles]
--	[ZGWSecurity].[Get_Account_Groups]
--	[ZGWSecurity].[Get_Account_Security]
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Account_By_Verification_Token]
	@P_VerificationToken NVARCHAR(MAX),
	@P_Debug INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @V_Account VARCHAR(128)
		, @V_Is_System_Admin bit
		, @V_SecurityEntitySeqId INT;

	SELECT TOP(1)
		  @V_Account = [ACCTS].[Account] 
		, @V_Is_System_Admin = [ACCTS].[Is_System_Admin]
		, @V_SecurityEntitySeqId = [ACCT_CHOICES].[SecurityEntityId]
	FROM [ZGWSecurity].[Accounts] AS [ACCTS] LEFT JOIN
		[ZGWCoreWeb].[Account_Choices] [ACCT_CHOICES] ON
			[ACCTS].[Account] = [ACCT_CHOICES].[Account]
	WHERE
		[VerificationToken] = @P_VerificationToken
	IF @P_Debug = 1
		BEGIN
			PRINT '@V_Account IS: ' + CONVERT(NVARCHAR(MAX), @V_Account);
			PRINT '@V_Is_System_Admin IS: ' + CONVERT(NVARCHAR(MAX), @V_Is_System_Admin);
			PRINT '@V_SecurityEntitySeqId IS: ' + CONVERT(NVARCHAR(MAX), @V_SecurityEntitySeqId);
		END
	--END IF
	EXEC [ZGWSecurity].[Get_Account]
		@V_Is_System_Admin,
		@V_Account,
		@V_SecurityEntitySeqId,
		@P_Debug
	RETURN 0;
END
GO