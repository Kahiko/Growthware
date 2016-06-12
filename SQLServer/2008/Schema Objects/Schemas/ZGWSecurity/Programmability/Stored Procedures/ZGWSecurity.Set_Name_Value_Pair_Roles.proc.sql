/*
Usage:
	DECLARE 
		@P_NVP_SeqID int = 1,
		@P_Security_Entity_SeqID INT = 1,
		@P_Role VARCHAR(MAX) = 'EveryOne',
		@P_Permissions_NVP_Detail_SeqID INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Debug int = 1

	exec ZGWSecurity.Set_Name_Value_Pair_Roles
		@P_NVP_SeqID,
		@P_Security_Entity_SeqID,
		@P_Role,
		@P_Permissions_NVP_Detail_SeqID,
		@P_Added_Updated_By,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/08/2011
-- Description:	Delete and inserts into ZGWSecurity.Roles_Security_Entities_Functions
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Name_Value_Pair_Roles]
	@P_NVP_SeqID INT,
	@P_Security_Entity_SeqID INT,
	@P_Role VARCHAR(1000),
	@P_Permissions_NVP_Detail_SeqID INT,
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS

IF @P_Debug = 1 PRINT('Starting ZGWSecurity.Set_Name_Value_Pair_Role')
BEGIN TRAN
	DECLARE @V_Role_SeqID INT
			,@V_Roles_Security_Entities_SeqID INT
			,@V_GROUP_NAME VARCHAR(50)
			,@V_Pos INT
			,@V_ErrorMsg VARCHAR(MAX)
			,@V_Now DATETIME = GETDATE()
	
	IF @P_Debug = 1 PRINT 'Deleting existing Role associated with the name value pair before inseting new ones.'
	EXEC ZGWSystem.Delete_Roles_Security_Entities_Permissions @P_NVP_SeqID,@P_Security_Entity_SeqID,@P_Permissions_NVP_Detail_SeqID, @P_Debug
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
			SET @V_GROUP_NAME = LTRIM(RTRIM(LEFT(@P_Role, @V_Pos - 1)))
			IF @V_GROUP_NAME <> ''
			BEGIN
				IF @P_Debug = 1 PRINT 'select the Role_SeqID first'
				SELECT @V_Role_SeqID = ZGWSecurity.Roles.Role_SeqID 
				FROM ZGWSecurity.Roles 
				WHERE [NAME]=@V_GROUP_NAME

 				SELECT
					@V_Roles_Security_Entities_SeqID=Roles_Security_Entities_SeqID
				FROM
					ZGWSecurity.Roles_Security_Entities
				WHERE
					Role_SeqID = @V_Role_SeqID AND
					Security_Entity_SeqID = @P_Security_Entity_SeqID
					IF @P_Debug = 1 PRINT('@V_Roles_Security_Entities_SeqID = ' + CONVERT(VARCHAR,@V_Roles_Security_Entities_SeqID))
				IF NOT EXISTS(
						SELECT 
							Roles_Security_Entities_SeqID 
						FROM 
							ZGWSecurity.Roles_Security_Entities_Permissions 
						WHERE 
						NVP_SeqID = @P_NVP_SeqID 
						AND Permissions_NVP_Detail_SeqID = @P_Permissions_NVP_Detail_SeqID
						AND Roles_Security_Entities_SeqID = @V_Roles_Security_Entities_SeqID
				)
				BEGIN TRY
					IF @P_Debug = 1 PRINT('Inserting record')
					INSERT ZGWSecurity.Roles_Security_Entities_Permissions (
						NVP_SeqID,
						Roles_Security_Entities_SeqID,
						Permissions_NVP_Detail_SeqID,
						Added_By,
						Added_Date
					)
					VALUES (
						@P_NVP_SeqID,
						@V_Roles_Security_Entities_SeqID,
						@P_Permissions_NVP_Detail_SeqID,
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