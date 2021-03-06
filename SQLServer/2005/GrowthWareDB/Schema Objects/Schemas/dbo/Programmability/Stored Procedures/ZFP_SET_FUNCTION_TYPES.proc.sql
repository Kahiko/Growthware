﻿CREATE PROCEDURE [ZFP_SET_FUNCTION_TYPES](
	@P_FUNCTION_TYPE_SEQ_ID INT,
	@P_NAME VARCHAR(50),
	@P_DESCRIPTION VARCHAR(256),
	@P_TEMPLATE VARCHAR(256),
	@P_IS_CONTENT INT,
	@P_ADDED_BY	INT,
	@P_ADDED_DATE DATETIME,
	@P_UPDATED_BY INT,
	@P_UPDATED_DATE DATETIME,
	@P_PRIMARY_KEY int OUTPUT,
	@P_ErrorCode int OUTPUT

) AS

	IF @P_FUNCTION_TYPE_SEQ_ID > -1
		BEGIN -- UPDATE PROFILE
			UPDATE ZFC_FUNCTION_TYPES
			SET 
				[NAME] = @P_NAME,
				[DESCRIPTION] = @P_DESCRIPTION,
				TEMPLATE = @P_TEMPLATE,
				IS_CONTENT = @P_IS_CONTENT,
				UPDATED_BY = @P_UPDATED_BY,
				UPDATED_DATE = @P_UPDATED_DATE
			WHERE
				FUNCTION_TYPE_SEQ_ID = @P_FUNCTION_TYPE_SEQ_ID

			SELECT @P_PRIMARY_KEY = @P_FUNCTION_TYPE_SEQ_ID
		END
	ELSE
	BEGIN -- INSERT a new row in the table.

			-- CHECK FOR DUPLICATE NAME BEFORE INSERTING
			IF EXISTS( SELECT @P_DESCRIPTION 
				   FROM ZFC_FUNCTION_TYPES
				   WHERE [NAME] = @P_NAME
			)
			BEGIN
				RAISERROR ('THE FUNCTION TYPE YOU ENTERED ALREADY EXISTS IN THE DATABASE.',16,1)
				RETURN
			END

			INSERT ZFC_FUNCTION_TYPES
			(
				[NAME],
				[DESCRIPTION],
				TEMPLATE,
				IS_CONTENT,
				ADDED_BY,
				ADDED_DATE,
				UPDATED_BY,
				UPDATED_DATE
			)
			VALUES
			(
				@P_NAME,
				@P_DESCRIPTION,
				@P_TEMPLATE,
				@P_IS_CONTENT,
				@P_ADDED_BY,
				@P_ADDED_DATE,
				@P_ADDED_BY,
				@P_ADDED_DATE
			)
			SELECT @P_PRIMARY_KEY=SCOPE_IDENTITY() -- Get the IDENTITY value for the row just inserted.
		END
-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR
