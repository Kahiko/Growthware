
CREATE VIEW [ZGWSystem].[vwSearchNVP]
	AS 
SELECT
	NVPSeqId
	, Schema_Name + '.' + Static_Name AS Name
	, Description
	, Status = (SELECT TOP(1) Name FROM ZGWSystem.Statuses WHERE StatusSeqId = StatusSeqId)
	, (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = NVP.Added_By) AS Added_By
	, Added_Date
	, (SELECT TOP(1) Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = NVP.Updated_By) AS Updated_By
	, Updated_Date
FROM 
	ZGWSystem.Name_Value_Pairs  NVP WITH(NOLOCK)

GO

