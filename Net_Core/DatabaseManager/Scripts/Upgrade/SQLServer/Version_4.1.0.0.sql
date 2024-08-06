-- Upgrade from 4.0.1.0 to 4.1.0.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
GO
-- Creeate the table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Registration_Information]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Registration_Information](
		[SecurityEntitySeqId] [int] NOT NULL,
		[SecurityEntitySeqId_Owner] [int] NOT NULL,
		[AccountChoices] [varchar](128) NOT NULL,
		[AddAccount] [int] NOT NULL,
		[Groups] [varchar](max) NULL,
		[Roles] [varchar](max) NULL,
		[Added_By] INT NOT NULL,
		[Added_Date] DATETIME DEFAULT (getdate()) NOT NULL,
		[Updated_By] INT NULL,
		[Updated_Date] DATETIME NULL,
		CONSTRAINT [PK_Registration_Information] PRIMARY KEY CLUSTERED ([SecurityEntitySeqId] ASC)
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY];

	ALTER TABLE [ZGWSecurity].[Registration_Information] WITH CHECK ADD CONSTRAINT [FK_Registration_Information_Security_Entities] FOREIGN KEY([SecurityEntitySeqId])
		REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId])
		ON UPDATE CASCADE
		ON DELETE CASCADE;

	ALTER TABLE [ZGWSecurity].[Registration_Information] WITH CHECK ADD CONSTRAINT [FK_Registration_Information_Security_Entities_Owner] FOREIGN KEY([SecurityEntitySeqId_Owner])
		REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId])
		ON UPDATE NO ACTION
		ON DELETE NO ACTION;

	
	ALTER TABLE [ZGWSecurity].[Registration_Information] WITH CHECK ADD CONSTRAINT [FK_Registration_Information_Accounts] FOREIGN KEY ([AddAccount]) 
		REFERENCES [ZGWSecurity].[Accounts] ([AccountSeqId])
		ON UPDATE NO ACTION
		ON DELETE NO ACTION;
