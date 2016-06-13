/*
Usage:
	DECLARE
		@P_NVP_SeqID int = 1,
		@P_Debug INT = 1

	exec ZGWSystem.Get_Name_Value_Pair_Details
		@P_NVP_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns name value pair details 
-- Note:
--	This not the most effecient however this should
--	not be called very often ... it is intended for the
--	front end to cache the information and only get called
--	when needed
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Get_Name_Value_Pair_Details]
	@P_NVP_SeqID int,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Name_Value_Pair_Details'

	CREATE TABLE #NVP_DETAILS (NVP_Detail_SeqID INT
								, NVP_SeqID INT
								, NVP_Detail_Name VARCHAR(50)
								, NVP_Detail_Value VARCHAR(300)
								, Status_SeqID INT
								, Sort_Order INT
								, Added_By INT
								, Added_Date DATETIME
								, Updated_By INT
								, Updated_Date DATETIME) 
	DECLARE @V_NVP_SeqID INT
			,@V_Static_Name VARCHAR(30)
			,@V_Schema_Name VARCHAR(30)
			,@V_Statement nvarchar(max)
	SET @V_Statement = 'SELECT * FROM '
	DECLARE V_Name_Value_Pairs CURSOR STATIC LOCAL FOR
		SELECT
			NVP_SeqID,
			Static_Name,
			[Schema_Name]
		FROM
			ZGWSystem.Name_Value_Pairs

	OPEN V_Name_Value_Pairs
		FETCH NEXT FROM V_Name_Value_Pairs
		INTO 
			@V_NVP_SeqID,  
			@V_Static_Name,
			@V_Schema_Name
		WHILE (@@FETCH_STATUS = 0)
			BEGIN
				SET @V_Statement =  @V_Statement + CONVERT(VARCHAR,@V_Schema_Name) + '.' + CONVERT(VARCHAR,@V_Static_Name) + ' UNION ALL SELECT * FROM '
				FETCH NEXT FROM V_Name_Value_Pairs INTO @V_NVP_SeqID, @V_Static_Name, @V_Schema_Name
			END
	CLOSE V_Name_Value_Pairs
	DEALLOCATE V_Name_Value_Pairs
	SET @V_Statement = SUBSTRING(@V_Statement, 0, LEN(@V_Statement) - 23)
	IF @P_Debug = 1 PRINT @V_Statement
	INSERT INTO #NVP_DETAILS EXECUTE dbo.sp_executesql @statement = @V_Statement

	IF @P_NVP_SeqID = -1
		SELECT 
			#NVP_DETAILS.NVP_Detail_SeqID as NVP_SEQ_DET_ID,
			#NVP_DETAILS.NVP_SeqID as NVP_SEQ_ID,
			ZGWSystem.Name_Value_Pairs.[Schema_Name] + '.' + ZGWSystem.Name_Value_Pairs.Static_Name as [Table_Name],
			#NVP_DETAILS.NVP_Detail_Name as NVP_DET_VALUE, 
			#NVP_DETAILS.NVP_Detail_Value as NVP_DET_TEXT, 
			#NVP_DETAILS.Status_SeqID as STATUS_SEQ_ID, 
			#NVP_DETAILS.Sort_Order, 
			(SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE Account_SeqID = #NVP_DETAILS.Added_By) AS Added_By,  
			#NVP_DETAILS.Added_Date, 
			(SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE Account_SeqID = #NVP_DETAILS.Updated_By) AS Updated_By,  
			#NVP_DETAILS.Updated_Date 
		FROM 
			#NVP_DETAILS,
			ZGWSystem.Name_Value_Pairs
		WHERE
			#NVP_DETAILS.NVP_SeqID = ZGWSystem.Name_Value_Pairs.NVP_SeqID
		ORDER BY
			ZGWSystem.Name_Value_Pairs.Static_Name,
			#NVP_DETAILS.NVP_Detail_Value
	ELSE
		SELECT 
			#NVP_DETAILS.NVP_Detail_SeqID as NVP_SEQ_DET_ID,
			#NVP_DETAILS.NVP_SeqID as NVP_SEQ_ID,
			ZGWSystem.Name_Value_Pairs.[Schema_Name] + '.' + ZGWSystem.Name_Value_Pairs.Static_Name as [Table_Name],
			#NVP_DETAILS.NVP_Detail_Name as NVP_DET_VALUE, 
			#NVP_DETAILS.NVP_Detail_Value as NVP_DET_TEXT, 
			#NVP_DETAILS.Status_SeqID as STATUS_SEQ_ID, 
			#NVP_DETAILS.Sort_Order, 
			(SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE Account_SeqID = #NVP_DETAILS.Added_By) AS Added_By, 
			#NVP_DETAILS.Added_Date, 
			(SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE Account_SeqID = #NVP_DETAILS.Updated_By) AS Updated_By, 
			#NVP_DETAILS.Updated_Date 
		FROM 
			#NVP_DETAILS,
			ZGWSystem.Name_Value_Pairs
		WHERE
			#NVP_DETAILS.NVP_SeqID = ZGWSystem.Name_Value_Pairs.NVP_SeqID
			AND ZGWSystem.Name_Value_Pairs.NVP_SeqID = @P_NVP_SeqID
		ORDER BY
			ZGWSystem.Name_Value_Pairs.Static_Name,
			#NVP_DETAILS.NVP_Detail_Value

	DROP TABLE #NVP_DETAILS

RETURN 0