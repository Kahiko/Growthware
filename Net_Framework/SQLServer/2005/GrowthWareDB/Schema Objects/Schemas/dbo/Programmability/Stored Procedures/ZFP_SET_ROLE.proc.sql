﻿CREATE PROCEDURE [ZFP_SET_ROLE] (
	@P_ROLE_SEQ_ID INT,
	@P_NAME VARCHAR(50),
	@P_DESCRIPTION VARCHAR(128),
	@P_IS_SYSTEM INT,
	@P_IS_SYSTEM_ONLY INT,
	@P_SE_SEQ_ID INT,
	@P_ADDED_BY	INT,
	@P_ADDED_DATE DATETIME,
	@P_UPDATED_BY INT,
	@P_UPDATED_DATE DATETIME,
	@P_PRIMARY_KEY int OUTPUT,
	@P_ErrorCode int OUTPUT

) AS
	DECLARE @RLS_SEQ_ID INT
	DECLARE @MYMSG AS VARCHAR(128)

	IF (SELECT COUNT(*) FROM ZFC_SECURITY_RLS WHERE IS_SYSTEM_ONLY = 1 AND [NAME] = @P_NAME) > 0
	BEGIN
		SET @MYMSG = 'THE ROLE YOU ENTERED ' + @P_NAME + ' IS FOR SYSTEM USE ONLY.'
		RAISERROR (@MYMSG,16,1)
		RETURN
	END

	IF @P_ROLE_SEQ_ID > -1
		BEGIN -- UPDATE PROFILE
			UPDATE ZFC_SECURITY_RLS
			SET 
				[NAME] = @P_NAME,
				[DESCRIPTION] = @P_DESCRIPTION,
				IS_SYSTEM = @P_IS_SYSTEM,
				IS_SYSTEM_ONLY = @P_IS_SYSTEM_ONLY,
				UPDATED_BY = @P_UPDATED_BY,
				UPDATED_DATE = @P_UPDATED_DATE
			WHERE
				ROLE_SEQ_ID = @P_ROLE_SEQ_ID

			SELECT @P_PRIMARY_KEY = @P_ROLE_SEQ_ID
		END
	ELSE
		BEGIN -- INSERT a new row in the table.
			-- CHECK FOR DUPLICATE NAME BEFORE INSERTING
			IF NOT EXISTS( SELECT [NAME] 
				   FROM ZFC_SECURITY_RLS
				   WHERE [NAME] = @P_NAME)
				BEGIN
					INSERT ZFC_SECURITY_RLS
					(
						[NAME],
						[DESCRIPTION],
						IS_SYSTEM,
						IS_SYSTEM_ONLY,
						ADDED_BY,
						ADDED_DATE,
						UPDATED_BY,
						UPDATED_DATE
					)
					VALUES
					(
						@P_NAME,
						@P_DESCRIPTION,
						@P_IS_SYSTEM,
						@P_IS_SYSTEM_ONLY,
						@P_ADDED_BY,
						@P_ADDED_DATE,
						@P_ADDED_BY,
						@P_ADDED_DATE
					)
					SELECT @P_PRIMARY_KEY=SCOPE_IDENTITY() -- Get the IDENTITY value for the row just inserted.
				END
			ELSE
				SET @P_PRIMARY_KEY = (SELECT ROLE_SEQ_ID FROM ZFC_SECURITY_RLS WHERE [NAME] = @P_NAME)
			-- END IF
		END
	-- END IF
	IF(SELECT COUNT(*) FROM ZFC_SECURITY_RLS_SE WHERE SE_SEQ_ID = @P_SE_SEQ_ID AND ROLE_SEQ_ID = @P_PRIMARY_KEY) = 0 
	BEGIN  -- ADD ROLE REFERENCE TO SE_SECURITY
			INSERT ZFC_SECURITY_RLS_SE (
				SE_SEQ_ID,
				ROLE_SEQ_ID
			)
			VALUES (
				@P_SE_SEQ_ID,
				@P_PRIMARY_KEY
			)
	END

-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR