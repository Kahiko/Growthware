﻿CREATE PROCEDURE [ZFP_SET_DIRECTORY](
	@P_FUNCTION_SEQ_ID INT,
	@P_DIRECTORY VARCHAR(255),
	@P_IMPERSONATE INT,
	@P_IMPERSONATE_ACCOUNT VARCHAR(50),
	@P_IMPERSONATE_PWD VARCHAR(50),
	@P_ADDED_BY INT,
	@P_ADDED_DATE DATETIME,
	@P_UPDATED_BY INT,
	@P_UPDATED_DATE DATETIME,
	@P_PRIMARY_KEY INT OUTPUT,
	@P_ErrorCode int OUTPUT
) AS
IF (SELECT COUNT(*) FROM ZFO_DIRECTORIES WHERE FUNCTION_SEQ_ID = @P_FUNCTION_SEQ_ID) = 0
	BEGIN
		INSERT ZFO_DIRECTORIES
		(
			FUNCTION_SEQ_ID,
			DIRECTORY,
			IMPERSONATE,
			IMPERSONATE_ACCOUNT,
			IMPERSONATE_PWD,
			ADDED_BY,
			ADDED_DATE,
			UPDATED_BY,
			UPDATED_DATE
		)
		VALUES
		(
			@P_FUNCTION_SEQ_ID,
			@P_DIRECTORY,
			@P_IMPERSONATE,
			@P_IMPERSONATE_ACCOUNT,
			@P_IMPERSONATE_PWD,
			@P_ADDED_BY,
			@P_ADDED_DATE,
			@P_UPDATED_BY,
			@P_UPDATED_DATE
		)

		SELECT @P_PRIMARY_KEY = @P_FUNCTION_SEQ_ID
	END
ELSE
	BEGIN
		UPDATE ZFO_DIRECTORIES
		SET 
			FUNCTION_SEQ_ID = @P_FUNCTION_SEQ_ID,
			DIRECTORY = @P_DIRECTORY,
			IMPERSONATE = @P_IMPERSONATE,
			IMPERSONATE_ACCOUNT = @P_IMPERSONATE_ACCOUNT,
			IMPERSONATE_PWD = @P_IMPERSONATE_PWD,
			UPDATED_BY = @P_UPDATED_BY,
			UPDATED_DATE = @P_UPDATED_DATE
		WHERE
			FUNCTION_SEQ_ID = @P_FUNCTION_SEQ_ID

		SELECT @P_PRIMARY_KEY = @P_FUNCTION_SEQ_ID
	END
-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR
