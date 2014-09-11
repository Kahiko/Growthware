/*
Usage:
	DECLARE 
		@P_Security_Entity_SeqID AS INT,
		@P_Group_SeqID AS INT,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Group
		@P_Security_Entity_SeqID,
		@P_Group_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description:	Retrieves one or more groups given the 
--	Security_Entity_SeqID and Group_SeqID.
-- Note:
--	If Group_SeqID is -1 all groups will be returned.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Group]
	@P_Security_Entity_SeqID AS INT,
	@P_Group_SeqID AS INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Group'
	
	IF @P_Group_SeqID > -1
		BEGIN
			IF @P_Debug = 1 PRINT 'SELECT an existing row from the table.'
			SELECT
				ZGWSecurity.Groups.Group_SeqID as GROUP_SEQ_ID
				, ZGWSecurity.Groups.Name
				, ZGWSecurity.Groups.[Description]
				, ZGWSecurity.Groups.Added_By
				, ZGWSecurity.Groups.Added_Date
				, ZGWSecurity.Groups.Updated_By
				, ZGWSecurity.Groups.Updated_Date
			FROM
				ZGWSecurity.Groups WITH(NOLOCK)
			WHERE
				Group_SeqID = @P_Group_SeqID

		END
	ELSE --
		BEGIN
			IF @P_Debug = 1 PRINT 'Getting all groups for a given Security Entity'
			SELECT
				ZGWSecurity.Groups.Group_SeqID as GROUP_SEQ_ID
				, ZGWSecurity.Groups.Name
				, ZGWSecurity.Groups.[Description]
				, ZGWSecurity.Groups.Added_By
				, ZGWSecurity.Groups.Added_Date
				, ZGWSecurity.Groups.Updated_By
				, ZGWSecurity.Groups.Updated_Date
			FROM
				ZGWSecurity.Groups WITH(NOLOCK),
				ZGWSecurity.Groups_Security_Entities WITH(NOLOCK)
			WHERE
				ZGWSecurity.Groups.Group_SeqID = ZGWSecurity.Groups_Security_Entities.Group_SeqID
				AND ZGWSecurity.Groups_Security_Entities.Security_Entity_SeqID = @P_Security_Entity_SeqID
			ORDER BY
				ZGWSecurity.Groups.Name
		END
	-- END IF		
	
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Group'
RETURN 0