
/*
Usage:
	DECLARE 
		@P_SecurityEntitySeqId INT = 1,
		@P_RoleSeqId INT = 1,
		@P_Debug INT = 0

	exec ZGWSecurity.Get_Accounts_Not_In_Role
		@P_SecurityEntitySeqId,
		@P_RoleSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/11/2011
-- Description:	Selects all accounts not in a role
--	given the SecurityEntitySeqId and RoleSeqId
-- Note: This should not be needed by the CoreWebApplication anymore
--	and was left for others that may need it.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Accounts_Not_In_Role]
	@P_SecurityEntitySeqId INT,
	@P_RoleSeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Accounts_In_Role'
	SELECT
	Accounts.Account AS ACCT
FROM
	Accounts
WHERE
		Account NOT IN(
					SELECT
	Accounts.Account
FROM
	ZGWSecurity.Accounts Accounts WITH(NOLOCK),
	ZGWSecurity.Roles_Security_Entities_Accounts AcctSecurity WITH(NOLOCK),
	ZGWSecurity.Roles_Security_Entities [Security] WITH(NOLOCK),
	ZGWSecurity.Roles Roles WITH(NOLOCK)
WHERE
						Accounts.AccountSeqId = AcctSecurity.AccountSeqId
	AND AcctSecurity.RolesSecurityEntitiesSeqId = Security.RolesSecurityEntitiesSeqId
	AND [Security].RoleSeqId = Roles.RoleSeqId
	AND Accounts.StatusSeqId <> 2
	AND Roles.RoleSeqId = @P_RoleSeqId
	AND [Security].SecurityEntitySeqId = @P_SecurityEntitySeqId
					)
ORDER BY
		Accounts.Account
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Accounts_In_Role'

RETURN 0

GO