END
GO
-- Add the extended properties
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = 0)
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'Used to Extend [ZGWSecurity].[Security_Entities].' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Registration_Information';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'SecurityEntitySeqId' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]')))
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'1 to 1 FK to Security_Entities and the SecuritySeqId the information is associated with' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Registration_Information', @level2type=N'COLUMN',@level2name=N'SecurityEntitySeqId';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'SecurityEntitySeqId_Owner' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]')))
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The SecurityEntitySeqId where the roles are associated. Example the roles may only be associated with "System" but you are creating an account in "Developer"' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Registration_Information', @level2type=N'COLUMN',@level2name=N'SecurityEntitySeqId_Owner';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Roles' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]')))
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'Comma separated list of roles' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Registration_Information', @level2type=N'COLUMN',@level2name=N'Roles';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Groups' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]')))
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'Comma separated list of groups' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Registration_Information', @level2type=N'COLUMN',@level2name=N'Groups';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'AccountChoices' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]')))
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'Used to set the default "Account_Choices" values' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Registration_Information', @level2type=N'COLUMN',@level2name=N'AccountChoices';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'AddAccount' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]')))
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'Used to set the default "Account_Choices" values' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Registration_Information', @level2type=N'COLUMN',@level2name=N'AddAccount';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Added_By' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]')))
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The AccountSeqId that added the record' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Registration_Information', @level2type=N'COLUMN',@level2name=N'Added_By';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Added_Date' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]')))
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The date the record was added default value of GETDATE()' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Registration_Information', @level2type=N'COLUMN',@level2name=N'Added_Date';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Updated_By' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]')))
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The AccountSeqId that updated the record' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Registration_Information', @level2type=N'COLUMN',@level2name=N'Updated_By';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Updated_Date' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]')))
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The date the record was updated set by the stored procedure' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Registration_Information', @level2type=N'COLUMN',@level2name=N'Updated_Date';
GO
/****** Start: Procedure [ZGWSecurity].[Set_Registration_Information] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'ZGWSecurity.Set_Registration_Information') AND type IN ( N'P' ,N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Registration_Information] AS'
	END
--End If
GO
/*
Usage:
	DECLARE 
		@P_SecurityEntitySeqId int = 1,
        @P_SecurityEntitySeqId_Owner int = 1,
        @P_AccountChoices [varchar](128) = 'Mike',
        @P_AddAccount [int] = 1,
        @P_Groups [varchar](max) = 'Everyone',
        @P_Roles [varchar](max) = 'Authenticated',
        @P_Added_Updated_By INT = 1,
		@P_Debug INT = 1

	EXECUTE [ZGWSecurity].[Set_Registration_Information]
		@P_SecurityEntitySeqId,
        @P_SecurityEntitySeqId_Owner,
        @P_AccountChoices,
        @P_AddAccount,
        @P_Groups,
        @P_Roles,
        @P_Added_Updated_By,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/10/2024
-- Description:	Inserts or updates [ZGWSecurity].[Set_Registration_Information]
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Registration_Information]
	@P_SecurityEntitySeqId int,
    @P_SecurityEntitySeqId_Owner int,
    @P_AccountChoices [varchar](128) NULL,
    @P_AddAccount [varchar](128) NULL,
    @P_Groups [varchar](max) NULL,
    @P_Roles [varchar](max) NULL,
    @P_Added_Updated_By [int],
	@P_Debug INT = 0
AS
	SET NOCOUNT ON;
	IF @P_Debug = 1 PRINT 'Starting ZGWSecurity.Set_Registration_Information';
    DECLARE @V_Now DATETIME = GETDATE();
    IF NOT EXISTS (SELECT NULL FROM [ZGWSecurity].[Security_Entities] WHERE SecurityEntitySeqId = @P_SecurityEntitySeqId)
    BEGIN
        RAISERROR ('SecurityEntitySeqId does not exist in table [ZGWSecurity].[Security_Entities]', 16, 1);
        RETURN 1
    END
    IF EXISTS (SELECT NULL FROM [ZGWSecurity].[Registration_Information] WHERE SecurityEntitySeqId = @P_SecurityEntitySeqId)
        BEGIN
			IF @P_Debug = 1 PRINT 'Updating Record';
            UPDATE [ZGWSecurity].[Registration_Information] SET 
                [SecurityEntitySeqId_Owner] = @P_SecurityEntitySeqId_Owner,
                [AccountChoices] = @P_AccountChoices,
                [AddAccount] = @P_AddAccount,
                [Groups] = @P_Groups,
                [Roles] = @P_Roles,
                [Updated_By] = @P_Added_Updated_By,
                [Updated_Date] = @V_Now
            WHERE [SecurityEntitySeqId] = @P_SecurityEntitySeqId;
        END
    ELSE
        BEGIN
			IF @P_Debug = 1 PRINT 'Inserting Record';
            INSERT INTO [ZGWSecurity].[Registration_Information] (
                [SecurityEntitySeqId],
                [SecurityEntitySeqId_Owner],
                [AccountChoices],
                [AddAccount],
                [Groups],
                [Roles],
                [Added_By],
                [Added_Date]
            ) VALUES (
                @P_SecurityEntitySeqId,
                @P_SecurityEntitySeqId_Owner,
                @P_AccountChoices,
                @P_AddAccount,
                @P_Groups,
                @P_Roles,
                @P_Added_Updated_By,
                @V_Now
            );
        END
    --END IF
    SELECT
         RI.[SecurityEntitySeqId]
        ,RI.[SecurityEntitySeqId_Owner]
        ,RI.[AccountChoices]
        ,RI.[AddAccount]
        ,RI.[Groups]
        ,RI.[Roles]
        ,RI.[Added_By]
        ,RI.[Added_Date]
        ,RI.[Updated_By]
        ,RI.[Updated_Date]
    FROM 
        [ZGWSecurity].[Registration_Information] RI
    WHERE
        RI.[SecurityEntitySeqId] = @P_SecurityEntitySeqId;
    SET NOCOUNT OFF;
    IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Set_Registration_Information';
RETURN 0

GO
/****** End: Procedure [ZGWSecurity].[Set_Registration_Information] ******/
/****** Start: Procedure [ZGWSecurity].[Delete_Registration_Information] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'ZGWSecurity.Delete_Registration_Information') AND type IN ( N'P' ,N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Delete_Registration_Information] AS'
	END
--End If
GO
/*
Usage:
	DECLARE 
		@P_SecurityEntitySeqId int = 1,
		@P_Debug INT = 1

	exec ZGWSecurity.Delete_Registration_Information
		@P_SecurityEntitySeqId,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/10/2024
-- Description:	Deletes a record from [ZGWSecurity].[Registration_Information] given
--  the SecurityEntitySeqId
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Delete_Registration_Information]
	@P_SecurityEntitySeqId int,
	@P_Debug INT = 0
AS
	SET NOCOUNT ON
	IF @P_Debug = 1 PRINT 'Starting [ZGWSecurity].[Delete_Registration_Information]'
    DECLARE @V_Now DATETIME = GETDATE();
    IF EXISTS (SELECT NULL FROM [ZGWSecurity].[Registration_Information] WHERE [SecurityEntitySeqId] = @P_SecurityEntitySeqId)
    BEGIN
        DELETE FROM [ZGWSecurity].[Registration_Information] WHERE [SecurityEntitySeqId] = @P_SecurityEntitySeqId;
    END
    IF @P_Debug = 1 PRINT 'Ending ZGWSecurity.Delete_Registration_Information'
RETURN 0

GO
/****** End: Procedure [ZGWSecurity].[Delete_Registration_Information] ******/
/****** Start: Procedure [ZGWSecurity].[Get_Registration_Information] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'ZGWSecurity.Get_Registration_Information') AND type IN ( N'P' ,N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Get_Registration_Information] AS'
	END
--End If
GO
/*
Usage:
	DECLARE @P_SecurityEntitySeqId int = -1
	EXECUTE [ZGWSecurity].[Get_Registration_Information] @P_SecurityEntitySeqId
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/10/2024
-- Description:	Returns registration information
-- Note:
--	SecurityEntitySeqId of -1 returns all registration information.
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Get_Registration_Information]
    @P_SecurityEntitySeqId int = -1
AS
	SET NOCOUNT ON;
    IF @P_SecurityEntitySeqId = -1
        BEGIN
            SELECT
                RI.[SecurityEntitySeqId],
                RI.[SecurityEntitySeqId_Owner],
                RI.[AccountChoices],
                RI.[AddAccount],
                RI.[Groups],
                RI.[Roles],
                RI.[Added_By],
                RI.[Added_Date],
                RI.[Updated_By],
                RI.[UPDATED_DATE]
            FROM
                [ZGWSecurity].[Registration_Information] AS RI
            ORDER BY
                RI.[SecurityEntitySeqId]
        END
    ELSE
        BEGIN
            SELECT
                RI.[SecurityEntitySeqId],
                RI.[SecurityEntitySeqId_Owner],
                RI.[AccountChoices],
                RI.[AddAccount],
                RI.[Groups],
                RI.[Roles],
                RI.[Added_By],
                RI.[Added_Date],
                RI.[Updated_By],
                RI.[UPDATED_DATE]
            FROM
                [ZGWSecurity].[Registration_Information] AS RI
            WHERE
                RI.[SecurityEntitySeqId] = @P_SecurityEntitySeqId
            ORDER BY
                RI.[SecurityEntitySeqId]
        END
    --END IF

    SET NOCOUNT OFF;

RETURN 0

GO
/****** Start: Procedure [ZGWSecurity].[Get_Registration_Information] ******/
/****** Start: Procedure [ZGWSecurity].[Set_Registration_Information] ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND object_id = OBJECT_ID(N'ZGWSecurity.Set_Registration_Information') AND type IN ( N'P' ,N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Registration_Information] AS'
	END
--End If
GO
/****** Start: Procedure [ZGWSecurity].[Set_Account] ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Set_Account]') AND type IN (N'P' ,N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [ZGWSecurity].[Set_Account] AS'
	END
--END IF
GO

/*
Usage:
	DECLARE 
		@P_AccountSeqId int = -1,
		@P_StatusSeqId int = 1,
		@P_Account VARCHAR(128) = 'test',
		@P_First_Name VARCHAR(15) = 'test',
		@P_Last_Name VARCHAR(15) = 'test',
		@P_Middle_Name VARCHAR(15) = 'test',
		@P_Preferred_Name VARCHAR(50) = 'test',
		@P_Email VARCHAR(128) = 'test@test.com',
		@P_Password VARCHAR(256) = 'test',
		@P_Password_Last_Set datetime = GETDATE(),
		@P_ResetToken VARCHAR(256) = NULL,
		@P_ResetTokenExpires datetime = NULL,
		@P_Failed_Attempts int = 0,
		@P_Added_Updated_By int = 1,
		@P_Last_Login datetime = GETDATE(),
		@P_Time_Zone int = -5,
		@P_Location VARCHAR(50) = 'desk',
		@P_Enable_Notifications int = 0,
		@P_Is_System_Admin int = 0,
		@P_Debug INT = 1
--Insert new
	EXEC ZGWSecurity.Set_Account 
		@P_AccountSeqId,
		@P_StatusSeqId,
		@P_Account,
		@P_First_Name,
		@P_Last_Name,
		@P_Middle_Name,
		@P_Preferred_Name,
		@P_Email,
		@P_Password,
		@P_Password_Last_Set,
		@P_ResetToken VARCHAR(256),
		@P_ResetTokenExpires datetime,
		@P_Failed_Attempts,
		@P_Added_Updated_By,
		@P_Last_Login,
		@P_Time_Zone,
		@P_Location,
		@P_Enable_Notifications,
		@P_Is_System_Admin,
		@P_Debug
--Update
	SET @P_AccountSeqId = (SELECT AccountSeqId FROM ZGWSecurity.Accounts WHERE Account = 'test')
	EXEC ZGWSecurity.Set_Account
		@P_AccountSeqId,
		@P_StatusSeqId,
		@P_Account,
		@P_First_Name,
		@P_Last_Name,
		@P_Middle_Name,
		@P_Preferred_Name,
		@P_Email,
		@P_Password,
		@P_Password_Last_Set,
		@P_ResetToken,
		@P_ResetTokenExpires,
		@P_Failed_Attempts,
		@P_Added_Updated_By,
		@P_Last_Login,
		@P_Time_Zone,
		@P_Location,
		@P_Enable_Notifications,
		@P_Is_System_Admin,
		@P_Debug
*/
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/26/2011
-- Description:	Inserts/Updates [ZGWSystem].[Account]
--	@P_StatusSeqId's value determines insert/update
--	a value of -1 is insert > -1 performs update
-- =============================================
-- Author:		Michael Regan
-- Create date: 05/10/2024
-- Description:	Adding @P_ResetToken and @P_ResetTokenExpires
-- =============================================
-- Author:		Michael Regan
-- Create date: 08/01/2024
-- Description:	Added check for @P_Password_Last_Set fixes 
--'SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM' error
-- =============================================
ALTER PROCEDURE [ZGWSecurity].[Set_Account] @P_AccountSeqId INT OUTPUT
	,@P_StatusSeqId INT
	,@P_Account VARCHAR(128)
	,@P_First_Name VARCHAR(35)
	,@P_Last_Name VARCHAR(35)
	,@P_Middle_Name VARCHAR(35)
	,@P_Preferred_Name VARCHAR(50)
	,@P_Email VARCHAR(128)
	,@P_Password VARCHAR(256)
	,@P_Password_Last_Set DATETIME
	,@P_ResetToken VARCHAR(256)
	,@P_ResetTokenExpires DATETIME
	,@P_Failed_Attempts INT
	,@P_Added_Updated_By INT
	,@P_Last_Login DATETIME
	,@P_Time_Zone INT
	,@P_Location VARCHAR(50)
	,@P_Enable_Notifications INT
	,@P_Is_System_Admin INT
	,@P_Debug INT = 0
AS
SET NOCOUNT ON;
IF @P_Debug = 1 PRINT 'Start Set_Account';
IF ISDATE(@P_Password_Last_Set) = 0 SET @P_Password_Last_Set = GETDATE();
IF ISDATE(@P_Last_Login) = 0 SET @P_Last_Login = NULL;

DECLARE @VSecurityEntitySeqId VARCHAR(1)
	,@V_SecurityEntityName VARCHAR(50)
	,@V_BackColor VARCHAR(15)
	,@V_LeftColor VARCHAR(15)
	,@V_HeadColor VARCHAR(15)
	,@V_HeaderForeColor VARCHAR(15)
	,@V_SubHeadColor VARCHAR(15)
	,@V_RowBackColor VARCHAR(15)
	,@V_AlternatingRowBackColor VARCHAR(15)
	,@V_ColorScheme VARCHAR(15)
	,@V_FavoriteAction VARCHAR(25)
	,@V_recordsPerPage VARCHAR(1000)
	,@V_Default_Account VARCHAR(50)
	,@V_Now DATETIME = GETDATE()

IF @P_AccountSeqId > - 1
BEGIN
	-- UPDATE PROFILE
	IF @P_Debug = 1 PRINT 'UPDATE [ZGWSecurity].[Accounts]';

	UPDATE [ZGWSecurity].[Accounts]
	SET StatusSeqId = @P_StatusSeqId
		,Account = @P_Account
		,First_Name = @P_First_Name
		,Last_Name = @P_Last_Name
		,Middle_Name = @P_Middle_Name
		,Preferred_Name = @P_Preferred_Name
		,Email = @P_Email
		,Password_Last_Set = @P_Password_Last_Set
		,[Password] = @P_Password
		,[ResetToken] = @P_ResetToken
		,[ResetTokenExpires] = @P_ResetTokenExpires
		,Failed_Attempts = @P_Failed_Attempts
		,Last_Login = @P_Last_Login
		,Time_Zone = @P_Time_Zone
		,Location = @P_Location
		,Is_System_Admin = @P_Is_System_Admin
		,Enable_Notifications = @P_Enable_Notifications
		,Updated_By = @P_Added_Updated_By
		,Updated_Date = @V_Now
	WHERE AccountSeqId = @P_AccountSeqId
