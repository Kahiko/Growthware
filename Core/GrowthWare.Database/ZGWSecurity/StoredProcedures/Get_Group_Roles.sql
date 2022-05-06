
/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId AS INT,
		@P_GroupSeqId AS INT,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Group_Roles
		@PSecurityEntitySeqId,
		@P_GroupSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description:	Retrievs all roles given the 
--	group id and secruity entity id
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Group_Roles]
	@PSecurityEntitySeqId AS INT,
	@P_GroupSeqId AS INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Group_Roles'

	SELECT 
		[Name] AS [Role] 
	FROM 
		ZGWSecurity.Roles WITH(NOLOCK) 
	WHERE 
		RoleSeqId IN 
			(SELECT 
				RoleSeqId 
			FROM 
				ZGWSecurity.Roles_Security_Entities WITH(NOLOCK) 
			WHERE 
				RolesSecurityEntitiesSeqId IN 
				(SELECT 
					RolesSecurityEntitiesSeqId 
				FROM 
					ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities WITH(NOLOCK) 
				WHERE GroupsSecurityEntitiesSeqId IN 
					(SELECT 
						GroupsSecurityEntitiesSeqId 
					FROM 
						ZGWSecurity.Groups_Security_Entities WITH(NOLOCK) 
					WHERE 
						SecurityEntitySeqId = @PSecurityEntitySeqId AND GroupSeqId = @P_GroupSeqId)))
	ORDER BY
		[Role]

	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Group_Roles'
RETURN 0

GO

