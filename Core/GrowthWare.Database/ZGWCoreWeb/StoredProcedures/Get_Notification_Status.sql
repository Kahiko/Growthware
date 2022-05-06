
/*
Usage:
	DECLARE
		@PSecurityEntitySeqId int = 1,
		@P_FunctionSeqId int = 1,
		@P_Account VARCHAR(128) = 'Developer',
		@P_Primary_Key INT = null,
		@P_Debug INT = 1

	exec ZGWCoreWeb.Get_Notification_Status
		@PSecurityEntitySeqId,
		@P_FunctionSeqId,
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
	@PSecurityEntitySeqId int,
	@P_FunctionSeqId int,
	@P_Account VARCHAR(128),
	@P_Primary_Key INT OUTPUT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Notification_Status'
	-- GET AccountSeqId
	DECLARE @V_AccountSeqId Int
	SET @V_AccountSeqId = (SELECT AccountSeqId FROM ZGWSecurity.Accounts WHERE Account = @P_Account)

	IF EXISTS
		(SELECT 
			Added_By
		FROM 
			ZGWCoreWeb.Notifications WITH(NOLOCK)
		WHERE 
			SecurityEntitySeqId = @PSecurityEntitySeqId
			AND FunctionSeqId = @P_FunctionSeqId
			AND Added_By = @V_AccountSeqId)
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

GO

