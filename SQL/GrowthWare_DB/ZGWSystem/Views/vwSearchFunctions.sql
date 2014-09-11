CREATE VIEW [ZGWSystem].[vwSearchFunctions]
	AS 
SELECT
	Function_SeqID
	, Name
	, Description
	, Action
	, (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE Account_SeqID = Added_By) AS Added_By
	, Added_Date
	, (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE Account_SeqID = Updated_By) AS Updated_By
	, Updated_Date
FROM 
	ZGWSecurity.Functions