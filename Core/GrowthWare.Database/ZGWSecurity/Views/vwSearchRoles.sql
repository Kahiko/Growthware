CREATE VIEW [ZGWSecurity].[vwSearchRoles] AS 
	SELECT
		R.[Role_SeqID] AS ROLE_SEQ_ID,
		R.[Name],
		R.[Description],
		R.[Is_System],
		R.[Is_System_Only],
		Added_By = (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE Account_SeqID = R.Added_By),
		R.Added_Date,
		[Updated_By] = (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE Account_SeqID = R.Updated_By),
		R.[Updated_Date],
		RSE.Security_Entity_SeqID
	FROM
		ZGWSecurity.Roles R WITH(NOLOCK)
		INNER JOIN ZGWSecurity.Roles_Security_Entities RSE WITH(NOLOCK)
			ON R.Role_SeqID = RSE.Role_SeqID

GO

