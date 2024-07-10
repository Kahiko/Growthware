CREATE TABLE [ZGWSecurity].[Registration_Information](
    [SecurityEntitySeqId] [int] NOT NULL,
    [SecurityEntitySeqId_Owner] [int] NOT NULL,
    [AccountChoices] [varchar](128) NULL,
    [AddAccount] [varchar](128) NULL,
    [Groups] [varchar](max) NULL,
    [Roles] [varchar](max) NULL,
    [Added_By] INT NOT NULL,
    [Added_Date] DATETIME DEFAULT (getdate()) NOT NULL,
    [Updated_By] INT NULL,
    [UPDATED_DATE] DATETIME NULL,
    CONSTRAINT [PK_Registration_Information] PRIMARY KEY CLUSTERED ([SecurityEntitySeqId] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];

ALTER TABLE [ZGWSecurity].[Registration_Information] WITH CHECK ADD CONSTRAINT [FK_Registration_Information_Security_Entities] FOREIGN KEY([SecurityEntitySeqId])
    REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId])
    ON UPDATE CASCADE
    ON DELETE CASCADE;

ALTER TABLE [ZGWSecurity].[Registration_Information] WITH CHECK ADD CONSTRAINT [FK_Registration_Information_Security_Entities_Owner] FOREIGN KEY([SecurityEntitySeqId_Owner])
    REFERENCES [ZGWSecurity].[Security_Entities] ([SecurityEntitySeqId])
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE [ZGWSecurity].[Registration_Information] WITH CHECK ADD CONSTRAINT [FK_Registration_Information_Accounts] FOREIGN KEY ([AddAccount]) 
    REFERENCES [ZGWSecurity].[Accounts] ([AccountSeqId])
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

