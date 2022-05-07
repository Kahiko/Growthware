
/*
Usage:
	DECLARE 
		@P_SecurityEntitySeqId int = 4,
		@P_Debug INT = 0

	exec ZGWSecurity.Delete_Function
		@P_SecurityEntitySeqId ,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/08/2011
-- Description:	Deletes a record from ZGWSecurity.Security_Entities
--	given the SecurityEntitySeqId
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Entity]
	@P_SecurityEntitySeqId int,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Start [ZGWSecurity].[Delete_Entity]'
	DELETE FROM ZGWSecurity.Security_Entities
	WHERE
		SecurityEntitySeqId = @P_SecurityEntitySeqId
	IF @P_Debug = 1 PRINT 'End [ZGWSecurity].[Delete_Entity]'
RETURN 0

GO

