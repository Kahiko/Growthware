
/*
Usage:
	DECLARE
		@P_NVPSeqId INT = 1,
		@P_NVP_DetailSeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSystem.Get_Name_Value_Pair_Detail
		@P_NVPSeqId,
		@P_NVP_DetailSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/11/2011
-- Description:	Returns name value pair detail
-- Note:
--	This not the most effecient however this should
--	not be called very often ... it is intended for the
--	front end to cache the information and only get called
--	when needed
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Get_Name_Value_Pair_Detail]
	@P_NVPSeqId INT,
	@P_NVP_DetailSeqId INT,
	@P_Debug INT = 0
AS
	DECLARE @V_TableName VARCHAR(30)
	DECLARE @V_Statement nvarchar(4000)
	SET @V_TableName = (
		SELECT [Schema_Name] + '.' + Static_Name
		FROM ZGWSystem.Name_Value_Pairs
		WHERE NVPSeqId = @P_NVPSeqId
	)
	SET @V_Statement = '
SELECT 
	  [NVP_DetailSeqId] AS [NVP_SEQ_DET_ID]
	, [NVPSeqId] as NVP_SEQ_ID
	, [NVP_Detail_Name] AS [NVP_DET_TEXT]
	, [NVP_Detail_Value] AS [NVP_DET_VALUE]
	, [StatusSeqId] AS [STATUS_SEQ_ID]
	, [Sort_Order] AS [SORT_ORDER]
	, [Added_By]
	, [Added_Date]
	, [Updated_By]
	, [Updated_Date] 
FROM ' + CONVERT(VARCHAR,@V_TableName) + '
WHERE
	NVP_DetailSeqId = ' + CONVERT(VARCHAR,@P_NVP_DetailSeqId)
	IF @P_Debug = 1 PRINT @V_Statement;
	EXECUTE dbo.sp_executesql @statement = @V_Statement
RETURN 0

GO

