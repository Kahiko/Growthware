CREATE VIEW [ZGWSecurity].[vwSearchGroups] AS
	SELECT
		G.[Group_SeqID] AS Group_SEQ_ID,
		G.[Name],
		G.[Description],
		Added_By = (SELECT Account FROM ZGWSecurity.Accounts WHERE Account_SeqID = G.Added_By),
		Added_Date = (SELECT Account FROM ZGWSecurity.Accounts WHERE Account_SeqID = G.Added_Date),
		G.[Updated_By],
		G.[Updated_Date],
		RSE.Security_Entity_SeqID
	FROM
		ZGWSecurity.Groups G WITH(NOLOCK)
		INNER JOIN ZGWSecurity.Groups_Security_Entities RSE WITH(NOLOCK)
			ON G.Group_SeqID = RSE.Group_SeqID

GO

