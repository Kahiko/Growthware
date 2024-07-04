-- Upgrade from 4.0.1.0 to 4.1.0.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
GO
-- Creeate the table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Registration_Roles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ZGWSecurity].[Registration_Roles](
		[SecurityEntitySeqId] [int] NOT NULL,
		[SecuritySeqId_With_Roles_Groups] [int] NOT NULL,
		[Roles] [varchar](max) NULL,
		[Groups] [varchar](max) NULL,
		CONSTRAINT [PK_Registration_Roles] PRIMARY KEY CLUSTERED ([SecurityEntitySeqId] ASC)
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY];

	ALTER TABLE [ZGWSecurity].[Registration_Roles] WITH CHECK ADD CONSTRAINT [FK_Registration_Roles_Security_Entities] FOREIGN KEY([SecurityEntitySeqId])
		REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId])
		ON UPDATE CASCADE
		ON DELETE CASCADE;
END
GO
-- Add the extended properties
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Roles]') AND [name] = N'MS_Description' AND [minor_id] = 0)
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'Used to Extend [ZGWSecurity].[Registration_Roles].' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Registration_Roles';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Roles]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'SecurityEntitySeqId' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Roles]')))
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'1 to 1 FK to Security_Entities' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Registration_Roles', @level2type=N'COLUMN',@level2name=N'SecurityEntitySeqId';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Roles]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'SecuritySeqId_With_Roles_Groups' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Roles]')))
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'The SecuritySeqId the roles are associated with' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Registration_Roles', @level2type=N'COLUMN',@level2name=N'SecuritySeqId_With_Roles_Groups';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Roles]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Roles' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Roles]')))
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'Comma separated list of roles' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Registration_Roles', @level2type=N'COLUMN',@level2name=N'Roles';
GO
IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Roles]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Groups' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Roles]')))
	EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'Comma separated list of groups' , @level0type=N'SCHEMA',@level0name=N'ZGWSecurity', @level1type=N'TABLE',@level1name=N'Registration_Roles', @level2type=N'COLUMN',@level2name=N'Groups';
GO

-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '4.1.0.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;
