
/*
Usage:
	EXEC ZGWCoreWeb.Set_Account_Choices
		@P_ACCT = N'Anonymous',
		@P_SecurityEntityID = 1,
		@P_SecurityEntityName = 'System',
		@P_BackColor = '#ffffff',
		@P_LeftColor = '#eeeeee',
		@P_HeadColor = '#C7C7C7',
		@P_Header_ForeColor = 'Black',
		@P_SubHeadColor = '#b6cbeb',
		@P_RowBackColor = '#b6cbeb',
		@P_AlternatingRowBackColor = '#6699cc',
		@P_ColorScheme = 'Blue',
		@P_FavoriteAction = 'Home',
		@P_recordsPerPage = 5
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates ZGWCoreWeb.Account_Choices based on @P_ACCT
-- =============================================
CREATE PROCEDURE [ZGWCoreWeb].[Set_Account_Choices]
	@P_ACCT VARCHAR(128),
	@P_SecurityEntityID int,
	@P_SecurityEntityName VARCHAR(256),
	@P_BackColor VARCHAR(15),
	@P_LeftColor VARCHAR(15),
	@P_HeadColor VARCHAR(15),
	@P_Header_ForeColor VARCHAR(15),
	@P_SubHeadColor VARCHAR(15),
	@P_RowBackColor VARCHAR(15),
	@P_AlternatingRowBackColor VARCHAR(15),
	@P_ColorScheme VARCHAR(15),
	@P_FavoriteAction VARCHAR(50),
	@P_recordsPerPage int
AS
-- INSERT a new row in the table.
	IF(SELECT COUNT(*) FROM ZGWCoreWeb.Account_Choices WHERE Account = @P_ACCT) <= 0
		BEGIN	
			INSERT ZGWCoreWeb.Account_Choices
			(
				Account,
				SecurityEntityID,
				SecurityEntityName,
				BackColor,
				LeftColor,
				HeadColor,
				Header_ForeColor,
				SubHeadColor,
				RowBackColor,
				AlternatingRowBackColor,
				ColorScheme,
				FavoriteAction,
				recordsPerPage
			)
			VALUES
			(
				@P_ACCT,
				@P_SecurityEntityID,
				@P_SecurityEntityName,
				@P_BackColor,
				@P_LeftColor,
				@P_HeadColor,
				@P_Header_ForeColor,
				@P_SubHeadColor,
				@P_RowBackColor,
				@P_AlternatingRowBackColor,
				@P_ColorScheme,
				@P_FavoriteAction,
				@P_recordsPerPage
			)
		END
	ELSE
		BEGIN
			UPDATE ZGWCoreWeb.Account_Choices
			SET
				SecurityEntityID = @P_SecurityEntityID,
				SecurityEntityName = @P_SecurityEntityName,
				BackColor =@P_BackColor ,
				LeftColor=@P_LeftColor,
				HeadColor=@P_HeadColor,
				Header_ForeColor=@P_Header_ForeColor,
				SubHeadColor=@P_SubHeadColor,
				RowBackColor=@P_RowBackColor,
				AlternatingRowBackColor=@P_AlternatingRowBackColor,
				ColorScheme=@P_ColorScheme,
				FavoriteAction=@P_FavoriteAction,
				recordsPerPage=@P_recordsPerPage
			WHERE
				Account=@P_ACCT
		END
	-- END IF
-- Get the Error Code for the statement just executed.
--SELECT @P_ErrorCode=@@ERROR

GO

