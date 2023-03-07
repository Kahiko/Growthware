/*
Usage:
	SELECT * FROM ZGWCoreWeb.vwSearchMessages WHERE SecurityEntitySeqId = 1
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns messages from ZGWCoreWeb.Messages
--	given the MessageSeqId.  If MessageSeqId = -1
--	all messages are returned.
-- =============================================
CREATE VIEW [ZGWCoreWeb].[vwSearchMessages] AS 
	SELECT 
		[MessageSeqId]
		,[SecurityEntitySeqId]
		,[Name]
		,[Title]
		,[Description]
		,[Format_As_HTML]
		,[Body]
		,(SELECT Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = msg.Added_By) AS [Added_By]
		,[Added_Date]
		,(SELECT Account FROM ZGWSecurity.Accounts WHERE AccountSeqId = msg.Updated_By) AS [Updated_By]
		,[Updated_Date] 
	FROM 
		[ZGWCoreWeb].[Messages] msg WITH(NOLOCK)

GO

