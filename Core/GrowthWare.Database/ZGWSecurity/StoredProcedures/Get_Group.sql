
/*
Usage:
	DECLARE 
		@P_SecurityEntitySeqId AS INT,
		@P_GroupSeqId AS INT,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Group
		@P_SecurityEntitySeqId,
		@P_GroupSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description:	Retrieves one or more groups given the 
--	SecurityEntitySeqId and GroupSeqId.
-- Note:
--	If GroupSeqId is -1 all groups will be returned.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Group]
	@P_SecurityEntitySeqId AS INT,
	@P_GroupSeqId AS INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Group'
	
	IF @P_GroupSeqId > -1
		BEGIN
			IF @P_Debug = 1 PRINT 'SELECT an existing row from the table.'
			SELECT
				ZGWSecurity.Groups.GroupSeqId as GROUP_SEQ_ID
				, ZGWSecurity.Groups.Name
				, ZGWSecurity.Groups.[Description]
				, ZGWSecurity.Groups.Added_By
				, ZGWSecurity.Groups.Added_Date
				, ZGWSecurity.Groups.Updated_By
				, ZGWSecurity.Groups.Updated_Date
			FROM
				ZGWSecurity.Groups WITH(NOLOCK)
			WHERE
				GroupSeqId = @P_GroupSeqId

		END
	ELSE --
		BEGIN
			IF @P_Debug = 1 PRINT 'Getting all groups for a given Security Entity'
			SELECT
				ZGWSecurity.Groups.GroupSeqId as GROUP_SEQ_ID
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
				ZGWSecurity.Groups.GroupSeqId = ZGWSecurity.Groups_Security_Entities.GroupSeqId
				AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId = @P_SecurityEntitySeqId
			ORDER BY
				ZGWSecurity.Groups.Name
		END
	-- END IF		
	
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Group'
RETURN 0

GO

