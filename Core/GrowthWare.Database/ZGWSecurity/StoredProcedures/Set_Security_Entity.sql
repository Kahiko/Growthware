
/*
Usage:
	DECLARE 
		@PSecurityEntitySeqId int = -1,
		@P_Name VARCHAR(256) = 'System',
		@P_Description VARCHAR(512) = 'System',
		@P_URL VARCHAR(128) = '',
		@P_StatusSeqId int = 1,
		@P_DAL VARCHAR(50) = '',
		@P_DAL_Name VARCHAR(50) = '',
		@P_DAL_Name_SPACE VARCHAR(256) = '',
		@P_DAL_String VARCHAR(512) = '',
		@P_Skin char(25) = '',
		@P_Style VARCHAR(25) = '',
		@P_Encryption_Type INT = 1,
		@P_ParentSecurityEntitySeqId int = 1,
		@P_Added_Updated_By INT = 2,
		@P_Primary_Key int,
		@P_Debug INT = 1

	exec ZGWSecurity.Set_Security_Entity
		@PSecurityEntitySeqId,
		@P_Name,
		@P_Description,
		@P_URL,
		@P_StatusSeqId,
		@P_DAL,
		@P_DAL_Name,
		@P_DAL_Name_SPACE,
		@P_DAL_String,
		@P_Skin,
		@P_Style,
		@P_Encryption_Type,
		@P_ParentSecurityEntitySeqId,
		@P_Added_Updated_By,
		@P_Primary_Key OUT,
		@P_Debug

	PRINT 'Primay key is: ' + CONVERT(VARCHAR(30),@P_Primary_Key)
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 09/02/2011
-- Description:	Inserts or updates ZGWSecurity.Security_Entities
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Security_Entity]
	@PSecurityEntitySeqId int,
	@P_Name VARCHAR(256),
	@P_Description VARCHAR(512),
	@P_URL VARCHAR(128),
	@P_StatusSeqId int,
	@P_DAL VARCHAR(50),
	@P_DAL_Name VARCHAR(50),
	@P_DAL_Name_SPACE VARCHAR(256),
	@P_DAL_String VARCHAR(512),
	@P_Skin char(25),
	@P_Style VARCHAR(25),
	@P_Encryption_Type INT,
	@P_ParentSecurityEntitySeqId int,
	@P_Added_Updated_By INT,
	@P_Primary_Key int OUTPUT,
	@P_Debug INT = 0
AS
	DECLARE @V_Now DATETIME = GETDATE()
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Security_Entity'
	IF @P_ParentSecurityEntitySeqId = @PSecurityEntitySeqId or @P_ParentSecurityEntitySeqId = -1 SET @P_ParentSecurityEntitySeqId = NULL
	IF @PSecurityEntitySeqId > -1
		BEGIN
			IF @P_Debug = 1 PRINT 'Update'
			UPDATE ZGWSecurity.Security_Entities
			SET 
				Name = @P_Name,
				[Description] = @P_Description,
				URL = @P_URL,
				StatusSeqId = @P_StatusSeqId,
				DAL = @P_DAL,
				DAL_Name = @P_DAL_Name,
				DAL_Name_Space = @P_DAL_Name_SPACE,
				DAL_String = @P_DAL_String,
				Skin = @P_Skin,
				Style = @P_Style,
				Encryption_Type = @P_Encryption_Type,
				ParentSecurityEntitySeqId = @P_ParentSecurityEntitySeqId,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = @V_Now
			WHERE
				SecurityEntitySeqId = @PSecurityEntitySeqId

			SELECT @P_Primary_Key = @PSecurityEntitySeqId
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
				StatusSeqId,
				DAL,
				DAL_Name,
				DAL_Name_SPACE,
				DAL_STRING,
				Skin,
				Style,
				Encryption_Type,
				ParentSecurityEntitySeqId,
				Added_By,
				Added_Date
			)
			VALUES
			(
				@P_Name,
				@P_Description,
				@P_URL,
				@P_StatusSeqId,
				@P_DAL,
				@P_DAL_Name,
				@P_DAL_Name_SPACE,
				@P_DAL_String,
				@P_Skin,
				@P_Style,
				@P_Encryption_Type,
				@P_ParentSecurityEntitySeqId,
				@P_Added_Updated_By,
				@V_Now
			)	
			-- Get the IDENTITY value for the row just inserted.
			SELECT @P_Primary_Key=SCOPE_IDENTITY()
		END
-- End if
IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Set_Security_Entity'
RETURN 0

GO

