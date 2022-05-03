
/*
Usage:
	EXEC ZGWCoreWeb.Set_Account_Choices
		@P_ACCT = N'Anonymous',
		@P_SE_SEQ_ID = 1,
		@P_SE_NAME = 'System',
		@P_Back_Color = '#ffffff',
		@P_Left_Color = '#eeeeee',
		@P_Head_Color = '#C7C7C7',
		@P_Header_ForeColor = 'Black',
		@P_Sub_Head_Color = '#b6cbeb',
		@P_Row_BackColor = '#b6cbeb',
		@P_AlternatingRow_BackColor = '#6699cc',
		@P_Color_Scheme = 'Blue',
		@P_Favorite_Action = 'Home',
		@P_Records_Per_Page = 5
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates ZGWCoreWeb.Account_Choices based on @P_ACCT
-- =============================================
CREATE PROCEDURE [ZGWCoreWeb].[Set_Account_Choices]
	@P_ACCT VARCHAR(128),
	@P_SE_SEQ_ID int,
	@P_SE_NAME VARCHAR(256),
	@P_Back_Color VARCHAR(15),
	@P_Left_Color VARCHAR(15),
	@P_Head_Color VARCHAR(15),
	@P_Header_ForeColor VARCHAR(15),
	@P_Sub_Head_Color VARCHAR(15),
	@P_Row_BackColor VARCHAR(15),
	@P_AlternatingRow_BackColor VARCHAR(15),
	@P_Color_Scheme VARCHAR(15),
	@P_Favorite_Action VARCHAR(50),
	@P_Records_Per_Page int
AS
-- INSERT a new row in the table.
	IF(SELECT COUNT(*) FROM ZGWCoreWeb.Account_Choices WHERE Account = @P_ACCT) <= 0
		BEGIN	
			INSERT ZGWCoreWeb.Account_Choices
			(
				Account,
				SE_SEQ_ID,
				SE_NAME,
				Back_Color,
				Left_Color,
				Head_Color,
				Header_ForeColor,
				Sub_Head_Color,
				Row_BackColor,
				AlternatingRow_BackColor,
				Color_Scheme,
				Favorite_Action,
				Records_Per_Page
			)
			VALUES
			(
				@P_ACCT,
				@P_SE_SEQ_ID,
				@P_SE_NAME,
				@P_Back_Color,
				@P_Left_Color,
				@P_Head_Color,
				@P_Header_ForeColor,
				@P_Sub_Head_Color,
				@P_Row_BackColor,
				@P_AlternatingRow_BackColor,
				@P_Color_Scheme,
				@P_Favorite_Action,
				@P_Records_Per_Page
			)
		END
	ELSE
		BEGIN
			UPDATE ZGWCoreWeb.Account_Choices
			SET
				SE_SEQ_ID = @P_SE_SEQ_ID,
				SE_NAME = @P_SE_NAME,
				Back_Color =@P_Back_Color ,
				Left_Color=@P_Left_Color,
				Head_Color=@P_Head_Color,
				Header_ForeColor=@P_Header_ForeColor,
				Sub_Head_Color=@P_Sub_Head_Color,
				Row_BackColor=@P_Row_BackColor,
				AlternatingRow_BackColor=@P_AlternatingRow_BackColor,
				Color_Scheme=@P_Color_Scheme,
				Favorite_Action=@P_Favorite_Action,
				Records_Per_Page=@P_Records_Per_Page
			WHERE
				Account=@P_ACCT
		END
	-- END IF
-- Get the Error Code for the statement just executed.
--SELECT @P_ErrorCode=@@ERROR

GO

