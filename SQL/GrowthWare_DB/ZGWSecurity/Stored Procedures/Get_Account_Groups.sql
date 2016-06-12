/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'Developer',
		@P_Security_Entity_SeqID INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Account_Groups
		@P_Account,
		@P_Security_Entity_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/09/2011
-- Description:	Selects all groups for a given Account and Entity
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Account_Groups]
	@P_Account VARCHAR(128),
	@P_Security_Entity_SeqID INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	SELECT
		ZGWSecurity.Groups.[Name] AS Groups
	FROM
		ZGWSecurity.Accounts WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities_Accounts WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Groups WITH(NOLOCK)
	WHERE
		ZGWSecurity.Accounts.Account = @P_Account
		AND ZGWSecurity.Accounts.Account_SeqID = ZGWSecurity.Groups_Security_Entities_Accounts.Account_SeqID
		AND ZGWSecurity.Groups_Security_Entities_Accounts.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID
		AND ZGWSecurity.Groups_Security_Entities.Group_SeqID = ZGWSecurity.Groups.Group_SeqID
		AND ZGWSecurity.Groups_Security_Entities.Security_Entity_SeqID = @P_Security_Entity_SeqID
	ORDER BY
		GROUPS

RETURN 0