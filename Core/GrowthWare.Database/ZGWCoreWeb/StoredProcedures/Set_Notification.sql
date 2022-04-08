
/*
Usage:
	DECLARE 
		@P_Security_Entity_SeqID int = 2,
		@P_Function_SeqID int = 1,
		@P_Account Varchar(128) = 'Developer',
		@P_Status int = 1,
		@P_Debug INT = 1

	exec ZGWCoreWeb.Set_Notification
		@P_Security_Entity_SeqID,
		@P_Function_SeqID,
		@P_Account,
		@P_Status,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/07/2011
-- Description:	Inserts or deletes from ZGWCoreWeb.Notifications
--	Status value 1 = insert, 0 = delete
-- =============================================
CREATE PROCEDURE [ZGWCoreWeb].[Set_Notification]
	@P_Security_Entity_SeqID int,
	@P_Function_SeqID int,
	@P_Account Varchar(128),
	@P_Status int,
	@P_Debug INT = 0
AS

IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Notification'
DECLARE @V_Account_SeqID int = (SELECT Account_SeqID FROM ZGWSecurity.Accounts WHERE Account = @P_Account)

IF @P_Status = 1
	BEGIN
	  IF NOT EXISTS
		(
			SELECT 
				Added_By 
			FROM 
				ZGWCoreWeb.Notifications 
			WHERE 
				Security_Entity_SeqID = @P_Security_Entity_SeqID
				AND Function_SeqID = @P_Function_SeqID
				AND Added_By = @V_Account_SeqID
		)
		BEGIN
			IF @P_Debug = 1 PRINT 'Insert'
			INSERT ZGWCoreWeb.Notifications
			(
			Security_Entity_SeqID,
			Function_SeqID,
			Added_By
			)
			VALUES
			(
			@P_Security_Entity_SeqID,
			@P_Function_SeqID,
			@V_Account_SeqID
			)
		END
	END
ELSE
	IF @P_Debug = 1 PRINT 'Delete'
	DELETE 
		ZGWCoreWeb.Notifications
	WHERE 
		Security_Entity_SeqID = @P_Security_Entity_SeqID
		AND Function_SeqID = @P_Function_SeqID
		AND Added_By = @V_Account_SeqID
--END IF
IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Set_Notification'
RETURN 0

GO

