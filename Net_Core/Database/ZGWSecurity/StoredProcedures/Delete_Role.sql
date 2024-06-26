
/*
Usage:
	DECLARE 
		@P_Name AS VARCHAR(50) = 'MyRole',
		@P_SecurityEntitySeqId AS INT = 1

	exec ZGWSecurity.Delete_Role
		@P_Name
		@P_SecurityEntitySeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/03/2011
-- Description:	Deletes a record from ZGWSecurity.Roles,
--	0 to x records from ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities and ZGWSecurity.Roles_Security_Entities
--	given the roles name and the SecurityEntitySeqId
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Role]
	@P_Name VARCHAR (50),
	@P_SecurityEntitySeqId	INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Delete_Role'
	/*
	NOTE : ** CASCADE DELETE SHOULD BE TURNED ON IN
		ZGWSecurity.Roles_Security_Entities FOR THIS TO WORK ELSE
		THIS MIGHT THROW AN ERROR
		**** 
	*/
	DECLARE @V_RolesSeqId INT
			
	SET @V_RolesSeqId = (SELECT RoleSeqId
FROM ZGWSecurity.Roles
WHERE [Name] = @P_Name)

	BEGIN TRANSACTION
		BEGIN
	-- DELETE ROLE FROM Groups_Security_Entities_Roles_Security_Entities
	/*
				Note:  This should not be necessary ... cascade delete and triggers should
				handle this and deleting the record from ZGWSecurity.Roles should be sufficient
				... this would be "overkill" and or for other datastores that
				don't support cascade delete or triggers.
				... no i don't know of one off hand and yes i know this is for sql server :)
			*/
	IF @P_Debug = 1 PRINT 'Deleting roles from ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities'
	DELETE ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
			WHERE (RolesSecurityEntitiesSeqId = 
						(SELECT
		RolesSecurityEntitiesSeqId
	FROM
		ZGWSecurity.Roles_Security_Entities
	WHERE 
							RoleSeqId = @V_RolesSeqId
		AND SecurityEntitySeqId = @P_SecurityEntitySeqId
						)
					)
END 

		BEGIN
	-- DELETE ROLE FROM ZGWSecurity.Roles_Security_Entities
	IF @P_Debug = 1 PRINT 'Deleting roles from ZGWSecurity.Roles_Security_Entities'
	DELETE ZGWSecurity.Roles_Security_Entities
			WHERE (
				RoleSeqId= @V_RolesSeqId AND
		SecurityEntitySeqId = @P_SecurityEntitySeqId
				   )
END 
		BEGIN
	-- Delete the role from ZGWSecurity.Roles if no other entites are using the role
	IF @P_Debug = 1 PRINT 'Deleting role from ZGWSecurity.Roles'
	IF (SELECT COUNT(*)
	FROM
		ZGWSecurity.Roles Roles,
		ZGWSecurity.Roles_Security_Entities RoleEntities
	WHERE
				Roles.RoleSeqId = RoleEntities.RoleSeqId
		AND Roles.RoleSeqId = @V_RolesSeqId) = 0
			BEGIN
		IF @P_Debug = 1 PRINT 'Role is not used by other entites'
		DELETE ZGWSecurity.Roles
				WHERE (RoleSeqId = @V_RolesSeqId)
	END
END
	IF @@ERROR <> 0
	 BEGIN
	-- Rollback the transaction
	ROLLBACK
	-- Raise an error and return
	RAISERROR ('Error in deleting role in ZGWSecurity.Roles.', 16, 1)
	RETURN 1
END
	COMMIT
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Role'
	RETURN 0

GO

