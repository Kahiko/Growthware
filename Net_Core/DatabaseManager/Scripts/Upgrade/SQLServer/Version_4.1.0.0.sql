-- Upgrade from 4.0.1.0 to 4.1.0.0
--USE [YourDatabaseName];
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

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '4.1.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;
