
/*
Usage:
	DECLARE 
		@P_TableOrView nvarchar(50) = '[ZGWSystem].[vwSearchFunctions]',              
		@P_SelectedPage int = 1,
		@P_PageSize int = 10,
		@P_Columns nvarchar(512) = 'FunctionSeqId, Name, Description, Action, Added_By, Added_Date, Updated_By, Updated_Date',
		@P_OrderByClause nvarchar(1024) = 'Action ASC',
		@P_WhereClause nvarchar(1024),
		@P_Debug BIT = 1

	exec ZGWSystem.Get_Paginated_Data
		@P_TableOrView,              
		@P_SelectedPage,
		@P_PageSize,
		@P_Columns,
		@P_OrderByClause,
		@P_WhereClause,
		@P_Debug
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
			@ParmDefinition NVARCHAR(500) = '',
			@SqlQuery nvarchar(4000) = '',
			@ReturnCount INT, 
			@TotalPages int, 
			@TotalRecords int,
			@LastPageReturn INT,
			@Con_OrderByClause VARCHAR(1024) = '',
			@EndOfOrderByClause VARCHAR(4) = '',
			@SplitId INT,
			@SplitData VARCHAR(50) = '';

	SET @P_WhereClause = ISNULL(@P_WhereClause,'1 = 1')
	IF @P_SelectedPage = 0 SET @P_SelectedPage = 1
 
	IF @P_WhereClause <> ''
	  BEGIN
		SET @P_WhereClause = ' WHERE ' + @P_WhereClause
	  END

	-- So we need to split @P_OrderByClause and create the oposite direction for each
	-- column specified.
	DECLARE @V_TblSplitValues TABLE (
		  [Id] INT
		, [Data] VARCHAR(50)
		, [Processed] BIT
	);
	INSERT @V_TblSplitValues SELECT [Id], [Data], 0 FROM [ZGWSystem].[udfSplit](@P_OrderByClause, ',');
	IF @P_Debug = 1 PRINT  @P_OrderByClause;
	--SELECT * FROM @V_TblSplitValues
	WHILE EXISTS (SELECT TOP(1) 1 FROM @V_TblSplitValues WHERE [Processed] = 0)
		BEGIN
			SELECT TOP(1) @SplitId = [Id], @SplitData = [Data] FROM @V_TblSplitValues WHERE [Processed] = 0;
			-- Check to see if @SplitData ends in "asc"
			-- if it does then replase asc with "desc" else replase desc with "asc"
			SET @SplitData = LTRIM(RTRIM(@SplitData));
			--PRINT @SplitData;
			SET @EndOfOrderByClause = UPPER(SUBSTRING(@SplitData, LEN(@SplitData) - 3, LEN(@SplitData)));
			--PRINT @EndOfOrderByClause;
			IF @EndOfOrderByClause = ' ASC'
				BEGIN
					--PRINT 'Ends in ASC';
					SET @SplitData = SUBSTRING(@SplitData, 0, LEN(@SplitData) -3) + ' DESC'
				END
			ELSE
				BEGIN
					--PRINT 'Ends in DESC';
					SET @SplitData = SUBSTRING(@SplitData, 0, LEN(@SplitData) -3) + 'ASC'
				END
			--END IF
			SET @Con_OrderByClause = @Con_OrderByClause + ', ' + @SplitData;
			UPDATE @V_TblSplitValues SET [Processed] = 1 WHERE [Id] = @SplitId;
		END
	--END WHILE
	SET @Con_OrderByClause = SUBSTRING(@Con_OrderByClause, 3, LEN(@Con_OrderByClause));
	IF @P_Debug = 1 PRINT '@Con_OrderByClause:'
	IF @P_Debug = 1 PRINT @Con_OrderByClause;

	SET @ReturnedRecords = (@P_PageSize * @P_SelectedPage)
	-- Get the total number of rows that can be returned
	SET @SqlQuery = N'SELECT @CountOUT = COUNT(*) FROM @TableOrView @WhereClause'
	SET @ParmDefinition = N'@CountOUT INT OUTPUT'
	SET @SqlQuery = REPLACE(@SqlQuery , '@WhereClause' , @P_WhereClause )
	SET @SqlQuery = REPLACE(@SqlQuery , '@TableOrView' , @P_TableOrView )
	
	IF @P_Debug = 1 PRINT @SqlQuery
	-- Get the requested data
	EXECUTE sp_executesql
		@SqlQuery,
		@ParmDefinition,
		@CountOUT=@ReturnCount OUTPUT

	--PRINT @ReturnCount
	SET @TotalRecords = @ReturnCount
	-- Finds number of pages
	SET @ReturnedRecords = (@P_PageSize * @P_SelectedPage)
	SET @TotalPages = @ReturnCount / @P_PageSize
	IF @TotalRecords % @P_PageSize > 0
	  BEGIN
		SET @TotalPages = @TotalPages + 1
	  END
	--PRINT '@TotalPages: ' + CONVERT(VARCHAR(20),@TotalPages)
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
			  ORDER BY ' + @Con_OrderByClause + ') AS T2
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
				+ ') AS T1 ORDER BY ' + @Con_OrderByClause
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

