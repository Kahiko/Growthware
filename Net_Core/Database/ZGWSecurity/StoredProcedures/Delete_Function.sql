
/*
Usage:
	DECLARE 
		@P_FunctionSeqId int = 4,
		@P_ErrorCode int

	exec ZGWSecurity.Delete_Function
		@P_FunctionSeqId ,
		@P_ErrorCode
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from ZGWSecurity.Functions
--	given the FunctionSeqId
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Function]
	@P_FunctionSeqId int,
	@P_ErrorCode int OUTPUT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Delete_Function'
	-- DELETE an existing row from the table.
	DELETE FROM ZGWSecurity.Functions WHERE	FunctionSeqId = @P_FunctionSeqId
	-- Get the Error Code for the statement just executed.
	SELECT @P_ErrorCode=@@ERROR
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Function'
	RETURN 0

GO

