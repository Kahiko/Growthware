-- Upgrade
-- 0 = FALSE 1 = TRUE
DECLARE 
    @P_UseAngular BIT = 1,
    @VSecurityEntitySeqId INT = (SELECT SecurityEntitySeqId FROM ZGWSecurity.Security_Entities WHERE [Name]='System'),
    @V_SystemID INT = (select AccountSeqId from ZGWSecurity.Accounts where Account = 'System'),
    @V_PRIMARY_KEY INT = NULL,
    @V_Debug BIT = 0;

exec ZGWSecurity.Set_Role -1,'SysAdmin','This is a special role that by virture of it being assigned to an account that account then will always return true for MayView, MayAdd, MayEdit and MayDelete!',1,0,@VSecurityEntitySeqId, @V_SystemID, @V_PRIMARY_KEY, @V_Debug
exec ZGWSecurity.Set_Account_Roles 'Developer',@VSecurityEntitySeqId,'Authenticated,AlwaysLogon,Developer,SysAdmin', @V_SystemID, @V_Debug
exec ZGWSystem.PrepForAngularJS @P_UseAngular;

-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '1.0.0.0',
    [Updated_By] = 3,
    [Updated_Date] = getdate()
WHERE [Version] = '3.0.0.0'
