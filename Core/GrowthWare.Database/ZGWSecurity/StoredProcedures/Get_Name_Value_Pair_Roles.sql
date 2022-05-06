
/*
Usage:
	DECLARE
		@P_NVPSeqId int = 1,
		@PSecurityEntitySeqId int = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Name_Value_Pair_Roles
		@P_NVPSeqId,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/22/2011
-- Description:	Returns roles associated with
--	Name Value Pairs 
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Name_Value_Pair_Roles]
		@P_NVPSeqId int = 1,
		@PSecurityEntitySeqId int = 1,
		@P_Debug INT = 1
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Start Get_Name_Value_Pair_Roles'
	SELECT
		ZGWSecurity.Roles.[Name] AS ROLES
	FROM
		ZGWSecurity.Roles_Security_Entities_Permissions,
		ZGWSecurity.Roles_Security_Entities,
		ZGWSecurity.Roles
	WHERE
		ZGWSecurity.Roles_Security_Entities_Permissions.NVPSeqId = @P_NVPSeqId
		AND ZGWSecurity.Roles_Security_Entities_Permissions.RolesSecurityEntitiesSeqId = ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId
		AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
		AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId = @PSecurityEntitySeqId
	ORDER BY
		ROLES
	IF @P_Debug = 1 PRINT 'Start Get_Name_Value_Pair_Roles'
RETURN 0

GO

