/*
Usage:
	DECLARE
		@P_Security_Entity_SeqID int = 1,
		@P_Function_SeqID int = 1,
		@P_Account VARCHAR(128) = 'Developer',
		@P_Primary_Key INT = null,
		@P_Debug INT = 1

	exec ZGWCoreWeb.Get_Notification_Status
		@P_Security_Entity_SeqID,
		@P_Function_SeqID,
		@P_Account,
		@P_Primary_Key,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns single value of 0 or 1
-- =============================================
CREATE PROCEDURE [ZGWCoreWeb].[Get_Notification_Status]
	@P_Security_Entity_SeqID int,
	@P_Function_SeqID int,
	@P_Account VARCHAR(128),
	@P_Primary_Key INT OUTPUT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Notification_Status'
	-- GET Account_SeqID
	DECLARE @V_Account_SeqID Int
	SET @V_Account_SeqID = (SELECT Account_SeqID FROM ZGWSecurity.Accounts WHERE Account = @P_Account)

	IF EXISTS
		(SELECT 
			Added_By
		FROM 
			ZGWCoreWeb.Notifications WITH(NOLOCK)
		WHERE 
			Security_Entity_SeqID = @P_Security_Entity_SeqID
			AND Function_SeqID = @P_Function_SeqID
			AND Added_By = @V_Account_SeqID)
		BEGIN
			SELECT @P_Primary_Key = 1
		END
	ELSE
		BEGIN
			SELECT @P_Primary_Key = 0
		END
	--END IF
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Notification_Status'

RETURN 0