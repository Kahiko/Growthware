
/*
Usage:
	DECLARE 
		@P_NVPSeqId int = -1,
		@P_Schema_Name VARCHAR(30) = 'dbo',
		@P_Static_Name VARCHAR(30) = 'Testing',
		@P_Display VARCHAR(128) = 'TestingNVP',
		@P_Description VARCHAR(256) = 'Just Testing the Name value Pair',
		@P_StatusSeqId INT = 1,
		@P_Added_Updated_By INT = 1,
		@P_ErrorCode int = null,
		@P_Debug bit = 1

	exec ZGWSystem.Set_Name_Value_Pair
		@P_NVPSeqId,
		@P_Schema_Name,
		@P_Static_Name,
		@P_Display,
		@P_Description,
		@P_StatusSeqId,
		@P_Added_Updated_By,
		@P_ErrorCode,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates ZGWCoreWeb.Account_Choices based on @P_Account
-- =============================================
-- Author:		Michael Regan
-- Create date: 04/12/2024
-- Description:	Changed to return a data row
-- =============================================
CREATE PROCEDURE [ZGWSystem].[Set_Name_Value_Pair]
	@P_NVPSeqId int,
	@P_Schema_Name VARCHAR(30),
	@P_Static_Name VARCHAR(30),
	@P_Display VARCHAR(128),
	@P_Description VARCHAR(256),
	@P_StatusSeqId INT,
	@P_Added_Updated_By INT,
	@P_ErrorCode int OUTPUT,
	@P_Debug INT = 0
AS
	IF @P_Debug = 1 PRINT 'Starting [ZGWSystem].[Set_Name_Value_Pair]'
	DECLARE @V_Now DATETIME = GETDATE()
			, @V_PrimaryKey int = -1;
	SET @V_PrimaryKey = @P_NVPSeqId;
	IF @P_NVPSeqId > -1
		BEGIN
	-- UPDATE PROFILE
	UPDATE ZGWSystem.Name_Value_Pairs
		SET 
			[Display] = @P_Display,
			[Description] = @P_Description,
			StatusSeqId = @P_StatusSeqId,
			Updated_By = @P_Added_Updated_By,
			Updated_Date = @V_Now
		WHERE
			NVPSeqId = @P_NVPSeqId
END
	ELSE
	BEGIN
	-- INSERT a new row in the table.

	-- CHECK FOR DUPLICATE NAME BEFORE INSERTING
	IF EXISTS(SELECT Static_Name FROM [ZGWSystem].[Name_Value_Pairs] WHERE Static_Name = @P_Static_Name)
		BEGIN
			RAISERROR ('The name value pair already exists in the database.',16,1)
			SELECT @P_ErrorCode=1
			RETURN
		END
	-- END IF
	IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID('[' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR(MAX),@P_Static_Name) + ']') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		-- Create the new table to hold the details for the name value pair
		DECLARE @V_Statement nvarchar(4000)

		set @V_Statement = 'CREATE TABLE [' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR,@P_Static_Name) + '](
					[NVP_DetailSeqId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
					[NVPSeqId] [int] NOT NULL,
					[NVP_Detail_Name] [varchar](50) NOT NULL,
					[NVP_Detail_Value] [varchar](300) NOT NULL,
					[StatusSeqId] [int] NOT NULL,
					[Sort_Order] [int] NOT NULL,
					[Added_By] [int] NOT NULL,
					[Added_DATE] [datetime] NOT NULL,
					[Updated_By] [int] NULL,
					[Updated_Date] [datetime] NULL,
					 CONSTRAINT [PK_' + CONVERT(VARCHAR,@P_Static_Name) + '] PRIMARY KEY CLUSTERED 
					(
						[NVP_DetailSeqId] ASC
					)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
					 CONSTRAINT [UK_' + CONVERT(VARCHAR,@P_Static_Name) + '] UNIQUE NONCLUSTERED 
					(
						[NVP_Detail_Name] ASC,	
						[NVP_Detail_Value] ASC
					)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
					) ON [PRIMARY]
					ALTER TABLE [' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR,@P_Static_Name) + '] WITH CHECK ADD CONSTRAINT [FK_' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '_' + CONVERT(VARCHAR,@P_Static_Name) + '_ZGWSystem_Statuses] FOREIGN KEY([StatusSeqId])
					REFERENCES [ZGWSystem].[Statuses] ([StatusSeqId])
					ON UPDATE CASCADE
					ON DELETE CASCADE
					ALTER TABLE [' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR,@P_Static_Name) + '] CHECK CONSTRAINT [FK_' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '_' + CONVERT(VARCHAR,@P_Static_Name) + '_ZGWSystem_Statuses]
					ALTER TABLE[' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR,@P_Static_Name) + ']  WITH CHECK ADD  CONSTRAINT [FK_' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '_' + CONVERT(VARCHAR,@P_Static_Name) + '_ZGWSystem_Name_Value_Pairs] FOREIGN KEY([NVPSeqId])
					REFERENCES [ZGWSystem].[Name_Value_Pairs] ([NVPSeqId])
					ON UPDATE CASCADE
					ON DELETE CASCADE
					ALTER TABLE [' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '].[' + CONVERT(VARCHAR,@P_Static_Name) + '] CHECK CONSTRAINT [FK_' + CONVERT(VARCHAR(MAX),@P_Schema_Name) + '_' + CONVERT(VARCHAR,@P_Static_Name) + '_ZGWSystem_Name_Value_Pairs]
					'
		IF @P_Debug = 1 PRINT  @V_Statement
		EXECUTE dbo.sp_executesql @statement = @V_Statement
	END
	-- END IF
	INSERT ZGWSystem.Name_Value_Pairs(
		[Schema_Name],
		[Static_Name],
		[Display],
		[Description],
		[StatusSeqId],
		[Added_By],
		[Added_Date]
	) VALUES (
		@P_Schema_Name,
		@P_Static_Name,
		@P_Display,
		@P_Description,
		@P_StatusSeqId,
		@P_Added_Updated_By,
		@V_Now
	)
	SELECT @V_PrimaryKey=SCOPE_IDENTITY()
-- Get the IDENTITY value for the row just inserted.
END
-- Get the Error Code for the statement just executed.
SELECT
	  [nvpSeqId] = [NVPSeqId]
	, [schemaName] = [Schema_Name]
	, [staticName] = [Static_Name]
	, [display]
	, [description]
	, [StatusSeqId]
	, [Added_By]
	, [Added_Date]
	, [Updated_By]
	, [Updated_Date]
FROM
	[ZGWSystem].[Name_Value_Pairs]
WHERE
	[NVPSeqId] = @V_PrimaryKey
SELECT @P_ErrorCode=@@ERROR
IF @P_Debug = 1 PRINT 'End [ZGWSystem].[Set_Name_Value_Pair]'

GO
