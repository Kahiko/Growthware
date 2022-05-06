
/*
Usage:
	DECLARE 
		@P_GroupSeqId INT = 1,
		@PSecurityEntitySeqId INT = 1,
		@P_Roles VARCHAR(MAX) = 'Anonymous, Authenticated',
		@P_Added_Updated_By INT = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Group_Roles
		@P_GroupSeqId,
		@PSecurityEntitySeqId,
		@P_Roles,
		@P_Added_Updated_By,
		@P_Debug

*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/02/2011
-- Description:	Deletes and inserts ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Group_Roles]
		@P_GroupSeqId INT,
		@PSecurityEntitySeqId INT,
		@P_Roles VARCHAR(MAX),
		@P_Added_Updated_By INT,
		@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Group_Roles'
	DECLARE @V_Roles_Security_Entities_SeqID INT
			,@V_Role_Name VARCHAR(50)
			,@V_Is_System INT
			,@V_POS INT
			,@V_Groups_Security_Entities_SeqID INT
			,@V_Now DATETIME = GETDATE()
		--NEED TO DELETE EXISTING Roles ASSOCITAED BEFORE 
		-- INSERTING NEW ONES. EXECUTION OF THIS STORED PROC
		-- IS MOVED FROM CODE			
		EXEC ZGWSecurity.Delete_Group_Roles @P_GroupSeqId,@PSecurityEntitySeqId, @P_Debug	
		SET @P_Roles = LTRIM(RTRIM(@P_Roles))+ ','
		SET @V_POS = CHARINDEX(',', @P_Roles, 1)
	
		IF REPLACE(@P_Roles, ',', '') <> ''
		BEGIN
			WHILE @V_POS > 0
			BEGIN
				SET @V_Role_Name = LTRIM(RTRIM(LEFT(@P_Roles, @V_POS - 1)))
				IF @V_Role_Name <> ''
				IF @P_Debug = 1 PRINT @V_Role_Name -- DEBUG
				BEGIN
					--SELECT THE RoleSeqId FROM THE Roles
					--TABLE FOR ALL THE Roles PASSED
					SELECT  
						@V_Roles_Security_Entities_SeqID = ZGWSecurity.Roles_Security_Entities.Roles_Security_Entities_SeqID
					FROM
					 	ZGWSecurity.Roles_Security_Entities
					WHERE 
						ZGWSecurity.Roles_Security_Entities.RoleSeqId = (SELECT RoleSeqId FROM ZGWSecurity.Roles WHERE ZGWSecurity.Roles.[Name] = @V_Role_Name)
						AND ZGWSecurity.Roles_Security_Entities.SecurityEntitySeqId = @PSecurityEntitySeqId
					IF @P_Debug = 1 PRINT @V_Roles_Security_Entities_SeqID

					SELECT
						@V_Groups_Security_Entities_SeqID = Groups_Security_Entities_SeqID
					FROM
						ZGWSecurity.Groups_Security_Entities
					WHERE
						SecurityEntitySeqId = @PSecurityEntitySeqId
						AND GroupSeqId = @P_GroupSeqId
					
					IF @P_Debug = 1 PRINT @V_Groups_Security_Entities_SeqID -- DEBUG
					/*
					INSERT THE ZGWSecurity.Groups_Security_Entities_Roles_Entities
					WITH Roles INFORMATION
					*/	
					IF @V_Roles_Security_Entities_SeqID IS NOT NULL
					BEGIN
									
						INSERT ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities (
							Groups_Security_Entities_SeqID,
							Roles_Security_Entities_SeqID,
							Added_By,
							Added_Date
						)VALUES(
							@V_Groups_Security_Entities_SeqID,
							@V_Roles_Security_Entities_SeqID,
							@P_Added_Updated_By,
							@V_Now
						)
				
						IF @P_Debug = 1 PRINT 'Inserted into ZGWSecurity.Groups_Security_Entities_Roles_Entities'
					END
	
				END
				SET @P_Roles = RIGHT(@P_Roles, LEN(@P_Roles) - @V_POS)
				SET @V_POS = CHARINDEX(',', @P_Roles, 1)
	
			END
		END	
	IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Set_Group_Roles'
RETURN 0

GO

