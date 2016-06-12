
CREATE FUNCTION [ZFF_GET_SE_PARENTS](@P_IncludeParent bit, @P_SE_SEQ_ID int)
RETURNS @retParents TABLE (SE_SEQ_ID int, PARENT_SE_SEQ_ID int)
AS
BEGIN
	IF (dbo.ZFF_ENABLE_INHERITANCE()=1)
		BEGIN
			IF (@P_IncludeParent=1)
				BEGIN
					INSERT INTO @retParents
						SELECT SE_SEQ_ID, Parent_SE_SEQ_ID
						FROM ZFC_SECURITY_ENTITIES WHERE SE_SEQ_ID=@P_SE_SEQ_ID
				END
			-- END IF
			IF (@P_SE_SEQ_ID=0 or @P_SE_SEQ_ID=dbo.ZFF_GET_DEFAULT_Security_Entity_ID()) RETURN
			DECLARE @Report_ID int, @Report_ParentID int
			DECLARE RetrieveReports CURSOR STATIC LOCAL FOR
				SELECT  SE_SEQ_ID,  Parent_SE_SEQ_ID
				FROM ZFC_SECURITY_ENTITIES WHERE SE_SEQ_ID=@P_SE_SEQ_ID
			OPEN RetrieveReports
				FETCH NEXT FROM RetrieveReports
				INTO @Report_ID,  @Report_ParentID
				WHILE (@@FETCH_STATUS = 0)
					BEGIN
						INSERT INTO @retParents SELECT * FROM dbo.ZFF_GET_SE_PARENTS(1, @Report_ParentID)
						FETCH NEXT FROM RetrieveReports INTO @Report_ID, @Report_ParentID
					END
			CLOSE RetrieveReports
			DEALLOCATE RetrieveReports
			RETURN
		END
	ELSE
		BEGIN
			INSERT INTO @retParents VALUES(dbo.ZFF_GET_DEFAULT_Security_Entity_ID(),1)
			INSERT INTO @retParents VALUES(@P_SE_SEQ_ID,1)
		END
	-- END IF
RETURN
END

