SET SCAN OFF;
CREATE USER Foundation IDENTIFIED BY Foundation DEFAULT TABLESPACE USERS TEMPORARY TABLESPACE TEMP;
GRANT CREATE SESSION, RESOURCE, CREATE VIEW TO Foundation;
GRANT CREATE ROLE to Foundation;

connect Foundation/Foundation;

CREATE OR REPLACE PACKAGE sqlserver_utilities AS
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
END sqlserver_utilities;

/

CREATE OR REPLACE PACKAGE BODY sqlserver_utilities AS
FUNCTION str_to_date(p_date_expr IN VARCHAR2) 
RETURN DATE
IS
      temp_val NUMBER;
      temp_exp VARCHAR2(50);
      format_str VARCHAR2(50) := NULL;
BEGIN
      IF p_date_expr IS NULL THEN
        RETURN NULL;
      END IF;
     
      temp_exp := TRIM(p_date_expr);
      
      -- only for 10g
      IF NOT DBMS_DB_VERSION.VER_LE_9_2 THEN
        IF REGEXP_INSTR(temp_exp, 
            '^\{d[[:space:]]*''([[:digit:]]{4})-([[:digit:]]{2})-([[:digit:]]{2})''\}$') = 1 THEN -- ISO861 format
            -- e.g. {d '2004-05-23' }
            temp_exp := REGEXP_REPLACE(temp_exp, 
                       '^\{d[[:space:]]*''([[:digit:]]{4})-([[:digit:]]{2})-([[:digit:]]{2})''\}$', 
                       '\1-\2-\3');
            format_str := 'YYYY-MM-DD';
        ELSIF REGEXP_INSTR(temp_exp, 
            '^\{t[[:space:]]*''([[:digit:]]{2}):([[:digit:]]{2}):([[:digit:]]{2})(\.[[:digit:]]{3})?''\}$',
            1, 1, 0, 'i') = 1 THEN -- ISO861 format
            -- e.g. { t '14:25:10.487' } 
            temp_exp := REGEXP_REPLACE(temp_exp, 
                        '^\{t[[:space:]]*''([[:digit:]]{2}):([[:digit:]]{2}):([[:digit:]]{2})(\.[[:digit:]]{3})?''\}$', 
                        TO_CHAR(SYSDATE, 'YYYY-MM-DD') || ' \1:\2:\3');
            format_str := 'YYYY-MM-DD HH24:MI:SS';
        ELSIF REGEXP_INSTR(temp_exp, 
            '^\{ts[[:space:]]*''([[:digit:]]{4})-([[:digit:]]{2})-([[:digit:]]{2})[[:space:]]*' ||
            '([[:digit:]]{2}):([[:digit:]]{2}):([[:digit:]]{2})(\.[[:digit:]]{3})?''\}$',
            1, 1, 0, 'i') = 1 THEN -- ISO861 format
            -- e.g. { ts '2005-05-23 14:25:10'} 
            temp_exp := REGEXP_REPLACE(temp_exp, 
                        '^\{ts[[:space:]]*''([[:digit:]]{4})-([[:digit:]]{2})-([[:digit:]]{2})[[:space:]]*' ||
                        '([[:digit:]]{2}):([[:digit:]]{2}):([[:digit:]]{2})(\.[[:digit:]]{3})?''\}$', 
                        '\1-\2-\3 \4:\5:\6');
            format_str := 'YYYY-MM-DD HH24:MI:SS';
        ELSIF REGEXP_INSTR(temp_exp, 
            '^([[:digit:]]{4})-([[:digit:]]{2})-([[:digit:]]{2})T([[:digit:]]{2}):([[:digit:]]{2}):([[:digit:]]{2})(\.[[:digit:]]{3})?$') = 1 THEN -- ISO861 format
            -- e.g. 2004-05-23T14:25:10.487
            temp_exp := REGEXP_REPLACE(temp_exp, 
                       '^([[:digit:]]{4})-([[:digit:]]{2})-([[:digit:]]{2})T([[:digit:]]{2}):([[:digit:]]{2}):([[:digit:]]{2})(\.[[:digit:]]{3})?$', 
                       '\1-\2-\3 \4:\5:\6');
            format_str := 'YYYY-MM-DD HH24:MI:SS';
        ELSIF REGEXP_INSTR(temp_exp, 
            '^([[:digit:]]{1,2})[[:space:]]*(am|pm)$', 
            1, 1, 0, 'i') = 1 THEN -- time format
           -- e.g. 4PM or 4 pm
           temp_exp := REGEXP_REPLACE(temp_exp, 
                       '^([[:digit:]]{1,2})[[:space:]]*(am|pm)$', 
                       TO_CHAR(SYSDATE, 'YYYY-MM-DD') || ' \1\2', 1, 1, 'i');
           format_str := 'YYYY-MM-DD HH12' || UPPER(REGEXP_SUBSTR(temp_exp, '(am|pm)$', 1, 1, 'i'));
        ELSIF REGEXP_INSTR(temp_exp, 
            '^([[:digit:]]{1,2}):([[:digit:]]{2})(:[[:digit:]]{2})?([\.:][[:digit:]]{1,3})?[[:space:]]*(am|pm)?$',
            1, 1, 0, 'i') = 1 THEN -- time format
           -- e.g. 14:30 or 14:30:20.9 or 4:30:10 PM
           temp_exp := REGEXP_REPLACE(temp_exp, 
                       '^([[:digit:]]{1,2})(:[[:digit:]]{2})(:[[:digit:]]{2})?([\.:][[:digit:]]{1,3})?[[:space:]]*(am|pm)?$', 
                       TO_CHAR(SYSDATE, 'YYYY-MM-DD') || '\1\2\3\5', 1, 1, 'i');
           format_str := 'YYYY-MM-DD HH24:MI:SS';
           IF REGEXP_INSTR(temp_exp, '(am|pm)$', 1, 1, 0, 'i') <> 0 THEN
              format_str := REPLACE(format_str, 'HH24', 'HH12') || UPPER(REGEXP_SUBSTR(temp_exp, '(am|pm)$', 1, 1, 'i'));
           END IF;
        ELSIF REGEXP_INSTR(temp_exp, '^([[:digit:]]{4})$') = 1 THEN -- unseparated string format
           -- 4 digits is interpreted as year, century must be specified
           temp_exp := REGEXP_REPLACE(temp_exp, 
                        '^([[:digit:]]{4})$', 
                       '(01) 01 \1');
           format_str := '(DD) MM YYYY';
        ELSIF REGEXP_INSTR(temp_exp, '^[[:digit:]]{6,8}$') = 1 THEN -- unseparated string format
           IF LENGTH(temp_exp) = 6 THEN
              format_str := 'YYYYMMDD';
              -- default two-digit year cutoff is 2050 i.e. 
              -- if the two digit year is greater than 50 the century prefix is interpreted as 19 otherwise it is 20.
              IF TO_NUMBER(SUBSTR(temp_exp, 1, 2)) > 50 THEN
                temp_exp := '19' || temp_exp;
              ELSE
                temp_exp := '20' || temp_exp;
              END IF;
           ELSE
              format_str := 'YYYYMMDD';
           END IF;
        ELSIF REGEXP_INSTR(temp_exp, '[-/\\.]') = 0 THEN --  alphanumeric date format
           IF REGEXP_INSTR(temp_exp, 
                 '^([[:alpha:]]+)[[:space:]]*,?[[:space:]]*([[:digit:]]{4})$') = 1 THEN 
              -- e.g. APRIL, 1996 or APR  1996
              temp_exp := REGEXP_REPLACE(temp_exp, 
                        '^([[:alpha:]]+)[[:space:]]*,?[[:space:]]*([[:digit:]]{4})$', 
                       '(01) \1 \2');
           ELSIF REGEXP_INSTR(temp_exp, 
                 '^([[:alpha:]]+)[[:space:]]+([[:digit:]]{1,2})?,?[[:space:]]+([[:digit:]]{2,4})$') = 1 THEN 
              -- e.g. APRIL 15, 1996 or APR 15 96
              temp_exp := REGEXP_REPLACE(temp_exp, 
                        '^([[:alpha:]]+)[[:space:]]+([[:digit:]]{1,2})?,?[[:space:]]+([[:digit:]]{2,4})$', 
                       '(\2) \1 \3');
           ELSIF REGEXP_INSTR(temp_exp,
                      '^([[:alpha:]]+)[[:space:]]+([[:digit:]]{2,4})([[:space:]]+)?([[:digit:]]{1,2})?$') = 1 THEN
              -- e.g. APRIL 1996 or APRIL 1996 15
              temp_exp := REGEXP_REPLACE(temp_exp, 
                           '^([[:alpha:]]+)[[:space:]]+([[:digit:]]{2,4})([[:space:]]+)?([[:digit:]]{1,2})?$', 
                       '(\4) \1 \2');     
           ELSIF REGEXP_INSTR(temp_exp,
                      '^([[:digit:]]{1,2})?[[:space:]]+([[:alpha:]]+),?[[:space:]]+([[:digit:]]{2,4})$') = 1 THEN
              -- e.g. 15 APR, 1996 or 15 APR 96 or APRIL 1996
              temp_exp := REGEXP_REPLACE(temp_exp, 
                           '^([[:digit:]]{1,2})?[[:space:]]+([[:alpha:]]+),?[[:space:]]+([[:digit:]]{2,4})$', 
                       '(\1) \2 \3'); 
              temp_exp := REPLACE(temp_exp, ',', '');
           ELSIF REGEXP_INSTR(temp_exp,
                      '^([[:digit:]]{1,2})?[[:space:]]+([[:digit:]]{2,4})[[:space:]]+([[:alpha:]]+)$') = 1 THEN
              -- e.g. 15 1996 APRIL or 1996 APR 
              temp_exp := REGEXP_REPLACE(temp_exp, 
                             '^([[:digit:]]{1,2})?[[:space:]]+([[:digit:]]{2,4})[[:space:]]+([[:alpha:]]+)$',
                             '(\1) \3 \2');
           ELSIF REGEXP_INSTR(temp_exp,
                      '^([[:digit:]]{2,4})[[:space:]]+([[:alpha:]]+)([[:space:]]+)?([[:digit:]]{1,2})?$') = 1 THEN
              -- e.g. 1996 APRIL 15 or 1996 APR
              temp_exp := REGEXP_REPLACE(temp_exp,
                             '^([[:digit:]]{2,4})[[:space:]]+([[:alpha:]]+)([[:space:]]+)?([[:digit:]]{1,2})?$',
                             '(\4) \2 \1');
           ELSIF REGEXP_INSTR(temp_exp,
                      '^([[:digit:]]{2,4})[[:space:]]+([[:digit:]]{1,2})?[[:space:]]+([[:alpha:]]+)$') = 1 THEN
              -- e.g. 1996 15 APRIL or 1996 APR
              temp_exp := REGEXP_REPLACE(temp_exp,
                             '^([[:digit:]]{2,4})[[:space:]]+([[:digit:]]{1,2})?[[:space:]]+([[:alpha:]]+)$',
                             '(\2) \3 \1');                            
           END IF;            
         
           temp_exp := REPLACE(temp_exp, '()', '(1)');
           IF TO_NUMBER(REGEXP_SUBSTR(temp_exp, '[[:digit:]]{2,4}$')) < 100 THEN
              IF TO_NUMBER(REGEXP_SUBSTR(temp_exp, '[[:digit:]]{2,4}$')) > 50 THEN
                temp_exp := REGEXP_REPLACE(temp_exp, '([[:digit:]]{2,4})$', '19' || '\1');
              ELSE
                temp_exp := REGEXP_REPLACE(temp_exp, '([[:digit:]]{2,4})$', '20' || '\1');
              END IF;
           END IF;
           format_str := '(DD) MON YYYY';
        ELSIF REGEXP_INSTR(temp_exp, '[-/\\.]') <> 0 THEN -- numeric date format
           -- require the setting for SET FORMAT to determine the interpretation of the numeric date format,
           -- default is mdy
           IF REGEXP_INSTR(temp_exp, 
              -- e.g. 4/15/1996 or 15/4/1996 or 4/96/15
                 '^([[:digit:]]{1,2})[-/\.]([[:digit:]]{1,2})[-/\.]([[:digit:]]{2,4})$') = 1 THEN
              temp_exp := REGEXP_REPLACE(temp_exp,
                             '^([[:digit:]]{1,2})[-/\.]([[:digit:]]{1,2})[-/\.]([[:digit:]]{2,4})$',
                             '\1/\2/\3');        
           ELSIF REGEXP_INSTR(temp_exp, 
                      '^([[:digit:]]{1,2})[-/\.]([[:digit:]]{2,4})[-/\.]([[:digit:]]{1,2})$') = 1 THEN
              -- e.g. 15/96/4
              temp_exp := REGEXP_REPLACE(temp_exp,
                             '^([[:digit:]]{1,2})[-/\.]([[:digit:]]{2,4})[-/\.]([[:digit:]]{1,2})$',
                             '\1/\3/\2');     
           ELSIF REGEXP_INSTR(temp_exp, 
                      '^([[:digit:]]{2,4})[-/\.]([[:digit:]]{1,2})[-/\.]([[:digit:]]{1,2})$') = 1 THEN
              -- e.g. 1996/4/15 or 1996/15/4
              temp_exp := REGEXP_REPLACE(temp_exp,
                             '^([[:digit:]]{2,4})[-/\.]([[:digit:]]{1,2})[-/\.]([[:digit:]]{1,2})$',
                             '\2/\3/\1'); 
           END IF;
         
           -- first component
           temp_val := TO_NUMBER(SUBSTR(temp_exp, 1, INSTR(temp_exp, '/') - 1));
           IF temp_val > 31 AND temp_val < 100 THEN
              format_str := 'YYYY/';
              IF temp_val > 50 THEN
                temp_exp := '19' || temp_exp;
              ELSE
                temp_exp := '20' || temp_exp;
              END IF;
           ELSIF temp_val > 12 THEN
              format_str := 'DD/';
           ELSE
              format_str := 'MM/';
           END IF;
          
           -- second component
           temp_val := TO_NUMBER(SUBSTR(temp_exp, INSTR(temp_exp, '/') + 1, INSTR(temp_exp, '/', 1, 2) - INSTR(temp_exp, '/') - 1));
           IF temp_val > 31 AND temp_val < 100 THEN
              format_str := format_str || 'YYYY/';
              IF temp_val > 50 THEN
                temp_exp := REGEXP_REPLACE(temp_exp, '/([[:digit:]]{2,4})/', '/19' || '\1/');
              ELSE
                temp_exp := REGEXP_REPLACE(temp_exp, '/([[:digit:]]{2,4})/', '/20' || '\1/');
              END IF;
           ELSIF temp_val > 12 THEN
              format_str := format_str || 'DD/';
           ELSE
              IF INSTR(format_str, 'MM') > 0 THEN
                 format_str := format_str || 'DD';
              ELSE
                 format_str := format_str || 'MM/';
              END IF;
           END IF;
          
           IF INSTR(format_str, 'MM') = 0 THEN
              format_str := format_str || 'MM';
           ELSIF INSTR(format_str, 'DD') = 0 THEN
              format_str := format_str || 'DD';
           ELSE
              IF TO_NUMBER(REGEXP_SUBSTR(temp_exp, '[[:digit:]]{2,4}$')) < 100 THEN
                IF TO_NUMBER(REGEXP_SUBSTR(temp_exp, '[[:digit:]]{2,4}$')) > 50 THEN
                  temp_exp := REGEXP_REPLACE(temp_exp, '([[:digit:]]{2,4})$', '19' || '\1');
                ELSE
                  temp_exp := REGEXP_REPLACE(temp_exp, '([[:digit:]]{2,4})$', '20' || '\1');
                END IF;
              END IF;
              format_str := format_str || '/YYYY';
           END IF;
        END IF;
      END IF;
      
      IF format_str IS NOT NULL THEN
         RETURN TO_DATE(temp_exp, format_str);
      ELSE 
         RETURN TO_DATE(temp_exp, 'DD-MON-YYYY HH24:MI:SS');
      END IF;
   EXCEPTION
      WHEN OTHERS THEN
         RETURN NULL;
   END str_to_date;
FUNCTION convert_(p_dataType IN VARCHAR2, p_expr IN VARCHAR2, p_style IN VARCHAR2 DEFAULT NULL)
RETURN VARCHAR2
IS
     v_ret_value VARCHAR2(50);
     v_format VARCHAR2(30);
     v_year_format VARCHAR2(5) := 'YY';
     v_format_type NUMBER;
     v_numeric_dataType BOOLEAN := TRUE;
     v_is_valid_date BINARY_INTEGER := 0;
BEGIN
    IF INSTR(UPPER(p_dataType), 'DATE') <> 0 OR INSTR(UPPER(p_dataType), 'CHAR') <> 0 OR
	INSTR(UPPER(p_dataType), 'CLOB') <> 0 THEN
	 v_numeric_dataType := FALSE;
    END IF;

    IF NOT v_numeric_dataType THEN
    	SELECT NVL2(TO_DATE(p_expr), 1, 0) INTO v_is_valid_date FROM DUAL;
  	END IF;
	
	IF (str_to_date(p_expr) IS NOT NULL OR v_is_valid_date != 0 ) THEN
	   IF p_style IS NULL THEN
          v_ret_value := TO_NCHAR(p_expr);
       ELSE -- convert date to character data
          v_format_type := TO_NUMBER(p_style);
          IF v_format_type > 100 THEN
            v_year_format := 'YYYY';	
          END IF;
          
          v_format := CASE 
                WHEN v_format_type = 1 OR v_format_type = 101 THEN 'MM/DD/' || v_year_format
                WHEN v_format_type = 2 OR v_format_type = 102 THEN v_year_format || '.MM.DD'
                WHEN v_format_type = 3 OR v_format_type = 103 THEN 'DD/MM/' || v_year_format
                WHEN v_format_type = 4 OR v_format_type = 104 THEN 'DD.MM.' || v_year_format
                WHEN v_format_type = 5 OR v_format_type = 105 THEN 'DD-MM-' || v_year_format
                WHEN v_format_type = 6 OR v_format_type = 106 THEN 'DD MM ' || v_year_format
                WHEN v_format_type = 7 OR v_format_type = 107 THEN 'MON DD, ' || v_year_format
                WHEN v_format_type = 8 OR v_format_type = 108 THEN 'HH12:MI:SS'
                WHEN v_format_type = 9 OR v_format_type = 109 THEN 'MON DD YYYY HH12:MI:SS.FF3AM'
                WHEN v_format_type = 10 OR v_format_type = 110 THEN 'MM-DD-' || v_year_format
                WHEN v_format_type = 11 OR v_format_type = 111 THEN v_year_format || '/MM/DD'
                WHEN v_format_type = 12 OR v_format_type = 112 THEN v_year_format || 'MMDD'
                WHEN v_format_type = 13 OR v_format_type = 113 THEN 'DD MON YYYY HH12:MI:SS.FF3'
                WHEN v_format_type = 14 OR v_format_type = 114 THEN 'HH24:MI:SS.FF3'
                WHEN v_format_type = 20 OR v_format_type = 120 THEN 'YYYY-MM-DD HH24:MI:SS'
                WHEN v_format_type = 21 OR v_format_type = 121 THEN 'YYYY-MM-DD HH24:MI:SS.FF3'
                WHEN v_format_type = 126 THEN 'YYYY-MM-DD HH12:MI:SS.FF3'
		        WHEN v_format_type = 127 THEN 'YYYY-MM-DD HH12:MI:SS.FF3'
                WHEN v_format_type = 130 THEN 'DD MON YYYY HH12:MI:SS:FF3AM'
                WHEN v_format_type = 131 THEN 'DD/MM/YY HH12:MI:SS:FF3AM'
              END;   

		v_ret_value := CASE 
			WHEN v_format_type = 9 OR v_format_type = 109 OR
				v_format_type = 13 OR v_format_type = 113 OR
				v_format_type = 14 OR v_format_type = 114 OR
				v_format_type = 20 OR v_format_type = 120 OR
				v_format_type = 21 OR v_format_type = 121 OR
				v_format_type = 126 OR v_format_type = 127 OR
				v_format_type = 130 OR v_format_type = 131 THEN
				
				CASE UPPER(p_dataType)
					WHEN 'DATE' THEN TO_CHAR(TO_TIMESTAMP(p_expr, v_format)) 
                           	ELSE TO_CHAR(TO_TIMESTAMP(p_expr), v_format)  
				END
			ELSE
				CASE UPPER(p_dataType)
					WHEN 'DATE' THEN TO_CHAR(TO_DATE(p_expr, v_format))
                    	      ELSE TO_CHAR(TO_DATE(p_expr), v_format) 
				END
		 	END;
       END IF;
    ELSE 
       -- convert money or smallmoney to character data
       IF SUBSTR(p_expr, 1, 1) = '$' THEN          
          v_ret_value := CASE TO_NUMBER(NVL(p_style, 1)) 
                     WHEN 1 THEN TO_CHAR(SUBSTR(p_expr, 2), '999999999999999990.00')
                     WHEN 2 THEN TO_CHAR(SUBSTR(p_expr, 2), '999,999,999,999,999,990.00')
                     WHEN 3 THEN TO_CHAR(SUBSTR(p_expr, 2), '999999999999999990.0000')
                    END;
       ELSE -- convert numeric data to character data
          v_ret_value := TO_CHAR(p_expr);
       END IF;
    END IF;
    
    RETURN v_ret_value;
EXCEPTION
    WHEN OTHERS THEN
       raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END convert_;
FUNCTION year_(p_date_str IN VARCHAR2)
RETURN NUMBER
IS
    v_date DATE;
BEGIN
    v_date := str_to_date(p_date_str);
    IF v_date IS NULL THEN
      RETURN NULL;
    END IF;
    
    RETURN TO_NUMBER(TO_CHAR(v_date, 'YY'));
EXCEPTION
    WHEN OTHERS THEN
      raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END year_;
FUNCTION stuff(p_expr VARCHAR2, p_startIdx NUMBER, p_len NUMBER, p_replace_expr VARCHAR2) 
RETURN VARCHAR2
IS
BEGIN
       RETURN REPLACE(p_expr, SUBSTR(p_expr, p_startIdx, p_len), p_replace_expr);
EXCEPTION
        WHEN OTHERS THEN
          raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END stuff;
FUNCTION dateadd(p_interval IN VARCHAR2, p_interval_val IN NUMBER, p_date_str IN VARCHAR2) 
RETURN DATE
IS
    v_ucase_interval VARCHAR2(10);
    v_date DATE;
BEGIN
    v_date := str_to_date(p_date_str);
    v_ucase_interval := UPPER(p_interval);
      
    IF v_ucase_interval IN ('YEAR', 'YY', 'YYYY') 
    THEN
      RETURN ADD_MONTHS(v_date, p_interval_val * 12);
    ELSIF v_ucase_interval IN ('QUARTER', 'QQ', 'Q') 
    THEN
      RETURN ADD_MONTHS(v_date, p_interval_val * 3);
    ELSIF v_ucase_interval IN ('MONTH', 'MM', 'M') 
    THEN
      RETURN ADD_MONTHS(v_date, p_interval_val);
    ElSIF v_ucase_interval IN ('DAYOFYEAR', 'DY', 'Y', 'DAY', 'DD', 'D', 'WEEKDAY', 'DW', 'W') 
    THEN
      RETURN v_date + p_interval_val;
    ElSIF v_ucase_interval IN ('WEEK', 'WK', 'WW') 
    THEN
      RETURN v_date + (p_interval_val * 7);
    ElSIF v_ucase_interval IN ('HOUR', 'HH') 
    THEN
      RETURN v_date + (p_interval_val / 24);
    ElSIF v_ucase_interval IN ('MINUTE', 'MI', 'N') 
    THEN
      RETURN v_date + (p_interval_val / 24 / 60);
    ElSIF v_ucase_interval IN ('SECOND', 'SS', 'S') 
    THEN
      RETURN v_date + (p_interval_val / 24 / 60 / 60);
    ElSIF v_ucase_interval IN ('MILLISECOND', 'MS') 
    THEN
      RETURN v_date + (p_interval_val / 24 / 60 / 60 / 1000);
    ELSE
      RETURN NULL;
    END IF;
EXCEPTION
    WHEN OTHERS THEN
      raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END dateadd;
FUNCTION isdate(p_expr IN VARCHAR2)
RETURN NUMBER
IS
     v_is_valid_date BINARY_INTEGER := 0;
BEGIN
    IF str_to_date(p_expr) IS NOT NULL THEN
       RETURN 1;
    ELSE 
       SELECT NVL2(TO_DATE(p_expr), 1, 0) INTO v_is_valid_date FROM DUAL;
       RETURN v_is_valid_date;
    END IF;    
EXCEPTION
    WHEN OTHERS THEN
       RETURN 0;
END isdate;
FUNCTION stats_date(p_table IN VARCHAR2, p_index IN VARCHAR2)
RETURN DATE
IS
    v_last_analyzed DATE;
BEGIN
    SELECT last_analyzed INTO v_last_analyzed
      FROM USER_IND_STATISTICS
     WHERE table_name LIKE UPPER(p_table)
       AND index_name LIKE UPPER(p_index);
  
    RETURN v_last_analyzed;
EXCEPTION
    WHEN OTHERS THEN
       raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END stats_date;
FUNCTION rand(p_seed NUMBER DEFAULT NULL)
RETURN NUMBER
IS
    v_rand_num NUMBER;
BEGIN
      IF p_seed IS NOT NULL THEN
         DBMS_RANDOM.SEED(p_seed);
      END IF;
      
      v_rand_num := DBMS_RANDOM.VALUE();
      
      RETURN v_rand_num;
EXCEPTION
     WHEN OTHERS THEN
       raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END rand;
FUNCTION to_base(p_dec NUMBER, p_base NUMBER) 
RETURN VARCHAR2
IS
    v_str VARCHAR2(255);
    v_num NUMBER;
    v_hex VARCHAR2(16) DEFAULT '0123456789ABCDEF';
BEGIN
    v_num := p_dec;
    
    IF p_dec IS NULL OR p_base IS NULL THEN
      RETURN NULL;
    END IF;

    IF TRUNC(p_dec) <> p_dec OR p_dec < 0 THEN
        RAISE PROGRAM_ERROR;
    END IF;
    
    LOOP
      v_str := SUBSTR(v_hex, MOD(v_num, p_base) + 1, 1) || v_str;
      v_num := TRUNC(v_num / p_base);
      
      EXIT WHEN v_num = 0;
    END LOOP;
    
    RETURN v_str;
EXCEPTION
    WHEN OTHERS THEN
      raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END to_base;
FUNCTION patindex(p_pattern IN VARCHAR2, p_expr IN VARCHAR2)
RETURN NUMBER
IS
    v_search_pattern VARCHAR2(100);
    v_pos NUMBER := 0;
BEGIN
      IF p_pattern IS NULL OR p_expr IS NULL THEN
         RETURN NULL;
      END IF;
      
      IF NOT DBMS_DB_VERSION.VER_LE_9_2 THEN
        v_search_pattern := p_pattern;
        v_search_pattern := REPLACE(v_search_pattern, '\', '\\');
        v_search_pattern := REPLACE(v_search_pattern, '*', '\*');
        v_search_pattern := REPLACE(v_search_pattern, '+', '\+');
        v_search_pattern := REPLACE(v_search_pattern, '?', '\?');
        v_search_pattern := REPLACE(v_search_pattern, '|', '\|');
        v_search_pattern := REPLACE(v_search_pattern, '^', '\^');
        v_search_pattern := REPLACE(v_search_pattern, '$', '\$');
        v_search_pattern := REPLACE(v_search_pattern, '.', '\.');
        v_search_pattern := REPLACE(v_search_pattern, '{', '\{');
        v_search_pattern := REPLACE(v_search_pattern, '_', '.');
        
        IF SUBSTR(v_search_pattern, 1, 1) != '%' AND 
              SUBSTR(v_search_pattern, -1, 1) != '%' THEN
           v_search_pattern := '^' || v_search_pattern || '$';
        ELSIF SUBSTR(v_search_pattern, 1, 1) != '%' THEN
           v_search_pattern := '^' || SUBSTR(v_search_pattern, 1, LENGTH(v_search_pattern) - 1);
        ELSIF SUBSTR(v_search_pattern, -1, 1) != '%' THEN
           v_search_pattern := SUBSTR(v_search_pattern, 2) || '$';
        ELSE
           v_search_pattern := SUBSTR(v_search_pattern, 2, LENGTH(v_search_pattern) - 2);
        END IF;
        
        v_pos := REGEXP_INSTR(p_expr, v_search_pattern);
      ELSE 
        v_pos := 0;
      END IF;
      
      RETURN v_pos;
EXCEPTION
    WHEN OTHERS THEN
      raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END patindex;
FUNCTION datediff(p_datepart VARCHAR2, p_start_date_str VARCHAR2, p_end_date_str VARCHAR2)
RETURN NUMBER
IS
    v_ret_value NUMBER := NULL;
    v_part VARCHAR2(15);
    v_start_date DATE;
    v_end_date DATE;
BEGIN
      v_start_date := str_to_date(p_start_date_str);
      v_end_date := str_to_date(p_end_date_str);
      v_part := UPPER(p_datepart);
   
      IF v_part IN ('YEAR', 'YY', 'YYYY') THEN
        IF EXTRACT(YEAR FROM v_end_date) - EXTRACT(YEAR FROM v_start_date) = 1 AND
          EXTRACT(MONTH FROM v_start_date) = 12 AND EXTRACT(MONTH FROM v_end_date) = 1 AND
          EXTRACT(DAY FROM v_start_date) = 31 AND EXTRACT(DAY FROM v_end_date) = 1 THEN
          -- When comparing December 31 to January 1 of the immediately succeeding year, 
          -- DateDiff for Year ("yyyy") returns 1, even though only a day has elapsed.
          v_ret_value := 1;
        ELSE
          v_ret_value := ROUND(MONTHS_BETWEEN(v_end_date, v_start_date) / 12);
        END IF;
      ELSIF v_part IN ('QUARTER', 'QQ', 'Q') THEN
         v_ret_value := ROUND(MONTHS_BETWEEN(v_end_date, v_start_date) / 3);
      ELSIF v_part IN ('MONTH', 'MM', 'M') THEN
         v_ret_value := ROUND(MONTHS_BETWEEN(v_end_date, v_start_date));
      ElSIF v_part IN ('DAYOFYEAR', 'DY', 'Y') THEN
         v_ret_value := ROUND(v_end_date - v_start_date);
      ElSIF v_part IN ('DAY', 'DD', 'D') THEN
         v_ret_value := ROUND(v_end_date - v_start_date);
      ElSIF v_part IN ('WEEK', 'WK', 'WW') THEN
         v_ret_value := ROUND((v_end_date - v_start_date) / 7);
      ELSIF v_part IN ('WEEKDAY', 'DW', 'W') THEN
         v_ret_value := TO_CHAR(v_end_date, 'D') - TO_CHAR(v_start_date, 'D');
      ElSIF v_part IN ('HOUR', 'HH') THEN
         v_ret_value := ROUND((v_end_date - v_start_date) * 24);
      ElSIF v_part IN ('MINUTE', 'MI', 'N') THEN
         v_ret_value := ROUND((v_end_date - v_start_date) * 24 * 60);
      ElSIF v_part IN ('SECOND', 'SS', 'S') THEN
         v_ret_value := ROUND((v_end_date - v_start_date) * 24 * 60 * 60);
      ElSIF v_part IN ('MILLISECOND', 'MS') THEN
         v_ret_value := ROUND((v_end_date - v_start_date) * 24 * 60 * 60 * 1000);
      END IF;
      
      RETURN v_ret_value;
EXCEPTION
     WHEN OTHERS THEN
      raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END datediff;
FUNCTION day_(p_date_str IN VARCHAR2)
RETURN NUMBER
IS
    v_date DATE;
BEGIN
    v_date := str_to_date(p_date_str);
    IF v_date IS NULL THEN
      RETURN NULL;
    END IF;
    
    RETURN TO_NUMBER(TO_CHAR(v_date, 'DD'));
EXCEPTION
    WHEN OTHERS THEN
      raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END day_;
FUNCTION ident_incr(p_sequence IN VARCHAR2)
RETURN NUMBER
IS
    v_incr_by NUMBER;
BEGIN
    SELECT increment_by INTO v_incr_by
       FROM USER_SEQUENCES
       WHERE sequence_name LIKE UPPER(p_sequence);

    RETURN v_incr_by;
EXCEPTION
    WHEN OTHERS THEN
       raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END ident_incr;
FUNCTION isnumeric(p_expr IN VARCHAR2)
RETURN NUMBER
IS
    numeric_val NUMBER;
    temp_str VARCHAR2(50);
BEGIN
    temp_str := p_expr;
    IF SUBSTR(temp_str, 1, 1) = '$' THEN
       temp_str := SUBSTR(temp_str, 2);
    END IF;
    
    numeric_val := TO_NUMBER(temp_str);
    RETURN 1;
EXCEPTION 
    WHEN OTHERS THEN
       RETURN 0;
END isnumeric;
FUNCTION hex(p_num VARCHAR2)
RETURN VARCHAR2
IS
BEGIN
    RETURN to_base(p_num, 16);
  EXCEPTION
    WHEN OTHERS THEN
      raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END hex;
FUNCTION difference(p_expr1 IN VARCHAR2, p_expr2 IN VARCHAR2)
RETURN NUMBER
IS
    sound_ex_val_1 CHAR(4);
    sound_ex_val_2 CHAR(4);
    similarity NUMBER := 0;
    idx NUMBER := 1; 
BEGIN
    IF p_expr1 IS NULL OR p_expr2 IS NULL THEN
       RETURN NULL;
    END IF;
    
    sound_ex_val_1 := SOUNDEX(p_expr1);
    sound_ex_val_2 := SOUNDEX(p_expr2);
    
    LOOP
       IF SUBSTR(sound_ex_val_1, idx, 1) = SUBSTR(sound_ex_val_2, idx, 1) THEN
          similarity := similarity + 1;
       END IF;
       
       idx := idx + 1;   
       EXIT WHEN idx > 4;
    END LOOP;
    
    RETURN similarity;
EXCEPTION
    WHEN OTHERS THEN
       raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END difference;
FUNCTION datepart(p_part_expr IN VARCHAR2, p_date_str IN VARCHAR2) 
RETURN NUMBER
IS
    v_part VARCHAR2(15);
    v_date DATE;
BEGIN
      v_date := str_to_date(p_date_str);
      v_part := UPPER(p_part_expr);
   
      IF v_part IN ('YEAR', 'YY', 'YYYY') THEN  RETURN TO_NUMBER(TO_CHAR(v_date, 'YYYY'));
      ELSIF v_part IN ('QUARTER', 'QQ', 'Q')  THEN RETURN TO_NUMBER(TO_CHAR(v_date, 'Q'));
      ELSIF v_part IN ('MONTH', 'MM', 'M') THEN RETURN TO_NUMBER(TO_CHAR(v_date, 'MM'));
      ElSIF v_part IN ('DAYOFYEAR', 'DY', 'Y') THEN RETURN TO_NUMBER(TO_CHAR(v_date, 'DDD'));
      ELSIF v_part IN ('DAY', 'DD', 'D') THEN RETURN TO_NUMBER(TO_CHAR(v_date, 'DD'));
      ELSIF v_part IN ('WEEKDAY', 'DW', 'W') THEN RETURN TO_NUMBER(TO_CHAR(v_date, 'D'));
      ElSIF v_part IN ('WEEK', 'WK', 'WW') THEN RETURN TO_NUMBER(TO_CHAR(v_date, 'IW'));
      ElSIF v_part IN ('HOUR', 'HH') THEN RETURN TO_NUMBER(TO_CHAR(v_date, 'HH24'));
      ElSIF v_part IN ('MINUTE', 'MI', 'N') THEN RETURN TO_NUMBER(TO_CHAR(v_date, 'MI'));
      ElSIF v_part IN ('SECOND', 'SS', 'S') THEN RETURN TO_NUMBER(TO_CHAR(v_date, 'SS'));
      ElSIF v_part IN ('MILLISECOND', 'MS') THEN RETURN TO_NUMBER(TO_CHAR(v_date, 'FF3'));
      ELSE
        RETURN NULL;
      END IF;
EXCEPTION
    WHEN OTHERS THEN
      raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END datepart;
FUNCTION radians(p_degree IN NUMBER)
RETURN NUMBER
IS
    v_rad NUMBER;
BEGIN
    v_rad := p_degree / 180 * pi();
    RETURN v_rad;
EXCEPTION
    WHEN OTHERS THEN
      raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END radians;
FUNCTION reverse_(p_expr IN VARCHAR2)
RETURN VARCHAR2
IS
    v_result VARCHAR2(2000) := NULL;
BEGIN      
    FOR i IN 1..LENGTH(p_expr) LOOP
      v_result := v_result || SUBSTR(p_expr, -i, 1);
    END LOOP;
  
    RETURN v_result;    
EXCEPTION 
    WHEN OTHERS THEN
      raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END reverse_;
FUNCTION parsename(p_object_name IN VARCHAR2, p_object_piece IN NUMBER)
RETURN VARCHAR2
IS
    ret_val VARCHAR2(150) := NULL;
    pos NUMBER;
    v_next_pos NUMBER;
BEGIN
    IF p_object_name IS NULL THEN 
       RETURN NULL;
    END IF;
    
    -- for 10g
    IF NOT DBMS_DB_VERSION.VER_LE_9_2 THEN
      IF p_object_piece = 1 THEN -- object name
         ret_val := REGEXP_SUBSTR(p_object_name, '(^[^\.]+$)|(\.[^\.]+$)');
         ret_val := REPLACE(ret_val, '.', '');
      ELSIF p_object_piece = 2 THEN -- schema name
         ret_val := REGEXP_SUBSTR(p_object_name, '([^\.]+)\.([^\.]+$)');
         ret_val := REGEXP_REPLACE(ret_val, '\.([^\.]+$)', '');
      ELSIF p_object_piece = 3 THEN -- database name
         ret_val := REGEXP_SUBSTR(p_object_name, '([^\.]+)\.([^\.]*)\.([^\.]+$)');
         ret_val := REGEXP_REPLACE(ret_val, '\.([^\.]*)\.([^\.]+$)', '');
      ELSIF p_object_piece = 4 THEN -- server name
         ret_val := REGEXP_SUBSTR(p_object_name, '^([^\.]+)\.([^\.]*)\.([^\.]*)\.([^\.]+$)');
         IF ret_val IS NOT NULL THEN
           ret_val := REGEXP_REPLACE(p_object_name, '^([^\.]+)\.([^\.]*)\.([^\.]*)\.([^\.]+$)', '\1');
         END IF;
      END IF;
    ELSE
      ret_val := p_object_name;
      v_next_pos := LENGTH(p_object_name);
      FOR i IN 1 .. p_object_piece LOOP
        pos := INSTR(p_object_name, '.', -1, i);
        IF pos > 0 THEN
          ret_val := SUBSTR(p_object_name, pos + 1, v_next_pos - pos);
        END IF;
        v_next_pos := pos;
      END LOOP;
      
      IF LENGTH(ret_val) = 0 THEN
        RETURN NULL;
      END IF;
    END IF;
    
    RETURN ret_val;
EXCEPTION
    WHEN OTHERS THEN
       raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END parsename;
FUNCTION month_(p_date_str IN VARCHAR2)
RETURN NUMBER
IS
    v_date DATE;
BEGIN
    v_date := str_to_date(p_date_str);
    IF v_date IS NULL THEN
      RETURN NULL;
    END IF;
    
    RETURN TO_NUMBER(TO_CHAR(v_date, 'MM'));
EXCEPTION
    WHEN OTHERS THEN
      raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END month_;
FUNCTION round_(p_expr NUMBER, p_len NUMBER, p_function NUMBER DEFAULT 0) 
RETURN NUMBER
IS
    v_ret_value NUMBER;
BEGIN
      IF p_function = 0 THEN
         v_ret_value := ROUND(p_expr, p_len);
      ELSE
         v_ret_value := TRUNC(p_expr, p_len);
      END IF;
      
      RETURN v_ret_value;
EXCEPTION
     WHEN OTHERS THEN
       raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END round_;
FUNCTION pi
RETURN NUMBER
IS
    pi NUMBER := 3.141592653589793116;
BEGIN
    RETURN pi;
END pi;
FUNCTION oct(p_num VARCHAR2)
RETURN VARCHAR2
IS
BEGIN
    RETURN to_base(p_num, 8);
EXCEPTION
    WHEN OTHERS THEN
      raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END oct;
FUNCTION str(p_expr IN NUMBER, p_len IN NUMBER DEFAULT 10, p_scale IN NUMBER DEFAULT 0) 
RETURN VARCHAR2
IS
    v_ret_val VARCHAR2(50);
    v_temp_val NUMBER;
    v_format_str VARCHAR2(50);
BEGIN
      IF p_len < LENGTH(TO_CHAR(p_expr)) THEN
         RETURN '**';
      END IF;
      
      v_temp_val := p_expr;
      v_temp_val := ROUND(v_temp_val, p_scale);
      
      IF p_scale > 0 THEN
         v_format_str := LPAD(' ', p_len - p_scale, '9');
         v_format_str := TRIM(v_format_str) || '.';
         v_format_str := RPAD(v_format_str, p_len, '0');
      ELSE
         v_format_str := LPAD('', p_len, '9');
      END IF;
      
      v_ret_val := TO_CHAR(v_temp_val, v_format_str);
      RETURN v_ret_val;
EXCEPTION 
      WHEN OTHERS THEN
        raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END str;
FUNCTION degrees(p_angle_radians IN NUMBER) 
RETURN NUMBER
IS
BEGIN
    IF p_angle_radians IS NULL THEN
      RETURN NULL;
    END IF;
    
    RETURN p_angle_radians / pi() * 180;
EXCEPTION 
    WHEN OTHERS THEN
      raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END degrees;
FUNCTION datename(p_part_expr IN VARCHAR2, p_date_str IN VARCHAR2) 
RETURN VARCHAR2
IS
    v_part VARCHAR2(15);
    v_date DATE;
BEGIN
      v_date := str_to_date(p_date_str);
      v_part := UPPER(p_part_expr);
   
      IF v_part IN ('YEAR', 'YY', 'YYYY') THEN RETURN TO_CHAR(v_date, 'YYYY');
      ELSIF v_part IN ('QUARTER', 'QQ', 'Q') THEN RETURN TO_CHAR(v_date, 'Q');
      ELSIF v_part IN ('MONTH', 'MM', 'M') THEN RETURN TO_CHAR(v_date, 'MONTH');
      ElSIF v_part IN ('DAYOFYEAR', 'DY', 'Y') THEN RETURN TO_CHAR(v_date, 'DDD');
      ELSIF v_part IN ('DAY', 'DD', 'D') THEN RETURN TO_CHAR(v_date, 'DD');
      ELSIF v_part IN ('WEEKDAY', 'DW', 'W') THEN RETURN TO_CHAR(v_date, 'DAY');
      ElSIF v_part IN ('WEEK', 'WK', 'WW') THEN RETURN TO_CHAR(v_date, 'IW');
      ElSIF v_part IN ('HOUR', 'HH') THEN RETURN TO_CHAR(v_date, 'HH24');
      ElSIF v_part IN ('MINUTE', 'MI', 'N') THEN RETURN TO_CHAR(v_date, 'MI');
      ElSIF v_part IN ('SECOND', 'SS', 'S') THEN RETURN TO_CHAR(v_date, 'SS');
      ElSIF v_part IN ('MILLISECOND', 'MS') THEN RETURN TO_CHAR(v_date, 'FF3');
      ELSE
        RETURN NULL;
      END IF;
EXCEPTION
    WHEN OTHERS THEN
      raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);  
