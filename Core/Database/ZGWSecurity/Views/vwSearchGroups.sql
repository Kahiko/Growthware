CREATE VIEW [ZGWSecurity].[vwSearchGroups] AS
	SELECT
		G.[GroupSeqId] AS Group_SEQ_ID,
		G.[Name],
		G.[Description],
		Added_By = (SELECT Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = G.Added_By),
		Added_Date = (SELECT Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = G.Added_Date),
		G.[Updated_By],
		G.[Updated_Date],
		RSE.SecurityEntitySeqId
	FROM
		ZGWSecurity.Groups G WITH(NOLOCK)
		INNER JOIN ZGWSecurity.Groups_Security_Entities RSE WITH(NOLOCK)
			ON G.GroupSeqId = RSE.GroupSeqId

GO

