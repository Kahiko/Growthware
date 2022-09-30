
/*
Usage:
	DECLARE 
		@P_RolesSecurityEntitiesSeqId AS INT = 2,
		@P_SecurityEntitySeqId AS INT = 1

	exec  [ZGWSecurity].[Delete_Roles_Accounts]
		@P_RolesSecurityEntitiesSeqId
		@P_SecurityEntitySeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/03/2011
-- Description:	Deletes a record from ZGWSecurity.Roles_Security_Entities_Accounts
--	given the RolesSecurityEntitiesSeqId and SecurityEntitySeqId
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Roles_Accounts]
	@P_ROLE_SEQ_ID AS INT,
	@P_SecurityEntitySeqId AS INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Roles_Accounts]'
	DECLARE @V_RolesSecurityEntitiesSeqId AS INT = (SELECT RolesSecurityEntitiesSeqId
FROM ZGWSecurity.Roles_Security_Entities
WHERE RoleSeqId = @P_ROLE_SEQ_ID)
	DELETE
		ZGWSecurity.Roles_Security_Entities_Accounts
	WHERE
		RolesSecurityEntitiesSeqId IN (
			SELECT
	RolesSecurityEntitiesSeqId
FROM
	ZGWSecurity.Roles_Security_Entities
WHERE 
				RolesSecurityEntitiesSeqId = @V_RolesSecurityEntitiesSeqId
	AND SecurityEntitySeqId = @P_SecurityEntitySeqId
		)
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Roles_Accounts]'
RETURN 0

GO

