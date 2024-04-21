/*
Usage:
	DECLARE
		@P_NVP_SeqID int = 1,
		@P_Security_Entity_SeqID int = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Name_Value_Pair_Groups
		@P_NVP_SeqID,
		@P_Security_Entity_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/22/2011
-- Description:	Returns groups associated with
--	Name Value Pairs 
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Name_Value_Pair_Groups]
		@P_NVP_SeqID int = 1,
		@P_Security_Entity_SeqID int = 1,
		@P_Debug INT = 1
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Start Get_Name_Value_Pair_Groups'
	SELECT
		ZGWSecurity.Groups.[Name] AS GROUPS
	FROM
		ZGWSecurity.Groups_Security_Entities_Permissions,
		ZGWSecurity.Groups_Security_Entities,
		ZGWSecurity.Groups
	WHERE
		ZGWSecurity.Groups_Security_Entities_Permissions.NVP_SeqID = @P_NVP_SeqID
		AND ZGWSecurity.Groups_Security_Entities_Permissions.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID
		AND ZGWSecurity.Groups_Security_Entities.Group_SeqID = ZGWSecurity.Groups.Group_SeqID
		AND ZGWSecurity.Groups_Security_Entities.Security_Entity_SeqID = @P_Security_Entity_SeqID
	ORDER BY
		GROUPS
	IF @P_Debug = 1 PRINT 'End Get_Name_Value_Pair_Groups'
RETURN 0