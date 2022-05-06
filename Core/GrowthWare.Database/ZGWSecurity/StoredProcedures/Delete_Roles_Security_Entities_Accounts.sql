
/*
Usage:
	DECLARE 
		@P_RoleSeqId AS INT = 2,
		@PSecurityEntitySeqId AS INT = 1

	exec  [ZGWSecurity].[Delete_Roles_Security_Entities_Accounts]
		@P_RoleSeqId
		@PSecurityEntitySeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/04/2011
-- Description:	Deletes a record from ZGWSecurity.Roles_Security_Entities_Accounts
--	given the Groups_Security_Entities_SeqID and SecurityEntitySeqId
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Roles_Security_Entities_Accounts]
	@P_RoleSeqId AS INT,
	@PSecurityEntitySeqId AS INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting [ZGWSecurity].[Delete_Roles_Security_Entities_Accounts]'
	DELETE
		ZGWSecurity.Roles_Security_Entities_Accounts
	WHERE
		Roles_Security_Entities_SeqID IN (
			SELECT 
				Roles_Security_Entities_SeqID 
			FROM 
				ZGWSecurity.Roles_Security_Entities 
			WHERE 
				RoleSeqId = @P_RoleSeqId 
				AND SecurityEntitySeqId = @PSecurityEntitySeqId
		)
	IF @P_Debug = 1 PRINT 'Ending [ZGWSecurity].[Delete_Roles_Security_Entities_Accounts]'
RETURN 0

GO

