/*
Usage:
	DECLARE 
		@P_Security_Entity_SeqID AS INT,
		@P_Group_SeqID AS INT,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Group_Roles
		@P_Security_Entity_SeqID,
		@P_Group_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description:	Retrievs all roles given the 
--	group id and secruity entity id
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Group_Roles]
	@P_Security_Entity_SeqID AS INT,
	@P_Group_SeqID AS INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Group_Roles'

	SELECT 
		[Name] AS [Role] 
	FROM 
		ZGWSecurity.Roles WITH(NOLOCK) 
	WHERE 
		Role_SeqID IN 
			(SELECT 
				Role_SeqID 
			FROM 
				ZGWSecurity.Roles_Security_Entities WITH(NOLOCK) 
			WHERE 
				Roles_Security_Entities_SeqID IN 
				(SELECT 
					Roles_Security_Entities_SeqID 
				FROM 
					ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities WITH(NOLOCK) 
				WHERE Groups_Security_Entities_SeqID IN 
					(SELECT 
						Groups_Security_Entities_SeqID 
					FROM 
						ZGWSecurity.Groups_Security_Entities WITH(NOLOCK) 
					WHERE 
						Security_Entity_SeqID = @P_Security_Entity_SeqID AND Group_SeqID = @P_Group_SeqID)))
	ORDER BY
		[Role]

	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Group_Roles'
RETURN 0