END
ELSE
BEGIN
	-- INSERT a new row in the table.
	SET NOCOUNT ON

	IF @P_Debug = 1 PRINT 'INSERT [ZGWSecurity].[Accounts]';

	INSERT [ZGWSecurity].[Accounts] (
		 StatusSeqId
		,Account
		,First_Name
		,Last_Name
		,Middle_Name
		,Preferred_Name
		,Email
		,Password_Last_Set
		,[Password]
		,[ResetToken]
		,[ResetTokenExpires]
		,FAILED_ATTEMPTS
		,IS_SYSTEM_ADMIN
		,Added_By
		,Added_Date
		,LAST_LOGIN
		,TIME_ZONE
		,Location
		,Enable_Notifications
	) VALUES (
		@P_StatusSeqId
		,@P_Account
		,@P_First_Name
		,@P_Last_Name
		,@P_Middle_Name
		,@P_Preferred_Name
		,@P_Email
		,@P_Password_Last_Set
		,@P_Password
		,NULL
		,NULL
		,@P_Failed_Attempts
		,@P_Is_System_Admin
		,@P_Added_Updated_By
		,@V_Now
		,@P_Last_Login
		,@P_Time_Zone
		,@P_Location
		,@P_Enable_Notifications
	);

	SET @P_AccountSeqId = SCOPE_IDENTITY()

	IF EXISTS (SELECT TOP(1) 1 FROM [ZGWSecurity].[Accounts] WHERE AccountSeqId = @P_AccountSeqId)
	BEGIN
		-- The Authenticated role is always added execpte for the Anonymous account
		IF(UPPER(@P_Account) <> 'ANONYMOUS')
			EXEC ZGWSecurity.Set_Account_Roles @P_Account ,1 ,'Authenticated' ,@P_Added_Updated_By ,@P_Debug;

		/*add an entry to account choice table*/
		IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Account_Choices' AND TABLE_SCHEMA = 'ZGWCoreWeb')
		BEGIN
			SELECT @V_Default_Account = Account
			FROM ZGWSecurity.Accounts
			WHERE AccountSeqId = @P_Added_Updated_By

			IF @V_Default_Account = NULL
				SET @V_Default_Account = 'ANONYMOUS'

			IF EXISTS (SELECT 1 FROM [ZGWCoreWeb].Account_Choices WHERE Account = @V_Default_Account)
			BEGIN
				-- Populate values from Account_Choices from the Anonymous account
				IF @P_Debug = 1
					PRINT 'Populating default values from the database for account ' + CONVERT(VARCHAR(MAX), @V_Default_Account)

				SELECT -- FILL THE DEFAULT VALUES
					@VSecurityEntitySeqId = SecurityEntityID
					,@V_SecurityEntityName = SecurityEntityName
					,@V_BackColor = BackColor
					,@V_LeftColor = LeftColor
					,@V_HeadColor = HeadColor
					,@V_HeaderForeColor = HeaderForeColor
					,@V_SubHeadColor = SubHeadColor
					,@V_RowBackColor = RowBackColor
					,@V_AlternatingRowBackColor = AlternatingRowBackColor
					,@V_ColorScheme = ColorScheme
					,@V_FavoriteAction = FavoriteAction
					,@V_recordsPerPage = recordsPerPage
				FROM [ZGWCoreWeb].Account_Choices
				WHERE Account = @V_Default_Account
			END
			ELSE
			BEGIN
				IF @P_Debug = 1
					PRINT 'Populating default values minimum values'

				SET @VSecurityEntitySeqId = (
						SELECT MIN(SecurityEntitySeqId)
						FROM ZGWSecurity.Security_Entities
					);
				SET @V_SecurityEntityName = (
						SELECT [Name]
						FROM ZGWSecurity.Security_Entities
						WHERE SecurityEntitySeqId = @VSecurityEntitySeqId
					);

				IF @VSecurityEntitySeqId = NULL
					SET @VSecurityEntitySeqId = 1

				IF @V_SecurityEntityName = NULL
					SET @V_SecurityEntityName = 'System'
			END
			--END IF

			IF @P_Debug = 1
				PRINT 'Executing ZGWCoreWeb.Set_Account_Choices'

			EXEC ZGWCoreWeb.Set_Account_Choices @P_Account
				,@VSecurityEntitySeqId
				,@V_SecurityEntityName
				,@V_BackColor
				,@V_LeftColor
				,@V_HeadColor
				,@V_HeaderForeColor
				,@V_SubHeadColor
				,@V_RowBackColor
				,@V_AlternatingRowBackColor
				,@V_ColorScheme
				,@V_FavoriteAction
				,@V_recordsPerPage
		END
		--END IF
	END
