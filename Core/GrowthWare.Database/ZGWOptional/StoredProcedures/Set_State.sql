
/*
Usage:
	DECLARE 
		@P_State VARCHAR(2) = 'MA',
		@P_Description VARCHAR(128) = 'Changed',
		@P_Status_SeqID INT = 1,
		@P_Updated_By INT = 1,
		@P_Primary_Key VARCHAR(2),
		@P_Debug INT = 0

	exec ZGWOptional.Set_State
		@P_State,
		@P_Description,
		@P_Status_SeqID,
		@P_Updated_By,
		@P_Primary_Key OUT,
		@P_Debug
	PRINT '@P_Primary_Key = ' + CONVERT(VARCHAR(MAX),@P_Primary_Key)
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/08/2011
-- Description:	Inserts into ZGWOptional.States
-- =============================================
CREATE PROCEDURE [ZGWOptional].[Set_State]
	@P_State CHAR(2),
	@P_Description VARCHAR(128),
	@P_Status_SeqID INT,
	@P_Updated_By INT,
	@P_Primary_Key CHAR(2) OUTPUT,
	@P_Debug INT = 0
AS
BEGIN
	DECLARE @V_Now DATETIME = GETDATE()
	UPDATE
		ZGWOptional.States
	SET 
		[State] = @P_State,
		[Description] = @P_Description,
		Status_SeqID = @P_Status_SeqID,
		Updated_By = @P_Updated_By,
		Updated_Date = @V_Now
	WHERE
		[State] = @P_State

	SELECT @P_Primary_Key = @P_State
END

RETURN 0

GO

