﻿/*
Usage:
	DECLARE 	
		@P_Message_SeqID INT, 
		@P_Security_Entity_SeqID INT = 1,
		@P_Debug INT = 1

	exec ZGWCoreWeb.Get_Messages
		@P_Message_SeqID,
		@P_Security_Entity_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/19/2011
-- Description:	Returns messages from ZGWCoreWeb.Messages
--	given the Message_SeqID.  If Message_SeqID = -1
--	all messages are returned.
-- =============================================
CREATE PROCEDURE [ZGWCoreWeb].[Get_Messages]
	@P_Message_SeqID INT,
	@P_Security_Entity_SeqID INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWCoreWeb.Get_Messages'
	DECLARE @V_Default_Security_Entity_SeqID INT = ZGWSecurity.Get_Default_Entity_ID()
	/*
		Don't like this ... need to think of a better way
		something along the lines of core defaul security entity message
		+ other security entity messages ...
	*/
	IF (SELECT COUNT(*) FROM ZGWCoreWeb.[Messages] WHERE Security_Entity_SeqID = @P_Security_Entity_SeqID) = 0
		BEGIN
			IF (SELECT COUNT(*) FROM ZGWCoreWeb.[Messages] WHERE Security_Entity_SeqID = @V_Default_Security_Entity_SeqID) > 0
				BEGIN
					INSERT INTO ZGWCoreWeb.[Messages]
						SELECT
							@P_Security_Entity_SeqID
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
							ZGWCoreWeb.[Messages] WHERE Security_Entity_SeqID = @V_Default_Security_Entity_SeqID
					IF @P_Debug = 1 PRINT 'Needed to add entries for all message for the requested Security_Entity'
				END
			ELSE
				IF @P_Debug = 1 PRINT 'There are no message as of yet stop trying to get them!'
				RETURN
			--END IF
		END
	--END IF

	IF @P_Message_SeqID <> -1
		BEGIN
			IF @P_Debug = 1 PRINT 'Getting single message'
			SELECT
				Message_SeqID as MESSAGE_SEQ_ID
				, Security_Entity_SeqID as SE_SEQ_ID
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
				Message_SeqID = @P_Message_SeqID
		END
	ELSE
		BEGIN
			IF @P_Debug = 1 PRINT 'Getting all messages'
			SELECT
				Message_SeqID as MESSAGE_SEQ_ID
				, Security_Entity_SeqID as SE_SEQ_ID
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