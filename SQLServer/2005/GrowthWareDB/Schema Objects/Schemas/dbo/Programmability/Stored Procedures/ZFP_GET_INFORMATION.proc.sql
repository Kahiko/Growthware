-- =============================================
-- Author:		Michael Regan
-- Create date: 7/19/2008
-- Description:	Retrieves the information from ZFC_INFORMATION -- there should only be 1 row.
-- =============================================
CREATE PROCEDURE [ZFP_GET_INFORMATION] 
	@P_ErrorCode int OUTPUT	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[Information_SEQ_ID],
		[Version],
		[Enable_Inheritance],
		[ADDED_BY],
		[ADDED_DATE],
		[UPDATED_BY],
		[UPDATED_DATE]
	FROM
		ZFC_INFORMATION
END
