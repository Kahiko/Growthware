
/*
Usage:
	DECLARE 
		@P_Security_Entity_SeqID AS INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Security_Entity
		@P_Security_Entity_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/23/2011
-- Description:	Retrieves security entity details
--	given the Security_Entity_SeqID
-- Note:
--	SeqID value of -1 will return all
--	security enties.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Security_Entity]
	@P_Security_Entity_SeqID AS INT = 1,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Security_Entity'
	IF @P_Security_Entity_SeqID = -1
		BEGIN
			IF @P_Debug = 1 PRINT 'Getting all Security_Enties'
			SELECT
				Security_Entity_SeqID as SecurityEntityID
				, Name
				, [Description]
				, URL
				, Status_SeqID as STATUS_SEQ_ID
				, DAL
				, DAL_Name
				, DAL_Name_Space
				, DAL_String
				, Skin
				, Style
				, Encryption_Type
				, Parent_Security_Entity_SeqID as PARENT_SecurityEntityID
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
			FROM
				ZGWSecurity.Security_Entities
			ORDER BY 
				NAME ASC
		END
	ELSE
		BEGIN
			IF @P_Debug = 1 PRINT 'Getting 1 row from Security_Enties'
			SELECT
				Security_Entity_SeqID as SecurityEntityID
				, Name
				, [Description]
				, URL
				, Status_SeqID as STATUS_SEQ_ID
				, DAL
				, DAL_Name
				, DAL_Name_Space
				, DAL_String
				, Skin
				, Style
				, Encryption_Type
				, Parent_Security_Entity_SeqID as PARENT_SecurityEntityID
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
			FROM 
				ZGWSecurity.Security_Entities
			WHERE
				Security_Entity_SeqID = @P_Security_Entity_SeqID
		END
	--End IF
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Security_Entity'
RETURN 0

GO

