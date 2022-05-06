
/*
Usage:
	DECLARE 
		@P_NVP_Detail_SeqID INT = 4,
		@P_NVP_SeqID int = 1,
		@P_Debug INT = 0

	exec [ZGWSystem].[Delete_Name_Value_Pair_Group]
		@P_NVP_Detail_SeqID,
		@P_NVP_SeqID,
		@P_Debug BIT
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/04/2011
-- Description:	Deletes a records from ZGWSecurity.Groups_Security_Entities_Permissions
--	given the NVP_SeqID, SecurityEntitySeqId, and Permissions_NVP_Detail_SeqID
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Delete_Groups_Security_Entities_Permissions]
	@P_NVP_SeqID INT,
	@PSecurityEntitySeqId INT,
	@P_Permissions_NVP_Detail_SeqID INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Start [ZGWSystem].[Delete_Groups_Security_Entities_Permissions]'
	DELETE FROM 
		ZGWSecurity.Groups_Security_Entities_Permissions
	WHERE 
		Groups_Security_Entities_SeqID IN(SELECT Groups_Security_Entities_SeqID FROM ZGWSecurity.Groups_Security_Entities WHERE SecurityEntitySeqId = @PSecurityEntitySeqId)
		AND NVP_SeqID = @P_NVP_SeqID
		AND Permissions_NVP_Detail_SeqID = @P_Permissions_NVP_Detail_SeqID
	IF @P_Debug = 1 PRINT 'End [ZGWSystem].[Delete_Groups_Security_Entities_Permissions]'
RETURN 0

GO

