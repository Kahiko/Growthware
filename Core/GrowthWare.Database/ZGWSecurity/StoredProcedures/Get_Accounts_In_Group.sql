
/*
Usage:
	DECLARE 
		@P_SecurityEntitySeqId INT = 1,
		@P_GroupSeqId INT = 1,
		@P_Debug INT = 0

	exec ZGWSecurity.Get_Accounts_In_Group
		@P_SecurityEntitySeqId,
		@P_GroupSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/10/2011
-- Description:	Selects all accounts in a group
--	given the SecurityEntitySeqId and GroupSeqId
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Accounts_In_Group]
	@P_SecurityEntitySeqId INT,
	@P_GroupSeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Accounts_In_Group'
	SELECT
		Accounts.Account AS ACCT
		, Accounts.Email AS Email
	FROM
		ZGWSecurity.Accounts Accounts WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities_Accounts AcctSecurity WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities Security WITH(NOLOCK),
		ZGWSecurity.Groups Groups WITH(NOLOCK)
	WHERE
		Accounts.AccountSeqId = AcctSecurity.AccountSeqId
		AND AcctSecurity.GroupsSecurityEntitiesSeqId = Security.GroupsSecurityEntitiesSeqId
		AND Security.GroupSeqId = Groups.GroupSeqId
		AND Accounts.StatusSeqId <> 2
		AND Groups.GroupSeqId = @P_GroupSeqId
		AND Security.SecurityEntitySeqId = @P_SecurityEntitySeqId
	ORDER BY
		Accounts.Account
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Accounts_In_Group'

RETURN 0

GO

