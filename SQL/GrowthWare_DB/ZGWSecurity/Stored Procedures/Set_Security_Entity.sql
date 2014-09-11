/*
Usage:
	DECLARE 
		@P_Security_Entity_SeqID int = -1,
		@P_Name VARCHAR(256) = 'System',
		@P_Description VARCHAR(512) = 'System',
		@P_URL VARCHAR(128) = '',
		@P_Status_SeqID int = 1,
		@P_DAL VARCHAR(50) = '',
		@P_DAL_Name VARCHAR(50) = '',
		@P_DAL_Name_SPACE VARCHAR(256) = '',
		@P_DAL_STRING VARCHAR(512) = '',
		@P_Skin char(25) = '',
		@P_Style VARCHAR(25) = '',
		@P_Encryption_Type INT = 1,
		@P_Parent_Security_Entity_SeqID int = 1,
		@P_Added_Updated_By INT = 2,
		@P_PRIMARY_KEY int,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Security_Entity
		@P_Security_Entity_SeqID,
		@P_Name,
		@P_Description,
		@P_URL,
		@P_Status_SeqID,
		@P_DAL,
		@P_DAL_Name,
		@P_DAL_Name_SPACE,
		@P_DAL_STRING,
		@P_Skin,
		@P_Style,
		@P_Encryption_Type,
		@P_Parent_Security_Entity_SeqID,
		@P_Added_Updated_By,
		@P_PRIMARY_KEY OUT,
		@P_Debug

	PRINT 'Primay key is: ' + CONVERT(VARCHAR(30),@P_Primary_Key)
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/02/2011
-- Description:	Inserts or updates ZGWSecurity.Security_Entities
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Security_Entity]
	@P_Security_Entity_SeqID int,
	@P_Name VARCHAR(256),
	@P_Description VARCHAR(512),
	@P_URL VARCHAR(128),
	@P_Status_SeqID int,
	@P_DAL VARCHAR(50),
	@P_DAL_Name VARCHAR(50),
	@P_DAL_Name_SPACE VARCHAR(256),
	@P_DAL_STRING VARCHAR(512),
	@P_Skin char(25),
	@P_Style VARCHAR(25),
	@P_Encryption_Type INT,
	@P_Parent_Security_Entity_SeqID int,
	@P_Added_Updated_By INT,
	@P_PRIMARY_KEY int OUTPUT,
	@P_Debug INT = 0
AS
	DECLARE @V_Now DATETIME = GETDATE()
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Security_Entity'
	IF @P_Parent_Security_Entity_SeqID = @P_Security_Entity_SeqID or @P_Parent_Security_Entity_SeqID = -1 SET @P_Parent_Security_Entity_SeqID = NULL
	IF @P_Security_Entity_SeqID > -1
		BEGIN
			IF @P_Debug = 1 PRINT 'Update'
			UPDATE ZGWSecurity.Security_Entities
			SET 
				Name = @P_Name,
				[Description] = @P_Description,
				URL = @P_URL,
				Status_SeqID = @P_Status_SeqID,
				DAL = @P_DAL,
				DAL_Name = @P_DAL_Name,
				DAL_Name_SPACE = @P_DAL_Name_SPACE,
				DAL_STRING = @P_DAL_STRING,
				Skin = @P_Skin,
				Style = @P_Style,
				Encryption_Type = @P_Encryption_Type,
				Parent_Security_Entity_SeqID = @P_Parent_Security_Entity_SeqID,
				UPDATED_BY = @P_Added_Updated_By,
				UPDATED_DATE = @V_Now
			WHERE
				Security_Entity_SeqID = @P_Security_Entity_SeqID

			SELECT @P_PRIMARY_KEY = @P_Security_Entity_SeqID
		END
	ELSE
		BEGIN
			IF @P_Debug = 1 PRINT 'Insert'
			-- INSERT a new row in the table.
			INSERT ZGWSecurity.Security_Entities
			(
				[Name],
				[Description],
				[URL],
				Status_SeqID,
				DAL,
				DAL_Name,
				DAL_Name_SPACE,
				DAL_STRING,
				Skin,
				Style,
				Encryption_Type,
				Parent_Security_Entity_SeqID,
				ADDED_BY,
				ADDED_DATE
			)
			VALUES
			(
				@P_Name,
				@P_Description,
				@P_URL,
				@P_Status_SeqID,
				@P_DAL,
				@P_DAL_Name,
				@P_DAL_Name_SPACE,
				@P_DAL_STRING,
				@P_Skin,
				@P_Style,
				@P_Encryption_Type,
				@P_Parent_Security_Entity_SeqID,
				@P_Added_Updated_By,
				@V_Now
			)	
			-- Get the IDENTITY value for the row just inserted.
			SELECT @P_PRIMARY_KEY=SCOPE_IDENTITY()
		END
-- End if
IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Set_Security_Entity'
RETURN 0