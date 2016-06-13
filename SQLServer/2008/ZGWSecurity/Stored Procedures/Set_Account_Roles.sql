/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'Developer',
		@P_Security_Entity_SeqID INT = 1,
		@P_Roles VARCHAR(max) = 'Developer',
		@P_Added_Updated_By INT = 2,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Account_Roles
		@P_Account,
		@P_Security_Entity_SeqID,
		@P_Roles,
		@P_Added_Updated_By,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/24/2011
-- Description:	Set's the roles associated
--	with an account.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Account_Roles]
	@P_Account VARCHAR(128),
	@P_Security_Entity_SeqID INT,
	@P_Roles VARCHAR(max),
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Account_Roles'
	DECLARE @V_ErrorCode INT
	DECLARE @V_ErrorMsg VARCHAR(MAX)

	BEGIN TRAN
		DECLARE @Account_SeqID INT
		SET @Account_SeqID = (SELECT Account_SeqID FROM ZGWSecurity.Accounts WHERE Account = @P_Account)
		-- Deleting old records before inseting any new ones.
		IF @P_Debug = 1 PRINT 'Calling ZGWSecurity.Delete_Account_Roles'
		EXEC ZGWSecurity.Delete_Account_Roles @Account_SeqID, @P_Security_Entity_SeqID, @V_ErrorCode, @P_Debug
		IF @V_ErrorCode <> 0
			BEGIN
				EXEC ZGWSystem.Log_Error_Info @P_Debug
				SET @V_ErrorMsg = 'Error executing ZGWSecurity.Delete_Account_Roles' + CHAR(10)
				RAISERROR(@V_ErrorMsg,16,1)
				RETURN @@ERROR
			END
		--END IF
		DECLARE @V_Role_SeqID AS 	INT
		DECLARE @V_SE_RLS_SECURITY_ID AS 	INT
		DECLARE @V_Role_Name AS	VARCHAR(50)
		DECLARE @V_Pos AS	INT
		SET @P_Roles = LTRIM(RTRIM(@P_Roles))+ ','
		SET @V_Pos = CHARINDEX(',', @P_Roles, 1)
		IF REPLACE(@P_Roles, ',', '') <> ''
			WHILE @V_Pos > 0
			BEGIN
				SET @V_Role_Name = LTRIM(RTRIM(LEFT(@P_Roles, @V_Pos - 1)))
				IF @V_Role_Name <> ''
				BEGIN
					--select the role seq id first
					SELECT @V_Role_SeqID = ZGWSecurity.Roles.Role_SeqID 
					FROM ZGWSecurity.Roles 
					WHERE [Name]=@V_ROLE_NAME

 					SELECT
						@V_SE_RLS_SECURITY_ID=Roles_Security_Entities_SeqID
					FROM
						ZGWSecurity.Roles_Security_Entities
					WHERE
						Role_SeqID = @V_Role_SeqID AND
						Security_Entity_SeqID = @P_Security_Entity_SeqID
						IF @P_Debug = 1 PRINT ('@V_SE_RLS_SECURITY_ID = ' + CONVERT(VARCHAR,@V_SE_RLS_SECURITY_ID))
					IF NOT EXISTS(
							SELECT 
								Roles_Security_Entities_SeqID 
							FROM 
								ZGWSecurity.Roles_Security_Entities_Accounts 
							WHERE 
							Account_SeqID = @Account_SeqID 
							AND Roles_Security_Entities_SeqID = @V_SE_RLS_SECURITY_ID
					)
					BEGIN TRY
						IF @P_Debug = 1 PRINT 'Inserting records'
						INSERT ZGWSecurity.Roles_Security_Entities_Accounts (
							Account_SeqID,
							Roles_Security_Entities_SeqID,
							Added_By
						)
						VALUES (
							@Account_SeqID,
							@V_SE_RLS_SECURITY_ID,
							@P_Added_Updated_By
						)
					END TRY
					BEGIN CATCH
						GOTO ABEND
					END CATCH
				END
					SET @P_Roles = RIGHT(@P_Roles, LEN(@P_Roles) - @V_Pos)
					SET @V_Pos = CHARINDEX(',', @P_Roles, 1)
			END
		IF @@ERROR <> 0 GOTO ABEND
	COMMIT TRAN
	RETURN 0
ABEND:
	IF @@ERROR <> 0
		BEGIN
			ROLLBACK TRAN
			EXEC ZGWSystem.Log_Error_Info @P_Debug
			SET @V_ErrorMsg = 'Error executing ZGWSecurity.Set_Account_Roles' + CHAR(10)
			IF @P_Debug = 1 PRINT @V_ErrorMsg
			--RAISERROR(@V_ErrorMsg,16,1)
			RETURN @@ERROR
		END
	--END IF