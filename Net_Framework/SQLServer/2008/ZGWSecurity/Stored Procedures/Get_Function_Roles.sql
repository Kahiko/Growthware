/*
Usage:
	DECLARE 
		@P_Security_Entity_SeqID INT = 1,
		@P_Function_SeqID INT = 1,
		@P_Permissions_SeqID INT = 1
		@P_Debug INT = 0

	exec ZGWSecurity.Get_Function_Roles
		@P_Security_Entity_SeqID,
		@P_Function_SeqID,
		@P_Permissions_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/15/2011
-- Description:	Selects roles given the security entity
--	function and permission.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Function_Roles]
	@P_Security_Entity_SeqID INT,
	@P_Function_SeqID INT,
	@P_Permissions_SeqID INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Function_Roles'
	IF @P_Function_SeqID > 0
		BEGIN
			SELECT
				ZGWSecurity.Roles.[Name] AS Roles
			FROM
				ZGWSecurity.Functions WITH(NOLOCK),
				ZGWSecurity.Roles_Security_Entities_Functions WITH(NOLOCK),
				ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
				ZGWSecurity.Roles WITH(NOLOCK)
			WHERE
				ZGWSecurity.Functions.Function_SeqID = @P_Function_SeqID
				AND ZGWSecurity.Functions.Function_SeqID = ZGWSecurity.Roles_Security_Entities_Functions.Function_SeqID
				AND ZGWSecurity.Roles_Security_Entities_Functions.Roles_Security_Entities_SeqID = ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID
				AND ZGWSecurity.Roles_Security_Entities_Functions.Permissions_NVP_Detail_SeqID = @P_Permissions_SeqID
				AND ZGWSecurity.Roles_Security_Entities.Role_SeqID = ZGWSecurity.Roles.Role_SeqID
				AND ZGWSecurity.Roles_Security_Entities.Security_Entity_SeqID = @P_Security_Entity_SeqID
			ORDER BY
				Roles
		END
	ELSE
		BEGIN
			SELECT
				ZGWSecurity.Functions.Function_SeqID AS 'Function_Seq_ID'
				,ZGWSecurity.Roles_Security_Entities_Functions.Permissions_NVP_Detail_SeqID AS 'PERMISSIONS_SEQ_ID'
				,ZGWSecurity.Roles.[Name] AS Role
			FROM
				ZGWSecurity.Functions WITH(NOLOCK),
				ZGWSecurity.Roles_Security_Entities_Functions WITH(NOLOCK),
				ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
				ZGWSecurity.Roles WITH(NOLOCK)
			WHERE
				ZGWSecurity.Functions.Function_SeqID = ZGWSecurity.Roles_Security_Entities_Functions.Function_SeqID
				AND ZGWSecurity.Roles_Security_Entities_Functions.Roles_Security_Entities_SeqID = ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID
				AND ZGWSecurity.Roles_Security_Entities.Role_SeqID = ZGWSecurity.Roles.Role_SeqID
				AND ZGWSecurity.Roles_Security_Entities.Security_Entity_SeqID = @P_Security_Entity_SeqID
			ORDER BY
				ZGWSecurity.Functions.Function_SeqID
				,ZGWSecurity.Roles_Security_Entities_Functions.Permissions_NVP_Detail_SeqID
		END
	--END IF
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Function_Roles'

RETURN 0