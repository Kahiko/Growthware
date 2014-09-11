/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'developer',
		@P_IS_SE_ADMIN INT = 1,
		@P_Security_Entity_SeqID AS INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Valid_Security_Entity
		@P_Account,
		@P_IS_SE_ADMIN,
		@P_Security_Entity_SeqID,
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
	@P_IS_SE_ADMIN INT,
	@P_Security_Entity_SeqID AS INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Valid_Security_Entity'
	DECLARE @V_ACTIVE_STATUS VARCHAR(50)
	DECLARE @T_VALID_SE TABLE (Security_Entity_SeqID INT)
	DECLARE @V_IS_SYS_ADMIN INT
	SET @V_ACTIVE_STATUS = (SELECT [Status_SeqID] FROM ZGWSystem.Statuses WHERE UPPER([Name]) = 'ACTIVE')
	SET @V_IS_SYS_ADMIN = (SELECT IS_SYSTEM_ADMIN FROM ZGWSecurity.Accounts WHERE UPPER(Account) = UPPER(@P_Account))
	IF @V_IS_SYS_ADMIN = 0
		BEGIN
			INSERT INTO @T_VALID_SE
				SELECT -- Security Entitys via roles
					ZGWSecurity.Roles_Security_Entities.Security_Entity_SeqID
				FROM
					ZGWSecurity.Accounts,
					ZGWSecurity.Roles_Security_Entities_Accounts,
					ZGWSecurity.Roles_Security_Entities,
					ZGWSecurity.Roles
				WHERE
					ZGWSecurity.Accounts.Account = @P_Account
					AND ZGWSecurity.Roles_Security_Entities_Accounts.Account_SeqID = ZGWSecurity.Accounts.Account_SeqID
					AND ZGWSecurity.Roles_Security_Entities_Accounts.Roles_Security_Entities_SeqID = ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID
					AND ZGWSecurity.Roles_Security_Entities.Role_SeqID = ZGWSecurity.Roles.Role_SeqID
					AND ZGWSecurity.Roles.IS_SYSTEM_ONLY = 0
				UNION
				SELECT -- Security Entitys via groups
					ZGWSecurity.Roles_Security_Entities.Security_Entity_SeqID
				FROM
					ZGWSecurity.Accounts,
					ZGWSecurity.Groups_Security_Entities_Accounts,
					ZGWSecurity.Groups_Security_Entities,
					ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities,
					ZGWSecurity.Roles_Security_Entities,
					ZGWSecurity.Roles
				WHERE
					ZGWSecurity.Accounts.Account = @P_Account
					AND ZGWSecurity.Groups_Security_Entities_Accounts.Account_SeqID = ZGWSecurity.Accounts.Account_SeqID
					AND ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Groups_Security_Entities_SeqID
					AND ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities.Roles_Security_Entities_SeqID
					AND ZGWSecurity.Roles_Security_Entities.Role_SeqID = ZGWSecurity.Roles.Role_SeqID
				IF @P_IS_SE_ADMIN = 0 -- FALSE
					BEGIN
						SELECT
							Security_Entity_SeqID AS SE_SEQ_ID,
							[Name],
							[Description]
						FROM
							ZGWSecurity.Security_Entities
						WHERE
							ZGWSecurity.Security_Entities.Security_Entity_SeqID IN (SELECT * FROM @T_VALID_SE)
							AND ZGWSecurity.Security_Entities.Status_SeqID = @V_ACTIVE_STATUS
						ORDER BY
							[Name]
					END
				ELSE
					BEGIN
						SELECT
							Security_Entity_SeqID AS SE_SEQ_ID,
							[Name],
							[Description]
						FROM
							ZGWSecurity.Security_Entities
						WHERE
							ZGWSecurity.Security_Entities.Security_Entity_SeqID IN (SELECT * FROM @T_VALID_SE)
							OR ZGWSecurity.Security_Entities.PARENT_Security_Entity_SeqID = @P_Security_Entity_SeqID
						ORDER BY
							[Name]
					END
				-- END IF
		END
	ELSE
		BEGIN
			SELECT
				Security_Entity_SeqID AS SE_SEQ_ID,
				[Name],
				[Description]
			FROM
				ZGWSecurity.Security_Entities
			ORDER BY
				[Name]
		END
	--END IF

RETURN 0