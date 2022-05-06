
/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId INT = 1,
		@P_FunctionSeqId INT = 1,
		@P_Permissions_SeqID INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Get_Function_Groups
		@PSecurityEntitySeqId,
		@P_FunctionSeqId,
		@P_Permissions_SeqID,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/12/2011
-- Description:	Selects groups given the security entity
--	function and permission.
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Get_Function_Groups]
	@PSecurityEntitySeqId INT,
	@P_FunctionSeqId INT,
	@P_Permissions_SeqID INT,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Get_Function_Groups'
	IF @P_FunctionSeqId > 0
		BEGIN
			SELECT
				ZGWSecurity.Groups.[Name] AS Groups
			FROM
				ZGWSecurity.Functions WITH(NOLOCK),
				ZGWSecurity.Groups_Security_Entities_Functions WITH(NOLOCK),
				ZGWSecurity.Groups_Security_Entities WITH(NOLOCK),
				ZGWSecurity.Groups WITH(NOLOCK)
			WHERE
				ZGWSecurity.Functions.FunctionSeqId = @P_FunctionSeqId
				AND ZGWSecurity.Functions.FunctionSeqId = ZGWSecurity.Groups_Security_Entities_Functions.FunctionSeqId
				AND ZGWSecurity.Groups_Security_Entities_Functions.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID
				AND ZGWSecurity.Groups_Security_Entities_Functions.Permissions_NVP_Detail_SeqID = @P_Permissions_SeqID
				AND ZGWSecurity.Groups_Security_Entities.GroupSeqId = ZGWSecurity.Groups.GroupSeqId
				AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId = @PSecurityEntitySeqId
			ORDER BY
				Groups
		END
	ELSE
		BEGIN
			SELECT
				ZGWSecurity.Functions.FunctionSeqId AS 'FUNCTION_SEQ_ID'
				,ZGWSecurity.Groups_Security_Entities_Functions.Permissions_NVP_Detail_SeqID AS 'PERMISSIONS_SEQ_ID'
				,ZGWSecurity.Groups.[Name] AS [Group]
			FROM
				ZGWSecurity.Functions WITH(NOLOCK),
				ZGWSecurity.Groups_Security_Entities_Functions WITH(NOLOCK),
				ZGWSecurity.Groups_Security_Entities WITH(NOLOCK),
				ZGWSecurity.Groups WITH(NOLOCK)
			WHERE
				ZGWSecurity.Functions.FunctionSeqId = ZGWSecurity.Groups_Security_Entities_Functions.FunctionSeqId
				AND ZGWSecurity.Groups_Security_Entities_Functions.Groups_Security_Entities_SeqID = ZGWSecurity.Groups_Security_Entities.Groups_Security_Entities_SeqID
				AND ZGWSecurity.Groups_Security_Entities.GroupSeqId = ZGWSecurity.Groups.GroupSeqId
				AND ZGWSecurity.Groups_Security_Entities.SecurityEntitySeqId = @PSecurityEntitySeqId
			ORDER BY
				FUNCTION_SEQ_ID
				, PERMISSIONS_SEQ_ID
				, [Group]
		END
	--END IF
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Get_Function_Groups'

RETURN 0

GO

