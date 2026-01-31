-- To allow advanced options to be changed.
EXEC sp_configure 'show advanced options', 1
-- To update the currently configured value for advanced options.
RECONFIGURE
-- To enable the feature.
EXEC sp_configure 'xp_cmdshell', 1
-- To update the currently configured value for this feature.
RECONFIGURE

DECLARE @V_Sql VARCHAR(8000)
	  , @V_DirectoryAndFile VARCHAR(255) = 'c:\Temp\Diagram\Security_ERD';

SET @V_Sql='BCP "select definition from ['+db_name()+'].[dbo].[sysdiagrams] where name = ''Security_ERD''" queryout "' + @V_DirectoryAndFile + '" -c -t -T -S LOCALHOST'
PRINT @V_Sql
EXEC xp_cmdshell @V_Sql

