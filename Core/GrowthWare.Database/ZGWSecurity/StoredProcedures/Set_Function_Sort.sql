
/*
Usage:
	DECLARE 
		@P_FunctionSeqId INT = 1,
		@P_Direction INT = 1,
		@P_Added_Updated_By INT = 2,
		@P_Primary_Key int,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Function_Sort
		@P_FunctionSeqId,
		@P_Direction,
		@P_Added_Updated_By,
		@P_Primary_Key,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/02/2011
-- Description:	Updates ZGWSecurity.Functions Sort_Order column
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Function_Sort]
	@P_FunctionSeqId INT,
	@P_Direction INT,
	@P_Added_Updated_By INT,
	@P_Primary_Key INT OUTPUT,
	@P_Debug INT = 0
AS
	DECLARE @V_Current_Sort_Order int
			,@V_Sort_Order_Move int
			,@V_Parent_SeqID INT
			,@V_Updated_Date DATETIME = GETDATE()
	-- Get the parent ID so only the menu items here can be effected
	SET @V_Parent_SeqID = (SELECT Parent_SeqID FROM ZGWSecurity.Functions WHERE FunctionSeqId = @P_FunctionSeqId)
	-- Get Current Sort Order
	SELECT 
		@V_Current_Sort_Order = Sort_Order
	FROM ZGWSecurity.Functions
	WHERE FunctionSeqId = @P_FunctionSeqId
	
	-- Get Sort Order for Section Above
	IF @P_Direction = 0 -- Down
		BEGIN
			SELECT @V_Sort_Order_Move = MIN( Sort_Order )
			FROM ZGWSecurity.Functions
			WHERE Sort_Order > @V_Current_Sort_Order
		END
	ELSE -- up
		BEGIN
			SELECT @V_Sort_Order_Move = MAX( Sort_Order )
			FROM ZGWSecurity.Functions
			WHERE Sort_Order < @V_Current_Sort_Order
		END
	-- END IF
	-- If no row to move, exit
	IF @V_Sort_Order_Move IS NULL
		return
	
	-- Otherwise, switch sort orders
	UPDATE ZGWSecurity.Functions SET
	  Sort_Order = @V_Current_Sort_Order
	  WHERE Sort_Order = @V_Sort_Order_Move
	
	UPDATE ZGWSecurity.Functions SET
	  Sort_Order = @V_Sort_Order_Move,
	  Updated_By = @P_Added_Updated_By,
	  Updated_Date = @V_Updated_Date
	  WHERE FunctionSeqId = @P_FunctionSeqId

RETURN 0

GO

