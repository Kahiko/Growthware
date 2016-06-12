CREATE PROCEDURE [ZFP_GET_NVP_DETAIL]
	@P_NVP_SEQ_ID INT,
	@P_NVP_SEQ_DET_ID INT,
	@P_ErrorCode int OUTPUT
AS
	DECLARE @V_STATIC_NAME VARCHAR(30)
	DECLARE @V_Statement nvarchar(4000)
	SET @V_STATIC_NAME = (SELECT STATIC_NAME FROM ZFC_NVP WHERE NVP_SEQ_ID = @P_NVP_SEQ_ID)
	SET @V_Statement = 'SELECT * FROM ' + CONVERT(VARCHAR,@V_STATIC_NAME) + '
	WHERE
		NVP_SEQ_DET_ID = ' + CONVERT(VARCHAR,@P_NVP_SEQ_DET_ID)

	EXECUTE dbo.sp_executesql @statement = @V_Statement
	-- PRINT @V_Statement
-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR
