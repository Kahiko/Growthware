CREATE TABLE [ZGWSecurity].[RefreshTokens] (
    [RefreshTokenId]  INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AccountSeqId]    INT           NOT NULL,
    [Token]           VARCHAR (MAX) NULL,
    [Expires]         DATETIME      NOT NULL,
    [Created]         DATETIME      NOT NULL,
    [CreatedByIp]     VARCHAR (25)  NULL,
    [Revoked]         TEXT          NULL,
    [RevokedByIp]     VARCHAR (25)  NULL,
    [ReplacedByToken] VARCHAR (MAX) NULL,
    [ReasonRevoked]   VARCHAR (512) NULL,
    CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED ([RefreshTokenId] ASC),
    CONSTRAINT [FK_RefreshTokens_Accounts] FOREIGN KEY ([AccountSeqId]) REFERENCES [ZGWSecurity].[Accounts] ([AccountSeqId]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

