/*
Usage:
	DECLARE @V_Status_SeqID INT,
			@V_Added_Updated_By INT,
			@V_PRIMARY_KEY INT,
			@V_ErrorCode INT
	SET @V_Status_SeqID = -1
	SET @V_Added_Updated_By = 1
	SET @V_PRIMARY_KEY = NULL -- Not needed when setup up the database
	SET @V_ErrorCode = NULL -- Not needed when setup up the database
Insert new
	exec [ZGWSystem].[Set_System_Status] @V_Status_SeqID,'Active','Active Status',@V_Added_Updated_By,@V_PRIMARY_KEY,@V_ErrorCode
Update
	SET @V_Status_SeqID = 1
	exec [ZGWSystem].[Set_System_Status] @V_Status_SeqID,'Active','Active Status',@V_Added_Updated_By,@V_PRIMARY_KEY,@V_ErrorCode
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates [ZGWSystem].[Statuses]
--	@P_Status_SeqID's value determines insert/update
--	a value of -1 is insert > -1 performs update
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Set_System_Status]
	@P_Status_SeqID int,
	@P_Name VARCHAR(25),
	@P_Description VARCHAR(512) = null,
	@P_Added_Updated_By int,
	@P_Primary_Key int OUTPUT,
	@P_ErrorCode int OUTPUT,
	@P_Debug INT = 0
AS
	IF @P_Status_SeqID > -1
		BEGIN -- UPDATE PROFILE
			UPDATE [ZGWSystem].[Statuses]
			SET 
				[Name] = @P_Name,
				[Description] = @P_Description,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = GETDATE()
			WHERE
				Status_SeqID = @P_Status_SeqID

			SELECT @P_Primary_Key = @P_Status_SeqID
		END
	ELSE
	BEGIN -- INSERT a new row in the table.

			-- CHECK FOR DUPLICATE NAME BEFORE INSERTING
			IF EXISTS( SELECT 1 FROM [ZGWSystem].[Statuses] WHERE [Name] = @P_Name)
				BEGIN
					RAISERROR ('THE STATUS YOU ENTERED ALREADY EXISTS IN THE DATABASE.',16,1)
					RETURN
				END
			-- END IF
			INSERT [ZGWSystem].[Statuses]
			(
				[Name],
				[Description],
				Added_By ,
				Added_Date 
			)
			VALUES
			(
				@P_Name,
				@P_Description,
				@P_Added_Updated_By ,
				GETDATE() 
			)
			SELECT @P_Status_SeqID = SCOPE_IDENTITY() -- Get the IDENTITY value for the row just inserted.
		END
-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR