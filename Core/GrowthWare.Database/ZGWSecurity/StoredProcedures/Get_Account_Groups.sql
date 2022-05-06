
/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'Developer',
		@PSecurityEntitySeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Account_Groups
		@P_Account,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/09/2011
-- Description:	Selects all groups for a given Account and Entity
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Account_Groups]
	@P_Account VARCHAR(128),
	@PSecurityEntitySeqId INT,
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
		AND ZGWSecurity.Accounts.AccountSeqId = ZGWSecurity.Groups_Security_Entities_Accounts.AccountSeqId
		AND ZGWSecurity.Groups_Security_Entities_Accounts.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId
		AND ZGWSecurity.Groups_Security_Entities.GroupSeqId = ZGWSecurity.Groups.GroupSeqId
		AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId = @PSecurityEntitySeqId
	ORDER BY
		GROUPS

RETURN 0

GO

