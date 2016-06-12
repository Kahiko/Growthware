CREATE PROCEDURE [ZFP_DEL_Security_Entity]
	@P_SE_SEQ_ID int,
	@P_ErrorCode int OUTPUT
AS
SET NOCOUNT ON
-- DELETE an existing row from the table.
DELETE FROM ZFC_SECURITY_ENTITIES
WHERE
	SE_SEQ_ID = @P_SE_SEQ_ID
-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR
