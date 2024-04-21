
/*
Usage:
	DECLARE 
		@P_State AS varchar(2) = 'ca',
		@P_Debug INT = 1

	exec ZGWOptional.Get_State
		@P_State,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/23/2011
-- Description:	Retrieves State details
--	given the state
-- Note:
--	SeqID value of -1 will return all
--	security enties.
-- =============================================
CREATE PROCEDURE [ZGWOptional].[Get_State]
	@P_State CHAR(2),
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWOptional.Get_State'
	IF @P_State <> '-1'
		BEGIN
	IF @P_Debug = 1 PRINT 'Getting a single record'
	SELECT
		[State]
				, [Description]
				, StatusSeqId
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM
		ZGWOptional.States
	WHERE [State] = @P_State
END
	ELSE
		BEGIN
	IF @P_Debug = 1 PRINT 'Getting all single records'
	SELECT
		[State]
				, [Description]
				, StatusSeqId
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM
		ZGWOptional.States
	ORDER BY
				[State]
END
	--END IF
	IF @P_Debug = 1 PRINT 'Ending ZGWOptional.Get_State'
RETURN 0

GO

