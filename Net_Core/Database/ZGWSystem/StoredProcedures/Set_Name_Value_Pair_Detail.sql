
/*
Usage:
	DECLARE 
		@V_NVP_DetailSeqId INT = -1,
		@V_NVPSeqId int = 5 ,
		@V_NVP_Detail_Name VARCHAR(50) = '7',
		@V_NVP_Detail_Value VARCHAR(300) = '7',
		@V_StatusSeqId INT = 1,
		@V_Sort_Order INT = 1,
		@V_Added_Updated_BY INT = 1,
		@V_ErrorCode int = null,
		@V_Debug bit = 1

	exec ZGWSystem.Set_Name_Value_Pair_Detail
		@V_NVP_DetailSeqId,
		@V_NVPSeqId,
		@V_NVP_Detail_Name,
		@V_NVP_Detail_Value,
		@V_StatusSeqId,
		@V_Sort_Order,
		@V_Added_Updated_BY,
		@V_ErrorCode,
		@V_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates ZGWCoreWeb.Account_Choices based on @P_Account
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Changed to return a data row after the update/insert
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Set_Name_Value_Pair_Detail]
	@P_NVP_DetailSeqId INT,
	@P_NVPSeqId int,
	@P_NVP_Detail_Name VARCHAR(50),
	@P_NVP_Detail_Value VARCHAR(300),
	@P_StatusSeqId INT,
	@P_Sort_Order INT,
	@P_Added_Updated_By INT,
	@P_ErrorCode int OUTPUT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting [ZGWSystem].[Set_Name_Value_Pair_Detail]'
	DECLARE 
		@V_Static_Name VARCHAR(30) = (
			SELECT Static_Name
			FROM ZGWSystem.Name_Value_Pairs
			WHERE NVPSeqId = @P_NVPSeqId
		)
		,@V_Schema_Name VARCHAR(30) = (
			SELECT [Schema_Name]
			FROM ZGWSystem.Name_Value_Pairs
			WHERE NVPSeqId = @P_NVPSeqId
		)
		,@V_Statement NVARCHAR(4000)
		,@V_Now DATETIME = GETDATE()

	IF @P_NVP_DetailSeqId > -1
		BEGIN
			-- UPDATE PROFILE
			SET @V_Statement = '
UPDATE ' + CONVERT(VARCHAR,@V_Schema_Name) + '.' + CONVERT(VARCHAR,@V_Static_Name) + '
SET 
	NVP_Detail_Name = ''' + CONVERT(VARCHAR,@P_NVP_Detail_Name) + ''',
	NVP_Detail_Value = ''' + CONVERT(VARCHAR,@P_NVP_Detail_Value) + ''',
	StatusSeqId = ' + CONVERT(VARCHAR,@P_StatusSeqId) + ',
	Sort_Order = ' + CONVERT(VARCHAR,@P_Sort_Order) + ',
	Updated_By = ' + CONVERT(VARCHAR,@P_Added_Updated_By) + ',
	UPDATED_DATE = ''' + CONVERT(VARCHAR,@V_Now) + '''
WHERE
	NVP_DetailSeqId = ' + CONVERT(VARCHAR,@P_NVP_DetailSeqId);
			IF @P_Debug = 1 PRINT @V_Statement
			EXECUTE dbo.sp_executesql @statement = @V_Statement
		END
	ELSE
		BEGIN
			-- INSERT a new row in the table.
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
			-- END IF
			SET @V_Statement = '
INSERT INTO ' + CONVERT(VARCHAR,@V_Schema_Name) + '.' + CONVERT(VARCHAR,@V_Static_Name) + '(
	NVPSeqId,
	NVP_Detail_Name,
	NVP_Detail_Value,
	StatusSeqId,
	Sort_Order,
	Added_By,
	ADDED_DATE
) VALUES (
	' + CONVERT(VARCHAR,@P_NVPSeqId) + ',
	''' + CONVERT(VARCHAR,@P_NVP_Detail_Name) + ''',
	''' + CONVERT(VARCHAR,@P_NVP_Detail_Value) + ''',
	' + CONVERT(VARCHAR,@P_StatusSeqId) + ',
	' + CONVERT(VARCHAR,@P_Sort_Order) + ',
	' + CONVERT(VARCHAR,@P_Added_Updated_By) + ',
	''' + CONVERT(VARCHAR,@V_Now) + '''
)';
			IF @P_Debug = 1 PRINT @V_Statement
			-- EXECUTE dbo.sp_executesql @statement = @V_Statement
			-- Get the IDENTITY value for the row just inserted.
			EXECUTE sp_executesql  @V_Statement, N'@P_NVP_DetailSeqId INTEGER OUTPUT', @P_NVP_DetailSeqId OUTPUT
		END
	-- Get the Error Code for the statement just executed.
	--PRINT 'SETTING ERROR CODE'
	SELECT @P_ErrorCode=@@ERROR
	EXEC ZGWSystem.Get_Name_Value_Pair_Detail @P_NVPSeqId, @P_NVP_DetailSeqId, @P_Debug

RETURN 0

GO
