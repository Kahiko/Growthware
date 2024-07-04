CREATE TABLE [ZGWSecurity].[Registration_Roles](
    [SecurityEntitySeqId] [int] NOT NULL,
    [RoleSourceSecuritySeqId] [int] NOT NULL,
    CONSTRAINT [PK_Registration_Roles] PRIMARY KEY CLUSTERED ([SecurityEntitySeqId] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];

ALTER TABLE [ZGWSecurity].[Registration_Roles] WITH CHECK ADD CONSTRAINT [FK_Registration_Roles_Security_Entities] FOREIGN KEY([SecurityEntitySeqId])
    REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId])
    ON UPDATE CASCADE
    ON DELETE CASCADE;