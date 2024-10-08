﻿/*
Usage:
	DECLARE
		@P_NVP_SeqID INT = 1,
		@P_NVP_Detail_SeqID INT = 1,
		@P_Debug INT = 1

	exec ZGWSystem.Get_Name_Value_Pair_Detail
		@P_NVP_SeqID,
		@P_NVP_Detail_SeqID,
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
	@P_NVP_SeqID INT,
	@P_NVP_Detail_SeqID INT,
	@P_Debug INT = 0
AS
	DECLARE @V_TableName VARCHAR(30)
	DECLARE @V_Statement nvarchar(4000)
	SET @V_TableName = (SELECT [Schema_Name] + '.' + Static_Name FROM ZGWSystem.Name_Value_Pairs WHERE NVP_SeqID = @P_NVP_SeqID)
	SET @V_Statement = 'SELECT NVP_SeqID as NVP_SEQ_ID, [Schema_Name], Static_Name, Display, Description, Status_SeqID as STATUS_SEQ_ID, Added_By, Added_Date, Updated_By, Updated_Date FROM ' + CONVERT(VARCHAR,@V_TableName) + '
	WHERE
		NVP_Detail_SeqID = ' + CONVERT(VARCHAR,@P_NVP_Detail_SeqID) + ' ORDER BY Static_Name'

	EXECUTE dbo.sp_executesql @statement = @V_Statement
RETURN 0