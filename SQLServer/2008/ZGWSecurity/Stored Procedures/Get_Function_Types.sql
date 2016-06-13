/*
Usage:
	DECLARE 
		@P_Function_SeqID INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Function_Types
		@P_Function_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/16/2011
-- Description:	Returns all function types
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Function_Types]
	@P_Function_Type_SeqID int = -1,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Function_Types'
	IF @P_Function_Type_SeqID = -1
		BEGIN
			IF @P_Debug = -1 PRINT 'Seleting all Function_Types'
			SELECT
				Function_Type_SeqID as FUNCTION_TYPE_SEQ_ID
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
				Function_Type_SeqID as FUNCTION_TYPE_SEQ_ID
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
				Function_Type_SeqID = @P_Function_Type_SeqID
		END
	--END IF
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Function_Types'
RETURN 0