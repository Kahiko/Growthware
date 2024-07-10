
/*
Usage:
	DECLARE 
		@P_SecurityEntitySeqId int = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Delete_Registration_Information
		@P_SecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/10/2024
-- Description:	Deletes a record from [ZGWSecurity].[Registration_Information] given
--  the SecurityEntitySeqId
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Delete_Registration_Information]
	@P_SecurityEntitySeqId int,

	@P_Debug INT = 0
AS
	DECLARE @V_Now DATETIME = GETDATE()
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting [ZGWSecurity].[Delete_Registration_Information]'
    DECLARE @V_Now DATETIME = GETDATE();
    IF EXISTS (SELECT NULL FROM [ZGWSecurity].[Registration_Information] WHERE [SecurityEntitySeqId] = @P_SecurityEntitySeqId)
    BEGIN
        DELETE FROM [ZGWSecurity].[Registration_Information] WHERE [SecurityEntitySeqId] = @P_SecurityEntitySeqId;
    END
    IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Registration_Information'
RETURN 0

GO
