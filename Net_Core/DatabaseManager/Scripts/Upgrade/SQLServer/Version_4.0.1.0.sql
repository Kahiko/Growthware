-- Upgrade script for version 4.0.1.0
SET NOCOUNT OFF;
Print 'Adding Forgot Password'
IF NOT EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [Action] = '/accounts/forgot-password')
	BEGIN
		DECLARE 
			@V_FunctionSeqId int = -1,
			@V_Name VARCHAR(30) = 'Forgot Password',
			@V_Description VARCHAR(512) = 'Forgot Password',
			@V_FunctionTypeSeqId INT = 1,
			@V_Source VARCHAR(512) = '',
			@V_Controller VARCHAR(512) = '',
			@V_Resolve VARCHAR(MAX) = '',
			@V_Enable_View_State int = 0,
			@V_Enable_Notifications int = 0,
			@V_Redirect_On_Timeout int = 0,
			@V_Is_Nav int = 0,
			@V_Link_Behavior int = 1,
			@V_NO_UI int = 0,
			@V_NAV_TYPE_ID int = 1,
			@V_Action VARCHAR(256) = '/accounts/forgot-password',
			@V_Meta_Key_Words VARCHAR(512) = '',
			@V_ParentSeqId int = 1,
			@V_Notes VARCHAR(512) = '',
			@V_Debug INT = 0,
			@V_SystemID INT = 1,
			@V_ViewPermission INT;

		EXEC ZGWSecurity.Set_Function
			@V_FunctionSeqId,
			@V_Name,
			@V_Description,
			@V_FunctionTypeSeqId,
			@V_Source,
			@V_Controller,
			@V_Resolve,
			@V_Enable_View_State,
			@V_Enable_Notifications,
			@V_Redirect_On_Timeout,
			@V_Is_Nav,
			@V_Link_Behavior,
			@V_NO_UI,
			@V_NAV_TYPE_ID,
			@V_Action,
			@V_Meta_Key_Words,
			@V_ParentSeqId,
			@V_Notes,
			@V_SystemID,
			@V_Debug;

		SET @V_FunctionSeqId = (SELECT FunctionSeqId from ZGWSecurity.Functions where action=@V_Action);
		SET @V_ViewPermission = (SELECT NVP_DetailSeqId FROM ZGWSecurity.Permissions WHERE NVP_Detail_Value = 'View')
		EXEC ZGWSecurity.Set_Function_Roles @V_FunctionSeqId, 1, 'Anonymous', @V_ViewPermission, @V_SystemID, @V_Debug;
	END
--END IF
/****** Start: Procedure [ZGWSecurity].[Get_Function_Sort] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'ZGWSecurity.Get_Function_Sort') AND type in (N'P', N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Function_Sort] AS'
	END
--End If
GO
/*
Usage:
	DECLARE 
		@P_FunctionSeqId INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Function_Sort
		@P_FunctionSeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/15/2011
-- Description:	Returns sorted function information
--	for related functions given the funtionSeqId
-- =============================================
-- Author:		Michael Regan
-- Create date: 05/03/2024
-- Description:	Fixed but where nothing is returned if the ParentSeqId <> 1
--  this should have been ParentSeqId = @V_Parent_ID
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Function_Sort]
	@P_FunctionSeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON;

	DECLARE @V_Parent_ID INT
			, @V_NAV_TYPE_ID INT;

	SET @V_Parent_ID = (
			SELECT ParentSeqId
			FROM ZGWSecurity.Functions
			WHERE FunctionSeqId = @P_FunctionSeqId
		);
	SET @V_NAV_TYPE_ID = (
			SELECT Navigation_Types_NVP_DetailSeqId
			FROM ZGWSecurity.Functions
			WHERE FunctionSeqId = @P_FunctionSeqId
		);
	SELECT 
		  FunctionSeqId AS FUNCTION_SEQ_ID
		, [Name]
		, [Action]
		, Sort_Order
	FROM ZGWSecurity.Functions WITH (NOLOCK)
	WHERE ParentSeqId = @V_PARENT_ID
		AND Is_Nav = 1
		AND Navigation_Types_NVP_DetailSeqId = @V_NAV_TYPE_ID
		AND ParentSeqId = @V_Parent_ID
	ORDER BY Sort_Order ASC;

SET NOCOUNT OFF;

GO

/****** End: Procedure [ZGWSecurity].[Get_Function_Sort] ******/

-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '4.0.1.0',
    [Updated_By] = 3,
    [Updated_Date] = getdate();

SET NOCOUNT ON