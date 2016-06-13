CREATE FUNCTION [ZGWSecurity].[Get_Default_Entity_ID]()
RETURNS INT
AS
BEGIN
	DECLARE @V_Retval INT = (SELECT TOP 1 Security_Entity_SeqID FROM ZGWSecurity.Security_Entities ORDER BY Security_Entity_SeqID ASC)
	RETURN @V_Retval
END