-- Downgrade from 4.1.0.0 to 4.0.1.0
USE [YourDatabaseName];
GO
SET NOCOUNT ON;
-- Drop the table
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ZGWSecurity].[Registration_Information]') AND type in (N'U'))
	DROP TABLE [ZGWSecurity].[Registration_Information];
GO
-- Drop the extended properties
IF EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = 0)
	EXEC sp_dropextendedproperty @level0type = N'SCHEMA' ,@level0name = [ZGWSecurity] ,@level1type = N'TABLE' ,@level1name = [Registration_Information] ,@name = N'MS_Description';
GO
IF EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'SecurityEntitySeqId' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]')))
	EXECUTE sys.sp_dropextendedproperty @name = N'MS_DESCRIPTION' ,@level0type = N'SCHEMA' ,@level0name = N'ZGWSecurity' ,@level1type = N'TABLE' ,@level1name = N'Registration_Information' ,@level2type = N'COLUMN' ,@level2name = N'SecurityEntitySeqId';
GO
IF EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'SecuritySeqId_With_Roles_Groups' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]')))
	EXECUTE sys.sp_dropextendedproperty @name = N'MS_DESCRIPTION' ,@level0type = N'SCHEMA' ,@level0name = N'ZGWSecurity' ,@level1type = N'TABLE' ,@level1name = N'Registration_Information' ,@level2type = N'COLUMN' ,@level2name = N'SecuritySeqId_With_Roles_Groups';
GO
IF EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Roles' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]')))
	EXECUTE sys.sp_dropextendedproperty @name = N'MS_DESCRIPTION' ,@level0type = N'SCHEMA' ,@level0name = N'ZGWSecurity' ,@level1type = N'TABLE' ,@level1name = N'Registration_Information' ,@level2type = N'COLUMN' ,@level2name = N'Roles';
GO
IF EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Groups' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]')))
	EXECUTE sys.sp_dropextendedproperty @name = N'MS_DESCRIPTION' ,@level0type = N'SCHEMA' ,@level0name = N'ZGWSecurity' ,@level1type = N'TABLE' ,@level1name = N'Registration_Information' ,@level2type = N'COLUMN' ,@level2name = N'Groups';
GO
IF EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'AccountChoices' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]')))
	EXECUTE sys.sp_dropextendedproperty @name = N'MS_DESCRIPTION' ,@level0type = N'SCHEMA' ,@level0name = N'ZGWSecurity' ,@level1type = N'TABLE' ,@level1name = N'Registration_Information' ,@level2type = N'COLUMN' ,@level2name = N'AccountChoices';
GO
IF EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]') AND [name] = N'MS_Description' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'AddAccount' AND [object_id] = OBJECT_ID('[ZGWSecurity].[Registration_Information]')))
	EXECUTE sys.sp_dropextendedproperty @name = N'MS_DESCRIPTION' ,@level0type = N'SCHEMA' ,@level0name = N'ZGWSecurity' ,@level1type = N'TABLE' ,@level1name = N'Registration_Information' ,@level2type = N'COLUMN' ,@level2name = N'AddAccount';
GO
-- Update the version
UPDATE [ZGWSystem].[Database_Information]
SET [Version] = '4.0.1.0'
	,[Updated_By] = 3
	,[Updated_Date] = getdate();

SET NOCOUNT OFF;
