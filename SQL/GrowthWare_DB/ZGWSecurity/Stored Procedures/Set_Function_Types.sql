/*
Usage:
	DECLARE 
			@P_Function_Type_SeqID INT = -1,
			@P_Name VARCHAR(50) = 'Name',
			@P_Description VARCHAR(512) = 'Description',
			@P_Template VARCHAR(512) = null,
			@P_Is_Content INT = 0,
			@P_Added_Updated_BY	INT = 2,
			@P_Primary_Key INT = NULL,
			@P_ErrorCode INT = NULL
--Insert new
	exec [ZGWSecurity].[Set_Function_Types]
			@P_Function_Type_SeqID,
			@P_Name,
			@P_Description,
			@P_Template,
			@P_Is_Content,
			@P_Added_Updated_BY,
			@P_Primary_Key,
			@P_ErrorCode
--Update
	SET @P_Function_Type_SeqID = (SELECT Function_Type_SeqID FROM [ZGWSecurity].[Function_Types] WHERE [Name] = @P_Name)
	exec [ZGWSecurity].[Set_Function_Types]
			@P_Function_Type_SeqID,
			@P_Name,
			@P_Description,
			@P_Template,
			@P_Is_Content,
			@P_Added_Updated_BY,
			@P_Primary_Key,
			@P_ErrorCode
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates [ZGWSecurity].[Function_Types]
--	Given the Function_Type_SeqID
--	a value of -1 is insert > -1 performs update
-- =============================================
CREATE PROCEDURE [ZGWSecurity].[Set_Function_Types]
	@P_Function_Type_SeqID INT,
	@P_Name VARCHAR(50),
	@P_Description VARCHAR(512),
	@P_Template VARCHAR(512),
	@P_Is_Content INT,
	@P_Added_Updated_BY	INT,
	@P_Primary_Key int OUTPUT,
	@P_ErrorCode int OUTPUT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Start [Set_Function_Types]'
	DECLARE @V_Now DATETIME = GETDATE()
	IF @P_Function_Type_SeqID > -1
		BEGIN -- UPDATE PROFILE
			IF @P_Debug = 1 PRINT 'Updating ZGWSecurity.Function_Types'
			UPDATE ZGWSecurity.Function_Types
			SET 
				[Name] = @P_Name,
				[Description] = @P_Description,
				Template = @P_Template,
				Is_Content = @P_Is_Content,
				UPDATED_BY = @P_Added_Updated_BY,
				UPDATED_DATE =@V_Now
			WHERE
				Function_Type_SeqID = @P_Function_Type_SeqID

			SELECT @P_Primary_Key = @P_Function_Type_SeqID
		END
	ELSE
	BEGIN -- INSERT a new row in the table.

			-- CHECK FOR DUPLICATE Name BEFORE INSERTING
			IF EXISTS( SELECT @P_Description 
				   FROM [ZGWSecurity].[Function_Types]
				   WHERE [Name] = @P_Name
			)
			BEGIN
				RAISERROR ('THE FUNCTION TYPE YOU ENTERED ALREADY EXISTS IN THE DATABASE.',16,1)
				RETURN
			END
			IF @P_Debug = 1 PRINT 'Inserting record into ZGWSecurity.Function_Types'
			INSERT ZGWSecurity.Function_Types
			(
				[Name],
				[Description],
				Template,
				Is_Content,
				ADDED_BY,
				ADDED_DATE
			)
			VALUES
			(
				@P_Name,
				@P_Description,
				@P_Template,
				@P_Is_Content,
				@P_Added_Updated_BY,
				@V_Now
			)
			SELECT @P_Primary_Key=SCOPE_IDENTITY() -- Get the IDENTITY value for the row just inserted.
		END
	-- Get the Error Code for the statement just executed.
	SELECT @P_ErrorCode=@@ERROR
	IF @P_Debug = 1 PRINT 'End [Set_Function_Types]'
RETURN 0