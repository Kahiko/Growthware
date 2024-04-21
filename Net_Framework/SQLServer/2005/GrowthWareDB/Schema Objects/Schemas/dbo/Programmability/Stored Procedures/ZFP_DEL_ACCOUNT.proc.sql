CREATE PROCEDURE [ZFP_DEL_ACCOUNT]
	@P_ACCT_SEQ_ID int,
	@P_ErrorCode int OUTPUT
AS
SET NOCOUNT ON
-- DELETE an existing row from the table.
DELETE FROM ZFC_ACCTS
WHERE
	ACCT_SEQ_ID = @P_ACCT_SEQ_ID
-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR
