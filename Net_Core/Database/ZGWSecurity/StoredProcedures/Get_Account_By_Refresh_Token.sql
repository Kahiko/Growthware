/*
Usage:

DECLARE 
	@P_Token NVARCHAR(MAX) = '',
	@P_Debug INT = 1

EXEC [ZGWSecurity].[Get_Account_By_Refresh_Token]
	@P_Account,
	@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 11/11/2022
-- Description:	Selects a single account given the Token
-- =============================================
-- Author:		Michael Regan
-- Create date: 11/11/2022
-- Description:	Now returns multiple tables given the RefreshToken the data tables are from:
--					[ZGWSecurity].[RefreshTokens]
--					[ZGWSecurity].[Get_Account_Roles]
--					[ZGWSecurity].[Get_Account_Groups]
--					[ZGWSecurity].[Get_Account_Security]
-- =============================================
CREATE OR ALTER PROCEDURE [ZGWSecurity].[Get_Account_By_Refresh_Token]
	@P_Token NVARCHAR(MAX),
	@P_Debug INT = 0
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @V_Account VARCHAR(128)
		, @V_Is_System_Admin bit
		, @V_SecurityEntitySeqId INT;

	SELECT TOP(1)
		  @V_Account = [ACCTS].[Account] 
		, @V_Is_System_Admin = [ACCTS].[Is_System_Admin]
		, @V_SecurityEntitySeqId = [ACCT_CHOICES].[SecurityEntityId]
	FROM 
		[ZGWSecurity].[Accounts] [ACCTS]
		INNER JOIN [ZGWSecurity].[RefreshTokens] [RT] ON
			[RT].[Token] = @P_Token
			AND [ACCTS].[AccountSeqId] = [RT].[AccountSeqId]
		LEFT JOIN [ZGWCoreWeb].[Account_Choices] [ACCT_CHOICES] ON
			[ACCTS].[Account] = [ACCT_CHOICES].[Account];

	IF @P_Debug = 1
		BEGIN
			PRINT '@V_Account IS: ' + CONVERT(NVARCHAR(MAX), @V_Account);
			PRINT '@V_Is_System_Admin IS: ' + CONVERT(NVARCHAR(MAX), @V_Is_System_Admin);
			PRINT '@V_SecurityEntitySeqId IS: ' + CONVERT(NVARCHAR(MAX), @V_SecurityEntitySeqId);
		END
	--END IF
	-- [ZGWSecurity].[Accounts] (Reduces code duplication)
	EXEC [ZGWSecurity].[Get_Account]
		@V_Is_System_Admin,
		@V_Account,
		@V_SecurityEntitySeqId,
		@P_Debug
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
			ON ACCT.[Account] = @V_Account AND RT.AccountSeqId = ACCT.[AccountSeqId]
    ORDER BY [Created] ASC;
	-- [ZGWSecurity].[Get_Account_Roles]
	EXEC [ZGWSecurity].[Get_Account_Roles] @V_Account, @V_SecurityEntitySeqId
	-- [ZGWSecurity].[Get_Account_Groups]
	EXEC [ZGWSecurity].[Get_Account_Groups] @V_Account, @V_SecurityEntitySeqId
	-- [ZGWSecurity].[Get_Account_Security]
	EXEC [ZGWSecurity].[Get_Account_Security] @V_Account, @V_SecurityEntitySeqId
	RETURN 0;
END;
GO