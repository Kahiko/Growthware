
/*
Usage:
	DECLARE 
		@P_NVP_DetailSeqId INT = 4,
		@P_NVPSeqId int = 1,
		@P_Debug INT = 0

	exec [ZGWSystem].[Delete_Name_Value_Pair_Group]
		@P_NVP_DetailSeqId,
		@P_NVPSeqId,
		@P_Debug BIT
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/04/2011
-- Description:	Deletes a records from ZGWSecurity.Groups_Security_Entities_Permissions
--	given the NVPSeqId, SecurityEntitySeqId, and PermissionsNVPDetailSeqId
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Delete_Groups_Security_Entities_Permissions]
	@P_NVPSeqId INT,
	@PSecurityEntitySeqId INT,
	@P_PermissionsNVPDetailSeqId INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Start [ZGWSystem].[Delete_Groups_Security_Entities_Permissions]'
	DELETE FROM 
		ZGWSecurity.Groups_Security_Entities_Permissions
	WHERE 
		GroupsSecurityEntitiesSeqId IN(SELECT GroupsSecurityEntitiesSeqId FROM ZGWSecurity.Groups_Security_Entities WHERE SecurityEntitySeqId = @PSecurityEntitySeqId)
		AND NVPSeqId = @P_NVPSeqId
		AND PermissionsNVPDetailSeqId = @P_PermissionsNVPDetailSeqId
	IF @P_Debug = 1 PRINT 'End [ZGWSystem].[Delete_Groups_Security_Entities_Permissions]'
RETURN 0

GO

