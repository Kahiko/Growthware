
/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId INT = 1,
		@P_RoleSeqId INT = 1,
		@P_Debug INT = 0

	exec ZGWSecurity.Get_Accounts_In_Role
		@PSecurityEntitySeqId,
		@P_RoleSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/09/2011
-- Description:	Selects all accounts in a role
--	given the SecurityEntitySeqId and RoleSeqId
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Accounts_In_Role]
	@PSecurityEntitySeqId INT,
	@P_RoleSeqId INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Accounts_In_Role'
	SELECT
		Accounts.Account AS ACCT
		, Accounts.Email AS Email
	FROM
		ZGWSecurity.Accounts Accounts WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities_Accounts AcctSecurity WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities [Security] WITH(NOLOCK),
		ZGWSecurity.Roles Roles WITH(NOLOCK)
	WHERE
		Accounts.AccountSeqId = AcctSecurity.AccountSeqId
		AND AcctSecurity.Roles_Security_Entities_SeqID = Security.Roles_Security_Entities_SeqID
		AND [Security].RoleSeqId = Roles.RoleSeqId
		AND Accounts.Status_SeqID <> 2
		AND Roles.RoleSeqId = @P_RoleSeqId
		AND [Security].SecurityEntitySeqId = @PSecurityEntitySeqId
	ORDER BY
		Accounts.Account
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Accounts_In_Role'

RETURN 0

GO

