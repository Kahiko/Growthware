/*
Usage:
	DECLARE 
		@P_NVP_SeqID int = 3,
		@P_Security_Entity_SeqID	INT = 1,
		@P_ErrorCode int

	exec ZGWSystem.Delete_Name_Value_Pair
		@P_NVP_SeqID,
		@P_Security_Entity_SeqID,
		@P_ErrorCode
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from ZGWSystem.Name_Value_Pairs
--	given the NVP_SeqID
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Delete_Name_Value_Pair]
	@P_NVP_SeqID INT,
	@P_Security_Entity_SeqID	INT,
	@P_Debug INT = 0
 AS
BEGIN
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Delete_Name_Value_Pair'
	DELETE FROM ZGWSystem.Name_Value_Pairs
	WHERE 
		NVP_SeqID = @P_NVP_SeqID
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Name_Value_Pair'
END
RETURN 0