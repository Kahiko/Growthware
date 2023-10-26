/*
Usage:
	DECLARE 
        @P_Source INT = 1
	  , @P_Target INT = 8
	  , @P_Added_Updated_By INT = 3;

	EXEC [ZGWSecurity].[Copy_Function_Security]
        @P_Source
	  , @P_Target
	  , @P_Added_Updated_By
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 10/22/2023
-- Description:	"Copies the group and role security for all functions"
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Copy_Function_Security]
      @P_Source INT
    , @P_Target INT
    , @P_Added_Updated_By INT
    , @P_Debug INT = 0
AS
BEGIN
    SET NOCOUNT ON;
-- Delete any Roles and Groups associated with the target (rely's on FK settings to delete from subsequent tables
    -- Groups_Security_Entities_Roles_Security_Entities does not have a FK cascade delete so we need to manually delete the records
	DELETE FROM 
        [ZGWSecurity].[Groups_Security_Entities_Roles_Security_Entities]
    WHERE 
        [RolesSecurityEntitiesSeqId] IN (SELECT [RolesSecurityEntitiesSeqId] FROM [ZGWSecurity].[Roles_Security_Entities] WHERE [SecurityEntitySeqId] =  @p_Target)

    DELETE FROM [ZGWSecurity].[Roles_Security_Entities] WHERE [SecurityEntitySeqId] =  @p_Target
    DELETE FROM [ZGWSecurity].[Groups_Security_Entities] WHERE [SecurityEntitySeqId] =  @p_Target
-- Associate Roles with the target
	INSERT INTO [ZGWSecurity].[Roles_Security_Entities]
	SELECT 
		  @P_Target
		, [RoleSeqId]
		, [Added_By]
		, GETDATE()
	FROM [ZGWSecurity].[Roles_Security_Entities] WITH(NOLOCK)
	WHERE [SecurityEntitySeqId] = @P_Source
-- Associate Groups with the target
	INSERT INTO [ZGWSecurity].[Groups_Security_Entities]
	SELECT 
		  @P_Target
		, [GroupsSecurityEntitiesSeqId]
		, [Added_By]
		, GETDATE()
	FROM [ZGWSecurity].[Groups_Security_Entities] WITH(NOLOCK)
	WHERE [SecurityEntitySeqId] = @P_Source
-- Associatet Roles with the Functions
	INSERT INTO [ZGWSecurity].[Roles_Security_Entities_Functions]
	SELECT
		 [RolesSecurityEntitiesSeqId] = (SELECT [RolesSecurityEntitiesSeqId] FROM [ZGWSecurity].[Roles_Security_Entities] WHERE [SecurityEntitySeqId] = @P_Target AND [RoleSeqId] = RSE.[RoleSeqId])
		,RSEF.[FunctionSeqId]
		,RSEF.[PermissionsNVPDetailSeqId]
		,@P_Added_Updated_By
		,GETDATE()
	FROM [ZGWSecurity].[Roles_Security_Entities] RSE
		INNER JOIN [ZGWSecurity].[Roles_Security_Entities_Functions] RSEF ON 1=1
			AND RSE.[SecurityEntitySeqId] = @P_Source
			AND RSEF.[RolesSecurityEntitiesSeqId] = RSE.[RolesSecurityEntitiesSeqId]
-- Associate Groups with the Functions
	INSERT INTO [ZGWSecurity].[Groups_Security_Entities_Functions]
	SELECT
		 [GroupsSecurityEntitiesSeqId] = (SELECT [GroupsSecurityEntitiesSeqId] FROM [ZGWSecurity].[Groups_Security_Entities_Functions] WHERE [SecurityEntitySeqId] = @P_Target AND [GroupSeqId] = GSE.[GroupSeqId])
		,GSEF.[FunctionSeqId]
		,GSEF.[PermissionsNVPDetailSeqId]
		,@P_Added_Updated_By
		,GETDATE()
	FROM [ZGWSecurity].[Groups_Security_Entities] GSE
		INNER JOIN [ZGWSecurity].[Groups_Security_Entities_Functions] GSEF ON 1=1
			AND GSE.[SecurityEntitySeqId] = @P_Source
			AND GSEF.[GroupsSecurityEntitiesSeqId] = GSE.[GroupsSecurityEntitiesSeqId]
END
RETURN 0

GO