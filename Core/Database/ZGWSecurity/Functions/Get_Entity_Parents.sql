/*
Usage:
	DECLARE @P_SecurityEntitySeqId INT = 1
	SELECT SecurityEntitySeqId FROM ZGWSecurity.Get_Entity_Parents(0,@P_SecurityEntitySeqId)
	SELECT SecurityEntitySeqId FROM ZGWSecurity.Get_Entity_Parents(1,@P_SecurityEntitySeqId)
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description: Returns all "Parent" Secutriy_EntitySeqId's
--	Given the SecurityEntitySeqId 
-- Note:
--	Works through recursion of MAXRECURSION 32767
-- =============================================
CREATE FUNCTION [ZGWSecurity].[Get_Entity_Parents]
(
	@P_IncludeParent bit, 
	@P_SecurityEntitySeqId int
)
RETURNS @retParents TABLE 
(
	SecurityEntitySeqId int, 
	PARENTSecurityEntitySeqId int
)
AS
BEGIN
	IF (ZGWSystem.Inheritance_Enabled()=1)
		BEGIN
			;WITH tblParent(SecurityEntitySeqId, ParentSecurityEntitySeqId) AS
			(
				SELECT SecurityEntitySeqId, ParentSecurityEntitySeqId
					FROM ZGWSecurity.Security_Entities WITH(NOLOCK) WHERE SecurityEntitySeqId = @P_SecurityEntitySeqId
				UNION ALL
				SELECT 
					SE.SecurityEntitySeqId, SE.ParentSecurityEntitySeqId
				FROM 
					ZGWSecurity.Security_Entities SE WITH(NOLOCK) 
					INNER JOIN tblParent
						ON SE.SecurityEntitySeqId = tblParent.ParentSecurityEntitySeqId
			)
			INSERT INTO @retParents(SecurityEntitySeqId)
			SELECT 
				tblParent.SecurityEntitySeqId
			FROM  
				tblParent
			WHERE SecurityEntitySeqId <> @P_SecurityEntitySeqId
			OPTION(MAXRECURSION 32767)
			IF (@P_IncludeParent=1) INSERT INTO @retParents(SecurityEntitySeqId)VALUES(@P_SecurityEntitySeqId);
		END
	ELSE
		BEGIN
			INSERT INTO @retParents VALUES(ZGWSecurity.Get_Default_Entity_ID(),1)
			IF (@P_IncludeParent=1) INSERT INTO @retParents VALUES(@P_SecurityEntitySeqId,1)
		END
	-- END IF
	RETURN

END

GO

