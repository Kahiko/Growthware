
/*
Usage:
	DECLARE 
		@P_NVP_Detail_SeqID INT = 4,
		@P_NVP_SeqID int = 1,
		@P_Debug INT = 0

	exec ZGWSystem.Delete_Name_Value_Pair_Detail
		@P_NVP_Detail_SeqID,
		@P_NVP_SeqID,
		@P_Debug BIT
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/04/2011
-- Description:	Deletes a records from xx where xx is the static_name column
--	from ZGWSystem.Name_Value_Pairs given the NVP_Detail_SeqID and NVP_SeqID
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Delete_Name_Value_Pair_Detail]
	@P_NVP_Detail_SeqID INT,
	@P_NVP_SeqID int,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Delete_Name_Value_Pair'
	DECLARE @V_Statement NVARCHAR(4000),
			@V_Static_Name VARCHAR(30)

	SET @V_Static_Name = (SELECT Static_Name FROM ZGWSystem.Name_Value_Pairs WHERE NVP_SeqID = @P_NVP_SeqID)

	SET @V_Statement= 'DELETE 
		   FROM ' + CONVERT(VARCHAR,@V_Static_Name) + '
		   WHERE NVP_Detail_SeqID= ''' + CONVERT(VARCHAR,@P_NVP_Detail_SeqID) + ''''
	EXECUTE sp_executesql @V_Statement
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Name_Value_Pair'
RETURN 0

GO

