
/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'developer',
		@P_Is_Se_Admin INT = 1,
		@PSecurityEntitySeqId AS INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Valid_Security_Entity
		@P_Account,
		@P_Is_Se_Admin,
		@PSecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/23/2011
-- Description:	Retrieves valid security entity details
--	for a given account.
-- Note:
--	SeqID value of -1 will return all
--	security enties.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Valid_Security_Entity]
	@P_Account VARCHAR(128),
	@P_Is_Se_Admin INT,
	@PSecurityEntitySeqId AS INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Valid_Security_Entity'
	DECLARE @V_Active_Status VARCHAR(50)
	DECLARE @T_Valic_Se TABLE (SecurityEntitySeqId INT)
	DECLARE @V_Is_Sys_Admin INT
	SET @V_Active_Status = (SELECT [StatusSeqId] FROM ZGWSystem.Statuses WHERE UPPER([Name]) = 'ACTIVE')
	SET @V_Is_Sys_Admin = (SELECT Is_System_Admin FROM ZGWSecurity.Accounts WHERE UPPER(Account) = UPPER(@P_Account))
	IF @V_Is_Sys_Admin = 0
		BEGIN
			INSERT INTO @T_Valic_Se
				SELECT -- Security Entitys via roles
					ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId
				FROM
					ZGWSecurity.Accounts,
					ZGWSecurity.Roles_Security_Entities_Accounts,
					ZGWSecurity.Roles_Security_Entities,
					ZGWSecurity.Roles
				WHERE
					ZGWSecurity.Accounts.Account = @P_Account
					AND ZGWSecurity.Roles_Security_Entities_Accounts.AccountSeqId = ZGWSecurity.Accounts.AccountSeqId
					AND ZGWSecurity.Roles_Security_Entities_Accounts.RolesSecurityEntitiesSeqId = ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId
					AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
					AND ZGWSecurity.Roles.Is_System_Only = 0
				UNION
				SELECT -- Security Entitys via groups
					ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId
				FROM
					ZGWSecurity.Accounts,
					ZGWSecurity.Groups_Security_Entities_Accounts,
					ZGWSecurity.Groups_Security_Entities,
					ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities,
					ZGWSecurity.Roles_Security_Entities,
					ZGWSecurity.Roles
				WHERE
					ZGWSecurity.Accounts.Account = @P_Account
					AND ZGWSecurity.Groups_Security_Entities_Accounts.AccountSeqId = ZGWSecurity.Accounts.AccountSeqId
					AND ZGWSecurity.Groups_Security_Entities.GroupsSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.GroupsSecurityEntitiesSeqId
					AND ZGWSecurity.Roles_Security_Entities.RolesSecurityEntitiesSeqId = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.RolesSecurityEntitiesSeqId
					AND ZGWSecurity.Roles_Security_Entities.RoleSeqId = ZGWSecurity.Roles.RoleSeqId
				IF @P_Is_Se_Admin = 0 -- FALSE
					BEGIN
						SELECT
							SecurityEntitySeqId AS SecurityEntityID,
							[Name],
							[Description]
						FROM
							ZGWSecurity.Security_Entities
						WHERE
							ZGWSecurity.Security_Entities.SecurityEntitySeqId IN (SELECT * FROM @T_Valic_Se)
							AND ZGWSecurity.Security_Entities.StatusSeqId = @V_Active_Status
						ORDER BY
							[Name]
					END
				ELSE
					BEGIN
						SELECT
							SecurityEntitySeqId AS SecurityEntityID,
							[Name],
							[Description]
						FROM
							ZGWSecurity.Security_Entities
						WHERE
							ZGWSecurity.Security_Entities.SecurityEntitySeqId IN (SELECT * FROM @T_Valic_Se)
							OR ZGWSecurity.Security_Entities.ParentSecurityEntitySeqId = @PSecurityEntitySeqId
						ORDER BY
							[Name]
					END
				-- END IF
		END
	ELSE
		BEGIN
			SELECT
				SecurityEntitySeqId AS SecurityEntityID,
				[Name],
				[Description]
			FROM
				ZGWSecurity.Security_Entities
			ORDER BY
				[Name]
		END
	--END IF

RETURN 0

GO

