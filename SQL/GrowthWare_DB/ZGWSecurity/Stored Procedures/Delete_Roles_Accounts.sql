/*
Usage:
	DECLARE 
		@P_Roles_Security_Entities_SeqID AS INT = 2,
		@P_Security_Entity_SeqID AS INT = 1

	exec  [ZGWSecurity].[Delete_Roles_Accounts]
		@P_Roles_Security_Entities_SeqID
		@P_Security_Entity_SeqID
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/03/2011
-- Description:	Deletes a record from ZGWSecurity.Roles_Security_Entities_Accounts
--	given the Roles_Security_Entities_SeqID and Security_Entity_SeqID
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Roles_Accounts]
	@P_ROLE_SEQ_ID AS INT,
	@P_Security_Entity_SeqID AS INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Roles_Accounts]'
	DECLARE @V_Roles_Security_Entities_SeqID AS INT = (SELECT Roles_Security_Entities_SeqID FROM ZGWSecurity.Roles_Security_Entities WHERE Role_SeqID = @P_ROLE_SEQ_ID)
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
				AND Security_Entity_SeqID = @P_Security_Entity_SeqID
		)
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Roles_Accounts]'
RETURN 0