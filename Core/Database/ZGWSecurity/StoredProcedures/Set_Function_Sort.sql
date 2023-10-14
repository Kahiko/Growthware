-- =============================================
-- Author:		Michael Regan
-- Create date: 09/02/2011
-- Description:	Updates ZGWSecurity.Functions Sort_Order column
-- =============================================
-- Author:		Michael Regan
-- Create date: 10/04/2023
-- Description:	Changed to accept all functions at once in order to support a drag and drop frontend
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Function_Sort]
	@P_Commaseparated_Ids VARCHAR(50),
	@P_Added_Updated_By INT,
	@P_Primary_Key INT OUTPUT,
	@P_Debug INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @V_TblSplitValues TABLE (
		[Id] INT
		,[Data] VARCHAR(50)
		,[Processed] BIT
	);
	DECLARE  @V_FunctionSeqId INT
			,@V_Id int;

	INSERT INTO @V_TblSplitValues SELECT [Id], [Data], 0  FROM [ZGWSecurity].[udfSplit](@P_Commaseparated_Ids, ',');
	--SELECT * FROM @V_TblSplitValues;
	WHILE (SELECT COUNT(*) FROM @V_TblSplitValues WHERE Processed = 0) > 0
		BEGIN
			SELECT TOP(1)
				@V_Id = [Id]
				, @V_FunctionSeqId = CONVERT(INT, [Data])
			FROM @V_TblSplitValues WHERE [Processed] = 0 ORDER BY [Id];
			-- PRINT 'FunctionSeqId ' + CONVERT(VARCHAR(10), @V_FunctionSeqId) + ' Sort = ' + CONVERT(VARCHAR(10), @V_Id - 1);
			UPDATE [ZGWSecurity].[Functions] SET [Sort_Order] = @V_Id - 1 WHERE [FunctionSeqId] = @V_FunctionSeqId;
			UPDATE @V_TblSplitValues SET [Processed] = 1 WHERE [Id] = @V_Id;
		END
	--END WHILE
END

RETURN 0

GO

