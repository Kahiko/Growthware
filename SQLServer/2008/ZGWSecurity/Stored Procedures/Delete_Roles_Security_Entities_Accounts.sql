/*
Usage:
	DECLARE 
		@P_Role_SeqID AS INT = 2,
		@P_Security_Entity_SeqID AS INT = 1

	exec  [ZGWSecurity].[Delete_Roles_Security_Entities_Accounts]
		@P_Role_SeqID
		@P_Security_Entity_SeqID
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/04/2011
-- Description:	Deletes a record from ZGWSecurity.Roles_Security_Entities_Accounts
--	given the Groups_Security_Entities_SeqID and Security_Entity_SeqID
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Roles_Security_Entities_Accounts]
	@P_Role_SeqID AS INT,
	@P_Security_Entity_SeqID AS INT,
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
				Role_SeqID = @P_Role_SeqID 
				AND Security_Entity_SeqID = @P_Security_Entity_SeqID
		)
	IF @P_Debug = 1 PRINT 'Ending [ZGWSecurity].[Delete_Roles_Security_Entities_Accounts]'
RETURN 0