/*
Usage:
	DECLARE 
		@P_Security_Entity_SeqID INT = 1,
		@P_Group_SeqID INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Accounts_Not_In_Group
		@P_Security_Entity_SeqID,
		@P_Group_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/09/2011
-- Description:	Selects all accounts not in a group
--	given the Security_Entity_SeqID and Group_SeqID
-- Note: This should not be needed by the CoreWebApplication anymore
--	and was left for others that may need it.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Accounts_Not_In_Group]
	@P_Security_Entity_SeqID INT,
	@P_Group_SeqID INT,
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
						Accounts.Account_SeqID = AcctSecurity.Account_SeqID
						AND AcctSecurity.Groups_Security_Entities_SeqID = Security.Groups_Security_Entities_SeqID
						AND Security.Group_SeqID = Groups.Group_SeqID
						AND Accounts.Status_SeqID <> 2
						AND Groups.Group_SeqID = @P_Group_SeqID
						AND [Security].Security_Entity_SeqID = @P_Security_Entity_SeqID
					)
	ORDER BY
		Accounts.Account
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Accounts_In_Group'

RETURN 0