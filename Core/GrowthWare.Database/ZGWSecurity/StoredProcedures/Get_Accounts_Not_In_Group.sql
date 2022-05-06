
/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId INT = 1,
		@P_GroupSeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Accounts_Not_In_Group
		@PSecurityEntitySeqId,
		@P_GroupSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/09/2011
-- Description:	Selects all accounts not in a group
--	given the SecurityEntitySeqId and GroupSeqId
-- Note: This should not be needed by the CoreWebApplication anymore
--	and was left for others that may need it.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Accounts_Not_In_Group]
	@PSecurityEntitySeqId INT,
	@P_GroupSeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Accounts_In_Group'
	SELECT
		Accounts.Account AS ACCT
	FROM
		Accounts
	WHERE
		Account NOT IN(SELECT
						Accounts.Account
					FROM
						ZGWSecurity.Accounts Accounts WITH(NOLOCK),
						ZGWSecurity.Groups_Security_Entities_Accounts AcctSecurity WITH(NOLOCK),
						ZGWSecurity.Groups_Security_Entities [Security] WITH(NOLOCK),
						ZGWSecurity.Groups Groups WITH(NOLOCK)
					WHERE
						Accounts.AccountSeqId = AcctSecurity.AccountSeqId
						AND AcctSecurity.Groups_Security_Entities_SeqID = Security.Groups_Security_Entities_SeqID
						AND Security.GroupSeqId = Groups.GroupSeqId
						AND Accounts.Status_SeqID <> 2
						AND Groups.GroupSeqId = @P_GroupSeqId
						AND [Security].SecurityEntitySeqId = @PSecurityEntitySeqId
					)
	ORDER BY
		Accounts.Account
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Accounts_In_Group'

RETURN 0

GO

