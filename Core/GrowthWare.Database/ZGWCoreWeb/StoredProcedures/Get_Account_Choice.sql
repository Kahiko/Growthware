
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
CREATE PROCEDURE [ZGWCoreWeb].[Get_Account_Choice]
	@P_Account VARCHAR(128),
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF EXISTS(SELECT Account FROM ZGWCoreWeb.Account_Choices WHERE Account = @P_Account)
		BEGIN
			IF @P_Debug = 1 PRINT 'Selecting client choices for ' + CONVERT(VARCHAR(25),@P_Account)
			SELECT
				Account AS ACCT
				, SecurityEntityID
				, SecurityEntityName
				, BackColor
				, LeftColor
				, Head_Color
				, Header_ForeColor
				, Sub_Head_Color
				, Row_BackColor
				, AlternatingRow_BackColor
				, Color_Scheme
				, Favorite_Action
				, Records_Per_Page
			FROM ZGWCoreWeb.Account_Choices
			WHERE
				Account = @P_Account
		END
	ELSE
		BEGIN
			IF @P_Debug = 1 PRINT 'Selecting client choices for the Anonymous account'
			SELECT
				Account AS ACCT
				, SecurityEntityID
				, SecurityEntityName
				, BackColor
				, LeftColor
				, Head_Color
				, Header_ForeColor
				, Sub_Head_Color
				, Row_BackColor
				, AlternatingRow_BackColor
				, COLOR_SCHEME
				, Favorite_Action
				, Records_Per_Page
			FROM ZGWCoreWeb.Account_Choices
			WHERE
				[Account] = 'Anonymous'
		END

RETURN 0

GO

