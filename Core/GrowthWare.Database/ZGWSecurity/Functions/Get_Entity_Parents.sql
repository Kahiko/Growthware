/*
Usage:
	DECLARE @P_Security_Entity_SeqID INT = 1
	SELECT Security_Entity_SeqID FROM ZGWSecurity.Get_Entity_Parents(0,@P_Security_Entity_SeqID)
	SELECT Security_Entity_SeqID FROM ZGWSecurity.Get_Entity_Parents(1,@P_Security_Entity_SeqID)
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/18/2011
-- Description: Returns all "Parent" Secutriy_Entity_SeqID's
--	Given the Security_Entity_SeqID 
-- Note:
--	Works through recursion of MAXRECURSION 32767
-- =============================================
CREATE FUNCTION [ZGWSecurity].[Get_Entity_Parents]
(
	@P_IncludeParent bit, 
	@P_Security_Entity_SeqID int
)
RETURNS @retParents TABLE 
(
	Security_Entity_SeqID int, 
	PARENT_Security_Entity_SeqID int
)
AS
BEGIN
	IF (ZGWSystem.Inheritance_Enabled()=1)
		BEGIN
			;WITH tblParent(Security_Entity_SeqID, Parent_Security_Entity_SeqID) AS
			(
				SELECT Security_Entity_SeqID, Parent_Security_Entity_SeqID
					FROM ZGWSecurity.Security_Entities WITH(NOLOCK) WHERE Security_Entity_SeqID = @P_Security_Entity_SeqID
				UNION ALL
				SELECT 
					SE.Security_Entity_SeqID, SE.Parent_Security_Entity_SeqID
				FROM 
					ZGWSecurity.Security_Entities SE WITH(NOLOCK) 
					INNER JOIN tblParent
						ON SE.Security_Entity_SeqID = tblParent.Parent_Security_Entity_SeqID
			)
			INSERT INTO @retParents(Security_Entity_SeqID)
			SELECT 
				tblParent.Security_Entity_SeqID
			FROM  
				tblParent
			WHERE Security_Entity_SeqID <> @P_Security_Entity_SeqID
			OPTION(MAXRECURSION 32767)
			IF (@P_IncludeParent=1) INSERT INTO @retParents(Security_Entity_SeqID)VALUES(@P_Security_Entity_SeqID);
		END
	ELSE
		BEGIN
			INSERT INTO @retParents VALUES(ZGWSecurity.Get_Default_Entity_ID(),1)
			IF (@P_IncludeParent=1) INSERT INTO @retParents VALUES(@P_Security_Entity_SeqID,1)
		END
	-- END IF
	RETURN

END

GO

