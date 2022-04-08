/*
Usage:
	DECLARE @V_Permission_ID INT
	SET @V_Permission_ID = ZGWSecurity.Get_View_Permission_SeqID()
	PRINT @V_Permission_ID
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description: Returns NVP_Detail_SeqID associated
--	with the NVP_Detail_Name value = 'view'
-- Note:
--	Created to allow change in a single location
--	should the sequence id change.
-- =============================================
CREATE FUNCTION [ZGWSecurity].[Get_View_Permission_SeqID]
(

)
RETURNS INT
AS
BEGIN
	RETURN 1
END

GO

