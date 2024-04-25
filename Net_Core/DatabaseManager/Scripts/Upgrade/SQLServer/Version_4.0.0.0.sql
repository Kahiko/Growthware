-- Upgrade

IF EXISTS (SELECT [FunctionSeqId] FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 18)
    UPDATE [ZGWOptional].[Directories] SET [Directory] = 'D:/Development/Growthware/Net_Core/Web.Api/CacheDependency/' WHERE [FunctionSeqId] = 18
IF EXISTS (SELECT [FunctionSeqId] FROM [ZGWSecurity].[Functions] WHERE [FunctionSeqId] = 19)
    UPDATE [ZGWOptional].[Directories] SET [Directory] = 'D:/Development/Growthware/Net_Core/Web.Api/Logs/' WHERE [FunctionSeqId] = 19

-- Update the version
UPDATE [ZGWSystem].[Database_Information] SET
    [Version] = '4.0.0.0',
    [Updated_By] = 3,
    [Updated_Date] = getdate()