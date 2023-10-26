-- Downgrade

DECLARE 
    @P_UseAngular BIT = 0

exec ZGWSystem.PrepForAngularJS
    @P_UseAngular

DELETE FROM [ZGWSecurity].[Roles] WHERE [Name] = 'SysAdmin';

UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '1.0.0.0',
    [Updated_By] = null,
    [Updated_Date] = null
--WHERE [Version] = '2.0.0.0'