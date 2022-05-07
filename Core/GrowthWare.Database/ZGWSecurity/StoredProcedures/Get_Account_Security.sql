
/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'Developer',
		@P_SecurityEntitySeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Account_Security
		@P_Account,
		@P_SecurityEntitySeqId,
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
	@P_SecurityEntitySeqId INT,
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
		AND ZGWSecurity.Roles_Security_Entities_Accounts.RolesSecurityEntitiesSeqId = ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId
		AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId FROM ZGWSecurity.Get_Entity_Parents(1,@P_SecurityEntitySeqId))
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
		ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Accounts.GroupsSecurityEntitiesSeqId AND
		ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId FROM ZGWSecurity.Get_Entity_Parents(1,@P_SecurityEntitySeqId)) AND
		ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId AND
		ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.RolesSecurityEntitiesSeqId AND
		ZGWSecurity.Roles.RoleSeqId = ZGWSecurity.Roles_Security_Entities.RoleSeqId
	ORDER BY
		Roles

RETURN 0

GO

