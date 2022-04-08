
/*
Usage:
	DECLARE 
		@P_Role_SeqID AS INT = -1,
		@P_Security_Entity_SeqID AS INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Role
		@P_Role_SeqID,
		@P_Security_Entity_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description:	Retrieves roles given the
--	the Role_SeqID and Security_Entity_SeqID
-- Note:
--	Role_SeqID of -1 returns all roles.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Role]
	@P_Role_SeqID INT,
	@P_Security_Entity_SeqID INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Start ZGWSecurity.Get_Role and SELECT an existing row from the table.'
	IF @P_Role_SeqID > -1 -- SELECT an existing row from the table.
		SELECT
			ZGWSecurity.Roles.[Role_SeqID] AS ROLE_SEQ_ID,
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
			Role_SeqID = @P_Role_SeqID
	ELSE -- GET ALL ROLES FOR A GIVEN Security Entity
		IF @P_Debug = 1 PRINT 'GET ALL ROLES FOR A GIVEN Security Entity.'
		SELECT
			ZGWSecurity.Roles.[Role_SeqID] AS ROLE_SEQ_ID,
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
			ZGWSecurity.Roles.Role_SeqID = ZGWSecurity.Roles_Security_Entities.Role_SeqID
			AND ZGWSecurity.Roles_Security_Entities.Security_Entity_SeqID = @P_Security_Entity_SeqID
		ORDER BY
			ZGWSecurity.Roles.[Name]
	-- END IF		
	IF @P_Debug = 1 PRINT 'End ZGWSecurity.Get_Role'
RETURN 0

GO

