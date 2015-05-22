SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
Usage:
	DECLARE 
		@P_Function_SeqID int = -1,
		@P_Name VARCHAR(30) = 'Testing',
		@P_Description VARCHAR(512) = 'Testing',
		@P_Function_Type_SeqID INT = 1,
		@P_Source VARCHAR(512) = '',
		@P_Enable_View_State int = 0,
		@P_Enable_Notifications int = 0,
		@P_Redirect_On_Timeout int = 0,
		@P_IS_NAV int = 0,
		@P_Link_Behavior int 0,
		@P_NO_UI int = 0,
		@P_NAV_TYPE_ID int = 1,
		@P_Action VARCHAR(256) = 'testing',
		@P_Meta_Key_Words VARCHAR(512) = '',
		@P_Parent_SeqID int = 1,
		@P_Notes VARCHAR(512) = '',
		@P_Added_Updated_By INT = 1
		@P_Debug INT = 0

	exec ZGWSecurity.Set_Function
		@P_Function_SeqID,
		@P_Name,
		@P_Description,
		@P_Function_Type_SeqID,
		@P_Source,
		@P_Enable_View_State,
		@P_Enable_Notifications,
		@P_Redirect_On_Timeout,
		@P_IS_NAV,
		@P_Link_Behavior,
		@P_NO_UI,
		@P_NAV_TYPE_ID,
		@P_Action,
		@P_Meta_Key_Words,
		@P_Parent_SeqID,
		@P_Notes,
		@P_Added_Updated_By
		@P_Debug
		
	PRINT 'Primary_Key = ' + CONVERT(VARCHAR(MAX),@P_Function_SeqID)
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/25/2011
-- Description:	Inserts or updates ZGWSecurity.Functions
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Function]
	@P_Function_SeqID int OUTPUT,
	@P_Name VARCHAR(30),
	@P_Description VARCHAR(512),
	@P_Function_Type_SeqID INT,
	@P_Source VARCHAR(512),
	@P_Enable_View_State int,
	@P_Enable_Notifications int,
	@P_Redirect_On_Timeout int,
	@P_IS_NAV int,
	@P_Link_Behavior int,
	@P_NO_UI int,
	@P_NAV_TYPE_ID int,
	@P_Action VARCHAR(256),
	@P_Meta_Key_Words VARCHAR(512),
	@P_Parent_SeqID int,
	@P_Notes VARCHAR(512),
	@P_Added_Updated_By INT,
	@P_Debug INT = 0,
	@P_Controller VARCHAR(512) = null
AS
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Function'
	DECLARE @V_Now DATETIME = GETDATE()
	IF @P_Function_SeqID > -1
		BEGIN -- UPDATE PROFILE
			UPDATE ZGWSecurity.Functions
			SET 
				[Name] = @P_Name,
				[Description] = @P_Description,
				Function_Type_SeqID = @P_Function_Type_SeqID,
				[Source] = @P_Source,
				Enable_View_State = @P_Enable_View_State,
				Enable_Notifications = @P_Enable_Notifications,
				Redirect_On_Timeout = @P_Redirect_On_Timeout,
				IS_NAV = @P_IS_NAV,
				Link_Behavior = @P_Link_Behavior,
				NO_UI = @P_NO_UI,
				Navigation_Types_NVP_Detail_SeqID = @P_NAV_TYPE_ID,
				Meta_Key_Words = @P_Meta_Key_Words,
				Parent_SeqID = @P_Parent_SeqID,
				Notes = @P_Notes,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = @V_Now
			WHERE
				Function_SeqID = @P_Function_SeqID
		END
	ELSE
		BEGIN
			IF @P_Debug = 1 PRINT 'Inserting new row'
			IF EXISTS( SELECT [Action]
				   FROM ZGWSecurity.Functions
				   WHERE [Action] = @P_Action
			)
			BEGIN
				RAISERROR ('THE FUNCTION YOU ENTERED ALREADY EXISTS IN THE DATABASE.',16,1)
				RETURN
			END
			INSERT ZGWSecurity.Functions
			(
				[Name],
				[Description],
				Function_Type_SeqID,
				[Source],
				Enable_View_State,
				Enable_Notifications,
				Redirect_On_Timeout,
				IS_NAV,
				Link_Behavior,
				NO_UI,
				Navigation_Types_NVP_Detail_SeqID,
				Meta_Key_Words,
				[Action],
				Parent_SeqID,
				Notes,
				ADDED_BY,
				ADDED_DATE
			)
			VALUES
			(
				@P_Name,
				@P_Description,
				@P_Function_Type_SeqID,
				@P_Source,
				@P_Enable_View_State,
				@P_Enable_Notifications,
				@P_Redirect_On_Timeout,
				@P_IS_NAV,
				@P_Link_Behavior,
				@P_NO_UI,
				@P_NAV_TYPE_ID,
				@P_Meta_Key_Words,
				@P_Action,
				@P_Parent_SeqID,
				@P_Notes,
				@P_Added_Updated_By,
				@V_Now
			)
			SELECT @P_Function_SeqID=SCOPE_IDENTITY() -- Get the IDENTITY value for the row just inserted.
			DECLARE @V_SORT_ORDER INT
			SET @V_SORT_ORDER = (SELECT MAX(SORT_ORDER) FROM ZGWSecurity.Functions WHERE Parent_SeqID = @P_Parent_SeqID) + 1
			UPDATE ZGWSecurity.Functions SET SORT_ORDER = @V_SORT_ORDER WHERE Function_SeqID = @P_Function_SeqID

		END
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Set_Function'
RETURN 0


UPDATE ZGWSecurity.Functions SET
	[Source] = 'Functions/System/Accounts/Logon.aspx'
WHERE
	[Action] = 'Logon'

exec [ZGWSystem].[Set_System_Status] -1,'SetAccountDetails','Please enter your account details',@V_Added_Updated_By,@V_PRIMARY_KEY,@V_ErrorCode