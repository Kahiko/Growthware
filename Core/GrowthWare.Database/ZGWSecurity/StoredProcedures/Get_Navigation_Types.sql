
/*
Usage:
	exec ZGWSecurity.Get_Navigation_Types
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns all navigation types
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Navigation_Types]
AS
	SET NOCOUNT ON
	SELECT
	NVP_DetailSeqId AS NVP_SEQ_DET_ID
		, NVPSeqId AS NVP_SEQ_ID
		, NVP_Detail_Name AS NVP_DET_VALUE
		, NVP_Detail_Value AS NVP_DET_TEXT
		, StatusSeqId AS STATUS_SEQ_ID
		, Sort_Order
		, Added_By
		, Added_Date
		, Updated_By
		, Updated_Date
FROM
	ZGWSecurity.Navigation_Types
ORDER BY
		NVP_Detail_Name

RETURN 0

GO

