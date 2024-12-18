
/*
Usage:
	DECLARE
		@P_NVPSeqId int = 1,
		@P_SecurityEntitySeqId int = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Name_Value_Pair_Groups
		@P_NVPSeqId,
		@P_SecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/22/2011
-- Description:	Returns groups associated with
--	Name Value Pairs 
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Name_Value_Pair_Groups]
	@P_NVPSeqId int = 1,
	@P_SecurityEntitySeqId int = 1,
	@P_Debug INT = 1
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Start Get_Name_Value_Pair_Groups'
	SELECT
	ZGWSecurity.Groups.[Name] AS GROUPS
FROM
	ZGWSecurity.Groups_Security_Entities_Permissions,
	ZGWSecurity.Groups_Security_Entities,
	ZGWSecurity.Groups
WHERE
		ZGWSecurity.Groups_Security_Entities_Permissions.NVPSeqId = @P_NVPSeqId
	AND ZGWSecurity.Groups_Security_Entities_Permissions.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId
	AND ZGWSecurity.Groups_Security_Entities.GroupSeqId = ZGWSecurity.Groups.GroupSeqId
	AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId = @P_SecurityEntitySeqId
ORDER BY
		GROUPS
	IF @P_Debug = 1 PRINT 'End Get_Name_Value_Pair_Groups'
RETURN 0

GO

