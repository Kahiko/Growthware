
/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'Developer',
		@PSecurityEntitySeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Account_Security
		@P_Account,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/09/2011
-- Description:	Selects all derived roles given the account
--	and Entity.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Account_Security]
	@P_Account VARCHAR(128),
	@PSecurityEntitySeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	SELECT
		ZGWSecurity.Roles.[Name] AS Roles
	FROM
		ZGWSecurity.Accounts WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities_Accounts WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles WITH(NOLOCK)
	WHERE
		ZGWSecurity.Accounts.Account = @P_Account
		AND ZGWSecurity.Roles_Security_Entities_Accounts.AccountSeqId = ZGWSecurity.Accounts.AccountSeqId
		AND ZGWSecurity.Roles_Security_Entities_Accounts.Roles_Security_Entities_SeqID = ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID
		AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))
		AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
	UNION
	SELECT
		ZGWSecurity.Roles.[Name] AS Roles
	FROM
		ZGWSecurity.Accounts WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities_Accounts WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles WITH(NOLOCK)
	WHERE
		ZGWSecurity.Accounts.Account = @P_Account AND
		ZGWSecurity.Groups_Security_Entities_Accounts.AccountSeqId = ZGWSecurity.Accounts.AccountSeqId AND
		ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Accounts.Groups_Security_Entities_SeqID AND
		ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId)) AND
		ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID AND
		ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Roles_Security_Entities_SeqID AND
		ZGWSecurity.Roles.RoleSeqId = ZGWSecurity.Roles_Security_Entities.RoleSeqId
	ORDER BY
		Roles

RETURN 0

GO

