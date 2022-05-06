
/*
Usage:
	DECLARE 
		@P_GroupSeqId INT = 1,
		@P_Name VARCHAR(128) = 'Test',
		@P_Description VARCHAR(512) = ' ',
		@PSecurityEntitySeqId INT = 1,
		@P_Added_Updated_By INT = 2,
		@P_Primary_Key int,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Group
		@P_GroupSeqId,
		@P_Name,
		@P_Description,
		@PSecurityEntitySeqId,
		@P_Added_Updated_By,
		@P_Primary_Key,
		@P_Debug
	PRINT 'Primary key is: ' + CONVERT(VARCHAR(30),@P_Primary_Key)
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/02/2011
-- Description:	Inserts or updates ZGWSecurity.Groups
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Group]
	@P_GroupSeqId INT,
	@P_Name VARCHAR(128),
	@P_Description VARCHAR(512),
	@PSecurityEntitySeqId INT,
	@P_Added_Updated_By INT,
	@P_Primary_Key int OUTPUT,
	@P_Debug INT = 0
AS
	DECLARE @RLS_SEQ_ID INT
			,@V_Added_Updated_Date DATETIME = GETDATE()

	IF @P_GroupSeqId > -1
		BEGIN -- UPDATE PROFILE
			UPDATE ZGWSecurity.Groups
			SET 
				[Name] = @P_Name,
				[Description] = @P_Description,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = @V_Added_Updated_Date
			WHERE
				GroupSeqId = @P_GroupSeqId

			SELECT @P_Primary_Key = @P_GroupSeqId
		END
	ELSE
		BEGIN -- INSERT a new row in the table.
			-- CHECK FOR DUPLICATE Name BEFORE INSERTING
			IF NOT EXISTS( SELECT [Name] 
				   FROM ZGWSecurity.Groups
				   WHERE [Name] = @P_Name)
				BEGIN
					INSERT ZGWSecurity.Groups
					(
						[Name],
						[Description],
						Added_By,
						Added_Date
					)
					VALUES
					(
						@P_Name,
						@P_Description,
						@P_Added_Updated_By,
						@V_Added_Updated_Date
					)
					SELECT @P_Primary_Key=SCOPE_IDENTITY() -- Get the IDENTITY value for the row just inserted.
				END
			ELSE
				--PRINT 'ENTERING SECURITY INFORMATION FOR THE GROUP'
				SET @P_Primary_Key = (SELECT GroupSeqId FROM ZGWSecurity.Groups WHERE [Name] = @P_Name)
			-- END IF
		END
	-- END IF
	IF(SELECT COUNT(*) FROM ZGWSecurity.Groups_Security_Entities WHERE SecurityEntitySeqId = @PSecurityEntitySeqId AND GroupSeqId = @P_Primary_Key) = 0 
	BEGIN  -- ADD GROUP REFERENCE TO SE_SECURITY
			INSERT ZGWSecurity.Groups_Security_Entities (
				SecurityEntitySeqId,
				GroupSeqId,
				Added_By,
				Added_Date
			)
			VALUES (
				@PSecurityEntitySeqId,
				@P_Primary_Key,
				@P_Added_Updated_By,
				@V_Added_Updated_Date
			)
	END
RETURN 0

GO