END datename;
FUNCTION ident_seed(p_sequence IN VARCHAR2)
RETURN NUMBER
IS
    v_seed NUMBER;
BEGIN
      SELECT min_value INTO v_seed
         FROM USER_SEQUENCES
         WHERE sequence_name LIKE UPPER(p_sequence);
  
      RETURN v_seed;
EXCEPTION
    WHEN OTHERS THEN
       raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END ident_seed;
FUNCTION quotename(p_str IN VARCHAR2, p_delimiters IN VARCHAR2 DEFAULT '[]')
RETURN VARCHAR2
IS
    v_ret_val VARCHAR2(150) := NULL;
BEGIN
    IF p_delimiters = '[]' THEN
       v_ret_val := '[' || REPLACE(p_str, ']', ']]') || ']';
    ELSIF p_delimiters = '"' THEN
       v_ret_val := '"' || p_str || '"';
    ELSIF p_delimiters = '''' THEN
       v_ret_val := '''' || p_str || '''';
      END IF;
     
      RETURN v_ret_val;
EXCEPTION
      WHEN OTHERS THEN
         raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END quotename;

FUNCTION fetch_status(p_cursorfound IN BOOLEAN)
RETURN NUMBER
IS
     v_fetch_status NUMBER := 0;
BEGIN
   CASE
     WHEN p_cursorfound THEN
        v_fetch_status := 0;
     ELSE
        v_fetch_status := -1;
     END CASE;
     return v_fetch_status;
EXCEPTION
    WHEN OTHERS THEN
       raise_application_error(-20000, DBMS_UTILITY.FORMAT_ERROR_STACK);
END fetch_status;
END sqlserver_utilities;

/

CREATE SEQUENCE  ZFC_SECURITY_RLS_ROLE_SEQ_ID_SEQ  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE SEQUENCE  ZFC_NAVIGATION_TYPES_NAV_TYPE_  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE SEQUENCE  ZCT_CONTENT_CONTENT_SEQ_ID_SEQ  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE SEQUENCE  ZFC_SECURITY_GRPS_SE_GRPS_SE_I  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE SEQUENCE  ZCT_RATINGS_Rating_id_SEQ  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE SEQUENCE  ZFC_FUNCTION_TYPES_FUNCTION_TY  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE SEQUENCE  ZFC_SECURITY_RLS_SE_RLS_SE_SEQ_ID_  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE SEQUENCE  ZFC_SECURITY_ENTITIES_SE_SEQ_ID_SEQ  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE SEQUENCE  ZFO_MESSAGES_MESSAGE_SEQ_ID_SEQ  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE SEQUENCE  ZFC_INFORMATION_Information_SEQ_ID  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE SEQUENCE  ZFC_ACCTS_ACCT_SEQ_ID_SEQ  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE SEQUENCE  ZFC_NVP_DETAILS_NVP_SEQ_DET_ID  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE SEQUENCE  ZFC_NVP_NVP_SEQ_ID_SEQ  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE SEQUENCE  ZFC_PERMISSIONS_PERMISSIONS_ID  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE SEQUENCE  ZFC_FUNCTIONS_FUNCTION_SEQ_ID_  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE SEQUENCE  ZFC_SYSTEM_STATUS_STATUS_SEQ_ID_SE  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE SEQUENCE  ZFC_SECURITY_GRPS_GROUP_SEQ_ID_SEQ  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE SEQUENCE  ZOP_WORK_FLOWS_WORK_FLOW_ID_SE  
  MINVALUE 1 MAXVALUE 999999999999999999999999 INCREMENT BY 1  NOCYCLE ;

CREATE TABLE ZFC_SECURITY_GRPS (
  GROUP_SEQ_ID NUMBER(10,0) NOT NULL,
  NAME VARCHAR2(128 CHAR) NOT NULL,
  DESCRIPTION VARCHAR2(512 CHAR) NOT NULL,
  ADDED_BY NUMBER(10,0) NOT NULL,
  ADDED_DATE DATE NOT NULL,
  UPDATED_BY NUMBER(10,0) NOT NULL,
  UPDATED_DATE DATE DEFAULT SYSDATE NOT NULL
);


ALTER TABLE ZFC_SECURITY_GRPS
ADD CONSTRAINT PK_GRPS PRIMARY KEY
(
  GROUP_SEQ_ID
)
ENABLE
;
ALTER TABLE ZFC_SECURITY_GRPS
ADD CONSTRAINT UK_ZFC_GRPS UNIQUE (
  NAME
)
ENABLE
;

CREATE TABLE ZFC_PERMISSIONS (
  PERMISSIONS_ID NUMBER(10,0) NOT NULL,
  DESCRIPTION VARCHAR2(128 CHAR) NOT NULL,
  ADDED_BY NUMBER(10,0) DEFAULT (2) NOT NULL,
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL,
  UPDATED_BY NUMBER(10,0) DEFAULT (2) NOT NULL,
  UPDATED_DATE DATE DEFAULT SYSDATE NOT NULL
);


ALTER TABLE ZFC_PERMISSIONS
ADD CONSTRAINT PK_ZFC_PERMISSIONS PRIMARY KEY
(
  PERMISSIONS_ID
)
ENABLE
;
CREATE INDEX UK_ZFC_PERMISSIONS ON ZFC_PERMISSIONS
(
  DESCRIPTION
) 
;

CREATE TABLE ZFC_NAVIGATION_TYPES (
  NAV_TYPE_SEQ_ID NUMBER(10,0) NOT NULL,
  DESCRIPTION VARCHAR2(128 CHAR) NOT NULL,
  ADDED_BY NUMBER(10,0) NOT NULL,
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL,
  UPDATED_BY NUMBER(10,0) NOT NULL,
  UPDATED_DATE DATE DEFAULT SYSDATE NOT NULL
);


ALTER TABLE ZFC_NAVIGATION_TYPES
ADD CONSTRAINT PK_ZFC_NAVIGATION_TYPE PRIMARY KEY
(
  NAV_TYPE_SEQ_ID
)
ENABLE
;

CREATE TABLE ZFC_SYSTEM_STATUS (
  STATUS_SEQ_ID NUMBER(10,0) NOT NULL,
  DESCRIPTION CHAR(25 CHAR) NOT NULL,
  ADDED_BY NUMBER(10,0) DEFAULT (1),
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL,
  UPDATED_BY NUMBER(10,0) DEFAULT (1),
  UPDATED_DATE DATE DEFAULT SYSDATE NOT NULL
);


ALTER TABLE ZFC_SYSTEM_STATUS
ADD CONSTRAINT PK_ZFC_SYSTEM_STATUS PRIMARY KEY
(
  STATUS_SEQ_ID
)
ENABLE
;
CREATE UNIQUE INDEX UK_ZFC_SYSTEM_STATUS ON ZFC_SYSTEM_STATUS
(
  DESCRIPTION
) 
;

CREATE TABLE ZCT_LINKS (
  CONTENT_SEQ_ID NUMBER(10,0) NOT NULL,
  Url VARCHAR2(255 CHAR) NOT NULL,
  SORT_ORDER NUMBER(10,0) DEFAULT (0) NOT NULL,
  CLICK_COUNT NUMBER(10,0) DEFAULT (0) NOT NULL
);


ALTER TABLE ZCT_LINKS
ADD CONSTRAINT PK_ZCT_LINKS PRIMARY KEY
(
  CONTENT_SEQ_ID
)
ENABLE
;

CREATE TABLE ZCT_RATINGS (
  Rating_id NUMBER(10,0) NOT NULL,
  Rating_CommunityID NUMBER(10,0) NOT NULL,
  CONTENT_SEQ_ID NUMBER(10,0) NOT NULL,
  Rating_UserID NUMBER(10,0) NOT NULL,
  Rating_Rating NUMBER(10,0) NOT NULL
);


ALTER TABLE ZCT_RATINGS
ADD CONSTRAINT PK_ZCT_RATINGS PRIMARY KEY
(
  Rating_id
)
ENABLE
;

CREATE TABLE ZCT_HAS_READ (
  hr_ACCT_SEQ_ID NUMBER(10,0) NOT NULL,
  CONTENT_SEQ_ID NUMBER(10,0) NOT NULL,
  hr_dateFirstRead DATE NOT NULL,
  hr_dateLastRead DATE NOT NULL
);


ALTER TABLE ZCT_HAS_READ
ADD CONSTRAINT PK_ZCT_HAS_READ PRIMARY KEY
(
  hr_ACCT_SEQ_ID,
  CONTENT_SEQ_ID
)
ENABLE
;

CREATE TABLE ZCT_DISCUSS (
  CONTENT_SEQ_ID NUMBER(10,0) NOT NULL,
  DISCUSS_LAST_COMMENT_ACCT_SEQ_ID NUMBER(10,0),
  DISCUSS_IS_ANNOUNCEMENT NUMBER(10,0) DEFAULT (0) NOT NULL,
  DISCUSS_IS_PINNED NUMBER(10,0) DEFAULT (0) NOT NULL,
  DISCUSS_IS_LOCKED NUMBER(10,0) DEFAULT (0) NOT NULL,
  DISCUSS_BODY_TEXT NCLOB
);


ALTER TABLE ZCT_DISCUSS
ADD CONSTRAINT PK_ZCT_DISCUSS PRIMARY KEY
(
  CONTENT_SEQ_ID
)
ENABLE
;

CREATE TABLE ZCT_NOTIFICATIONS (
  NOTIFICATION_SEQ_ID NUMBER(10,0) NOT NULL,
  SE_SEQ_ID NUMBER(10,0) NOT NULL,
  FUNCTION_SEQ_ID NUMBER(10,0) NOT NULL,
  ID NUMBER(10,0) NOT NULL,
  ACCT_SEQ_ID NUMBER(10,0) NOT NULL,
  ADDED_DATE DATE NOT NULL
);


ALTER TABLE ZCT_NOTIFICATIONS
ADD CONSTRAINT PK_ZCT_NOTIFICATIONS PRIMARY KEY
(
  NOTIFICATION_SEQ_ID
)
ENABLE
;

CREATE TABLE ZFC_SECURITY_NVP_RLS (
  NVP_SEQ_ID NUMBER(10,0) NOT NULL,
  RLS_SE_SEQ_ID NUMBER(10,0) NOT NULL,
  PERMISSIONS_SEQ_ID NUMBER(10,0) NOT NULL,
  ADDED_BY NUMBER(10,0) NOT NULL,
  ADDED_DATE DATE NOT NULL
);



CREATE TABLE ZFC_NVP_DETAILS (
  NVP_SEQ_DET_ID NUMBER(10,0) NOT NULL,
  NVP_SEQ_ID NUMBER(10,0) NOT NULL,
  NVP_DET_CODE NUMBER(10,0) NOT NULL,
  NVP_DET_VALUE VARCHAR2(300 CHAR) NOT NULL,
  STATUS_SEQ_ID NUMBER(10,0) NOT NULL,
  ADDED_BY NUMBER(10,0) NOT NULL,
  ADDED_DATE DATE NOT NULL,
  UPDATED_BY NUMBER(10,0),
  UPDATED_DATE DATE
);


ALTER TABLE ZFC_NVP_DETAILS
ADD CONSTRAINT PK_ZFC_NVP_DETAILS PRIMARY KEY
(
  NVP_SEQ_DET_ID
)
ENABLE
;

CREATE TABLE ZFC_SECURITY_NVP_GRPS (
  GRPS_SE_SEQ_ID NUMBER(10,0) NOT NULL,
  NVP_SEQ_ID NUMBER(10,0) NOT NULL,
  PERMISSIONS_ID NUMBER(10,0) NOT NULL,
  ADDED_BY NUMBER(10,0) NOT NULL,
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL
);



CREATE TABLE ZFC_FUNCTIONS (
  FUNCTION_SEQ_ID NUMBER(10,0) NOT NULL,
  NAME VARCHAR2(30 CHAR) NOT NULL,
  DESCRIPTION VARCHAR2(128 CHAR) NOT NULL,
  FUNCTION_TYPE_SEQ_ID NUMBER(10,0),
  ALLOW_HTML_INPUT NUMBER(10,0) DEFAULT (0) NOT NULL,
  ALLOW_COMMENT_HTML_INPUT NUMBER(10,0) DEFAULT (0) NOT NULL,
  SOURCE VARCHAR2(512 CHAR),
  ENABLE_VIEW_STATE NUMBER(10,0) NOT NULL,
  IS_NAV NUMBER(10,0) NOT NULL,
  NAV_TYPE_ID NUMBER(10,0) NOT NULL,
  ACTION VARCHAR2(256 CHAR) NOT NULL,
  PARENT_FUNCTION_SEQ_ID NUMBER(10,0),
  NOTES VARCHAR2(512 CHAR),
  SORT_ORDER NUMBER(10,0) DEFAULT (0) NOT NULL,
  ADDED_BY NUMBER(10,0) NOT NULL,
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL,
  UPDATED_BY NUMBER(10,0) NOT NULL,
  UPDATED_DATE DATE DEFAULT SYSDATE NOT NULL
);


ALTER TABLE ZFC_FUNCTIONS
ADD CONSTRAINT PK_ZFC_FUNCTIONS_1 PRIMARY KEY
(
  FUNCTION_SEQ_ID
)
ENABLE
;
ALTER TABLE ZFC_FUNCTIONS
ADD CONSTRAINT UK_ZFC_FUNCTIONS UNIQUE (
  ACTION
)
ENABLE
;
ALTER TABLE ZFC_FUNCTIONS
ADD CONSTRAINT PK_ZFC_FUNCTIONS UNIQUE (
  FUNCTION_SEQ_ID
)
ENABLE
;

CREATE TABLE ZFC_ACCTS (
  ACCT_SEQ_ID NUMBER(10,0) NOT NULL,
  ACCT VARCHAR2(128 CHAR) NOT NULL,
  EMAIL VARCHAR2(128 CHAR),
  ENABLE_NOTIFICATIONS NUMBER(10,0),
  IS_SYSTEM_ADMIN NUMBER(10,0) DEFAULT (0) NOT NULL,
  STATUS_SEQ_ID NUMBER(10,0) NOT NULL,
  PASSWORD_LAST_SET DATE DEFAULT SYSDATE NOT NULL,
  PWD VARCHAR2(256 CHAR) NOT NULL,
  FAILED_ATTEMPTS NUMBER(10,0) NOT NULL,
  FIRST_NAME VARCHAR2(15 CHAR) NOT NULL,
  LAST_LOGIN DATE DEFAULT SYSDATE NOT NULL,
  LAST_NAME VARCHAR2(15 CHAR) NOT NULL,
  LOCATION VARCHAR2(128 CHAR),
  MIDDLE_NAME VARCHAR2(15 CHAR),
  PREFERED_NAME VARCHAR2(50 CHAR),
  TIME_ZONE NUMBER(10,0),
  ADDED_BY NUMBER(10,0) DEFAULT (1) NOT NULL,
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL,
  UPDATED_BY NUMBER(10,0) DEFAULT (1) NOT NULL,
  UPDATED_DATE DATE DEFAULT SYSDATE NOT NULL
);


ALTER TABLE ZFC_ACCTS
ADD CONSTRAINT PK_ZFC_ACCTS PRIMARY KEY
(
  ACCT_SEQ_ID
)
ENABLE
;
ALTER TABLE ZFC_ACCTS
ADD CONSTRAINT UK_ZFC_ACCTS UNIQUE (
  ACCT
)
ENABLE
;

CREATE TABLE ZOP_STATES (
  STATE VARCHAR2(2 CHAR) NOT NULL,
  DESCRIPTION VARCHAR2(128 CHAR),
  STATUS_SEQ_ID NUMBER(10,0),
  ADDED_BY NUMBER(10,0) DEFAULT (1) NOT NULL,
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL,
  UPDATED_BY NUMBER(10,0) DEFAULT (1) NOT NULL,
  UPDATED_DATE DATE DEFAULT SYSDATE NOT NULL
);


ALTER TABLE ZOP_STATES
ADD CONSTRAINT PK_ZOP_STATES PRIMARY KEY
(
  STATE
)
ENABLE
;

CREATE TABLE ZFO_MESSAGES (
  MESSAGE_SEQ_ID NUMBER(10,0) NOT NULL,
  SE_SEQ_ID NUMBER(10,0) NOT NULL,
  NAME VARCHAR2(50 CHAR) NOT NULL,
  TITLE VARCHAR2(100 CHAR) NOT NULL,
  DESCRIPTION VARCHAR2(128 CHAR) NOT NULL,
  FORMAT_AS_HTML NUMBER(10,0) DEFAULT (0) NOT NULL,
  BODY NCLOB NOT NULL,
  ADDED_BY NUMBER(10,0) NOT NULL,
  ADDED_DATE DATE NOT NULL,
  UPDATED_BY NUMBER(10,0),
  UPDATED_DATE DATE
);


ALTER TABLE ZFO_MESSAGES
ADD CONSTRAINT PK_ZFO_MESSAGES PRIMARY KEY
(
  MESSAGE_SEQ_ID
)
ENABLE
;

CREATE TABLE ZFC_SECURITY_RLS_SE (
  RLS_SE_SEQ_ID NUMBER(10,0) NOT NULL,
  SE_SEQ_ID NUMBER(10,0) NOT NULL,
  ROLE_SEQ_ID NUMBER(10,0) NOT NULL,
  ADDED_BY NUMBER(10,0) DEFAULT (2) NOT NULL,
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL
);


ALTER TABLE ZFC_SECURITY_RLS_SE
ADD CONSTRAINT PK_ZFC_RLS_SE PRIMARY KEY
(
  RLS_SE_SEQ_ID
)
ENABLE
;
ALTER TABLE ZFC_SECURITY_RLS_SE
ADD CONSTRAINT UK_ZFC_RLS_SE UNIQUE (
  SE_SEQ_ID,
  ROLE_SEQ_ID
)
ENABLE
;
CREATE UNIQUE INDEX UK_ZFC_RLS_SE ON ZFC_SECURITY_RLS_SE
(
  SE_SEQ_ID,
  ROLE_SEQ_ID
) 
;

CREATE TABLE ZOP_CALENDAR (
  SE_SEQ_ID NUMBER(10,0) NOT NULL,
  CALENDAR_NAME VARCHAR2(50 CHAR) NOT NULL,
  ENTRY_DATE DATE NOT NULL,
  COMMENT_ VARCHAR2(100 CHAR) NOT NULL,
  ACTIVE NUMBER(10,0) NOT NULL,
  ADDED_BY NUMBER(10,0) DEFAULT (1) NOT NULL,
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL,
  UPDATED_BY NUMBER(10,0) DEFAULT (1) NOT NULL,
  UPDATED_DATE DATE DEFAULT SYSDATE NOT NULL
);


COMMENT ON COLUMN ZOP_CALENDAR.COMMENT_ IS 'ORIGINAL NAME:COMMENT'
;


CREATE TABLE ZFC_SECURITY_GRPS_SE (
  GRPS_SE_SEQ_ID NUMBER(10,0) NOT NULL,
  SE_SEQ_ID NUMBER(10,0) NOT NULL,
  GROUP_SEQ_ID NUMBER(10,0) NOT NULL,
  ADDED_BY NUMBER(10,0) NOT NULL,
  ADDED_DATE DATE NOT NULL
);


ALTER TABLE ZFC_SECURITY_GRPS_SE
ADD CONSTRAINT PK_ZFC_GRPS_SE PRIMARY KEY
(
  GRPS_SE_SEQ_ID
)
ENABLE
;
ALTER TABLE ZFC_SECURITY_GRPS_SE
ADD CONSTRAINT UK_ZFC_GRPS_SE UNIQUE (
  SE_SEQ_ID,
  GROUP_SEQ_ID
)
ENABLE
;

CREATE TABLE ZCT_CONTENT (
  CONTENT_SEQ_ID NUMBER(10,0) NOT NULL,
  SE_SEQ_ID NUMBER(10,0) NOT NULL,
  ACCT_SEQ_ID NUMBER(10,0),
  EDIT_ACCT_SEQ_ID NUMBER(10,0),
  PARENT_ID NUMBER(10,0) DEFAULT (-1) NOT NULL,
  PAGE_TYPE NUMBER(10,0) NOT NULL,
  FUNCTION_SEQ_ID NUMBER(10,0) NOT NULL,
  TITLE VARCHAR2(100 CHAR) NOT NULL,
  DESCRIPTION VARCHAR2(128 CHAR) NOT NULL,
  META_DESC VARCHAR2(250 CHAR) NOT NULL,
  META_KEYS VARCHAR2(250 CHAR) NOT NULL,
  ADDED_DATE DATE DEFAULT getutcdate() NOT NULL,
  UPDATED_DATE DATE DEFAULT getutcdate(),
  COMMENTED_DATE DATE DEFAULT getutcdate(),
  TOPIC_ID NUMBER(10,0),
  REPLY_ID NUMBER(10,0) DEFAULT (-1) NOT NULL,
  MODERATION_STATUS NUMBER(10,0) DEFAULT (1) NOT NULL,
  MODERATION_ACCT_SEQ_ID NUMBER(10,0) DEFAULT (-1) NOT NULL,
  RATING NUMBER(10,0) DEFAULT (-1) NOT NULL,
  SORT_ORDER DATE DEFAULT getutcdate() NOT NULL,
  VISIBLE_DATE DATE DEFAULT getutcdate() NOT NULL,
  VIEW_COUNT NUMBER(10,0) DEFAULT (0) NOT NULL,
  TIME_STAMP NUMBER(8,0) NOT NULL,
  REMOTE_COMMUNITY_UNIQUE_ID CHAR(36 CHAR),
  REMOTE_ID NUMBER(10,0),
  REMOTE_ACCOUNT VARCHAR2(50 CHAR),
  REMOTE_TIMESTAMP RAW(50)
);


ALTER TABLE ZCT_CONTENT
ADD CONSTRAINT PK_ZCT_CONTENT PRIMARY KEY
(
  CONTENT_SEQ_ID
)
ENABLE
;
CREATE INDEX IX_ZCT_CONTENT ON ZCT_CONTENT
(
  VIEW_COUNT
) 
;
CREATE INDEX SectionID ON ZCT_CONTENT
(
  FUNCTION_SEQ_ID
) 
;

CREATE TABLE ZFC_SECURITY_GRPS_GRPS (
  GRPS_SE_SEQ_ID NUMBER(10,0) NOT NULL,
  GROUP_SEQ_ID NUMBER(10,0) NOT NULL,
  ADDED_BY NUMBER(10,0) NOT NULL,
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL
);


ALTER TABLE ZFC_SECURITY_GRPS_GRPS
ADD CONSTRAINT UK_ZFC_GRPS_GRPS UNIQUE (
  GRPS_SE_SEQ_ID,
  GROUP_SEQ_ID
)
ENABLE
;

CREATE TABLE ZFC_SECURITY_FUNCT_RLS (
  RLS_SE_SEQ_ID NUMBER(10,0) NOT NULL,
  FUNCTION_SEQ_ID NUMBER(10,0) NOT NULL,
  PERMISSIONS_ID NUMBER(10,0) NOT NULL,
  ADDED_BY NUMBER(10,0) NOT NULL,
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL
);


ALTER TABLE ZFC_SECURITY_FUNCT_RLS
ADD CONSTRAINT UK_ZFC_FUNCT_PER_RLS UNIQUE (
  RLS_SE_SEQ_ID,
  FUNCTION_SEQ_ID,
  PERMISSIONS_ID
)
ENABLE
;
CREATE UNIQUE INDEX UK_ZFC_FUNCT_PER_RLS ON ZFC_SECURITY_FUNCT_RLS
(
  RLS_SE_SEQ_ID,
  FUNCTION_SEQ_ID,
  PERMISSIONS_ID
) 
;

CREATE TABLE ZFC_SECURITY_FUNCT_GRPS (
  GRPS_SE_SEQ_ID NUMBER(10,0) NOT NULL,
  FUNCTION_SEQ_ID NUMBER(10,0) NOT NULL,
  PERMISSIONS_ID NUMBER(10,0) NOT NULL,
  ADDED_BY NUMBER(10,0) NOT NULL,
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL
);


ALTER TABLE ZFC_SECURITY_FUNCT_GRPS
ADD CONSTRAINT UK_ZFC_FUNCT_PER_GRPS UNIQUE (
  GRPS_SE_SEQ_ID,
  FUNCTION_SEQ_ID,
  PERMISSIONS_ID
)
ENABLE
;
CREATE UNIQUE INDEX UK_ZFC_FUNCT_PER_GRPS ON ZFC_SECURITY_FUNCT_GRPS
(
  GRPS_SE_SEQ_ID,
  FUNCTION_SEQ_ID,
  PERMISSIONS_ID
) 
;

CREATE TABLE ZFO_DIRECTORIES (
  FUNCTION_SEQ_ID NUMBER(10,0) NOT NULL,
  DIRECTORY VARCHAR2(255 CHAR) NOT NULL,
  IMPERSONATE NUMBER(10,0) NOT NULL,
  IMPERSONATE_ACCOUNT VARCHAR2(50 CHAR),
  IMPERSONATE_PWD VARCHAR2(50 CHAR),
  ADDED_BY NUMBER(10,0) NOT NULL,
  ADDED_DATE DATE NOT NULL,
  UPDATED_BY NUMBER(10,0) NOT NULL,
  UPDATED_DATE DATE NOT NULL
);


ALTER TABLE ZFO_DIRECTORIES
ADD CONSTRAINT PK_ZFO_DIRECTORIES PRIMARY KEY
(
  FUNCTION_SEQ_ID
)
ENABLE
;

CREATE TABLE ZOP_WORK_FLOWS (
  WORK_FLOW_ID NUMBER(10,0) NOT NULL,
  ORDER_ID NUMBER(10,0) NOT NULL,
  WORK_FLOW_NAME VARCHAR2(50 CHAR) NOT NULL,
  FUNCTION_SEQ_ID NUMBER(10,0) NOT NULL
);


ALTER TABLE ZOP_WORK_FLOWS
ADD CONSTRAINT PK_ZOP_WORK_FLOWS PRIMARY KEY
(
  WORK_FLOW_ID
)
ENABLE
;

CREATE TABLE ZCT_HTML_PAGES (
  FUNCTION_SEQ_ID NUMBER(10,0) NOT NULL,
  HTML_PAGE NCLOB NOT NULL,
  ADDED_BY NUMBER(10,0) NOT NULL,
  ADDED_DATE DATE NOT NULL,
  UPDATED_BY NUMBER(10,0) NOT NULL,
  UPDATED_DATE DATE NOT NULL
);


ALTER TABLE ZCT_HTML_PAGES
ADD CONSTRAINT PK_ZCT_HTML_PAGES PRIMARY KEY
(
  FUNCTION_SEQ_ID
)
ENABLE
;

CREATE TABLE ZFC_INFORMATION (
  Information_SEQ_ID NUMBER(10,0) NOT NULL,
  Version VARCHAR2(50 CHAR) NOT NULL,
  Enable_Inheritance NUMBER(10,0) NOT NULL,
  ADDED_BY NUMBER(10,0) DEFAULT (1) NOT NULL,
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL,
  UPDATED_BY NUMBER(10,0) DEFAULT (1) NOT NULL,
  UPDATED_DATE DATE DEFAULT SYSDATE NOT NULL
);


ALTER TABLE ZFC_INFORMATION
ADD CONSTRAINT PK_ZFC_INFORMATION PRIMARY KEY
(
  Information_SEQ_ID
)
ENABLE
;

CREATE TABLE ZFC_SECURITY_ACCTS_RLS (
  RLS_SE_SEQ_ID NUMBER(10,0) NOT NULL,
  ACCT_SEQ_ID NUMBER(10,0) NOT NULL,
  ADDED_BY NUMBER(10,0) DEFAULT (1) NOT NULL,
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL
);



CREATE TABLE ZFO_ACCT_CHOICES (
  ACCT VARCHAR2(128 CHAR) NOT NULL,
  SE_SEQ_ID VARCHAR2(50 CHAR),
  SE_NAME VARCHAR2(256 CHAR),
  BACK_COLOR VARCHAR2(15 CHAR),
  LEFT_COLOR VARCHAR2(15 CHAR),
  HEAD_COLOR VARCHAR2(15 CHAR),
  SUB_HEAD_COLOR VARCHAR2(15 CHAR),
  COLOR_SCHEME VARCHAR2(15 CHAR),
  FAVORIATE_ACTION VARCHAR2(50 CHAR),
  THIN_ACTIONS VARCHAR2(1000 CHAR),
  WIDE_ACTIONS VARCHAR2(1000 CHAR),
  RECORDS_PER_PAGE VARCHAR2(9 CHAR),
  EDIT_ID VARCHAR2(50 CHAR)
);


ALTER TABLE ZFO_ACCT_CHOICES
ADD CONSTRAINT UK_ZFO_ACCT_CHOICES UNIQUE (
  ACCT
)
ENABLE
;

CREATE TABLE ZFC_SECURITY_ACCTS_GRPS (
  GRPS_SE_SEQ_ID NUMBER(10,0) NOT NULL,
  ACCT_SEQ_ID NUMBER(10,0) NOT NULL,
  ADDED_BY NUMBER(10,0) DEFAULT (1) NOT NULL,
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL
);



CREATE TABLE ZFC_NVP (
  NVP_SEQ_ID NUMBER(10,0) NOT NULL,
  TABLE_NAME VARCHAR2(30 CHAR) NOT NULL,
  DISPLAY VARCHAR2(128 CHAR) NOT NULL,
  DESCRIPTION VARCHAR2(128 CHAR) NOT NULL,
  ADDED_BY NUMBER(10,0) NOT NULL,
  ADDED_DATE DATE NOT NULL,
  UPDATED_BY NUMBER(10,0),
  UPDATED_DATE DATE
);


ALTER TABLE ZFC_NVP
ADD CONSTRAINT PK_ZFC_NVP PRIMARY KEY
(
  NVP_SEQ_ID
)
ENABLE
;

CREATE TABLE ZFC_SECURITY_ENTITIES (
  SE_SEQ_ID NUMBER(10,0) NOT NULL,
  NAME VARCHAR2(256 CHAR) NOT NULL,
  DESCRIPTION VARCHAR2(128 CHAR),
  URL VARCHAR2(128 CHAR),
  STATUS_SEQ_ID NUMBER(10,0) NOT NULL,
  DAL NCHAR(100) NOT NULL,
  DAL_NAME NCHAR(100) NOT NULL,
  DAL_NAME_SPACE VARCHAR2(256 CHAR) NOT NULL,
  DAL_STRING VARCHAR2(512 CHAR) NOT NULL,
  SKIN NCHAR(50) NOT NULL,
  STYLE VARCHAR2(25 CHAR) NOT NULL,
  ENCRYPTION_TYPE NUMBER(10,0) NOT NULL,
  PARENT_SE_SEQ_ID NUMBER(10,0),
  ADDED_BY NUMBER(10,0) DEFAULT (1) NOT NULL,
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL,
  UPDATED_BY NUMBER(10,0) DEFAULT (1) NOT NULL,
  UPDATED_DATE DATE DEFAULT SYSDATE NOT NULL
);


ALTER TABLE ZFC_SECURITY_ENTITIES
ADD CONSTRAINT PK_SECURITY_ENTITIES PRIMARY KEY
(
  SE_SEQ_ID
)
ENABLE
;
CREATE UNIQUE INDEX UK_ZFC_SECURITY_ENTITIES ON ZFC_SECURITY_ENTITIES
(
  NAME
) 
;

CREATE TABLE ZOP_ZIPCODES (
  STATE VARCHAR2(2 CHAR) NOT NULL,
  ZIP_CODE NUMBER(10,0) NOT NULL,
  AREA_CODE NUMBER(10,0) NOT NULL,
  CITY VARCHAR2(255 CHAR),
  TIME_ZONE VARCHAR2(255 CHAR)
);


ALTER TABLE ZOP_ZIPCODES
ADD CONSTRAINT uk_ZOP_ZIPCODES UNIQUE (
  STATE,
  ZIP_CODE,
  CITY
)
ENABLE
;

CREATE TABLE ZFC_SECURITY_GRPS_RLS (
  RLS_SE_SEQ_ID NUMBER(10,0) NOT NULL,
  GRPS_SE_SEQ_ID NUMBER(10,0) NOT NULL,
  ADDED_BY NUMBER(10,0) NOT NULL,
  ADDED_DATE DATE NOT NULL
);


CREATE UNIQUE INDEX UK_ZFC_SECURITY_GRPS_RLS ON ZFC_SECURITY_GRPS_RLS
(
  RLS_SE_SEQ_ID,
  GRPS_SE_SEQ_ID
) 
;

CREATE TABLE ZFC_FUNCTION_TYPES (
  FUNCTION_TYPE_SEQ_ID NUMBER(10,0) NOT NULL,
  NAME VARCHAR2(50 CHAR) NOT NULL,
  DESCRIPTION VARCHAR2(128 CHAR) NOT NULL,
  TEMPLATE VARCHAR2(512 CHAR),
  IS_CONTENT NUMBER(10,0) NOT NULL,
  ADDED_BY NUMBER(10,0) NOT NULL,
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL,
  UPDATED_BY NUMBER(10,0) NOT NULL,
  UPDATED_DATE DATE DEFAULT SYSDATE NOT NULL
);


ALTER TABLE ZFC_FUNCTION_TYPES
ADD CONSTRAINT PK_ZFC_FUNCTION_TYPES PRIMARY KEY
(
  FUNCTION_TYPE_SEQ_ID
)
ENABLE
;
ALTER TABLE ZFC_FUNCTION_TYPES
ADD CONSTRAINT UK_ZFC_FUNCTION_TYPES UNIQUE (
  NAME
)
ENABLE
;

CREATE TABLE ZFC_SECURITY_RLS (
  ROLE_SEQ_ID NUMBER(10,0) NOT NULL,
  NAME VARCHAR2(50 CHAR) NOT NULL,
  DESCRIPTION VARCHAR2(128 CHAR) NOT NULL,
  IS_SYSTEM NUMBER(10,0) NOT NULL,
  IS_SYSTEM_ONLY NUMBER(10,0) DEFAULT (0) NOT NULL,
  ADDED_BY NUMBER(10,0) DEFAULT (2) NOT NULL,
  ADDED_DATE DATE DEFAULT SYSDATE NOT NULL,
  UPDATED_BY NUMBER(10,0) NOT NULL,
  UPDATED_DATE DATE DEFAULT SYSDATE NOT NULL
);


ALTER TABLE ZFC_SECURITY_RLS
ADD CONSTRAINT PK_RLS PRIMARY KEY
(
  ROLE_SEQ_ID
)
ENABLE
;
ALTER TABLE ZFC_SECURITY_RLS
ADD CONSTRAINT UK_ZFC_RLS UNIQUE (
  NAME
)
ENABLE
;

CREATE ROLE db_accessadmin_Foundation ;

CREATE ROLE db_securityadmin_Foundation ;

CREATE ROLE db_ddladmin_Foundation ;

CREATE ROLE db_backupoperator_Foundation ;

CREATE ROLE db_datareader_Foundation ;

CREATE ROLE db_datawriter_Foundation ;

CREATE ROLE db_denydatareader_Foundation ;

CREATE ROLE db_denydatawriter_Foundation ;

CREATE ROLE public_Foundation ;

CREATE ROLE db_owner_Foundation ;

connect Foundation/Foundation;

CREATE OR REPLACE FUNCTION ZFF_ENABLE_INHERITANCE
RETURN NUMBER
AS
   v_V_RETURN_VAL NUMBER(10,0);
BEGIN
   SELECT Enable_Inheritance
     INTO v_V_RETURN_VAL
     FROM ZFC_INFORMATION
      WHERE Information_SEQ_ID = 1;

   RETURN v_V_RETURN_VAL;-- 0 = FALSE 1 = TRUE
   

END;
/
/* Translation Extracted DDL For Required Objects */
CREATE GLOBAL TEMPORARY TABLE tt_v_retFindChildren
(
  SE_SEQ_ID NUMBER(10,0) ,
  PARENT_SE_SEQ_ID NUMBER(10,0) 
);
CREATE OR REPLACE PACKAGE ZFF_GET_SE_CHILDREN_pkg
AS
   TYPE tt_v_retFindChildren_type IS TABLE OF tt_v_retFindChildren%ROWTYPE;
END;


CREATE OR REPLACE FUNCTION ZFF_GET_SE_CHILDREN
(
  v_P_INCLUDE_PARENT IN NUMBER,
  v_P_SE_SEQ_ID IN NUMBER
)
RETURN ZFF_GET_SE_CHILDREN_pkg.tt_v_retFindChildren_type PIPELINED
AS
   CURSOR RetrieveChildren
     IS SELECT SE_SEQ_ID,
   PARENT_SE_SEQ_ID
     FROM ZFC_SECURITY_ENTITIES
      WHERE PARENT_SE_SEQ_ID = v_P_SE_SEQ_ID;
   v_V_SE_SEQ_ID NUMBER(10,0);
   v_V_PARENT_SE_SEQ_ID NUMBER(10,0);
   v_temp SYS_REFCURSOR;
   v_temp_1 TT_V_RETFINDCHILDREN%ROWTYPE;
BEGIN
   IF ( v_P_INCLUDE_PARENT = 1 ) THEN
   BEGIN
      INSERT INTO tt_v_retFindChildren
        ( SELECT SE_SEQ_ID,
                 PARENT_SE_SEQ_ID
          FROM ZFC_SECURITY_ENTITIES
             WHERE SE_SEQ_ID = v_P_SE_SEQ_ID );

   END;
   END IF;

   OPEN RetrieveChildren;

   FETCH RetrieveChildren INTO v_V_SE_SEQ_ID,v_V_PARENT_SE_SEQ_ID;

   WHILE ( sqlserver_utilities.fetch_status(RetrieveChildren%FOUND) = 0 )
   LOOP
      BEGIN
         INSERT INTO tt_v_retFindChildren
           ( SELECT SE_SEQ_ID,
                    PARENT_SE_SEQ_ID
             FROM TABLE(ZFF_GET_SE_CHILDREN(0, v_V_SE_SEQ_ID))  );

         INSERT INTO tt_v_retFindChildren
           VALUES ( v_V_SE_SEQ_ID, v_V_PARENT_SE_SEQ_ID );

         FETCH RetrieveChildren INTO v_V_SE_SEQ_ID,v_V_PARENT_SE_SEQ_ID;

      END;
   END LOOP;

   -- END WHILE
   CLOSE RetrieveChildren;

   OPEN v_temp FOR
      SELECT *
        FROM tt_v_retFindChildren;

   LOOP
      FETCH v_temp INTO v_temp_1;
      EXIT WHEN v_temp%NOTFOUND;
      PIPE ROW ( v_temp_1 );
   END LOOP;

END;
/
/* Translation Extracted DDL For Required Objects */
CREATE GLOBAL TEMPORARY TABLE tt_v_retParents
(
  SE_SEQ_ID NUMBER(10,0) ,
  PARENT_SE_SEQ_ID NUMBER(10,0) 
);
CREATE OR REPLACE PACKAGE ZFF_GET_SE_PARENTS_pkg
AS
   TYPE tt_v_retParents_type IS TABLE OF tt_v_retParents%ROWTYPE;
END;


CREATE OR REPLACE FUNCTION ZFF_GET_SE_PARENTS
(
  v_P_IncludeParent IN NUMBER,
  v_P_SE_SEQ_ID IN NUMBER
)
RETURN ZFF_GET_SE_PARENTS_pkg.tt_v_retParents_type PIPELINED
AS
   CURSOR RetrieveReports
     IS SELECT SE_SEQ_ID,
   Parent_SE_SEQ_ID
     FROM ZFC_SECURITY_ENTITIES
      WHERE SE_SEQ_ID = v_P_SE_SEQ_ID;
   v_temp SYS_REFCURSOR;
   v_temp_1 TT_V_RETPARENTS%ROWTYPE;
BEGIN
   IF ( ZFF_ENABLE_INHERITANCE() = 1 ) THEN
   DECLARE
      v_temp SYS_REFCURSOR;
      v_temp_1 TT_V_RETPARENTS%ROWTYPE;
      v_Report_ID NUMBER(10,0);
      v_Report_ParentID NUMBER(10,0);
      v_temp_2 SYS_REFCURSOR;
      v_temp_3 TT_V_RETPARENTS%ROWTYPE;
   BEGIN
      IF ( v_P_IncludeParent = 1 ) THEN
      BEGIN
         INSERT INTO tt_v_retParents
           ( SELECT SE_SEQ_ID,
                    Parent_SE_SEQ_ID
             FROM ZFC_SECURITY_ENTITIES
                WHERE SE_SEQ_ID = v_P_SE_SEQ_ID );

      END;
      END IF;

      -- END IF
      IF ( v_P_SE_SEQ_ID = 0
        OR v_P_SE_SEQ_ID = ZFF_GET_DEFAULT_Security_Entity_ID() ) THEN
         OPEN v_tempv_temp_2 FOR
            SELECT *
              FROM tt_v_retParents;

         LOOP
            FETCH v_tempv_temp_2 INTO v_temp_1v_temp_3;
            EXIT WHEN v_tempv_temp_2%NOTFOUND;
            PIPE ROW ( v_temp_1v_temp_3 );
         END LOOP;

      END IF;

      OPEN RetrieveReports;

      FETCH RetrieveReports INTO v_Report_ID,v_Report_ParentID;

      WHILE ( sqlserver_utilities.fetch_status(RetrieveReports%FOUND) = 0 )
      LOOP
         BEGIN
            INSERT INTO tt_v_retParents
              ( SELECT *
                FROM TABLE(ZFF_GET_SE_PARENTS(1, v_Report_ParentID))  );

            FETCH RetrieveReports INTO v_Report_ID,v_Report_ParentID;

         END;
      END LOOP;

      CLOSE RetrieveReports;

      OPEN v_tempv_temp_2 FOR
         SELECT *
           FROM tt_v_retParents;

      LOOP
         FETCH v_tempv_temp_2 INTO ;
         EXIT WHEN v_tempv_temp_2%NOTFOUND;
         PIPE ROW (  );
      END LOOP;

   END;
   ELSE
   BEGIN
      INSERT INTO tt_v_retParents
        VALUES ( ZFF_GET_DEFAULT_Security_Entity_ID(), 1 );

      INSERT INTO tt_v_retParents
        VALUES ( v_P_SE_SEQ_ID, 1 );
      -- END IF
   END;
   END IF;

   OPEN v_temp FOR
      SELECT *
        FROM tt_v_retParents;

   LOOP
      FETCH v_temp INTO v_temp_1;
      EXIT WHEN v_temp%NOTFOUND;
      PIPE ROW ( v_temp_1 );
   END LOOP;

END;
/
CREATE OR REPLACE FUNCTION ZFF_GET_DEFAULT_Security_Entity_
RETURN NUMBER
AS
BEGIN
   RETURN 1;

END;
/
CREATE OR REPLACE FUNCTION ZFF_GET_VIEW_PERMISSION_ID
RETURN NUMBER
AS
BEGIN
   RETURN 1;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_DEL_ACCOUNT
(
  v_P_ACCT_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      -- DELETE an existing row from the table.
      DELETE ZFC_ACCTS

         WHERE ACCT_SEQ_ID = v_P_ACCT_SEQ_ID;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_DEL_ACCT_RL_SECURITY
(
  v_P_ACCT_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ADDUPD_BY IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      DELETE ZFC_SECURITY_ACCTS_RLS

         WHERE RLS_SE_SEQ_ID IN ( SELECT RLS_SE_SEQ_ID
                              FROM ZFC_SECURITY_RLS_SE
                                 WHERE SE_SEQ_ID = v_P_SE_SEQ_ID )
                 AND ACCT_SEQ_ID = v_P_ACCT_SEQ_ID;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_DEL_Security_Entity
(
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      -- DELETE an existing row from the table.
      DELETE ZFC_SECURITY_ENTITIES

         WHERE SE_SEQ_ID = v_P_SE_SEQ_ID;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_DEL_FUNCTION
(
  v_P_FUNCTION_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      -- DELETE an existing row from the table.
      DELETE ZFC_FUNCTIONS

         WHERE FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_DEL_FUNCTION_GRP_SECURITY
(
  v_P_FUNCTION_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_PERMISSIONS_ID IN NUMBER DEFAULT NULL ,
  v_P_ADDUPD_BY IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      DELETE ZFC_SECURITY_FUNCT_GRPS

         WHERE GRPS_SE_SEQ_ID IN ( SELECT GRPS_SE_SEQ_ID
                               FROM ZFC_SECURITY_GRPS_SE
                                  WHERE SE_SEQ_ID = v_P_SE_SEQ_ID )
                 AND FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_DEL_FUNCTION_RL_SECURITY
(
  v_P_FUNCTION_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_PERMISSIONS_ID IN NUMBER DEFAULT NULL ,
  v_P_ADDUPD_BY IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      DELETE ZFC_SECURITY_FUNCT_RLS

         WHERE RLS_SE_SEQ_ID IN ( SELECT RLS_SE_SEQ_ID
                              FROM ZFC_SECURITY_RLS_SE
                                 WHERE SE_SEQ_ID = v_P_SE_SEQ_ID )
                 AND FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID
                 AND PERMISSIONS_ID = v_P_PERMISSIONS_ID;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_DEL_GROUP_ROLES
(
  v_P_GROUP_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ADD_UP_BY IN NUMBER DEFAULT NULL
)
AS
BEGIN

   DELETE ZFC_SECURITY_GRPS_RLS

      WHERE GRPS_SE_SEQ_ID IN ( SELECT GRPS_SE_SEQ_ID
                            FROM ZFC_SECURITY_GRPS_SE
                               WHERE SE_SEQ_ID = v_P_SE_SEQ_ID
                                       AND GROUP_SEQ_ID = v_P_GROUP_SEQ_ID );

END;
/
CREATE OR REPLACE PROCEDURE ZFP_DEL_GRP_ACCTS
(
  v_P_GROUP_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ADDUPD_BY IN NUMBER DEFAULT NULL
)
AS
BEGIN

   DELETE ZFC_SECURITY_ACCTS_GRPS

      WHERE GRPS_SE_SEQ_ID IN ( SELECT GRPS_SE_SEQ_ID
                            FROM ZFC_SECURITY_GRPS_SE
                               WHERE GROUP_SEQ_ID = v_P_GROUP_SEQ_ID
                                       AND SE_SEQ_ID = v_P_SE_SEQ_ID );

END;
/
CREATE OR REPLACE PROCEDURE ZFP_DEL_RL_ACCTS
(
  v_P_ROLE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ADDUPD_BY IN NUMBER DEFAULT NULL
)
AS
BEGIN

   DELETE ZFC_SECURITY_ACCTS_RLS

      WHERE RLS_SE_SEQ_ID IN ( SELECT RLS_SE_SEQ_ID
                           FROM ZFC_SECURITY_RLS_SE
                              WHERE ROLE_SEQ_ID = v_P_ROLE_SEQ_ID
                                      AND SE_SEQ_ID = v_P_SE_SEQ_ID );

END;
/
CREATE OR REPLACE FUNCTION ZFP_DEL_ROLE
(
  v_P_ROLE_NAME IN NVARCHAR2 DEFAULT NULL ,
  v_P_SECURITY_ENTITY_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ADDUPD_BY IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  v_P_ISVALID OUT NVARCHAR2
)
RETURN VARCHAR2
AS
   v_sys_error NUMBER := 0;
   /*
	NOTE : ** CASCADE DELETE SHOULD BE TURNED ON IN
		ZFC_SECURITY_RLS_SE FOR THIS TO WORK ELSE
		THIS MIGHT THROW AN ERROR
		**** 
	*/
   v_ROLE_SEQ_ID NUMBER(10,0);
BEGIN

   SELECT ROLE_SEQ_ID
     INTO v_ROLE_SEQ_ID
     FROM ZFC_SECURITY_RLS
      WHERE NAME = v_P_ROLE_NAME;

   SET TRANSACTION READ WRITE;

   BEGIN-- DELETE ROLE FROM ZB_SE_SECURITY
   
      DELETE ZFC_SECURITY_RLS_SE

         WHERE ( ROLE_SEQ_ID = v_ROLE_SEQ_ID
                 AND SE_SEQ_ID = v_P_SECURITY_ENTITY_SEQ_ID );

   END;
   DECLARE
      v_temp NUMBER(1, 0) := 0;
   BEGIN-- DELETE ROLE FROM ZFC_SECURITY_RLS
   
      BEGIN
         SELECT 1 INTO v_temp
           FROM DUAL
          WHERE ( SELECT COUNT(*)
                  FROM ZFC_SECURITY_RLS ,
                       ZFC_SECURITY_RLS_SE
                     WHERE ZFC_SECURITY_RLS.ROLE_SEQ_ID = ZFC_SECURITY_RLS_SE.ROLE_SEQ_ID
                             AND ZFC_SECURITY_RLS.ROLE_SEQ_ID = v_ROLE_SEQ_ID ) = 0;
      EXCEPTION
         WHEN OTHERS THEN
            NULL;
      END;

      IF v_temp = 1 THEN
      BEGIN
         DELETE ZFC_SECURITY_RLS

            WHERE ( ROLE_SEQ_ID = v_ROLE_SEQ_ID );

      END;
      END IF;

   END;
   --  DELETE ROLE FROM ZB_RLS
   IF v_sys_error <> 0 THEN
   BEGIN
      -- Rollback the transaction
      ROLLBACK;

      -- Raise an error and return
      /*Limitation:Syntax Not Recognized:( "Error in deleting department in DeleteDepartment." , 16 , 1 ) */
      v_P_ISVALID := 0;

      RETURN;

   END;
   END IF;

   COMMIT;

   v_P_ISVALID := 1;

   RETURN v_P_ISVALID;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_DEL_WORK_FLOWS
(
  v_P_PRIMARY_KEY IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      -- DELETE an existing row from the table.
      DELETE ZOP_WORK_FLOWS

         WHERE WORK_FLOW_ID = v_P_PRIMARY_KEY;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
/* Translation Extracted DDL For Required Objects */
CREATE GLOBAL TEMPORARY TABLE tt_v_V_Accounts
(
  ACCT_SEQ_ID NUMBER(10,0) ,
  ACCT VARCHAR2(100) ,
  EMAIL VARCHAR2(100) ,
  ENABLE_NOTIFICATIONS NUMBER(1,0) ,
  IS_SYSTEM_ADMIN NUMBER(10,0) ,
  STATUS_SEQ_ID NUMBER(10,0) ,
  PWD VARCHAR2(256) ,
  FAILED_ATTEMPTS NUMBER(10,0) ,
  FIRST_NAME VARCHAR2(30) ,
  LAST_LOGIN DATE ,
  LAST_NAME VARCHAR2(30) ,
  LOCATION VARCHAR2(100) ,
  MIDDLE_NAME VARCHAR2(30) ,
  PREFERED_NAME VARCHAR2(100) ,
  TIME_ZONE NUMBER(10,0) ,
  ADDED_BY NUMBER(10,0) ,
  ADDED_DATE DATE ,
  UPDATED_BY NUMBER(10,0) ,
  UPDATED_DATE DATE 
);


CREATE OR REPLACE PROCEDURE ZFP_GET_ACCT
(
  v_P_IS_SYSTEM_ADMIN IN NUMBER DEFAULT NULL ,
  v_P_ACCOUNT IN VARCHAR2 DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR,
  cv_2 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   -- SELECT all rows from the table.
   IF LENGTH(v_P_ACCOUNT) = 0 THEN
   BEGIN
      IF v_P_IS_SYSTEM_ADMIN = 1 THEN
      BEGIN
         OPEN cv_1 FOR
            SELECT ACCT_SEQ_ID,
                   STATUS_SEQ_ID,
                   ACCT,
                   FIRST_NAME,
                   LAST_NAME,
                   MIDDLE_NAME,
                   PREFERED_NAME,
                   EMAIL,
                   PASSWORD_LAST_SET,
                   PWD,
                   FAILED_ATTEMPTS,
                   ADDED_BY,
                   ADDED_DATE,
                   LAST_LOGIN,
                   TIME_ZONE,
                   LOCATION,
                   ENABLE_NOTIFICATIONS,
                   UPDATED_BY,
                   UPDATED_DATE
              FROM ZFC_ACCTS
              ORDER BY ACCT ASC;

      END;
      ELSE
      BEGIN
         INSERT INTO tt_v_V_Accounts
           ( SELECT ZFC_ACCTS.ACCT_SEQ_ID,-- Roles via roles
           
                         ZFC_ACCTS.ACCT,
                         ZFC_ACCTS.EMAIL,
                         ZFC_ACCTS.ENABLE_NOTIFICATIONS,
                         ZFC_ACCTS.IS_SYSTEM_ADMIN,
                         ZFC_ACCTS.STATUS_SEQ_ID,
                         ZFC_ACCTS.PWD,
                         ZFC_ACCTS.FAILED_ATTEMPTS,
                         ZFC_ACCTS.FIRST_NAME,
                         ZFC_ACCTS.LAST_LOGIN,
                         ZFC_ACCTS.LAST_NAME,
                         ZFC_ACCTS.LOCATION,
                         ZFC_ACCTS.MIDDLE_NAME,
                         ZFC_ACCTS.PREFERED_NAME,
                         ZFC_ACCTS.TIME_ZONE,
                         ZFC_ACCTS.ADDED_BY,
                         ZFC_ACCTS.ADDED_DATE,
                         ZFC_ACCTS.UPDATED_BY,
                         ZFC_ACCTS.UPDATED_DATE
             FROM ZFC_ACCTS ,
                  ZFC_SECURITY_ACCTS_RLS ,
                  ZFC_SECURITY_RLS_SE ,
                  ZFC_SECURITY_RLS
                WHERE ZFC_SECURITY_ACCTS_RLS.ACCT_SEQ_ID = ZFC_ACCTS.ACCT_SEQ_ID
                        AND ZFC_SECURITY_ACCTS_RLS.RLS_SE_SEQ_ID = ZFC_SECURITY_RLS_SE.RLS_SE_SEQ_ID
                        AND ZFC_SECURITY_RLS_SE.SE_SEQ_ID IN ( SELECT SE_SEQ_ID
                                                           FROM TABLE(ZFF_GET_SE_PARENTS(1, v_P_SE_SEQ_ID))  )
                        AND ZFC_SECURITY_RLS_SE.ROLE_SEQ_ID = ZFC_SECURITY_RLS.ROLE_SEQ_ID
             UNION
             SELECT ZFC_ACCTS.ACCT_SEQ_ID,-- Roles via groups
             
                         ZFC_ACCTS.ACCT,
                         ZFC_ACCTS.EMAIL,
                         ZFC_ACCTS.ENABLE_NOTIFICATIONS,
                         ZFC_ACCTS.IS_SYSTEM_ADMIN,
                         ZFC_ACCTS.STATUS_SEQ_ID,
                         ZFC_ACCTS.PWD,
                         ZFC_ACCTS.FAILED_ATTEMPTS,
                         ZFC_ACCTS.FIRST_NAME,
                         ZFC_ACCTS.LAST_LOGIN,
                         ZFC_ACCTS.LAST_NAME,
                         ZFC_ACCTS.LOCATION,
                         ZFC_ACCTS.MIDDLE_NAME,
                         ZFC_ACCTS.PREFERED_NAME,
                         ZFC_ACCTS.TIME_ZONE,
                         ZFC_ACCTS.ADDED_BY,
                         ZFC_ACCTS.ADDED_DATE,
                         ZFC_ACCTS.UPDATED_BY,
                         ZFC_ACCTS.UPDATED_DATE
             FROM ZFC_ACCTS ,
                  ZFC_SECURITY_ACCTS_GRPS ,
                  ZFC_SECURITY_GRPS_SE ,
                  ZFC_SECURITY_GRPS_RLS ,
                  ZFC_SECURITY_RLS_SE ,
                  ZFC_SECURITY_RLS
                WHERE ZFC_SECURITY_ACCTS_GRPS.ACCT_SEQ_ID = ZFC_ACCTS.ACCT_SEQ_ID
                        AND ZFC_SECURITY_GRPS_SE.SE_SEQ_ID IN ( SELECT SE_SEQ_ID
                                                            FROM TABLE(ZFF_GET_SE_PARENTS(1, v_P_SE_SEQ_ID))  )
                        AND ZFC_SECURITY_GRPS_SE.GRPS_SE_SEQ_ID = ZFC_SECURITY_GRPS_RLS.GRPS_SE_SEQ_ID
                        AND ZFC_SECURITY_RLS_SE.RLS_SE_SEQ_ID = ZFC_SECURITY_GRPS_RLS.RLS_SE_SEQ_ID
                        AND ZFC_SECURITY_RLS_SE.ROLE_SEQ_ID = ZFC_SECURITY_RLS.ROLE_SEQ_ID );

         OPEN cv_1 FOR
            SELECT DISTINCT *
              FROM tt_v_V_Accounts ;

      END;
      END IF;

   END;
   -- END IF
   ELSE
   NULL
   END IF;

   BEGIN
      OPEN cv_2 FOR
         -- SELECT an existing row from the table.
         SELECT ACCT_SEQ_ID,
                     STATUS_SEQ_ID,
                     ACCT,
                     FIRST_NAME,
                     LAST_NAME,
                     MIDDLE_NAME,
                     PREFERED_NAME,
                     EMAIL,
                     PASSWORD_LAST_SET,
                     PWD,
                     FAILED_ATTEMPTS,
                     IS_SYSTEM_ADMIN,
                     ADDED_BY,
                     ADDED_DATE,
                     LAST_LOGIN,
                     TIME_ZONE,
                     LOCATION,
                     ENABLE_NOTIFICATIONS,
                     UPDATED_BY,
                     UPDATED_DATE
           FROM ZFC_ACCTS
            WHERE ACCT = v_P_ACCOUNT;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   -- END IF
   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_ACCTS_NI_GRP
(
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_GROUP_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      OPEN cv_1 FOR
         SELECT ACCT
           FROM ZFC_ACCTS
            WHERE ACCT NOT IN ( SELECT Accounts.ACCT
                                FROM ZFC_ACCTS Accounts,
                                     ZFC_SECURITY_ACCTS_GRPS AcctSecurity,
                                     ZFC_SECURITY_GRPS_SE Security,
                                     ZFC_SECURITY_GRPS Groups
                                   WHERE Accounts.ACCT_SEQ_ID = AcctSecurity.ACCT_SEQ_ID
                                           AND AcctSecurity.GRPS_SE_SEQ_ID = Security.GRPS_SE_SEQ_ID
                                           AND Security.GROUP_SEQ_ID = Groups.GROUP_SEQ_ID
                                           AND Accounts.STATUS_SEQ_ID <> 2
                                           AND Groups.GROUP_SEQ_ID = v_P_GROUP_SEQ_ID
                                           AND Security.SE_SEQ_ID = v_P_SE_SEQ_ID )
           ORDER BY ACCT;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_ACCTS_NI_RL
(
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ROLE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      OPEN cv_1 FOR
         SELECT ACCT
           FROM ZFC_ACCTS
            WHERE ACCT NOT IN ( SELECT Accounts.ACCT
                                FROM ZFC_ACCTS Accounts,
                                     ZFC_SECURITY_ACCTS_RLS AcctSecurity,
                                     ZFC_SECURITY_RLS_SE Security,
                                     ZFC_SECURITY_RLS Roles
                                   WHERE Accounts.ACCT_SEQ_ID = AcctSecurity.ACCT_SEQ_ID
                                           AND AcctSecurity.RLS_SE_SEQ_ID = Security.RLS_SE_SEQ_ID
                                           AND Security.ROLE_SEQ_ID = Roles.ROLE_SEQ_ID
                                           AND Accounts.STATUS_SEQ_ID <> 2
                                           AND Roles.ROLE_SEQ_ID = v_P_ROLE_SEQ_ID
                                           AND Security.SE_SEQ_ID = v_P_SE_SEQ_ID )
           ORDER BY ACCT;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_ACCTS_N_GRP
(
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_GROUP_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      OPEN cv_1 FOR
         SELECT Accounts.ACCT
           FROM ZFC_ACCTS Accounts,
                ZFC_SECURITY_ACCTS_GRPS AcctSecurity,
                ZFC_SECURITY_GRPS_SE Security,
                ZFC_SECURITY_GRPS Groups
            WHERE Accounts.ACCT_SEQ_ID = AcctSecurity.ACCT_SEQ_ID
                    AND AcctSecurity.GRPS_SE_SEQ_ID = Security.GRPS_SE_SEQ_ID
                    AND Security.GROUP_SEQ_ID = Groups.GROUP_SEQ_ID
                    AND Accounts.STATUS_SEQ_ID <> 2
                    AND Groups.GROUP_SEQ_ID = v_P_GROUP_SEQ_ID
                    AND Security.SE_SEQ_ID = v_P_SE_SEQ_ID
           ORDER BY Accounts.ACCT;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_ACCTS_N_RL
(
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ROLE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      OPEN cv_1 FOR
         SELECT Accounts.ACCT
           FROM ZFC_ACCTS Accounts,
                ZFC_SECURITY_ACCTS_RLS AcctSecurity,
                ZFC_SECURITY_RLS_SE Security,
                ZFC_SECURITY_RLS Roles
            WHERE Accounts.ACCT_SEQ_ID = AcctSecurity.ACCT_SEQ_ID
                    AND AcctSecurity.RLS_SE_SEQ_ID = Security.RLS_SE_SEQ_ID
                    AND Security.ROLE_SEQ_ID = Roles.ROLE_SEQ_ID
                    AND Accounts.STATUS_SEQ_ID <> 2
                    AND Roles.ROLE_SEQ_ID = v_P_ROLE_SEQ_ID
                    AND Security.SE_SEQ_ID = v_P_SE_SEQ_ID
           ORDER BY Accounts.ACCT;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_ACCT_CHOICES
(
  v_P_ACCOUNT IN VARCHAR2 DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      OPEN cv_1 FOR
         -- SELECT an existing row from the table.
         SELECT ACCT,
                     SE_SEQ_ID,
                     SE_NAME,
                     BACK_COLOR,
                     LEFT_COLOR,
                     HEAD_COLOR,
                     SUB_HEAD_COLOR,
                     COLOR_SCHEME,
                     FAVORIATE_ACTION,
                     THIN_ACTIONS,
                     WIDE_ACTIONS,
                     RECORDS_PER_PAGE,
                     EDIT_ID
           FROM ZFO_ACCT_CHOICES
            WHERE ACCT = v_P_ACCOUNT;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_ACCT_GRPS
(
  v_P_ACCOUNT IN VARCHAR2 DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      OPEN cv_1 FOR
         -- SELECT one or more existing rows from the table.
         SELECT ZFC_SECURITY_GRPS.NAME GROUPS
           FROM ZFC_ACCTS ,
                ZFC_SECURITY_ACCTS_GRPS ,
                ZFC_SECURITY_GRPS_SE ,
                ZFC_SECURITY_GRPS
            WHERE ZFC_ACCTS.ACCT = v_P_ACCOUNT
                    AND ZFC_ACCTS.ACCT_SEQ_ID = ZFC_SECURITY_ACCTS_GRPS.ACCT_SEQ_ID
                    AND ZFC_SECURITY_ACCTS_GRPS.GRPS_SE_SEQ_ID = ZFC_SECURITY_GRPS_SE.GRPS_SE_SEQ_ID
                    AND ZFC_SECURITY_GRPS_SE.GROUP_SEQ_ID = ZFC_SECURITY_GRPS.GROUP_SEQ_ID
                    AND ZFC_SECURITY_GRPS_SE.SE_SEQ_ID = v_P_SE_SEQ_ID
           ORDER BY GROUPS;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_ACCT_RLS
(
  v_P_ACCOUNT IN VARCHAR2 DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      OPEN cv_1 FOR
         -- SELECT one or more existing rows from the table.
         SELECT ZFC_SECURITY_RLS.NAME ROLES
           FROM ZFC_ACCTS ,
                ZFC_SECURITY_ACCTS_RLS ,
                ZFC_SECURITY_RLS_SE ,
                ZFC_SECURITY_RLS
            WHERE ZFC_ACCTS.ACCT = v_P_ACCOUNT
                    AND ZFC_ACCTS.ACCT_SEQ_ID = ZFC_SECURITY_ACCTS_RLS.ACCT_SEQ_ID
                    AND ZFC_SECURITY_ACCTS_RLS.RLS_SE_SEQ_ID = ZFC_SECURITY_RLS_SE.RLS_SE_SEQ_ID
                    AND ZFC_SECURITY_RLS_SE.ROLE_SEQ_ID = ZFC_SECURITY_RLS.ROLE_SEQ_ID
                    AND ZFC_SECURITY_RLS_SE.SE_SEQ_ID = v_P_SE_SEQ_ID
           ORDER BY ROLES;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_ACCT_SECURITY
(
  v_P_ACCT IN VARCHAR2 DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   OPEN cv_1 FOR
      SELECT ZFC_SECURITY_RLS.NAME Roles
        FROM ZFC_ACCTS ,
             ZFC_SECURITY_ACCTS_RLS ,
             ZFC_SECURITY_RLS_SE ,
             ZFC_SECURITY_RLS
         WHERE ZFC_ACCTS.ACCT = v_P_ACCT
                 AND ZFC_SECURITY_ACCTS_RLS.ACCT_SEQ_ID = ZFC_ACCTS.ACCT_SEQ_ID
                 AND ZFC_SECURITY_ACCTS_RLS.RLS_SE_SEQ_ID = ZFC_SECURITY_RLS_SE.RLS_SE_SEQ_ID
                 AND ZFC_SECURITY_RLS_SE.SE_SEQ_ID IN ( SELECT SE_SEQ_ID
                                                    FROM TABLE(ZFF_GET_SE_PARENTS(1, v_P_SE_SEQ_ID))  )
                 AND ZFC_SECURITY_RLS_SE.ROLE_SEQ_ID = ZFC_SECURITY_RLS.ROLE_SEQ_ID
      UNION
      SELECT ZFC_SECURITY_RLS.NAME Roles
        FROM ZFC_ACCTS ,
             ZFC_SECURITY_ACCTS_GRPS ,
             ZFC_SECURITY_GRPS_SE ,
             ZFC_SECURITY_GRPS_RLS ,
             ZFC_SECURITY_RLS_SE ,
             ZFC_SECURITY_RLS
         WHERE ZFC_ACCTS.ACCT = v_P_ACCT
                 AND ZFC_SECURITY_ACCTS_GRPS.ACCT_SEQ_ID = ZFC_ACCTS.ACCT_SEQ_ID
                 AND ZFC_SECURITY_GRPS_SE.SE_SEQ_ID IN ( SELECT SE_SEQ_ID
                                                     FROM TABLE(ZFF_GET_SE_PARENTS(1, v_P_SE_SEQ_ID))  )
                 AND ZFC_SECURITY_GRPS_SE.GRPS_SE_SEQ_ID = ZFC_SECURITY_GRPS_RLS.GRPS_SE_SEQ_ID
                 AND ZFC_SECURITY_RLS_SE.RLS_SE_SEQ_ID = ZFC_SECURITY_GRPS_RLS.RLS_SE_SEQ_ID
                 AND ZFC_SECURITY_RLS_SE.ROLE_SEQ_ID = ZFC_SECURITY_RLS.ROLE_SEQ_ID;

   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_Security_Entity
(
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   -- SELECT an existing row OR rows from the table.
   IF v_P_SE_SEQ_ID = -1 THEN
   BEGIN
      OPEN cv_1 FOR
         SELECT SE_SEQ_ID,
                NAME,
                DESCRIPTION,
                URL,
                STATUS_SEQ_ID,
                DAL,
                DAL_NAME,
                DAL_NAME_SPACE,
                DAL_STRING,
                SKIN,
                STYLE,
                PARENT_SE_SEQ_ID,
                ENCRYPTION_TYPE,
                ADDED_BY,
                ADDED_DATE,
                UPDATED_BY,
                UPDATED_DATE
           FROM ZFC_SECURITY_ENTITIES
           ORDER BY NAME ASC;

   END;
   ELSE
   BEGIN
      OPEN cv_1 FOR
         SELECT SE_SEQ_ID,
                NAME,
                DESCRIPTION,
                URL,
                STATUS_SEQ_ID,
                DAL,
                DAL_NAME,
                DAL_NAME_SPACE,
                DAL_STRING,
                SKIN,
                STYLE,
                PARENT_SE_SEQ_ID,
                ENCRYPTION_TYPE,
                ADDED_BY,
                ADDED_DATE,
                UPDATED_BY,
                UPDATED_DATE
           FROM ZFC_SECURITY_ENTITIES
            WHERE SE_SEQ_ID = v_P_SE_SEQ_ID
           ORDER BY NAME ASC;

   END;
   END IF;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_DIRECTORY
(
  v_P_FUNCTION_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   IF v_P_FUNCTION_SEQ_ID = -1 THEN
   BEGIN
      OPEN cv_1 FOR
         SELECT FUNCTION_SEQ_ID,
                DIRECTORY,
                IMPERSONATE,
                IMPERSONATE_ACCOUNT,
                IMPERSONATE_PWD,
                ADDED_BY,
                ADDED_DATE,
                UPDATED_BY,
                UPDATED_DATE
           FROM ZFO_DIRECTORIES ;

   END;
   ELSE
   BEGIN
      OPEN cv_1 FOR
         SELECT FUNCTION_SEQ_ID,
                DIRECTORY,
                IMPERSONATE,
                IMPERSONATE_ACCOUNT,
                IMPERSONATE_PWD,
                ADDED_BY,
                ADDED_DATE,
                UPDATED_BY,
                UPDATED_DATE
           FROM ZFO_DIRECTORIES
            WHERE FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID;

   END;
   END IF;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_FUNCTION
(
  v_P_FUNCTION_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   IF v_P_FUNCTION_SEQ_ID <> -1 THEN
   BEGIN-- SELECT an existing row from the table.
   
      OPEN cv_1 FOR
         SELECT FUNCTION_SEQ_ID,
                NAME,
                DESCRIPTION,
                FUNCTION_TYPE_SEQ_ID,
                ALLOW_HTML_INPUT,
                ALLOW_COMMENT_HTML_INPUT,
                SOURCE,
                ENABLE_VIEW_STATE,
                IS_NAV,
                NAV_TYPE_ID,
                ACTION,
                PARENT_FUNCTION_SEQ_ID,
                ADDED_BY,
                ADDED_DATE,
                UPDATED_BY,
                UPDATED_DATE
           FROM ZFC_FUNCTIONS
            WHERE FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID
           ORDER BY FUNCTION_SEQ_ID ASC;

   END;
   ELSE
   BEGIN
      OPEN cv_1 FOR
         SELECT FUNCTION_SEQ_ID,
                NAME,
                DESCRIPTION,
                FUNCTION_TYPE_SEQ_ID,
                ALLOW_HTML_INPUT,
                ALLOW_COMMENT_HTML_INPUT,
                SOURCE,
                ENABLE_VIEW_STATE,
                IS_NAV,
                NAV_TYPE_ID,
                ACTION,
                PARENT_FUNCTION_SEQ_ID,
                ADDED_BY,
                ADDED_DATE,
                UPDATED_BY,
                UPDATED_DATE
           FROM ZFC_FUNCTIONS
           ORDER BY FUNCTION_SEQ_ID ASC;

   END;
   END IF;

   -- END IF
   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_FUNCTIONS_SECURITY
(
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   OPEN cv_1 FOR
      SELECT DISTINCT FUNCTIONS.FUNCTION_SEQ_ID FUNCTION_SEQ_ID,-- Directly assigned roles
      
                           PERMISSIONS.PERMISSIONS_ID,
                           ROLES.NAME ROLE
        FROM ZFC_SECURITY_RLS_SE SE_ROLES,
             ZFC_SECURITY_RLS ROLES,
             ZFC_SECURITY_FUNCT_RLS SECURITY,
             ZFC_FUNCTIONS FUNCTIONS,
             ZFC_PERMISSIONS PERMISSIONS
         WHERE SE_ROLES.ROLE_SEQ_ID = ROLES.ROLE_SEQ_ID
                 AND SECURITY.RLS_SE_SEQ_ID = SE_ROLES.RLS_SE_SEQ_ID
                 AND SECURITY.FUNCTION_SEQ_ID = FUNCTIONS.FUNCTION_SEQ_ID
                 AND PERMISSIONS.PERMISSIONS_ID = SECURITY.PERMISSIONS_ID
                 AND SE_ROLES.SE_SEQ_ID IN ( SELECT SE_SEQ_ID
                                         FROM TABLE(ZFF_GET_SE_PARENTS(1, v_P_SE_SEQ_ID))  )
      UNION
      SELECT DISTINCT FUNCTIONS.FUNCTION_SEQ_ID FUNCTION_SEQ_ID,-- Roles assigned via groups
      
                           PERMISSIONS.PERMISSIONS_ID,
                           ROLES.NAME ROLE
        FROM ZFC_SECURITY_FUNCT_GRPS ,
             ZFC_SECURITY_GRPS_SE ,
             ZFC_SECURITY_GRPS_RLS ,
             ZFC_SECURITY_RLS_SE ,
             ZFC_SECURITY_RLS ROLES,
             ZFC_FUNCTIONS FUNCTIONS,
             ZFC_PERMISSIONS PERMISSIONS
         WHERE ZFC_SECURITY_FUNCT_GRPS.FUNCTION_SEQ_ID = FUNCTIONS.FUNCTION_SEQ_ID
                 AND ZFC_SECURITY_GRPS_SE.GRPS_SE_SEQ_ID = ZFC_SECURITY_FUNCT_GRPS.GRPS_SE_SEQ_ID
                 AND ZFC_SECURITY_GRPS_RLS.GRPS_SE_SEQ_ID = ZFC_SECURITY_GRPS_SE.GRPS_SE_SEQ_ID
                 AND ZFC_SECURITY_RLS_SE.RLS_SE_SEQ_ID = ZFC_SECURITY_GRPS_RLS.RLS_SE_SEQ_ID
                 AND ROLES.ROLE_SEQ_ID = ZFC_SECURITY_RLS_SE.ROLE_SEQ_ID
                 AND PERMISSIONS.PERMISSIONS_ID = ZFC_SECURITY_FUNCT_GRPS.PERMISSIONS_ID
                 AND ZFC_SECURITY_GRPS_SE.SE_SEQ_ID IN ( SELECT SE_SEQ_ID
                                                     FROM TABLE(ZFF_GET_SE_PARENTS(1, v_P_SE_SEQ_ID))  );

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_FUNCTION_GRPS
(
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_FUNCTION_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_PERMISSIONS_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      OPEN cv_1 FOR
         -- SELECT one or more existing rows from the table.
         SELECT ZFC_SECURITY_GRPS.NAME Groups
           FROM ZFC_FUNCTIONS ,
                ZFC_SECURITY_FUNCT_GRPS ,
                ZFC_SECURITY_GRPS_SE ,
                ZFC_SECURITY_GRPS
            WHERE ZFC_FUNCTIONS.FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID
                    AND ZFC_FUNCTIONS.FUNCTION_SEQ_ID = ZFC_SECURITY_FUNCT_GRPS.FUNCTION_SEQ_ID
                    AND ZFC_SECURITY_FUNCT_GRPS.GRPS_SE_SEQ_ID = ZFC_SECURITY_GRPS_SE.GRPS_SE_SEQ_ID
                    AND ZFC_SECURITY_FUNCT_GRPS.PERMISSIONS_ID = v_P_PERMISSIONS_ID
                    AND ZFC_SECURITY_GRPS_SE.GROUP_SEQ_ID = ZFC_SECURITY_GRPS.GROUP_SEQ_ID
                    AND ZFC_SECURITY_GRPS_SE.SE_SEQ_ID = v_P_SE_SEQ_ID
           ORDER BY Groups;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_FUNCTION_RLS
(
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_FUNCTION_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_PERMISSIONS_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      OPEN cv_1 FOR
         -- SELECT one or more existing rows from the table.
         SELECT ZFC_SECURITY_RLS.NAME ROLES
           FROM ZFC_FUNCTIONS ,
                ZFC_SECURITY_FUNCT_RLS ,
                ZFC_SECURITY_RLS_SE ,
                ZFC_SECURITY_RLS
            WHERE ZFC_FUNCTIONS.FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID
                    AND ZFC_FUNCTIONS.FUNCTION_SEQ_ID = ZFC_SECURITY_FUNCT_RLS.FUNCTION_SEQ_ID
                    AND ZFC_SECURITY_FUNCT_RLS.RLS_SE_SEQ_ID = ZFC_SECURITY_RLS_SE.RLS_SE_SEQ_ID
                    AND ZFC_SECURITY_FUNCT_RLS.PERMISSIONS_ID = v_P_PERMISSIONS_ID
                    AND ZFC_SECURITY_RLS_SE.ROLE_SEQ_ID = ZFC_SECURITY_RLS.ROLE_SEQ_ID
                    AND ZFC_SECURITY_RLS_SE.SE_SEQ_ID = v_P_SE_SEQ_ID
           ORDER BY ROLES;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_FUNCTION_SORT
(
  v_P_FUNCTION_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_V_PARENT_ID NUMBER(10,0);
   v_V_NAV_TYPE_ID NUMBER(10,0);
BEGIN

   SELECT PARENT_FUNCTION_SEQ_ID
     INTO v_V_PARENT_ID
     FROM ZFC_FUNCTIONS
      WHERE FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID;

   SELECT NAV_TYPE_ID
     INTO v_V_NAV_TYPE_ID
     FROM ZFC_FUNCTIONS
      WHERE FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID;

   OPEN cv_1 FOR
      SELECT FUNCTION_SEQ_ID,
             NAME,
             ACTION,
             SORT_ORDER
        FROM ZFC_FUNCTIONS
         WHERE PARENT_FUNCTION_SEQ_ID = v_V_PARENT_ID
                 AND IS_NAV = 1
                 AND NAV_TYPE_ID = v_V_NAV_TYPE_ID
        ORDER BY SORT_ORDER ASC;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_FUNCTION_TYPES
(
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      OPEN cv_1 FOR
         -- SELECT all rows from the table.
         SELECT *
           FROM ZFC_FUNCTION_TYPES ;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_GROUP
(
  v_P_GROUP_SEQ_ID IN NUMBER DEFAULT NULL ,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
BEGIN

   OPEN cv_1 FOR
      SELECT *
        FROM ZFC_SECURITY_GRPS
         WHERE GROUP_SEQ_ID = v_P_GROUP_SEQ_ID;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_INFORMATION
-- =============================================
-- Author:		Michael Regan
-- Create date: 7/19/2008
-- Description:	Retrieves the information from ZFC_INFORMATION -- there should only be 1 row.
-- =============================================
(
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
BEGIN

   OPEN cv_1 FOR
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SELECT Information_SEQ_ID,
                  Version,
                  Enable_Inheritance,
                  ADDED_BY,
                  ADDED_DATE,
                  UPDATED_BY,
                  UPDATED_DATE
        FROM ZFC_INFORMATION ;

END;
/
/* Translation Extracted DDL For Required Objects */
CREATE GLOBAL TEMPORARY TABLE tt_v_V_AvalibleMenuItems
(
  ID NUMBER(10,0) ,
  TITLE VARCHAR2(30) ,
  DESCRIPTION VARCHAR2(256) ,
  URL VARCHAR2(256) ,
  PARENT NUMBER(10,0) ,
  SORT_ORDER NUMBER(10,0) ,
  ROLE VARCHAR2(50) ,
  FUNCTION_TYPE_SEQ_ID NUMBER(10,0) 
);
CREATE GLOBAL TEMPORARY TABLE tt_v_V_AccountRoles
(
  Roles VARCHAR2(30) 
);
CREATE GLOBAL TEMPORARY TABLE tt_v_V_AllMenuItems
(
  ID NUMBER(10,0) ,
  TITLE VARCHAR2(30) ,
  DESCRIPTION VARCHAR2(256) ,
  URL VARCHAR2(256) ,
  PARENT NUMBER(10,0) ,
  SORT_ORDER NUMBER(10,0) ,
  ROLE VARCHAR2(50) ,
  FUNCTION_TYPE_SEQ_ID NUMBER(10,0) 
);
CREATE GLOBAL TEMPORARY TABLE tt_v_V_DistinctMenuItems
(
  ID NUMBER(10,0) ,
  TITLE VARCHAR2(30) ,
  DESCRIPTION VARCHAR2(256) ,
  URL VARCHAR2(256) ,
  PARENT NUMBER(10,0) ,
  SORT_ORDER NUMBER(10,0) ,
  FUNCTION_TYPE_SEQ_ID NUMBER(10,0) 
);


CREATE OR REPLACE PROCEDURE ZFP_GET_MENU_DATA
(
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_NAV_TYPE_ID IN NUMBER DEFAULT NULL ,
  v_P_ACCT IN VARCHAR2 DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
   v_V_PERMISSION_ID NUMBER(10,0);
BEGIN

   v_V_PERMISSION_ID := ZFF_GET_VIEW_PERMISSION_ID();

   INSERT INTO tt_v_V_AvalibleMenuItems
     ( SELECT FUNCTIONS.FUNCTION_SEQ_ID ID,-- Menu items via roles
     
                   FUNCTIONS.NAME TITLE,
                   FUNCTIONS.DESCRIPTION,
                   FUNCTIONS.ACTION URL,
                   FUNCTIONS.PARENT_FUNCTION_SEQ_ID PARENT,
                   FUNCTIONS.SORT_ORDER SORT_ORDER,
                   ROLES.NAME ROLE,
                   FUNCTIONS.FUNCTION_TYPE_SEQ_ID
       FROM ZFC_SECURITY_RLS_SE SE_ROLES,
            ZFC_SECURITY_RLS ROLES,
            ZFC_SECURITY_FUNCT_RLS SECURITY,
            ZFC_FUNCTIONS FUNCTIONS,
            ZFC_PERMISSIONS PERMISSIONS
          WHERE SE_ROLES.ROLE_SEQ_ID = ROLES.ROLE_SEQ_ID
                  AND SECURITY.RLS_SE_SEQ_ID = SE_ROLES.RLS_SE_SEQ_ID
                  AND SECURITY.FUNCTION_SEQ_ID = FUNCTIONS.FUNCTION_SEQ_ID
                  AND PERMISSIONS.PERMISSIONS_ID = SECURITY.PERMISSIONS_ID
                  AND PERMISSIONS.PERMISSIONS_ID = v_V_PERMISSION_ID
                  AND FUNCTIONS.NAV_TYPE_ID = v_P_NAV_TYPE_ID
                  AND FUNCTIONS.IS_NAV = 1
                  AND SE_ROLES.SE_SEQ_ID IN ( SELECT SE_SEQ_ID
                                          FROM TABLE(ZFF_GET_SE_PARENTS(1, v_P_SE_SEQ_ID))  ) );

   INSERT INTO tt_v_V_AvalibleMenuItems
     ( SELECT FUNCTIONS.FUNCTION_SEQ_ID ID,-- Menu items via groups
     
                   FUNCTIONS.NAME TITLE,
                   FUNCTIONS.DESCRIPTION,
                   FUNCTIONS.ACTION URL,
                   FUNCTIONS.PARENT_FUNCTION_SEQ_ID PARENT,
                   FUNCTIONS.SORT_ORDER SORT_ORDER,
                   ROLES.NAME ROLE,
                   FUNCTIONS.FUNCTION_TYPE_SEQ_ID
       FROM ZFC_SECURITY_FUNCT_GRPS ,
            ZFC_SECURITY_GRPS_SE ,
            ZFC_SECURITY_GRPS_RLS ,
            ZFC_SECURITY_RLS_SE ,
            ZFC_SECURITY_RLS ROLES,
            ZFC_FUNCTIONS FUNCTIONS,
            ZFC_PERMISSIONS PERMISSIONS
          WHERE ZFC_SECURITY_FUNCT_GRPS.FUNCTION_SEQ_ID = FUNCTIONS.FUNCTION_SEQ_ID
                  AND ZFC_SECURITY_GRPS_SE.GRPS_SE_SEQ_ID = ZFC_SECURITY_FUNCT_GRPS.GRPS_SE_SEQ_ID
                  AND ZFC_SECURITY_GRPS_RLS.GRPS_SE_SEQ_ID = ZFC_SECURITY_GRPS_SE.GRPS_SE_SEQ_ID
                  AND ZFC_SECURITY_RLS_SE.RLS_SE_SEQ_ID = ZFC_SECURITY_GRPS_RLS.RLS_SE_SEQ_ID
                  AND ROLES.ROLE_SEQ_ID = ZFC_SECURITY_RLS_SE.ROLE_SEQ_ID
                  AND PERMISSIONS.PERMISSIONS_ID = ZFC_SECURITY_FUNCT_GRPS.PERMISSIONS_ID
                  AND PERMISSIONS.PERMISSIONS_ID = v_V_PERMISSION_ID
                  AND FUNCTIONS.NAV_TYPE_ID = v_P_NAV_TYPE_ID
                  AND FUNCTIONS.IS_NAV = 1
                  AND ZFC_SECURITY_GRPS_SE.SE_SEQ_ID IN ( SELECT SE_SEQ_ID
                                                      FROM TABLE(ZFF_GET_SE_PARENTS(1, v_P_SE_SEQ_ID))  ) );

   --SELECT * FROM @V_AvalibleMenuItems -- DEBUG
   -- Roles belonging to the account
   INSERT INTO tt_v_V_AccountRoles
     ( SELECT-- Roles via roles
      ZFC_SECURITY_RLS.NAME Roles
       FROM ZFC_ACCTS ,
            ZFC_SECURITY_ACCTS_RLS ,
            ZFC_SECURITY_RLS_SE ,
            ZFC_SECURITY_RLS
          WHERE ZFC_ACCTS.ACCT = v_P_ACCT
                  AND ZFC_SECURITY_ACCTS_RLS.ACCT_SEQ_ID = ZFC_ACCTS.ACCT_SEQ_ID
                  AND ZFC_SECURITY_ACCTS_RLS.RLS_SE_SEQ_ID = ZFC_SECURITY_RLS_SE.RLS_SE_SEQ_ID
                  AND ZFC_SECURITY_RLS_SE.SE_SEQ_ID IN ( SELECT SE_SEQ_ID
                                                     FROM TABLE(ZFF_GET_SE_PARENTS(1, v_P_SE_SEQ_ID))  )
                  AND ZFC_SECURITY_RLS_SE.ROLE_SEQ_ID = ZFC_SECURITY_RLS.ROLE_SEQ_ID
       UNION
       SELECT-- Roles via groups
        ZFC_SECURITY_RLS.NAME Roles
       FROM ZFC_ACCTS ,
            ZFC_SECURITY_ACCTS_GRPS ,
            ZFC_SECURITY_GRPS_SE ,
            ZFC_SECURITY_GRPS_RLS ,
            ZFC_SECURITY_RLS_SE ,
            ZFC_SECURITY_RLS
          WHERE ZFC_ACCTS.ACCT = v_P_ACCT
                  AND ZFC_SECURITY_ACCTS_GRPS.ACCT_SEQ_ID = ZFC_ACCTS.ACCT_SEQ_ID
                  AND ZFC_SECURITY_GRPS_SE.SE_SEQ_ID IN ( SELECT SE_SEQ_ID
                                                      FROM TABLE(ZFF_GET_SE_PARENTS(1, v_P_SE_SEQ_ID))  )
                  AND ZFC_SECURITY_GRPS_SE.GRPS_SE_SEQ_ID = ZFC_SECURITY_GRPS_RLS.GRPS_SE_SEQ_ID
                  AND ZFC_SECURITY_RLS_SE.RLS_SE_SEQ_ID = ZFC_SECURITY_GRPS_RLS.RLS_SE_SEQ_ID
                  AND ZFC_SECURITY_RLS_SE.ROLE_SEQ_ID = ZFC_SECURITY_RLS.ROLE_SEQ_ID );

   --SELECT * FROM @V_AccountRoles -- DEBUG
   INSERT INTO tt_v_V_AllMenuItems
     ( SELECT ID,-- Last but not least get the menu items when there are matching account roles.
     
                   TITLE,
                   DESCRIPTION,
                   URL,
                   PARENT,
                   SORT_ORDER,
                   ROLE,
                   FUNCTION_TYPE_SEQ_ID
       FROM tt_v_V_AvalibleMenuItems
          WHERE ROLE IN ( SELECT DISTINCT *
                          FROM tt_v_V_AccountRoles  ) );

   INSERT INTO tt_v_V_DistinctMenuItems
     ( SELECT DISTINCT ID,
                       TITLE,
                       DESCRIPTION,
                       URL,
                       PARENT,
                       SORT_ORDER,
                       FUNCTION_TYPE_SEQ_ID
       FROM tt_v_V_AllMenuItems  );

   BEGIN
      OPEN cv_1 FOR
         SELECT ID MenuID,
                TITLE,
                DESCRIPTION,
                URL,
                PARENT ParentID,
                SORT_ORDER,
                FUNCTION_TYPE_SEQ_ID
           FROM tt_v_V_DistinctMenuItems
           ORDER BY SORT_ORDER;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_MESSAGE
(
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_MESSAGE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   IF v_P_MESSAGE_SEQ_ID > -1 THEN
      OPEN cv_1 FOR
         SELECT *
           FROM ZFO_MESSAGES
            WHERE MESSAGE_SEQ_ID = v_P_MESSAGE_SEQ_ID;

   ELSE
      OPEN cv_1 FOR
         SELECT *
           FROM ZFO_MESSAGES
            WHERE SE_SEQ_ID = v_P_SE_SEQ_ID;

   END IF;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_NAVIGATION_TYPES
(
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      OPEN cv_1 FOR
         -- SELECT all rows from the table.
         SELECT *
           FROM ZFC_NAVIGATION_TYPES
           ORDER BY DESCRIPTION;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_ROLE
(
  v_P_ROLE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   IF v_P_ROLE_SEQ_ID > -1 THEN-- SELECT an existing row from the table.
   
      OPEN cv_1 FOR
         SELECT ROLE_SEQ_ID,
                NAME,
                DESCRIPTION,
                IS_SYSTEM,
                IS_SYSTEM_ONLY,
                ADDED_BY,
                ADDED_DATE,
                UPDATED_BY,
                UPDATED_DATE
           FROM ZFC_SECURITY_RLS
            WHERE ROLE_SEQ_ID = v_P_ROLE_SEQ_ID;

   ELSE-- GET ALL ROLES FOR A GIVEN Security Entity
   
      OPEN cv_1 FOR
         SELECT ZFC_SECURITY_RLS.ROLE_SEQ_ID,
                ZFC_SECURITY_RLS.NAME,
                ZFC_SECURITY_RLS.DESCRIPTION,
                ZFC_SECURITY_RLS.IS_SYSTEM,
                ZFC_SECURITY_RLS.IS_SYSTEM_ONLY,
                ZFC_SECURITY_RLS.ADDED_BY,
                ZFC_SECURITY_RLS.ADDED_DATE,
                ZFC_SECURITY_RLS.UPDATED_BY,
                ZFC_SECURITY_RLS.UPDATED_DATE
           FROM ZFC_SECURITY_RLS ,
                ZFC_SECURITY_RLS_SE
            WHERE ZFC_SECURITY_RLS.ROLE_SEQ_ID = ZFC_SECURITY_RLS_SE.ROLE_SEQ_ID
                    AND ZFC_SECURITY_RLS_SE.SE_SEQ_ID = v_P_SE_SEQ_ID;

   END IF;

   -- END IF
   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_STATE
(
  v_P_STATE IN VARCHAR2 DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      IF v_P_STATE = 'NEG1' THEN
      BEGIN
         OPEN cv_1 FOR
            SELECT STATE,
                   DESCRIPTION,
                   STATUS_SEQ_ID,
                   ADDED_BY,
                   ADDED_DATE,
                   UPDATED_BY,
                   UPDATED_DATE
              FROM ZOP_STATES
               WHERE STATE = v_P_STATE;

      END;
      ELSE
      BEGIN
         OPEN cv_1 FOR
            SELECT STATE,
                   DESCRIPTION,
                   STATUS_SEQ_ID,
                   ADDED_BY,
                   ADDED_DATE,
                   UPDATED_BY,
                   UPDATED_DATE
              FROM ZOP_STATES ;

      END;
      END IF;

   END;
   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
/* Translation Extracted DDL For Required Objects */
CREATE GLOBAL TEMPORARY TABLE tt_v_T_VALID_SE
(
  SE_SEQ_ID NUMBER(10,0) 
);


CREATE OR REPLACE PROCEDURE ZFP_GET_VALID_SES
(
  v_P_ACCT IN VARCHAR2 DEFAULT NULL ,
  v_P_IS_SE_ADMIN IN NUMBER DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR,
  cv_2 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
   v_V_ACTIVE_STATUS VARCHAR2(50);
   v_V_IS_SYS_ADMIN NUMBER(10,0);
BEGIN

   SELECT STATUS_SEQ_ID
     INTO v_V_ACTIVE_STATUS
     FROM ZFC_SYSTEM_STATUS
      WHERE UPPER(DESCRIPTION) = 'ACTIVE';

   SELECT IS_SYSTEM_ADMIN
     INTO v_V_IS_SYS_ADMIN
     FROM ZFC_ACCTS
      WHERE UPPER(ACCT) = UPPER(v_P_ACCT);

   IF v_V_IS_SYS_ADMIN = 0 THEN
   BEGIN
      INSERT INTO tt_v_T_VALID_SE
        ( SELECT-- Security Entitys via roles
         ZFC_SECURITY_RLS_SE.SE_SEQ_ID
          FROM ZFC_ACCTS ,
               ZFC_SECURITY_ACCTS_RLS ,
               ZFC_SECURITY_RLS_SE ,
               ZFC_SECURITY_RLS
             WHERE ZFC_ACCTS.ACCT = v_P_ACCT
                     AND ZFC_SECURITY_ACCTS_RLS.ACCT_SEQ_ID = ZFC_ACCTS.ACCT_SEQ_ID
                     AND ZFC_SECURITY_ACCTS_RLS.RLS_SE_SEQ_ID = ZFC_SECURITY_RLS_SE.RLS_SE_SEQ_ID
                     AND ZFC_SECURITY_RLS_SE.ROLE_SEQ_ID = ZFC_SECURITY_RLS.ROLE_SEQ_ID
                     AND ZFC_SECURITY_RLS.IS_SYSTEM_ONLY = 0
          UNION
          SELECT-- Security Entitys via groups
           ZFC_SECURITY_RLS_SE.SE_SEQ_ID
          FROM ZFC_ACCTS ,
               ZFC_SECURITY_ACCTS_GRPS ,
               ZFC_SECURITY_GRPS_SE ,
               ZFC_SECURITY_GRPS_RLS ,
               ZFC_SECURITY_RLS_SE ,
               ZFC_SECURITY_RLS
             WHERE ZFC_ACCTS.ACCT = v_P_ACCT
                     AND ZFC_SECURITY_ACCTS_GRPS.ACCT_SEQ_ID = ZFC_ACCTS.ACCT_SEQ_ID
                     AND ZFC_SECURITY_GRPS_SE.GRPS_SE_SEQ_ID = ZFC_SECURITY_GRPS_RLS.GRPS_SE_SEQ_ID
                     AND ZFC_SECURITY_RLS_SE.RLS_SE_SEQ_ID = ZFC_SECURITY_GRPS_RLS.RLS_SE_SEQ_ID
                     AND ZFC_SECURITY_RLS_SE.ROLE_SEQ_ID = ZFC_SECURITY_RLS.ROLE_SEQ_ID );

      IF v_P_IS_SE_ADMIN = 0 THEN-- FALSE
      
      BEGIN
         OPEN cv_1 FOR
            SELECT SE_SEQ_ID,
                   NAME,
                   DESCRIPTION
              FROM ZFC_SECURITY_ENTITIES
               WHERE SE_SEQ_ID IN ( SELECT *
                                FROM tt_v_T_VALID_SE  )
                       AND STATUS_SEQ_ID = v_V_ACTIVE_STATUS;

      END;
      ELSE
      BEGIN
         OPEN cv_1 FOR
            SELECT SE_SEQ_ID,
                   NAME,
                   DESCRIPTION
              FROM ZFC_SECURITY_ENTITIES
               WHERE SE_SEQ_ID IN ( SELECT *
                                FROM tt_v_T_VALID_SE  )
                       OR PARENT_SE_SEQ_ID = v_P_SE_SEQ_ID;

      END;
      END IF;

   END;
   -- END IF
   ELSE
   BEGIN
      OPEN cv_2 FOR
         SELECT SE_SEQ_ID,
                NAME,
                DESCRIPTION
           FROM ZFC_SECURITY_ENTITIES ;

   END;
   END IF;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_GET_WORK_FLOW
(
  v_P_PRIMARY_KEY IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER,
  cv_1 IN OUT SYS_REFCURSOR
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      OPEN cv_1 FOR
         -- SELECT A ROW  from the table.
         SELECT WORK_FLOW_ID,
                      ORDER_ID,
                      WORK_FLOW_NAME,
                      FUNCTION_SEQ_ID
           FROM ZOP_WORK_FLOWS
            WHERE WORK_FLOW_ID = v_P_PRIMARY_KEY
           ORDER BY ORDER_ID;
   EXCEPTION
      WHEN OTHERS THEN
         v_sys_error := SQLCODE;
   END;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_SYSTEM_STATUS
(
  iv_P_STATUS_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_DESCRIPTION IN VARCHAR2 DEFAULT NULL ,
  v_P_ADDED_BY IN NUMBER DEFAULT NULL ,
  v_P_ADDED_DATE IN DATE DEFAULT NULL ,
  v_P_UPDATED_BY IN NUMBER DEFAULT NULL ,
  v_P_UPDATED_DATE IN DATE DEFAULT NULL ,
  v_P_PRIMARY_KEY OUT NUMBER,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_P_STATUS_SEQ_ID NUMBER(10,0) := iv_P_STATUS_SEQ_ID;
   v_sys_error NUMBER := 0;
BEGIN

   IF v_P_STATUS_SEQ_ID > -1 THEN
   BEGIN-- UPDATE PROFILE
   
      UPDATE ZFC_SYSTEM_STATUS
         SET DESCRIPTION = v_P_DESCRIPTION,
             ADDED_BY = v_P_ADDED_BY,
             ADDED_DATE = v_P_ADDED_DATE,
             UPDATED_BY = v_P_UPDATED_BY,
             UPDATED_DATE = v_P_UPDATED_DATE
         WHERE STATUS_SEQ_ID = v_P_STATUS_SEQ_ID;

      v_P_PRIMARY_KEY := v_P_STATUS_SEQ_ID;

   END;
   ELSE
   DECLARE
      v_temp NUMBER(1, 0) := 0;
   BEGIN-- INSERT a new row in the table.
   
      BEGIN
         SELECT 1 INTO v_temp
           FROM DUAL
          WHERE EXISTS ( SELECT STATUS_SEQ_ID
                         FROM ZFC_SYSTEM_STATUS
                            WHERE STATUS_SEQ_ID = v_P_STATUS_SEQ_ID );
      EXCEPTION
         WHEN OTHERS THEN
            NULL;
      END;

      -- CHECK FOR DUPLICATE NAME BEFORE INSERTING
      IF v_temp = 1 THEN
      BEGIN
         raise_application_error( -20002, 'THE STATUS YOU ENTERED ALREADY EXISTS IN THE DATABASE.' );

         RETURN;

      END;
      END IF;

      INSERT INTO ZFC_SYSTEM_STATUS
        ( DESCRIPTION, ADDED_BY, ADDED_DATE, UPDATED_BY, UPDATED_DATE )
        VALUES ( v_P_DESCRIPTION, v_P_ADDED_BY, v_P_ADDED_DATE, v_P_UPDATED_BY, v_P_UPDATED_DATE );

      v_P_STATUS_SEQ_ID := sqlserver_utilities.identity;-- Get the IDENTITY value for the row just inserted.
      

   END;
   END IF;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_ACCT_CHOICES
(
  v_P_ACCT IN VARCHAR2 DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_SE_NAME IN VARCHAR2 DEFAULT NULL ,
  v_P_BACK_COLOR IN VARCHAR2 DEFAULT NULL ,
  v_P_LEFT_COLOR IN VARCHAR2 DEFAULT NULL ,
  v_P_HEAD_COLOR IN VARCHAR2 DEFAULT NULL ,
  v_P_SUB_HEAD_COLOR IN VARCHAR2 DEFAULT NULL ,
  v_P_COLOR_SCHEME IN VARCHAR2 DEFAULT NULL ,
  v_P_FAVORIATE_ACTION IN VARCHAR2 DEFAULT NULL ,
  v_P_THIN_ACTIONS IN VARCHAR2 DEFAULT NULL ,
  v_P_WIDE_ACTIONS IN VARCHAR2 DEFAULT NULL ,
  v_P_RECORDS_PER_PAGE IN NUMBER DEFAULT NULL ,
  v_P_EDIT_ID--,
   IN VARCHAR2 DEFAULT NULL
)
AS
   v_temp NUMBER(1, 0) := 0;
BEGIN

   BEGIN
      SELECT 1 INTO v_temp
        FROM DUAL
       WHERE ( SELECT COUNT(*)
               FROM ZFO_ACCT_CHOICES
                  WHERE ACCT = v_P_ACCT ) <= 0;
   EXCEPTION
      WHEN OTHERS THEN
         NULL;
   END;

   --@P_ErrorCode int OUTPUT
   -- INSERT a new row in the table.
   IF v_temp = 1 THEN
   BEGIN
      INSERT INTO ZFO_ACCT_CHOICES
        ( ACCT, SE_SEQ_ID, SE_NAME, BACK_COLOR, LEFT_COLOR, HEAD_COLOR, SUB_HEAD_COLOR, COLOR_SCHEME, FAVORIATE_ACTION, THIN_ACTIONS, WIDE_ACTIONS, RECORDS_PER_PAGE, EDIT_ID )
        VALUES ( v_P_ACCT, v_P_SE_SEQ_ID, v_P_SE_NAME, v_P_BACK_COLOR, v_P_LEFT_COLOR, v_P_HEAD_COLOR, v_P_SUB_HEAD_COLOR, v_P_COLOR_SCHEME, v_P_FAVORIATE_ACTION, v_P_THIN_ACTIONS, v_P_WIDE_ACTIONS, v_P_RECORDS_PER_PAGE, v_P_EDIT_ID );

   END;
   ELSE
   BEGIN
      UPDATE ZFO_ACCT_CHOICES
         SET SE_SEQ_ID = v_P_SE_SEQ_ID,
             SE_NAME = v_P_SE_NAME,
             BACK_COLOR = v_P_BACK_COLOR,
             LEFT_COLOR = v_P_LEFT_COLOR,
             HEAD_COLOR = v_P_HEAD_COLOR,
             SUB_HEAD_COLOR = v_P_SUB_HEAD_COLOR,
             COLOR_SCHEME = v_P_COLOR_SCHEME,
             FAVORIATE_ACTION = v_P_FAVORIATE_ACTION,
             THIN_ACTIONS = v_P_THIN_ACTIONS,
             WIDE_ACTIONS = v_P_WIDE_ACTIONS,
             RECORDS_PER_PAGE = v_P_RECORDS_PER_PAGE,
             EDIT_ID = v_P_EDIT_ID
         WHERE ACCT = v_P_ACCT;
      -- END IF
      -- Get the Error Code for the statement just executed.
      --SELECT @P_ErrorCode=@@ERROR
   END;
   END IF;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_ACCOUNT
(
  v_P_ACCT_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_STATUS_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ACCOUNT IN VARCHAR2 DEFAULT NULL ,
  v_P_FIRST_NAME IN VARCHAR2 DEFAULT NULL ,
  v_P_LAST_NAME IN VARCHAR2 DEFAULT NULL ,
  v_P_MIDDLE_NAME IN VARCHAR2 DEFAULT NULL ,
  v_P_PREFERED_NAME IN VARCHAR2 DEFAULT NULL ,
  v_P_EMAIL IN VARCHAR2 DEFAULT NULL ,
  v_P_PWD IN VARCHAR2 DEFAULT NULL ,
  v_P_PASSWORD_LAST_SET IN DATE DEFAULT NULL ,
  v_P_FAILED_ATTEMPTS IN NUMBER DEFAULT NULL ,
  v_P_ADDED_BY IN NUMBER DEFAULT NULL ,
  v_P_ADDED_DATE IN DATE DEFAULT NULL ,
  v_P_LAST_LOGIN IN DATE DEFAULT NULL ,
  v_P_TIME_ZONE IN NUMBER DEFAULT NULL ,
  v_P_LOCATION IN VARCHAR2 DEFAULT NULL ,
  v_P_ENABLE_NOTIFICATIONS IN NUMBER DEFAULT NULL ,
  v_P_IS_SYSTEM_ADMIN IN NUMBER DEFAULT NULL ,
  v_P_UPDATED_BY IN NUMBER DEFAULT NULL ,
  v_P_UPDATED_DATE IN DATE DEFAULT NULL ,
  v_P_PRIMARY_KEY OUT NUMBER,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
   v_SECURITY_ENTITY_SEQ_ID VARCHAR2(1);
   v_Security_Entity_NAME VARCHAR2(50);
   v_BACK_COLOR VARCHAR2(15);
   v_LEFT_COLOR VARCHAR2(15);
   v_HEAD_COLOR VARCHAR2(15);
   v_SUB_HEAD_COLOR VARCHAR2(15);
   v_COLOR_SCHEME VARCHAR2(15);
   v_THIN_ACTIONS VARCHAR2(256);
   v_WIDE_ACTIONS VARCHAR2(256);
   v_MODULE_ACTION VARCHAR2(25);
   v_RECORDS_PER_PAGE VARCHAR2(1000);
   v_DEFAULT_ACCOUNT VARCHAR2(50);
   v_EDIT_ID VARCHAR2(50);
BEGIN

   IF v_P_ACCT_SEQ_ID > -1 THEN
   BEGIN-- UPDATE PROFILE
   
      UPDATE ZFC_ACCTS
         SET STATUS_SEQ_ID = v_P_STATUS_SEQ_ID,
             ACCT = v_P_ACCOUNT,
             FIRST_NAME = v_P_FIRST_NAME,
             LAST_NAME = v_P_LAST_NAME,
             MIDDLE_NAME = v_P_MIDDLE_NAME,
             PREFERED_NAME = v_P_PREFERED_NAME,
             EMAIL = v_P_EMAIL,
             PASSWORD_LAST_SET = v_P_Password_Last_Set,
             PWD = v_P_PWD,
             FAILED_ATTEMPTS = v_P_FAILED_ATTEMPTS,
             ADDED_BY = v_P_ADDED_BY,
             ADDED_DATE = v_P_ADDED_DATE,
             LAST_LOGIN = v_P_LAST_LOGIN,
             TIME_ZONE = v_P_TIME_ZONE,
             LOCATION = v_P_LOCATION,
             IS_SYSTEM_ADMIN = v_P_IS_SYSTEM_ADMIN,
             ENABLE_NOTIFICATIONS = v_P_ENABLE_NOTIFICATIONS,
             UPDATED_BY = v_P_UPDATED_BY,
             UPDATED_DATE = v_P_UPDATED_DATE
         WHERE ACCT_SEQ_ID = v_P_ACCT_SEQ_ID;

      v_P_PRIMARY_KEY := v_P_ACCT_SEQ_ID;

   END;
   ELSE
   BEGIN-- INSERT a new row in the table.
   
      INSERT INTO ZFC_ACCTS
        ( STATUS_SEQ_ID, ACCT, FIRST_NAME, LAST_NAME, MIDDLE_NAME, PREFERED_NAME, EMAIL, PASSWORD_LAST_SET, PWD, FAILED_ATTEMPTS, IS_SYSTEM_ADMIN, ADDED_BY, ADDED_DATE, LAST_LOGIN, TIME_ZONE, LOCATION, ENABLE_NOTIFICATIONS, UPDATED_BY, UPDATED_DATE )
        VALUES ( v_P_STATUS_SEQ_ID, v_P_ACCOUNT, v_P_FIRST_NAME, v_P_LAST_NAME, v_P_MIDDLE_NAME, v_P_PREFERED_NAME, v_P_EMAIL, v_P_PASSWORD_LAST_SET, v_P_PWD, v_P_FAILED_ATTEMPTS, v_P_IS_SYSTEM_ADMIN, v_P_ADDED_BY, v_P_ADDED_DATE, v_P_LAST_LOGIN, v_P_TIME_ZONE, v_P_LOCATION, v_P_ENABLE_NOTIFICATIONS, v_P_UPDATED_BY, v_P_UPDATED_DATE );

      v_P_PRIMARY_KEY := sqlserver_utilities.identity;-- Get the IDENTITY value for the row just inserted.
      

      IF v_P_PRIMARY_KEY > 0 THEN
      DECLARE
         v_temp NUMBER(1, 0) := 0;
      BEGIN
         BEGIN
            SELECT 1 INTO v_temp
              FROM DUAL
             WHERE EXISTS ( SELECT *
                            FROM sysobjects
                               WHERE id = NULL/*TODO:OBJECT_ID(N'[dbo].[ZFO_ACCT_CHOICES]')*/
                                       AND NULL/*TODO:OBJECTPROPERTY(id, N'IsUserTable')*/ = 1 );
         EXCEPTION
            WHEN OTHERS THEN
               NULL;
         END;

         /*add an entry to account choice table*/
         IF v_temp = 1 THEN
         BEGIN
            SELECT ACCT
              INTO v_DEFAULT_ACCOUNT
              FROM ZFC_ACCTS
               WHERE ACCT_SEQ_ID = v_P_UPDATED_BY;

            IF v_DEFAULT_ACCOUNT IS NULL THEN
               v_DEFAULT_ACCOUNT := 'ANONYMOUS';

            END IF;

            SELECT SE_SEQ_ID,-- FILL THE DEFAULT VALUES
            
                         SE_NAME,
                         BACK_COLOR,
                         LEFT_COLOR,
                         HEAD_COLOR,
                         SUB_HEAD_COLOR,
                         COLOR_SCHEME,
                         FAVORIATE_ACTION,
                         THIN_ACTIONS,
                         WIDE_ACTIONS,
                         RECORDS_PER_PAGE,
                         EDIT_ID
              INTO v_SECURITY_ENTITY_SEQ_ID,
                   v_Security_Entity_NAME,
                   v_BACK_COLOR,
                   v_LEFT_COLOR,
                   v_HEAD_COLOR,
                   v_SUB_HEAD_COLOR,
                   v_COLOR_SCHEME,
                   v_MODULE_ACTION,
                   v_THIN_ACTIONS,
                   v_WIDE_ACTIONS,
                   v_RECORDS_PER_PAGE,
                   v_EDIT_ID
              FROM ZFO_ACCT_CHOICES
               WHERE ACCT = v_DEFAULT_ACCOUNT;

            ZFP_SET_ACCT_CHOICES(v_P_ACCOUNT,
                                 v_SECURITY_ENTITY_SEQ_ID,
                                 v_Security_Entity_NAME,
                                 v_BACK_COLOR,
                                 v_LEFT_COLOR,
                                 v_HEAD_COLOR,
                                 v_SUB_HEAD_COLOR,
                                 v_COLOR_SCHEME,
                                 v_MODULE_ACTION,
                                 v_THIN_ACTIONS,
                                 v_WIDE_ACTIONS,
                                 v_RECORDS_PER_PAGE,
                                 v_EDIT_ID);

         END;
         END IF;

      END;
      END IF;

   END;
   END IF;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_INFORMATION
-- =============================================
-- Author:		Michael Regan
-- Create date: 7/19/2008
-- Description:	Add's or updates the information in the ZFC_INFORMATION table.
-- =============================================
(
  v_P_Information_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_VERSION IN VARCHAR2 DEFAULT NULL ,
  v_P_Enable_Inheritance IN NUMBER DEFAULT NULL ,
  v_P_ADD_UP_BY IN NUMBER DEFAULT NULL ,
  v_P_PRIMARY_KEY OUT NUMBER,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_temp NUMBER(1, 0) := 0;
BEGIN

   BEGIN
      SELECT 1 INTO v_temp
        FROM DUAL
       WHERE ( SELECT COUNT(*)
               FROM ZFC_INFORMATION  ) = 0;
   EXCEPTION
      WHEN OTHERS THEN
         NULL;
   END;

   IF v_temp = 1 THEN
   BEGIN-- INSERT
   
      INSERT INTO ZFC_INFORMATION
        ( VERSION, Enable_Inheritance, ADDED_BY, ADDED_DATE, UPDATED_BY-- NO NULL VALUES PLEASE
        , UPDATED_DATE )-- NO NULL VALUES PLEASE
        
        VALUES ( v_P_VERSION, v_P_Enable_Inheritance, v_P_ADD_UP_BY, SYSDATE, v_P_ADD_UP_BY, SYSDATE );

      v_P_PRIMARY_KEY := sqlserver_utilities.identity;-- Get the IDENTITY value for the row just inserted.
      

   END;
   ELSE-- UPDATE
   
   BEGIN
      UPDATE ZFC_INFORMATION
         SET VERSION = v_P_VERSION,
             Enable_Inheritance = v_P_Enable_Inheritance,
             UPDATED_BY = v_P_ADD_UP_BY,
             UPDATED_DATE = SYSDATE
         WHERE Information_SEQ_ID = v_P_Information_SEQ_ID;

      v_P_PRIMARY_KEY := v_P_Information_SEQ_ID;
      -- END IF
   END;
   END IF;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_FUNCTION_TYPES
(
  v_P_FUNCTION_TYPE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_NAME IN VARCHAR2 DEFAULT NULL ,
  v_P_DESCRIPTION IN VARCHAR2 DEFAULT NULL ,
  v_P_TEMPLATE IN VARCHAR2 DEFAULT NULL ,
  v_P_IS_CONTENT IN NUMBER DEFAULT NULL ,
  v_P_ADDED_BY IN NUMBER DEFAULT NULL ,
  v_P_ADDED_DATE IN DATE DEFAULT NULL ,
  v_P_UPDATED_BY IN NUMBER DEFAULT NULL ,
  v_P_UPDATED_DATE IN DATE DEFAULT NULL ,
  v_P_PRIMARY_KEY OUT NUMBER,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   IF v_P_FUNCTION_TYPE_SEQ_ID > -1 THEN
   BEGIN-- UPDATE PROFILE
   
      UPDATE ZFC_FUNCTION_TYPES
         SET NAME = v_P_NAME,
             DESCRIPTION = v_P_DESCRIPTION,
             TEMPLATE = v_P_TEMPLATE,
             IS_CONTENT = v_P_IS_CONTENT,
             UPDATED_BY = v_P_UPDATED_BY,
             UPDATED_DATE = v_P_UPDATED_DATE
         WHERE FUNCTION_TYPE_SEQ_ID = v_P_FUNCTION_TYPE_SEQ_ID;

      v_P_PRIMARY_KEY := v_P_FUNCTION_TYPE_SEQ_ID;

   END;
   ELSE
   DECLARE
      v_temp NUMBER(1, 0) := 0;
   BEGIN-- INSERT a new row in the table.
   
      BEGIN
         SELECT 1 INTO v_temp
           FROM DUAL
          WHERE EXISTS ( SELECT v_P_DESCRIPTION
                         FROM ZFC_FUNCTION_TYPES
                            WHERE NAME = v_P_NAME );
      EXCEPTION
         WHEN OTHERS THEN
            NULL;
      END;

      -- CHECK FOR DUPLICATE NAME BEFORE INSERTING
      IF v_temp = 1 THEN
      BEGIN
         raise_application_error( -20002, 'THE FUNCTION TYPE YOU ENTERED ALREADY EXISTS IN THE DATABASE.' );

         RETURN;

      END;
      END IF;

      INSERT INTO ZFC_FUNCTION_TYPES
        ( NAME, DESCRIPTION, TEMPLATE, IS_CONTENT, ADDED_BY, ADDED_DATE, UPDATED_BY, UPDATED_DATE )
        VALUES ( v_P_NAME, v_P_DESCRIPTION, v_P_TEMPLATE, v_P_IS_CONTENT, v_P_ADDED_BY, v_P_ADDED_DATE, v_P_ADDED_BY, v_P_ADDED_DATE );

      v_P_PRIMARY_KEY := sqlserver_utilities.identity;-- Get the IDENTITY value for the row just inserted.
      

   END;
   END IF;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_NAVIGATION_TYPES
(
  v_P_NAV_TYPE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_DESCRIPTION IN VARCHAR2 DEFAULT NULL ,
  v_P_ADDED_BY IN NUMBER DEFAULT NULL ,
  v_P_ADDED_DATE IN DATE DEFAULT NULL ,
  v_P_UPDATED_BY IN NUMBER DEFAULT NULL ,
  v_P_UPDATED_DATE IN DATE DEFAULT NULL ,
  v_P_PRIMARY_KEY OUT NUMBER,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   IF v_P_NAV_TYPE_SEQ_ID > -1 THEN
   BEGIN-- UPDATE PROFILE
   
      UPDATE ZFC_NAVIGATION_TYPES
         SET DESCRIPTION = v_P_DESCRIPTION,
             UPDATED_BY = v_P_UPDATED_BY,
             UPDATED_DATE = v_P_UPDATED_DATE
         WHERE NAV_TYPE_SEQ_ID = v_P_NAV_TYPE_SEQ_ID;

      v_P_PRIMARY_KEY := v_P_NAV_TYPE_SEQ_ID;

   END;
   ELSE
   DECLARE
      v_temp NUMBER(1, 0) := 0;
   BEGIN-- INSERT a new row in the table.
   
      BEGIN
         SELECT 1 INTO v_temp
           FROM DUAL
          WHERE EXISTS ( SELECT v_P_DESCRIPTION
                         FROM ZFC_NAVIGATION_TYPES
                            WHERE DESCRIPTION = v_P_DESCRIPTION );
      EXCEPTION
         WHEN OTHERS THEN
            NULL;
      END;

      -- CHECK FOR DUPLICATE NAME BEFORE INSERTING
      IF v_temp = 1 THEN
      BEGIN
         raise_application_error( -20002, 'THE NAVIGATION TYPE YOU ENTERED ALREADY EXISTS IN THE DATABASE.' );

         RETURN;

      END;
      END IF;

      INSERT INTO ZFC_NAVIGATION_TYPES
        ( DESCRIPTION, ADDED_BY, ADDED_DATE, UPDATED_BY, UPDATED_DATE )
        VALUES ( v_P_DESCRIPTION, v_P_ADDED_BY, v_P_ADDED_DATE, v_P_ADDED_BY, v_P_ADDED_DATE );

      v_P_PRIMARY_KEY := sqlserver_utilities.identity;-- Get the IDENTITY value for the row just inserted.
      

   END;
   END IF;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_PERMISSIONS
(
  iv_P_PERMISSIONS_ID IN NUMBER DEFAULT NULL ,
  v_P_DESCRIPTION IN VARCHAR2 DEFAULT NULL ,
  v_P_ADDED_BY IN NUMBER DEFAULT NULL ,
  v_P_ADDED_DATE IN DATE DEFAULT NULL ,
  v_P_UPDATED_BY IN NUMBER DEFAULT NULL ,
  v_P_UPDATED_DATE IN DATE DEFAULT NULL ,
  v_P_PRIMARY_KEY OUT NUMBER,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_P_PERMISSIONS_ID NUMBER(10,0) := iv_P_PERMISSIONS_ID;
   v_sys_error NUMBER := 0;
BEGIN

   IF v_P_PERMISSIONS_ID > -1 THEN
   BEGIN-- UPDATE PROFILE
   
      UPDATE ZFC_PERMISSIONS
         SET DESCRIPTION = v_P_DESCRIPTION,
             ADDED_BY = v_P_ADDED_BY,
             ADDED_DATE = v_P_ADDED_DATE,
             UPDATED_BY = v_P_UPDATED_BY,
             UPDATED_DATE = v_P_UPDATED_DATE
         WHERE PERMISSIONS_ID = v_P_PERMISSIONS_ID;

      v_P_PRIMARY_KEY := v_P_PERMISSIONS_ID;

   END;
   ELSE
   DECLARE
      v_temp NUMBER(1, 0) := 0;
   BEGIN-- INSERT a new row in the table.
   
      BEGIN
         SELECT 1 INTO v_temp
           FROM DUAL
          WHERE EXISTS ( SELECT PERMISSIONS_ID
                         FROM ZFC_PERMISSIONS
                            WHERE PERMISSIONS_ID = v_P_PERMISSIONS_ID );
      EXCEPTION
         WHEN OTHERS THEN
            NULL;
      END;

      -- CHECK FOR DUPLICATE NAME BEFORE INSERTING
      IF v_temp = 1 THEN
      BEGIN
         raise_application_error( -20002, 'THE PERMISSION YOU ENTERED ALREADY EXISTS IN THE DATABASE.' );

         RETURN;

      END;
      END IF;

      INSERT INTO ZFC_PERMISSIONS
        ( DESCRIPTION, ADDED_BY, ADDED_DATE, UPDATED_BY, UPDATED_DATE )
        VALUES ( v_P_DESCRIPTION, v_P_ADDED_BY, v_P_ADDED_DATE, v_P_UPDATED_BY, v_P_UPDATED_DATE );

      v_P_PERMISSIONS_ID := sqlserver_utilities.identity;-- Get the IDENTITY value for the row just inserted.
      

   END;
   END IF;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_SECURITY_ENTITIES
(
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_NAME IN VARCHAR2 DEFAULT NULL ,
  v_P_DESCRIPTION IN VARCHAR2 DEFAULT NULL ,
  v_P_URL IN VARCHAR2 DEFAULT NULL ,
  v_P_STATUS_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_DAL IN VARCHAR2 DEFAULT NULL ,
  v_P_DAL_NAME IN VARCHAR2 DEFAULT NULL ,
  v_P_DAL_NAME_SPACE IN VARCHAR2 DEFAULT NULL ,
  v_P_DAL_STRING IN VARCHAR2 DEFAULT NULL ,
  v_P_SKIN IN CHAR DEFAULT NULL ,
  v_P_STYLE IN VARCHAR2 DEFAULT NULL ,
  v_P_ENCRYPTION_TYPE IN NUMBER DEFAULT NULL ,
  v_P_PARENT_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_PRIMARY_KEY OUT NUMBER,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   IF v_P_PARENT_SE_SEQ_ID = v_P_SE_SEQ_ID
     OR v_P_PARENT_SE_SEQ_ID = -1 THEN
      v_P_PARENT_SE_SEQ_ID := NULL;

   END IF;

   IF v_P_SE_SEQ_ID > -1 THEN
   BEGIN
      UPDATE ZFC_SECURITY_ENTITIES
         SET NAME = v_P_NAME,
             DESCRIPTION = v_P_DESCRIPTION,
             URL = v_P_URL,
             STATUS_SEQ_ID = v_P_STATUS_SEQ_ID,
             DAL = v_P_DAL,
             DAL_NAME = v_P_DAL_NAME,
             DAL_NAME_SPACE = v_P_DAL_NAME_SPACE,
             DAL_STRING = v_P_DAL_STRING,
             SKIN = v_P_SKIN,
             STYLE = v_P_STYLE,
             ENCRYPTION_TYPE = v_P_ENCRYPTION_TYPE,
             PARENT_SE_SEQ_ID = v_P_PARENT_SE_SEQ_ID
         WHERE SE_SEQ_ID = v_P_SE_SEQ_ID;

      v_P_PRIMARY_KEY := v_P_SE_SEQ_ID;

   END;
   ELSE
   BEGIN
      -- INSERT a new row in the table.
      INSERT INTO ZFC_SECURITY_ENTITIES
        ( NAME, DESCRIPTION, URL, STATUS_SEQ_ID, DAL, DAL_NAME, DAL_NAME_SPACE, DAL_STRING, SKIN, STYLE, ENCRYPTION_TYPE, PARENT_SE_SEQ_ID )
        VALUES ( v_P_NAME, v_P_DESCRIPTION, v_P_URL, v_P_STATUS_SEQ_ID, v_P_DAL, v_P_DAL_NAME, v_P_DAL_NAME_SPACE, v_P_DAL_STRING, v_P_SKIN, v_P_STYLE, v_P_ENCRYPTION_TYPE, v_P_PARENT_SE_SEQ_ID );

      -- Get the IDENTITY value for the row just inserted.
      v_P_PRIMARY_KEY := sqlserver_utilities.identity;

   END;
   END IF;

   -- End if
   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_ROLE
(
  v_P_ROLE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_NAME IN VARCHAR2 DEFAULT NULL ,
  v_P_DESCRIPTION IN VARCHAR2 DEFAULT NULL ,
  v_P_IS_SYSTEM IN NUMBER DEFAULT NULL ,
  v_P_IS_SYSTEM_ONLY IN NUMBER DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ADDED_BY IN NUMBER DEFAULT NULL ,
  v_P_ADDED_DATE IN DATE DEFAULT NULL ,
  v_P_UPDATED_BY IN NUMBER DEFAULT NULL ,
  v_P_UPDATED_DATE IN DATE DEFAULT NULL ,
  v_P_PRIMARY_KEY OUT NUMBER,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
   v_RLS_SEQ_ID NUMBER(10,0);
   v_MYMSG VARCHAR2(128);
   v_temp NUMBER(1, 0) := 0;
BEGIN

   BEGIN
      SELECT 1 INTO v_temp
        FROM DUAL
       WHERE ( SELECT COUNT(*)
               FROM ZFC_SECURITY_RLS
                  WHERE IS_SYSTEM_ONLY = 1
                          AND NAME = v_P_NAME ) > 0;
   EXCEPTION
      WHEN OTHERS THEN
         NULL;
   END;

   IF v_temp = 1 THEN
   BEGIN
      v_MYMSG := 'THE ROLE YOU ENTERED ' || v_P_NAME || ' IS FOR SYSTEM USE ONLY.';

      raise_application_error( -20002, || ':' ||v_MYMSG );

      RETURN;

   END;
   END IF;

   IF v_P_ROLE_SEQ_ID > -1 THEN
   BEGIN-- UPDATE PROFILE
   
      UPDATE ZFC_SECURITY_RLS
         SET NAME = v_P_NAME,
             DESCRIPTION = v_P_DESCRIPTION,
             IS_SYSTEM = v_P_IS_SYSTEM,
             IS_SYSTEM_ONLY = v_P_IS_SYSTEM_ONLY,
             UPDATED_BY = v_P_UPDATED_BY,
             UPDATED_DATE = v_P_UPDATED_DATE
         WHERE ROLE_SEQ_ID = v_P_ROLE_SEQ_ID;

      v_P_PRIMARY_KEY := v_P_ROLE_SEQ_ID;

   END;
   ELSE
   DECLARE
      v_temp NUMBER(1, 0) := 0;
   BEGIN-- INSERT a new row in the table.
   
      BEGIN
         SELECT 1 INTO v_temp
           FROM DUAL
          WHERE NOT EXISTS ( SELECT NAME
                             FROM ZFC_SECURITY_RLS
                                WHERE NAME = v_P_NAME );
      EXCEPTION
         WHEN OTHERS THEN
            NULL;
      END;

      -- CHECK FOR DUPLICATE NAME BEFORE INSERTING
      IF v_temp = 1 THEN
      BEGIN
         INSERT INTO ZFC_SECURITY_RLS
           ( NAME, DESCRIPTION, IS_SYSTEM, IS_SYSTEM_ONLY, ADDED_BY, ADDED_DATE, UPDATED_BY, UPDATED_DATE )
           VALUES ( v_P_NAME, v_P_DESCRIPTION, v_P_IS_SYSTEM, v_P_IS_SYSTEM_ONLY, v_P_ADDED_BY, v_P_ADDED_DATE, v_P_ADDED_BY, v_P_ADDED_DATE );

         v_P_PRIMARY_KEY := sqlserver_utilities.identity;-- Get the IDENTITY value for the row just inserted.
         

      END;
      ELSE
         SELECT ROLE_SEQ_ID
           INTO v_P_PRIMARY_KEY
           FROM ZFC_SECURITY_RLS
            WHERE NAME = v_P_NAME;

      END IF;

   END;
   END IF;

   BEGIN
      SELECT 1 INTO v_temp
        FROM DUAL
       WHERE ( SELECT COUNT(*)
               FROM ZFC_SECURITY_RLS_SE
                  WHERE SE_SEQ_ID = v_P_SE_SEQ_ID
                          AND ROLE_SEQ_ID = v_P_PRIMARY_KEY ) = 0;
   EXCEPTION
      WHEN OTHERS THEN
         NULL;
   END;

   -- END IF
   -- END IF
   IF v_temp = 1 THEN
   BEGIN-- ADD ROLE REFERENCE TO SE_SECURITY
   
      INSERT INTO ZFC_SECURITY_RLS_SE
        ( SE_SEQ_ID, ROLE_SEQ_ID )
        VALUES ( v_P_SE_SEQ_ID, v_P_PRIMARY_KEY );

   END;
   END IF;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_GROUP
(
  v_P_GROUP_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_NAME IN VARCHAR2 DEFAULT NULL ,
  v_P_DESCRIPTION IN VARCHAR2 DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ADDED_BY IN NUMBER DEFAULT NULL ,
  v_P_ADDED_DATE IN DATE DEFAULT NULL ,
  v_P_UPDATED_BY IN NUMBER DEFAULT NULL ,
  v_P_UPDATED_DATE IN DATE DEFAULT NULL ,
  v_P_PRIMARY_KEY OUT NUMBER,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
   v_RLS_SEQ_ID NUMBER(10,0);
   v_MYMSG VARCHAR2(128);
   v_now DATE;
   v_temp NUMBER(1, 0) := 0;
BEGIN

   v_now := SYSDATE;

   IF v_P_GROUP_SEQ_ID > -1 THEN
   BEGIN-- UPDATE PROFILE
   
      UPDATE ZFC_SECURITY_GRPS
         SET NAME = v_P_NAME,
             DESCRIPTION = v_P_DESCRIPTION,
             UPDATED_BY = v_P_UPDATED_BY,
             UPDATED_DATE = v_P_UPDATED_DATE
         WHERE GROUP_SEQ_ID = v_P_GROUP_SEQ_ID;

      v_P_PRIMARY_KEY := v_P_GROUP_SEQ_ID;

   END;
   ELSE
   DECLARE
      v_temp NUMBER(1, 0) := 0;
   BEGIN-- INSERT a new row in the table.
   
      BEGIN
         SELECT 1 INTO v_temp
           FROM DUAL
          WHERE NOT EXISTS ( SELECT NAME
                             FROM ZFC_SECURITY_GRPS
                                WHERE NAME = v_P_NAME );
      EXCEPTION
         WHEN OTHERS THEN
            NULL;
      END;

      -- CHECK FOR DUPLICATE NAME BEFORE INSERTING
      IF v_temp = 1 THEN
      BEGIN
         INSERT INTO ZFC_SECURITY_GRPS
           ( NAME, DESCRIPTION, ADDED_BY, ADDED_DATE, UPDATED_BY, UPDATED_DATE )
           VALUES ( v_P_NAME, v_P_DESCRIPTION, v_P_ADDED_BY, v_P_ADDED_DATE, v_P_ADDED_BY, v_P_ADDED_DATE );

         v_P_PRIMARY_KEY := sqlserver_utilities.identity;-- Get the IDENTITY value for the row just inserted.
         

      END;
      ELSE
         --PRINT 'ENTERING SECURITY INFORMATION FOR THE GROUP'
         SELECT GROUP_SEQ_ID
           INTO v_P_PRIMARY_KEY
           FROM ZFC_SECURITY_GRPS
            WHERE NAME = v_P_NAME;

      END IF;

   END;
   END IF;

   BEGIN
      SELECT 1 INTO v_temp
        FROM DUAL
       WHERE ( SELECT COUNT(*)
               FROM ZFC_SECURITY_GRPS_SE
                  WHERE SE_SEQ_ID = v_P_SE_SEQ_ID
                          AND GROUP_SEQ_ID = v_P_PRIMARY_KEY ) = 0;
   EXCEPTION
      WHEN OTHERS THEN
         NULL;
   END;

   -- END IF
   -- END IF
   IF v_temp = 1 THEN
   BEGIN-- ADD GROUP REFERENCE TO SE_SECURITY
   
      INSERT INTO ZFC_SECURITY_GRPS_SE
        ( SE_SEQ_ID, GROUP_SEQ_ID, ADDED_BY, ADDED_DATE )
        VALUES ( v_P_SE_SEQ_ID, v_P_PRIMARY_KEY, v_P_ADDED_BY, v_P_ADDED_DATE );

   END;
   END IF;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_GROUP_RLS
(
  v_P_GROUP_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ROLES IN NVARCHAR2 DEFAULT NULL ,
  v_P_ADD_UP_BY IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
   v_V_RLS_SE_SEQ_ID NUMBER(10,0);
   v_V_ROLE_NAME VARCHAR2(50);
   v_V_IS_SYSTEM NUMBER(10,0);
   v_V_POS NUMBER(10,0);
   v_V_GRPS_SE_SEQ_ID NUMBER(10,0);
BEGIN

   --NEED TO DELETE EXISTING ROLES ASSOCITAED BEFORE
   -- INSERTING NEW ONES. EXECUTION OF THIS STORED PROC
   -- IS MOVED FROM CODE
   ZFP_DEL_GROUP_ROLES(v_P_GROUP_SEQ_ID,
                             v_P_SE_SEQ_ID,
                             v_P_ADD_UP_BY);

   v_P_ROLES := LTRIM(RTRIM(v_P_ROLES)) || ',';

   v_V_POS := INSTR(v_P_ROLES, ',', 1);

   IF REPLACE(v_P_ROLES, ',', '') <> '' THEN
   BEGIN
      WHILE v_V_POS > 0
      LOOP
         BEGIN
            v_V_ROLE_NAME := LTRIM(RTRIM(SUBSTR(v_P_ROLES, 0, v_V_POS - 1)));

            IF v_V_ROLE_NAME <> '' THEN
            --print @V_ROLE_NAME -- DEBUG
            BEGIN
               --SELECT THE ROLE_SEQ_ID FROM THE ROLES
               --TABLE FOR ALL THE ROLES PASSED
               SELECT RLS_SE_SEQ_ID
                 INTO v_V_RLS_SE_SEQ_ID
                 FROM ZFC_SECURITY_RLS_SE
                  WHERE ROLE_SEQ_ID = ( SELECT ROLE_SEQ_ID
                                    FROM ZFC_SECURITY_RLS
                                       WHERE NAME = v_V_ROLE_NAME )
                          AND SE_SEQ_ID = v_P_SE_SEQ_ID;

               -- print @V_RLS_SE_SEQ_ID -- DEBUG
               SELECT GRPS_SE_SEQ_ID
                 INTO v_V_GRPS_SE_SEQ_ID
                 FROM ZFC_SECURITY_GRPS_SE
                  WHERE SE_SEQ_ID = v_P_SE_SEQ_ID
                          AND GROUP_SEQ_ID = v_P_GROUP_SEQ_ID;

               --print @V_GRPS_SE_SEQ_ID -- DEBUG
               /*
					INSERT THE ZFC_SECURITY_GRPS_RLS
					WITH ROLES INFORMATION
					*/
               IF v_V_RLS_SE_SEQ_ID IS NOT NULL THEN
               BEGIN
                  INSERT INTO ZFC_SECURITY_GRPS_RLS
                    ( RLS_SE_SEQ_ID, GRPS_SE_SEQ_ID, ADDED_BY, ADDED_DATE )
                    VALUES ( v_V_RLS_SE_SEQ_ID, v_V_GRPS_SE_SEQ_ID, v_P_ADD_UP_BY, SYSDATE );

               END;
               END IF;

            END;
            END IF;

            --PRINT '*****************INSERTED INTO ZFC_SECURITY_GRPS_RLS'
            v_P_ROLES := SUBSTR(v_P_ROLES, -1, LENGTH(v_P_ROLES) - v_V_POS);

            v_V_POS := INSTR(v_P_ROLES, ',', 1);

         END;
      END LOOP;

   END;
   END IF;

   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_ACCT_RLS
(
  v_P_ACCT IN VARCHAR2 DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ROLES IN VARCHAR2 DEFAULT NULL ,
  v_P_ADDUPD_BY IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
   v_ACCT_SEQ_ID NUMBER(10,0);
   v_V_ROLE_SEQ_ID NUMBER(10,0);
   v_V_SE_RLS_SECURITY_ID NUMBER(10,0);
   v_V_ROLE_NAME VARCHAR2(50);
   v_V_Pos NUMBER(10,0);
BEGIN

   SET TRANSACTION READ WRITE;

   SELECT ACCT_SEQ_ID
     INTO v_ACCT_SEQ_ID
     FROM ZFC_ACCTS
      WHERE ACCT = v_P_ACCT;

   -- NEED TO DELETE EXISTING ROLE ASSOCITAED WITH THE FUNCTION BEFORE
   -- INSERTING NEW ONES.
   ZFP_DEL_ACCT_RL_SECURITY(v_ACCT_SEQ_ID,
                                  v_P_SE_SEQ_ID,
                                  v_P_ADDUPD_BY,
                                  v_P_ErrorCode);

   v_P_ROLES := LTRIM(RTRIM(v_P_ROLES)) || ',';

   v_V_Pos := INSTR(v_P_ROLES, ',', 1);

   IF REPLACE(v_P_ROLES, ',', '') <> '' THEN
      WHILE v_V_Pos > 0
      LOOP
         BEGIN
            v_V_ROLE_NAME := LTRIM(RTRIM(SUBSTR(v_P_ROLES, 0, v_V_Pos - 1)));

            IF v_V_ROLE_NAME <> '' THEN
            DECLARE
               v_temp NUMBER(1, 0) := 0;
            BEGIN
               --select the role seq id first
               SELECT ROLE_SEQ_ID
                 INTO v_V_ROLE_SEQ_ID
                 FROM ZFC_SECURITY_RLS
                  WHERE NAME = v_V_ROLE_NAME;

               SELECT RLS_SE_SEQ_ID
                 INTO v_V_SE_RLS_SECURITY_ID
                 FROM ZFC_SECURITY_RLS_SE
                  WHERE ROLE_SEQ_ID = v_V_ROLE_SEQ_ID
                          AND SE_SEQ_ID = v_P_SE_SEQ_ID;

               BEGIN
                  SELECT 1 INTO v_temp
                    FROM DUAL
                   WHERE NOT EXISTS ( SELECT RLS_SE_SEQ_ID
                                      FROM ZFC_SECURITY_ACCTS_RLS
                                         WHERE ACCT_SEQ_ID = v_ACCT_SEQ_ID
                                                 AND RLS_SE_SEQ_ID = v_V_SE_RLS_SECURITY_ID );
               EXCEPTION
                  WHEN OTHERS THEN
                     NULL;
               END;

               --PRINT('@V_SE_RLS_SECURITY_ID = ' + CONVERT(VARCHAR,@V_SE_RLS_SECURITY_ID))
               IF v_temp = 1 THEN
               BEGIN
                  --PRINT('INSERT RECORD')
                  INSERT INTO ZFC_SECURITY_ACCTS_RLS
                    ( ACCT_SEQ_ID, RLS_SE_SEQ_ID, ADDED_BY )
                    VALUES ( v_ACCT_SEQ_ID, v_V_SE_RLS_SECURITY_ID, v_P_ADDUPD_BY );

               END;
               END IF;

            END;
            END IF;

            v_P_ROLES := SUBSTR(v_P_ROLES, -1, LENGTH(v_P_ROLES) - v_V_Pos);

            v_V_Pos := INSTR(v_P_ROLES, ',', 1);

            v_P_ErrorCode := v_sys_error;

         END;
      END LOOP;

   END IF;

   IF v_P_ErrorCode <> 0 THEN
      GOTO ABEND;

   END IF;

   COMMIT;

   <<ABEND>>

   IF v_P_ErrorCode <> 0 THEN
   BEGIN
      DBMS_OUTPUT.PUT_LINE('Yikes!');

      ROLLBACK;

   END;
   END IF;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_FUNCTION
(
  v_P_FUNCTION_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_NAME IN VARCHAR2 DEFAULT NULL ,
  v_P_DESCRIPTION IN VARCHAR2 DEFAULT NULL ,
  v_P_FUNCTION_TYPE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ALLOW_HTML_INPUT IN NUMBER DEFAULT NULL ,
  v_P_ALLOW_COMMENT_HTML_INPUT IN NUMBER DEFAULT NULL ,
  v_P_SOURCE IN VARCHAR2 DEFAULT NULL ,
  v_P_ENABLE_VIEW_STATE IN NUMBER DEFAULT NULL ,
  v_P_IS_NAV IN NUMBER DEFAULT NULL ,
  v_P_NAV_TYPE_ID IN NUMBER DEFAULT NULL ,
  v_P_ACTION IN VARCHAR2 DEFAULT NULL ,
  v_P_PARENT_FUNCTION_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_NOTES IN VARCHAR2 DEFAULT NULL ,
  v_P_ADDED_BY IN NUMBER DEFAULT NULL ,
  v_P_ADDED_DATE IN DATE DEFAULT NULL ,
  v_P_UPDATED_BY IN NUMBER DEFAULT NULL ,
  v_P_UPDATED_DATE IN DATE DEFAULT NULL ,
  v_P_PRIMARY_KEY OUT NUMBER,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   IF v_P_FUNCTION_SEQ_ID > -1 THEN
   BEGIN-- UPDATE PROFILE
   
      UPDATE ZFC_FUNCTIONS
         SET NAME = v_P_NAME,
             DESCRIPTION = v_P_DESCRIPTION,
             FUNCTION_TYPE_SEQ_ID = v_P_FUNCTION_TYPE_SEQ_ID,
             ALLOW_HTML_INPUT = v_P_ALLOW_HTML_INPUT,
             ALLOW_COMMENT_HTML_INPUT = v_P_ALLOW_COMMENT_HTML_INPUT,
             SOURCE = v_P_SOURCE,
             ENABLE_VIEW_STATE = v_P_ENABLE_VIEW_STATE,
             IS_NAV = v_P_IS_NAV,
             NAV_TYPE_ID = v_P_NAV_TYPE_ID,
             --[ACTION] = @P_ACTION,
             PARENT_FUNCTION_SEQ_ID = v_P_PARENT_FUNCTION_SEQ_ID,
             NOTES = v_P_NOTES,
             UPDATED_BY = v_P_UPDATED_BY,
             UPDATED_DATE = v_P_UPDATED_DATE
         WHERE FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID;

      v_P_PRIMARY_KEY := v_P_FUNCTION_SEQ_ID;

   END;
   ELSE
   DECLARE
      v_temp NUMBER(1, 0) := 0;
      v_V_SORT_ORDER NUMBER(10,0);
   BEGIN-- INSERT a new row in the table.
   
      BEGIN
         SELECT 1 INTO v_temp
           FROM DUAL
          WHERE EXISTS ( SELECT ACTION
                         FROM ZFC_FUNCTIONS
                            WHERE ACTION = v_P_ACTION );
      EXCEPTION
         WHEN OTHERS THEN
            NULL;
      END;

      -- CHECK FOR DUPLICATE NAME BEFORE INSERTING
      IF v_temp = 1 THEN
      BEGIN
         raise_application_error( -20002, 'THE FUNCTION YOU ENTERED ALREADY EXISTS IN THE DATABASE.' );

         RETURN;

      END;
      END IF;

      INSERT INTO ZFC_FUNCTIONS
        ( NAME, DESCRIPTION, FUNCTION_TYPE_SEQ_ID, ALLOW_HTML_INPUT, ALLOW_COMMENT_HTML_INPUT, SOURCE, ENABLE_VIEW_STATE, IS_NAV, NAV_TYPE_ID, ACTION, PARENT_FUNCTION_SEQ_ID, NOTES, ADDED_BY, ADDED_DATE, UPDATED_BY, UPDATED_DATE )
        VALUES ( v_P_NAME, v_P_DESCRIPTION, v_P_FUNCTION_TYPE_SEQ_ID, v_P_ALLOW_HTML_INPUT, v_P_ALLOW_COMMENT_HTML_INPUT, v_P_SOURCE, v_P_ENABLE_VIEW_STATE, v_P_IS_NAV, v_P_NAV_TYPE_ID, v_P_ACTION, v_P_PARENT_FUNCTION_SEQ_ID, v_P_NOTES, v_P_ADDED_BY, v_P_ADDED_DATE, v_P_ADDED_BY, v_P_ADDED_DATE );

      v_P_PRIMARY_KEY := sqlserver_utilities.identity;-- Get the IDENTITY value for the row just inserted.
      

      SELECT ( SELECT MAX(SORT_ORDER)
               FROM ZFC_FUNCTIONS
                  WHERE PARENT_FUNCTION_SEQ_ID = v_P_PARENT_FUNCTION_SEQ_ID ) + 1
        INTO v_V_SORT_ORDER
        FROM DUAL ;

      UPDATE ZFC_FUNCTIONS
         SET SORT_ORDER = v_V_SORT_ORDER
         WHERE FUNCTION_SEQ_ID = v_P_PRIMARY_KEY;

   END;
   END IF;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_FUNCTION_RLS
(
  v_P_FUNCTION_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ROLES IN VARCHAR2 DEFAULT NULL ,
  v_P_PERMISSIONS_ID IN NUMBER DEFAULT NULL ,
  v_P_ADDUPD_BY IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
   v_V_ROLE_SEQ_ID NUMBER(10,0);
   v_V_SE_RLS_SECURITY_ID NUMBER(10,0);
   v_V_ROLE_NAME VARCHAR2(50);
   v_V_Pos NUMBER(10,0);
BEGIN

   SET TRANSACTION READ WRITE;

   -- NEED TO DELETE EXISTING ROLE ASSOCITAED WITH THE FUNCTION BEFORE
   -- INSERTING NEW ONES.
   ZFP_DEL_FUNCTION_RL_SECURITY(v_P_FUNCTION_SEQ_ID,
                                      v_P_SE_SEQ_ID,
                                      v_P_PERMISSIONS_ID,
                                      v_P_ADDUPD_BY,
                                      v_P_ErrorCode);

   v_P_ROLES := LTRIM(RTRIM(v_P_ROLES)) || ',';

   v_V_Pos := INSTR(v_P_ROLES, ',', 1);

   IF REPLACE(v_P_ROLES, ',', '') <> '' THEN
      WHILE v_V_Pos > 0
      LOOP
         BEGIN
            v_V_ROLE_NAME := LTRIM(RTRIM(SUBSTR(v_P_ROLES, 0, v_V_Pos - 1)));

            IF v_V_ROLE_NAME <> '' THEN
            DECLARE
               v_temp NUMBER(1, 0) := 0;
            BEGIN
               --select the role seq id first
               SELECT ROLE_SEQ_ID
                 INTO v_V_ROLE_SEQ_ID
                 FROM ZFC_SECURITY_RLS
                  WHERE NAME = v_V_ROLE_NAME;

               SELECT RLS_SE_SEQ_ID
                 INTO v_V_SE_RLS_SECURITY_ID
                 FROM ZFC_SECURITY_RLS_SE
                  WHERE ROLE_SEQ_ID = v_V_ROLE_SEQ_ID
                          AND SE_SEQ_ID = v_P_SE_SEQ_ID;

               BEGIN
                  SELECT 1 INTO v_temp
                    FROM DUAL
                   WHERE NOT EXISTS ( SELECT RLS_SE_SEQ_ID
                                      FROM ZFC_SECURITY_FUNCT_RLS
                                         WHERE FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID
                                                 AND PERMISSIONS_ID = v_P_PERMISSIONS_ID
                                                 AND RLS_SE_SEQ_ID = v_V_SE_RLS_SECURITY_ID );
               EXCEPTION
                  WHEN OTHERS THEN
                     NULL;
               END;

               --PRINT('@V_SE_RLS_SECURITY_ID = ' + CONVERT(VARCHAR,@V_SE_RLS_SECURITY_ID))
               IF v_temp = 1 THEN
               BEGIN
                  --PRINT('INSERT RECORD')
                  INSERT INTO ZFC_SECURITY_FUNCT_RLS
                    ( FUNCTION_SEQ_ID, RLS_SE_SEQ_ID, PERMISSIONS_ID, ADDED_BY )
                    VALUES ( v_P_FUNCTION_SEQ_ID, v_V_SE_RLS_SECURITY_ID, v_P_PERMISSIONS_ID, v_P_ADDUPD_BY );

               END;
               END IF;

            END;
            END IF;

            v_P_ROLES := SUBSTR(v_P_ROLES, -1, LENGTH(v_P_ROLES) - v_V_Pos);

            v_V_Pos := INSTR(v_P_ROLES, ',', 1);

            v_P_ErrorCode := v_sys_error;

         END;
      END LOOP;

   END IF;

   v_P_ErrorCode := v_sys_error;

   IF v_P_ErrorCode <> 0 THEN
      GOTO ABEND;

   END IF;

   COMMIT;

   <<ABEND>>

   IF v_P_ErrorCode <> 0 THEN
   BEGIN
      DBMS_OUTPUT.PUT_LINE('Yikes!');

      ROLLBACK;

   END;
   END IF;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_DIRECTORY
(
  v_P_FUNCTION_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_DIRECTORY IN VARCHAR2 DEFAULT NULL ,
  v_P_IMPERSONATE IN NUMBER DEFAULT NULL ,
  v_P_IMPERSONATE_ACCOUNT IN VARCHAR2 DEFAULT NULL ,
  v_P_IMPERSONATE_PWD IN VARCHAR2 DEFAULT NULL ,
  v_P_ADDED_BY IN NUMBER DEFAULT NULL ,
  v_P_ADDED_DATE IN DATE DEFAULT NULL ,
  v_P_UPDATED_BY IN NUMBER DEFAULT NULL ,
  v_P_UPDATED_DATE IN DATE DEFAULT NULL ,
  v_P_PRIMARY_KEY OUT NUMBER,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
   v_temp NUMBER(1, 0) := 0;
BEGIN

   BEGIN
      SELECT 1 INTO v_temp
        FROM DUAL
       WHERE ( SELECT COUNT(*)
               FROM ZFO_DIRECTORIES
                  WHERE FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID ) = 0;
   EXCEPTION
      WHEN OTHERS THEN
         NULL;
   END;

   IF v_temp = 1 THEN
   BEGIN
      INSERT INTO ZFO_DIRECTORIES
        ( FUNCTION_SEQ_ID, DIRECTORY, IMPERSONATE, IMPERSONATE_ACCOUNT, IMPERSONATE_PWD, ADDED_BY, ADDED_DATE, UPDATED_BY, UPDATED_DATE )
        VALUES ( v_P_FUNCTION_SEQ_ID, v_P_DIRECTORY, v_P_IMPERSONATE, v_P_IMPERSONATE_ACCOUNT, v_P_IMPERSONATE_PWD, v_P_ADDED_BY, v_P_ADDED_DATE, v_P_UPDATED_BY, v_P_UPDATED_DATE );

      v_P_PRIMARY_KEY := v_P_FUNCTION_SEQ_ID;

   END;
   ELSE
   BEGIN
      UPDATE ZFO_DIRECTORIES
         SET FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID,
             DIRECTORY = v_P_DIRECTORY,
             IMPERSONATE = v_P_IMPERSONATE,
             IMPERSONATE_ACCOUNT = v_P_IMPERSONATE_ACCOUNT,
             IMPERSONATE_PWD = v_P_IMPERSONATE_PWD,
             UPDATED_BY = v_P_UPDATED_BY,
             UPDATED_DATE = v_P_UPDATED_DATE
         WHERE FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID;

      v_P_PRIMARY_KEY := v_P_FUNCTION_SEQ_ID;

   END;
   END IF;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_MESSAGE
(
  v_P_MESSAGE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_NAME IN VARCHAR2 DEFAULT NULL ,
  v_P_TITLE IN VARCHAR2 DEFAULT NULL ,
  v_P_DESCRIPTION IN VARCHAR2 DEFAULT NULL ,
  v_P_BODY IN NCLOB DEFAULT NULL ,
  v_P_FORMAT_AS_HTML IN NUMBER DEFAULT NULL ,
  v_P_ADDED_BY IN NUMBER DEFAULT NULL ,
  v_P_ADDED_DATE IN DATE DEFAULT NULL ,
  v_P_UPDATED_BY IN NUMBER DEFAULT NULL ,
  v_P_UPDATED_DATE IN DATE DEFAULT NULL ,
  v_P_PRIMARY_KEY OUT NUMBER,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
   v_MYMSG VARCHAR2(128);
BEGIN

   IF v_P_MESSAGE_SEQ_ID > -1 THEN
   BEGIN-- UPDATE PROFILE
   
      UPDATE ZFO_MESSAGES
         SET SE_SEQ_ID = v_P_SE_SEQ_ID,
             NAME = v_P_NAME,
             TITLE = v_P_TITLE,
             DESCRIPTION = v_P_DESCRIPTION,
             FORMAT_AS_HTML = v_P_FORMAT_AS_HTML,
             BODY = v_P_BODY
         WHERE MESSAGE_SEQ_ID = v_P_MESSAGE_SEQ_ID;

      v_P_PRIMARY_KEY := v_P_MESSAGE_SEQ_ID;-- set the output id just in case.
      

   END;
   ELSE
   DECLARE
      v_temp NUMBER(1, 0) := 0;
   BEGIN-- INSERT a new row in the table.
   
      BEGIN
         SELECT 1 INTO v_temp
           FROM DUAL
          WHERE EXISTS ( SELECT NAME
                         FROM ZFO_MESSAGES
                            WHERE NAME = v_P_NAME
                                    AND SE_SEQ_ID = v_P_SE_SEQ_ID );
      EXCEPTION
         WHEN OTHERS THEN
            NULL;
      END;

      -- CHECK FOR DUPLICATE NAME BEFORE INSERTING
      IF v_temp = 1 THEN
      BEGIN
         raise_application_error( -20002, 'THE MESSAGE YOU ENTERED ALREADY EXISTS IN THE DATABASE.' );

         RETURN;

      END;
      END IF;

      INSERT INTO ZFO_MESSAGES
        ( SE_SEQ_ID, NAME, TITLE, DESCRIPTION, BODY, FORMAT_AS_HTML, ADDED_BY, ADDED_DATE, UPDATED_BY, UPDATED_DATE )
        VALUES ( v_P_SE_SEQ_ID, v_P_NAME, v_P_TITLE, v_P_DESCRIPTION, v_P_BODY, v_P_FORMAT_AS_HTML, v_P_ADDED_BY, v_P_ADDED_DATE, v_P_ADDED_BY, v_P_ADDED_DATE );

      v_P_PRIMARY_KEY := sqlserver_utilities.identity;-- Get the IDENTITY value for the row just inserted.
      

   END;
   END IF;

   -- END IF
   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_POP_BASE_DATA
(
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
   -- SELECT all rows from the table.
   v_now DATE;
   /* */
   v_V_DeveloperID NUMBER(10,0);
   v_V_SE_SEQ_ID NUMBER(10,0);
   v_V_EVERYONE_ID NUMBER(10,0);
   v_V_MyAction VARCHAR2(256);
   v_V_FUNCTION_TYPE NUMBER(10,0);
   v_V_EnableViewStateTrue NUMBER(10,0);
   v_V_EnableViewStateFalse NUMBER(10,0);
   v_V_IsNavTrue NUMBER(10,0);
   v_V_IsNavFalse NUMBER(10,0);
   v_V_NAV_TYPE NUMBER(10,0);
   v_V_ParentID NUMBER(10,0);
   v_V_FunctionID NUMBER(10,0);
   v_V_ViewPermission NUMBER(10,0);
   v_V_AddPermission NUMBER(10,0);
   v_V_EditPermission NUMBER(10,0);
   v_V_DeletePermission NUMBER(10,0);
   v_V_NAV_TYPE_Hierarchical NUMBER(10,0);
   v_V_NAV_TYPE_Vertical NUMBER(10,0);
   v_V_NAV_TYPE_Horizontal NUMBER(10,0);
   v_V_CHANGE_PASSWORD NUMBER(10,0);
   v_V_INACTIVE NUMBER(10,0);
   v_V_ACTIVE NUMBER(10,0);
   v_V_ALLOW_HTML_INPUT NUMBER(10,0);
   v_V_ALLOW_COMMENT_HTML_INPUT NUMBER(10,0);
   v_V_IS_CONTENT NUMBER(10,0);
   v_V_FORMAT_AS_HTML_TRUE NUMBER(10,0);
   v_V_FORMAT_AS_HTML_FALSE NUMBER(10,0);
   v_V_PRIMARY_KEY NUMBER(10,0);
   v_V_ErrorCode NUMBER(10,0);
   v_V_FUNCTION_SEQ_ID NUMBER(10,0);
   v_V_ENCRYPTION_TYPE NUMBER(10,0);
   v_V_ENABLE_INHERITANCE NUMBER(10,0);
BEGIN

   v_now := SYSDATE;

   DECLARE
      v_ZFP_SET_INFORMATION_param VARCHAR2(256);
      v_ZFP_SET_INFORMATION_param_1 VARCHAR2(256);
      v_ZFP_SET_GROUP_RLS_param VARCHAR2(256);
   BEGIN
      v_V_FORMAT_AS_HTML_TRUE := 1;-- FALSE
      

      v_V_FORMAT_AS_HTML_FALSE := 0;-- FALSE
      

      v_V_ALLOW_HTML_INPUT := 0;-- FALSE
      

      v_V_ALLOW_COMMENT_HTML_INPUT := 0;-- FALSE
      

      v_V_IS_CONTENT := 0;-- FALSE
      

      v_V_PRIMARY_KEY := NULL;-- Not needed when setup up the database
      

      v_V_ErrorCode := NULL;-- Not needed when setup up the database
      

      v_V_ENCRYPTION_TYPE := 1;-- TripleDES
      

      v_V_ENABLE_INHERITANCE := 1;-- 0 = FALSE 1 = TRUE
      

      -- Setup ZFC_SYSTEM_STATUS
      DBMS_OUTPUT.PUT_LINE('Adding System Status');

      ZFP_SET_SYSTEM_STATUS(-1,
                            'Active',
                            1,
                            v_now,
                            1,
                            v_now,
                            v_V_PRIMARY_KEY,
                            v_V_ErrorCode);

      ZFP_SET_SYSTEM_STATUS(-1,
                            'Inactive',
                            1,
                            v_now,
                            1,
                            v_now,
                            v_V_PRIMARY_KEY,
                            v_V_ErrorCode);

      ZFP_SET_SYSTEM_STATUS(-1,
                            'Disabled',
                            1,
                            v_now,
                            1,
                            v_now,
                            v_V_PRIMARY_KEY,
                            v_V_ErrorCode);

      ZFP_SET_SYSTEM_STATUS(-1,
                            'ChangePassword',
                            1,
                            v_now,
                            1,
                            v_now,
                            v_V_PRIMARY_KEY,
                            v_V_ErrorCode);

      SELECT STATUS_SEQ_ID
        INTO v_V_CHANGE_PASSWORD
        FROM ZFC_SYSTEM_STATUS
         WHERE DESCRIPTION = 'ChangePassword';

      SELECT STATUS_SEQ_ID
        INTO v_V_INACTIVE
        FROM ZFC_SYSTEM_STATUS
         WHERE DESCRIPTION = 'Inactive';

      SELECT STATUS_SEQ_ID
        INTO v_V_ACTIVE
        FROM ZFC_SYSTEM_STATUS
         WHERE DESCRIPTION = 'Active';

      --
      DBMS_OUTPUT.PUT_LINE('Adding Accounts');

      -- Add the anonymous account
      ZFP_SET_ACCOUNT(-1,
                            1,
                            'Anonymous',
                            'Anonymous',
                            'Anonymous',
                            '',
                            'Anonymous-Account',
                            'me@me.com',
                            'none',
                            v_now,
                            0,
                            1,
                            v_now,
                            v_now,
                            -5,
                            'none',
                            0,
                            0,
                            0,
                            v_now,
                            v_V_PRIMARY_KEY,
                            v_V_ErrorCode);

      -- BEFORE ADDING ANY MORE ACCOUNTS SETUP ZF_ACCT_CHOICES
      ZFP_SET_ACCT_CHOICES('Anonymous',
                                 1,
                                 'All',
                                 '#ffffff',
                                 '#eeeeee',
                                 '#6699cc',
                                 '#b6cbeb',
                                 'Blue',
                                 'FavoriateAction',
                                 'ThinActions',
                                 'WideActions',
                                 5,
                                 '');

      -- Add the system administrator account
      ZFP_SET_ACCOUNT(-1,
                            v_V_CHANGE_PASSWORD,
                            'Developer',
                            'System',
                            'Developer',
                            '',
                            'System-Developer',
                            'michael.regan@verizon.net',
                            'none',
                            v_now,
                            0,
                            1,
                            v_now,
                            v_now,
                            -5,
                            'none',
                            0,
                            1,
                            1,
                            v_now,
                            v_V_PRIMARY_KEY,
                            v_V_ErrorCode);

      -- testing account
      ZFP_SET_ACCOUNT(-1,
                            v_V_CHANGE_PASSWORD,
                            'Mike',
                            'System',
                            'Tester',
                            '',
                            'System-Tester',
                            'michael.regan@verizon.net',
                            'none',
                            v_now,
                            0,
                            0,
                            v_now,
                            v_now,
                            -5,
                            'none',
                            0,
                            0,
                            1,
                            v_now,
                            v_V_PRIMARY_KEY,
                            v_V_ErrorCode);

      SELECT ACCT_SEQ_ID
        INTO v_V_DeveloperID
        FROM ZFC_ACCTS
         WHERE ACCT = 'Developer';

      DBMS_OUTPUT.PUT_LINE('Adding DB Information');

      v_ZFP_SET_INFORMATION_param := NULL;

      v_ZFP_SET_INFORMATION_param_1 := NULL;

      ZFP_SET_INFORMATION(-1,
                          '1.0',
                          v_V_ENABLE_INHERITANCE,
                          v_V_DeveloperID,
                          v_P_PRIMARY_KEY => v_ZFP_SET_INFORMATION_param,
                          v_P_ErrorCode => v_ZFP_SET_INFORMATION_param_1);

      DBMS_OUTPUT.PUT_LINE('Adding Function types');

      -- Setup ZFC_FUNCTION_TYPES
      ZFP_SET_FUNCTION_TYPES(-1,
                                   'Module',
                                   'used for modules',
                                   '',
                                   '0',
                                   v_V_DeveloperID,
                                   v_now,
                                   v_V_DeveloperID,
                                   v_now,
                                   v_V_PRIMARY_KEY,
                                   v_V_ErrorCode);

      ZFP_SET_FUNCTION_TYPES(-1,
                             'Security',
                             'used as a container for security.',
                             'none',
                             '0',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      ZFP_SET_FUNCTION_TYPES(-1,
                             'Menu Item',
                             'designates entry is a menu item.',
                             'none',
                             '0',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      ZFP_SET_FUNCTION_TYPES(-1,
                             'File Manager',
                             'Used for managing files and directories',
                             'Modules\System\FileManagement\FileManagerControl.ascx',
                             '0',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      ZFP_SET_FUNCTION_TYPES(-1,
                             'Articles',
                             'Contains articles that can represent news articles or other content',
                             'Modules\System\Content\ContentControl.ascx',
                             '1',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      ZFP_SET_FUNCTION_TYPES(-1,
                             'Links',
                             'Contains internal and external links',
                             'Modules\System\Content\ContentControl.ascx',
                             '1',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      ZFP_SET_FUNCTION_TYPES(-1,
                             'Downloads',
                             'Allows downloading of files',
                             'Modules\System\Content\ContentControl.ascx',
                             '1',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      ZFP_SET_FUNCTION_TYPES(-1,
                             'Photo Gallery',
                             'Enables you to display a gallery of images',
                             'Modules\System\Content\ContentControl.ascx',
                             '1',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      ZFP_SET_FUNCTION_TYPES(-1,
                             'Books',
                             'Contains book listings',
                             'Modules\System\Content\ContentControl.ascx',
                             '1',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      ZFP_SET_FUNCTION_TYPES(-1,
                             'Events',
                             'Contains event listings',
                             'Modules\System\Content\ContentControl.ascx',
                             '1',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      ZFP_SET_FUNCTION_TYPES(-1,
                             'HTML Page',
                             'Contains a single editable HTML page',
                             'Modules\System\Content\ContentControl.ascx',
                             '1',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      ZFP_SET_FUNCTION_TYPES(-1,
                             'Discuss',
                             'Contains a discussion area in which users can add posts',
                             'Modules\System\Content\ContentControl.ascx',
                             '1',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding navigation types');

      -- Setup ZFC_NAVIGATION_TYPES
      ZFP_SET_NAVIGATION_TYPES(-1,
                                     'Horizontal',
                                     v_V_DeveloperID,
                                     v_now,
                                     v_V_DeveloperID,
                                     v_now,
                                     v_V_PRIMARY_KEY,
                                     v_V_ErrorCode);

      ZFP_SET_NAVIGATION_TYPES(-1,
                               'Vertical',
                               v_V_DeveloperID,
                               v_now,
                               v_V_DeveloperID,
                               v_now,
                               v_V_PRIMARY_KEY,
                               v_V_ErrorCode);

      ZFP_SET_NAVIGATION_TYPES(-1,
                               'Hierarchical',
                               v_V_DeveloperID,
                               v_now,
                               v_V_DeveloperID,
                               v_now,
                               v_V_PRIMARY_KEY,
                               v_V_ErrorCode);

      SELECT NAV_TYPE_SEQ_ID
        INTO v_V_NAV_TYPE_Hierarchical
        FROM ZFC_NAVIGATION_TYPES
         WHERE DESCRIPTION = 'Hierarchical';

      SELECT NAV_TYPE_SEQ_ID
        INTO v_V_NAV_TYPE_Vertical
        FROM ZFC_NAVIGATION_TYPES
         WHERE DESCRIPTION = 'Vertical';

      SELECT NAV_TYPE_SEQ_ID
        INTO v_V_NAV_TYPE_Horizontal
        FROM ZFC_NAVIGATION_TYPES
         WHERE DESCRIPTION = 'Horizontal';

      DBMS_OUTPUT.PUT_LINE('Adding permissions');

      -- Setup ZFC_PERMISSIONS
      ZFP_SET_PERMISSIONS(-1,
                                'View',
                                v_V_DeveloperID,
                                v_now,
                                v_V_DeveloperID,
                                v_now,
                                v_V_PRIMARY_KEY,
                                v_V_ErrorCode);

      ZFP_SET_PERMISSIONS(-1,
                          'Edit',
                          v_V_DeveloperID,
                          v_now,
                          v_V_DeveloperID,
                          v_now,
                          v_V_PRIMARY_KEY,
                          v_V_ErrorCode);

      ZFP_SET_PERMISSIONS(-1,
                          'Add',
                          v_V_DeveloperID,
                          v_now,
                          v_V_DeveloperID,
                          v_now,
                          v_V_PRIMARY_KEY,
                          v_V_ErrorCode);

      ZFP_SET_PERMISSIONS(-1,
                          'Delete',
                          v_V_DeveloperID,
                          v_now,
                          v_V_DeveloperID,
                          v_now,
                          v_V_PRIMARY_KEY,
                          v_V_ErrorCode);

      SELECT PERMISSIONS_ID
        INTO v_V_ViewPermission
        FROM ZFC_PERMISSIONS
         WHERE DESCRIPTION = 'View';

      SELECT PERMISSIONS_ID
        INTO v_V_AddPermission
        FROM ZFC_PERMISSIONS
         WHERE DESCRIPTION = 'Add';

      SELECT PERMISSIONS_ID
        INTO v_V_EditPermission
        FROM ZFC_PERMISSIONS
         WHERE DESCRIPTION = 'Edit';

      SELECT PERMISSIONS_ID
        INTO v_V_DeletePermission
        FROM ZFC_PERMISSIONS
         WHERE DESCRIPTION = 'Delete';

      DBMS_OUTPUT.PUT_LINE('Adding Security Entity');

      ZFP_SET_SECURITY_ENTITIES(-1,
                             'All',
                             'All Security Entitys',
                             'no url',
                             1,
                             'SQLServer',
                             'FoundationFramework',
                             'Foundation.Framework.SQLServer',
                             'Server=localhost;Initial Catalog=Foundation;Integrated Security=true;',
                             'Blue Arrow',
                             'Default',
                             v_V_ENCRYPTION_TYPE,
                             -1,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      SELECT SE_SEQ_ID
        INTO v_V_SE_SEQ_ID
        FROM ZFC_SECURITY_ENTITIES
         WHERE NAME = 'All';

      DBMS_OUTPUT.PUT_LINE('Adding roles');

      -- Setup ZF_RLS
      ZFP_SET_ROLE(-1,
                         'Anonymous',
                         'The anonymous role.',
                         1,
                         0,
                         1,
                         v_V_DeveloperID,
                         v_now,
                         v_V_DeveloperID,
                         v_now,
                         v_V_PRIMARY_KEY,
                         v_V_ErrorCode);

      ZFP_SET_ROLE(-1,
                   'Authenticated',
                   'The authenticated role.',
                   1,
                   1,
                   1,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

      ZFP_SET_ROLE(-1,
                   'Developer',
                   'The developer role.',
                   1,
                   1,
                   1,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

      ZFP_SET_ROLE(-1,
                   'AlwaysLogon',
                   'Assign this role to allow logon when the system is under maintance.',
                   1,
                   0,
                   1,
                   v_V_DeveloperID,
                   v_now,
                   v_V_DeveloperID,
                   v_now,
                   v_V_PRIMARY_KEY,
                   v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Groups');

      -- group id,group name,group description,Security Entity,added by,added date,updated by,updated date
      ZFP_SET_GROUP(-1,
                          'Everyone',
                          'Group representing both the authenticated and the anonymous roles.',
                          1,
                          v_V_DeveloperID,
                          v_now,
                          v_V_DeveloperID,
                          v_now,
                          v_V_PRIMARY_KEY,
                          v_V_ErrorCode);

      SELECT GROUP_SEQ_ID
        INTO v_V_EVERYONE_ID
        FROM ZFC_SECURITY_GRPS
         WHERE NAME = 'Everyone';

      v_ZFP_SET_GROUP_RLS_param := NULL;

      -- group id, Security Entity,comma sep roles,added by,ErrorCode
      ZFP_SET_GROUP_RLS(v_V_EVERYONE_ID,
                              v_V_SE_SEQ_ID,
                              'Authenticated,Anonymous',
                              v_V_DeveloperID,
                              v_P_ErrorCode => v_ZFP_SET_GROUP_RLS_param);

      DBMS_OUTPUT.PUT_LINE('Adding account security');

      -- Setup the security
      -- Setup the account security
      ZFP_SET_ACCT_RLS('Anonymous',
                             1,
                             'Anonymous',
                             v_V_DeveloperID,
                             v_V_ErrorCode);

      ZFP_SET_ACCT_RLS('Developer',
                       1,
                       'Developer,Authenticated,AlwaysLogon',
                       v_V_DeveloperID,
                       v_V_ErrorCode);

      ZFP_SET_ACCT_RLS('mike',
                       1,
                       'Authenticated',
                       v_V_DeveloperID,
                       v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding functions');

      -- Add functions
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_EnableViewStateTrue := 1;

      v_V_EnableViewStateFalse := 0;

      v_V_IsNavTrue := 1;

      v_V_IsNavFalse := 0;

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Menu Item';

      v_V_MyAction := 'mnuRootMenu';

      ZFP_SET_FUNCTION(-1,
                       'Root Menu',
                       'Place Holer',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'none',
                       v_V_EnableViewStateFalse,
                       v_V_IsNavFalse,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       0,
                       'mnuRootHolder',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      v_V_MyAction := 'GenericHome';

      ZFP_SET_FUNCTION(-1,
                       'Home',
                       'Generic Home',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Home\GenericHome.ascx',
                       v_V_EnableViewStateFalse,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Horizontal,
                       'GenericHome',
                       1,
                       'Shown when not authenticated',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Anonymous',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      v_V_MyAction := 'Home';

      ZFP_SET_FUNCTION(-1,
                       'Home',
                       'Home',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Home\Home.ascx',
                       v_V_EnableViewStateFalse,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Horizontal,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Shown when authenticated',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Authenticated',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      v_V_MyAction := 'Logon';

      ZFP_SET_FUNCTION(-1,
                       'Logon',
                       'Logon',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Accounts\Logon.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Vertical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Loggs on an account',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Anonymous,Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      v_V_MyAction := 'Logoff';

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Menu Item';

      ZFP_SET_FUNCTION(-1,
                       'Logoff',
                       'Logoff',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Accounts\Logoff.ascx',
                       v_V_EnableViewStateFalse,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Horizontal,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Loggs off the system.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Authenticated',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Menu Item';

      v_V_MyAction := 'mnuAdmin';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuRootMenu';

      ZFP_SET_FUNCTION(-1,
                       'Admin',
                       'Administration',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'none',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Administration tasks.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Menu Item';

      v_V_MyAction := 'mnuCalendars';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuRootMenu';

      ZFP_SET_FUNCTION(-1,
                       'Calendars',
                       'Calendars',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'none',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Access to the calendar.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Authenticated,Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Menu Item';

      v_V_MyAction := 'mnuReports';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuRootMenu';

      ZFP_SET_FUNCTION(-1,
                       'Reports',
                       'Reports',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'none',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Access to the reports.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Menu Item';

      v_V_MyAction := 'mnuMyProfile';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuRootMenu';

      ZFP_SET_FUNCTION(-1,
                       'My Profile',
                       'My Profile',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'none',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Access to profile information.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Authenticated,Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding System Administrator menu');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Menu Item';

      v_V_MyAction := 'mnuSystem Administration';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuRootMenu';

      ZFP_SET_FUNCTION(-1,
                       'SysAdmin',
                       'System Administration',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'none',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Serves as the root menu item for the hierarchical menus.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuRootMenu';

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Manage Functions');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Menu Item';

      v_V_MyAction := 'mnuManage Functions';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'Manage Functions',
                       'Manage Functions',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'none',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Menu item for functions.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Add Functions');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'AddFunctions';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuManage Functions';

      ZFP_SET_FUNCTION(-1,
                       'Add Functions',
                       'Add Functions',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Functions\AddEditFunctions.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Adds a function to the system.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Copy Function Security');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Copy Function Security';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuManage Functions';

      ZFP_SET_FUNCTION(-1,
                       'Copy Function Security',
                       'Copy Function Security',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Functions\CopyFunctionSecurity.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Adds a function to the system.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Manage Security Entitys');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Menu Item';

      v_V_MyAction := 'mnuManage Security Entitys';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'Manage Security Entitys',
                       'Manage Security Entitys',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'none',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Menu item for Security Entitys.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('File Management menu');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Menu Item';

      v_V_MyAction := 'mnuManageFiles';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'Manage Files',
                       'Manage Files',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'none',
                       v_V_EnableViewStateFalse,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to manage files.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('cache directory management');

      -- Add module
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'File Manager';

      v_V_MyAction := 'Manage Cachedependency';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuManageFiles';

      ZFP_SET_FUNCTION(-1,
                       'Manage Cachedependency',
                       'Manage Cachedependency',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\FileManagement\FileManagerControl.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to manage the cache dependency direcory.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      -- Set security
      SELECT function_seq_id
        INTO v_V_FUNCTION_SEQ_ID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FUNCTION_SEQ_ID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      ZFP_SET_FUNCTION_RLS(v_V_FUNCTION_SEQ_ID,
                           1,
                           'Developer',
                           v_V_AddPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      ZFP_SET_FUNCTION_RLS(v_V_FUNCTION_SEQ_ID,
                           1,
                           'Developer',
                           v_V_EditPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      ZFP_SET_FUNCTION_RLS(v_V_FUNCTION_SEQ_ID,
                           1,
                           'Developer',
                           v_V_DeletePermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding cache directory');

      -- Add directory information
      SELECT FUNCTION_SEQ_ID
        INTO v_V_FUNCTION_SEQ_ID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_DIRECTORY(v_V_FUNCTION_SEQ_ID,
                        'D:\WebProjects\2005\Foundation\FoundationProjects\FoundationWeb\CacheDependency',
                        0,
                        '',
                        '',
                        v_V_DeveloperID,
                        v_now,
                        v_V_DeveloperID,
                        v_now,
                        v_V_PRIMARY_KEY,
                        v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('cache directory management');

      -- Add module
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'File Manager';

      v_V_MyAction := 'Manage Logs';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuManageFiles';

      ZFP_SET_FUNCTION(-1,
                       'Manage Logs',
                       'Manage Logs',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\FileManagement\FileManagerControl.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to manage the logs direcory.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FUNCTION_SEQ_ID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      -- Set security
      ZFP_SET_FUNCTION_RLS(v_V_FUNCTION_SEQ_ID,
                                 1,
                                 'Developer',
                                 v_V_ViewPermission,
                                 v_V_DeveloperID,
                                 v_V_ErrorCode);

      ZFP_SET_FUNCTION_RLS(v_V_FUNCTION_SEQ_ID,
                           1,
                           'Developer',
                           v_V_AddPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      ZFP_SET_FUNCTION_RLS(v_V_FUNCTION_SEQ_ID,
                           1,
                           'Developer',
                           v_V_EditPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      ZFP_SET_FUNCTION_RLS(v_V_FUNCTION_SEQ_ID,
                           1,
                           'Developer',
                           v_V_DeletePermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding log log');

      -- Add directory information
      SELECT FUNCTION_SEQ_ID
        INTO v_V_FUNCTION_SEQ_ID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_DIRECTORY(v_V_FUNCTION_SEQ_ID,
                        'D:\WebProjects\2005\Foundation\FoundationProjects\FoundationWeb\Logs',
                        0,
                        '',
                        '',
                        v_V_DeveloperID,
                        v_now,
                        v_V_DeveloperID,
                        v_now,
                        v_V_PRIMARY_KEY,
                        v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Manage Name/Value Pairs');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Menu Item';

      v_V_MyAction := 'mnuManage Name Value Pairs';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'Manage Name/Value Pairs',
                       'Manage Name/Value Pairs',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'none',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Menu item for name/value pairs.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Add Edit Groups');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Add Edit Groups';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuAdmin';

      ZFP_SET_FUNCTION(-1,
                       'Add Edit Groups',
                       'Add Edit Groups',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Groups\AddEditGroups.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to add or edit roles.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_AddPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_EditPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_DeletePermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Manage Messages');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Menu Item';

      v_V_MyAction := 'mnuManage Messages';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuAdmin';

      ZFP_SET_FUNCTION(-1,
                       'Manage Messages',
                       'Manage Messages',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'none',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Menu item for messages.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Manage States');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Menu Item';

      v_V_MyAction := 'mnuManage States';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'Manage States',
                       'Manage States',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'none',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Menu item for states.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Manage Work Flows');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Menu Item';

      v_V_MyAction := 'mnuWork Flows';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'Manage Work Flows',
                       'Manage Work Flows',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'none',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Menu item for work flows.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Encryption Helper');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'EncryptionHelper';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'Encryption Helper',
                       'Encryption Helper',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Encrypt\EncryptDecrypt.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Menu item for work flows.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding GUID Helper');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'GUIDHelper';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'GUID Helper',
                       'Displays''s a GUID',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Encrypt\GUIDHelper.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Displays a GUID may be necessary if you need to change the GUID in your project files.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Random Number');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Random Numbers';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'Random Numbers',
                       'Displays''s a set of randomly generated number''s',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Encrypt\RandomNumbers.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Displays''s a set of randomly generated number''s.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Set Log Level');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'SetLogLevel';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'Set Log Level',
                       'Set Log Level',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Logs\SetLogLevel.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to set the log level of the application ... Debug, Error, Warn, Fatal.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Update Anonymous Profile');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'UpdateAnonymousProfile';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'Update Anonymous Profile',
                       'Update Anonymous Profile',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\AnonymousAccount\UpdateAnonymousCache.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Remove any cached information for the anonymous account.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Search Functions');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'SearchFunctions';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuManage Functions';

      ZFP_SET_FUNCTION(-1,
                       'Search Functions',
                       'Search Functions',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Functions\SearchFunctions.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Searches for functions in the system.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_DeletePermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Edit Functions');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'EditFunctions';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'Edit Functions',
                       'Edit Functions',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Functions\AddEditFunctions.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavFalse,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Edits a function in the system.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Function Security');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Function Security';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuReports';

      ZFP_SET_FUNCTION(-1,
                       'Function Security',
                       'Function Security',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Reports\FunctionSecurity.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Displays a report for function security.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Security By Role');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Security By Role';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuReports';

      ZFP_SET_FUNCTION(-1,
                       'Security By Role',
                       'Security By Role',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Reports\SecurityByRole.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Displays a report for security by role.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Change Password');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Change Password';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuMyProfile';

      ZFP_SET_FUNCTION(-1,
                       'Change Password',
                       'Change Password',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Accounts\ChangePassword.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to change an accounts password.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Authenticated',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Change Colors');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Change Colors';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuMyProfile';

      ZFP_SET_FUNCTION(-1,
                       'Change Colors',
                       'Change Colors',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Accounts\ChangeColors.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to change an accounts color scheme.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Authenticated',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Select Preferences');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Select Preferences';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuMyProfile';

      ZFP_SET_FUNCTION(-1,
                       'Select Preferences',
                       'Select Preferences',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Accounts\SelectPreferences.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to select preference for an account, records per page etc.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Authenticated',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Edit Account');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Edit Account';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuMyProfile';

      ZFP_SET_FUNCTION(-1,
                       'Edit Account',
                       'Edit Account',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Accounts\AddEditAccount.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to edit an account profile.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Authenticated',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Edit Other Account');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Edit Other Account';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuMyProfile';

      ZFP_SET_FUNCTION(-1,
                       'Edit Other Account',
                       'Edit Other Account',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Accounts\AddEditAccount.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavFalse,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to edit anothers account profile.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Community Calendar');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Community Calendar';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuCalendars';

      ZFP_SET_FUNCTION(-1,
                       'Community Calendar',
                       'Community Calendar',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Calendar\CommunityCalendar.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to show calendar data.  Created as an example module.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Authenticated',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Add Account');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Add Account';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuAdmin';

      ZFP_SET_FUNCTION(-1,
                       'Add Account',
                       'Add Account',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Accounts\AddEditAccount.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to add an accounts password.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Add Edit Roles');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Add Edit Roles';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuAdmin';

      ZFP_SET_FUNCTION(-1,
                       'Add Edit Roles',
                       'Add Edit Roles',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Roles\AddEditRoles.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to add or edit roles.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_AddPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_EditPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_DeletePermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Add Edit Name Value Pairs Details');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Add Edit List of values';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuAdmin';

      ZFP_SET_FUNCTION(-1,
                       'Add Edit List of Values',
                       'Add Edit List of Values',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\LOV\AddEditLOV.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to add or edit a list of value details.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding ViewAccountRoleTab');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Security';

      v_V_MyAction := 'ViewAccountRoleTab';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuAdmin';

      ZFP_SET_FUNCTION(-1,
                       'ViewAccountRoleTab',
                       'View Accounts Roles Tab',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'None',
                       v_V_EnableViewStateFalse,
                       v_V_IsNavFalse,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used as a security holder for roles that can view the accounts role tab.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding ViewFunctionRoleTab');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Security';

      v_V_MyAction := 'ViewFunctionRoleTab';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuAdmin';

      ZFP_SET_FUNCTION(-1,
                       'ViewFunctionRoleTab',
                       'View Functions Roles Tab',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'None',
                       v_V_EnableViewStateFalse,
                       v_V_IsNavFalse,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used as a security holder for roles that can view the functions role tab.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding ViewAccountGroupTab');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Security';

      v_V_MyAction := 'ViewAccountGroupTab';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuAdmin';

      ZFP_SET_FUNCTION(-1,
                       'ViewAccountGroupTab',
                       'View Accounts Groups Tab',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'None',
                       v_V_EnableViewStateFalse,
                       v_V_IsNavFalse,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used as a security holder for groups that can view the accounts group tab.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding ViewFunctionGroupTab');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Security';

      v_V_MyAction := 'ViewFunctionGroupTab';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuAdmin';

      ZFP_SET_FUNCTION(-1,
                       'ViewFunctionGroupTab',
                       'View Function Groups Tab',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'None',
                       v_V_EnableViewStateFalse,
                       v_V_IsNavFalse,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used as a security holder for groups that can view the functions group tab.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Search Accounts');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Search Accounts';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuAdmin';

      ZFP_SET_FUNCTION(-1,
                       'Search Accounts',
                       'Search Accounts',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Accounts\SearchAccounts.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to select accounts for edit.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Edit Role Members');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Edit Role Members';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuAdmin';

      ZFP_SET_FUNCTION(-1,
                       'Edit Role Members',
                       'Edit Role Members',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Roles\EditRoleMembers.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavFalse,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to add or remove members of a role.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_EditPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Edit Group Members');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Edit Group Members';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuAdmin';

      ZFP_SET_FUNCTION(-1,
                       'Edit Group Members',
                       'Edit Group Members',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Groups\EditGroupMembers.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavFalse,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to add or remove members of a role.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Navigation');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'VerticalMenu';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuAdmin';

      ZFP_SET_FUNCTION(-1,
                       'Navigation',
                       'Navigation',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Navigation\VerticalMenuUserControl.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavFalse,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Contains link items for the vertical menus.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Anonymous,Authenticated',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Not Avalible');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'NotAvalible';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuAdmin';

      ZFP_SET_FUNCTION(-1,
                       'Not Avalible',
                       'Not Avalible',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Errors\NotAvailable.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavFalse,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Error page when the action is not avalible.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Anonymous,Authenticated',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      --AccessDenied
      DBMS_OUTPUT.PUT_LINE('Adding Access Denied');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'AccessDenied';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuAdmin';

      ZFP_SET_FUNCTION(-1,
                       'Access Denied',
                       'Access Denied',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Errors\AccessDenied.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavFalse,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Error page when the account being used does not have sufficient access to the view permission.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Anonymous,Authenticated',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      --Adding Error
      DBMS_OUTPUT.PUT_LINE('Adding Error');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'DisplayError';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuAdmin';

      ZFP_SET_FUNCTION(-1,
                       'Display Error',
                       'Display Error',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Errors\DisplayError.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavFalse,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Error page when unknown or unexpected error occurs.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Anonymous,Authenticated',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      --Select A Security Entity
      DBMS_OUTPUT.PUT_LINE('Adding Select A Security Entity');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Select A Security Entity';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuAdmin';

      ZFP_SET_FUNCTION(-1,
                       'Select A Security Entity',
                       'Select A Security Entity',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\SecurityEntities\SelectSecurityEntity.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Vertical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to select a Security Entity.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Authenticated',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Web configuration
      DBMS_OUTPUT.PUT_LINE('Adding Web configuration');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Web Config';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'Web Config',
                       'Web Config',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Configuration\AddEditWebConfig.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Adds or edits web.config file settings.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Line Count
      DBMS_OUTPUT.PUT_LINE('Adding Line Count');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Line Count';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'Line Count',
                       'Line Count',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\LineCount.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Utility to count the lines of code.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Add a Security Entity
      DBMS_OUTPUT.PUT_LINE('Adding Add Security Entitys');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Add Security Entitys';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuManage Security Entitys';

      ZFP_SET_FUNCTION(-1,
                       'Add Security Entitys',
                       'Add Security Entitys',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\SecurityEntities\AddEditSecurityEntities.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to add a Security Entity.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Edit a Security Entity
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Edit a Security Entity';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuManage Security Entitys';

      ZFP_SET_FUNCTION(-1,
                       'Edit a Security Entity',
                       'Edit a Security Entity',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\SecurityEntities\AddEditSecurityEntities.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavFalse,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to edit a Security Entity.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Search Security Entitys
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Search Security Entitys';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuManage Security Entitys';

      ZFP_SET_FUNCTION(-1,
                       'Search Security Entitys',
                       'Search Security Entitys',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\SecurityEntities\SearchSecurityEntities.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to search a Security Entity.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Add a Name Value Pair
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Add Name Value Pairs';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuManage Name Value Pairs';

      ZFP_SET_FUNCTION(-1,
                       'Add Name Value Pairs',
                       'Add Security Entitys',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\NVP\AddEditNVP.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to add a name/value pair.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Edit a Name Value Pair
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Edit Name Value Pairs';

      --SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
      ZFP_SET_FUNCTION(-1,
                             'Edit a Name Value Pair',
                             'Edit a Name Value Pair',
                             v_V_FUNCTION_TYPE,
                             v_V_ALLOW_HTML_INPUT,
                             v_V_ALLOW_COMMENT_HTML_INPUT,
                             'Modules\System\Administration\NVP\AddEditNVP.ascx',
                             v_V_EnableViewStateTrue,
                             v_V_IsNavFalse,
                             v_V_NAV_TYPE_Hierarchical,
                             v_V_MyAction,
                             v_V_ParentID,
                             'Used to edit a name/value pair.',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Search Name Value Pairs
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Search Name Value Pairs';

      --SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
      ZFP_SET_FUNCTION(-1,
                             'Search Name Value Pairs',
                             'Search Name Value Pairs',
                             v_V_FUNCTION_TYPE,
                             v_V_ALLOW_HTML_INPUT,
                             v_V_ALLOW_COMMENT_HTML_INPUT,
                             'Modules\System\Administration\NVP\SearchNVP.ascx',
                             v_V_EnableViewStateTrue,
                             v_V_IsNavTrue,
                             v_V_NAV_TYPE_Hierarchical,
                             v_V_MyAction,
                             v_V_ParentID,
                             'Used to search a name/value pair.',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Add a Message
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Add Message';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuManage Messages';

      ZFP_SET_FUNCTION(-1,
                       'Add Message',
                       'Add Message',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Messages\AddEditMessage.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to add a message.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Edit a Message
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Edit Message';

      --SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
      ZFP_SET_FUNCTION(-1,
                             'Edit a Message',
                             'Edit a Message',
                             v_V_FUNCTION_TYPE,
                             v_V_ALLOW_HTML_INPUT,
                             v_V_ALLOW_COMMENT_HTML_INPUT,
                             'Modules\System\Administration\Messages\AddEditMessage.ascx',
                             v_V_EnableViewStateTrue,
                             v_V_IsNavFalse,
                             v_V_NAV_TYPE_Hierarchical,
                             v_V_MyAction,
                             v_V_ParentID,
                             'Used to edit a Message.',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Search Message
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Search Messages';

      --SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
      ZFP_SET_FUNCTION(-1,
                             'Search Messages',
                             'Search Messages',
                             v_V_FUNCTION_TYPE,
                             v_V_ALLOW_HTML_INPUT,
                             v_V_ALLOW_COMMENT_HTML_INPUT,
                             'Modules\System\Administration\Messages\SearchMessages.ascx',
                             v_V_EnableViewStateTrue,
                             v_V_IsNavTrue,
                             v_V_NAV_TYPE_Hierarchical,
                             v_V_MyAction,
                             v_V_ParentID,
                             'Used to search a Message.',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Edit a State
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Edit State';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuManage States';

      ZFP_SET_FUNCTION(-1,
                       'Edit a State',
                       'Edit a State',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\States\AddEditStates.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavFalse,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to edit a State.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Search State
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Search States';

      --SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
      ZFP_SET_FUNCTION(-1,
                             'Search States',
                             'Search States',
                             v_V_FUNCTION_TYPE,
                             v_V_ALLOW_HTML_INPUT,
                             v_V_ALLOW_COMMENT_HTML_INPUT,
                             'Modules\System\Administration\States\SearchStates.ascx',
                             v_V_EnableViewStateTrue,
                             v_V_IsNavTrue,
                             v_V_NAV_TYPE_Hierarchical,
                             v_V_MyAction,
                             v_V_ParentID,
                             'Used to search a State.',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Add a Workflows
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Add a Workflow';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuWork Flows';

      ZFP_SET_FUNCTION(-1,
                       'Add a Workflow',
                       'Add a Workflow',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Messages\AddEditMessage.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to add a Workflow.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Edit a Workflows
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Edit a Workflow';

      --SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
      ZFP_SET_FUNCTION(-1,
                             'Edit a Workflow',
                             'Edit a Workflow',
                             v_V_FUNCTION_TYPE,
                             v_V_ALLOW_HTML_INPUT,
                             v_V_ALLOW_COMMENT_HTML_INPUT,
                             'Modules\System\Administration\Messages\AddEditMessage.ascx',
                             v_V_EnableViewStateTrue,
                             v_V_IsNavFalse,
                             v_V_NAV_TYPE_Hierarchical,
                             v_V_MyAction,
                             v_V_ParentID,
                             'Used to edit a Workflow.',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Search Workflows
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Search Workflows';

      --SET @V_ParentID = (SELECT FUNCTION_SEQ_ID FROM ZFC_FUNCTIONS WHERE [ACTION] = 'mnuManage Security Entitys')
      ZFP_SET_FUNCTION(-1,
                             'Search Workflows',
                             'Search Workflows',
                             v_V_FUNCTION_TYPE,
                             v_V_ALLOW_HTML_INPUT,
                             v_V_ALLOW_COMMENT_HTML_INPUT,
                             'Modules\System\Administration\Messages\SearchMessages.ascx',
                             v_V_EnableViewStateTrue,
                             v_V_IsNavTrue,
                             v_V_NAV_TYPE_Hierarchical,
                             v_V_MyAction,
                             v_V_ParentID,
                             'Used to search a Workflows.',
                             v_V_DeveloperID,
                             v_now,
                             v_V_DeveloperID,
                             v_now,
                             v_V_PRIMARY_KEY,
                             v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Update Session
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Update';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'Update',
                       'Update',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Accounts\UpdateSession.ascx',
                       v_V_EnableViewStateFalse,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Horizontal,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to update the session menus and roles.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Authenticated',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      -- Under Maintance
      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'UnderMaintance';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'Under Maintance',
                       'Under Maintance',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\UnderMaintance.ascx',
                       v_V_EnableViewStateFalse,
                       v_V_IsNavFalse,
                       v_V_NAV_TYPE_Horizontal,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to update the session menus and roles.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Anonymous,Authenticated',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding AlwaysLogon');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Security';

      v_V_MyAction := 'AlwaysLogon';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'Always Logon',
                       'Always Logon',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'none',
                       v_V_EnableViewStateFalse,
                       v_V_IsNavFalse,
                       v_V_NAV_TYPE_Horizontal,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to update the session menus and roles.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'AlwaysLogon',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding Edit DB Information');

      SELECT FUNCTION_TYPE_SEQ_ID
        INTO v_V_FUNCTION_TYPE
        FROM ZFC_FUNCTION_TYPES
         WHERE NAME = 'Module';

      v_V_MyAction := 'Edit DB Information';

      SELECT FUNCTION_SEQ_ID
        INTO v_V_ParentID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = 'mnuSystem Administration';

      ZFP_SET_FUNCTION(-1,
                       'Edit DB Information',
                       'Edit DB Information',
                       v_V_FUNCTION_TYPE,
                       v_V_ALLOW_HTML_INPUT,
                       v_V_ALLOW_COMMENT_HTML_INPUT,
                       'Modules\System\Administration\Configuration\AddEditDBInformation.ascx',
                       v_V_EnableViewStateTrue,
                       v_V_IsNavTrue,
                       v_V_NAV_TYPE_Hierarchical,
                       v_V_MyAction,
                       v_V_ParentID,
                       'Used to update the ZF_Information table, enable inheritence.',
                       v_V_DeveloperID,
                       v_now,
                       v_V_DeveloperID,
                       v_now,
                       v_V_PRIMARY_KEY,
                       v_V_ErrorCode);

      SELECT function_seq_id
        INTO v_V_FunctionID
        FROM ZFC_FUNCTIONS
         WHERE ACTION = v_V_MyAction;

      ZFP_SET_FUNCTION_RLS(v_V_FunctionID,
                           1,
                           'Developer',
                           v_V_ViewPermission,
                           v_V_DeveloperID,
                           v_V_ErrorCode);

      DBMS_OUTPUT.PUT_LINE('Adding messages');

      ZFP_SET_MESSAGE(-1,
                      v_V_SE_SEQ_ID,
                      'Logon Error',
                      'Logon Error',
                      'Displayed when logon fails',
                      '<b>Invalid Account or Password!</b>',
                      v_V_FORMAT_AS_HTML_TRUE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      ZFP_SET_MESSAGE(-1,
                      v_V_SE_SEQ_ID,
                      'New Account',
                      'New Account',
                      'Message sent when an account is created.',
                      'Dear <FullName>,
                      	There has been a request for a new account:
                      		Please Use this link to logon:
                      	 <Server>Default.aspx?Action=Logon&Account=<AccountName>&Password=<Password>
                      	<b>Please note once you have logged on using this link you will only be able to change our password.</b>',
                      v_V_FORMAT_AS_HTML_FALSE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      ZFP_SET_MESSAGE(-1,
                      v_V_SE_SEQ_ID,
                      'Request Password Reset UI',
                      'Request Password Reset UI',
                      'Displayed when new password is requested',
                      '<b>An EMail has been send to your account with instructions!</b>',
                      v_V_FORMAT_AS_HTML_TRUE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      ZFP_SET_MESSAGE(-1,
                      v_V_SE_SEQ_ID,
                      'RequestNewPassword',
                      'Request New Password',
                      'Request New Password',
                      'Dear <FullName>,
                      	There has been a request for a password change:
                      		Please Use this link to logon:
                      	 <Server>Default.aspx?Action=Logon&Account=<AccountName>&Password=<Password>
                      	<b>Please note once you have logged on using this link you will only be able to change our password.</b>',
                      v_V_FORMAT_AS_HTML_TRUE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      ZFP_SET_MESSAGE(-1,
                      1,
                      'WebConfigNotSaved',
                      'Blank Environment Text',
                      'Blank Environment Text',
                      'Settings have not been saved.',
                      v_V_FORMAT_AS_HTML_FALSE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      ZFP_SET_MESSAGE(-1,
                      1,
                      'WebConfigIsLocked',
                      'Web Config Is Locked',
                      'Web Config Is Locked',
                      'Configuration Section is locked. Unable to modify.',
                      v_V_FORMAT_AS_HTML_FALSE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      ZFP_SET_MESSAGE(-1,
                      1,
                      'WebConfigEnvironmentRequired',
                      'Web Config Environment Required',
                      'Web Config Environment Required',
                      'You have selected a new environment but did not give the name.',
                      v_V_FORMAT_AS_HTML_FALSE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      ZFP_SET_MESSAGE(-1,
                      1,
                      'ErrorAccountDetails',
                      'Error Account Details',
                      'Error Account Details',
                      'Could not set account details.',
                      v_V_FORMAT_AS_HTML_FALSE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      ZFP_SET_MESSAGE(-1,
                      1,
                      'PasswordSendMailError',
                      'Password Send Mail Error',
                      'Password Send Mail Error',
                      'The password was reset, but, an email could not be sent.',
                      v_V_FORMAT_AS_HTML_FALSE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      ZFP_SET_MESSAGE(-1,
                      1,
                      'DisabledAccount',
                      'Disabled Account',
                      'Disabled Account',
                      'This account is disabled.',
                      v_V_FORMAT_AS_HTML_FALSE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      ZFP_SET_MESSAGE(-1,
                      1,
                      'SuccessChangePassword',
                      'Success Change Password',
                      'Success Change Password',
                      'Your password has been changed.',
                      v_V_FORMAT_AS_HTML_FALSE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      ZFP_SET_MESSAGE(-1,
                      1,
                      'UnSuccessChangePassword',
                      'UnSuccess Change Password',
                      'UnSuccess ChangePassword',
                      'Your password has NOT been changed.',
                      v_V_FORMAT_AS_HTML_FALSE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      ZFP_SET_MESSAGE(-1,
                      1,
                      'PasswordNotMatched',
                      'Password Not Matched',
                      'Password Not Matched',
                      'The OLD password did not match your current password.',
                      v_V_FORMAT_AS_HTML_FALSE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      ZFP_SET_MESSAGE(-1,
                      1,
                      'UnderMaintance',
                      'Under Maintance',
                      'Under Maintance',
                      'The system is currently under maintance and logons have been limited.',
                      v_V_FORMAT_AS_HTML_FALSE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      ZFP_SET_MESSAGE(-1,
                      1,
                      'UnderConstruction',
                      'Under Construction',
                      'Under Construction',
                      'The system is currently under construction.',
                      v_V_FORMAT_AS_HTML_FALSE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      ZFP_SET_MESSAGE(-1,
                      1,
                      'NoDataFound',
                      'No Data Found',
                      'No Data Found',
                      'No Data Found.',
                      v_V_FORMAT_AS_HTML_FALSE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      ZFP_SET_MESSAGE(-1,
                      1,
                      'ChangedSelectedSecurityEntity',
                      'Changed Selected Security Entity',
                      'Message for when a account changes the selected Security Entity.',
                      'You have changed your selected Security Entity.',
                      v_V_FORMAT_AS_HTML_FALSE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      ZFP_SET_MESSAGE(-1,
                      1,
                      'SameAccountChangeAccount',
                      'Same Account Change Account',
                      'Message for when a account changes their own account.',
                      'showMSG("If you change your account the system will need to log you off.")',
                      v_V_FORMAT_AS_HTML_FALSE,
                      v_V_DeveloperID,
                      v_now,
                      v_V_DeveloperID,
                      v_now,
                      v_V_PRIMARY_KEY,
                      v_V_ErrorCode);

      -- Insert States
      DBMS_OUTPUT.PUT_LINE('Adding States');

      DELETE ZOP_STATES
      ;

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'AA', 'Armed Forces Americas', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'AE', 'Armed Forces Africa', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'AK', 'Alaska', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'AL', 'Alabama', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'AP', 'Armed Forces Pacific', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'AR', 'Arkansas', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'AS', 'American Samoa', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'AZ', 'Arizona', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'CA', 'California', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'CO', 'Colorado', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'CT', 'Connecticut', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'DC', 'District Of Columbia', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'DE', 'Delaware', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'FL', 'Florida', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'FM', 'Federated States of Micro', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'GA', 'Georgia', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'GU', 'Gaum', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'HI', 'Hawaii', v_V_ACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'IA', 'Iowa', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'ID', 'Idaho', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'IL', 'Illinois', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'IN', 'Indiana', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'KS', 'Kansas', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'KY', 'Kentucky', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'LA', 'Louisiana', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'MA', 'Massachusetts', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'MD', 'Maryland', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'ME', 'Maine', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'MH', 'Marshall Islands', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'MI', 'Michigan', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'MN', 'Minnesota', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'MO', 'Missouri', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'MP', 'Northern Mariana Islands', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'MS', 'Mississippi', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'MT', 'Montana', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'NC', 'North Carolina', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'ND', 'North Dakota', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'NE', 'Nebraska', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'NH', 'New Hampshire', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'NJ', 'New Jersey', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'NM', 'New Mexico', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'NV', 'Nevada', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'NY', 'New York', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'OH', 'Ohio', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'OK', 'Oklahoma', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'OR', 'Oregon', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'PA', 'Pennsylvania', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'PR', 'Puerto Rico', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'PW', 'Palau', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'RI', 'Rhode Island', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'SC', 'South Carolina', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'SD', 'South Dakota', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'TN', 'Tennessee', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'TX', 'Texas', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'UT', 'Utah', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'VA', 'Virginia', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'VI', 'Virgin Islands', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'VT', 'Vermont', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'WA', 'Washington', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'WI', 'Wisconsin', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'WV', 'West Virginia', v_V_INACTIVE );

      INSERT INTO ZOP_STATES
        ( STATE, DESCRIPTION, STATUS_SEQ_ID )
        VALUES ( 'WY', 'Wyoming', v_V_INACTIVE );

      NULL/*TODO:update statistics ZOP_STATES*/;

   END;
   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_FUNCTION_GRPS
(
  v_P_FUNCTION_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_GROUPS IN VARCHAR2 DEFAULT NULL ,
  v_P_PERMISSIONS_ID IN NUMBER DEFAULT NULL ,
  v_P_ADDUPD_BY IN NUMBER DEFAULT NULL ,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
   v_V_GROUP_SEQ_ID NUMBER(10,0);
   v_V_GRPS_SE_SEQ_ID NUMBER(10,0);
   v_V_GROUP_NAME VARCHAR2(50);
   v_V_Pos NUMBER(10,0);
BEGIN

   SET TRANSACTION READ WRITE;

   -- NEED TO DELETE EXISTING ROLE ASSOCITAED WITH THE FUNCTION BEFORE
   -- INSERTING NEW ONES.
   ZFP_DEL_FUNCTION_GRP_SECURITY(v_P_FUNCTION_SEQ_ID,
                                       v_P_SE_SEQ_ID,
                                       v_P_PERMISSIONS_ID,
                                       v_P_ADDUPD_BY,
                                       v_P_ErrorCode);

   v_P_GROUPS := LTRIM(RTRIM(v_P_GROUPS)) || ',';

   v_V_Pos := INSTR(v_P_GROUPS, ',', 1);

   IF REPLACE(v_P_GROUPS, ',', '') <> '' THEN
      WHILE v_V_Pos > 0
      LOOP
         BEGIN
            v_V_GROUP_NAME := LTRIM(RTRIM(SUBSTR(v_P_GROUPS, 0, v_V_Pos - 1)));

            IF v_V_GROUP_NAME <> '' THEN
            DECLARE
               v_temp NUMBER(1, 0) := 0;
            BEGIN
               --select the role seq id first
               SELECT GROUP_SEQ_ID
                 INTO v_V_GROUP_SEQ_ID
                 FROM ZFC_SECURITY_GRPS
                  WHERE NAME = v_V_GROUP_NAME;

               SELECT GRPS_SE_SEQ_ID
                 INTO v_V_GRPS_SE_SEQ_ID
                 FROM ZFC_SECURITY_GRPS_SE
                  WHERE GROUP_SEQ_ID = v_V_GROUP_SEQ_ID
                          AND SE_SEQ_ID = v_P_SE_SEQ_ID;

               BEGIN
                  SELECT 1 INTO v_temp
                    FROM DUAL
                   WHERE NOT EXISTS ( SELECT GRPS_SE_SEQ_ID
                                      FROM ZFC_SECURITY_FUNCT_GRPS
                                         WHERE FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID
                                                 AND PERMISSIONS_ID = v_P_PERMISSIONS_ID
                                                 AND GRPS_SE_SEQ_ID = v_V_GRPS_SE_SEQ_ID );
               EXCEPTION
                  WHEN OTHERS THEN
                     NULL;
               END;

               --PRINT('@V_GRPS_SE_SEQ_ID = ' + CONVERT(VARCHAR,@V_GRPS_SE_SEQ_ID))
               IF v_temp = 1 THEN
               BEGIN
                  --PRINT('INSERT RECORD')
                  INSERT INTO ZFC_SECURITY_FUNCT_GRPS
                    ( FUNCTION_SEQ_ID, GRPS_SE_SEQ_ID, PERMISSIONS_ID, ADDED_BY )
                    VALUES ( v_P_FUNCTION_SEQ_ID, v_V_GRPS_SE_SEQ_ID, v_P_PERMISSIONS_ID, v_P_ADDUPD_BY );

               END;
               END IF;

            END;
            END IF;

            v_P_GROUPS := SUBSTR(v_P_GROUPS, -1, LENGTH(v_P_GROUPS) - v_V_Pos);

            v_V_Pos := INSTR(v_P_GROUPS, ',', 1);

            v_P_ErrorCode := v_sys_error;

         END;
      END LOOP;

   END IF;

   IF v_P_ErrorCode <> 0 THEN
      GOTO ABEND;

   END IF;

   COMMIT;

   <<ABEND>>

   IF v_P_ErrorCode <> 0 THEN
   BEGIN
      DBMS_OUTPUT.PUT_LINE('Yikes!');

      ROLLBACK;

   END;
   END IF;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_FUNCTION_SORT
(
  v_P_FUNCTION_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_DIRECTION IN NUMBER DEFAULT NULL ,
  v_P_UPDATED_BY IN NUMBER DEFAULT NULL ,
  v_P_UPDATED_DATE IN DATE DEFAULT NULL ,
  v_P_PRIMARY_KEY OUT NUMBER,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_V_SORT_ORDER_CURRENT NUMBER(10,0);
   v_V_SORT_ORDER_MOVE NUMBER(10,0);
   v_V_PARENT_FUNCTION_SEQ_ID NUMBER(10,0);
BEGIN

   -- Get the parent ID so only the menu items here can be effected
   SELECT PARENT_FUNCTION_SEQ_ID
     INTO v_V_PARENT_FUNCTION_SEQ_ID
     FROM ZFC_FUNCTIONS
      WHERE FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID;

   -- Get Current Sort Order
   SELECT SORT_ORDER
     INTO v_V_SORT_ORDER_CURRENT
     FROM ZFC_FUNCTIONS
      WHERE FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID;

   -- Get Sort Order for Section Above
   IF v_P_DIRECTION = 0 THEN-- Down
   
   BEGIN
      SELECT MIN(SORT_ORDER)
        INTO v_V_SORT_ORDER_MOVE
        FROM ZFC_FUNCTIONS
         WHERE SORT_ORDER > v_V_SORT_ORDER_CURRENT;

   END;
   ELSE-- up
   
   BEGIN
      SELECT MAX(SORT_ORDER)
        INTO v_V_SORT_ORDER_MOVE
        FROM ZFC_FUNCTIONS
         WHERE SORT_ORDER < v_V_SORT_ORDER_CURRENT;

   END;
   END IF;

   -- END IF
   -- If no row to move, exit
   IF v_V_SORT_ORDER_MOVE IS NULL THEN
      -- Otherwise, switch sort orders
      RETURN UPDATE_;
      /*Limitation:Syntax Not Recognized:SORT_ORDER = @V_SORT_ORDER_CURRENT WHERE SORT_ORDER = @V_SORT_ORDER_MOVE */

   END IF;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_NVP
(
  v_P_NVP_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_TABLE_NAME IN VARCHAR2 DEFAULT NULL ,
  v_P_DISPLAY IN VARCHAR2 DEFAULT NULL ,
  v_P_DESCRIPTION IN VARCHAR2 DEFAULT NULL ,
  v_P_ADDED_BY IN NUMBER DEFAULT NULL ,
  v_P_ADDED_DATE IN DATE DEFAULT NULL ,
  v_P_UPDATED_BY IN NUMBER DEFAULT NULL ,
  v_P_UPDATED_DATE IN DATE DEFAULT NULL ,
  v_P_PRIMARY_KEY OUT NUMBER,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   IF v_P_NVP_SEQ_ID > -1 THEN
   BEGIN-- UPDATE PROFILE
   
      UPDATE ZFC_NVP
         SET TABLE_NAME = v_P_TABLE_NAME,
             DISPLAY = v_P_DISPLAY,
             DESCRIPTION = v_P_DESCRIPTION,
             UPDATED_BY = v_P_UPDATED_BY,
             UPDATED_DATE = v_P_UPDATED_DATE
         WHERE NVP_SEQ_ID = v_P_NVP_SEQ_ID;

      v_P_PRIMARY_KEY := v_P_NVP_SEQ_ID;

   END;
   ELSE
   DECLARE
      v_temp NUMBER(1, 0) := 0;
   BEGIN-- INSERT a new row in the table.
   
      BEGIN
         SELECT 1 INTO v_temp
           FROM DUAL
          WHERE EXISTS ( SELECT TABLE_NAME
                         FROM ZFC_NVP
                            WHERE TABLE_NAME = v_P_TABLE_NAME );
      EXCEPTION
         WHEN OTHERS THEN
            NULL;
      END;

      -- CHECK FOR DUPLICATE NAME BEFORE INSERTING
      IF v_temp = 1 THEN
      BEGIN
         raise_application_error( -20002, 'The name value pair already exists in the database.' );

         RETURN;

      END;
      END IF;

      INSERT INTO ZFC_NVP
        ( TABLE_NAME, DISPLAY, DESCRIPTION, ADDED_BY, ADDED_DATE, UPDATED_BY, UPDATED_DATE )
        VALUES ( v_P_TABLE_NAME, v_P_DISPLAY, v_P_DESCRIPTION, v_P_ADDED_BY, v_P_ADDED_DATE, v_P_ADDED_BY, v_P_ADDED_DATE );

      v_P_PRIMARY_KEY := sqlserver_utilities.identity;-- Get the IDENTITY value for the row just inserted.
      

   END;
   END IF;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_ROLE_ACCTS
-- =============================================
-- Author:		<Michael Regan>
-- Create date: <01010001,,>
-- Description:	<yep,,>
-- =============================================
(
  v_P_ROLE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_SE_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_ACCOUNT IN VARCHAR2 DEFAULT NULL ,
  v_P_ADDUPD_BY IN NUMBER DEFAULT NULL
)
AS
   v_V_ACCT_SEQ_ID NUMBER(10,0);
   v_V_RLS_SE_SEQ_ID NUMBER(10,0);
BEGIN

   SELECT ACCT_SEQ_ID
     INTO v_V_ACCT_SEQ_ID
     FROM ZFC_ACCTS
      WHERE ACCT = v_P_ACCOUNT;

   SELECT RLS_SE_SEQ_ID
     INTO v_V_RLS_SE_SEQ_ID
     FROM ZFC_SECURITY_RLS_SE
      WHERE ROLE_SEQ_ID = v_P_ROLE_SEQ_ID
              AND SE_SEQ_ID = v_P_SE_SEQ_ID;

   INSERT INTO ZFC_SECURITY_ACCTS_RLS
     ( RLS_SE_SEQ_ID, ACCT_SEQ_ID, ADDED_BY )
     VALUES ( v_V_RLS_SE_SEQ_ID, v_V_ACCT_SEQ_ID, v_P_ADDUPD_BY );

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_STATE
(
  v_P_STATE IN VARCHAR2 DEFAULT NULL ,
  v_P_DESCRIPTION IN VARCHAR2 DEFAULT NULL ,
  v_P_STATUS_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_UPDATED_BY IN NUMBER DEFAULT NULL ,
  v_P_UPDATED_DATE IN DATE DEFAULT NULL ,
  v_P_PRIMARY_KEY OUT VARCHAR2,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   BEGIN
      UPDATE ZOP_STATES
         SET STATE = v_P_STATE,
             DESCRIPTION = v_P_DESCRIPTION,
             STATUS_SEQ_ID = v_P_STATUS_SEQ_ID,
             UPDATED_BY = v_P_UPDATED_BY,
             UPDATED_DATE = v_P_UPDATED_DATE
         WHERE STATE = v_P_STATE;

      v_P_PRIMARY_KEY := v_P_STATE;

   END;
   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE PROCEDURE ZFP_SET_WORK_FLOWS
(
  v_P_WORK_FLOW_ID IN NUMBER DEFAULT NULL ,
  v_P_ORDER_ID IN NUMBER DEFAULT NULL ,
  v_P_WORK_FLOW_NAME IN VARCHAR2 DEFAULT NULL ,
  v_P_FUNCTION_SEQ_ID IN NUMBER DEFAULT NULL ,
  v_P_PRIMARY_KEY OUT NUMBER,
  v_P_ErrorCode OUT NUMBER
)
AS
   v_sys_error NUMBER := 0;
BEGIN

   IF v_P_PRIMARY_KEY > -1 THEN
   BEGIN
      -- UPDATE an existing row in the table.
      UPDATE ZOP_WORK_FLOWS
         SET ORDER_ID = v_P_ORDER_ID,
             WORK_FLOW_NAME = v_P_WORK_FLOW_NAME,
             FUNCTION_SEQ_ID = v_P_FUNCTION_SEQ_ID
         WHERE WORK_FLOW_ID = v_P_WORK_FLOW_ID;

      v_P_PRIMARY_KEY := v_P_WORK_FLOW_ID;

   END;
   ELSE
   BEGIN
      -- INSERT a new row in the table.
      INSERT INTO ZOP_WORK_FLOWS
        ( ORDER_ID, WORK_FLOW_NAME, FUNCTION_SEQ_ID )
        VALUES ( v_P_ORDER_ID, v_P_WORK_FLOW_NAME, v_P_FUNCTION_SEQ_ID );

      -- Get the IDENTITY value for the row just inserted.
      v_P_PRIMARY_KEY := sqlserver_utilities.identity;

   END;
   END IF;

   -- Get the Error Code for the statement just executed.
   v_P_ErrorCode := v_sys_error;

END;
/
CREATE OR REPLACE TRIGGER tgr_RLS_SE_Delete
   BEFORE DELETE
   ON ZFC_SECURITY_RLS_SE
   FOR EACH ROW
BEGIN
   DELETE ZFC_SECURITY_GRPS_RLS

      WHERE ZFC_SECURITY_GRPS_RLS.RLS_SE_SEQ_ID = ( SELECT :OLD.RLS_SE_SEQ_ID
                                                  FROM DUAL  );

END;
/

CREATE OR REPLACE TRIGGER tgr_GRPS_SE_Delete
   BEFORE DELETE
   ON ZFC_SECURITY_GRPS_SE
   FOR EACH ROW
BEGIN
   DELETE ZFC_SECURITY_GRPS_RLS

      WHERE ZFC_SECURITY_GRPS_RLS.GRPS_SE_SEQ_ID = ( SELECT :OLD.GRPS_SE_SEQ_ID
                                                   FROM DUAL  );

END;
/

connect Foundation/Foundation;

grant public_Foundation to Foundation;
grant db_owner_Foundation to Foundation;
ALTER TABLE ZCT_LINKS
ADD CONSTRAINT FK_ZCT_LINKS_ZCT_CONTENT FOREIGN KEY
(
  CONTENT_SEQ_ID
)
REFERENCES ZCT_CONTENT
(
  CONTENT_SEQ_ID
)
ENABLE
;

ALTER TABLE ZCT_NOTIFICATIONS
ADD CONSTRAINT FK_ZCT_NOTIFICATIONS_ZFC_FUNCT FOREIGN KEY
(
  FUNCTION_SEQ_ID
)
REFERENCES ZFC_FUNCTIONS
(
  FUNCTION_SEQ_ID
)
ENABLE
;

ALTER TABLE ZCT_NOTIFICATIONS
ADD CONSTRAINT FK_ZCT_NOTIFICATIONS_ZCT_CONTE FOREIGN KEY
(
  ID
)
REFERENCES ZCT_CONTENT
(
  CONTENT_SEQ_ID
)
ENABLE
;

ALTER TABLE ZCT_NOTIFICATIONS
ADD CONSTRAINT FK_ZCT_NOTIFICATIONS_ZFC_SESIN FOREIGN KEY
(
  SE_SEQ_ID
)
REFERENCES ZFC_SECURITY_ENTITIES
(
  SE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZOP_STATES
ADD CONSTRAINT FK_ZOP_STATES_ZFC_SYSTEM_STATU FOREIGN KEY
(
  STATUS_SEQ_ID
)
REFERENCES ZFC_SYSTEM_STATUS
(
  STATUS_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_GRPS_SE
ADD CONSTRAINT FK_ZFC_SECURITY_GRPS_SE_ZFC_SE FOREIGN KEY
(
  GROUP_SEQ_ID
)
REFERENCES ZFC_SECURITY_GRPS
(
  GROUP_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_GRPS_SE
ADD CONSTRAINT FK_ZFC_SECURITY_GRPS_SE_ZFC_SE FOREIGN KEY
(
  SE_SEQ_ID
)
REFERENCES ZFC_SECURITY_ENTITIES
(
  SE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_FUNCT_RLS
ADD CONSTRAINT FK_ZFC_FUNCT_RLS_SECURITY_ZF_1 FOREIGN KEY
(
  PERMISSIONS_ID
)
REFERENCES ZFC_PERMISSIONS
(
  PERMISSIONS_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_FUNCT_RLS
ADD CONSTRAINT FK_ZFC_FUNCT_RLS_SECURITY_ZFC_ FOREIGN KEY
(
  FUNCTION_SEQ_ID
)
REFERENCES ZFC_FUNCTIONS
(
  FUNCTION_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_FUNCT_RLS
ADD CONSTRAINT FK_ZFC_FUNCT_PER_RLS_ZFC_RLS_B FOREIGN KEY
(
  RLS_SE_SEQ_ID
)
REFERENCES ZFC_SECURITY_RLS_SE
(
  RLS_SE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZCT_HTML_PAGES
ADD CONSTRAINT FK_ZCT_HTML_PAGES_ZFC_FUNCTION FOREIGN KEY
(
  FUNCTION_SEQ_ID
)
REFERENCES ZFC_FUNCTIONS
(
  FUNCTION_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_ACCTS_GRPS
ADD CONSTRAINT FK_ZFC_ACCTS_GRPS_ZFC_GRPS_SE FOREIGN KEY
(
  GRPS_SE_SEQ_ID
)
REFERENCES ZFC_SECURITY_GRPS_SE
(
  GRPS_SE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_ACCTS_GRPS
ADD CONSTRAINT FK_ZFC_ACCTS_GRPS_ZFC_ACCTS FOREIGN KEY
(
  ACCT_SEQ_ID
)
REFERENCES ZFC_ACCTS
(
  ACCT_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_GRPS_RLS
ADD CONSTRAINT FK_ZFC_SECURITY_GRPS_RLS_ZFC_1 FOREIGN KEY
(
  RLS_SE_SEQ_ID
)
REFERENCES ZFC_SECURITY_RLS_SE
(
  RLS_SE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_GRPS_RLS
ADD CONSTRAINT FK_ZFC_SECURITY_GRPS_RLS_ZFC_S FOREIGN KEY
(
  GRPS_SE_SEQ_ID
)
REFERENCES ZFC_SECURITY_GRPS_SE
(
  GRPS_SE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZCT_RATINGS
ADD CONSTRAINT FK_ZCT_RATINGS_ZCT_CONTENT FOREIGN KEY
(
  CONTENT_SEQ_ID
)
REFERENCES ZCT_CONTENT
(
  CONTENT_SEQ_ID
)
ENABLE
;

ALTER TABLE ZCT_HAS_READ
ADD CONSTRAINT FK_ZCT_HAS_READ_ZCT_CONTENT FOREIGN KEY
(
  CONTENT_SEQ_ID
)
REFERENCES ZCT_CONTENT
(
  CONTENT_SEQ_ID
)
ENABLE
;

ALTER TABLE ZCT_HAS_READ
ADD CONSTRAINT FK_ZCT_HAS_READ_ZFC_ACCTS FOREIGN KEY
(
  hr_ACCT_SEQ_ID
)
REFERENCES ZFC_ACCTS
(
  ACCT_SEQ_ID
)
ENABLE
;

ALTER TABLE ZCT_DISCUSS
ADD CONSTRAINT FK_ZCT_DISCUSS_ZCT_CONTENT FOREIGN KEY
(
  CONTENT_SEQ_ID
)
REFERENCES ZCT_CONTENT
(
  CONTENT_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_NVP_RLS
ADD CONSTRAINT FK_ZFC_NVP_RLS_SEC_ZFC_RLS_SE FOREIGN KEY
(
  RLS_SE_SEQ_ID
)
REFERENCES ZFC_SECURITY_RLS_SE
(
  RLS_SE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_NVP_RLS
ADD CONSTRAINT FK_ZFC_NVP_RLS_SEC_ZFC_PERMISS FOREIGN KEY
(
  PERMISSIONS_SEQ_ID
)
REFERENCES ZFC_PERMISSIONS
(
  PERMISSIONS_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_NVP_RLS
ADD CONSTRAINT FK_ZFC_NVP_RLS_SEC_ZFC_NVP FOREIGN KEY
(
  NVP_SEQ_ID
)
REFERENCES ZFC_NVP
(
  NVP_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_NVP_DETAILS
ADD CONSTRAINT FK_ZFC_NVP_DETAILS_ZFC_SYSTEM_ FOREIGN KEY
(
  STATUS_SEQ_ID
)
REFERENCES ZFC_SYSTEM_STATUS
(
  STATUS_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_NVP_DETAILS
ADD CONSTRAINT FK_ZFC_NVP_DETAILS_ZFC_NVP FOREIGN KEY
(
  NVP_SEQ_ID
)
REFERENCES ZFC_NVP
(
  NVP_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_NVP_GRPS
ADD CONSTRAINT FK_ZFC_NVP_GRPS_SEC_ZFC_PERMIS FOREIGN KEY
(
  PERMISSIONS_ID
)
REFERENCES ZFC_PERMISSIONS
(
  PERMISSIONS_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_NVP_GRPS
ADD CONSTRAINT FK_ZFC_NVP_GRPS_SEC_ZFC_NVP FOREIGN KEY
(
  NVP_SEQ_ID
)
REFERENCES ZFC_NVP
(
  NVP_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_NVP_GRPS
ADD CONSTRAINT FK_ZFC_NVP_GRPS_SEC_ZFC_GRPS_B FOREIGN KEY
(
  GRPS_SE_SEQ_ID
)
REFERENCES ZFC_SECURITY_GRPS_SE
(
  GRPS_SE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_FUNCTIONS
ADD CONSTRAINT FK_ZFC_FUNCTIONS_ZFC_NAVIGATIO FOREIGN KEY
(
  NAV_TYPE_ID
)
REFERENCES ZFC_NAVIGATION_TYPES
(
  NAV_TYPE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_FUNCTIONS
ADD CONSTRAINT FK_ZFC_FUNCTIONS_ZFC_FUNCTIONS FOREIGN KEY
(
  PARENT_FUNCTION_SEQ_ID
)
REFERENCES ZFC_FUNCTIONS
(
  FUNCTION_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_FUNCTIONS
ADD CONSTRAINT FK_ZFC_FUNCTIONS_ZFC_FUNCTION_ FOREIGN KEY
(
  FUNCTION_TYPE_SEQ_ID
)
REFERENCES ZFC_FUNCTION_TYPES
(
  FUNCTION_TYPE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_ACCTS
ADD CONSTRAINT FK_ZFC_ACCTS_ZFC_SYSTEM_STATUS FOREIGN KEY
(
  STATUS_SEQ_ID
)
REFERENCES ZFC_SYSTEM_STATUS
(
  STATUS_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFO_MESSAGES
ADD CONSTRAINT FK_ZFO_MESSAGES_ZFC_SECURITY_E FOREIGN KEY
(
  SE_SEQ_ID
)
REFERENCES ZFC_SECURITY_ENTITIES
(
  SE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_RLS_SE
ADD CONSTRAINT FK_ZFC_SECURITY_RLS_SE_ZFC_SEC FOREIGN KEY
(
  ROLE_SEQ_ID
)
REFERENCES ZFC_SECURITY_RLS
(
  ROLE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_RLS_SE
ADD CONSTRAINT FK_ZFC_SECURITY_RLS_SE_ZFC_SES FOREIGN KEY
(
  SE_SEQ_ID
)
REFERENCES ZFC_SECURITY_ENTITIES
(
  SE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZOP_CALENDAR
ADD CONSTRAINT FK_ZOP_CALENDAR_ZFC_SECURITY_E FOREIGN KEY
(
  SE_SEQ_ID
)
REFERENCES ZFC_SECURITY_ENTITIES
(
  SE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZCT_CONTENT
ADD CONSTRAINT FK_ZFC_ZFC_ACCTS FOREIGN KEY
(
  ACCT_SEQ_ID
)
REFERENCES ZFC_ACCTS
(
  ACCT_SEQ_ID
)
ENABLE
;

ALTER TABLE ZCT_CONTENT
ADD CONSTRAINT FK_ZFC_ZFC_FUNCTIONS FOREIGN KEY
(
  FUNCTION_SEQ_ID
)
REFERENCES ZFC_FUNCTIONS
(
  FUNCTION_SEQ_ID
)
ENABLE
;

ALTER TABLE ZCT_CONTENT
ADD CONSTRAINT FK_ZFC_ZFC_SECURITY_ENTITIES FOREIGN KEY
(
  SE_SEQ_ID
)
REFERENCES ZFC_SECURITY_ENTITIES
(
  SE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_GRPS_GRPS
ADD CONSTRAINT FK_ZFC_GRPS_GRPS_ZFC_GRPS FOREIGN KEY
(
  GROUP_SEQ_ID
)
REFERENCES ZFC_SECURITY_GRPS
(
  GROUP_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_FUNCT_GRPS
ADD CONSTRAINT FK_ZFC_FUNCT_PER_GRPS_ZFC_GRPS FOREIGN KEY
(
  GRPS_SE_SEQ_ID
)
REFERENCES ZFC_SECURITY_GRPS_SE
(
  GRPS_SE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_FUNCT_GRPS
ADD CONSTRAINT FK_ZFC_FUNCT_GRPS_SECURITY_Z_1 FOREIGN KEY
(
  PERMISSIONS_ID
)
REFERENCES ZFC_PERMISSIONS
(
  PERMISSIONS_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_FUNCT_GRPS
ADD CONSTRAINT FK_ZFC_FUNCT_GRPS_SECURITY_ZFC FOREIGN KEY
(
  FUNCTION_SEQ_ID
)
REFERENCES ZFC_FUNCTIONS
(
  FUNCTION_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFO_DIRECTORIES
ADD CONSTRAINT FK_ZFO_DIRECTORIES_ZFC_FUNCTIO FOREIGN KEY
(
  FUNCTION_SEQ_ID
)
REFERENCES ZFC_FUNCTIONS
(
  FUNCTION_SEQ_ID
)
ENABLE
;

ALTER TABLE ZOP_WORK_FLOWS
ADD CONSTRAINT FK_ZOP_WORK_FLOWS_ZFC_FUNCTION FOREIGN KEY
(
  FUNCTION_SEQ_ID
)
REFERENCES ZFC_FUNCTIONS
(
  FUNCTION_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_ACCTS_RLS
ADD CONSTRAINT FK_ZFC_ACCTS_RLS_ZFC_RLS_SE FOREIGN KEY
(
  RLS_SE_SEQ_ID
)
REFERENCES ZFC_SECURITY_RLS_SE
(
  RLS_SE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_ACCTS_RLS
ADD CONSTRAINT FK_ZFC_ACCTS_RLS_ZFC_ACCTS FOREIGN KEY
(
  ACCT_SEQ_ID
)
REFERENCES ZFC_ACCTS
(
  ACCT_SEQ_ID
)
ENABLE
;

ALTER TABLE ZFO_ACCT_CHOICES
ADD CONSTRAINT FK_ZFO_ACCT_CHOICES_ZFC_ACCTS FOREIGN KEY
(
  ACCT
)
REFERENCES ZFC_ACCTS
(
  ACCT
)
ENABLE
;

ALTER TABLE ZFC_SECURITY_ENTITIES
ADD CONSTRAINT FK_ZFC_SECURITY_ENTITIES_ZFC_SESI FOREIGN KEY
(
  PARENT_SE_SEQ_ID
)
REFERENCES ZFC_SECURITY_ENTITIES
(
  SE_SEQ_ID
)
ENABLE
;

ALTER TABLE ZOP_ZIPCODES
ADD CONSTRAINT FK_ZOP_ZIPCODES_ZOP_STATES FOREIGN KEY
(
  STATE
)
REFERENCES ZOP_STATES
(
  STATE
)
ENABLE
;

CREATE OR REPLACE TRIGGER ZFC_SECURITY_RLS_ROLE_SEQ_ID_TRG BEFORE INSERT OR UPDATE ON ZFC_SECURITY_RLS
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.ROLE_SEQ_ID IS NULL THEN
    SELECT  ZFC_SECURITY_RLS_ROLE_SEQ_ID_SEQ.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(ROLE_SEQ_ID) INTO v_newVal FROM ZFC_SECURITY_RLS;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZFC_SECURITY_RLS_ROLE_SEQ_ID_SEQ.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.ROLE_SEQ_ID := v_newVal;
  END IF;
END;

/

CREATE OR REPLACE TRIGGER ZFC_NAVIGATION_TYPES_NAV_TYP_1 BEFORE INSERT OR UPDATE ON ZFC_NAVIGATION_TYPES
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.NAV_TYPE_SEQ_ID IS NULL THEN
    SELECT  ZFC_NAVIGATION_TYPES_NAV_TYPE_.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(NAV_TYPE_SEQ_ID) INTO v_newVal FROM ZFC_NAVIGATION_TYPES;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZFC_NAVIGATION_TYPES_NAV_TYPE_.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.NAV_TYPE_SEQ_ID := v_newVal;
  END IF;
END;

/

CREATE OR REPLACE TRIGGER ZCT_CONTENT_CONTENT_SEQ_ID_TRG BEFORE INSERT OR UPDATE ON ZCT_CONTENT
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.CONTENT_SEQ_ID IS NULL THEN
    SELECT  ZCT_CONTENT_CONTENT_SEQ_ID_SEQ.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(CONTENT_SEQ_ID) INTO v_newVal FROM ZCT_CONTENT;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZCT_CONTENT_CONTENT_SEQ_ID_SEQ.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.CONTENT_SEQ_ID := v_newVal;
  END IF;
END;

/

CREATE OR REPLACE TRIGGER ZFC_SECURITY_GRPS_SE_GRPS_SE_1 BEFORE INSERT OR UPDATE ON ZFC_SECURITY_GRPS_SE
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.GRPS_SE_SEQ_ID IS NULL THEN
    SELECT  ZFC_SECURITY_GRPS_SE_GRPS_SE_I.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(GRPS_SE_SEQ_ID) INTO v_newVal FROM ZFC_SECURITY_GRPS_SE;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZFC_SECURITY_GRPS_SE_GRPS_SE_I.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.GRPS_SE_SEQ_ID := v_newVal;
  END IF;
END;

/

CREATE OR REPLACE TRIGGER ZCT_RATINGS_Rating_id_TRG BEFORE INSERT OR UPDATE ON ZCT_RATINGS
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.Rating_id IS NULL THEN
    SELECT  ZCT_RATINGS_Rating_id_SEQ.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(Rating_id) INTO v_newVal FROM ZCT_RATINGS;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZCT_RATINGS_Rating_id_SEQ.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.Rating_id := v_newVal;
  END IF;
END;

/

CREATE OR REPLACE TRIGGER ZFC_ACCTS_ACCT_SEQ_ID_TRG BEFORE INSERT OR UPDATE ON ZFC_ACCTS
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.ACCT_SEQ_ID IS NULL THEN
    SELECT  ZFC_ACCTS_ACCT_SEQ_ID_SEQ.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(ACCT_SEQ_ID) INTO v_newVal FROM ZFC_ACCTS;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZFC_ACCTS_ACCT_SEQ_ID_SEQ.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.ACCT_SEQ_ID := v_newVal;
  END IF;
END;

/

CREATE OR REPLACE TRIGGER ZFC_FUNCTION_TYPES_FUNCTION__1 BEFORE INSERT OR UPDATE ON ZFC_FUNCTION_TYPES
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.FUNCTION_TYPE_SEQ_ID IS NULL THEN
    SELECT  ZFC_FUNCTION_TYPES_FUNCTION_TY.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(FUNCTION_TYPE_SEQ_ID) INTO v_newVal FROM ZFC_FUNCTION_TYPES;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZFC_FUNCTION_TYPES_FUNCTION_TY.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.FUNCTION_TYPE_SEQ_ID := v_newVal;
  END IF;
END;

/

CREATE OR REPLACE TRIGGER ZFC_SECURITY_RLS_SE_RLS_SE_I_1 BEFORE INSERT OR UPDATE ON ZFC_SECURITY_RLS_SE
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.RLS_SE_SEQ_ID IS NULL THEN
    SELECT  ZFC_SECURITY_RLS_SE_RLS_SE_SEQ_ID_.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(RLS_SE_SEQ_ID) INTO v_newVal FROM ZFC_SECURITY_RLS_SE;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZFC_SECURITY_RLS_SE_RLS_SE_SEQ_ID_.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.RLS_SE_SEQ_ID := v_newVal;
  END IF;
END;

/

CREATE OR REPLACE TRIGGER ZFC_SECURITY_ENTITIES_SE_SEQ_ID_TRG BEFORE INSERT OR UPDATE ON ZFC_SECURITY_ENTITIES
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.SE_SEQ_ID IS NULL THEN
    SELECT  ZFC_SECURITY_ENTITIES_SE_SEQ_ID_SEQ.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(SE_SEQ_ID) INTO v_newVal FROM ZFC_SECURITY_ENTITIES;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZFC_SECURITY_ENTITIES_SE_SEQ_ID_SEQ.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.SE_SEQ_ID := v_newVal;
  END IF;
END;

/

CREATE OR REPLACE TRIGGER ZFO_MESSAGES_MESSAGE_SEQ_ID_TRG BEFORE INSERT OR UPDATE ON ZFO_MESSAGES
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.MESSAGE_SEQ_ID IS NULL THEN
    SELECT  ZFO_MESSAGES_MESSAGE_SEQ_ID_SEQ.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(MESSAGE_SEQ_ID) INTO v_newVal FROM ZFO_MESSAGES;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZFO_MESSAGES_MESSAGE_SEQ_ID_SEQ.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.MESSAGE_SEQ_ID := v_newVal;
  END IF;
END;

/

CREATE OR REPLACE TRIGGER ZFC_INFORMATION_INFORMATION__1 BEFORE INSERT OR UPDATE ON ZFC_INFORMATION
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.Information_SEQ_ID IS NULL THEN
    SELECT  ZFC_INFORMATION_Information_SEQ_ID.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(Information_SEQ_ID) INTO v_newVal FROM ZFC_INFORMATION;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZFC_INFORMATION_Information_SEQ_ID.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.Information_SEQ_ID := v_newVal;
  END IF;
END;

/

CREATE OR REPLACE TRIGGER ZFC_NVP_DETAILS_NVP_SEQ_DET__1 BEFORE INSERT OR UPDATE ON ZFC_NVP_DETAILS
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.NVP_SEQ_DET_ID IS NULL THEN
    SELECT  ZFC_NVP_DETAILS_NVP_SEQ_DET_ID.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(NVP_SEQ_DET_ID) INTO v_newVal FROM ZFC_NVP_DETAILS;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZFC_NVP_DETAILS_NVP_SEQ_DET_ID.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.NVP_SEQ_DET_ID := v_newVal;
  END IF;
END;

/

CREATE OR REPLACE TRIGGER ZFC_NVP_NVP_SEQ_ID_TRG BEFORE INSERT OR UPDATE ON ZFC_NVP
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.NVP_SEQ_ID IS NULL THEN
    SELECT  ZFC_NVP_NVP_SEQ_ID_SEQ.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(NVP_SEQ_ID) INTO v_newVal FROM ZFC_NVP;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZFC_NVP_NVP_SEQ_ID_SEQ.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.NVP_SEQ_ID := v_newVal;
  END IF;
END;

/

CREATE OR REPLACE TRIGGER ZFC_SYSTEM_STATUS_STATUS_SEQ_ID_TR BEFORE INSERT OR UPDATE ON ZFC_SYSTEM_STATUS
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.STATUS_SEQ_ID IS NULL THEN
    SELECT  ZFC_SYSTEM_STATUS_STATUS_SEQ_ID_SE.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(STATUS_SEQ_ID) INTO v_newVal FROM ZFC_SYSTEM_STATUS;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZFC_SYSTEM_STATUS_STATUS_SEQ_ID_SE.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.STATUS_SEQ_ID := v_newVal;
  END IF;
END;

/

CREATE OR REPLACE TRIGGER ZFC_SECURITY_GRPS_GROUP_SEQ_ID_TRG BEFORE INSERT OR UPDATE ON ZFC_SECURITY_GRPS
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.GROUP_SEQ_ID IS NULL THEN
    SELECT  ZFC_SECURITY_GRPS_GROUP_SEQ_ID_SEQ.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(GROUP_SEQ_ID) INTO v_newVal FROM ZFC_SECURITY_GRPS;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZFC_SECURITY_GRPS_GROUP_SEQ_ID_SEQ.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.GROUP_SEQ_ID := v_newVal;
  END IF;
END;

/

CREATE OR REPLACE TRIGGER ZOP_WORK_FLOWS_WORK_FLOW_ID_TR BEFORE INSERT OR UPDATE ON ZOP_WORK_FLOWS
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.WORK_FLOW_ID IS NULL THEN
    SELECT  ZOP_WORK_FLOWS_WORK_FLOW_ID_SE.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(WORK_FLOW_ID) INTO v_newVal FROM ZOP_WORK_FLOWS;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZOP_WORK_FLOWS_WORK_FLOW_ID_SE.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.WORK_FLOW_ID := v_newVal;
  END IF;
END;

/

CREATE OR REPLACE TRIGGER ZFC_PERMISSIONS_PERMISSIONS__1 BEFORE INSERT OR UPDATE ON ZFC_PERMISSIONS
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.PERMISSIONS_ID IS NULL THEN
    SELECT  ZFC_PERMISSIONS_PERMISSIONS_ID.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(PERMISSIONS_ID) INTO v_newVal FROM ZFC_PERMISSIONS;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZFC_PERMISSIONS_PERMISSIONS_ID.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.PERMISSIONS_ID := v_newVal;
  END IF;
END;

/

CREATE OR REPLACE TRIGGER ZFC_FUNCTIONS_FUNCTION_SEQ_I_1 BEFORE INSERT OR UPDATE ON ZFC_FUNCTIONS
FOR EACH ROW
DECLARE 
v_newVal NUMBER(12) := 0;
v_incval NUMBER(12) := 0;
BEGIN
  IF INSERTING AND :new.FUNCTION_SEQ_ID IS NULL THEN
    SELECT  ZFC_FUNCTIONS_FUNCTION_SEQ_ID_.NEXTVAL INTO v_newVal FROM DUAL;
    -- If this is the first time this table have been inserted into (sequence == 1)
    IF v_newVal = 1 THEN 
      --get the max indentity value from the table
      SELECT max(FUNCTION_SEQ_ID) INTO v_newVal FROM ZFC_FUNCTIONS;
      v_newVal := v_newVal + 1;
      --set the sequence to that value
      LOOP
           EXIT WHEN v_incval>=v_newVal;
           SELECT ZFC_FUNCTIONS_FUNCTION_SEQ_ID_.nextval INTO v_incval FROM dual;
      END LOOP;
    END IF;
    -- save this to emulate @@identity
   sqlserver_utilities.identity := v_newVal; 
   -- assign the value from the sequence to emulate the identity column
   :new.FUNCTION_SEQ_ID := v_newVal;
  END IF;
END;

/

