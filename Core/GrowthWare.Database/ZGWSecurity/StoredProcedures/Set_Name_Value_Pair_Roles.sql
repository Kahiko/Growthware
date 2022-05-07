
/*
Usage:
	DECLARE 
		@P_NVPSeqId int = 1,
		@P_SecurityEntitySeqId INT = 1,
		@P_Role VARCHAR(MAX) = 'EveryOne',
		@P_PermissionsNVPDetailSeqId INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Debug int = 1

	exec ZGWSecurity.Set_Name_Value_Pair_Roles
		@P_NVPSeqId,
		@P_SecurityEntitySeqId,
		@P_Role,
		@P_PermissionsNVPDetailSeqId,
		@P_Added_Updated_By,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/08/2011
-- Description:	Delete and inserts into ZGWSecurity.Roles_Security_Entities_Functions
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Name_Value_Pair_Roles]
	@P_NVPSeqId INT,
	@P_SecurityEntitySeqId INT,
	@P_Role VARCHAR(1000),
	@P_PermissionsNVPDetailSeqId INT,
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS

IF @P_Debug = 1 PRINT('Starting ZGWSecurity.Set_Name_Value_Pair_Role')
BEGIN TRAN
	DECLARE @V_RoleSeqId INT
			,@V_RolesSecurityEntitiesSeqId INT
			,@V_Group_Name VARCHAR(50)
			,@V_Pos INT
			,@V_ErrorMsg VARCHAR(MAX)
			,@V_Now DATETIME = GETDATE()
	
	IF @P_Debug = 1 PRINT 'Deleting existing Role associated with the name value pair before inseting new ones.'
	EXEC ZGWSystem.Delete_Roles_Security_Entities_Permissions @P_NVPSeqId,@P_SecurityEntitySeqId,@P_PermissionsNVPDetailSeqId, @P_Debug
	IF @@ERROR <> 0
		BEGIN
			GOTO ABEND
		END
	--END IF	
	SET @P_Role = LTRIM(RTRIM(@P_Role))+ ','
	SET @V_Pos = CHARINDEX(',', @P_Role, 1)
	IF REPLACE(@P_Role, ',', '') <> ''
		WHILE @V_Pos > 0
		BEGIN
			SET @V_Group_Name = LTRIM(RTRIM(LEFT(@P_Role, @V_Pos - 1)))
			IF @V_Group_Name <> ''
			BEGIN
				IF @P_Debug = 1 PRINT 'select the RoleSeqId first'
				SELECT @V_RoleSeqId = ZGWSecurity.Roles.RoleSeqId 
				FROM ZGWSecurity.Roles 
				WHERE [Name]=@V_Group_Name

 				SELECT
					@V_RolesSecurityEntitiesSeqId=RolesSecurityEntitiesSeqId
				FROM
					ZGWSecurity.Roles_Security_Entities
				WHERE
					RoleSeqId = @V_RoleSeqId AND
					SecurityEntitySeqId = @P_SecurityEntitySeqId
					IF @P_Debug = 1 PRINT('@V_RolesSecurityEntitiesSeqId = ' + CONVERT(VARCHAR,@V_RolesSecurityEntitiesSeqId))
				IF NOT EXISTS(
						SELECT 
							RolesSecurityEntitiesSeqId 
						FROM 
							ZGWSecurity.Roles_Security_Entities_Permissions 
						WHERE 
						NVPSeqId = @P_NVPSeqId 
						AND PermissionsNVPDetailSeqId = @P_PermissionsNVPDetailSeqId
						AND RolesSecurityEntitiesSeqId = @V_RolesSecurityEntitiesSeqId
				)
				BEGIN TRY
					IF @P_Debug = 1 PRINT('Inserting record')
					INSERT ZGWSecurity.Roles_Security_Entities_Permissions (
						NVPSeqId,
						RolesSecurityEntitiesSeqId,
						PermissionsNVPDetailSeqId,
						Added_By,
						Added_Date
					)
					VALUES (
						@P_NVPSeqId,
						@V_RolesSecurityEntitiesSeqId,
						@P_PermissionsNVPDetailSeqId,
						@P_Added_Updated_By,
						@V_Now
					)
				END TRY
				BEGIN CATCH
					GOTO ABEND
				END CATCH
			END
				SET @P_Role = RIGHT(@P_Role, LEN(@P_Role) - @V_Pos)
				SET @V_Pos = CHARINDEX(',', @P_Role, 1)
		END
	--END IF
IF @@ERROR = 0
	BEGIN
		COMMIT TRAN
		IF @P_Debug = 1 PRINT('Ending ZGWSecurity.Set_Name_Value_Pair_Role')
		RETURN 0
	END
ABEND:
BEGIN
	ROLLBACK TRAN
	EXEC ZGWSystem.Log_Error_Info @P_Debug
	SET @V_ErrorMsg = 'Error executing ZGWSecurity.Set_Name_Value_Pair_Role' + CHAR(10)
	SET @V_ErrorMsg = @V_ErrorMsg + ERROR_MESSAGE()
	RAISERROR(@V_ErrorMsg,16,1)
	RETURN @@ERROR
END

GO

