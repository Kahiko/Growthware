
/*
Usage:
	DECLARE 
		@P_SecurityEntitySeqId INT = 1,
		@P_FunctionSeqId INT = 1,
		@P_PermissionsSeqId INT = 1
		@P_Debug INT = 0

	exec ZGWSecurity.Get_Function_Roles
		@P_SecurityEntitySeqId,
		@P_FunctionSeqId,
		@P_PermissionsSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/15/2011
-- Description:	Selects roles given the security entity
--	function and permission.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Function_Roles]
	@P_SecurityEntitySeqId INT,
	@P_FunctionSeqId INT,
	@P_PermissionsSeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Function_Roles'
	IF @P_FunctionSeqId > 0
		BEGIN
			SELECT
				ZGWSecurity.Roles.[Name] AS Roles
			FROM
				ZGWSecurity.Functions WITH(NOLOCK),
				ZGWSecurity.Roles_Security_Entities_Functions WITH(NOLOCK),
				ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
				ZGWSecurity.Roles WITH(NOLOCK)
			WHERE
				ZGWSecurity.Functions.FunctionSeqId = @P_FunctionSeqId
				AND ZGWSecurity.Functions.FunctionSeqId = ZGWSecurity.Roles_Security_Entities_Functions.FunctionSeqId
				AND ZGWSecurity.Roles_Security_Entities_Functions.RolesSecurityEntitiesSeqId = ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId
				AND ZGWSecurity.Roles_Security_Entities_Functions.PermissionsNVPDetailSeqId = @P_PermissionsSeqId
				AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
				AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId = @P_SecurityEntitySeqId
			ORDER BY
				Roles
		END
	ELSE
		BEGIN
			SELECT
				ZGWSecurity.Functions.FunctionSeqId AS 'Function_Seq_ID'
				,ZGWSecurity.Roles_Security_Entities_Functions.PermissionsNVPDetailSeqId AS 'PERMISSIONS_SEQ_ID'
				,ZGWSecurity.Roles.[Name] AS Role
			FROM
				ZGWSecurity.Functions WITH(NOLOCK),
				ZGWSecurity.Roles_Security_Entities_Functions WITH(NOLOCK),
				ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
				ZGWSecurity.Roles WITH(NOLOCK)
			WHERE
				ZGWSecurity.Functions.FunctionSeqId = ZGWSecurity.Roles_Security_Entities_Functions.FunctionSeqId
				AND ZGWSecurity.Roles_Security_Entities_Functions.RolesSecurityEntitiesSeqId = ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId
				AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
				AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId = @P_SecurityEntitySeqId
			ORDER BY
				ZGWSecurity.Functions.FunctionSeqId
				,ZGWSecurity.Roles_Security_Entities_Functions.PermissionsNVPDetailSeqId
		END
	--END IF
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Function_Roles'

RETURN 0

GO

