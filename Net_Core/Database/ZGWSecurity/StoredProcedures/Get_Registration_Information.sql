/*
Usage:
	DECLARE @P_SecurityEntitySeqId int = -1
	EXECUTE [ZGWSecurity].[Get_Registration_Information] @P_SecurityEntitySeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/10/2024
-- Description:	Returns registration information
-- Note:
--	SecurityEntitySeqId of -1 returns all registration information.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Registration_Information]
    @P_SecurityEntitySeqId int = -1
AS
	SET NOCOUNT ON;
    IF @P_SecurityEntitySeqId = -1
        BEGIN
            SELECT
                RI.[SecurityEntitySeqId],
                RI.[SecurityEntitySeqId_Owner],
                RI.[AccountChoices],
                RI.[AddAccount],
                RI.[Groups],
                RI.[Roles],
                RI.[Added_By],
                RI.[Added_Date],
                RI.[Updated_By],
                RI.[UPDATED_DATE]
            FROM
                [ZGWSecurity].[Registration_Information] AS RI
            ORDER BY
                RI.[SecurityEntitySeqId]
        END
    ELSE
        BEGIN
            SELECT
                RI.[SecurityEntitySeqId],
                RI.[SecurityEntitySeqId_Owner],
                RI.[AccountChoices],
                RI.[AddAccount],
                RI.[Groups],
                RI.[Roles],
                RI.[Added_By],
                RI.[Added_Date],
                RI.[Updated_By],
                RI.[UPDATED_DATE]
            FROM
                [ZGWSecurity].[Registration_Information] AS RI
            WHERE
                RI.[SecurityEntitySeqId] = @P_SecurityEntitySeqId
            ORDER BY
                RI.[SecurityEntitySeqId]
        END
    --END IF

    SET NOCOUNT OFF;

RETURN 0

GO

