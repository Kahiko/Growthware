﻿CREATE PROCEDURE [ZFP_SET_ACCT_CHOICES]
	@P_ACCT VARCHAR(25),
	@P_SE_SEQ_ID int,
	@P_SE_NAME VARCHAR(256),
	@P_BACK_COLOR VARCHAR(15),
	@P_LEFT_COLOR VARCHAR(15),
	@P_HEAD_COLOR VARCHAR(15),
	@P_SUB_HEAD_COLOR VARCHAR(15),
	@P_COLOR_SCHEME VARCHAR(15),
	@P_FAVORIATE_ACTION VARCHAR(50),
	@P_THIN_ACTIONS VARCHAR(4000),
	@P_WIDE_ACTIONS VARCHAR(4000),
	@P_RECORDS_PER_PAGE int
	--@P_ErrorCode int OUTPUT
AS
-- INSERT a new row in the table.
	IF(SELECT COUNT(*) FROM ZFO_ACCT_CHOICES WHERE ACCT = @P_ACCT) <= 0
		BEGIN	
			INSERT ZFO_ACCT_CHOICES
			(
				ACCT,
				SE_SEQ_ID,
				SE_NAME,
				BACK_COLOR,
				LEFT_COLOR,
				HEAD_COLOR,
				SUB_HEAD_COLOR,
				COLOR_SCHEME,
				FAVORIATE_ACTION,
				THIN_ACTIONS,
				WIDE_ACTIONS,
				RECORDS_PER_PAGE
			)
			VALUES
			(
				@P_ACCT,
				@P_SE_SEQ_ID,
				@P_SE_NAME,
				@P_BACK_COLOR,
				@P_LEFT_COLOR,
				@P_HEAD_COLOR,
				@P_SUB_HEAD_COLOR,
				@P_COLOR_SCHEME,
				@P_FAVORIATE_ACTION,
				@P_THIN_ACTIONS,
				@P_WIDE_ACTIONS,
				@P_RECORDS_PER_PAGE
			)
		END
	ELSE
		BEGIN
			UPDATE ZFO_ACCT_CHOICES
			SET
				SE_SEQ_ID = @P_SE_SEQ_ID,
				SE_NAME = @P_SE_NAME,
				BACK_COLOR =@P_BACK_COLOR ,
				LEFT_COLOR=@P_LEFT_COLOR,
				HEAD_COLOR=@P_HEAD_COLOR,
				SUB_HEAD_COLOR=@P_SUB_HEAD_COLOR,
				COLOR_SCHEME=@P_COLOR_SCHEME,
				FAVORIATE_ACTION=@P_FAVORIATE_ACTION,
				THIN_ACTIONS=@P_THIN_ACTIONS,
				WIDE_ACTIONS = @P_WIDE_ACTIONS,
				RECORDS_PER_PAGE=@P_RECORDS_PER_PAGE
			WHERE
				ACCT=@P_ACCT
		END
	-- END IF
-- Get the Error Code for the statement just executed.
--SELECT @P_ErrorCode=@@ERROR