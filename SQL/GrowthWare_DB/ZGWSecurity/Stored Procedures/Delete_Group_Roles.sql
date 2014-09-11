/*
Usage:
	DECLARE 
		@P_Group_SeqID int = 4,
		@P_Security_Entity_SeqID	INT = 1,
		@P_Debug INT = 0

	exec ZGWSecurity.Delete_Function_Groups
		@P_Group_SeqID,
		@P_Security_Entity_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/29/2011
-- Description:	Deletes a record from ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
--	given the Group_SeqID and Security_Entity_SeqID
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Group_Roles]
	@P_Group_SeqID INT,
	@P_Security_Entity_SeqID INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Begin [ZGWSecurity].[Delete_Group_Roles]'
	DELETE
		ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
	WHERE
		Groups_Security_Entities_SeqID IN (SELECT Groups_Security_Entities_SeqID 
					FROM ZGWSecurity.Groups_Security_Entities 
					WHERE Security_Entity_SeqID=@P_Security_Entity_SeqID
					AND Group_SeqID = @P_Group_SeqID)
	IF @P_Debug = 1 PRINT 'End [ZGWSecurity].[Delete_Group_Roles]'
RETURN 0