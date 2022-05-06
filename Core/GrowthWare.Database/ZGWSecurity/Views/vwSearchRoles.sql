CREATE VIEW [ZGWSecurity].[vwSearchRoles] AS 
	SELECT
		R.[RoleSeqId] AS ROLE_SEQ_ID,
		R.[Name],
		R.[Description],
		R.[Is_System],
		R.[Is_System_Only],
		Added_By = (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = R.Added_By),
		R.Added_Date,
		[Updated_By] = (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = R.Updated_By),
		R.[Updated_Date],
		RSE.SecurityEntitySeqId
	FROM
		ZGWSecurity.Roles R WITH(NOLOCK)
		INNER JOIN ZGWSecurity.Roles_Security_Entities RSE WITH(NOLOCK)
			ON R.RoleSeqId = RSE.RoleSeqId

GO

