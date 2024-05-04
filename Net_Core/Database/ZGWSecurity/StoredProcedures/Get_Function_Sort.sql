
/*
Usage:
	DECLARE 
		@P_FunctionSeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Function_Sort
		@P_FunctionSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/15/2011
-- Description:	Returns sorted function information
--	for related functions given the funtionSeqId
-- =============================================
-- Author:		Michael Regan
-- Create date: 05/03/2024
-- Description:	Fixed but where nothing is returned if the ParentSeqId <> 1
--  this should have been ParentSeqId = @V_Parent_ID
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Function_Sort]
	@P_FunctionSeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON;

	DECLARE @V_Parent_ID INT
			, @V_NAV_TYPE_ID INT;

	SET @V_Parent_ID = (
			SELECT ParentSeqId
			FROM ZGWSecurity.Functions
			WHERE FunctionSeqId = @P_FunctionSeqId
		);
	SET @V_NAV_TYPE_ID = (
			SELECT Navigation_Types_NVP_DetailSeqId
			FROM ZGWSecurity.Functions
			WHERE FunctionSeqId = @P_FunctionSeqId
		);
	SELECT 
		  FunctionSeqId AS FUNCTION_SEQ_ID
		, [Name]
		, [Action]
		, Sort_Order
	FROM ZGWSecurity.Functions WITH (NOLOCK)
	WHERE ParentSeqId = @V_PARENT_ID
		AND Is_Nav = 1
		AND Navigation_Types_NVP_DetailSeqId = @V_NAV_TYPE_ID
		AND ParentSeqId = @V_Parent_ID
	ORDER BY Sort_Order ASC;

SET NOCOUNT OFF;

GO
