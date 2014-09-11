/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'Developer',
		@P_Security_Entity_SeqID INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Account_Security
		@P_Account,
		@P_Security_Entity_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/09/2011
-- Description:	Selects all derived roles given the account
--	and Entity.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Account_Security]
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
		AND ZGWSecurity.Roles_Security_Entities_Accounts.Account_SeqID = ZGWSecurity.Accounts.Account_SeqID
		AND ZGWSecurity.Roles_Security_Entities_Accounts.Roles_Security_Entities_SeqID = ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID
		AND ZGWSecurity.Roles_Security_Entities.Security_Entity_SeqID IN (SELECT Security_Entity_SeqID FROM ZGWSecurity.Get_Entity_Parents(1,@P_Security_Entity_SeqID))
		AND ZGWSecurity.Roles_Security_Entities.Role_SeqID = ZGWSecurity.Roles.Role_SeqID
	UNION
	SELECT
		ZGWSecurity.Roles.[Name] AS Roles
	FROM
		ZGWSecurity.Accounts WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities_Accounts WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities WITH(NOLOCK),
		ZGWSecurity.Roles WITH(NOLOCK)
	WHERE
		ZGWSecurity.Accounts.Account = @P_Account AND
		ZGWSecurity.Groups_Security_Entities_Accounts.Account_SeqID = ZGWSecurity.Accounts.Account_SeqID AND
		ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Accounts.Groups_Security_Entities_SeqID AND
		ZGWSecurity.Groups_Security_Entities.Security_Entity_SeqID IN (SELECT Security_Entity_SeqID FROM ZGWSecurity.Get_Entity_Parents(1,@P_Security_Entity_SeqID)) AND
		ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID AND
		ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Roles_Security_Entities_SeqID AND
		ZGWSecurity.Roles.Role_SeqID = ZGWSecurity.Roles_Security_Entities.Role_SeqID
	ORDER BY
		Roles

RETURN 0