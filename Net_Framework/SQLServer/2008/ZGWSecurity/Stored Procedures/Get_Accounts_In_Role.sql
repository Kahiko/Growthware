﻿/*
Usage:
	DECLARE 
		@P_Security_Entity_SeqID INT = 1,
		@P_Role_SeqID INT = 1,
		@P_Debug INT = 0

	exec ZGWSecurity.Get_Accounts_In_Role
		@P_Security_Entity_SeqID,
		@P_Role_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/09/2011
-- Description:	Selects all accounts in a role
--	given the Security_Entity_SeqID and Role_SeqID
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Accounts_In_Role]
	@P_Security_Entity_SeqID INT,
	@P_Role_SeqID INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Accounts_In_Role'
	SELECT
		Accounts.Account AS ACCT
		, Accounts.Email AS Email
	FROM
		ZGWSecurity.Accounts Accounts WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities_Accounts AcctSecurity WITH(NOLOCK),
		ZGWSecurity.Roles_Security_Entities [Security] WITH(NOLOCK),
		ZGWSecurity.Roles Roles WITH(NOLOCK)
	WHERE
		Accounts.Account_SeqID = AcctSecurity.Account_SeqID
		AND AcctSecurity.Roles_Security_Entities_SeqID = Security.Roles_Security_Entities_SeqID
		AND [Security].Role_SeqID = Roles.Role_SeqID
		AND Accounts.Status_SeqID <> 2
		AND Roles.Role_SeqID = @P_Role_SeqID
		AND [Security].Security_Entity_SeqID = @P_Security_Entity_SeqID
	ORDER BY
		Accounts.Account
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Accounts_In_Role'

RETURN 0