
/*
Usage:
	DECLARE 
		@P_FunctionSeqId int = 4,
		@PSecurityEntitySeqId	INT = 1,
		@P_Permissions_NVP_DetailSeqId INT = 1,
		@P_ErrorCode int,
		@P_Debug INT = 1

	exec ZGWSecurity.Delete_Function_Groups
		@P_FunctionSeqId,
		@PSecurityEntitySeqId,
		@P_Permissions_NVP_DetailSeqId,
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
	@PSecurityEntitySeqId	INT,
	@P_Permissions_NVP_DetailSeqId INT,
	@P_ErrorCode INT OUTPUT,
	@P_Debug INT = 0
 AS
BEGIN
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Delete_Function_Groups'
	DELETE FROM 
		ZGWSecurity.Groups_Security_Entities_Functions
	WHERE 
		Groups_Security_EntitiesSeqId IN(SELECT Groups_Security_EntitiesSeqId FROM ZGWSecurity.Groups_Security_Entities WHERE SecurityEntitySeqId = @PSecurityEntitySeqId)
		AND FunctionSeqId = @P_FunctionSeqId
		AND Permissions_NVP_DetailSeqId = @P_Permissions_NVP_DetailSeqId
	SELECT @P_ErrorCode = @@error
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Function_Groups'
END
RETURN 0

GO

