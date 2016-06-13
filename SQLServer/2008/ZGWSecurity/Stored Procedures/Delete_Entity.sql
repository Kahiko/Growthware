/*
Usage:
	DECLARE 
		@P_Security_Entity_SeqID int = 4,
		@P_Debug INT = 0

	exec ZGWSecurity.Delete_Function
		@P_Security_Entity_SeqID ,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/08/2011
-- Description:	Deletes a record from ZGWSecurity.Security_Entities
--	given the Security_Entity_SeqID
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Entity]
	@P_Security_Entity_SeqID int,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Start [ZGWSecurity].[Delete_Entity]'
	DELETE FROM ZGWSecurity.Security_Entities
	WHERE
		Security_Entity_SeqID = @P_Security_Entity_SeqID
	IF @P_Debug = 1 PRINT 'End [ZGWSecurity].[Delete_Entity]'
RETURN 0