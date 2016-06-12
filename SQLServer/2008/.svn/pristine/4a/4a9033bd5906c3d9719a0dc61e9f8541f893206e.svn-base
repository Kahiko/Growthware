/*
Usage:
	exec ZGWSystem.Get_Database_Information
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description:	Retrievs the database information from
--	ZGWSystem.Database_Information
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Get_Database_Information]
AS
	SET NOCOUNT ON
	SELECT TOP 1
		Database_Information_SeqID as Information_SEQ_ID
		, [Version]
		, Enable_Inheritance
		, Added_By
		, Added_Date
		, Updated_By
		, Updated_Date
	FROM
		ZGWSystem.Database_Information WITH(NOLOCK)
	ORDER BY
		Updated_Date DESC
RETURN 0