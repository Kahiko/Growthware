
/*
Usage:
	DECLARE 
		@P_FunctionSeqId int = 1,
		@P_SecurityEntitySeqId INT = 1,
		@P_Groups VARCHAR(MAX) = 'EveryOne',
		@P_PermissionsNVPDetailSeqId INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Function_Groups
		@P_FunctionSeqId,
		@P_SecurityEntitySeqId,
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
CREATE PROCEDURE [ZGWSecurity].[Set_Function_Groups]
	@P_FunctionSeqId int,
	@P_SecurityEntitySeqId INT,
	@P_Groups VARCHAR(MAX),
	@P_PermissionsNVPDetailSeqId INT,
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS
BEGIN TRANSACTION
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Groups_Security_Entities_Functions'
	-- NEED TO DELETE EXISTING Group ASSOCITAED WITH THE FUNCTION BEFORE 
	-- INSERTING NEW ONES.
	
	DECLARE @V_ErrorCodde INT
			,@V_GroupSeqId INT
			,@V_GroupsSecurityEntitiesSeqId AS INT
			,@V_Group_Name VARCHAR(50)
			,@V_Pos INT
			,@V_ErrorMsg VARCHAR(MAX)
			,@V_Now DATETIME = GETDATE()

	EXEC ZGWSecurity.Delete_Function_Groups @P_FunctionSeqId,@P_SecurityEntitySeqId,@P_PermissionsNVPDetailSeqId,@P_Added_Updated_By,@V_ErrorCodde
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
			BEGIN
	-- go through all the Groups and add if necessary
	SET @V_Group_Name = LTRIM(RTRIM(LEFT(@P_Groups, @V_Pos - 1)))
	IF @V_Group_Name <> ''
				BEGIN
		--select the Group seq id first
		SELECT
			@V_GroupSeqId = ZGWSecurity.Groups.GroupSeqId
		FROM
			ZGWSecurity.Groups
		WHERE 
						[Name]=@V_Group_Name

		--select the GroupsSecurityEntitiesSeqId
		SELECT
			@V_GroupsSecurityEntitiesSeqId=GroupsSecurityEntitiesSeqId
		FROM
			ZGWSecurity.Groups_Security_Entities
		WHERE
						GroupSeqId = @V_GroupSeqId AND
			SecurityEntitySeqId = @P_SecurityEntitySeqId

		IF @P_Debug = 1 PRINT('@V_GroupsSecurityEntitiesSeqId = ' + CONVERT(VARCHAR,@V_GroupsSecurityEntitiesSeqId))
		IF NOT EXISTS(
							SELECT
			GroupsSecurityEntitiesSeqId
		FROM
			ZGWSecurity.Groups_Security_Entities_Functions
		WHERE 
							FunctionSeqId = @P_FunctionSeqId
			AND PermissionsNVPDetailSeqId = @P_PermissionsNVPDetailSeqId
			AND GroupsSecurityEntitiesSeqId = @V_GroupsSecurityEntitiesSeqId)
						BEGIN TRY-- INSERT RECORD
							INSERT ZGWSecurity.Groups_Security_Entities_Functions
			(
			FunctionSeqId,
			GroupsSecurityEntitiesSeqId,
			PermissionsNVPDetailSeqId,
			Added_By,
			Added_Date
			)
		VALUES
			(
				@P_FunctionSeqId,
				@V_GroupsSecurityEntitiesSeqId,
				@P_PermissionsNVPDetailSeqId,
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

GO

