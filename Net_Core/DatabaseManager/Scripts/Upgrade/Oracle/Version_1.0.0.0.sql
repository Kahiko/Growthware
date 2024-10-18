-- Upgrade Populate all the tables (DML)/
-- All command must end in a forward slash, the forward slash will be removed before being executed./

-- CREATE OR REPLACE PACKAGE SQLSERVER_UTILITIES/
 CREATE OR REPLACE EDITIONABLE PACKAGE "YourDatabaseName"."SQLSERVER_UTILITIES" AS
    identity NUMBER(10);
    FUNCTION convert_(p_dataType IN VARCHAR2, p_expr IN VARCHAR2, p_style IN VARCHAR2 DEFAULT NULL) RETURN VARCHAR2;
    FUNCTION year_(p_date_str IN VARCHAR2) RETURN NUMBER;
    FUNCTION stuff(p_expr VARCHAR2, p_startIdx NUMBER, p_len NUMBER, p_replace_expr VARCHAR2)  RETURN VARCHAR2;
    FUNCTION dateadd(p_interval IN VARCHAR2, p_interval_val IN NUMBER, p_date_str IN VARCHAR2)  RETURN DATE;
    FUNCTION isdate(p_expr IN VARCHAR2) RETURN NUMBER;
    FUNCTION stats_date(p_table IN VARCHAR2, p_index IN VARCHAR2) RETURN DATE;
    FUNCTION rand(p_seed NUMBER DEFAULT NULL) RETURN NUMBER;
    FUNCTION to_base(p_dec NUMBER, p_base NUMBER)  RETURN VARCHAR2;
    FUNCTION patindex(p_pattern IN VARCHAR2, p_expr IN VARCHAR2) RETURN NUMBER;
    FUNCTION datediff(p_datepart VARCHAR2, p_start_date_str VARCHAR2, p_end_date_str VARCHAR2) RETURN NUMBER;
    FUNCTION day_(p_date_str IN VARCHAR2) RETURN NUMBER;
    FUNCTION ident_incr(p_sequence IN VARCHAR2) RETURN NUMBER;
    FUNCTION isnumeric(p_expr IN VARCHAR2) RETURN NUMBER;
    FUNCTION hex(p_num VARCHAR2) RETURN VARCHAR2;
    FUNCTION difference(p_expr1 IN VARCHAR2, p_expr2 IN VARCHAR2) RETURN NUMBER;
    FUNCTION datepart(p_part_expr IN VARCHAR2, p_date_str IN VARCHAR2)  RETURN NUMBER;
    FUNCTION radians(p_degree IN NUMBER) RETURN NUMBER;
    FUNCTION reverse_(p_expr IN VARCHAR2) RETURN VARCHAR2;
    FUNCTION parsename(p_object_name IN VARCHAR2, p_object_piece IN NUMBER) RETURN VARCHAR2;
    FUNCTION month_(p_date_str IN VARCHAR2) RETURN NUMBER;
    FUNCTION round_(p_expr NUMBER, p_len NUMBER, p_function NUMBER DEFAULT 0)  RETURN NUMBER;
    FUNCTION pi RETURN NUMBER;
    FUNCTION oct(p_num VARCHAR2) RETURN VARCHAR2;
    FUNCTION str(p_expr IN NUMBER, p_len IN NUMBER DEFAULT 10, p_scale IN NUMBER DEFAULT 0)  RETURN VARCHAR2;
    FUNCTION degrees(p_angle_radians IN NUMBER)  RETURN NUMBER;
    FUNCTION datename(p_part_expr IN VARCHAR2, p_date_str IN VARCHAR2)  RETURN VARCHAR2;
    FUNCTION ident_seed(p_sequence IN VARCHAR2) RETURN NUMBER;
    FUNCTION quotename(p_str IN VARCHAR2, p_delimiters IN VARCHAR2 DEFAULT '[]') RETURN VARCHAR2;
    FUNCTION str_to_date(p_date_expr IN VARCHAR2)  RETURN DATE;
    FUNCTION fetch_status(p_cursorfound IN BOOLEAN) RETURN NUMBER;
END SQLSERVER_UTILITIES;
/
COMMIT
/
-- CREATE OR REPLACE PACKAGE BODY SQLSERVER_UTILITIES/
CREATE OR REPLACE PACKAGE BODY "YourDatabaseName"."SQLSERVER_UTILITIES" AS
    identity NUMBER(10);
END SQLSERVER_UTILITIES;

/
-- Insert the version/
ALTER SESSION SET current_schema=ZGWSystem/
INSERT INTO Database_Information( 
    Version, 
    Enable_Inheritance, 
    Added_By, 
    Added_Date 
) VALUES ( 
    '1.0.0.0', 
    1, 
    2, 
    SYSDATE
)
/
COMMIT
/