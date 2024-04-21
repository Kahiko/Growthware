-- =============================================
-- Author:		Michael Regan
-- Create date: 7/19/2008
-- Description:	Add's or updates the information in the ZFC_INFORMATION table.
-- =============================================
CREATE PROCEDURE [ZFP_SET_INFORMATION] 
	@P_Information_SEQ_ID INT,
	@P_VERSION VARCHAR(15),
	@P_Enable_Inheritance INT,
	@P_ADD_UP_BY INT,
	@P_PRIMARY_KEY int OUTPUT,
	@P_ErrorCode int OUTPUT

AS
BEGIN
	SET NOCOUNT ON;
	IF (SELECT COUNT(*) FROM ZFC_INFORMATION) = 0
		BEGIN -- INSERT
			INSERT ZFC_INFORMATION
			(
				VERSION,
				Enable_Inheritance,
				ADDED_BY,
				ADDED_DATE,
				UPDATED_BY, -- NO NULL VALUES PLEASE
				UPDATED_DATE -- NO NULL VALUES PLEASE
			)
			VALUES
			(
				@P_VERSION,
				@P_Enable_Inheritance,
				@P_ADD_UP_BY,
				GETDATE(),
				@P_ADD_UP_BY,
				GETDATE()
			)
			SELECT @P_PRIMARY_KEY = SCOPE_IDENTITY()-- Get the IDENTITY value for the row just inserted.
		END
	ELSE-- UPDATE
		BEGIN
			UPDATE ZFC_INFORMATION
			SET 
				VERSION = @P_VERSION,
				Enable_Inheritance = @P_Enable_Inheritance,
				UPDATED_BY = @P_ADD_UP_BY,
				UPDATED_DATE = GETDATE()
			WHERE
				Information_SEQ_ID = @P_Information_SEQ_ID

			SET @P_PRIMARY_KEY = @P_Information_SEQ_ID

		END
	-- END IF
END
