
/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Function_Security
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description:	Returns all Roles for all functions
--	given the SecurityEntitySeqId and NVP_DetailSeqId from
--	ZGWSecurity.Permissions or PermissionsNVPDetailSeqId
--	from ZGWSecurity.Groups_Security_Entities_Functions and 
--	ZGWSecurity.Roles_Security_Entities_Functions
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Function_Security]
	@PSecurityEntitySeqId int = -1,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Function_Security'
	DECLARE @V_AvalibleItems TABLE (FUNCTION_SEQ_ID INT, PERMISSIONS_SEQ_ID INT, ROLE VARCHAR(50))
	INSERT INTO @V_AvalibleItems
		SELECT DISTINCT -- Directly assigned Roles
			Functions.FunctionSeqId,
			[Permissions].NVP_DetailSeqId,
			Roles.[Name] AS [ROLE]
		FROM
			ZGWSecurity.Roles_Security_Entities Roles_Security_Entities WITH(NOLOCK),
			ZGWSecurity.Roles Roles WITH(NOLOCK),
			ZGWSecurity.Roles_Security_Entities_Functions [Security] WITH(NOLOCK),
			ZGWSecurity.Functions WITH(NOLOCK),
			ZGWSecurity.[Permissions] WITH(NOLOCK)
		WHERE
			Roles_Security_Entities.RoleSeqId = Roles.RoleSeqId
			AND [Security].RolesSecurityEntitiesSeqId = Roles_Security_Entities.RolesSecurityEntitiesSeqId
			AND [Security].FunctionSeqId = [FUNCTIONS].FunctionSeqId
			AND [Permissions].NVP_DetailSeqId = SECURITY.PermissionsNVPDetailSeqId
			AND Roles_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))
		UNION
		SELECT DISTINCT -- Roles assigned via groups
			Functions.FunctionSeqId,
			[Permissions].NVP_DetailSeqId,
			Roles.[Name] AS [ROLE]
		FROM
			ZGWSecurity.Groups_Security_Entities_Functions WITH(NOLOCK),
			ZGWSecurity.Groups_Security_Entities WITH(NOLOCK),
			ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities WITH(NOLOCK),
			ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
			ZGWSecurity.Roles Roles,
			ZGWSecurity.Functions WITH(NOLOCK),
			ZGWSecurity.[Permissions] WITH(NOLOCK)
		WHERE
			ZGWSecurity.Groups_Security_Entities_Functions.FunctionSeqId = [FUNCTIONS].FunctionSeqId
			AND ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Functions.GroupsSecurityEntitiesSeqId
			AND ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId
			AND ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.RolesSecurityEntitiesSeqId
			AND Roles.RoleSeqId = ZGWSecurity.Roles_Security_Entities.RoleSeqId
			AND [Permissions].NVP_DetailSeqId = ZGWSecurity.Groups_Security_Entities_Functions.PermissionsNVPDetailSeqId
			AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))

	IF (SELECT COUNT(*) FROM @V_AvalibleItems) > 0
		BEGIN
			SELECT
				*			
			FROM 
				@V_AvalibleItems
			ORDER BY
				FUNCTION_SEQ_ID
				,[ROLE]

			EXEC ZGWSecurity.Get_Function_Roles @PSecurityEntitySeqId, -1, -1, @P_Debug

			EXEC ZGWSecurity.Get_Function_Groups @PSecurityEntitySeqId, -1, -1, @P_Debug

		END
	ELSE
		BEGIN
			IF @P_Debug = 1 
				BEGIN
					PRINT 'No Security Information was not found '
					PRINT 'Now settings the ParentSecurityEntitySeqId '
					PRINT 'the defaul Security_Entity and executing '
					PRINT 'ZGWSecurity.Get_Function_Security'
				END
			--END IF
			UPDATE ZGWSecurity.Security_Entities
				SET 
					ParentSecurityEntitySeqId = ZGWSecurity.Get_Default_Entity_ID()
				WHERE
					SecurityEntitySeqId = @PSecurityEntitySeqId
			EXEC ZGWSecurity.Get_Function_Security @PSecurityEntitySeqId, NULL
		END
	-- END IF
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Function_Security'
RETURN 0

GO

