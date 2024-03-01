-- Downgrade
SET NOCOUNT ON;

DECLARE @V_MyAction VARCHAR(256) = 'SwaggerAPI';

IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [Action] = @V_MyAction)
    DELETE FROM [ZGWSecurity].[Functions] WHERE [Action] = @V_MyAction

SET @V_MyAction = 'RevokeToken';
IF EXISTS (SELECT 1 FROM [ZGWSecurity].[Functions] WHERE [Action] = @V_MyAction)
    DELETE FROM [ZGWSecurity].[Functions] WHERE [Action] = @V_MyAction

UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '3.0.0.0',
    [Updated_By] = null,
    [Updated_Date] = null