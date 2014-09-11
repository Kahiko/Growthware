/*
Usage:
	DECLARE 
		@P_Function_SeqID int = 4,
		@P_Security_Entity_SeqID	INT = 1,
		@P_Permissions_NVP_Detail_SeqID INT = 1,
		@P_ErrorCode int,
		@P_Debug INT = 1

	exec ZGWSecurity.Delete_Function_Groups
		@P_Function_SeqID,
		@P_Security_Entity_SeqID,
		@P_Permissions_NVP_Detail_SeqID,
		@P_ErrorCode OUT,
		@P_Debug BIT
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from ZGWSecurity.Groups_Security_Entities_Functions 
--	given the Function_SeqID and Security_Entity_SeqID
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Function_Groups]
	@P_Function_SeqID INT,
	@P_Security_Entity_SeqID	INT,
	@P_Permissions_NVP_Detail_SeqID INT,
	@P_ErrorCode INT OUTPUT,
	@P_Debug INT = 0
 AS
BEGIN
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Delete_Function_Groups'
	DELETE FROM 
		ZGWSecurity.Groups_Security_Entities_Functions
	WHERE 
		Groups_Security_Entities_SeqID IN(SELECT Groups_Security_Entities_SeqID FROM ZGWSecurity.Groups_Security_Entities WHERE Security_Entity_SeqID = @P_Security_Entity_SeqID)
		AND Function_SeqID = @P_Function_SeqID
		AND Permissions_NVP_Detail_SeqID = @P_Permissions_NVP_Detail_SeqID
	SELECT @P_ErrorCode = @@error
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Function_Groups'
END
RETURN 0