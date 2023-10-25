/*
Usage:
	DECLARE 
        @P_Source INT = 1
	  , @P_Target INT = 10
	  , @P_Added_Updated_By INT = 4;

	EXEC [ZGWSecurity].[Copy_Function_Security]
        @P_Source
	  , @P_Target
	  , @P_Added_Updated_By
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 10/22/2023
-- Description:	"Copies the group and role security for all functions"
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Copy_Function_Security]
      @P_Source INT
    , @P_Target INT
    , @P_Added_Updated_By INT
    , @P_Debug INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    -- Delete any Roles or Groups associated with the target (rely's on FK settings to delete from subsequent tables
    DELETE FROM [ZGWSecurity].[Roles_Security_Entities] WHERE [SecurityEntitySeqId] =  @p_Target
    DELETE FROM [ZGWSecurity].[Groups_Security_Entities] WHERE [SecurityEntitySeqId] =  @p_Target
    -- Insert the new values for the target
	INSERT INTO [ZGWSecurity].[Roles_Security_Entities]
	SELECT 
		  @P_Target
		, [RoleSeqId]
		, [Added_By]
		, GETDATE()
	FROM [ZGWSecurity].[Roles_Security_Entities] WITH(NOLOCK)
	WHERE [SecurityEntitySeqId] = @P_Source
END
RETURN 0

GO