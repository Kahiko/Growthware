
/*
Usage:
	DECLARE 
		@P_FunctionSeqId int = 1,
		@PSecurityEntitySeqId INT = 1,
		@P_Roles VARCHAR(MAX) = 'EveryOne',
		@P_Permissions_NVP_Detail_SeqID INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Function_Roles
		@P_FunctionSeqId,
		@PSecurityEntitySeqId,
		@P_Roles,
		@P_Permissions_NVP_Detail_SeqID,
		@P_Added_Updated_By,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/01/2011
-- Description:	Delete and inserts into ZGWSecurity.Roles_Security_Entities_Functions
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Function_Roles]
	@P_FunctionSeqId int,
	@PSecurityEntitySeqId INT,
	@P_Roles VARCHAR(MAX),
	@P_Permissions_NVP_Detail_SeqID INT,
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS
BEGIN TRANSACTION
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Function_Roles'
	-- NEED TO DELETE EXISTING ROLE ASSOCITAED WITH THE FUNCTION BEFORE 
	-- INSERTING NEW ONES.
	
	DECLARE @V_ErrorCodde INT
			,@V_RoleSeqId AS INT
			,@V_Roles_Security_Entities_SeqID AS INT
			,@V_Role_Name AS VARCHAR(50)
			,@V_Pos AS INT
			,@V_ErrorMsg VARCHAR(MAX)
	EXEC ZGWSecurity.Delete_Function_Roles @P_FunctionSeqId,@PSecurityEntitySeqId,@P_Permissions_NVP_Detail_SeqID,@P_Added_Updated_By,@V_ErrorCodde
	IF @@ERROR <> 0
		BEGIN
			GOTO ABEND
		END
	--END IF	

	SET @P_Roles = LTRIM(RTRIM(@P_Roles))+ ','
	SET @V_Pos = CHARINDEX(',', @P_Roles, 1)
	IF LEN(REPLACE(@P_Roles, ',', '')) > 0
		WHILE @V_Pos > 0
			BEGIN -- go through all the roles and add if necessary
				SET @V_Role_Name = LTRIM(RTRIM(LEFT(@P_Roles, @V_Pos - 1)))
				IF @V_Role_Name <> ''
				BEGIN
					--select the role seq id first
					SELECT 
						@V_RoleSeqId = ZGWSecurity.Roles.RoleSeqId 
					FROM 
						ZGWSecurity.Roles 
					WHERE 
						[Name]=@V_Role_Name
						
					--select the Roles_Security_Entities_SeqID
					SELECT
						@V_Roles_Security_Entities_SeqID=Roles_Security_Entities_SeqID
					FROM
						ZGWSecurity.Roles_Security_Entities
					WHERE
						RoleSeqId = @V_RoleSeqId AND
						SecurityEntitySeqId = @PSecurityEntitySeqId
						
					IF @P_Debug = 1 PRINT('@V_Roles_Security_Entities_SeqID = ' + CONVERT(VARCHAR,@V_Roles_Security_Entities_SeqID))
					IF NOT EXISTS(
							SELECT 
								Roles_Security_Entities_SeqID 
							FROM 
								ZGWSecurity.Roles_Security_Entities_Functions 
							WHERE 
							FunctionSeqId = @P_FunctionSeqId 
							AND Permissions_NVP_Detail_SeqID = @P_Permissions_NVP_Detail_SeqID
							AND Roles_Security_Entities_SeqID = @V_Roles_Security_Entities_SeqID)
						BEGIN TRY
							IF @P_Debug = 1 PRINT 'Insert new record'
							INSERT ZGWSecurity.Roles_Security_Entities_Functions (
								FunctionSeqId,
								Roles_Security_Entities_SeqID,
								Permissions_NVP_Detail_SeqID,
								Added_By
							)
							VALUES (
								@P_FunctionSeqId,
								@V_Roles_Security_Entities_SeqID,
								@P_Permissions_NVP_Detail_SeqID,
								@P_Added_Updated_By
							)
						END TRY
						BEGIN CATCH
							GOTO ABEND
						END CATCH
					--END IF
				END
				SET @P_Roles = RIGHT(@P_Roles, LEN(@P_Roles) - @V_Pos)
				SET @V_Pos = CHARINDEX(',', @P_Roles, 1)
			END
		--END WHILE
	IF @@error <> 0 GOTO ABEND
Commit Transaction
RETURN 0
ABEND:
	ROLLBACK TRAN
	EXEC ZGWSystem.Log_Error_Info @P_Debug
	SET @V_ErrorMsg = 'Error executing ZGWSecurity.Set_Function_Roles' + CHAR(10)
	SET @V_ErrorMsg = @V_ErrorMsg + ERROR_MESSAGE()
	RAISERROR(@V_ErrorMsg,16,1)
	RETURN @@ERROR

GO