END -- Get the Error Code for the statement just executed.

IF @P_Debug = 1
	PRINT '@P_AccountSeqId = '

IF @P_Debug = 1
	PRINT @P_AccountSeqId

/* -- GOING BACK TO USING AN OUTPUT PARAMETER.
	SELECT
		  AccountSeqId
		, Account
		, Email
		, Enable_Notifications
		, Is_System_Admin
		, StatusSeqId
		, Password_Last_Set
		, Password
		, ResetToken
		, ResetTokenExpires
		, Failed_Attempts
		, First_Name
		, Last_Login
		, Last_Name
		, Location
		, Middle_Name
		, Preferred_Name
		, Time_Zone
		, Added_By
		, Added_Date
		, Updated_By
		, Updated_Date
	FROM 
		[ZGWSecurity].[Accounts] 
	WHERE 
		AccountSeqId = @P_AccountSeqId
*/
SET NOCOUNT OFF;
IF @P_Debug = 1 PRINT 'End Set_Account';

GO
/****** End: Procedure [ZGWSecurity].[Set_Account] ******/
DECLARE @V_SecurityEntitySeqId INT = 1,
        @V_FORMAT_AS_HTML_TRUE INT = 1, -- TRUE
        @V_PRIMARY_KEY INT,
        @V_SystemID INT = (SELECT AccountSeqId FROM ZGWSecurity.Accounts WHERE Account = 'System'),
        @V_Debug INT = 0;

