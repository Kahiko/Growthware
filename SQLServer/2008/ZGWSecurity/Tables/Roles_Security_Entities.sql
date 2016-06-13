CREATE TABLE [ZGWSecurity].[Roles_Security_Entities] (
    [Roles_Security_Entities_SeqID] INT      IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Security_Entity_SeqID]         INT      NOT NULL,
    [Role_SeqID]                    INT      NOT NULL,
    [Added_By]                      INT      NOT NULL,
    [Added_Date]                    DATETIME NOT NULL,
    CONSTRAINT [PK_Roles_Security_Entities] PRIMARY KEY CLUSTERED ([Roles_Security_Entities_SeqID] ASC),
    CONSTRAINT [FK_Roles_Security_Entities_Entities] FOREIGN KEY ([Security_Entity_SeqID]) REFERENCES [ZGWSecurity].[Security_Entities] ([Security_Entity_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Roles_Security_Entities_Roles] FOREIGN KEY ([Role_SeqID]) REFERENCES [ZGWSecurity].[Roles] ([Role_SeqID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

-- =============================================
-- Author:		Michael Regan
-- Create date: 07/15/2011
-- Description:	Used to remove entries from
--	ZGWSecurity.Roles_Security_Entities_Roles_Security_Entities
-- =============================================
CREATE TRIGGER [ZGWSecurity].[trgrZGWSecurity_Roles_Security_Entities] 
   ON  [ZGWSecurity].[Roles_Security_Entities]
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE FROM ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities WHERE Roles_Security_Entities_SeqID = (SELECT Roles_Security_Entities_SeqID FROM deleted) 
    -- Insert statements for trigger here

END