/*
Usage:
	DECLARE 
		@P_NVP_SeqID int = 1,
		@P_Security_Entity_SeqID INT = 1,
		@P_Groups VARCHAR(MAX) = 'EveryOne',
		@P_Permissions_NVP_Detail_SeqID INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Debug int = 1

	exec ZGWSecurity.Set_Name_Value_Pair_Groups
		@P_NVP_SeqID,
		@P_Security_Entity_SeqID,
		@P_Groups,
		@P_Permissions_NVP_Detail_SeqID,
		@P_Added_Updated_By,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/26/2011
-- Description:	Delete and inserts into ZGWSecurity.Groups_Security_Entities_Functions
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Name_Value_Pair_Groups]
	@P_NVP_SeqID INT,
	@P_Security_Entity_SeqID INT,
	@P_Groups VARCHAR(1000),
	@P_Permissions_NVP_Detail_SeqID INT,
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS

IF @P_Debug = 1 PRINT('Starting ZGWSecurity.Set_Name_Value_Pair_Groups')
BEGIN TRAN
	DECLARE @V_Group_SeqID INT
			,@V_Groups_Security_Entities_SeqID INT
			,@V_GROUP_NAME VARCHAR(50)
			,@V_Pos INT
			,@V_ErrorMsg VARCHAR(MAX)
	
	IF @P_Debug = 1 PRINT 'Deleting existing Groups associated with the name value pair before inseting new ones.'
	EXEC ZGWSystem.Delete_Groups_Security_Entities_Permissions @P_NVP_SeqID,@P_Security_Entity_SeqID,@P_Permissions_NVP_Detail_SeqID, @P_Debug
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
				SELECT @V_Group_SeqID = ZGWSecurity.Groups.Group_SeqID 
				FROM ZGWSecurity.Groups 
				WHERE [Name]=@V_GROUP_NAME

 				SELECT
					@V_Groups_Security_Entities_SeqID=Groups_Security_Entities_SeqID
				FROM
					ZGWSecurity.Groups_Security_Entities
				WHERE
					Group_SeqID = @V_Group_SeqID AND
					Security_Entity_SeqID = @P_Security_Entity_SeqID
					IF @P_Debug = 1 PRINT('@V_Groups_Security_Entities_SeqID = ' + CONVERT(VARCHAR,@V_Groups_Security_Entities_SeqID))
				IF NOT EXISTS(
						SELECT 
							Groups_Security_Entities_SeqID 
						FROM 
							ZGWSecurity.Groups_Security_Entities_Permissions 
						WHERE 
						NVP_SeqID = @P_NVP_SeqID 
						AND Permissions_NVP_Detail_SeqID = @P_Permissions_NVP_Detail_SeqID
						AND Groups_Security_Entities_SeqID = @V_Groups_Security_Entities_SeqID
				)
				BEGIN TRY
					IF @P_Debug = 1 PRINT('Inserting record')
					INSERT ZGWSecurity.Groups_Security_Entities_Permissions (
						NVP_SeqID,
						Groups_Security_Entities_SeqID,
						Permissions_NVP_Detail_SeqID,
						Added_By
					)
					VALUES (
						@P_NVP_SeqID,
						@V_Groups_Security_Entities_SeqID,
						@P_Permissions_NVP_Detail_SeqID,
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