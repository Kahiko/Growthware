CREATE PROCEDURE [ZFP_DEL_NVP_DETAIL]
	@P_NVP_SEQ_DET_ID INT,
	@P_NVP_SEQ_ID int,
	@P_ErrorCode int OUTPUT
AS
SET NOCOUNT ON
-- DELETE an existing row from the table.
	DECLARE @V_Statement NVARCHAR(4000),
			@V_STATIC_NAME VARCHAR(30)

	SET @V_STATIC_NAME = (SELECT STATIC_NAME FROM ZFC_NVP WHERE NVP_SEQ_ID = @P_NVP_SEQ_ID)

	SET @V_Statement= 'DELETE 
		   FROM ' + CONVERT(VARCHAR,@V_STATIC_NAME) + '
		   WHERE NVP_SEQ_DET_ID = ''' + CONVERT(VARCHAR,@P_NVP_SEQ_DET_ID) + ''''
	EXECUTE sp_executesql @V_Statement
-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR
