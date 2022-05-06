
/*
Usage:
	DECLARE 
		@P_Groups_Security_EntitiesSeqId AS INT = 2,
		@PSecurityEntitySeqId AS INT = 1

	exec  [ZGWSecurity].[Delete_Groups_Accounts]
		@P_Groups_Security_EntitiesSeqId
		@PSecurityEntitySeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/03/2011
-- Description:	Deletes a record from ZGWSecurity.Groups_Security_Entities_Accounts
--	given the Groups_Security_EntitiesSeqId and SecurityEntitySeqId
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Groups_Accounts]
	@P_Groups_Security_EntitiesSeqId AS INT,
	@PSecurityEntitySeqId AS INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Groups_Accounts]'
	DELETE
		ZGWSecurity.Groups_Security_Entities_Accounts
	WHERE
		Groups_Security_EntitiesSeqId IN (
			SELECT 
				Groups_Security_EntitiesSeqId 
			FROM 
				ZGWSecurity.Groups_Security_Entities 
			WHERE 
				Groups_Security_EntitiesSeqId = @P_Groups_Security_EntitiesSeqId 
				AND SecurityEntitySeqId = @PSecurityEntitySeqId
		)
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Groups_Accounts]'
RETURN 0

GO

