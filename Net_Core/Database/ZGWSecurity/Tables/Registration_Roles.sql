CREATE TABLE [ZGWSecurity].[Registration_Information](
    [SecurityEntitySeqId] [int] NOT NULL,
    [SecuritySeqId_With_Roles_Groups] [int] NOT NULL,
    [AccountChoices] [varchar](128) NULL,
    [AddAccount] [varchar](128) NULL,
    [Groups] [varchar](max) NULL,
    [Roles] [varchar](max) NULL,
    CONSTRAINT [PK_Registration_Information] PRIMARY KEY CLUSTERED ([SecurityEntitySeqId] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];

ALTER TABLE [ZGWSecurity].[Registration_Information] WITH CHECK ADD CONSTRAINT [FK_Registration_Information_Security_Entities] FOREIGN KEY([SecurityEntitySeqId])
    REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId])
    ON UPDATE CASCADE
    ON DELETE CASCADE;
