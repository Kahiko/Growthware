/*
Usage:
	DECLARE 
		@P_Function_SeqID int = 1,
		@P_Security_Entity_SeqID INT = 1,
		@P_Groups VARCHAR(MAX) = 'EveryOne',
		@P_Permissions_NVP_Detail_SeqID INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Function_Groups
		@P_Function_SeqID,
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
CREATE PROCEDURE [ZGWSecurity].[Set_Function_Groups]
	@P_Function_SeqID int,
	@P_Security_Entity_SeqID INT,
	@P_Groups VARCHAR(MAX),
	@P_Permissions_NVP_Detail_SeqID INT,
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS
BEGIN TRANSACTION
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Groups_Security_Entities_Functions'
	-- NEED TO DELETE EXISTING Group ASSOCITAED WITH THE FUNCTION BEFORE 
	-- INSERTING NEW ONES.
	
	DECLARE @V_ErrorCodde INT
			,@V_Group_SeqID INT
			,@V_Groups_Security_Entities_SeqID AS INT
			,@V_Group_Name VARCHAR(50)
			,@V_Pos INT
			,@V_ErrorMsg VARCHAR(MAX)
			,@V_Now DATETIME = GETDATE()

	EXEC ZGWSecurity.Delete_Function_Groups @P_Function_SeqID,@P_Security_Entity_SeqID,@P_Permissions_NVP_Detail_SeqID,@P_Added_Updated_By,@V_ErrorCodde
	IF @@ERROR <> 0
	BEGIN
		EXEC ZGWSystem.Log_Error_Info @P_Debug
		SET @V_ErrorMsg = 'Error executing ZGWSecurity.Delete_Function_Groups' + CHAR(10)
		RAISERROR(@V_ErrorMsg,16,1)
		RETURN @@ERROR
	END
	SET @P_Groups = LTRIM(RTRIM(@P_Groups))+ ','
	SET @V_Pos = CHARINDEX(',', @P_Groups, 1)
	IF LEN(REPLACE(@P_Groups, ',', '')) > 0
		WHILE @V_Pos > 0
			BEGIN -- go through all the Groups and add if necessary
				SET @V_Group_Name = LTRIM(RTRIM(LEFT(@P_Groups, @V_Pos - 1)))
				IF @V_Group_Name <> ''
				BEGIN
					--select the Group seq id first
					SELECT 
						@V_Group_SeqID = ZGWSecurity.Groups.Group_SeqID 
					FROM 
						ZGWSecurity.Groups 
					WHERE 
						[Name]=@V_Group_Name
						
					--select the Groups_Security_Entities_SeqID
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
								ZGWSecurity.Groups_Security_Entities_Functions 
							WHERE 
							Function_SeqID = @P_Function_SeqID 
							AND Permissions_NVP_Detail_SeqID = @P_Permissions_NVP_Detail_SeqID
							AND Groups_Security_Entities_SeqID = @V_Groups_Security_Entities_SeqID)
						BEGIN TRY-- INSERT RECORD
							INSERT ZGWSecurity.Groups_Security_Entities_Functions (
								Function_SeqID,
								Groups_Security_Entities_SeqID,
								Permissions_NVP_Detail_SeqID,
								ADDED_BY,
								Added_Date
							)
							VALUES (
								@P_Function_SeqID,
								@V_Groups_Security_Entities_SeqID,
								@P_Permissions_NVP_Detail_SeqID,
								@P_Added_Updated_By,
								@V_Now
							)
						END TRY
						BEGIN CATCH
							GOTO ABEND
						END CATCH
					--END IF
				END
				SET @P_Groups = RIGHT(@P_Groups, LEN(@P_Groups) - @V_Pos)
				SET @V_Pos = CHARINDEX(',', @P_Groups, 1)
			END
		--END WHILE
	IF @@error <> 0 GOTO ABEND
Commit Transaction
RETURN 0
ABEND:
	IF @@error <> 0
		BEGIN
			ROLLBACK TRAN
			EXEC ZGWSystem.Log_Error_Info @P_Debug
			SET @V_ErrorMsg = 'Error executing ZGWSecurity.Set_Account_Roles' + CHAR(10)
			SET @V_ErrorMsg = @V_ErrorMsg + ERROR_MESSAGE()
			RAISERROR(@V_ErrorMsg,16,1)
			RETURN @@ERROR
		END
	--END IF