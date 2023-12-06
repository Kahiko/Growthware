
/*
Usage:
	DECLARE 
		@P_AccountSeqId int = 4,
		@P_Debug INT = 0

	exec  ZGWSecurity.Delete_Account
		@P_AccountSeqId ,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from [ZGWSecurity].[Accounts]
--	given the AccountSeqId
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Account]
	@P_AccountSeqId int,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting [ZGWSecurity].[Delete_Account]'
	-- DELETE an existing row from the table.
	DELETE FROM ZGWSecurity.Accounts
	WHERE
		AccountSeqId = @P_AccountSeqId
	IF @P_Debug = 1 PRINT 'Ending [ZGWSecurity].[Delete_Account]'
RETURN 0

GO

