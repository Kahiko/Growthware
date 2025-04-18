
/*
Usage:
	DECLARE 
		@P_FunctionSeqId INT = 1,
		@P_Directory VARCHAR(255) = '',
		@P_Impersonate INT = 0,
		@P_Impersonating_Account VARCHAR(50) = '',
		@P_Impersonating_Password VARCHAR(50) = '',
		@P_Added_Updated_By INT = 1,
		@P_Primary_Key INT,
		@P_Debug INT = 0

	exec ZGWOptional.Set_Directory
		@P_FunctionSeqId,
		@P_Directory,
		@P_Impersonate,
		@P_Impersonating_Account,
		@P_Impersonating_Password,
		@P_Added_Updated_By,
		@P_Primary_Key OUT,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/25/2011
-- Description:	Inserts or updates ZGWOptional.Directories
-- =============================================
CREATE PROCEDURE [ZGWOptional].[Set_Directory]
	@P_FunctionSeqId INT,
	@P_Directory VARCHAR(255),
	@P_Impersonate INT,
	@P_Impersonating_Account VARCHAR(50),
	@P_Impersonating_Password VARCHAR(50),
	@P_Added_Updated_By INT,
	@P_Primary_Key INT OUTPUT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting ZGWOptional.Set_Directory'
	DECLARE @V_Now DATETIME = GETDATE()
	IF (SELECT COUNT(*)
FROM ZGWOptional.Directories
WHERE FunctionSeqId = @P_FunctionSeqId) = 0
		BEGIN
	IF @P_Debug = 1 PRINT 'Insert Row'
	INSERT ZGWOptional.Directories
		(
		FunctionSeqId,
		Directory,
		Impersonate,
		Impersonating_Account,
		Impersonating_Password,
		Added_By,
		Added_Date
		)
	VALUES
		(
			@P_FunctionSeqId,
			@P_Directory,
			@P_Impersonate,
			@P_Impersonating_Account,
			@P_Impersonating_Password,
			@P_Added_Updated_By,
			@V_Now
			)

	SELECT @P_Primary_Key = @P_FunctionSeqId
END
	ELSE
		BEGIN
	IF @P_Debug = 1 PRINT 'Update Row'
	UPDATE ZGWOptional.Directories
			SET 
				FunctionSeqId = @P_FunctionSeqId,
				Directory = @P_Directory,
				Impersonate = @P_Impersonate,
				Impersonating_Account = @P_Impersonating_Account,
				Impersonating_Password = @P_Impersonating_Password,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = @V_Now
			WHERE
				FunctionSeqId = @P_FunctionSeqId

	SELECT @P_Primary_Key = @P_FunctionSeqId
END
	--end if
	IF @P_Debug = 1 PRINT 'Ending Optional.Set_Directory'
RETURN 0

GO

