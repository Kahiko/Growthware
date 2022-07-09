
/*
Usage:
	DECLARE 
		@P_TableOrView nvarchar(50) = 'ZGWSecurity.Functions',              
		@P_SelectedPage int = 1,
		@P_PageSize int = 10,
		@P_Columns nvarchar(512) = 'FunctionSeqId, Name, Description, Action, Added_By, Added_Date, Updated_By, Updated_Date',
		@P_OrderByClause nvarchar(1024) = 'Action ASC',
		@P_WhereClause nvarchar(1024)

	exec ZGWSystem.Get_Paginated_Data
		@P_TableOrView,              
		@P_SelectedPage,
		@P_PageSize,
		@P_Columns,
		@P_OrderByClause,
		@P_WhereClause
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/02/2012
-- Description:	Gets paginated data
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Get_Paginated_Data]
	@P_TableOrView nvarchar (50),              
	@P_SelectedPage int,
	@P_PageSize int,
	@P_Columns nvarchar(512),
	@P_OrderByClause nvarchar(1024),
	@P_WhereClause nvarchar(1024),
	@P_Debug bit = 0
AS
	DECLARE @ReturnedRecords int, 
			@ParmDefinition NVARCHAR(500),
			@SqlQuery nvarchar(4000),
			@ReturnCount INT, 
			@TotalPages int, 
			@TotalRecords int,
			@LastPageReturn INT

	SET @P_WhereClause = ISNULL(@P_WhereClause,'1 = 1')
	IF @P_SelectedPage = 0 SET @P_SelectedPage = 1
 
	IF @P_WhereClause <> ''
	  BEGIN
		SET @P_WhereClause = ' WHERE ' + @P_WhereClause
	  END

	SET @ReturnedRecords = (@P_PageSize * @P_SelectedPage)
	-- Get the total number of rows that can be returned
	SET @SqlQuery = N'SELECT @CountOUT = COUNT(*) FROM @TableOrView @WhereClause'
	SET @ParmDefinition = N'@CountOUT INT OUTPUT'
	SET @SqlQuery = REPLACE(@SqlQuery , '@WhereClause' , @P_WhereClause )
	SET @SqlQuery = REPLACE(@SqlQuery , '@TableOrView' , @P_TableOrView )
	
	PRINT @SqlQuery
	-- Get the requested data
	EXECUTE sp_executesql
		@SqlQuery,
		@ParmDefinition,
		@CountOUT=@ReturnCount OUTPUT

	PRINT @ReturnCount
	SET @TotalRecords = @ReturnCount
	-- Finds number of pages
	SET @ReturnedRecords = (@P_PageSize * @P_SelectedPage)
	SET @TotalPages = @ReturnCount / @P_PageSize
	IF @TotalRecords % @P_PageSize > 0
	  BEGIN
		SET @TotalPages = @TotalPages + 1
	  END
	PRINT '@TotalPages: ' + CONVERT(VARCHAR(20),@TotalPages)
	--SELECT @ReturnCount as TotalRecords
	
	SET @ParmDefinition = N'@ReturnCount INT'

	SET NOCOUNT ON

	-- Checks if current page is last page
	IF @P_SelectedPage != @TotalPages
		BEGIN -- Current page is not last page
			IF @P_Debug = 1 PRINT 'Current page is not last page'
			SET @SqlQuery = N'SELECT @ReturnCount as TotalRecords, * FROM
			(SELECT TOP ' + CAST(@P_PageSize as varchar(10)) + ' *  FROM
			  (SELECT TOP ' + CAST(@ReturnedRecords as varchar(10)) + ' ' + @P_Columns +
				' FROM ' + @P_TableOrView + @P_WhereClause + '
				ORDER BY ' + @P_OrderByClause + ') AS T1
			  ORDER BY ' + @P_OrderByClause + ') AS T2
			ORDER BY ' + @P_OrderByClause
		END
	ELSE
		BEGIN -- Current page is last page
			IF @P_Debug = 1 PRINT 'Current page is last page'
			IF (@ReturnCount % @P_PageSize) = 0 
				BEGIN
					SET @LastPageReturn = @P_PageSize
				END
			ELSE
				BEGIN
					SET @LastPageReturn = @ReturnCount % @P_PageSize
				END
			--END IF
			SET @SqlQuery = N'SELECT @ReturnCount as TotalRecords, * FROM (SELECT TOP (' + CAST((@LastPageReturn) as varchar(10)) + ')'
				+ ' *  FROM (SELECT TOP ' + CAST(@ReturnedRecords as varchar(10)) + ' ' + @P_Columns
				+ ' FROM ' + @P_TableOrView + @P_WhereClause 
				+ ' ORDER BY ' + @P_OrderByClause
				+ ') AS T1 ORDER BY ' + @P_OrderByClause
				+ ') AS T2 ORDER BY ' + @P_OrderByClause
		END
	--END IF
	 
	IF @P_Debug = 1 PRINT @SqlQuery

	EXECUTE sp_executesql
		@SqlQuery,
		@ParmDefinition,
		@ReturnCount
	SET NOCOUNT OFF

RETURN 0

GO

