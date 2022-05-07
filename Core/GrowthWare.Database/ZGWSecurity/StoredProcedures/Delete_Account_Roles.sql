
/*
Usage:
	DECLARE 
		@P_AccountSeqId int = 4,
		@P_SecurityEntitySeqId	INT = 1,
		@P_ErrorCode int

	exec ZGWSecurity.Delete_Account_Roles
		@P_AccountSeqId,
		@P_SecurityEntitySeqId,
		@P_ErrorCode
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from ZGWSecurity.Roles_Security_Entities_Accounts
--	given the AccountSeqId and SecurityEntitySeqId
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Account_Roles]
	@P_AccountSeqId INT,
	@P_SecurityEntitySeqId	INT,
	@P_ErrorCode INT OUTPUT,
	@P_Debug INT = 0
 AS
BEGIN
	IF @P_Debug = 1 PRINT 'Start [ZGWSecurity].[Delete_Account_Roles]'
	DELETE FROM 
		ZGWSecurity.Roles_Security_Entities_Accounts 
	WHERE 
		RolesSecurityEntitiesSeqId IN(SELECT RolesSecurityEntitiesSeqId FROM ZGWSecurity.Roles_Security_Entities WHERE SecurityEntitySeqId = @P_SecurityEntitySeqId)
		AND AccountSeqId = @P_AccountSeqId
	SELECT @P_ErrorCode = @@error
	IF @P_Debug = 1 PRINT 'End [ZGWSecurity].[Delete_Account_Roles]'
END
RETURN 0

GO

