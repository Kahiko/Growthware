-- Downgrade

IF EXISTS (SELECT [FunctionSeqId] FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 18)
    UPDATE [ZGWOptional].[Directories] SET [Directory] = 'D:/Development/Growthware/Core/Web.Angular/CacheDependency/' WHERE [FunctionSeqId] = 18;
IF EXISTS (SELECT [FunctionSeqId] FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 19)
    UPDATE [ZGWOptional].[Directories] SET [Directory] = 'D:/Development/Growthware/Core/Web.Angular/Logs/' WHERE [FunctionSeqId] = 19;

-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '3.1.0.0',
    [Updated_By] = 3,
    [Updated_Date] = getdate()