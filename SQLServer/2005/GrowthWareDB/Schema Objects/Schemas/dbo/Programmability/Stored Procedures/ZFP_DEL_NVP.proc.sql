CREATE PROCEDURE [ZFP_DEL_NVP]
	@P_NVP_SEQ_ID int,
	@P_ErrorCode int OUTPUT
AS
SET NOCOUNT ON
-- DELETE an existing row from the table.
DELETE FROM ZOF_NVP
WHERE
	NVP_SEQ_ID = @P_NVP_SEQ_ID
-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR
