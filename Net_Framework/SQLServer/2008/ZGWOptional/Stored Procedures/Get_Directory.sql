/*
Usage:
	DECLARE 
		@P_Function_SeqID INT = 1,
		@P_Debug INT = 1

	exec ZGWOptional.Get_Directory
		@P_Function_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/11/2011
-- Description:	Selects directory infomation given
--	the Function_SeqID. When Function_SeqID = -1
--	all rows in the table are retruned.
-- =============================================
CREATE PROCEDURE [ZGWOptional].[Get_Directory]
		@P_Function_SeqID INT,
		@P_Debug INT = 0
AS
IF @P_Debug = 1 PRINT 'Starting ZGWOptional.Get_Directory'
IF @P_Function_SeqID = -1
	BEGIN
		IF @P_Debug = 1 PRINT 'Getting all'
		SELECT
			Function_SeqID as FUNCTION_SEQ_ID
			, Directory
			, Impersonate
			, Impersonating_Account as IMPERSONATE_ACCOUNT
			, Impersonating_Password as IMPERSONATE_PWD
			, Added_By
			, Added_Date
			, Updated_By
			, Updated_Date
		FROM
			ZGWOptional.Directories WITH(NOLOCK)
		ORDER BY
			Directory
	END
ELSE
	BEGIN
		IF @P_Debug = 1 PRINT 'Getting 1'
		SELECT
			Function_SeqID as FUNCTION_SEQ_ID
			, Directory
			, Impersonate
			, Impersonating_Account as IMPERSONATE_ACCOUNT
			, Impersonating_Password as IMPERSONATE_PWD
			, Added_By
			, Added_Date
			, Updated_By
			, Updated_Date
		FROM
			ZGWOptional.Directories
		WHERE
			Function_SeqID = @P_Function_SeqID
	END
-- end if
IF @P_Debug = 1 PRINT 'Ending ZGWOptional.Get_Directory'

RETURN 0