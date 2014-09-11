/*
Usage:
	DECLARE 
		@P_Groups_Security_Entities_SeqID AS INT = 2,
		@P_Security_Entity_SeqID AS INT = 1

	exec  [ZGWSecurity].[Delete_Groups_Accounts]
		@P_Groups_Security_Entities_SeqID
		@P_Security_Entity_SeqID
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/03/2011
-- Description:	Deletes a record from ZGWSecurity.Groups_Security_Entities_Accounts
--	given the Groups_Security_Entities_SeqID and Security_Entity_SeqID
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Groups_Accounts]
	@P_Groups_Security_Entities_SeqID AS INT,
	@P_Security_Entity_SeqID AS INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Groups_Accounts]'
	DELETE
		ZGWSecurity.Groups_Security_Entities_Accounts
	WHERE
		Groups_Security_Entities_SeqID IN (
			SELECT 
				Groups_Security_Entities_SeqID 
			FROM 
				ZGWSecurity.Groups_Security_Entities 
			WHERE 
				Groups_Security_Entities_SeqID = @P_Groups_Security_Entities_SeqID 
				AND Security_Entity_SeqID = @P_Security_Entity_SeqID
		)
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Groups_Accounts]'
RETURN 0