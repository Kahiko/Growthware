
CREATE VIEW [ZGWOptional].[vwSearchStates]
	AS 
SELECT
	[State]
	, [Description]
	, [Status] = (SELECT TOP(1) [Name] FROM [ZGWSystem].[Statuses] WHERE [Status_SeqID] = States.[Status_SeqID])
	, (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE Account_SeqID = States.Added_By) AS Added_By
	, Added_Date
	, (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE Account_SeqID = States.Updated_By) AS Updated_By
	, Updated_Date
FROM 
	[ZGWOptional].[States] States WITH(NOLOCK)