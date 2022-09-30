
/*
Usage:
	DECLARE
		@P_SQL VARCHAR(MAX) = 'SELECT * FROM ZGWOptional.Calendars',
		@P_Debug INT = 1

	exec ZGWSystem.Get_JSON
		@P_SQL,
		@P_Debug
*/
-- =============================================
-- Author:		karuta
-- Create date: 05/25/2013
-- Description:	JSON from SQL Statement 
-- Note:
--  Found at
--	http://karuta.wordpress.com/2011/08/31/gerar-json-em-qualquer-clausula-sql/
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Get_JSON]
	@P_SQL VARCHAR(MAX),
	@P_Debug INT = 0
AS
BEGIN
	DECLARE @SQL NVARCHAR(MAX)
	DECLARE @XMLString VARCHAR(MAX)
	DECLARE @XML XML
	DECLARE @Paramlist NVARCHAR(1000)
	SET @Paramlist = N'@XML XML OUTPUT'
	SET @SQL = 'WITH PrepareTable (XMLString) '
	SET @SQL = @SQL + 'AS ( '
	SET @SQL = @SQL + @P_SQL+ ' FOR XML RAW, TYPE, ELEMENTS '
	SET @SQL = @SQL + ') '
	SET @SQL = @SQL + 'SELECT @XML = XMLString FROM PrepareTable '
	EXEC sp_executesql @SQL, @Paramlist, @XML=@XML OUTPUT
	SET @XMLString = CAST(@XML AS VARCHAR(MAX))

	DECLARE @JSON VARCHAR(MAX)
	DECLARE @Row VARCHAR(MAX)
	DECLARE @RowStart INT
	DECLARE @RowEnd INT
	DECLARE @FieldStart INT
	DECLARE @FieldEnd INT
	DECLARE @KEY VARCHAR(MAX)
	DECLARE @Value VARCHAR(MAX)

	DECLARE @StartRoot VARCHAR(100);
	SET @StartRoot = '<row>'
	DECLARE @EndRoot VARCHAR(100);
	SET @EndRoot = '</row>'
	DECLARE @StartField VARCHAR(100);
	SET @StartField = '<'
	DECLARE @EndField VARCHAR(100);
	SET @EndField = '>'

	SET @RowStart = CharIndex(@StartRoot, @XMLString, 0)
	SET @JSON = ''
	WHILE @RowStart > 0
	BEGIN
		SET @RowStart = @RowStart+Len(@StartRoot)
		SET @RowEnd = CharIndex(@EndRoot, @XMLString, @RowStart)
		SET @Row = SubString(@XMLString, @RowStart, @RowEnd-@RowStart)
		SET @JSON = @JSON+'{'

		-- for each row
		SET @FieldStart = CharIndex(@StartField, @Row, 0)
		WHILE @FieldStart > 0
		BEGIN
			-- parse node key
			SET @FieldStart = @FieldStart+Len(@StartField)
			SET @FieldEnd = CharIndex(@EndField, @Row, @FieldStart)
			SET @KEY = SubString(@Row, @FieldStart, @FieldEnd-@FieldStart)
			SET @JSON = @JSON+'"'+@KEY+'":'

			-- parse node value
			SET @FieldStart = @FieldEnd+1
			SET @FieldEnd = CharIndex('</', @Row, @FieldStart)
			SET @Value = SubString(@Row, @FieldStart, @FieldEnd-@FieldStart)
			SET @JSON = @JSON+'"'+@Value+'",'

			SET @FieldStart = @FieldStart+Len(@StartField)
			SET @FieldEnd = CharIndex(@EndField, @Row, @FieldStart)
			SET @FieldStart = CharIndex(@StartField, @Row, @FieldEnd)
		END
		IF LEN(@JSON)>0 SET @JSON = SubString(@JSON, 0, LEN(@JSON))
		SET @JSON = @JSON+'},'
		--/ for each row

		SET @RowStart = CharIndex(@StartRoot, @XMLString, @RowEnd)
	END
	IF LEN(@JSON)>0 SET @JSON = SubString(@JSON, 0, LEN(@JSON))
	SET @JSON = '[' + @JSON + ']'
	SELECT @JSON

END

GO

