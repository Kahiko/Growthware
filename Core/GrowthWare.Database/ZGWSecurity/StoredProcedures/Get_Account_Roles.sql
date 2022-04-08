
/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'Developer',
		@P_Security_Entity_SeqID INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Account_Roles
		@P_Account,
		@P_Security_Entity_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/09/2011
-- Description:	Selects all groups for a given Account and Entity
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Account_Roles]
	@P_Account VARCHAR(128),
	@P_Security_Entity_SeqID INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	SELECT
		ZGWSecurity.Roles.[Name] AS Roles
	FROM
		ZGWSecurity.Accounts WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities_Accounts WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles WITH(NOLOCK)
	WHERE
		ZGWSecurity.Accounts.Account = @P_Account
		AND ZGWSecurity.Accounts.Account_SeqID = ZGWSecurity.Roles_Security_Entities_Accounts.Account_SeqID
		AND ZGWSecurity.Roles_Security_Entities_Accounts.Roles_Security_Entities_SeqID = ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID
		AND ZGWSecurity.Roles_Security_Entities.Role_SeqID = ZGWSecurity.Roles.Role_SeqID
		AND ZGWSecurity.Roles_Security_Entities.Security_Entity_SeqID = @P_Security_Entity_SeqID
	ORDER BY
		ROLES

RETURN 0

GO

