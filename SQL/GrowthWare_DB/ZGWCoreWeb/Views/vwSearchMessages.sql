/*
Usage:
	SELECT * FROM ZGWCoreWeb.vwSearchMessages WHERE Security_Entity_SeqID = 1
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns messages from ZGWCoreWeb.Messages
--	given the Message_SeqID.  If Message_SeqID = -1
--	all messages are returned.
-- =============================================
CREATE VIEW [ZGWCoreWeb].[vwSearchMessages] AS 
	SELECT 
		[Message_SeqID]
		,[Security_Entity_SeqID]
		,[Name]
		,[Title]
		,[Description]
		,[Format_As_HTML]
		,[Body]
		,(SELECT Account FROM ZGWSecurity.Accounts WHERE Account_SeqID = msg.Added_By) AS [Added_By]
		,[Added_Date]
		,(SELECT Account FROM ZGWSecurity.Accounts WHERE Account_SeqID = msg.Updated_By) AS [Updated_By]
		,[Updated_Date] 
	FROM 
		[ZGWCoreWeb].[Messages] msg WITH(NOLOCK)