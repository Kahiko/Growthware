/*
Usage:
	DECLARE 
		@P_Function_SeqID INT = 1
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Function
		@P_Function_SeqID
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/12/2011
-- Description:	Selects function given
--	the Function_SeqID. When Function_SeqID = -1
--	all rows in the table are retruned.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Function]
	@P_Function_SeqID int,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Function'
	IF @P_Function_SeqID <> -1
		BEGIN -- SELECT an existing row from the table.
			IF @P_Debug = 1 PRINT 'Selecting single record'
			SELECT
				Function_SeqID AS FUNCTION_SEQ_ID
				, [Name]
				, [Description]
				, Function_Type_SeqID AS FUNCTION_TYPE_SEQ_ID
				, [Source]
				, [Controller]
				, Enable_View_State
				, Enable_Notifications
				, Redirect_On_Timeout
				, Is_Nav
				, Link_Behavior
				, No_UI
				, Navigation_Types_NVP_Detail_SeqID AS NAVIGATION_NVP_SEQ_DET_ID
				, Meta_Key_Words
				, [Action]
				, Parent_SeqID AS PARENT_FUNCTION_SEQ_ID
				, Notes
				, Sort_Order
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
			FROM
				ZGWSecurity.Functions WITH(NOLOCK)
			WHERE
				Function_SeqID = @P_Function_SeqID
			ORDER BY 
				[Name] ASC
		END
	ELSE
		BEGIN
			IF @P_Debug = 1 PRINT 'Selecting all records'
			SELECT
				Function_SeqID AS FUNCTION_SEQ_ID
				, [Name]
				, [Description]
				, Function_Type_SeqID AS FUNCTION_TYPE_SEQ_ID
				, [Source]
				, [Controller]
				, Enable_View_State
				, Enable_Notifications
				, Redirect_On_Timeout
				, Is_Nav
				, Link_Behavior
				, No_UI
				, Navigation_Types_NVP_Detail_SeqID AS NAVIGATION_NVP_SEQ_DET_ID
				, Meta_Key_Words
				, [Action]
				, Parent_SeqID AS PARENT_FUNCTION_SEQ_ID
				, Notes
				, Sort_Order
				, Added_By
				, Added_Date
				, Updated_By
				, Updated_Date
			FROM
				ZGWSecurity.Functions WITH(NOLOCK)
			ORDER BY 
				[Name] ASC
		END
	-- END IF
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Function'

RETURN 0