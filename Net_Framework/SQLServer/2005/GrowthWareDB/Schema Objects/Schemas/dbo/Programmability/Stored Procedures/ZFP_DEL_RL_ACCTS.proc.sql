﻿
CREATE PROCEDURE [ZFP_DEL_RL_ACCTS](
	@P_ROLE_SEQ_ID AS INT,
	@P_SE_SEQ_ID AS INT,
	@P_ADDUPD_BY	INT
)
AS
	DELETE
		ZFC_SECURITY_ACCTS_RLS
	WHERE
		RLS_SE_SEQ_ID IN (
			SELECT 
				RLS_SE_SEQ_ID 
			FROM 
				ZFC_SECURITY_RLS_SE 
			WHERE 
				ROLE_SEQ_ID = @P_ROLE_SEQ_ID 
				AND SE_SEQ_ID = @P_SE_SEQ_ID
		)
