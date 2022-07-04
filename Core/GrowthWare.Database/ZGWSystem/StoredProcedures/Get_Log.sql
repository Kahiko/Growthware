
/*
Usage:
    DECLARE
		@P_LogSeqId int = 1

	EXEC ZGWSystem.Get_Log @P_LogSeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/04/2022
-- Description:	Retrievs a row from the [ZGWSystem].[Logging] table
--	ZGWSystem.Get_Log
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Get_Log]
	  @P_LogSeqId int
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

