SELECT
	 RT.[RefreshTokenId]
    ,RT.[AccountSeqId]
    ,[Token] = SUBSTRING(RT.[Token], LEN(RT.[Token]) - 6, 7)
	--,[Full Tolken] = RT.[Token]
    ,RT.[Expires]
    ,RT.[Created]
    ,RT.[CreatedByIp]
    ,RT.[Revoked]
    ,RT.[RevokedByIp]
    --,RT.[ReplacedByToken]
	,[ReplacedByToken] = SUBSTRING(RT.[ReplacedByToken], LEN(RT.[ReplacedByToken]) - 6, 7)
    ,RT.[ReasonRevoked]
FROM [ZGWSecurity].[RefreshTokens] RT

