
/*
Usage:
	DECLARE 
		@P_GroupSeqId int = 4,
		@PSecurityEntitySeqId	INT = 1,
		@P_Debug INT = 0

	exec ZGWSecurity.Delete_Function_Groups
		@P_GroupSeqId,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/29/2011
-- Description:	Deletes a record from ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
--	given the GroupSeqId and SecurityEntitySeqId
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Group_Roles]
	@P_GroupSeqId INT,
	@PSecurityEntitySeqId INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Group_Roles]'
	DELETE
		ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
	WHERE
		Groups_Security_Entities_SeqID IN (SELECT Groups_Security_Entities_SeqID 
					FROM ZGWSecurity.Groups_Security_Entities 
					WHERE SecurityEntitySeqId=@PSecurityEntitySeqId
					AND GroupSeqId = @P_GroupSeqId)
	IF @P_Debug = 1 PRINT 'End [ZGWSecurity].[Delete_Group_Roles]'
RETURN 0

GO

