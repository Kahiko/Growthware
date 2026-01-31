SET NOCOUNT ON;
DECLARE @V_DiagramName VARCHAR(50) = 'Security_ERD'
IF EXISTS (SELECT 1 FROM sysDiagrams WHERE name = @V_DiagramName)
	BEGIN
		PRINT 'Deleted existing ' + @V_DiagramName;
		DELETE sysDiagrams WHERE name = @V_DiagramName;
	END
ELSE
	PRINT 'No need to delete ' + @V_DiagramName;
--END IF

IF OBJECT_ID('tempdb..#tbl') IS NOT NULL
	BEGIN
		PRINT 'Table exists';
		DROP TABLE #tbl;
	END
ELSE
	BEGIN
		PRINT 'Table does not exist';
	END
--END IF

CREATE TABLE #tbl (DEF VARBINARY(max))

BULK INSERT #tbl FROM 'c:\Temp\Diagram\Security_ERD'
--SELECT * FROM #tbl;
INSERT INTO sysdiagrams
	(name			, principal_id	, version , definition) VALUES 
	('Security_ERD'	, 1				, 1		  , (select def from #tbl))

IF OBJECT_ID('tempdb..#tbl') IS NOT NULL
	BEGIN
		PRINT 'Cleanup: Droping the #tbl table';
		DROP TABLE #tbl;
	END
--END IF