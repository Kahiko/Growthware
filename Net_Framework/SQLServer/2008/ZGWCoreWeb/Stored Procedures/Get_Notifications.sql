/*
Usage:
	DECLARE
		@P_Security_Entity_SeqID int,
		@P_Function_SeqID int,
		@P_Debug INT = 1

	exec ZGWCoreWeb.Get_Notifications
		@P_Security_Entity_SeqID,
		@P_Function_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns single value of 0 or 1
-- =============================================
CREATE PROCEDURE [ZGWCoreWeb].[Get_Notifications]
	@P_Security_Entity_SeqID int,
	@P_Function_SeqID int,
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
				ON Accounts.Account_SeqID = Notifications.Added_By
	WHERE
		Accounts.Enable_Notifications = @V_Enable_Notifications
		AND Notifications.Function_SeqID = @P_Function_SeqID
		AND Notifications.Security_Entity_SeqID = @P_Security_Entity_SeqID
	ORDER BY
		Email

	IF @P_Debug = 1 PRINT 'Ending ZGWCoreWeb.Get_Notifications'

RETURN 0