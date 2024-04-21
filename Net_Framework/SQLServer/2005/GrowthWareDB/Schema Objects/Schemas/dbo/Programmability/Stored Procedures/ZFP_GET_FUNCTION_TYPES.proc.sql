CREATE PROCEDURE [ZFP_GET_FUNCTION_TYPES]
	@P_ErrorCode int OUTPUT
AS
-- SELECT all rows from the table.
SELECT
	*
FROM
	ZFC_FUNCTION_TYPES

-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR
