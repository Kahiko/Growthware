CREATE VIEW [ZGWSystem].[vwSearchFunctions]
	AS 
SELECT
	FunctionSeqId
	, Name
	, Description
	, Action
	, (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = FUN.Added_By) AS Added_By
	, Added_Date
	, (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = FUN.Updated_By) AS Updated_By
	, Updated_Date
FROM 
	ZGWSecurity.Functions FUN WITH(NOLOCK)

GO

