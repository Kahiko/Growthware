CREATE PROCEDURE [ZFP_GET_NAVIGATION_TYPES]
	@P_ErrorCode int OUTPUT
AS
-- SELECT all rows from the table.
SELECT
	*
FROM
	ZFC_NAVIGATION_TYPES
ORDER BY
	NVP_DET_TEXT

-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR
