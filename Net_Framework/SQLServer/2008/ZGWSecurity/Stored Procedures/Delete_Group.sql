/*
Usage:
	DECLARE 
		@P_Group_SeqID int = 4,
		@P_Security_Entity_SeqID	INT = 1,
		@P_Debug INT = 0

	exec ZGWSecurity.Delete_Group
		@P_Group_SeqID,
		@P_Security_Entity_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from ZGWSecurity.Groups
--	given the Group_SeqID and Security_Entity_SeqID
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Group]
	@P_Group_SeqID INT,
	@P_Security_Entity_SeqID INT,
	@P_Debug INT = 0
 AS
BEGIN
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Delete_Group'
	/*
	NOTE : ** CASCADE DELETE SHOULD BE TURNED ON IN
		ZGWSecurity.Groups_Security_Entities FOR THIS TO WORK ELSE
		THIS MIGHT THROW AN ERROR
		**** 
	*/
	DECLARE @GROUP_COUNT INT
	BEGIN TRANSACTION
		BEGIN -- DELETE GROUP FROM ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
			IF @P_Debug = 1 PRINT 'Deleting rows from ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities'
			DELETE ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
			WHERE (Groups_Security_Entities_SeqID = 
						(SELECT 
							Groups_Security_Entities_SeqID 
						FROM 
							ZGWSecurity.Groups_Security_Entities 
						WHERE 
							Group_SeqID=@P_Group_SeqID 
							AND Security_Entity_SeqID = @P_Security_Entity_SeqID
						)
					)
		END 

		BEGIN -- DELETE GROUP FROM ZGWSecurity.Groups_Security_Entities
			IF @P_Debug = 1 PRINT 'Deleting rows from ZGWSecurity.Groups_Security_Entities'
			DELETE ZGWSecurity.Groups_Security_Entities
			WHERE (
				Group_SeqID = @P_Group_SeqID AND
				Security_Entity_SeqID = @P_Security_Entity_SeqID
				   )
		END
		
		BEGIN -- DELETE GROUP FROM ZGWSecurity.Groups_Security_Entities
			SET @GROUP_COUNT = (SELECT COUNT(*) FROM
						ZGWSecurity.Groups,
						ZGWSecurity.Groups_Security_Entities
						WHERE
						ZGWSecurity.Groups.Group_SeqID = ZGWSecurity.Groups_Security_Entities.Group_SeqID
						AND ZGWSecurity.Groups.Group_SeqID = @P_Group_SeqID)
			-- PRINT @GROUP_COUNT -- for debug
			IF @GROUP_COUNT = 0
				IF @P_Debug = 1 PRINT 'Role is not used by other entites'
				BEGIN
					DELETE ZGWSecurity.Groups
					WHERE (Group_SeqID = @P_Group_SeqID)
				END
			-- END IF
		END --  DELETE GROUP FROM ZGWSecurity.Groups
	IF @@ERROR <> 0
	 BEGIN
		-- Rollback the transaction
		ROLLBACK

		-- Raise an error and return
		RAISERROR ('Error in deleting group.', 16, 1)
		RETURN
	 END
	COMMIT
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Group'
END
RETURN 0