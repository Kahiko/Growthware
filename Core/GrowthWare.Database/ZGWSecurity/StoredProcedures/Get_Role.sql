
/*
Usage:
	DECLARE 
		@P_RoleSeqId AS INT = -1,
		@PSecurityEntitySeqId AS INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Role
		@P_RoleSeqId,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description:	Retrieves roles given the
--	the RoleSeqId and SecurityEntitySeqId
-- Note:
--	RoleSeqId of -1 returns all roles.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Role]
	@P_RoleSeqId INT,
	@PSecurityEntitySeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Start ZGWSecurity.Get_Role and SELECT an existing row from the table.'
	IF @P_RoleSeqId > -1 -- SELECT an existing row from the table.
		SELECT
			ZGWSecurity.Roles.[RoleSeqId] AS ROLE_SEQ_ID,
			ZGWSecurity.Roles.[Name],
			ZGWSecurity.Roles.[Description],
			ZGWSecurity.Roles.[Is_System],
			ZGWSecurity.Roles.[Is_System_Only],
			ZGWSecurity.Roles.[Added_By],
			ZGWSecurity.Roles.[Added_Date],
			ZGWSecurity.Roles.[Updated_By],
			ZGWSecurity.Roles.[Updated_Date]
		FROM
			ZGWSecurity.Roles
		WHERE
			RoleSeqId = @P_RoleSeqId
	ELSE -- GET ALL ROLES FOR A GIVEN Security Entity
		IF @P_Debug = 1 PRINT 'GET ALL ROLES FOR A GIVEN Security Entity.'
		SELECT
			ZGWSecurity.Roles.[RoleSeqId] AS ROLE_SEQ_ID,
			ZGWSecurity.Roles.[Name],
			ZGWSecurity.Roles.[Description],
			ZGWSecurity.Roles.[Is_System],
			ZGWSecurity.Roles.[Is_System_Only],
			ZGWSecurity.Roles.[Added_By],
			ZGWSecurity.Roles.[Added_Date],
			ZGWSecurity.Roles.[Updated_By],
			ZGWSecurity.Roles.[Updated_Date]
		FROM
			ZGWSecurity.Roles,
			ZGWSecurity.Roles_Security_Entities
		WHERE
			ZGWSecurity.Roles.RoleSeqId = ZGWSecurity.Roles_Security_Entities.RoleSeqId
			AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId = @PSecurityEntitySeqId
		ORDER BY
			ZGWSecurity.Roles.[Name]
	-- END IF		
	IF @P_Debug = 1 PRINT 'End ZGWSecurity.Get_Role'
RETURN 0

GO

