
/*
Usage:
	DECLARE 
		@P_SecurityEntitySeqId int = 2,
		@P_FunctionSeqId int = 1,
		@P_Account Varchar(128) = 'Developer',
		@P_Status int = 1,
		@P_Debug INT = 1

	exec ZGWCoreWeb.Set_Notification
		@P_SecurityEntitySeqId,
		@P_FunctionSeqId,
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
	@P_SecurityEntitySeqId int,
	@P_FunctionSeqId int,
	@P_Account Varchar(128),
	@P_Status int,
	@P_Debug INT = 0
AS

IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Notification'
DECLARE @V_AccountSeqId int = (SELECT AccountSeqId
FROM ZGWSecurity.Accounts
WHERE Account = @P_Account)

IF @P_Status = 1
	BEGIN
	IF NOT EXISTS
		(
			SELECT
		Added_By
	FROM
		ZGWCoreWeb.Notifications
	WHERE 
				SecurityEntitySeqId = @P_SecurityEntitySeqId
		AND FunctionSeqId = @P_FunctionSeqId
		AND Added_By = @V_AccountSeqId
		)
		BEGIN
		IF @P_Debug = 1 PRINT 'Insert'
		INSERT ZGWCoreWeb.Notifications
			(
			SecurityEntitySeqId,
			FunctionSeqId,
			Added_By
			)
		VALUES
			(
				@P_SecurityEntitySeqId,
				@P_FunctionSeqId,
				@V_AccountSeqId
			)
	END
END
ELSE
	IF @P_Debug = 1 PRINT 'Delete'
	DELETE 
		ZGWCoreWeb.Notifications
	WHERE 
		SecurityEntitySeqId = @P_SecurityEntitySeqId
	AND FunctionSeqId = @P_FunctionSeqId
	AND Added_By = @V_AccountSeqId
--END IF
IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Set_Notification'
RETURN 0

GO

