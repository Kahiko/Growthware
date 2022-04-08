
/*
Usage:
	DECLARE 
		@P_Account_SeqID int = 4,
		@P_Security_Entity_SeqID	INT = 1,
		@P_ErrorCode int

	exec ZGWSecurity.Delete_Account_Roles
		@P_Account_SeqID,
		@P_Security_Entity_SeqID,
		@P_ErrorCode
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from ZGWSecurity.Roles_Security_Entities_Accounts
--	given the Account_SeqID and Security_Entity_SeqID
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Account_Roles]
	@P_Account_SeqID INT,
	@P_Security_Entity_SeqID	INT,
	@P_ErrorCode INT OUTPUT,
	@P_Debug INT = 0
 AS
BEGIN
	IF @P_Debug = 1 PRINT 'Start [ZGWSecurity].[Delete_Account_Roles]'
	DELETE FROM 
		ZGWSecurity.Roles_Security_Entities_Accounts 
	WHERE 
		Roles_Security_Entities_SeqID IN(SELECT Roles_Security_Entities_SeqID FROM ZGWSecurity.Roles_Security_Entities WHERE Security_Entity_SeqID = @P_Security_Entity_SeqID)
		AND Account_SeqID = @P_Account_SeqID
	SELECT @P_ErrorCode = @@error
	IF @P_Debug = 1 PRINT 'End [ZGWSecurity].[Delete_Account_Roles]'
END
RETURN 0

GO

