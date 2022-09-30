
/*
Usage:
    DECLARE
        @P_Account       VARCHAR (128) = 'System'
        , @P_Component     VARCHAR (50)  = 'UI'
        , @P_ClassName     VARCHAR (50)  = 'TestClass'
        , @P_Level         VARCHAR (5)   = 'Debug'
        , @P_MethodName    VARCHAR (50)  = 'TestMethod'
        , @P_Msg           VARCHAR (MAX) = 'Just testing'
        , @P_Primary_Key int

    EXEC [ZGWSystem].[Set_Log]
            @P_Account
            , @P_Component
            , @P_ClassName
            , @P_Level
            , @P_MethodName
            , @P_Msg
            , @P_Primary_Key OUTPUT
    PRINT @P_Primary_Key
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/04/2022
-- Description:	Inserts a row into the [ZGWSystem].[Logging] table
--	ZGWSystem.Get_Log
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Set_Log]
	@P_Account       VARCHAR (128),
	@P_Component     VARCHAR (50),
	@P_ClassName     VARCHAR (50),
	@P_Level         VARCHAR (5),
	@P_MethodName    VARCHAR (50),
	@P_Msg           VARCHAR (MAX),
	@P_Primary_Key int OUTPUT
AS
    SET NOCOUNT ON;
    INSERT [ZGWSystem].[Logging](
		[Account]
		, [Component]
		, [ClassName]
		, [Level]
		, [LogDate]
		, [MethodName]
		, [Msg]
	)VALUES(
		@P_Account
        , @P_Component
        , @P_ClassName
        , @P_Level
        , GETDATE()
        , @P_MethodName
        , @P_Msg
    )
    SELECT @P_Primary_Key = SCOPE_IDENTITY()-- Get the IDENTITY value for the row just inserted.
    RETURN 0;

GO