IF NOT EXISTS (SELECT TOP(1) NULL FROM [ZGWCoreWeb].[Messages] WHERE [Name] = 'RegistrationSuccess')
    BEGIN
        EXEC ZGWCoreWeb.Set_Message 
            -1,
            @V_SecurityEntitySeqId,
            'RegistrationSuccess', -- Name
            'Registration Success', -- Title
            'Sent to the registering email/account when registration is successful', -- Description
            '<p>Please use the below link to verify your account:</p>
<p><a href="<Server>/accounts/verify-account?verificationToken=<VerificationToken>&email=<Email>">Verify Account</a></p>
<p>When you click on the above link your account will be verified and you will be logged in.  The system will not allow you to use any secured features until you change your password.</p>
<p>Please note the verification link will only work once.</p>', -- Body
            @V_FORMAT_AS_HTML_TRUE,
            @V_SystemID, 
            @V_Primary_Key, 
            @V_Debug;
    END
--END IF
IF NOT EXISTS (SELECT TOP(1) NULL FROM [ZGWCoreWeb].[Messages] WHERE [Name] = 'RegistrationError')
    BEGIN
        EXEC ZGWCoreWeb.Set_Message 
            -1,
            @V_SecurityEntitySeqId,
            'RegistrationError', -- Name
            'Registration Error', -- Title
            'Sent to the registering email/account when registration fails', -- Description
            'An attempt was made to register an account using this email.', -- Body
            @V_FORMAT_AS_HTML_TRUE,
            @V_SystemID, 
            @V_Primary_Key, 
            @V_Debug;
    END
--END IF

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '4.1.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;
