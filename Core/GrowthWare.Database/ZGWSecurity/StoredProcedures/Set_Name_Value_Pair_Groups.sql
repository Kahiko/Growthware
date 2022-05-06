
/*
Usage:
	DECLARE 
		@P_NVPSeqId int = 1,
		@PSecurityEntitySeqId INT = 1,
		@P_Groups VARCHAR(MAX) = 'EveryOne',
		@P_PermissionsNVPDetailSeqId INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Debug int = 1

	exec ZGWSecurity.Set_Name_Value_Pair_Groups
		@P_NVPSeqId,
		@PSecurityEntitySeqId,
		@P_Groups,
		@P_PermissionsNVPDetailSeqId,
		@P_Added_Updated_By,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/26/2011
-- Description:	Delete and inserts into ZGWSecurity.Groups_Security_Entities_Functions
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Name_Value_Pair_Groups]
	@P_NVPSeqId INT,
	@PSecurityEntitySeqId INT,
	@P_Groups VARCHAR(1000),
	@P_PermissionsNVPDetailSeqId INT,
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS

IF @P_Debug = 1 PRINT('Starting ZGWSecurity.Set_Name_Value_Pair_Groups')
BEGIN TRAN
	DECLARE @V_GroupSeqId INT
			,@V_GroupsSecurityEntitiesSeqId INT
			,@V_GROUP_NAME VARCHAR(50)
			,@V_Pos INT
			,@V_ErrorMsg VARCHAR(MAX)
	
	IF @P_Debug = 1 PRINT 'Deleting existing Groups associated with the name value pair before inseting new ones.'
	EXEC ZGWSystem.Delete_Groups_Security_Entities_Permissions @P_NVPSeqId,@PSecurityEntitySeqId,@P_PermissionsNVPDetailSeqId, @P_Debug
	IF @@ERROR <> 0
		BEGIN
			EXEC ZGWSystem.Log_Error_Info @P_Debug
			SET @V_ErrorMsg = 'Error executing ZGWSecurity.Delete_Groups_Security_Entities_Permissions' + CHAR(10)
			RAISERROR(@V_ErrorMsg,16,1)
			RETURN @@ERROR
		END
	--END IF	
	SET @P_Groups = LTRIM(RTRIM(@P_Groups))+ ','
	SET @V_Pos = CHARINDEX(',', @P_Groups, 1)
	IF REPLACE(@P_Groups, ',', '') <> ''
		WHILE @V_Pos > 0
		BEGIN
			SET @V_GROUP_NAME = LTRIM(RTRIM(LEFT(@P_Groups, @V_Pos - 1)))
			IF @V_GROUP_NAME <> ''
			BEGIN
				IF @P_Debug = 1 PRINT 'select the GROUP seq id first'
				SELECT @V_GroupSeqId = ZGWSecurity.Groups.GroupSeqId 
				FROM ZGWSecurity.Groups 
				WHERE [Name]=@V_GROUP_NAME

 				SELECT
					@V_GroupsSecurityEntitiesSeqId=GroupsSecurityEntitiesSeqId
				FROM
					ZGWSecurity.Groups_Security_Entities
				WHERE
					GroupSeqId = @V_GroupSeqId AND
					SecurityEntitySeqId = @PSecurityEntitySeqId
					IF @P_Debug = 1 PRINT('@V_GroupsSecurityEntitiesSeqId = ' + CONVERT(VARCHAR,@V_GroupsSecurityEntitiesSeqId))
				IF NOT EXISTS(
						SELECT 
							GroupsSecurityEntitiesSeqId 
						FROM 
							ZGWSecurity.Groups_Security_Entities_Permissions 
						WHERE 
						NVPSeqId = @P_NVPSeqId 
						AND PermissionsNVPDetailSeqId = @P_PermissionsNVPDetailSeqId
						AND GroupsSecurityEntitiesSeqId = @V_GroupsSecurityEntitiesSeqId
				)
				BEGIN TRY
					IF @P_Debug = 1 PRINT('Inserting record')
					INSERT ZGWSecurity.Groups_Security_Entities_Permissions (
						NVPSeqId,
						GroupsSecurityEntitiesSeqId,
						PermissionsNVPDetailSeqId,
						Added_By
					)
					VALUES (
						@P_NVPSeqId,
						@V_GroupsSecurityEntitiesSeqId,
						@P_PermissionsNVPDetailSeqId,
						@P_Added_Updated_By
					)
				END TRY
				BEGIN CATCH
					GOTO ABEND
				END CATCH
			END
				SET @P_Groups = RIGHT(@P_Groups, LEN(@P_Groups) - @V_Pos)
				SET @V_Pos = CHARINDEX(',', @P_Groups, 1)
		END
	--END IF
IF @@ERROR = 0
	BEGIN
		COMMIT TRAN
		IF @P_Debug = 1 PRINT('Ending ZGWSecurity.Set_Name_Value_Pair_Groups')
		RETURN 0
	END
ABEND:
BEGIN
	ROLLBACK TRAN
	EXEC ZGWSystem.Log_Error_Info @P_Debug
	SET @V_ErrorMsg = 'Error executing ZGWSecurity.Set_Name_Value_Pair_Groups' + CHAR(10)
	SET @V_ErrorMsg = @V_ErrorMsg + ERROR_MESSAGE()
	RAISERROR(@V_ErrorMsg,16,1)
	RETURN @@ERROR
END

GO

