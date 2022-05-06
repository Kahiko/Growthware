CREATE FUNCTION [ZGWSecurity].[Get_Default_Entity_ID]()
RETURNS INT
AS
BEGIN
	DECLARE @V_Retval INT = (SELECT TOP 1 SecurityEntitySeqId FROM ZGWSecurity.Security_Entities ORDER BY SecurityEntitySeqId ASC)
	RETURN @V_Retval
END

GO

