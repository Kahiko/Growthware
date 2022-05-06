
/*
Usage:
	DECLARE 
		@P_RoleSeqId INT = 1,
		@PSecurityEntitySeqId INT = 1,
		@P_Account VARCHAR(128) = 'Developer',
		@P_Added_Updated_By INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Role_Accounts
		@P_RoleSeqId,
		@PSecurityEntitySeqId,
		@P_Account,
		@P_Added_Updated_By,
		@P_Debug

*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/08/2011
-- Description:	Inserts into ZGWSecurity.Roles_Security_Entities_Accounts
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Role_Accounts]
	@P_RoleSeqId INT,
	@PSecurityEntitySeqId INT,
	@P_Account VARCHAR(128),
	@P_Added_Updated_By INT,
	@P_Debug INT = 1
AS
IF @P_Debug = 1	PRINT 'Starting ZGWSecurity.Set_Role_Accounts'
DECLARE @V_AccountSeqId AS INT
		,@V_RolesSecurityEntitiesSeqId AS INT
		,@V_ErrorMsg VARCHAR(MAX)
BEGIN TRAN
	SET NOCOUNT OFF;
	SET @V_AccountSeqId = (SELECT ZGWSecurity.Accounts.AccountSeqId FROM ZGWSecurity.Accounts WHERE Account = @P_Account)
	SET @V_RolesSecurityEntitiesSeqId = (
			SELECT
				RolesSecurityEntitiesSeqId
			FROM
				ZGWSecurity.Roles_Security_Entities
			WHERE
				RoleSeqId = @P_RoleSeqId
				AND SecurityEntitySeqId = @PSecurityEntitySeqId
		)
	BEGIN TRY
		INSERT INTO
			ZGWSecurity.Roles_Security_Entities_Accounts(RolesSecurityEntitiesSeqId,AccountSeqId,Added_By)
		VALUES(
			@V_RolesSecurityEntitiesSeqId,
			@V_AccountSeqId,
			@P_Added_Updated_By
		)
	END TRY
	BEGIN CATCH
		GOTO ABEND
	END CATCH
COMMIT TRAN
IF @P_Debug = 1	PRINT 'Ending ZGWSecurity.Set_Role_Accounts'
RETURN 0
ABEND:
	ROLLBACK TRAN
	EXEC ZGWSystem.Log_Error_Info @P_Debug
	SET @V_ErrorMsg = 'Error executing ZGWSecurity.Set_Role' + CHAR(10)
	SET @V_ErrorMsg = @V_ErrorMsg + ERROR_MESSAGE()
	RAISERROR(@V_ErrorMsg,16,1)
	RETURN @@ERROR	
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Roles'

GO

