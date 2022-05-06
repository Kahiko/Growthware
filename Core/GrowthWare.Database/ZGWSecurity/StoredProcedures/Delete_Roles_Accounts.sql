
/*
Usage:
	DECLARE 
		@P_Roles_Security_Entities_SeqID AS INT = 2,
		@PSecurityEntitySeqId AS INT = 1

	exec  [ZGWSecurity].[Delete_Roles_Accounts]
		@P_Roles_Security_Entities_SeqID
		@PSecurityEntitySeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/03/2011
-- Description:	Deletes a record from ZGWSecurity.Roles_Security_Entities_Accounts
--	given the Roles_Security_Entities_SeqID and SecurityEntitySeqId
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Roles_Accounts]
	@P_ROLE_SEQ_ID AS INT,
	@PSecurityEntitySeqId AS INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Roles_Accounts]'
	DECLARE @V_Roles_Security_Entities_SeqID AS INT = (SELECT Roles_Security_Entities_SeqID FROM ZGWSecurity.Roles_Security_Entities WHERE RoleSeqId = @P_ROLE_SEQ_ID)
	DELETE
		ZGWSecurity.Roles_Security_Entities_Accounts
	WHERE
		Roles_Security_Entities_SeqID IN (
			SELECT 
				Roles_Security_Entities_SeqID 
			FROM 
				ZGWSecurity.Roles_Security_Entities 
			WHERE 
				Roles_Security_Entities_SeqID = @V_Roles_Security_Entities_SeqID
				AND SecurityEntitySeqId = @PSecurityEntitySeqId
		)
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Roles_Accounts]'
RETURN 0

GO

