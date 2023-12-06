CREATE FUNCTION [ZGWSecurity].[udfSplit] (
	 @P_Text VARCHAR(MAX)
	,@P_Delimiter NCHAR(1)
)
RETURNS @V_TblSplitValues TABLE (
	[Id] INT
	,[Data] VARCHAR(50)
	)
AS
BEGIN
	DECLARE @V_AuxString VARCHAR(MAX);

	SET @V_AuxString = REPLACE(@P_Text, @P_Delimiter, '~');

	WITH Split (
		stpos
		,endpos
	) AS (
		SELECT 0 AS stpos
			,CHARINDEX('~', @V_AuxString) AS endpos
		
		UNION ALL
		
		SELECT CAST(endpos AS INT) + 1
			,CHARINDEX('~', @V_AuxString, endpos + 1)
		FROM Split
		WHERE endpos > 0
	)
	INSERT @V_TblSplitValues
	SELECT [Id] = ROW_NUMBER() OVER (ORDER BY (SELECT 1))
		,[Data] = SUBSTRING(@V_AuxString, stpos, COALESCE(NULLIF(endpos, 0), LEN(@V_AuxString) + 1) - stpos)
	FROM Split;

	RETURN;
END
GO