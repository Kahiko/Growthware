/*
Usage:
	DECLARE 
		@P_Account_SeqID int = 4,
		@P_Security_Entity_SeqID	INT = 1,
		@P_ErrorCode int

	exec  ZGWSecurity.Delete_Account_Groups
		@P_Account_SeqID,
		@P_Security_Entity_SeqID,
		@P_ErrorCode
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from ZGWSecurity.Groups_Security_Entities_Accounts 
--	given the Account_SeqID and Security_Entity_SeqID
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Account_Groups]
	@P_Account_SeqID INT,
	@P_Security_Entity_SeqID	INT,
	@P_Debug INT = 0
 AS
BEGIN
	IF @P_Debug = 1 PRINT 'Starting [ZGWSecurity].[Delete_Account_Groups]'
	DELETE FROM 
		ZGWSecurity.Groups_Security_Entities_Accounts 
	WHERE 
		Groups_Security_Entities_SeqID IN(SELECT Groups_Security_Entities_SeqID FROM ZGWSecurity.Groups_Security_Entities WHERE Security_Entity_SeqID = @P_Security_Entity_SeqID)
		AND Account_SeqID = @P_Account_SeqID
	IF @P_Debug = 1 PRINT 'Ending [ZGWSecurity].[Delete_Account_Groups]'
END
RETURN 0