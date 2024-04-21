
/*
Usage:
    DECLARE
		@P_LogSeqId int = 1

	exec ZGWSystem.Get_Log @P_LogSeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/04/2022
-- Description:	Retrievs a row from the [ZGWSystem].[Logging]
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Get_Log]
	@P_LogSeqId int,
	@P_StartDate VARCHAR(10) = NULL,
	@P_EndDate VARCHAR(10) = NULL
AS
    SET NOCOUNT ON;
    SELECT TOP 1
	[Account]
        , [Component]
        , [ClassName]
        , [Level]
        , [LogDate]
        , [LogSeqId]
        , [MethodName]
        , [Msg]
FROM
	[ZGWSystem].[Logging] WITH(NOLOCK)
WHERE
		[LogSeqId] = @P_LogSeqId;

    RETURN 0;

GO

