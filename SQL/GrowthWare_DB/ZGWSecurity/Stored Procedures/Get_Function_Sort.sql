/*
Usage:
	DECLARE 
		@P_Function_SeqID INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Function_Sort
		@P_Function_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/15/2011
-- Description:	Returns sorted function information
--	for related functions given the funtion_seqid
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Function_Sort]
	@P_Function_SeqID INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	DECLARE @V_Parent_ID INT
	DECLARE @V_NAV_TYPE_ID INT
	SET @V_Parent_ID = (SELECT Parent_SeqID FROM ZGWSecurity.Functions WHERE Function_SeqID = @P_Function_SeqID)
	SET @V_NAV_TYPE_ID = (SELECT Navigation_Types_NVP_Detail_SeqID FROM ZGWSecurity.Functions WHERE Function_SeqID = @P_Function_SeqID)
	SELECT
		Function_SeqID as FUNCTION_SEQ_ID,
		[Name],
		[Action],
		Sort_Order
	FROM
		ZGWSecurity.Functions WITH(NOLOCK)
	WHERE
		Parent_SeqID = @V_PARENT_ID
		AND Is_Nav = 1
		AND Navigation_Types_NVP_Detail_SeqID = @V_NAV_TYPE_ID
		AND Parent_SeqID <> 1
	ORDER BY
		Sort_Order ASC

RETURN 0