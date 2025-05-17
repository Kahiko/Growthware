-- Downgrade from 6.0.0.0 to 5.2.0.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
GO

DECLARE 
      @V_FunctionSeqId INT = -1
    , @V_Action VARCHAR(256) = '/sys_admin/searchDBLogs'
	, @P_ErrorCode BIT = 0;

IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [Action] = @V_Action)
	BEGIN
		PRINT 'Deleting ' + CONVERT(VARCHAR(MAX), @V_Action);
		SET @V_FunctionSeqId = (SELECT TOP(1) [FunctionSeqId] FROM [ZGWSecurity].[Functions] WHERE [Action] =  @V_Action);	
		EXEC [ZGWSecurity].[Delete_Function]
			@V_FunctionSeqId ,
			@P_ErrorCode;
	END
--END IF

/****** Start: [ZGWCoreWeb].[Get_Messages] ******/
GO
/*
Usage:
	DECLARE 	
		@P_MessageSeqId INT, 
		@P_SecurityEntitySeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWCoreWeb.Get_Messages
		@P_MessageSeqId,
		@P_SecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns messages from ZGWCoreWeb.Messages
--	given the MessageSeqId.  If MessageSeqId = -1
--	all messages are returned.
-- =============================================
CREATE OR ALTER PROCEDURE [ZGWCoreWeb].[Get_Messages]
	@P_MessageSeqId INT,
	@P_SecurityEntitySeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWCoreWeb.Get_Messages'
	DECLARE @V_DefaultSecurityEntitySeqId INT = ZGWSecurity.Get_Default_Entity_ID()
	/*
		Don't like this ... need to think of a better way
		something along the lines of core defaul security entity message
		+ other security entity messages ...
	*/
	IF (SELECT COUNT(*)
FROM ZGWCoreWeb.[Messages]
WHERE SecurityEntitySeqId = @P_SecurityEntitySeqId) = 0
		BEGIN
	IF (SELECT COUNT(*)
	FROM ZGWCoreWeb.[Messages]
	WHERE SecurityEntitySeqId = @V_DefaultSecurityEntitySeqId) > 0
				BEGIN
		INSERT INTO ZGWCoreWeb.[Messages]
		SELECT
			@P_SecurityEntitySeqId
							, Name
							, Title
							, [Description]
							, Format_As_HTML
							, Body
							, Added_By
							, Added_Date
							, Updated_By
							, Updated_Date
		FROM
			ZGWCoreWeb.[Messages]
		WHERE SecurityEntitySeqId = @V_DefaultSecurityEntitySeqId
		IF @P_Debug = 1 PRINT 'Needed to add entries for all message for the requested Security_Entity'
	END
			ELSE
				IF @P_Debug = 1 PRINT 'There are no message as of yet stop trying to get them!'
	RETURN
--END IF
END
	--END IF

	IF @P_MessageSeqId <> -1
		BEGIN
	IF @P_Debug = 1 PRINT 'Getting single message'
	SELECT
		MessageSeqId as MESSAGE_SEQ_ID
				, SecurityEntitySeqId as SecurityEntityID
				, NAME
				, TITLE
				, [Description]
				, Format_As_HTML
				, BODY
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM
		ZGWCoreWeb.[Messages]
	WHERE
				MessageSeqId = @P_MessageSeqId
END
	ELSE
		BEGIN
	IF @P_Debug = 1 PRINT 'Getting all messages'
	SELECT
		MessageSeqId as MESSAGE_SEQ_ID
				, SecurityEntitySeqId as SecurityEntityID
				, NAME
				, TITLE
				, [Description]
				, Format_As_HTML
				, BODY
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
	FROM
		ZGWCoreWeb.[Messages]
	ORDER BY
				[Name]
END
	--END IF
RETURN 0
GO

/****** End: [ZGWCoreWeb].[Get_Messages] ******/

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '5.2.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;