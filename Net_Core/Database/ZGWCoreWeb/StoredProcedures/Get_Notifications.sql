
/*
Usage:
	DECLARE
		@P_SecurityEntitySeqId int,
		@P_FunctionSeqId int,
		@P_Debug INT = 1

	exec ZGWCoreWeb.Get_Notifications
		@P_SecurityEntitySeqId,
		@P_FunctionSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns single value of 0 or 1
-- =============================================
CREATE PROCEDURE [ZGWCoreWeb].[Get_Notifications]
	@P_SecurityEntitySeqId int,
	@P_FunctionSeqId int,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWCoreWeb.Get_Notifications'
	DECLARE @V_Enable_Notifications Int
	SET @V_Enable_Notifications = 1 -- True

	SELECT
	Email
FROM
	ZGWSecurity.Accounts Accounts WITH(NOLOCK)
	INNER JOIN ZGWCoreWeb.Notifications Notifications WITH(NOLOCK)
	ON Accounts.AccountSeqId = Notifications.Added_By
WHERE
		Accounts.Enable_Notifications = @V_Enable_Notifications
	AND Notifications.FunctionSeqId = @P_FunctionSeqId
	AND Notifications.SecurityEntitySeqId = @P_SecurityEntitySeqId
ORDER BY
		Email

	IF @P_Debug = 1 PRINT 'Ending ZGWCoreWeb.Get_Notifications'

RETURN 0

GO

