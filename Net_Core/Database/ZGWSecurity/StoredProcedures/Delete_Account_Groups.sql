
/*
Usage:
	DECLARE 
		@P_AccountSeqId int = 4,
		@P_SecurityEntitySeqId	INT = 1,
		@P_ErrorCode int

	exec  ZGWSecurity.Delete_Account_Groups
		@P_AccountSeqId,
		@P_SecurityEntitySeqId,
		@P_ErrorCode
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from ZGWSecurity.Groups_Security_Entities_Accounts 
--	given the AccountSeqId and SecurityEntitySeqId
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Account_Groups]
	@P_AccountSeqId INT,
	@P_SecurityEntitySeqId	INT,
	@P_Debug INT = 0
AS
BEGIN
	IF @P_Debug = 1 PRINT 'Starting [ZGWSecurity].[Delete_Account_Groups]'
	DELETE FROM 
		ZGWSecurity.Groups_Security_Entities_Accounts 
	WHERE 
		GroupsSecurityEntitiesSeqId IN(SELECT GroupsSecurityEntitiesSeqId
		FROM ZGWSecurity.Groups_Security_Entities
		WHERE SecurityEntitySeqId = @P_SecurityEntitySeqId)
		AND AccountSeqId = @P_AccountSeqId
	IF @P_Debug = 1 PRINT 'Ending [ZGWSecurity].[Delete_Account_Groups]'
END
RETURN 0

GO

