﻿CREATE PROCEDURE [ZFP_SET_NVP]
	@P_NVP_SEQ_ID int,
	@P_STATIC_NAME VARCHAR(30),
	@P_DISPLAY VARCHAR(128),
	@P_DESCRIPTION VARCHAR(256),
	@P_STATUS_SEQ_ID INT,
	@P_ADDED_BY INT,
	@P_ADDED_DATE DATETIME,
	@P_UPDATED_BY INT,
	@P_UPDATED_DATE DATETIME,
	@P_PRIMARY_KEY INT OUTPUT,
	@P_ErrorCode int OUTPUT
AS
	IF @P_NVP_SEQ_ID > -1
		BEGIN -- UPDATE PROFILE
			UPDATE ZFC_NVP
			SET 
				[DISPLAY] = @P_DISPLAY,
				[DESCRIPTION] = @P_DESCRIPTION,
				STATUS_SEQ_ID = @P_STATUS_SEQ_ID,
				UPDATED_BY = @P_UPDATED_BY,
				UPDATED_DATE = @P_UPDATED_DATE
			WHERE
				NVP_SEQ_ID = @P_NVP_SEQ_ID

			SELECT @P_PRIMARY_KEY = @P_NVP_SEQ_ID
		END
	ELSE
	BEGIN -- INSERT a new row in the table.

			-- CHECK FOR DUPLICATE NAME BEFORE INSERTING
			IF EXISTS( 
					SELECT 
						STATIC_NAME
					FROM 
						ZFC_NVP
					WHERE 
						STATIC_NAME = @P_STATIC_NAME
			)
			BEGIN
				RAISERROR ('The name value pair already exists in the database.',16,1)
				RETURN
			END
			IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(@P_STATIC_NAME) AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
			BEGIN -- Create the new table to hold the details for the name value pair
				DECLARE @V_Statement nvarchar(4000)

				set @V_Statement = 'CREATE TABLE [' + CONVERT(VARCHAR,@P_STATIC_NAME) + '](
					[NVP_SEQ_DET_ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
					[NVP_SEQ_ID] [int] NOT NULL,
					[NVP_DET_VALUE] [varchar](50) NOT NULL,
					[NVP_DET_TEXT] [varchar](300) NOT NULL,
					[STATUS_SEQ_ID] [int] NOT NULL,
					[SORT_ORDER] [int] NOT NULL,
					[ADDED_BY] [int] NOT NULL,
					[ADDED_DATE] [datetime] NOT NULL,
					[UPDATED_BY] [int] NULL,
					[UPDATED_DATE] [datetime] NULL,
					 CONSTRAINT [PK_' + CONVERT(VARCHAR,@P_STATIC_NAME) + '] PRIMARY KEY CLUSTERED 
					(
						[NVP_SEQ_DET_ID] ASC
					)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
					 CONSTRAINT [UK_' + CONVERT(VARCHAR,@P_STATIC_NAME) + '] UNIQUE NONCLUSTERED 
					(
						[NVP_DET_VALUE] ASC,	
						[NVP_DET_TEXT] ASC
					)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
					) ON [PRIMARY]
					ALTER TABLE [' + CONVERT(VARCHAR,@P_STATIC_NAME) + '] WITH CHECK ADD CONSTRAINT [FK_' + CONVERT(VARCHAR,@P_STATIC_NAME) + '_ZFC_SYSTEM_STATUS] FOREIGN KEY([STATUS_SEQ_ID])
					REFERENCES [ZFC_SYSTEM_STATUS] ([STATUS_SEQ_ID])
					ON UPDATE CASCADE
					ON DELETE CASCADE
					ALTER TABLE [' + CONVERT(VARCHAR,@P_STATIC_NAME) + '] CHECK CONSTRAINT [FK_' + CONVERT(VARCHAR,@P_STATIC_NAME) + '_ZFC_SYSTEM_STATUS]
					ALTER TABLE [' + CONVERT(VARCHAR,@P_STATIC_NAME) + ']  WITH CHECK ADD  CONSTRAINT [FK_' + CONVERT(VARCHAR,@P_STATIC_NAME) + '_ZFC_NVP] FOREIGN KEY([NVP_SEQ_ID])
					REFERENCES [ZFC_NVP] ([NVP_SEQ_ID])
					ON UPDATE CASCADE
					ON DELETE CASCADE
					ALTER TABLE [' + CONVERT(VARCHAR,@P_STATIC_NAME) + '] CHECK CONSTRAINT [FK_' + CONVERT(VARCHAR,@P_STATIC_NAME) + '_ZFC_NVP]
					' 
				EXECUTE dbo.sp_executesql @statement = @V_Statement

			END
			INSERT ZFC_NVP
			(
				STATIC_NAME,
				[DISPLAY],
				[DESCRIPTION],
				STATUS_SEQ_ID,
				ADDED_BY,
				ADDED_DATE,
				UPDATED_BY,
				UPDATED_DATE
			)
			VALUES
			(
				@P_STATIC_NAME,
				@P_DISPLAY,
				@P_DESCRIPTION,
				@P_STATUS_SEQ_ID,
				@P_ADDED_BY,
				@P_ADDED_DATE,
				@P_ADDED_BY,
				@P_ADDED_DATE
			)
			SELECT @P_PRIMARY_KEY=SCOPE_IDENTITY() -- Get the IDENTITY value for the row just inserted.
		END
-- Get the Error Code for the statement just executed.
SELECT @P_ErrorCode=@@ERROR