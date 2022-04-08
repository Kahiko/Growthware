
/*
Usage:
	DECLARE 
		@P_Account_SeqID int = 4,
		@P_Debug INT = 0

	exec  ZGWSecurity.Delete_Account
		@P_Account_SeqID ,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Deletes a record from [ZGWSecurity].[Accounts]
--	given the Account_SeqID
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Account]
	@P_Account_SeqID int,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting [ZGWSecurity].[Delete_Account]'
	-- DELETE an existing row from the table.
	DELETE FROM ZGWSecurity.Accounts
	WHERE
		Account_SeqID = @P_Account_SeqID
	IF @P_Debug = 1 PRINT 'Ending [ZGWSecurity].[Delete_Account]'
RETURN 0

GO

