CREATE PROCEDURE [ZFP_DEL_WORK_FLOWS]
	@P_PRIMARY_KEY int,
	@P_ErrorCode int OUTPUT
AS
SET NOCOUNT ON
-- DELETE an existing row from the table.
DELETE FROM ZOP_WORK_FLOWS
WHERE
	WORK_FLOW_ID = @P_PRIMARY_KEY
-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR
