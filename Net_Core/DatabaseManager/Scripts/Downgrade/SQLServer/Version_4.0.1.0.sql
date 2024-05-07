-- Downgrade script for version 4.0.1.0
DECLARE 
	@V_FunctionSeqId int,
	@V_MyAction VARCHAR(256) = '/accounts/forgot-password',
	@V_ErrorCode int;

SET @V_FunctionSeqId = (SELECT FunctionSeqId from ZGWSecurity.Functions where action=@V_MyAction);
EXEC ZGWSecurity.Delete_Function
	@V_FunctionSeqId ,
	@V_ErrorCode;

/****** Start: Procedure [ZGWSecurity].[Get_Function_Sort] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'[ZGWSecurity].[Get_Function_Sort]') AND type in (N'P', N'PC'))
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
ALTER PROCEDURE [ZGWSecurity].[Get_Function_Sort]
	@P_FunctionSeqId INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	DECLARE @V_Parent_ID INT
	DECLARE @V_NAV_TYPE_ID INT
	SET @V_Parent_ID = (SELECT ParentSeqId
FROM ZGWSecurity.Functions
WHERE FunctionSeqId = @P_FunctionSeqId)
	SET @V_NAV_TYPE_ID = (SELECT Navigation_Types_NVP_DetailSeqId
FROM ZGWSecurity.Functions
WHERE FunctionSeqId = @P_FunctionSeqId)
	SELECT
	FunctionSeqId as FUNCTION_SEQ_ID,
	[Name],
	[Action],
	Sort_Order
FROM
	ZGWSecurity.Functions WITH(NOLOCK)
WHERE
		ParentSeqId = @V_PARENT_ID
	AND Is_Nav = 1
	AND Navigation_Types_NVP_DetailSeqId = @V_NAV_TYPE_ID
	AND ParentSeqId <> 1
ORDER BY
		Sort_Order ASC

RETURN 0

GO
/****** End: Procedure [ZGWSecurity].[Get_Function_Sort] ******/

-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '4.0.0.0',
    [Updated_By] = 3,
    [Updated_Date] = getdate()