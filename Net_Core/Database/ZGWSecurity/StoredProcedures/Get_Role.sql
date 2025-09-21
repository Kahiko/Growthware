/*
Usage:
DECLARE 
	@P_RoleSeqId AS INT = -1,
	@P_SecurityEntitySeqId AS INT = 6,
	@P_Debug INT = 1

EXEC [ZGWSecurity].[Get_Role]
	@P_RoleSeqId,
	@P_SecurityEntitySeqId,
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
-- Author:		Michael Regan
-- Create date: 05/27/2025
-- Description:	Fixed returning too much information needed to add the BEGIN/END keywords it worked bu
--	no need in returning too much information if it's not needed.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Role]
	@P_RoleSeqId INT,
	@P_SecurityEntitySeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Start ZGWSecurity.Get_Role and SELECT an existing row from the table.'
	IF @P_RoleSeqId > -1 -- SELECT an existing row from the table.
		BEGIN
			SELECT
				[Roles].[RoleSeqId] AS [ROLE_SEQ_ID],
				[Roles].[Name],
				[Roles].[Description],
				[Roles].[Is_System],
				[Roles].[Is_System_Only],
				[Roles].[Added_By],
				[Roles].[Added_Date],
				[Roles].[Updated_By],
				[Roles].[Updated_Date]
			FROM
				[ZGWSecurity].[Roles] AS [Roles]
			WHERE
				[Roles].RoleSeqId = @P_RoleSeqId
		END
	ELSE -- GET ALL ROLES FOR A GIVEN Security Entity
		BEGIN
			IF @P_Debug = 1 PRINT 'GET ALL ROLES FOR A GIVEN Security Entity.'
			SELECT
				[Roles].[RoleSeqId] AS ROLE_SEQ_ID,
				[Roles].[Name],
				[Roles].[Description],
				[Roles].[Is_System],
				[Roles].[Is_System_Only],
				[Roles].[Added_By],
				[Roles].[Added_Date],
				[Roles].[Updated_By],
				[Roles].[Updated_Date]
			FROM
				[ZGWSecurity].[Roles] AS [Roles]
				INNER JOIN [ZGWSecurity].[Roles_Security_Entities] AS [RSE] ON
					[RSE].[SecurityEntitySeqId] = @P_SecurityEntitySeqId
					AND [Roles].[RoleSeqId] = [RSE].[RoleSeqId]
			ORDER BY
				[Roles].[Name]
		END
	-- END IF		
	IF @P_Debug = 1 PRINT 'End ZGWSecurity.Get_Role'
RETURN 0
GO