﻿CREATE PROCEDURE [ZFP_GET_Security_Entity]
	@P_SE_SEQ_ID int,
	@P_ErrorCode int OUTPUT
AS
-- SELECT an existing row OR rows from the table.
IF @P_SE_SEQ_ID = -1
	BEGIN
		SELECT
			SE_SEQ_ID,
			NAME,
			DESCRIPTION,
			URL,
			STATUS_SEQ_ID,
			DAL,
			DAL_NAME,
			DAL_NAME_SPACE,
			DAL_STRING,
			SKIN,
			STYLE,
			PARENT_SE_SEQ_ID,
			ENCRYPTION_TYPE,
			ADDED_BY,
			ADDED_DATE,
			UPDATED_BY,
			UPDATED_DATE
		FROM
			ZFC_SECURITY_ENTITIES
		ORDER BY 
			NAME ASC
	END
ELSE
	BEGIN
		SELECT
			SE_SEQ_ID,
			NAME,
			DESCRIPTION,
			URL,
			STATUS_SEQ_ID,
			DAL,
			DAL_NAME,
			DAL_NAME_SPACE,
			DAL_STRING,
			SKIN,
			STYLE,
			PARENT_SE_SEQ_ID,
			ENCRYPTION_TYPE,
			ADDED_BY,
			ADDED_DATE,
			UPDATED_BY,
			UPDATED_DATE
		FROM 
			ZFC_SECURITY_ENTITIES
		WHERE
			SE_SEQ_ID = @P_SE_SEQ_ID
		ORDER BY 
			NAME ASC
	END
-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR