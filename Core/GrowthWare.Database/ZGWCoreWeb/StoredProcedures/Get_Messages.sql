
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
CREATE PROCEDURE [ZGWCoreWeb].[Get_Messages]
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
	IF (SELECT COUNT(*) FROM ZGWCoreWeb.[Messages] WHERE SecurityEntitySeqId = @P_SecurityEntitySeqId) = 0
		BEGIN
			IF (SELECT COUNT(*) FROM ZGWCoreWeb.[Messages] WHERE SecurityEntitySeqId = @V_DefaultSecurityEntitySeqId) > 0
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
							ZGWCoreWeb.[Messages] WHERE SecurityEntitySeqId = @V_DefaultSecurityEntitySeqId
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

