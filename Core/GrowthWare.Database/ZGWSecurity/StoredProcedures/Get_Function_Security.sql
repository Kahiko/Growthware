
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
--	given the SecurityEntitySeqId and NVP_Detail_SeqID from
--	ZGWSecurity.Permissions or Permissions_NVP_Detail_SeqID
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
			[Permissions].NVP_Detail_SeqID,
			Roles.[Name] AS [ROLE]
		FROM
			ZGWSecurity.Roles_Security_Entities Roles_Security_Entities WITH(NOLOCK),
			ZGWSecurity.Roles Roles WITH(NOLOCK),
			ZGWSecurity.Roles_Security_Entities_Functions [Security] WITH(NOLOCK),
			ZGWSecurity.Functions WITH(NOLOCK),
			ZGWSecurity.[Permissions] WITH(NOLOCK)
		WHERE
			Roles_Security_Entities.RoleSeqId = Roles.RoleSeqId
			AND [Security].Roles_Security_Entities_SeqID = Roles_Security_Entities.Roles_Security_Entities_SeqID
			AND [Security].FunctionSeqId = [FUNCTIONS].FunctionSeqId
			AND [Permissions].NVP_Detail_SeqID = SECURITY.Permissions_NVP_Detail_SeqID
			AND Roles_Security_Entities.SecurityEntitySeqId IN (SELECT SecurityEntitySeqId FROM ZGWSecurity.Get_Entity_Parents(1,@PSecurityEntitySeqId))
		UNION
		SELECT DISTINCT -- Roles assigned via groups
			Functions.FunctionSeqId,
			[Permissions].NVP_Detail_SeqID,
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
			AND ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Functions.Groups_Security_Entities_SeqID
			AND ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID
			AND ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Roles_Security_Entities_SeqID
			AND Roles.RoleSeqId = ZGWSecurity.Roles_Security_Entities.RoleSeqId
			AND [Permissions].NVP_Detail_SeqID = ZGWSecurity.Groups_Security_Entities_Functions.Permissions_NVP_Detail_SeqID
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

