﻿CREATE   FUNCTION [ZFF_ENABLE_INHERITANCE] () 
RETURNS int AS 
BEGIN
	DECLARE @V_RETURN_VAL INT
	SET @V_RETURN_VAL = (SELECT Enable_Inheritance FROM ZFC_INFORMATION WHERE Information_SEQ_ID = 1)
	RETURN @V_RETURN_VAL -- 0 = FALSE 1 = TRUE
END