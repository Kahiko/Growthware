
/*
Usage:
	DECLARE 
		@P_FunctionSeqId int = -1,
		@P_Name VARCHAR(30) = 'Testing',
		@P_Description VARCHAR(512) = 'Testing',
		@P_FunctionTypeSeqId INT = 1,
		@P_Source VARCHAR(512) = '',
		@P_Controller VARCHAR(512) = '',
		@P_Enable_View_State int = 0,
		@P_Enable_Notifications int = 0,
		@P_Redirect_On_Timeout int = 0,
		@P_Is_Nav int = 0,
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
		@P_FunctionSeqId,
		@P_Name,
		@P_Description,
		@P_FunctionTypeSeqId,
		@P_Source,
		@P_Controller,
		@P_Enable_View_State,
		@P_Enable_Notifications,
		@P_Redirect_On_Timeout,
		@P_Is_Nav,
		@P_Link_Behavior,
		@P_NO_UI,
		@P_NAV_TYPE_ID,
		@P_Action,
		@P_Meta_Key_Words,
		@P_Parent_SeqID,
		@P_Notes,
		@P_Added_Updated_By
		@P_Debug
		
	PRINT 'Primary_Key = ' + CONVERT(VARCHAR(MAX),@P_FunctionSeqId)
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/25/2011
-- Description:	Inserts or updates ZGWSecurity.Functions
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Function]
	@P_FunctionSeqId int OUTPUT,
	@P_Name VARCHAR(30),
	@P_Description VARCHAR(512),
	@P_FunctionTypeSeqId INT,
	@P_Source VARCHAR(512),
	@P_Controller VARCHAR(512) = NULL,
	@P_Resolve VARCHAR(MAX) = NULL,
	@P_Enable_View_State int,
	@P_Enable_Notifications int,
	@P_Redirect_On_Timeout int,
	@P_Is_Nav int,
	@P_Link_Behavior int,
	@P_NO_UI int,
	@P_NAV_TYPE_ID int,
	@P_Action VARCHAR(256),
	@P_Meta_Key_Words VARCHAR(512),
	@P_Parent_SeqID int,
	@P_Notes VARCHAR(512),
	@P_Added_Updated_By INT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Function'
	DECLARE @V_Now DATETIME = GETDATE()
	IF @P_FunctionSeqId > -1
		BEGIN -- UPDATE PROFILE
			UPDATE ZGWSecurity.Functions
			SET 
				[Name] = @P_Name,
				[Description] = @P_Description,
				FunctionTypeSeqId = @P_FunctionTypeSeqId,
				[Source] = @P_Source,
				[Controller] = @P_Controller,
				[Resolve] = @P_Resolve,
				Enable_View_State = @P_Enable_View_State,
				Enable_Notifications = @P_Enable_Notifications,
				Redirect_On_Timeout = @P_Redirect_On_Timeout,
				Is_Nav = @P_Is_Nav,
				Link_Behavior = @P_Link_Behavior,
				No_UI = @P_NO_UI,
				Navigation_Types_NVP_Detail_SeqID = @P_NAV_TYPE_ID,
				Meta_Key_Words = @P_Meta_Key_Words,
				Parent_SeqID = @P_Parent_SeqID,
				Notes = @P_Notes,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = @V_Now
			WHERE
				FunctionSeqId = @P_FunctionSeqId
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
				FunctionTypeSeqId,
				[Source],
				[Controller],
				[Resolve],
				Enable_View_State,
				Enable_Notifications,
				Redirect_On_Timeout,
				Is_Nav,
				Link_Behavior,
				NO_UI,
				Navigation_Types_NVP_Detail_SeqID,
				Meta_Key_Words,
				[Action],
				Parent_SeqID,
				Notes,
				Sort_Order,
				Added_By,
				Added_Date
			)
			VALUES
			(
				@P_Name,
				@P_Description,
				@P_FunctionTypeSeqId,
				@P_Source,
				@P_Controller,
				@P_Resolve,
				@P_Enable_View_State,
				@P_Enable_Notifications,
				@P_Redirect_On_Timeout,
				@P_Is_Nav,
				@P_Link_Behavior,
				@P_NO_UI,
				@P_NAV_TYPE_ID,
				@P_Meta_Key_Words,
				@P_Action,
				@P_Parent_SeqID,
				@P_Notes,
				0,
				@P_Added_Updated_By,
				@V_Now
			)
			SELECT @P_FunctionSeqId=SCOPE_IDENTITY() -- Get the IDENTITY value for the row just inserted.
			DECLARE @V_Sort_Order INT
			SET @V_Sort_Order = (SELECT MAX(Sort_Order) FROM ZGWSecurity.Functions WHERE Parent_SeqID = @P_Parent_SeqID) + 1
			UPDATE ZGWSecurity.Functions SET Sort_Order = ISNULL(@V_Sort_Order,0) WHERE FunctionSeqId = @P_FunctionSeqId

		END
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Set_Function'
RETURN 0

GO

