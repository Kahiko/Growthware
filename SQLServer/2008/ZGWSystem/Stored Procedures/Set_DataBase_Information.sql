/*
Usage:
	DECLARE 
		@P_Database_Information_SeqID INT = 1,
		@P_Version VARCHAR(15) = '3',
		@P_Enable_Inheritance INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_Primary_Key int,
		@P_Debug INT = 1

	exec ZGWSystem.Set_DataBase_Information
		@P_Database_Information_SeqID,
		@P_Version,
		@P_Enable_Inheritance,
		@P_Added_Updated_By,
		@P_Primary_Key,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/28/2011
-- Description:	Inserts or updates a record from [ZGWSystem].[Set_DataBase_Information]
--	given the Database_Information_SeqID
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Set_DataBase_Information]
	@P_Database_Information_SeqID INT,
	@P_Version VARCHAR(15),
	@P_Enable_Inheritance INT,
	@P_Added_Updated_By INT,
	@P_Primary_Key int OUTPUT,
	@P_Debug INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @V_Now DATETIME = GETDATE()
	IF (SELECT COUNT(*) FROM [ZGWSystem].[Database_Information]) = 0
		BEGIN -- INSERT
			INSERT [ZGWSystem].[Database_Information]
			(
				Version,
				Enable_Inheritance,
				Added_By,
				Added_Date
			)
			VALUES
			(
				@P_Version,
				@P_Enable_Inheritance,
				@P_Added_Updated_By,
				@V_Now
			)
			SELECT @P_Primary_Key = SCOPE_IDENTITY()-- Get the IDENTITY value for the row just inserted.
		END
	ELSE-- UPDATE
		BEGIN
			UPDATE [ZGWSystem].[Database_Information]
			SET 
				[Version] = @P_Version,
				Enable_Inheritance = @P_Enable_Inheritance,
				Updated_By = @P_Added_Updated_By,
				Updated_Date = @V_Now
			WHERE
				Database_Information_SeqID = @P_Database_Information_SeqID

			SET @P_Primary_Key = @P_Database_Information_SeqID

		END
	-- END IF
END
RETURN 0