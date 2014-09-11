/*
Usage:
	DECLARE
		@P_NVP_SeqID int = 1,
		@P_Security_Entity_SeqID int = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Name_Value_Pair_Roles
		@P_NVP_SeqID,
		@P_Security_Entity_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/22/2011
-- Description:	Returns roles associated with
--	Name Value Pairs 
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Name_Value_Pair_Roles]
		@P_NVP_SeqID int = 1,
		@P_Security_Entity_SeqID int = 1,
		@P_Debug INT = 1
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Start Get_Name_Value_Pair_Roles'
	SELECT
		ZGWSecurity.Roles.[NAME] AS ROLES
	FROM
		ZGWSecurity.Roles_Security_Entities_Permissions,
		ZGWSecurity.Roles_Security_Entities,
		ZGWSecurity.Roles
	WHERE
		ZGWSecurity.Roles_Security_Entities_Permissions.NVP_SeqID = @P_NVP_SeqID
		AND ZGWSecurity.Roles_Security_Entities_Permissions.Roles_Security_Entities_SeqID = ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID
		AND ZGWSecurity.Roles_Security_Entities.Role_SeqID = ZGWSecurity.Roles.Role_SeqID
		AND ZGWSecurity.Roles_Security_Entities.Security_Entity_SeqID = @P_Security_Entity_SeqID
	ORDER BY
		ROLES
	IF @P_Debug = 1 PRINT 'Start Get_Name_Value_Pair_Roles'
RETURN 0