
/*
Usage:
	DECLARE 
		@P_NVPSeqId int = 3,
		@P_SecurityEntitySeqId	INT = 1,
		@P_ErrorCode int

	exec ZGWSystem.Delete_Name_Value_Pair
		@P_NVPSeqId,
		@P_SecurityEntitySeqId,
		@P_ErrorCode
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from ZGWSystem.Name_Value_Pairs
--	given the NVPSeqId
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Delete_Name_Value_Pair]
	@P_NVPSeqId INT,
	@P_SecurityEntitySeqId	INT,
	@P_Debug INT = 0
 AS
BEGIN
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Delete_Name_Value_Pair'
	DELETE FROM ZGWSystem.Name_Value_Pairs
	WHERE 
		NVPSeqId = @P_NVPSeqId
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Name_Value_Pair'
END
RETURN 0

GO

