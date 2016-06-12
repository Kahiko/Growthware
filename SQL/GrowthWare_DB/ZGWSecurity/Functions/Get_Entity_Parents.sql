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
--	Works through recursion and should be changed
--	to use CTE, however due to constratins i'm leaving
--	this close to the orginial and will attempt
--	to change this later after the bulk of development
--	has been completed.  (if i remember)
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
			IF (@P_IncludeParent=1)
				BEGIN
					INSERT INTO @retParents
						SELECT Security_Entity_SeqID, Parent_Security_Entity_SeqID
						FROM ZGWSecurity.Security_Entities WHERE Security_Entity_SeqID=@P_Security_Entity_SeqID
				END
			-- END IF
			IF (@P_Security_Entity_SeqID=0 or @P_Security_Entity_SeqID=ZGWSecurity.Get_Default_Entity_ID()) RETURN
			DECLARE @Report_ID int, @Report_ParentID int
			DECLARE RetrieveReports CURSOR STATIC LOCAL FOR
				SELECT  Security_Entity_SeqID,  Parent_Security_Entity_SeqID
				FROM ZGWSecurity.Security_Entities WHERE Security_Entity_SeqID=@P_Security_Entity_SeqID
			OPEN RetrieveReports
				FETCH NEXT FROM RetrieveReports
				INTO @Report_ID,  @Report_ParentID
				WHILE (@@FETCH_STATUS = 0)
					BEGIN
						INSERT INTO @retParents SELECT * FROM ZGWSecurity.Get_Entity_Parents(1, @Report_ParentID)
						FETCH NEXT FROM RetrieveReports INTO @Report_ID, @Report_ParentID
					END
			CLOSE RetrieveReports
			DEALLOCATE RetrieveReports
			RETURN
		END
	ELSE
		BEGIN
			INSERT INTO @retParents VALUES(ZGWSecurity.Get_Default_Entity_ID(),1)
			INSERT INTO @retParents VALUES(@P_Security_Entity_SeqID,1)
		END
	-- END IF
RETURN

END