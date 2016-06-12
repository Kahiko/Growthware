
-- =============================================
-- Author:		Michael Regan
-- Create date: 07/15/2011
-- Description:	Used to remove entries from
--	ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities
-- =============================================
CREATE TRIGGER [ZGWSecurity].[trgrZGWSecurity_Groups_Security_Entities] 
   ON  [ZGWSecurity].[Groups_Security_Entities]
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE FROM ZGWSecurity.Groups_Security_Entities_Roles_Security_Entities WHERE Groups_Security_Entities_SeqID = (SELECT Groups_Security_Entities_SeqID FROM deleted) 
    -- Insert statements for trigger here

END