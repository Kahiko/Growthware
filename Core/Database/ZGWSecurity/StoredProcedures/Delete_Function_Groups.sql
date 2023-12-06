
/*
Usage:
	DECLARE 
		@P_FunctionSeqId int = 4,
		@P_SecurityEntitySeqId	INT = 1,
		@P_PermissionsNVPDetailSeqId INT = 1,
		@P_ErrorCode int,
		@P_Debug INT = 1

	exec ZGWSecurity.Delete_Function_Groups
		@P_FunctionSeqId,
		@P_SecurityEntitySeqId,
		@P_PermissionsNVPDetailSeqId,
		@P_ErrorCode OUT,
		@P_Debug BIT
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from ZGWSecurity.Groups_Security_Entities_Functions 
--	given the FunctionSeqId and SecurityEntitySeqId
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Function_Groups]
	@P_FunctionSeqId INT,
	@P_SecurityEntitySeqId	INT,
	@P_PermissionsNVPDetailSeqId INT,
	@P_ErrorCode INT OUTPUT,
	@P_Debug INT = 0
AS
BEGIN
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Delete_Function_Groups'
	DELETE FROM 
		ZGWSecurity.Groups_Security_Entities_Functions
	WHERE 
		GroupsSecurityEntitiesSeqId IN(SELECT GroupsSecurityEntitiesSeqId
		FROM ZGWSecurity.Groups_Security_Entities
		WHERE SecurityEntitySeqId = @P_SecurityEntitySeqId)
		AND FunctionSeqId = @P_FunctionSeqId
		AND PermissionsNVPDetailSeqId = @P_PermissionsNVPDetailSeqId
	SELECT @P_ErrorCode = @@error
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Function_Groups'
END
RETURN 0

GO

