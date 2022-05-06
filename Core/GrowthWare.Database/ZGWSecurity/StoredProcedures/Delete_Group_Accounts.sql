
/*
Usage:
	DECLARE 
		@P_GroupSeqId AS INT = 2,
		@PSecurityEntitySeqId AS INT = 1

	exec  [ZGWSecurity].[Delete_Group_Accounts]
		@P_GroupSeqId
		@PSecurityEntitySeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/03/2011
-- Description:	Deletes a record from ZGWSecurity.Groups_Security_Entities_Accounts
--	given theGroupSeqId and SecurityEntitySeqId
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Group_Accounts]
	@P_GroupSeqId AS INT,
	@PSecurityEntitySeqId AS INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Group_Accounts]'
	DELETE
		ZGWSecurity.Groups_Security_Entities_Accounts
	WHERE
		Groups_Security_Entities_SeqID IN (
			SELECT 
				Groups_Security_Entities_SeqID 
			FROM 
				ZGWSecurity.Groups_Security_Entities 
			WHERE 
				GroupSeqId = @P_GroupSeqId 
				AND SecurityEntitySeqId = @PSecurityEntitySeqId
		)
	IF @P_Debug = 1 PRINT 'End [ZGWSecurity].[Delete_Group_Accounts]'
RETURN 0

GO

