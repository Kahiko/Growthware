/*
Usage:
	DECLARE 
		@P_Account VARCHAR(128) = 'Developer',
		@P_Debug INT = 0

	exec ZGWCoreWeb.Get_Account_Choice
		@P_Account ,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/08/2011
-- Description: Gets a record from xx
--	given the Account
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/26/2024
-- Description: Updated to match [ZGWCoreWeb].[Account_Choices] changes
-- =============================================
CREATE PROCEDURE [ZGWCoreWeb].[Get_Account_Choice]
	@P_Account VARCHAR(128),
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF EXISTS(SELECT Account FROM [ZGWCoreWeb].[Account_Choices] WHERE Account = @P_Account)
		BEGIN
            IF @P_Debug = 1 PRINT 'Selecting client choices for ' + CONVERT(VARCHAR(25),@P_Account)
            SELECT
                  [Account]
                , [SecurityEntityId]
                , [SecurityEntityName]
                , [FavoriteAction]
                , [RecordsPerPage]
                , [ColorScheme]
                , [EvenRow]
                , [EvenFont]
                , [OddRow]
                , [OddFont]
                , [HeaderRow]
                , [HeaderFont]
                , [Background]
            FROM 
                [ZGWCoreWeb].[Account_Choices]
            WHERE
                [Account] = @P_Account
        END
	ELSE
		BEGIN
            IF @P_Debug = 1 PRINT 'Selecting client choices for the Anonymous account'
            SELECT
                  [Account]
                , [SecurityEntityId]
                , [SecurityEntityName]
                , [FavoriteAction]
                , [RecordsPerPage]
                , [ColorScheme]
                , [EvenRow]
                , [EvenFont]
                , [OddRow]
                , [OddFont]
                , [HeaderRow]
                , [HeaderFont]
                , [Background]
            FROM 
                [ZGWCoreWeb].[Account_Choices]
            WHERE
                [Account] = 'Anonymous';
        END
    -- END IF

RETURN 0
GO
