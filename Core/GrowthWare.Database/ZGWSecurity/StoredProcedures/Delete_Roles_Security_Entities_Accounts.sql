
/*
Usage:
	DECLARE 
		@P_RoleSeqId AS INT = 2,
		@P_SecurityEntitySeqId AS INT = 1

	exec  [ZGWSecurity].[Delete_Roles_Security_Entities_Accounts]
		@P_RoleSeqId
		@P_SecurityEntitySeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/04/2011
-- Description:	Deletes a record from ZGWSecurity.Roles_Security_Entities_Accounts
--	given the GroupsSecurityEntitiesSeqId and SecurityEntitySeqId
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Roles_Security_Entities_Accounts]
	@P_RoleSeqId AS INT,
	@P_SecurityEntitySeqId AS INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting [ZGWSecurity].[Delete_Roles_Security_Entities_Accounts]'
	DELETE
		ZGWSecurity.Roles_Security_Entities_Accounts
	WHERE
		RolesSecurityEntitiesSeqId IN (
			SELECT 
				RolesSecurityEntitiesSeqId 
			FROM 
				ZGWSecurity.Roles_Security_Entities 
			WHERE 
				RoleSeqId = @P_RoleSeqId 
				AND SecurityEntitySeqId = @P_SecurityEntitySeqId
		)
	IF @P_Debug = 1 PRINT 'Ending [ZGWSecurity].[Delete_Roles_Security_Entities_Accounts]'
RETURN 0

GO

