
/*
Usage:
	DECLARE 
		@P_FunctionSeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Function_Types
		@P_FunctionSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/16/2011
-- Description:	Returns all function types
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Function_Types]
	@P_FunctionTypeSeqId int = -1,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Function_Types'
	IF @P_FunctionTypeSeqId = -1
		BEGIN
	IF @P_Debug = -1 PRINT 'Seleting all Function_Types'
	SELECT
		FunctionTypeSeqId as FUNCTION_TYPE_SEQ_ID
				, Name
				, [Description]
				, Template
				, Is_Content
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM
		ZGWSecurity.Function_Types WITH(NOLOCK)
	ORDER BY
				[Name]
END
	ELSE
		BEGIN
	IF @P_Debug = 1 PRINT 'Seleting single Function_Type'
	SELECT
		FunctionTypeSeqId as FUNCTION_TYPE_SEQ_ID
				, Name
				, [Description]
				, Template
				, Is_Content
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM
		ZGWSecurity.Function_Types WITH(NOLOCK)
	WHERE
				FunctionTypeSeqId = @P_FunctionTypeSeqId
END
	--END IF
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Function_Types'
RETURN 0

GO

