
/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'Developer',
		@PSecurityEntitySeqId INT = 1,
		@P_Groups VARCHAR(max) = 'Everyone',
		@P_Added_Updated_By INT = 2,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Account_Groups
		@P_Account,
		@PSecurityEntitySeqId,
		@P_Groups,
		@P_Added_Updated_By,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/25/2011
-- Description:	Set's the Groups associated
--	with an account.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Account_Groups]
	@P_Account VARCHAR(128),
	@PSecurityEntitySeqId INT,
	@P_Groups VARCHAR(max),
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Account_Groups'
	DECLARE @V_ErrorCode INT
	DECLARE @V_ErrorMsg VARCHAR(MAX)

	BEGIN TRAN
		DECLARE @AccountSeqId INT
		SET @AccountSeqId = (SELECT AccountSeqId FROM ZGWSecurity.Accounts WHERE Account = @P_Account)
		-- Deleting old records before inseting any new ones.
		IF @P_Debug = 1 PRINT 'Calling ZGWSecurity.Delete_Account_Groups'
		EXEC ZGWSecurity.Delete_Account_Groups @AccountSeqId, @PSecurityEntitySeqId, @P_Debug
		IF @@ERROR <> 0
			BEGIN
				EXEC ZGWSystem.Log_Error_Info @P_Debug
				SET @V_ErrorMsg = 'Error executing ZGWSecurity.Delete_Account_Groups' + CHAR(10)
				RAISERROR(@V_ErrorMsg,16,1)
				GOTO ABEND
			END
		--END IF
		DECLARE @V_GroupSeqId AS 	INT
		DECLARE @V_SecurityEntity_GroupSeqID AS 	INT
		DECLARE @V_Group_Name AS	VARCHAR(50)
		DECLARE @V_Pos AS	INT
		SET @P_Groups = LTRIM(RTRIM(@P_Groups))+ ','
		SET @V_Pos = CHARINDEX(',', @P_Groups, 1)
		IF REPLACE(@P_Groups, ',', '') <> ''
			WHILE @V_Pos > 0
			BEGIN
				SET @V_Group_Name = LTRIM(RTRIM(LEFT(@P_Groups, @V_Pos - 1)))
				IF @V_Group_Name <> ''
				BEGIN
					--select the role seq id first
					SELECT @V_GroupSeqId = ZGWSecurity.Groups.GroupSeqId 
					FROM ZGWSecurity.Groups 
					WHERE [Name]=@V_Group_Name

 					SELECT
						@V_SecurityEntity_GroupSeqID=Groups_Security_Entities_SeqID
					FROM
						ZGWSecurity.Groups_Security_Entities
					WHERE
						GroupSeqId = @V_GroupSeqId AND
						SecurityEntitySeqId = @PSecurityEntitySeqId
						IF @P_Debug = 1 PRINT ('@V_SecurityEntity_GroupSeqID = ' + CONVERT(VARCHAR,@V_SecurityEntity_GroupSeqID))
					IF NOT EXISTS(
							SELECT 
								Groups_Security_Entities_SeqID 
							FROM 
								ZGWSecurity.Groups_Security_Entities_Accounts 
							WHERE 
							AccountSeqId = @AccountSeqId 
							AND Groups_Security_Entities_SeqID = @V_SecurityEntity_GroupSeqID
					)
					BEGIN TRY
						IF @P_Debug = 1 PRINT 'Inserting records'
						INSERT ZGWSecurity.Groups_Security_Entities_Accounts (
							AccountSeqId,
							Groups_Security_Entities_SeqID,
							Added_By
						)
						VALUES (
							@AccountSeqId,
							@V_SecurityEntity_GroupSeqID,
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
		IF @@ERROR <> 0 GOTO ABEND
	COMMIT TRAN
	GOTO DONE
ABEND:
	BEGIN
		ROLLBACK TRAN
		EXEC ZGWSystem.Log_Error_Info @P_Debug
		SET @V_ErrorMsg = 'Error executing ZGWSecurity.Set_Account_Groups' + CHAR(10)
		SET @V_ErrorMsg = @V_ErrorMsg + ERROR_MESSAGE()
		RAISERROR(@V_ErrorMsg,16,1)
		RETURN @@ERROR
	END
DONE:
RETURN 0

GO

