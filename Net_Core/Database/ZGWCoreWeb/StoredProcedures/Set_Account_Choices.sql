/*
Usage:
	EXEC ZGWCoreWeb.Set_Account_Choices
		@P_ACCT = N'Anonymous',
		@P_SecurityEntityId = 1,
		@P_SecurityEntityName = 'System',
        @P_ColorScheme = 'Blue',
        @P_EvenRow = '#6699cc',
        @P_EvenFont = 'White',
        @P_OddRow = '#b6cbeb',
        @P_OddFont = 'Black',
        @P_HeaderRow = '#C7C7C7',
        @P_HeaderFont = 'Black',
        @P_Background = '#ffffff',
		@P_FavoriteAction = 'Home',
		@P_recordsPerPage = 5
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates ZGWCoreWeb.Account_Choices based on @P_ACCT
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/26/2024
-- Description: Updated to match [ZGWCoreWeb].[Account_Choices] changes
-- =============================================
CREATE PROCEDURE [ZGWCoreWeb].[Set_Account_Choices]
	@P_Account VARCHAR(128),
	@P_SecurityEntityId int,
	@P_SecurityEntityName VARCHAR(256),
    @P_ColorScheme VARCHAR(15),
    @P_EvenRow VARCHAR(15),
    @P_EvenFont VARCHAR(15),
    @P_OddRow VARCHAR(15),
    @P_OddFont VARCHAR(15),
    @P_HeaderRow VARCHAR(15),
    @P_HeaderFont VARCHAR(15),
    @P_Background VARCHAR(15),
	@P_FavoriteAction VARCHAR(50),
	@P_RecordsPerPage int
AS
-- INSERT a new row in the table.
	IF(SELECT COUNT(*) FROM [ZGWCoreWeb].[Account_Choices] WHERE Account = @P_Account) <= 0
		BEGIN
			INSERT [ZGWCoreWeb].[Account_Choices] (
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
			) VALUES (
                  @P_Account
                , @P_SecurityEntityId
                , @P_SecurityEntityName
                , @P_FavoriteAction
                , @P_RecordsPerPage
                , @P_ColorScheme
                , @P_EvenRow
                , @P_EvenFont
                , @P_OddRow
                , @P_OddFont
                , @P_HeaderRow
                , @P_HeaderFont
                , @P_Background
			);
		END
	ELSE
		BEGIN
		UPDATE [ZGWCoreWeb].[Account_Choices]
			SET
                  SecurityEntityId = @P_SecurityEntityId
                , SecurityEntityName = @P_SecurityEntityName
                , FavoriteAction = @P_FavoriteAction
                , RecordsPerPage = @P_RecordsPerPage
                , ColorScheme = @P_ColorScheme
                , EvenRow = @P_EvenRow
                , EvenFont = @P_EvenFont
                , OddRow = @P_OddRow
                , OddFont = @P_OddFont
                , HeaderRow = @P_HeaderRow
                , HeaderFont = @P_HeaderFont
                , Background = @P_Background
			WHERE
				Account=@P_Account;
		END
	-- END IF
-- Get the Error Code for the statement just executed.
--SELECT @P_ErrorCode=@@ERROR
GO
