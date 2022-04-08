
/*
Usage:
	DECLARE 
		@P_Role_SeqID INT = -1,
		@P_Name VARCHAR(50) = 'Test',
		@P_Description VARCHAR(128) = 'Testing',
		@P_Is_System INT = 0,
		@P_Is_System_Only INT = 0,
		@P_Security_Entity_SeqID INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Primary_Key int,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Role
		@P_Role_SeqID,
		@P_Name,
		@P_Description,
		@P_Is_System,
		@P_Is_System_Only,
		@P_Security_Entity_SeqID,
		@P_Added_Updated_By,
		@P_Primary_Key OUT,
		@P_Debug
		
	PRINT '@P_Primary_Key = ' + CONVERT(VARCHAR(MAX),@P_Primary_Key)
	
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/08/2011
-- Description:	Inserts or updates ZGWSecurity.Roles and
--	ZGWSecurity.Roles_Security_Entities
-- Note: @P_Role_SeqID value of -1 inserts a new record
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Role]
	@P_Role_SeqID INT,
	@P_Name VARCHAR(50),
	@P_Description VARCHAR(128),
	@P_Is_System INT,
	@P_Is_System_Only INT,
	@P_Security_Entity_SeqID INT,
	@P_Added_Updated_By INT,
	@P_Primary_Key int OUTPUT,
	@P_Debug INT = 0
AS
IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Roles'
BEGIN TRAN
	DECLARE @V_RLS_SEQ_ID INT
			,@V_Message AS VARCHAR(128)
			,@V_Now DATETIME = GETDATE()
			,@V_ErrorMsg VARCHAR(MAX)

	IF (SELECT COUNT(*) FROM ZGWSecurity.Roles WHERE Is_System_Only = 1 AND [Name] = @P_Name) > 0
	BEGIN
		SET @V_Message = 'The role you entered ' + @P_Name + ' is for system use only.'
		RAISERROR (@V_Message,16,1)
		RETURN
	END

	IF @P_Role_SeqID > -1
		BEGIN
			IF @P_Debug = 1 PRINT 'Updating role in ZGWSecurity.Roles'
			UPDATE ZGWSecurity.Roles
			SET 
				[Name] = @P_Name,
				[Description] = @P_Description,
				Is_System = @P_Is_System,
				Is_System_Only = @P_Is_System_Only,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = @V_Now
			WHERE
				Role_SeqID = @P_Role_SeqID

			SELECT @P_Primary_Key = @P_Role_SeqID
		END
	ELSE
		BEGIN TRY -- INSERT a new row in the table.
			-- CHECK FOR DUPLICATE Name BEFORE INSERTING
			IF NOT EXISTS( SELECT [Name] 
				   FROM ZGWSecurity.Roles
				   WHERE [Name] = @P_Name)
				BEGIN
					IF @P_Debug = 1 PRINT 'Add role to ZGWSecurity.Roles'
					INSERT ZGWSecurity.Roles
					(
						[Name],
						[Description],
						Is_System,
						Is_System_Only,
						Added_By,
						Added_Date
					)
					VALUES
					(
						@P_Name,
						@P_Description,
						@P_Is_System,
						@P_Is_System_Only,
						@P_Added_Updated_By,
						@V_Now
					)
					SELECT @P_Primary_Key=SCOPE_IDENTITY() -- Get the IDENTITY value for the row just inserted.
				END
			ELSE
				SET @P_Primary_Key = (SELECT Role_SeqID FROM ZGWSecurity.Roles WHERE [Name] = @P_Name)
			-- END IF
		END TRY
		BEGIN CATCH
			GOTO ABEND		
		END CATCH
	-- END IF
	IF(SELECT COUNT(*) FROM ZGWSecurity.Roles_Security_Entities WHERE Security_Entity_SeqID = @P_Security_Entity_SeqID AND Role_SeqID = @P_Primary_Key) = 0 
	BEGIN TRY  -- ADD ROLE REFERENCE TO SE_SECURITY
			IF @P_Debug = 1 PRINT 'Add role reference to ZGWSecurity.Roles_Security_Entities'
			INSERT ZGWSecurity.Roles_Security_Entities (
				Security_Entity_SeqID
				, Role_SeqID
				, Added_By
				, Added_Date
			)
			VALUES (
				@P_Security_Entity_SeqID,
				@P_Primary_Key,
				@P_Added_Updated_By,
				@V_Now
			)
	END TRY
	BEGIN CATCH
		GOTO ABEND	
	END CATCH
COMMIT TRAN
IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Roles'
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

