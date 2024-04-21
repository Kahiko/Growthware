
/*
Usage:
	EXEC [ZGWSystem].[Add_Data_Files] 
		@P_DB_Name = 'tempdb'
		, @P_Debug = 1
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 2017/05/03
-- Description:	Adds X number of data files to
--	a database given the database name.  
--	Based on number of cores:
--		< 8 1 per core
--		between 8 & 32 inclusive then 1/2 the number of cores
--		> 32 then the number of cores / 4
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Add_Data_Files]
	@P_DB_Name SYSNAME -- Name of the database
	,
	@P_Debug BIT = 0
AS
BEGIN
	SET NOCOUNT ON

	IF @P_Debug = 1 PRINT '-- Instance name: ' + @@servername + ' ;
/* 
	Version: ' + @@version + '*/'

	-- Variables

	DECLARE @V_BITS BIGINT						-- Affinty Mask
			, @V_NUMPROCS SMALLINT				-- Number of cores addressed by instance
			, @V_DB_File_Count Int				-- Number of exisiting datafiles
			, @V_DB_Location NVARCHAR(4000)		-- Location of DB primary datafile
			, @V_File_LogicalName NVARCHAR(4000)-- The logical file name
			, @V_FileGrowthType NVARCHAR(400)	-- The type of file growth MB or %
			, @V_FileGrowth NVARCHAR(400)		-- The amount the file grows at
			, @V_Count INT						-- Counter
			, @V_SQLStatement NVARCHAR(max)		-- Dynamic SQL holder
			, @V_NewFile_Size_MB INT			-- Size of the new files,in Megabytes
			, @V_NewFile_Growth_MB INT			-- New files growth rate,in Megabytes
			, @V_NewFileLocation NVARCHAR(4000) -- New files path
			, @V_OS NVARCHAR(256)				-- The OS Type for us Windows or Linux (Ubuntu reported as)
			, @V_DBFileName VARCHAR(MAX);
	-- Used to remove the database filename if needed "Azmuth.mdf"

	-- Initialize variables

	SELECT @V_Count = 1, @V_BITS = 1
			, @V_OS = (SELECT host_platform
		FROM sys.dm_os_host_info)
			, @V_DBFileName = @P_DB_Name + '.mdf';
	SELECT
		@V_NewFile_Size_MB = 4096		-- Four Gbytes, it's easy to increase that after file creation but harder to shrink.
		, @V_NewFile_Growth_MB = 512	-- 512 Mbytes, can be easily shrunk
		, @V_NewFileLocation = NULL;
	-- NULL means create in same location as primary file.

	IF OBJECT_ID('tempdb..#SVer') IS NOT NULL
		BEGIN
		DROP TABLE #SVer;
	END
	--END IF

	CREATE TABLE #SVer
	(
		[ID] INT,
		[Name] sysname,
		[Internal_Value] INT,
		[Value] NVARCHAR(512)
	);

	INSERT #SVer
	EXEC master.dbo.xp_msver ProcessorCount;

	-- Get total number of Cores detected by the Operating system

	SELECT @V_NUMPROCS = Internal_Value
	FROM #SVer;
	IF @P_Debug = 1 PRINT '-- TOTAL numbers of CPU cores on server: ' + cast(@V_NUMPROCS as varchar(5));
	SET @V_NUMPROCS  = 0;

	-- Get number of Cores addressed by instance.

	WHILE @V_Count <= (SELECT Internal_Value
		FROM #SVer ) AND @V_Count <=32
		BEGIN
		SELECT @V_NUMPROCS =
				CASE 
					WHEN  CAST (VALUE AS INT) & @V_BITS > 0 THEN @V_NUMPROCS + 1 
					ELSE @V_NUMPROCS 
				END
		FROM sys.configurations
		WHERE NAME = 'AFFINITY MASK'
		SET  @V_BITS = (@V_BITS * 2)
		SET @V_Count = @V_Count + 1
	END
	--END WHILE

	IF (SELECT Internal_Value
	FROM #SVer) > 32
		BEGIN
		WHILE @V_Count <= (SELECT Internal_Value
		FROM #SVer )
				BEGIN
			SELECT @V_NUMPROCS = 
						CASE 
							WHEN  CAST (VALUE AS INT) & @V_BITS > 0 THEN @V_NUMPROCS + 1 
							ELSE @V_NUMPROCS 
						END
			FROM sys.configurations
			WHERE [NAME] = 'AFFINITY64 MASK';
			SET  @V_BITS = (@V_BITS * 2);
			SET @V_Count = @V_Count + 1;
		END
	--END WHILE
	END
	--END IF

	If @V_NUMPROCS = 0 SELECT @V_NUMPROCS=  Internal_Value
	FROM #SVer;

	IF @P_Debug = 1 PRINT '-- Number of CPU cores Configured for usage by instance: ' + cast(@V_NUMPROCS as varchar(5));

	-------------------------------------------------------------------------------------
	-- Here you define how many files should exist per core ; Feel free to change
	-------------------------------------------------------------------------------------

	-- IF cores < 8 then no change , IF between 8 & 32 inclusive then 1/2 of cores number
	IF @V_NUMPROCS >8 AND @V_NUMPROCS <=32
		SELECT @V_NUMPROCS = @V_NUMPROCS /2;
	--END IF

	-- IF cores > 32 then files should be 1/4 of cores number
	If @V_NUMPROCS >32
		SELECT @V_NUMPROCS = @V_NUMPROCS /4;
	--END IF
	Create Table ##temp
	(
		DatabaseName sysname,
		LogicalName sysname,
		Location nvarchar(500),
		SizeMB decimal (18,2),
		Growth INT
	)
	SET @V_SQLStatement = '
Use [' + @P_DB_Name + '];
Insert Into ##temp (DatabaseName, LogicalName, Location, SizeMB, Growth)
    SELECT 
		  [DatabaseName] = DB_NAME()
		, [LogicalName] = [name]
		, [Location] = REPLACE(physical_name, DB_NAME() + ''.mdf'', '''')
		, [SizeMB] = Cast(Cast(Round(cast(size as decimal) * 8.0/1024.0,2) as decimal(18,2)) as nvarchar)
		, [Growth] = Growth
    FROM sys.database_files WHERE [type]=0
'
	EXEC sp_executesql @V_SQLStatement;
	SELECT TOP(1)
		@V_DB_Location = [Location]
	, @V_NewFile_Size_MB = [SizeMB]
	, @V_File_LogicalName = [LogicalName]
	, @V_FileGrowth = [Growth]
	FROM ##temp
	-- IF CHARINDEX('Linux',@V_OS) > 0 --&& CHARINDEX('/',@V_DB_Location) <> 1
	-- 	BEGIN
	-- 		SET @V_DB_Location = '/' + @V_DB_Location;
	-- 	END
	-- --END IF
	IF CHARINDEX(@V_DBFileName,@V_DB_Location) > 0
	BEGIN
		SET @V_DB_Location = REPLACE(@V_DB_Location, @V_DBFileName, '');
	End
	--END IF
	IF @P_Debug = 1 PRINT '@V_DB_Location IS:';
	IF @P_Debug = 1 PRINT @V_DB_Location;
	SELECT @V_DB_File_Count=COUNT(*)
	FROM ##temp;
	DROP TABLE ##temp

	SELECT TOP(1)
		@V_FileGrowthType =  
		CASE mf.is_percent_growth
			WHEN 1 THEN '%'
			WHEN 0 THEN ' MB'
		END
	FROM sys.master_files mf
	WHERE DB_NAME(mf.database_id) = @P_DB_Name and [type]=0;

	IF @P_Debug = 1 PRINT '-- Current Number of ' + @P_DB_Name + ' datafiles: ' + cast(@V_DB_File_Count as varchar(5));

	-- Determine IF we already have enough datafiles
	If @V_DB_File_Count >= @V_NUMPROCS
		BEGIN
		PRINT '--****Number of Recommedned datafiles is already exists****';
		RETURN;
	End
	--END IF

	SET @V_NewFileLocation= Isnull(@V_NewFileLocation,@V_DB_Location);
	PRINT @V_NewFileLocation;

	-- Determine IF the new location exists or not
	DECLARE @file_results table(file_exists int,
		file_is_a_directory int,
		parent_directory_exists int);

	INSERT INTO @file_results
		(file_exists, file_is_a_directory, parent_directory_exists)
	EXEC [master].[dbo].xp_fileexist @V_NewFileLocation;
	SELECT *
	FROM @file_results;
	IF (SELECT file_is_a_directory
	FROM @file_results ) = 0
		BEGIN
		PRINT '-- New files Directory Does NOT exist , please specify a correct folder!';
		RETURN;
	END
	--END IF

	-- Determine IF we have enough free space on the destination drive

	DECLARE @FreeSpace Table (Drive char(1),
		MB_Free BIGINT)
	INSERT INTO @FreeSpace
	exec master..xp_fixeddrives

	IF (SELECT MB_Free
	FROM @FreeSpace
	WHERE drive = LEFT(@V_NewFileLocation,1) ) < @V_NUMPROCS * @V_NewFile_Size_MB
		BEGIN
		PRINT '-- WARNING: Not enough free space on ' + Upper(LEFT(@V_NewFileLocation,1)) + ':\ to accomodate the new files. Around '+ cast(@V_NUMPROCS * @V_NewFile_Size_MB as varchar(10))+ ' Mbytes are needed; Please add more space or choose a new location!';
		RETURN;
	END
	--END IF

	-- Determine IF any of the exisiting datafiles have different size than proposed ones.
	If exists
	(
		SELECT
		(CONVERT (BIGINT, size) * 8)/1024
	FROM tempdb.sys.database_files
	WHERE 
			type_desc= 'Rows'
		AND (CONVERT (BIGINT, size) * 8)/1024  <> @V_NewFile_Size_MB
	)

	IF @P_Debug = 1 PRINT
	'
/*
WARNING: Some Existing datafile(s) do NOT have the same size as new ones.
It''s recommended IF ALL datafiles have same size for optimal proportional-fill performance.Use ALTER DATABASE AND DBCC SHRINKFILE to resize files
 
Optimizing ' + @P_DB_Name + ' Performance : http://msdn.microsoft.com/en-us/library/ms175527.aspx
	'

	IF @P_Debug = 1 PRINT '****Proposed New ' + @P_DB_Name + ' Datafiles, PLEASE REVIEW CODE BEFORE RUNNIG  *****/
	------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
 
	'
	-- Generate the statements
	WHILE @V_DB_File_Count < @V_NUMPROCS
		BEGIN
		SELECT @V_SQLStatement = 
'ALTER DATABASE [' + @P_DB_Name + '] ADD FILE (NAME = N'''+ @V_File_LogicalName + '_' + CAST(@V_DB_File_Count +1 AS VARCHAR (5))+''',FILENAME = N'''+ @V_NewFileLocation + @P_DB_Name + '_' + CAST (@V_DB_File_Count +1 AS VARCHAR(5)) +'.ndf'',SIZE = ' + CAST(@V_NewFile_Size_MB AS VARCHAR(15)) + 'MB,FILEGROWTH = ' + CAST(@V_FileGrowth AS VARCHAR(15)) + @V_FileGrowthType +' )';
		IF @P_Debug = 1 
				PRINT @V_SQLStatement;
			ELSE
				EXEC sp_executesql @V_SQLStatement;
		--END IF
		SET @V_DB_File_Count += 1;
	END
--END WHILE
END

GO

