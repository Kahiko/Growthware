/*
Usage:
	DECLARE 
		@P_MessageSeqId INT = 1,
		@P_SecurityEntitySeqId INT = 2,
		@P_Name VARCHAR(50) 'Test',
		@P_Title VARCHAR(100) = 'Just Testing',
		@P_Description VARCHAR(512) = 'Some description',
		@P_Body VARCHAR(MAX) = 'The body',
		@P_Format_As_HTML INT = 0,
		@P_Added_Updated_By INT = 1,
		@P_Primary_Key int,
		@P_Debug INT = 1

	exec ZGWCoreWeb.Set_Message
		@P_MessageSeqId,
		@P_SecurityEntitySeqId,
		@P_Name,
		@P_Title,
		@P_Description,
		@P_Body,
		@P_Format_As_HTML,
		@P_Added_Updated_By,
		@P_Primary_Key OUT,
		@P_Debug

	PRINT 'Primay key is: ' + CONVERT(VARCHAR(30),@P_Primary_Key)
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/02/2011
-- Description:	Inserts or updates ZGWCoreWeb.[Messages]
-- =============================================
-- Author:		Michael Regan
-- Modified date: 09/24/2023
-- Description:	Was not return the correct value for P_Primary_Key when updating
-- =============================================
CREATE PROCEDURE [ZGWCoreWeb].[Set_Message] @P_MessageSeqId INT
	,@P_SecurityEntitySeqId INT
	,@P_Name VARCHAR(50)
	,@P_Title VARCHAR(100)
	,@P_Description VARCHAR(512)
	,@P_Body VARCHAR(MAX)
	,@P_Format_As_HTML INT
	,@P_Added_Updated_By INT
	,@P_Primary_Key INT OUTPUT
	,@P_Debug INT = 0
AS
IF @P_Debug = 1
	PRINT 'Starting ZGWSecurity.Set_Message'

DECLARE @V_Now DATETIME = GETDATE()

IF @P_MessageSeqId > - 1
BEGIN
	-- UPDATE PROFILE
	-- CHECK FOR DUPLICATE Name BEFORE INSERTING
	IF EXISTS (SELECT [Name] FROM ZGWCoreWeb.[Messages] WHERE [Name] = @P_Name AND SecurityEntitySeqId = @P_SecurityEntitySeqId)
		BEGIN
			UPDATE ZGWCoreWeb.[Messages]
				SET SecurityEntitySeqId = @P_SecurityEntitySeqId
					,[Name] = @P_Name
					,Title = @P_Title
					,[Description] = @P_Description
					,Format_As_HTML = @P_Format_As_HTML
					,Body = @P_Body
					,Updated_By = @P_Added_Updated_By
					,Updated_Date = GETDATE()
				WHERE MessageSeqId = @P_MessageSeqId
					AND SecurityEntitySeqId = @P_SecurityEntitySeqId
			-- set the output id just in case.
			SELECT 
				@P_Primary_Key = [MessageSeqId]
			FROM 
				ZGWCoreWeb.[Messages] 
			WHERE 
				[Name] = @P_Name AND SecurityEntitySeqId = @P_SecurityEntitySeqId
		END
	ELSE
		BEGIN
			INSERT ZGWCoreWeb.[Messages] (
					SecurityEntitySeqId
				,[Name]
				,Title
				,[Description]
				,Body
				,Format_As_HTML
				,Added_By
				,Added_Date
			) VALUES (
					@P_SecurityEntitySeqId
				,@P_Name
				,@P_Title
				,@P_Description
				,@P_Body
				,@P_Format_As_HTML
				,@P_Added_Updated_By
				,@V_Now
			);
			-- Get the IDENTITY value for the row just inserted.
			SELECT @P_Primary_Key = SCOPE_IDENTITY()
		END
	END
ELSE
	BEGIN
		-- INSERT a new row in the table.
		-- CHECK FOR DUPLICATE Name BEFORE INSERTING
		IF EXISTS (SELECT [Name] FROM ZGWCoreWeb.[Messages] WHERE [Name] = @P_Name AND SecurityEntitySeqId = @P_SecurityEntitySeqId)
			BEGIN
				RAISERROR ('The message you entered already exists in the database.',16,1);
				RETURN
			END
		-- END IF
		INSERT ZGWCoreWeb.[Messages] (
			 SecurityEntitySeqId
			,[Name]
			,Title
			,[Description]
			,Body
			,Format_As_HTML
			,Added_By
			,Added_Date
		) VALUES (
			 @P_SecurityEntitySeqId
			,@P_Name
			,@P_Title
			,@P_Description
			,@P_Body
			,@P_Format_As_HTML
			,@P_Added_Updated_By
			,@V_Now
			);

		SELECT @P_Primary_Key = SCOPE_IDENTITY()
			-- Get the IDENTITY value for the row just inserted.
	END
-- END IF
IF @P_Debug = 1
	PRINT 'Ending ZGWSecurity.Set_Message'

RETURN 0
GO
