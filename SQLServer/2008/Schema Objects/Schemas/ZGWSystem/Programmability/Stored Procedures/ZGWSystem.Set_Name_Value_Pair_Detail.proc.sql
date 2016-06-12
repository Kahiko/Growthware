/*
Usage:
	DECLARE 
		@V_NVP_Detail_SeqID INT = -1,
		@V_NVP_SeqID int = (SELECT NVP_SeqID FROM ZGWSystem.Name_Value_Pairs WHERE STATIC_NAME = 'Navigation_Types') ,
		@V_NVP_Detail_Name VARCHAR(50) = 'Test',
		@V_NVP_Detail_Value VARCHAR(300) = 'Test value',
		@V_Status_SeqID INT = 1,
		@V_Sort_Order INT = 1,
		@V_Added_Updated_BY INT = 1,
		@V_Primary_Key INT = null,
		@V_ErrorCode int = null,
		@V_Debug bit = 1

	exec ZGWSystem.Set_Name_Value_Pair_Detail
		@V_NVP_Detail_SeqID,
		@V_NVP_SeqID,
		@V_NVP_Detail_Name,
		@V_NVP_Detail_Value,
		@V_Status_SeqID,
		@V_Sort_Order,
		@V_Added_Updated_BY,
		@V_Primary_Key,
		@V_ErrorCode,
		@V_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates ZGWCoreWeb.Account_Choices based on @P_Account
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Set_Name_Value_Pair_Detail]
	@P_NVP_Detail_SeqID INT,
	@P_NVP_SeqID int,
	@P_NVP_Detail_Name VARCHAR(50),
	@P_NVP_Detail_Value VARCHAR(300),
	@P_Status_SeqID INT,
	@P_Sort_Order INT,
	@P_Added_Updated_By INT,
	@P_Primary_Key INT OUTPUT,
	@P_ErrorCode int OUTPUT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting [ZGWSystem].[Set_Name_Value_Pair_Detail]'
	DECLARE 
		@V_Static_Name VARCHAR(30) = (SELECT Static_Name FROM ZGWSystem.Name_Value_Pairs WHERE NVP_SeqID = @P_NVP_SeqID)
		,@V_Schema_Name VARCHAR(30) = (SELECT [Schema_Name] FROM ZGWSystem.Name_Value_Pairs WHERE NVP_SeqID = @P_NVP_SeqID)
		,@V_Statement NVARCHAR(4000)
		,@V_Now DATETIME = GETDATE()

	IF @P_NVP_Detail_SeqID > -1
		BEGIN -- UPDATE PROFILE
			SET @V_Statement = 'UPDATE ' + CONVERT(VARCHAR,@V_Schema_Name) + '.' + CONVERT(VARCHAR,@V_Static_Name) + '
			SET 
				NVP_Detail_Name = ''' + CONVERT(VARCHAR,@P_NVP_Detail_Name) + ''',
				NVP_Detail_Value = ''' + CONVERT(VARCHAR,@P_NVP_Detail_Value) + ''',
				Status_SeqID = ' + CONVERT(VARCHAR,@P_Status_SeqID) + ',
				Sort_Order = ' + CONVERT(VARCHAR,@P_Sort_Order) + ',
				UPDATED_BY = ' + CONVERT(VARCHAR,@P_Added_Updated_By) + ',
				UPDATED_DATE = ''' + CONVERT(VARCHAR,@V_Now) + '''
			WHERE
				NVP_Detail_SeqID = ' + CONVERT(VARCHAR,@P_NVP_Detail_SeqID)
			IF @P_Debug = 1 PRINT @V_Statement
			EXECUTE dbo.sp_executesql @statement = @V_Statement
			SELECT @P_Primary_Key = @P_NVP_SeqID
		END
	ELSE
		BEGIN -- INSERT a new row in the table.
			-- CHECK FOR DUPLICATE NAME BEFORE INSERTING
			DECLARE @V_COUNT INT
			SET @V_Statement= 'SET @V_COUNT = (SELECT COUNT(*)
				   FROM ' + CONVERT(VARCHAR,@V_Schema_Name) + '.' + CONVERT(VARCHAR,@V_Static_Name) + '
				   WHERE NVP_Detail_Value = ''' + CONVERT(VARCHAR,@P_NVP_Detail_Value) + ''' AND NVP_Detail_Name = ''' + CONVERT(VARCHAR,@P_NVP_Detail_Name) + ''')'
			IF @P_Debug = 1 PRINT @V_Statement
			EXECUTE sp_executesql @V_Statement,N'@V_COUNT int output',@V_COUNT output
			IF @V_COUNT > 0
				BEGIN
					RAISERROR ('The entry already exists in the database.',16,1)
					RETURN
				END
			SET @V_Statement = 'INSERT INTO ' + CONVERT(VARCHAR,@V_Schema_Name) + '.' + CONVERT(VARCHAR,@V_Static_Name) + '(
					NVP_SeqID,
					NVP_Detail_Name,
					NVP_Detail_Value,
					Status_SeqID,
					Sort_Order,
					Added_By,
					ADDED_DATE
				)
				VALUES
				(
					' + CONVERT(VARCHAR,@P_NVP_SeqID) + ',
					''' + CONVERT(VARCHAR,@P_NVP_Detail_Name) + ''',
					''' + CONVERT(VARCHAR,@P_NVP_Detail_Value) + ''',
					' + CONVERT(VARCHAR,@P_Status_SeqID) + ',
					' + CONVERT(VARCHAR,@P_Sort_Order) + ',
					' + CONVERT(VARCHAR,@P_Added_Updated_By) + ',
					''' + CONVERT(VARCHAR,@V_Now) + '''
				)'
			IF @P_Debug = 1 PRINT @V_Statement
			EXECUTE dbo.sp_executesql @statement = @V_Statement
			SELECT @P_Primary_Key=SCOPE_IDENTITY() -- Get the IDENTITY value for the row just inserted.
			--PRINT 'DONE ADDING'
		END
-- Get the Error Code for the statement just executed.
--PRINT 'SETTING ERROR CODE'
SELECT @P_ErrorCode=@@ERROR
RETURN 0