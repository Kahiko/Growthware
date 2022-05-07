
/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'Developer',
		@P_SecurityEntitySeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Account_Roles
		@P_Account,
		@P_SecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/09/2011
-- Description:	Selects all groups for a given Account and Entity
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Account_Roles]
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
		AND ZGWSecurity.Accounts.AccountSeqId = ZGWSecurity.Roles_Security_Entities_Accounts.AccountSeqId
		AND ZGWSecurity.Roles_Security_Entities_Accounts.RolesSecurityEntitiesSeqId = ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId
		AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
		AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId = @P_SecurityEntitySeqId
	ORDER BY
		ROLES

RETURN 0

GO

