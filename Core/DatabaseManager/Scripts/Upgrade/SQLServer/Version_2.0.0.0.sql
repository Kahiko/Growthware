-- Upgrade
-- 0 = FALSE 1 = TRUE
DECLARE 
    @P_UseAngular BIT = 1

exec ZGWSystem.PrepForAngularJS
    @P_UseAngular

-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '2.0.0.0',
    [Updated_By] = 3,
    [Updated_Date] = getdate()
WHERE [Version] = '1.0.0.0'
