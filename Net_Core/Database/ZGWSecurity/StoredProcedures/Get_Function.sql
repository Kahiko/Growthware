/*
Usage:

DECLARE 
	 @P_FunctionSeqId INT = 1
	,@P_SecurityEntitySeqId INT = 1
	,@P_Debug INT = 1

EXEC [ZGWSecurity].[Get_Function]
	 @P_FunctionSeqId
	,@P_SecurityEntitySeqId
	,@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/12/2011
-- Description:	Selects function given
--	the FunctionSeqId. When FunctionSeqId = -1
--	all rows in the table are retruned.
-- =============================================
-- Author:		Michael Regan
-- Create date: 05/26/2025
-- Description:	Now returns all of the needed data when getting "all" functions.
--	When FunctionSeqId = -1.
-- =============================================

CREATE PROCEDURE [ZGWSecurity].[Get_Function] 
	 @P_FunctionSeqId INT
	,@P_SecurityEntitySeqId INT
	,@P_Debug INT = 0
AS
SET NOCOUNT ON;
IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Function'

IF @P_FunctionSeqId <> - 1
	BEGIN
		-- SELECT an existing row from the table.
		IF @P_Debug = 1 PRINT 'Selecting single record'

		SELECT 
			 [Functions].[FunctionSeqId] AS [FUNCTION_SEQ_ID]
			,[Functions].[Name]
			,[Functions].[Description]
			,[Functions].[FunctionTypeSeqId] AS [FUNCTION_TYPE_SEQ_ID]
			,[Functions].[Source]
			,[Functions].[Controller]
			,[Functions].[Resolve]
			,[Functions].[Enable_View_State]
			,[Functions].[Enable_Notifications]
			,[Functions].[Redirect_On_Timeout]
			,[Functions].[Is_Nav]
			,[Functions].[Link_Behavior]
			,[Functions].[No_UI]
			,[Functions].[Navigation_Types_NVP_DetailSeqId] AS [NAVIGATION_NVP_SEQ_DET_ID]
			,[Functions].[Meta_Key_Words]
			,[Functions].[Action]
			,[Functions].[ParentSeqId] AS [PARENT_FUNCTION_SEQ_ID]
			,[Functions].[Notes]
			,[Functions].[Sort_Order]
			,[Functions].[Added_By]
			,[Functions].[Added_Date]
			,[Functions].[Updated_By]
			,[Functions].[Updated_Date]
		FROM [ZGWSecurity].[Functions] AS [Functions] WITH (NOLOCK)
		WHERE [Functions].[FunctionSeqId] = @P_FunctionSeqId
		ORDER BY [Functions].[Name] ASC
	END
ELSE
	BEGIN
		IF @P_Debug = 1 PRINT 'Selecting all records'

		EXEC [ZGWSecurity].[Get_Function_Security] @P_SecurityEntitySeqId, @P_Debug;
		SELECT 
			 [Functions].[FunctionSeqId] AS [FUNCTION_SEQ_ID]
			,[Functions].[Name]
			,[Functions].[Description]
			,[Functions].[FunctionTypeSeqId] AS [FUNCTION_TYPE_SEQ_ID]
			,[Functions].[Source]
			,[Functions].[Controller]
			,[Functions].[Resolve]
			,[Functions].[Enable_View_State]
			,[Functions].[Enable_Notifications]
			,[Functions].[Redirect_On_Timeout]
			,[Functions].[Is_Nav]
			,[Functions].[Link_Behavior]
			,[Functions].[No_UI]
			,[Functions].[Navigation_Types_NVP_DetailSeqId] AS [NAVIGATION_NVP_SEQ_DET_ID]
			,[Functions].[Meta_Key_Words]
			,[Functions].[Action]
			,[Functions].[ParentSeqId] AS [PARENT_FUNCTION_SEQ_ID]
			,[Functions].[Notes]
			,[Functions].[Sort_Order]
			,[Functions].[Added_By]
			,[Functions].[Added_Date]
			,[Functions].[Updated_By]
			,[Functions].[Updated_Date]
		FROM [ZGWSecurity].[Functions] AS [Functions] WITH (NOLOCK)
		ORDER BY [Functions].[Name] ASC;
	END
-- END IF
IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Function'

RETURN 0
GO