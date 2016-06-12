CREATE PROCEDURE [ZGWSystem].[Log_Error_Info]
	@P_Return_Row bit = 0
AS
	DECLARE @V_Error_SeqID INT
	INSERT ZGWSystem.Data_Errors (
		[ErrorNumber],
		[ErrorSeverity],
		[ErrorState],
		[ErrorProcedure],
		[ErrorLine],
		[ErrorMessage],
		[ErrorDate]
	)
	VALUES (
		ERROR_NUMBER(),
		ERROR_SEVERITY(),
		ERROR_STATE(),
		ERROR_PROCEDURE(),
		ERROR_LINE(),
		ERROR_MESSAGE(),
		GETDATE()
	)
	IF @P_Return_Row = 1
		BEGIN
			SELECT @V_Error_SeqID = SCOPE_IDENTITY()
	
			SELECT 
				[ErrorNumber],
				[ErrorSeverity],
				[ErrorState],
				[ErrorProcedure],
				[ErrorLine],
				[ErrorMessage],
				[ErrorDate]
			FROM
				ZGWSystem.Data_Errors
			WHERE
				Error_SeqID = @V_Error_SeqID
		END
	-- END IF
RETURN 0