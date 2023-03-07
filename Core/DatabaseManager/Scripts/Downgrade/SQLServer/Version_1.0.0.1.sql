-- Downgrade

DELETE FROM [ZGWSecurity].[Functions] WHERE [Action] = 'SaveAccount'

UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '1.0.0.0',
    [Updated_By] = null,
    [Updated_Date] = null
WHERE [Version] = '1.0.0.1'