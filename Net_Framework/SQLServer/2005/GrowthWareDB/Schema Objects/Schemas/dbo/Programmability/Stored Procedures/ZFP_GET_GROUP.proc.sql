﻿CREATE PROCEDURE [ZFP_GET_GROUP] (
	@P_SE_SEQ_ID AS INT,
	@P_GROUP_SEQ_ID AS INT,
	@P_ErrorCode INT OUTPUT
)
AS
	SET NOCOUNT ON
	IF @P_GROUP_SEQ_ID > -1 -- SELECT an existing row from the table.
		SELECT
			ZFC_SECURITY_GRPS.[GROUP_SEQ_ID],
			ZFC_SECURITY_GRPS.[NAME],
			ZFC_SECURITY_GRPS.[DESCRIPTION],
			ZFC_SECURITY_GRPS.[ADDED_BY],
			ZFC_SECURITY_GRPS.[ADDED_DATE],
			ZFC_SECURITY_GRPS.[UPDATED_BY],
			ZFC_SECURITY_GRPS.[UPDATED_DATE]
		FROM
			ZFC_SECURITY_GRPS
		WHERE
			GROUP_SEQ_ID = @P_GROUP_SEQ_ID
	ELSE -- GET ALL GROUPS FOR A GIVEN Security Entity
		SELECT
			ZFC_SECURITY_GRPS.[GROUP_SEQ_ID],
			ZFC_SECURITY_GRPS.[NAME],
			ZFC_SECURITY_GRPS.[DESCRIPTION],
			ZFC_SECURITY_GRPS.[ADDED_BY],
			ZFC_SECURITY_GRPS.[ADDED_DATE],
			ZFC_SECURITY_GRPS.[UPDATED_BY],
			ZFC_SECURITY_GRPS.[UPDATED_DATE]
		FROM
			ZFC_SECURITY_GRPS,
			ZFC_SECURITY_GRPS_SE
		WHERE
			ZFC_SECURITY_GRPS.GROUP_SEQ_ID = ZFC_SECURITY_GRPS_SE.GROUP_SEQ_ID
			AND ZFC_SECURITY_GRPS_SE.SE_SEQ_ID = @P_SE_SEQ_ID
	-- END IF		
	-- Get the Error Code for the statement just executed.
	SELECT @P_ErrorCode=@@ERROR